namespace UniversityDbApp.Models;

public class DashboardViewModel
{
    public int DepartmentCount { get; set; }
    public int TeacherCount { get; set; }
    public bool DatabaseConnected { get; set; }
    public IReadOnlyList<Department> Departments { get; set; } = [];
    public IReadOnlyList<Teacher> RecentTeachers { get; set; } = [];
}
