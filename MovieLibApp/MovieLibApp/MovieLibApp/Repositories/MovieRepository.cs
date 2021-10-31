using MovieLibApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibApp.Repositories
{
    public static class MovieRepository
    {
        private const string _APIKEY = "f78e1fcb5f014afcc2dcfcbe5ae93cf5";
        private const string _BASEURI = "https://api.themoviedb.org/3";

        private static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        private static string GetURL(string endpoint, string paramString = "")
        {
            return $"{_BASEURI}{endpoint}?api_key={_APIKEY}&{paramString}";
        }

        public static async Task<MovieResult> GetPopularMoviesAsync(int pageId = 1)
        {
            string endpoint = "/movie/popular";
            string paramString = $"page={pageId}";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = await client.GetStringAsync(GetURL(endpoint, paramString));

                    if (json != null)
                        return JsonConvert.DeserializeObject<MovieResult>(json);

                    return new MovieResult();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<MovieResult> SearchMovieAsync(string query, int pageId = 1)
        {
            string endpoint = "/search/movie";
            string paramString = $"page={pageId}&query={query}";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = await client.GetStringAsync(GetURL(endpoint, paramString));

                    if (json != null)
                    {
                        var result = JsonConvert.DeserializeObject<MovieResult>(json);
                        result.Query = query;
                        return result;
                    }

                    return new MovieResult();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
