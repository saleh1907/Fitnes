using System.ComponentModel.DataAnnotations;

namespace WebApplication4.ViewModels.TrainerViewModels;

public class TrainerCreateVM
{
    [Required,MaxLength(256),MinLength(3)]
    public string Name { get; set; } = string.Empty;
    [Required,MaxLength(1024),MinLength(3)]
    public string Description { get; set; } = string.Empty;
    [Required]
    public int DepartamentId { get; set; }
    [Required]
    public IFormFile Image { get; set; } = null!;
}
