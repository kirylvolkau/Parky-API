using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Repository
{
    public class AccountRepository : Repository<User> ,IAccountRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        
        public AccountRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<User> LoginAsync(string url, User objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), 
                    Encoding.UTF8, 
                    "application/json");
            }
            else
            {
                return new User();
            }
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(jsonString);
            }
            else
            {
                return new User();
            }
        }

        public async Task<bool> RegisterAsync(string url, User objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), 
                    Encoding.UTF8, 
                    "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

       
    }
}