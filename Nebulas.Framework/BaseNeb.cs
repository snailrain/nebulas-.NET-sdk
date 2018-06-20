using Nebulas.Schema;
using Nebulas.Schema.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nebulas
{
    public class BaseNeb
    {
        protected HttpRequest _request { get; set; }
        protected string _path { get; set; }

        /// <summary>
        /// Camel与C#规则转换
        /// </summary>
        public static JsonSerializerSettings CamelCaseSetting = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        /// <summary>
        /// Camel + 下划线 与 C# 规则转换
        /// </summary>
        public static JsonSerializerSettings UnderLineSetting = new JsonSerializerSettings
        {
            ContractResolver = new UnderlineSplitContractResolver()
        };

        public BaseNeb(HttpRequest request)
        {
            setRequest(request);
        }

        public BaseNeb(Neb neb)
        {
            setRequest(neb.GetRequest());
        }

        protected void setRequest(HttpRequest request)
        {
            _request = request;
        }



        protected async Task<TRq<T>> sendRequestAsync<T> (string method, string api, string paramsOptions, JsonSerializerSettings setting = null)
        {
            string action = _path + api;
            return await _request.RequestAsync<T>(method, action, paramsOptions, setting);
        }

        protected async Task<TRq<T>> sendRequestAsync<T>(string method, string api, dynamic paramsOptions, JsonSerializerSettings setting = null)
        {
            string paras = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(paramsOptions);
            return await sendRequestAsync<T>(method, api, paras, setting);
        }

    }
}
