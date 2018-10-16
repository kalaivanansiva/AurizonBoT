using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.Sample.LuisBot
{
    public class RestService
    {

       static HttpClient client = new HttpClient();

        public static async Task<T> GetLeavesAsync<T>(string path) where T : new()
        {           
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                MicrosoftAppCredentials.TrustServiceUrl(path);
                HttpResponseMessage response = await client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    var strresponse = await response.Content.ReadAsStringAsync();
                    var levbal=(T)JsonConvert.DeserializeObject(strresponse, typeof(T));
                    return levbal;
                }
            }
            catch(Exception)
            {
               
            }
            return (T)(object)"{}";
        }

        public RestService()
        {

        }

    }
}
