using Nebulas.Schema;
using Nebulas.Schema.Response;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace Nebulas
{
    public class HttpRequest
    {
        private string _host { get; set; }
        private int _timeout { get; set; }
        private string _apiVersion { get; set; }
        public Action<string> OnDownloadProgressEvent{ get; set; }

        public HttpRequest(string host, int timeout = 0, string apiVersion = "v1")
        {
            _host = host;
            _timeout = timeout;
            _apiVersion = apiVersion;
        }

        public void Sethost(string host)
        {
            _host = host;
        }

        public void SetAPIVersion(string apiVersion)
        {
            _apiVersion = apiVersion;
        }

        private string createUrl(string api)
        {
            return _host + "/" + _apiVersion + "/" + api;
        }

        public async Task<TRq<T>> RequestAsync<T>(string method, string api, string payload, JsonSerializerSettings setting = null)
        {
            setting = setting == null ? new JsonSerializerSettings() : setting;
            
            HttpClient client = new HttpClient();
            HttpItem item = new HttpItem();
            item.Method = method;
            item.URL = createUrl(api);
            item.Postdata = payload;
            var result = await Task.Run(() => client.GetHtml(item));
            var json = JsonConvert.DeserializeObject<TRq<T>>(result.Html, setting);
            if (json.Result == null)
            {
                var error = JsonConvert.DeserializeObject<dynamic>(result.Html);
                if (error.error.Value != null)
                {
                    throw new Exception(error.error.Value);
                }
            }
            return json;
        }

        public void BeginRequestAsync(string method, string api, string payload)
        {
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp(createUrl(api));
            httpRequest.Method = method;
            httpRequest.KeepAlive = true;
            byte[] data = Encoding.Default.GetBytes(payload.ToString());
            using (Stream stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            httpRequest.BeginGetResponse(ResponseCallBack, httpRequest);
        }

        private void ResponseCallBack(IAsyncResult asyncResult)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)asyncResult.AsyncState;
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asyncResult);
            Stream stream = httpWebResponse.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string result = reader.ReadLine();
            OnDownloadProgressEvent(result);
        }
    }
}
