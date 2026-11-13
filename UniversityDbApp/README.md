# UniversityDbApp

Абай атындағы Қазақ ұлттық педагогикалық университетінің кафедралары мен оқытушылар құрамын басқаруға арналған ASP.NET Core MVC веб-қосымшасы. Жоба Week 3 зертханалық жұмысының **«EF Core жүйесін орнату және дерекқор құрылымын Code-First арқылы құру»** тақырыбына сәйкес жасалған.

## Зертханалық жұмыстың мақсаты

- Entity Framework Core және SQL Server провайдерін қолдану.
- Code-First тәсілімен модельдер мен дерекқор құрылымын жасау.
- `Department` және `Teacher` арасындағы бірден-көпке байланысты конфигурациялау.
- Миграцияны құрып, SQL Server дерекқорына қолдану.
- ASP.NET Core MVC арқылы CRUD операцияларын орындау.

## Технологиялар

- C# және ASP.NET Core MVC (`net10.0`)
- Entity Framework Core 10
- Microsoft SQL Server Express
- Razor Views, HTML, CSS, JavaScript
- Bootstrap 5

## Дерекқор архитектурасы

`Department` кафедраның атауын, сипаттамасын, орналасқан жерін және құрылған күнін сақтайды. `Teacher` оқытушының аты-жөнін, поштасын, пәнін, еңбек өтілін және `DepartmentId` сыртқы кілтін сақтайды.

Байланыс: **Department (1) → Teacher (N)**. `Department.Teachers` және `Teacher.Department` навигациялық қасиеттері Fluent API арқылы байланысқан. Әр оқытушы бір кафедраға бекітіледі; кафедра жойылғанда оған тиесілі оқытушылар каскадты түрде жойылады. Оқытушының электрондық поштасы қайталанбауы үшін бірегей индекс қойылған.

## Жоба құрылымы

```text
UniversityDbApp/
├── Controllers/       # Home, Departments, Teachers
├── Data/              # AppDbContext және бастапқы деректер
├── Migrations/        # EF Core миграциялары
├── Models/             # Department, Teacher, DashboardViewModel
├── Views/              # Razor беттері
├── wwwroot/            # CSS, JavaScript және статикалық файлдар
├── appsettings.json    # SQL Server қосылым жолы
├── Program.cs          # MVC және DbContext DI тіркеуі
└── UniversityDbApp.csproj
```

## NuGet пакеттері

- `Microsoft.EntityFrameworkCore.SqlServer` 10.0.15 — EF Core-дың SQL Server провайдері.
- `Microsoft.EntityFrameworkCore.Tools` 10.0.15 — миграцияларды басқару құралдары.

## SQL Server қосылымы

Жоба осы компьютерде табылып, қосылымы тексерілген `localhost\SQLEXPRESS` данасын пайдаланады. Бастапқы конфигурация `appsettings.json` файлында:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=UniversityDbAppDb;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```

Windows Integrated Authentication қолданылады; құпиясөз репозиторийге сақталмайды. SQL Server данасының атауы басқа ортада өзгеше болса, қосылым жолындағы `Server` мәнін жаңартыңыз. SQL Server Express қызметі жұмыс істеп тұруы керек.

## Миграцияны құру және қолдану

`dotnet-ef` әлі орнатылмаса, жоба каталогында орнатыңыз. Репозиторийде `InitialDb` миграциясы дайын; дерекқорды жаңа ортада құру үшін `database update` жеткілікті:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.15
dotnet ef database update
```

Егер модельдерді өзгертіп, алғашқы миграцияны жаңадан жасап жатсаңыз, миграцияны тек бір рет жасаңыз. Visual Studio Package Manager Console командалары:

```powershell
Add-Migration InitialDb
Update-Database
```

Қолданыстағы осы жобаға `Add-Migration InitialDb` командасын қайта орындамаңыз: миграция репозиторийде бар. Модель өзгерген сайын жаңа атаумен миграция жасап, `dotnet ef database update` командасын орындаңыз. Мысал деректері алғашқы миграция арқылы SQL Server-ге қосылады: 5 кафедра және 12 оқытушы.

## SQL Server кестелерін тексеру

SSMS немесе `sqlcmd` арқылы `localhost\SQLEXPRESS` серверіне Windows Authentication арқылы қосылып, келесі сұрауларды орындаңыз:

```sql
USE UniversityDbAppDb;

SELECT name
FROM sys.tables
WHERE name IN ('Departments', 'Teachers', '__EFMigrationsHistory');

SELECT
    fk.name AS ForeignKeyName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS ForeignKeyColumn,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable,
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS ReferencedColumn
FROM sys.foreign_keys AS fk
JOIN sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id;

SELECT * FROM dbo.__EFMigrationsHistory;
```

`Departments` пен `Teachers` кестелері, `Teachers.DepartmentId → Departments.Id` сыртқы кілті және `InitialDb` миграциясы көрінуі керек.

## Іске қосу

Алдымен миграцияны қолданыңыз, содан соң:

```powershell
dotnet restore
dotnet run
```

Терминалда көрсетілген жергілікті HTTP мекенжайын браузерде ашыңыз (әдепкі порт — `5134`). Басты беттегі статистика дерекқордан оқылады. Кафедралар мен оқытушылар бөлімдерінде жазбаларды көру, қосу, өңдеу, ашу, жою, оқытушыны аты-жөні/поштасы бойынша іздеу және кафедра бойынша сүзу мүмкіндігі бар.

## Скриншоттар

GitHub жарияланымына `docs/screenshots/` каталогына келесі скриншоттарды қосуға болады:

1. Басқару тақтасы: статистика және SQL Server қосылым күйі.
2. Кафедралар тізімі және кафедра мәліметтері.
3. Оқытушылар тізімі, кафедра сүзгісі және іздеу нәтижесі.
4. Оқытушыны кафедраға бекіту формасы.
5. SSMS жүйесіндегі кестелер, сыртқы кілт және `__EFMigrationsHistory`.

## GitHub-қа жариялау

GitHub-та бос репозиторий ашып, оның мекенжайын төмендегі `<репозиторий-URL>` орнына қойыңыз:

```powershell
git init
git add .
git commit -m "Build UniversityDbApp with EF Core Code-First"
git branch -M main
git remote add origin <репозиторий-URL>
git push -u origin main
```

Қосылым жолына құпиясөз енгізбеңіз. Нақты ортаға арналған құпия конфигурацияны `appsettings.Local.json` немесе User Secrets ішінде сақтаңыз; олар `.gitignore` ережелерінде қамтылған.

## Орындаған

- **Студент:** Үсенов Нұрдаулет
- **Тобы:** ИСК-242, 3 курс
- **Университет:** Абай атындағы Қазақ ұлттық педагогикалық университеті
