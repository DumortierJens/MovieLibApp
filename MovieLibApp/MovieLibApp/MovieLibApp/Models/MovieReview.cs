using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibApp.Models
{
    public class MovieReview
    {
        public int MovieId { get; set; }
        public int AccountId { get; set; }
        public string Review { get; set; }
    }
}
