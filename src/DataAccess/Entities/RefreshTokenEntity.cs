using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class RefreshTokenEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RefreshTokenId { get; set; }
        public string Token { get; set; } = null!;

        public DateTime ExpiresOn { get; set; }

        public UserEntity User { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
