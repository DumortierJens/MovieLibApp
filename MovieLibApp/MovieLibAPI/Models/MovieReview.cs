using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibAPI.Models
{
    public class MovieReview
    {
        public int MovieId { get; set; }
        public int AccountId { get; set; }
        public string Review { get; set; }
    }
}
