using System;
using Crestron.SimplSharp.Net.Http;
using Crestron.SimplSharp.Net.Https;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PepperDash.Core;
using Serilog.Events;
using PepperDash.Essentials.Plugins.Interfaces;
using PepperDash.Essentials.Plugins.Utilities;

namespace PepperDash.Essentials.Plugins
{
    public class EpiphanPearlSecureClient : IEpiphanPearlClient
    {
        private readonly HttpsClient _client;

        private readonly HttpsHeader _authHeader;

        private string _basePath;

        public EpiphanPearlSecureClient(string host, string username, string password) 
        {
            _client = new HttpsClient();

            _client.HostVerification = false;

            _basePath = string.Format("https://{0}", host);

            _authHeader = HttpHelpers.GetSecureAuthorizationHeader(username, password);
        }

        public T Get<T>(string path) where T: class
        {
            var request = CreateRequest(path, Crestron.SimplSharp.Net.Https.RequestType.Get);

            var response = SendRequest(request);

            if (string.IsNullOrEmpty(response))
            {
                return null;
            }

            try
            {
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

        public TResponse Post<TBody, TResponse>(string path, TBody body) where TBody : class where TResponse:class
        {
            var request = CreateRequest(path, Crestron.SimplSharp.Net.Https.RequestType.Post);

            request.Header.ContentType = "application/json";
            request.ContentString = body != null ? JsonConvert.ToString(body) : string.Empty;

            var response = SendRequest(request);

            if (string.IsNullOrEmpty(response))
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
            var request = CreateRequest(path, Crestron.SimplSharp.Net.Https.RequestType.Post);

            request.Header.ContentType = "application/json";

            var response = SendRequest(request);

            if (string.IsNullOrEmpty(response))
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
            var request = CreateRequest(path, Crestron.SimplSharp.Net.Https.RequestType.Delete);

            return SendRequest(request);
        }

        public void setHost(string host)
        {
            _basePath = string.Format("http://{0}/api", host);
        }
        private string SendRequest(HttpsClientRequest request)
        {
            try
            {
                //Debug.LogMessage(LogEventLevel.Error, "Request to {0): {1}", request.Url, request.ContentString);
                var response = _client.Dispatch(request);

                //Debug.LogMessage(LogEventLevel.Error, "Response from request to {0}: {1} {2}", request.Url, response.Code,
                    //response.ContentString);

                return response.ContentString;
            }
            catch (Exception ex)
            {
                Debug.LogMessage(LogEventLevel.Error, "[SendRequest] Exception sending to {0}: {1}", request.Url, ex.Message);
                Debug.LogMessage(LogEventLevel.Debug, "Stack Trace: {0}", ex.StackTrace);

                if (ex.InnerException == null) return null;

                Debug.LogMessage(LogEventLevel.Error, "[SendRequest] Exception sending to {0}: {1}", request.Url, ex.InnerException.Message);
                Debug.LogMessage(LogEventLevel.Debug, "Stack Trace: {0}", ex.InnerException.StackTrace);

                return null;
            }
        }

        private HttpsClientRequest CreateRequest(string path, Crestron.SimplSharp.Net.Https.RequestType requestType)
        {
            var request = new HttpsClientRequest
            {
                Url = new UrlParser(string.Format("{0}/api{1}", _basePath, path)),
                RequestType = requestType
            };

            request.Header.AddHeader(_authHeader);

            return request;
        }
    }
}