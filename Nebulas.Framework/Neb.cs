namespace Nebulas
{
    public class Neb
    {
        private HttpRequest _request { get; set; }
        public API API { get; set; }
        public Admin Admin { get; set; }


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
            Admin = new Admin(_request);
        }

        public HttpRequest GetRequest()
        {
            return _request;
        }
    }


}
