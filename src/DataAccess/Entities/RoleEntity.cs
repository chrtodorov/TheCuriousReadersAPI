using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class RoleEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RoleId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<CustomerEntity> Customers { get; set; } = new HashSet<CustomerEntity>();
        public ICollection<LibrarianEntity> Librarians { get; set; } = new HashSet<LibrarianEntity>();
    }
}
