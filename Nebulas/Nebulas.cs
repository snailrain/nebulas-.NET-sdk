using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace Nebulas
{
    public class Neb
    {
        private HttpRequest _request { get; set; }
        public API API { get; set; }

        public Neb(HttpRequest request)
        {
            SetRequest(request);
        }

        /// <summary>
        /// 设置请求
        /// </summary>
        public void SetRequest(HttpRequest request)
        {
            _request = request;
            API = new API(_request);
        }
    }

    public class API
    {
        private HttpRequest _request { get; set; }
        private string _path { get; set; }

        public API(HttpRequest request)
        {
            setRequest(request);
        }

        private void setRequest(HttpRequest request)
        {
            _request = request;
            _path = "/user";
        }

        private async Task<string> sendRequestAsync(string method,string api,string paramsOptions)
        {
            string action = _path + api;
            return await _request.RequestAsync(method, action, paramsOptions);
        }

        public async Task<string> CallAsync(string from, string to, string value, string nonce, int gasPrice, int gasLimit, string function,string args)
        {
            CallRequest callRequest = new CallRequest()
            {
                From = from,
                To = to,
                Value = value,
                Nonce = nonce,
                GasPrice = Convert.ToString(gasPrice),
                GasLimit = Convert.ToString(gasLimit),
                Contract = new ContractRequest() {
                    Function = function,
                    Args = args
                }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(callRequest, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            return await sendRequestAsync("post", "/call", json);
        }


    }

    public class CallRequest
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Value { get; set; }
        public string Nonce { get; set; }
        public string GasPrice { get; set; }
        public string GasLimit { get; set; }
        public ContractRequest Contract { get; set; }

    }

    public class ContractRequest
    {
        public string Function { get; set; }
        public string Args { get; set; }
    }
}
