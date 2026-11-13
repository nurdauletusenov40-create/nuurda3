using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Data;
using UniversityDbApp.Models;

namespace UniversityDbApp.Controllers;

public class DepartmentsController(AppDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var departments = await context.Departments
            .Include(department => department.Teachers)
            .AsNoTracking()
            .OrderBy(department => department.Name)
            .ToListAsync();
        return View(departments);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
            return NotFound();

        var department = await context.Departments
            .Include(item => item.Teachers)
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);
        return department is null ? NotFound() : View(department);
    }

    public IActionResult Create() => View(new Department());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description,OfficeLocation")] Department department)
    {
        if (!ModelState.IsValid)
            return View(department);

        department.CreatedAt = DateTime.UtcNow;
        context.Add(department);
        await context.SaveChangesAsync();
        TempData["Success"] = "Жаңа кафедра сәтті қосылды.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
            return NotFound();

        var department = await context.Departments.FindAsync(id);
        return department is null ? NotFound() : View(department);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,OfficeLocation")] Department department)
    {
        if (id != department.Id)
            return NotFound();
        if (!ModelState.IsValid)
            return View(department);

        var existing = await context.Departments.FindAsync(id);
        if (existing is null)
            return NotFound();

        existing.Name = department.Name;
        existing.Description = department.Description;
        existing.OfficeLocation = department.OfficeLocation;
        await context.SaveChangesAsync();
        TempData["Success"] = "Кафедра ақпараты жаңартылды.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
            return NotFound();

        var department = await context.Departments
            .Include(item => item.Teachers)
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);
        return department is null ? NotFound() : View(department);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var department = await context.Departments.FindAsync(id);
        if (department is null)
            return NotFound();

        context.Departments.Remove(department);
        await context.SaveChangesAsync();
        TempData["Success"] = "Кафедра және оған тиесілі оқытушылар жойылды.";
        return RedirectToAction(nameof(Index));
    }
}
