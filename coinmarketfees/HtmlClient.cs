using Newtonsoft.Json;
using RestSharp;

namespace coinmarketfees
{
    public class HtmlClient
    {
        protected const string _baseUrl = "https://coinmarketfees.com";
        protected readonly RestClient client = new RestClient();

        protected virtual string FetchHtml(string url)
        {
            var client = new RestClient();
            var r = new RestRequest(url, Method.Get);
            var response = client.Execute(r);
            var html = response.Content;
            return html;
        }

        public virtual string GetExchange(string name)
        {
            string url = $"{_baseUrl}/exchange/{name}";
            return FetchHtml(url);
        }

        public virtual string GetExchanges()
        {
            string url = $"{_baseUrl}/exchanges";
            return FetchHtml(url);
        }

        public virtual string GetFeesInfo(int exchange, int page = 1)
        {
            string url = $"{_baseUrl}/search-all.php?action=exchange_search_coin&exchange={exchange}&dataPage={page}";
            var html = FetchHtml(url);
            var dyna = JsonConvert.DeserializeObject(html);
            html = (dyna as dynamic).html;
            return html;
        }
    }
}
