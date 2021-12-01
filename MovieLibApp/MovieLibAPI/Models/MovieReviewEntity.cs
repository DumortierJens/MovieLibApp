using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibAPI.Models
{
    public class MovieReviewEntity : TableEntity
    {
        public MovieReviewEntity()
        {

        }

        public MovieReviewEntity(MovieReview movieReview)
        {
            this.PartitionKey = movieReview.AccountId.ToString();
            this.RowKey = movieReview.MovieId.ToString();
            this.ETag = "*";


            Review = movieReview.Review;
        }

        public int AccountId => int.Parse(PartitionKey);
        public int MovieId => int.Parse(RowKey);
        public string Review { get; set; }
    }
}
