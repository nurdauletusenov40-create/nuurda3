using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Models;

namespace UniversityDbApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Teacher> Teachers => Set<Teacher>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(department => department.Name).HasMaxLength(120).IsRequired();
            entity.Property(department => department.Description).HasMaxLength(600);
            entity.Property(department => department.OfficeLocation).HasMaxLength(120);
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.Property(teacher => teacher.FullName).HasMaxLength(160).IsRequired();
            entity.Property(teacher => teacher.Email).HasMaxLength(180).IsRequired();
            entity.Property(teacher => teacher.Subject).HasMaxLength(120).IsRequired();
            entity.HasIndex(teacher => teacher.Email).IsUnique();
            entity.HasOne(teacher => teacher.Department)
                .WithMany(department => department.Teachers)
                .HasForeignKey(teacher => teacher.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Педагогика және психология", Description = "Педагогика, білім беру психологиясы және тәрбие жұмысы.", OfficeLocation = "Бас корпус, 312-кабинет", CreatedAt = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Department { Id = 2, Name = "Математика және информатика", Description = "Математика, деректер ғылымы және цифрлық технологиялар.", OfficeLocation = "№ 2 корпус, 214-кабинет", CreatedAt = new DateTime(2026, 1, 16, 0, 0, 0, DateTimeKind.Utc) },
            new Department { Id = 3, Name = "Қазақ тілі мен әдебиеті", Description = "Қазақ тіл білімі, әдебиеттану және академиялық жазылым.", OfficeLocation = "Бас корпус, 408-кабинет", CreatedAt = new DateTime(2026, 1, 17, 0, 0, 0, DateTimeKind.Utc) },
            new Department { Id = 4, Name = "Жаратылыстану ғылымдары", Description = "Биология, экология және химия бағыттары.", OfficeLocation = "№ 3 корпус, 126-кабинет", CreatedAt = new DateTime(2026, 1, 18, 0, 0, 0, DateTimeKind.Utc) },
            new Department { Id = 5, Name = "Шетел тілдері", Description = "Ағылшын тілі, лингвистика және халықаралық коммуникация.", OfficeLocation = "№ 4 корпус, 305-кабинет", CreatedAt = new DateTime(2026, 1, 19, 0, 0, 0, DateTimeKind.Utc) });

        modelBuilder.Entity<Teacher>().HasData(
            new Teacher { Id = 1, FullName = "Айгерім Сәрсенова", Email = "a.sarsenova@university.kz", Subject = "Педагогика негіздері", ExperienceYears = 12, DepartmentId = 1 },
            new Teacher { Id = 2, FullName = "Данияр Қасымов", Email = "d.kassymov@university.kz", Subject = "Білім беру психологиясы", ExperienceYears = 8, DepartmentId = 1 },
            new Teacher { Id = 3, FullName = "Ерлан Нұртаев", Email = "e.nurtayev@university.kz", Subject = "Алгоритмдер және бағдарламалау", ExperienceYears = 10, DepartmentId = 2 },
            new Teacher { Id = 4, FullName = "Мадина Әлиева", Email = "m.alieva@university.kz", Subject = "Деректер қоры", ExperienceYears = 6, DepartmentId = 2 },
            new Teacher { Id = 5, FullName = "Арман Оразбек", Email = "a.orazbek@university.kz", Subject = "Дискретті математика", ExperienceYears = 14, DepartmentId = 2 },
            new Teacher { Id = 6, FullName = "Гүлнар Бекенова", Email = "g.bekenova@university.kz", Subject = "Қазақ әдебиетінің тарихы", ExperienceYears = 18, DepartmentId = 3 },
            new Teacher { Id = 7, FullName = "Нұрсұлтан Әбдіров", Email = "n.abdirov@university.kz", Subject = "Қазіргі қазақ тілі", ExperienceYears = 5, DepartmentId = 3 },
            new Teacher { Id = 8, FullName = "Динара Мырзахмет", Email = "d.myrzakhmet@university.kz", Subject = "Экология және тұрақты даму", ExperienceYears = 9, DepartmentId = 4 },
            new Teacher { Id = 9, FullName = "Алихан Төлеген", Email = "a.tolegen@university.kz", Subject = "Жалпы биология", ExperienceYears = 7, DepartmentId = 4 },
            new Teacher { Id = 10, FullName = "Sarah Thompson", Email = "s.thompson@university.kz", Subject = "Academic English", ExperienceYears = 11, DepartmentId = 5 },
            new Teacher { Id = 11, FullName = "Меруерт Ибраева", Email = "m.ibraeva@university.kz", Subject = "Ағылшын тілі грамматикасы", ExperienceYears = 4, DepartmentId = 5 },
            new Teacher { Id = 12, FullName = "James Wilson", Email = "j.wilson@university.kz", Subject = "Intercultural Communication", ExperienceYears = 15, DepartmentId = 5 });
    }
}
