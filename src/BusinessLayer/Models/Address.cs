using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models
{
    public class Address
    {
        [Required]
        [MaxLength(60)]
        public string Country { get; set; } = null!;

        [Required]
        [MaxLength(128)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(256)]
        public string Street { get; set; } = null!;

        [Required]
        public string StreetNumber { get; set; }

        [MaxLength(65)]
        public string? BuildingNumber { get; set; }

        [MaxLength(65)]
        public string? ApartmentNumber { get; set; }

        [MaxLength(1028)]
        public string? AdditionalInfo { get; set; } = null!;
    }
}
