using BusinessLayer.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Requests
{
    public record UserRequest
    {
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string EmailAddress { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public AccountStatus Status { get; set; } = AccountStatus.Pending;

        [Required]
        public string RoleName { get; set; } = null!;

        [MaxLength(60)]
        public string? Country { get; set; } = null!;

        [MaxLength(100)]
        public string? City { get; set; } = null!;

        [MaxLength(100)]
        public string? Street { get; set; } = null!;

        public int? StreetNumber { get; set; }

        public int? BuildingNumber { get; set; }

        public int? ApartmentNumber { get; set; }

        public string? AdditionalInfo { get; set; } = null!;
    }
}
