using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibApp.Models
{
    public class MovieReview
    {
        public MovieReview() { }

        public MovieReview(int accountId)
        {
            AccountId = accountId;
        }

        public int AccountId { get; set; }
        public string Review { get; set; } = "";
    }
}
