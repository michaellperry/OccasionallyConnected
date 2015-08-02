using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Bridge.Identity
{
    public class IdentityServiceProxy
    {
        private readonly Uri _baseUri;

        public string ApiKey { get; set; }

        public IdentityServiceProxy(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public async Task<Guid> GetTechnicianIdentifier(string login)
        {
            var uri = new Uri(_baseUri, login);

            using (var client = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(ApiKey))
                    client.DefaultRequestHeaders.Add("ApiKey", ApiKey);

                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();
                return Guid.Parse(result);
            }
        }
    }
}
