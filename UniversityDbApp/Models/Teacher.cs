using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniversityDbApp.Models;

public class Teacher
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Оқытушының аты-жөнін енгізіңіз.")]
    [StringLength(160, MinimumLength = 3, ErrorMessage = "Аты-жөні 3–160 таңба аралығында болуы керек.")]
    [Display(Name = "Аты-жөні")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Электрондық поштаны енгізіңіз.")]
    [EmailAddress(ErrorMessage = "Электрондық пошта пішімін тексеріңіз.")]
    [StringLength(180)]
    [Display(Name = "Электрондық пошта")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пәнді енгізіңіз.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Пән атауы 2–120 таңба аралығында болуы керек.")]
    [Display(Name = "Оқытатын пәні")]
    public string Subject { get; set; } = string.Empty;

    [Range(0, 60, ErrorMessage = "Еңбек өтілі 0–60 жыл аралығында болуы керек.")]
    [Display(Name = "Еңбек өтілі (жыл)")]
    public int ExperienceYears { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Кафедраны таңдаңыз.")]
    [Display(Name = "Кафедра")]
    public int DepartmentId { get; set; }

    [ValidateNever]
    public Department Department { get; set; } = null!;
}
