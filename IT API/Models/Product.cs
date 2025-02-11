﻿namespace ProductApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; } 
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string[] Category { get; set; }
    }
}
