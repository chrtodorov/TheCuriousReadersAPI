﻿using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models
{
    public class Address
    {
        [Required]
        [MaxLength(60)]
        public string Country { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Street { get; set; } = null!;

        [Required]
        public int StreetNumber { get; set; }

        public int? BuildingNumber { get; set; }

        public int? ApartmentNumber { get; set; }

        public string? AdditionalInfo { get; set; } = null!;
    }
}
