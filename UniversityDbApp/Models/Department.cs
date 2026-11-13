using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniversityDbApp.Models;

public class Department
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Кафедра атауын енгізіңіз.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Атауы 2–120 таңба аралығында болуы керек.")]
    [Display(Name = "Кафедра атауы")]
    public string Name { get; set; } = string.Empty;

    [StringLength(600, ErrorMessage = "Сипаттама 600 таңбадан аспауы керек.")]
    [Display(Name = "Сипаттамасы")]
    public string? Description { get; set; }

    [StringLength(120, ErrorMessage = "Кабинет атауы 120 таңбадан аспауы керек.")]
    [Display(Name = "Орналасқан жері")]
    public string? OfficeLocation { get; set; }

    [Display(Name = "Құрылған күні")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ValidateNever]
    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
