using System;
using System.Collections.Generic;
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

        public HttpRequest(string host, int timeout = 0, string apiVersion = "v1")
        {
            _host = host;
            _timeout = timeout;
            _apiVersion = apiVersion;
        }

        public void SetHost(string host)
        {
            _host = host;
        }

        public void SetAPIVersion(string apiVersion) {
            _apiVersion = apiVersion;
        }

        private string createUrl(string api)
        {
            return _host + "/" + _apiVersion + api;
        }

        public async Task<string> RequestAsync(string method,string api,string payload)
        {
            HttpClient client = new HttpClient();
            HttpItem item = new HttpItem();
            item.Method = method;
            item.URL = createUrl(api);
            item.Postdata = payload;
            var result = await Task.Run(()=>client.GetHtml(item));
            return result.Html;
        }
    }
}
