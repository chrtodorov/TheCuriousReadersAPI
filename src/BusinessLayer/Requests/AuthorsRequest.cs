using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.Requests;

public class AuthorsRequest
{
    [MaxLength(30)]
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(30)]
    [Required]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string Bio { get; set; } = string.Empty;
}