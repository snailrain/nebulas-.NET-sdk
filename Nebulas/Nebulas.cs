using Nebulas.Schema;
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


}
