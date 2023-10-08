using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace RecipesForEveryone.Models
{
    public class SpoonacularApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.spoonacular.com/recipes/";

        public SpoonacularApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetRandomRecipes(int numberOfRecipes, string apiKey)
        {
            string url = $"{BaseUrl}random?apiKey={apiKey}&number={numberOfRecipes}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
}