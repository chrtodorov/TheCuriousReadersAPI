using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities;

public class AuthorEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid AuthorId { get; set; }

    [MaxLength(30)]
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(30)]
    [Required]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string Bio { get; set; } = string.Empty;

    public ICollection<BookEntity>? Books { get; set; }
}