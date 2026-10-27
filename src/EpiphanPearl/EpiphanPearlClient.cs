using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using PepperDash.Core;
using Serilog.Events;
using PepperDash.Essentials.Plugins.Interfaces;

namespace PepperDash.Essentials.Plugins
{
    /// <summary>
    /// The Pearl's REST API, over plain HTTP or TLS.
    /// </summary>
    /// <remarks>
    /// <para>Built on <see cref="System.Net.Http.HttpClient"/>, replacing a pair of Crestron-client
    /// implementations — one per scheme, which had drifted apart until the secure one could neither
    /// change host nor send a body. This one is safe to share across threads, so the poll timer, the
    /// room's reads after a command and the commands themselves need not take turns; and it always
    /// has a timeout, where the Crestron clients waited a minute unless told otherwise.</para>
    /// <para>Synchronous, because the controller is: <see cref="System.Net.Http.HttpClient.Send(HttpRequestMessage)"/>
    /// blocks on the calling thread rather than waiting on a task.</para>
    /// </remarks>
    public class EpiphanPearlClient : IEpiphanPearlClient
    {
        /// <summary>The Pearl is on the LAN, so anything slower than this is a Pearl that is not
        /// answering, and waiting longer only stacks up the polls behind it.</summary>
        private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(5);

        private readonly IKeyed _parent;
        private readonly HttpClient _client;
        private readonly string _scheme;

        private volatile string _basePath;

        /// <summary>The last status code that was not a success, so a Pearl that starts refusing
        /// us — a changed password, say — is reported once rather than on every poll.</summary>
        private int _lastFailureCode;

        public EpiphanPearlClient(IKeyed parent, string host, bool secure, string username, string password)
        {
            _parent = parent;
            _scheme = secure ? "https" : "http";

            var handler = new HttpClientHandler();

            if (secure)
            {
                // The Pearl serves a self-signed certificate. The Crestron client this replaces had
                // host verification off for the same reason.
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            }

            _client = new HttpClient(handler) { Timeout = RequestTimeout };

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password))));

            SetHost(host);
        }

        public void SetHost(string host)
        {
            _basePath = string.Format("{0}://{1}/api", _scheme, host);
        }

        public T Get<T>(string path) where T : class
        {
            return Deserialize<T>(path, Send(HttpMethod.Get, path, null));
        }

        public TResponse Post<TBody, TResponse>(string path, TBody body) where TBody : class where TResponse : class
        {
            var json = body != null ? JsonConvert.SerializeObject(body) : string.Empty;

            return Deserialize<TResponse>(path, Send(HttpMethod.Post, path, json));
        }

        public TResponse Post<TResponse>(string path) where TResponse : class
        {
            return Deserialize<TResponse>(path, Send(HttpMethod.Post, path, string.Empty));
        }

        public string Delete(string path)
        {
            return Send(HttpMethod.Delete, path, null);
        }

        /// <summary>
        /// Send a request and return the body, or null if nothing came back.
        /// </summary>
        /// <remarks>
        /// The body is returned whatever the status code: the Pearl explains a refused command in
        /// a JSON body with an error status, and the controller reads that.
        /// </remarks>
        private string Send(HttpMethod method, string path, string json)
        {
            var url = _basePath + path;

            try
            {
                using (var request = new HttpRequestMessage(method, url))
                {
                    if (json != null)
                    {
                        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    }

                    Debug.LogMessage(LogEventLevel.Verbose, _parent, "{0} {1} {2}", method, url, json ?? string.Empty);

                    using (var response = _client.Send(request))
                    using (var reader = new StreamReader(response.Content.ReadAsStream()))
                    {
                        var content = reader.ReadToEnd();
                        var code = (int)response.StatusCode;

                        if (response.IsSuccessStatusCode)
                        {
                            _lastFailureCode = 0;
                        }
                        else if (code != _lastFailureCode)
                        {
                            _lastFailureCode = code;
                            Debug.LogMessage(LogEventLevel.Warning, _parent, "Pearl answered {0} to {1} {2}", code, method, path);
                        }

                        Debug.LogMessage(LogEventLevel.Verbose, _parent, "Response to {0}: {1} {2}", url, code, content);

                        return content;
                    }
                }
            }
            catch (Exception ex)
            {
                // Debug, not Error: an unreachable Pearl fails every request of every poll, and the
                // communication monitor already says so once when it goes offline.
                Debug.LogMessage(LogEventLevel.Debug, _parent, "{0} {1} failed: {2}", method, url, ex.Message);
                return null;
            }
        }

        private T Deserialize<T>(string path, string content) where T : class
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception ex)
            {
                Debug.LogMessage(LogEventLevel.Debug, _parent, "Unreadable response to {0}: {1}", path, ex.Message);
                return null;
            }
        }
    }
}
