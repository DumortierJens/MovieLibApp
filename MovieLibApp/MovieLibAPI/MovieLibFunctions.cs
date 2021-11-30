using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using MovieLibAPI.Models;

namespace MovieLibAPI
{
    public static class MovieLibFunctions
    {
        [FunctionName("GetReviewByMovieId")]
        public static async Task<IActionResult> GetReviewByMovieId(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "reviews/{movieId}")] HttpRequest req,
            string movieId)
        {
            try
            {
                string accountId = req.Query["accountid"];

                string connectionString = Environment.GetEnvironmentVariable("StorageAccount");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
                CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                CloudTable table = cloudTableClient.GetTableReference("reviews");

                TableQuery<MovieReviewEntity> rangeQuery = new TableQuery<MovieReviewEntity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, accountId))
                    .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, movieId));

                var queryResult = await table.ExecuteQuerySegmentedAsync<MovieReviewEntity>(rangeQuery, null);
                List<MovieReview> movieReviews = new List<MovieReview>();

                foreach (var reg in queryResult.Results)
                {
                    movieReviews.Add(new MovieReview()
                    {
                        AccountId = reg.AccountId,
                        MovieId = reg.MovieId,
                        Review = reg.Review
                    });
                }

                return new OkObjectResult(movieReviews);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
                throw;
            }
        }

        [FunctionName("GetReviews")]
        public static async Task<IActionResult> GetReviews(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "reviews")] HttpRequest req)
        {
            try
            {
                string accountId = req.Query["accountid"];

                string connectionString = Environment.GetEnvironmentVariable("StorageAccount");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
                CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                CloudTable table = cloudTableClient.GetTableReference("reviews");

                TableQuery<MovieReviewEntity> rangeQuery = new TableQuery<MovieReviewEntity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, accountId));

                var queryResult = await table.ExecuteQuerySegmentedAsync<MovieReviewEntity>(rangeQuery, null);
                List<MovieReview> movieReviews = new List<MovieReview>();

                foreach (var reg in queryResult.Results)
                {
                    movieReviews.Add(new MovieReview()
                    {
                        AccountId = reg.AccountId,
                        MovieId = reg.MovieId,
                        Review = reg.Review
                    });
                }

                return new OkObjectResult(movieReviews);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
                throw;
            }
        }

        [FunctionName("AddMovieReview")]
        public static async Task<IActionResult> AddMovieReview(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "reviews")] HttpRequest req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                MovieReview movieReview = JsonConvert.DeserializeObject<MovieReview>(requestBody);

                MovieReviewEntity movieReviewEntity = new MovieReviewEntity(movieReview);

                string connectionString = Environment.GetEnvironmentVariable("StorageAccount");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
                CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                CloudTable table = cloudTableClient.GetTableReference("reviews");
                await table.CreateIfNotExistsAsync();

                TableOperation insertOperation = TableOperation.Insert(movieReviewEntity);
                await table.ExecuteAsync(insertOperation);

                return new OkObjectResult(movieReview);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
                throw;
            }
        }

        [FunctionName("UpdateMovieReview")]
        public static async Task<IActionResult> UpdateMovieReview(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "reviews")] HttpRequest req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                MovieReview movieReview = JsonConvert.DeserializeObject<MovieReview>(requestBody);

                MovieReviewEntity movieReviewEntity = new MovieReviewEntity(movieReview);

                string connectionString = Environment.GetEnvironmentVariable("StorageAccount");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
                CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                CloudTable table = cloudTableClient.GetTableReference("reviews");
                await table.CreateIfNotExistsAsync();

                TableOperation insertOperation = TableOperation.Merge(movieReviewEntity);
                await table.ExecuteAsync(insertOperation);

                return new OkObjectResult(movieReview);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
                throw;
            }
        }

        [FunctionName("DeleteMovieReview")]
        public static async Task<IActionResult> DeleteMovieReview(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "reviews")] HttpRequest req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                MovieReview movieReview = JsonConvert.DeserializeObject<MovieReview>(requestBody);

                MovieReviewEntity movieReviewEntity = new MovieReviewEntity(movieReview);

                string connectionString = Environment.GetEnvironmentVariable("StorageAccount");
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
                CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
                CloudTable table = cloudTableClient.GetTableReference("reviews");
                await table.CreateIfNotExistsAsync();

                TableOperation insertOperation = TableOperation.Delete(movieReviewEntity);
                await table.ExecuteAsync(insertOperation);

                return new OkObjectResult(movieReview);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
                throw;
            }
        }
    }
}
