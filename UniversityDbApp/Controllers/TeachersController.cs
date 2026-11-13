using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Data;
using UniversityDbApp.Models;

namespace UniversityDbApp.Controllers;

public class TeachersController(AppDbContext context) : Controller
{
    public async Task<IActionResult> Index(string? search, int? departmentId)
    {
        IQueryable<Teacher> teachersQuery = context.Teachers
            .Include(teacher => teacher.Department)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
            teachersQuery = teachersQuery.Where(teacher => teacher.FullName.Contains(search) || teacher.Email.Contains(search));
        if (departmentId.HasValue)
            teachersQuery = teachersQuery.Where(teacher => teacher.DepartmentId == departmentId.Value);

        ViewBag.Search = search;
        ViewBag.DepartmentId = departmentId;
        await PopulateDepartmentsAsync(departmentId);
        return View(await teachersQuery.OrderBy(teacher => teacher.FullName).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
            return NotFound();

        var teacher = await context.Teachers
            .Include(item => item.Department)
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);
        return teacher is null ? NotFound() : View(teacher);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateDepartmentsAsync();
        return View(new Teacher());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FullName,Email,Subject,ExperienceYears,DepartmentId")] Teacher teacher)
    {
        await ValidateEmailAsync(teacher.Email);
        if (!ModelState.IsValid)
        {
            await PopulateDepartmentsAsync(teacher.DepartmentId);
            return View(teacher);
        }

        context.Add(teacher);
        await context.SaveChangesAsync();
        TempData["Success"] = "Жаңа оқытушы сәтті қосылды.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
            return NotFound();

        var teacher = await context.Teachers.FindAsync(id);
        if (teacher is null)
            return NotFound();
        await PopulateDepartmentsAsync(teacher.DepartmentId);
        return View(teacher);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Subject,ExperienceYears,DepartmentId")] Teacher teacher)
    {
        if (id != teacher.Id)
            return NotFound();

        await ValidateEmailAsync(teacher.Email, id);
        if (!ModelState.IsValid)
        {
            await PopulateDepartmentsAsync(teacher.DepartmentId);
            return View(teacher);
        }

        var existing = await context.Teachers.FindAsync(id);
        if (existing is null)
            return NotFound();

        existing.FullName = teacher.FullName;
        existing.Email = teacher.Email;
        existing.Subject = teacher.Subject;
        existing.ExperienceYears = teacher.ExperienceYears;
        existing.DepartmentId = teacher.DepartmentId;
        await context.SaveChangesAsync();
        TempData["Success"] = "Оқытушы ақпараты жаңартылды.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
            return NotFound();

        var teacher = await context.Teachers
            .Include(item => item.Department)
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);
        return teacher is null ? NotFound() : View(teacher);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var teacher = await context.Teachers.FindAsync(id);
        if (teacher is null)
            return NotFound();

        context.Teachers.Remove(teacher);
        await context.SaveChangesAsync();
        TempData["Success"] = "Оқытушы жазбасы жойылды.";
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidateEmailAsync(string email, int? currentId = null)
    {
        if (await context.Teachers.AnyAsync(teacher => teacher.Email == email && teacher.Id != currentId))
            ModelState.AddModelError(nameof(Teacher.Email), "Бұл электрондық пошта бұрын тіркелген.");
    }

    private async Task PopulateDepartmentsAsync(int? selectedId = null)
    {
        var departments = await context.Departments.AsNoTracking().OrderBy(item => item.Name).ToListAsync();
        ViewData["DepartmentId"] = new SelectList(departments, "Id", "Name", selectedId);
    }
}
