using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{

    public class CustomerEntity : UserEntity
    {
        [Required]
        public AddressEntity Address { get; set; } = null!;
        public Guid AddressId { get; set; }
    }
}
