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
        private static readonly string _APIKEY = UserSecretsRepository.Settings["APIKEY"];
        private static readonly string _SESSIONID = UserSecretsRepository.Settings["SESSIONID"];
        private const string _BASEURI = "https://api.themoviedb.org/3";

        private static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        /// <summary>
        /// Create API request url
        /// </summary>
        /// <param name="endpoint">Endpoint of the request</param>
        /// <param name="paramString">Params of the request</param>
        /// <returns>API request url</returns>
        private static string GetURL(string endpoint, string paramString = "")
        {
            return $"{_BASEURI}{endpoint}?api_key={_APIKEY}&session_id={_SESSIONID}&{paramString}";
        }

        /// <summary>
        /// Get the account id
        /// </summary>
        /// <returns>Account id</returns>
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

        /// <summary>
        /// Get page of popular movies
        /// </summary>
        /// <param name="pageId">Pagenumber</param>
        /// <returns>Popular movie page</returns>
        public static async Task<PopularMoviePage> GetPopularMoviesAsync(int pageId = 1)
        {
            string endpoint = "/movie/popular";
            string paramString = $"page={pageId}";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = await client.GetStringAsync(GetURL(endpoint, paramString));

                    if (json != null)
                        return JsonConvert.DeserializeObject<PopularMoviePage>(json);

                    return new PopularMoviePage();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Get page of searched movies
        /// </summary>
        /// <param name="searchQuery">Query for searching a movie</param>
        /// <param name="pageId">Pagenumber</param>
        /// <returns>Search movie page</returns>
        public static async Task<SearchMoviePage> SearchMovieAsync(string searchQuery, int pageId = 1)
        {
            string endpoint = "/search/movie";
            string paramString = $"page={pageId}&query={searchQuery}";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = await client.GetStringAsync(GetURL(endpoint, paramString));

                    if (json != null)
                    {
                        SearchMoviePage searchMoviePage = JsonConvert.DeserializeObject<SearchMoviePage>(json);
                        searchMoviePage.SearchQuery = searchQuery;
                        return searchMoviePage;
                    }

                    return new SearchMoviePage();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Get page of favorite movies of an account
        /// </summary>
        /// <param name="accountId">Id of the account</param>
        /// <param name="pageId">Pagenumber</param>
        /// <returns>Favorite movie page</returns>
        public static async Task<FavoriteMoviePage> GetFavoriteMoviesAsync(int accountId, int pageId = 1)
        {
            string endpoint = $"/account/{accountId}/favorite/movies";
            string paramString = $"page={pageId}";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = await client.GetStringAsync(GetURL(endpoint, paramString));

                    if (json != null)
                    {
                        FavoriteMoviePage favoriteMoviePage = JsonConvert.DeserializeObject<FavoriteMoviePage>(json);
                        favoriteMoviePage.AccountId = accountId;
                        return favoriteMoviePage;
                    }

                    return new FavoriteMoviePage();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Update favorite movie of an account
        /// </summary>
        /// <param name="accountId">Id of the account</param>
        /// <param name="movieId">Id of the movie</param>
        /// <param name="isFavorite">Is favorite movie</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get movie details of a movie
        /// </summary>
        /// <param name="movieId">Id of the movie</param>
        /// <returns>Movie details</returns>
        public static async Task<Movie> GetMovieDetailsAsync(int movieId)
        {
            string endpoint = $"/movie/{movieId}";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = await client.GetStringAsync(GetURL(endpoint));

                    if (json != null)
                        return JsonConvert.DeserializeObject<Movie>(json);

                    return new Movie();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Get account details of a movie
        /// </summary>
        /// <param name="movie">The movie</param>
        /// <returns>Movie with account details</returns>
        public static async Task<Movie> GetMovieAccountDetailsAsync(Movie movie)
        {
            string endpoint = $"/movie/{movie.Id}/account_states";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = await client.GetStringAsync(GetURL(endpoint));

                    if (json != null)
                    {
                        JObject jObject = JsonConvert.DeserializeObject<JObject>(json);
                        JToken dataFavorite = jObject.SelectToken("favorite");
                        movie.IsFavorite = dataFavorite.ToObject<bool>();
                    }

                    return movie;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
