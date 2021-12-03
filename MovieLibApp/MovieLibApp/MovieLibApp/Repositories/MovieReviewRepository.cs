using MovieLibApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibApp.Repositories
{
    public static class MovieReviewRepository
    {
        private const string _BASEURI = "https://movielibapp.azurewebsites.net/api";

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
        /// <returns>API request url</returns>
        private static string GetURL(string endpoint)
        {
            return $"{_BASEURI}{endpoint}";
        }

        /// <summary>
        /// Get review details of a movie
        /// </summary>
        /// <param name="accountId">Id of the account</param>
        /// <param name="movie">The movie</param>
        /// <returns>Movie with review details</returns>
        public static async Task<Movie> GetMovieReviewAsync(int accountId, Movie movie)
        {
            string endpoint = $"/reviews/{movie.Id}?accountid={accountId}";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = await client.GetStringAsync(GetURL(endpoint));

                    movie.MovieReview = new MovieReview(accountId);
                    if (json != null)
                    {
                        List<MovieReview> results = JsonConvert.DeserializeObject<List<MovieReview>>(json);
                        if (results.Count > 0)
                            movie.MovieReview = results[0];
                    }

                    return movie;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Add a movie review
        /// </summary>
        /// <param name="movieId">Id of the movie</param>
        /// <param name="movieReview">Movie review with a account id and the review</param>
        public static async Task AddMovieReviewAsync(int movieId, MovieReview movieReview)
        {
            string endpoint = $"/reviews";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(new {
                        movieId,
                        accountId = movieReview.AccountId,
                        review = movieReview.Review
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
        /// Update a movie review
        /// </summary>
        /// <param name="movieId">Id of the movie</param>
        /// <param name="movieReview">Movie review with a account id and the review</param>
        public static async Task UpdateMovieReviewAsync(int movieId, MovieReview movieReview)
        {
            string endpoint = $"/reviews";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(new
                    {
                        movieId,
                        accountId = movieReview.AccountId,
                        review = movieReview.Review
                    });
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync(GetURL(endpoint), content);
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
        /// Delete a movie review
        /// </summary>
        /// <param name="movieId">Id of the movie</param>
        /// <param name="movieReview">Movie review with a account id</param>
        public static async Task DeleteMovieReviewAsync(int movieId, MovieReview movieReview)
        {
            string endpoint = $"/reviews";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(new
                    {
                        movieId,
                        accountId = movieReview.AccountId,
                        review = movieReview.Review
                    });
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Content = content,
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(GetURL(endpoint))
                    };

                    var response = await client.SendAsync(request);
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
