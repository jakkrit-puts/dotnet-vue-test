using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models;

public class User
{

    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // public User() { }

}
