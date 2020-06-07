using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Repository
{
    public class Repository<T> : IRepository<T> where T:class
    {
        private IHttpClientFactory _clientFactory;

        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        
        public async Task<T> GetAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url+id);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
            return null;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
            }
            return null;

        }

        public async Task<bool> CreateAsync(string url, T objToCreate)
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
           return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url+id);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> UpdateAsync(string url, T objToUpdate, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url+id);
            if (objToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToUpdate), 
                    Encoding.UTF8, 
                    "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            return response.StatusCode == HttpStatusCode.NoContent;
        }
    }
}