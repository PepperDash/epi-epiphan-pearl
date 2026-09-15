using System;
using Crestron.SimplSharp.Net.Http;
using Newtonsoft.Json;
using PepperDash.Core;
using Serilog.Events;
using PepperDash.Essentials.Plugins.Interfaces;
using PepperDash.Essentials.Plugins.Utilities;

namespace PepperDash.Essentials.Plugins
{
    public class EpiphanPearlClient : IEpiphanPearlClient
    {
        private readonly HttpClient _client;

        private readonly HttpHeader _authHeader;

        private string _basePath;

        public EpiphanPearlClient(string host, string username, string password)
        {
            _client = new HttpClient();

            _basePath = string.Format("http://{0}/api", host);

            _authHeader = HttpHelpers.GetAuthorizationHeader(username, password);
        }

        public T Get<T>(string path) where T:class
        {
            var request = CreateRequest(path, RequestType.Get);

            var response = SendRequest(request);

            if (response == null || response.Length <= 0)
            {
                Debug.LogMessage(LogEventLevel.Debug, "[T Get<T>] Response {0} to {1} is null or empty", response, request.Url);
                return null;
            }

            try
            {
                Debug.LogMessage(LogEventLevel.Debug, "[T Get<T>] Response to {0}: {1}", request.Url, response);
                return JsonConvert.DeserializeObject<T>(response);
            }
            catch (Exception ex)
            {
                Debug.LogMessage(LogEventLevel.Error, "[T Get<T>] Exception sending to {0}: {1}", request.Url, ex.Message);
                Debug.LogMessage(LogEventLevel.Debug, "Stack Trace: {0}", ex.StackTrace);

                if (ex.InnerException == null) return null;

                Debug.LogMessage(LogEventLevel.Error, "[T Get<T>] Exception sending to {0}: {1}", request.Url, ex.InnerException.Message);
                Debug.LogMessage(LogEventLevel.Debug, "Stack Trace: {0}", ex.InnerException.StackTrace);

                return null;
            }
        }

        public TResponse Post<TBody, TResponse> (string path, TBody body) where TBody: class where TResponse: class
        {
            var request = CreateRequest(path, RequestType.Post);

            request.Header.ContentType = "application/json";
            request.ContentString = body != null ? JsonConvert.SerializeObject(body) : string.Empty;

            Debug.LogMessage(LogEventLevel.Debug, "Post request: {0} - {1}", request.Url, request.ContentString);

            var response = SendRequest(request);

            if (response == null)
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<TResponse>(response);
            }
            catch (Exception ex)
            {
                Debug.LogMessage(LogEventLevel.Error, "[TResponse Post<TBody, TResponse>] Exception sending to {0}: {1}", request.Url, ex.Message);
                Debug.LogMessage(LogEventLevel.Debug, "Stack Trace: {0}", ex.StackTrace);

                if (ex.InnerException == null) return null;

                Debug.LogMessage(LogEventLevel.Error, "[TResponse Post<TBody, TResponse>] Exception sending to {0}: {1}", request.Url, ex.InnerException.Message);
                Debug.LogMessage(LogEventLevel.Debug, "Stack Trace: {0}", ex.InnerException.StackTrace);

                return null;
            }
        }

        public TResponse Post<TResponse>(string path)
            where TResponse : class
        {
            var request = CreateRequest(path, RequestType.Post);

            request.Header.ContentType = "application/json";

            var response = SendRequest(request);

            if (response == null)
            {
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<TResponse>(response);
            }
            catch (Exception ex)
            {
                Debug.LogMessage(LogEventLevel.Error, "[TResponse Post<TResponse>] Exception sending to {0}: {1}", request.Url, ex.Message);
                Debug.LogMessage(LogEventLevel.Debug, "Stack Trace: {0}", ex.StackTrace);

                if (ex.InnerException == null) return null;

                Debug.LogMessage(LogEventLevel.Error, "[TResponse Post<TResponse>] Exception sending to {0}: {1}", request.Url, ex.InnerException.Message);
                Debug.LogMessage(LogEventLevel.Debug, "Stack Trace: {0}", ex.InnerException.StackTrace);

                return null;
            }
        }

        public string Delete(string path)
        {
            var request = CreateRequest(path, RequestType.Delete);

            return SendRequest(request);
        }

        public void setHost(string host)
        {
            _basePath = string.Format("http://{0}/api", host);
        }

        private string SendRequest(HttpClientRequest request)
        {
            if (request == null)
            {
                Debug.LogMessage(LogEventLevel.Debug, "[SendRequest] Request is null");
                return null;
            }

            if (_client == null)
            {
                Debug.LogMessage(LogEventLevel.Debug, "[SendRequest] HttpClient is null");
                return null;
            }

            try
            {
                Debug.LogMessage(LogEventLevel.Debug, "[SendRequest] Dispatching request to {0}", request.Url); // Log before dispatch
                var response = _client.Dispatch(request);

                if (response == null)
                {
                    Debug.LogMessage(LogEventLevel.Debug, "[SendRequest] Response is null after dispatching request to {0}", request.Url);
                    return null;
                }

                //Debug.LogMessage(LogEventLevel.Error, "Raw response bytes: {0}", BitConverter.ToString(response.ContentBytes));

                try
                {
                    // Attempt to parse the response content as a string
                    var contentString = response.ContentString;
                    return contentString;
                }
                catch (Exception ex)
                {
                    Debug.LogMessage(LogEventLevel.Debug, "[SendRequest] Error converting response to string for URL {0}: {1}", request.Url, ex.Message);
                    return null;
                }
                finally
                {
                    response.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.LogMessage(LogEventLevel.Error, "[SendRequest] Exception sending to {0}: {1}", request.Url, ex.Message);
                Debug.LogMessage(LogEventLevel.Debug, "Stack Trace: {0}", ex.StackTrace);

                if (ex.InnerException != null)
                {
                    Debug.LogMessage(LogEventLevel.Error, "[SendRequest] Inner Exception sending to {0}: {1}", request.Url, ex.InnerException.Message);
                    Debug.LogMessage(LogEventLevel.Debug, "Inner Stack Trace: {0}", ex.InnerException.StackTrace);
                }

                return null;
            }

        }

        private HttpClientRequest CreateRequest(string path, RequestType requestType)
        {
            var request = new HttpClientRequest
            {
                Url = new UrlParser(string.Format("{0}{1}", _basePath, path)),
                RequestType = requestType
            };

            request.Header.AddHeader(_authHeader);

            return request;
        }
    }

}
