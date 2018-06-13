using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AbstractPrinteryWpf
{
    public static class APIClient
    {
        private static HttpClient customer = new HttpClient();

        public static void Connect()
        {
            customer.BaseAddress = new Uri(ConfigurationManager.AppSettings["IPAddress"]);
            customer.DefaultRequestHeaders.Accept.Clear();
            customer.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static Task<HttpResponseMessage> GetRequest(string requestUrl)
        {
            return customer.GetAsync(requestUrl);
        }

        public static Task<HttpResponseMessage> PostRequest<T>(string requestUrl, T model)
        {
            return customer.PostAsJsonAsync(requestUrl, model);
        }

        public static T GetElement<T>(Task<HttpResponseMessage> response)
        {
            return response.Result.Content.ReadAsAsync<T>().Result;
        }

        public static string GetError(Task<HttpResponseMessage> response)
        {
            return response.Result.Content.ReadAsStringAsync().Result;
        }
    }
}
