using MovieLibApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibApp.Repositories
{
    public static class MovieRepository
    {
        private const string _APIKEY = "f78e1fcb5f014afcc2dcfcbe5ae93cf5";
        private const string _SESSIONID = "6f68c0c7b0880f54ffce05a62d4c0081ddeb9d73";
        private const string _BASEURI = "https://api.themoviedb.org/3";

        private static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        private static string GetURL(string endpoint, string paramString = "")
        {
            return $"{_BASEURI}{endpoint}?api_key={_APIKEY}&session_id={_SESSIONID}&{paramString}";
        }

        public static async Task<MoviePage> GetPopularMoviesAsync(int pageId = 1)
        {
            string endpoint = "/movie/popular";
            string paramString = $"page={pageId}";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = await client.GetStringAsync(GetURL(endpoint, paramString));

                    if (json != null)
                        return JsonConvert.DeserializeObject<MoviePage>(json);

                    return new MoviePage();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<MoviePage> SearchMovieAsync(string query, int pageId = 1)
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
                        MoviePage moviePage = JsonConvert.DeserializeObject<MoviePage>(json);
                        moviePage.Query = query;
                        return moviePage;
                    }

                    return new MoviePage();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<int> GetAccountId()
        {
            string endpoint = "/account";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = await client.GetStringAsync(GetURL(endpoint));

                    if (json != null)
                    {
                        JObject jObject = JsonConvert.DeserializeObject<JObject>(json);
                        JToken data = jObject.SelectToken("id");
                        return data.ToObject<int>();
                    }

                    return -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<MoviePage> GetFavoriteMoviesAsync(int accountId, int pageId = 1)
        {
            string endpoint = $"/account/{accountId}/favorite/movies";
            string paramString = $"page={pageId}";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = await client.GetStringAsync(GetURL(endpoint, paramString));

                    if (json != null)
                        return JsonConvert.DeserializeObject<MoviePage>(json);

                    return new MoviePage();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task UpdateMovieAsFavoriteAsync(int accountId, int movieId, bool isFavorite)
        {
            string endpoint = $"/account/{accountId}/favorite";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(new { 
                        media_type = "movie",
                        media_id = movieId,
                        favorite = isFavorite
                    });
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(GetURL(endpoint), content);
                    if (!response.IsSuccessStatusCode)
                        throw new Exception(response.ToString());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
