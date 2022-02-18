using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Models;

public class Book
{
    public Guid BookId { get; set; }

    [Required]
    [MaxLength(17)]
    public string Isbn { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(4000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Genre { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string CoverUrl { get; set; } = string.Empty;

    [Required]
    public Guid? PublisherId { get; set; }

    [Required]
    [MinLength(1)]
    public ICollection<Guid>? AuthorsIds { get; set; }
}