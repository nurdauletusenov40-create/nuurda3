using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Data;
using UniversityDbApp.Models;

namespace UniversityDbApp.Controllers;

public class HomeController(AppDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var model = new DashboardViewModel
        {
            DatabaseConnected = await context.Database.CanConnectAsync()
        };

        if (model.DatabaseConnected)
        {
            model.DepartmentCount = await context.Departments.CountAsync();
            model.TeacherCount = await context.Teachers.CountAsync();
            model.Departments = await context.Departments
                .Include(department => department.Teachers)
                .AsNoTracking()
                .OrderBy(department => department.Name)
                .ToListAsync();
            model.RecentTeachers = await context.Teachers
                .Include(teacher => teacher.Department)
                .AsNoTracking()
                .OrderByDescending(teacher => teacher.Id)
                .Take(5)
                .ToListAsync();
        }

        return View(model);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
