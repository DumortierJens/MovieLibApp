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
        private const string _BASEURI = "http://192.168.1.51:7071/api";

        private static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        private static string GetURL(string endpoint)
        {
            return $"{_BASEURI}{endpoint}";
        }

        public static async Task<MovieReview> GetMovieReviewAsync(int accountId, Movie movie)
        {
            string endpoint = $"/reviews/{movie.Id}?accountid={accountId}";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = await client.GetStringAsync(GetURL(endpoint));

                    if (json != null)
                        return JsonConvert.DeserializeObject<List<MovieReview>>(json)[0];

                    return new MovieReview();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task AddMovieReviewAsync(MovieReview movieReview)
        {
            string endpoint = $"/reviews";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(movieReview);
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

        public static async Task UpdateMovieReviewAsync(MovieReview movieReview)
        {
            string endpoint = $"/reviews";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(movieReview);
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

        public static async Task DeleteMovieReviewAsync(MovieReview movieReview)
        {
            string endpoint = $"/reviews";

            using (HttpClient client = GetClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(movieReview);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Content = content,
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(endpoint)
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
