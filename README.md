# ITI_ST
## This ReadMe Contain How to Run The Project on your local PC and the whole Routes of Lab3,4 and 5 you can check it


## How to Run

1. Clone the repository
```bash
   git clone <repository-url>
   cd Lab3
```

2. Restore dependencies
```bash
   dotnet restore
```

3. Set your connection string in `appsettings.json`
```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Lab3Db;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
```

4. Apply migrations
```bash
   dotnet ef database update
```

5. Run the project
```bash
   dotnet run
```





BackEnd Tasks from ITI in SummerTraining

## Lab3
### Routes

| Function           | Route                      |
| ------------------ | -------------------------- |
| عرض كل الأقسام     | `/Departments/Index`       |
| إضافة Department   | `/Departments/Create`      |
| تعديل Department   | `/Departments/Edit/1`      |
| عرض Employees لقسم | `/Departments/Employees/1` |
| عرض كل الموظفين    | `/Employees/Index`         |
| إضافة Employee     | `/Employees/Create`        |
| تعديل Employee     | `/Employees/Edit/1`        |




## ----------------------------------------------------------------------------------

## Lab4
### Routes
## Routes / Endpoints

All routing uses ASP.NET Core's default conventional route defined in `Program.cs`:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

No `[Route]` attributes are used — every URL below comes from this single pattern, which is why some actions are reachable through more than one URL (see Notes column).

| HTTP Method | Route / URL | Controller | Action | What it does | Notes |
|---|---|---|---|---|---|
| GET | `/` | HomeController | `Index()` | Shows welcome message (from `UserName` cookie) and visit counter (from Session) | Same action as `/Home` and `/Home/Index` |
| GET | `/Home` | HomeController | `Index()` | Same as above | Default action for this controller |
| GET | `/Home/Index` | HomeController | `Index()` | Same as above | Explicit form of the same URL |
| GET | `/Home/SetUserName` | HomeController | `SetUserName()` | Displays the form to enter/change the username | Pre-fills with the current `UserName` cookie value if set |
| POST | `/Home/SetUserName` | HomeController | `SetUserName(UserNameViewModel)` | Validates input, saves username into a Cookie (30-day expiry), redirects to Home | Same URL as the GET above, different HTTP verb |
| POST | `/Home/SetTheme` | HomeController | `SetTheme(string theme)` | Saves `Light`/`Dark` into the `Theme` cookie, redirects back to the page you came from | Triggered by the Light/Dark buttons in the layout banner on every page |
| GET | `/Home/Privacy` | HomeController | `Privacy()` | Default template privacy page | Not part of Lab 4 requirements, left from scaffolding |
| GET | `/Home/Error` | HomeController | `Error()` | Displays the friendly error page with a Request Id | Also invoked automatically by `app.UseExceptionHandler("/Home/Error")` on unhandled exceptions (Production mode only) |
| GET | `/Account/Login` | AccountController | `Login()` | Displays the login form | |
| POST | `/Account/Login` | AccountController | `Login(LoginViewModel)` | Validates input, stores username in Session (`LoggedInUser`), redirects to Home | Same URL as GET, different verb |
| GET | `/Account/Logout` | AccountController | `Logout()` | Clears the entire Session, redirects to Login | |
| GET | `/Dashboard` | DashboardController | `Index()` | Async queries: total employees, total departments, average/highest/lowest salary | Default action for this controller |
| GET | `/Dashboard/Index` | DashboardController | `Index()` | Same as above | Explicit form of the same URL |
| GET | `/Employees` | EmployeesController | `Index(string? searchName)` | Lists all employees (with Department name via `Include`) | Default action for this controller |
| GET | `/Employees/Index` | EmployeesController | `Index(string? searchName)` | Same as above | Explicit form of the same URL |
| GET | `/Employees?searchName=...` | EmployeesController | `Index(string? searchName)` | Filters the list by employee name (case-insensitive `Contains`) | `searchName` is saved to the `EmployeeSearch` cookie when present; read back from the cookie when absent (e.g. a plain refresh), so the filter persists |
| GET | `/Employees/Create` | EmployeesController | `Create()` | Displays the Add Employee form with populated Department dropdown | |
| POST | `/Employees/Create` | EmployeesController | `Create(EmployeeViewModel)` | Validates + checks duplicate name-in-department, saves new employee | Same URL as GET, different verb |
| GET | `/Employees/Edit/{id}` | EmployeesController | `Edit(int? id)` | Loads employee by Id, displays pre-filled Edit form | `{id}` is a required route parameter |
| POST | `/Employees/Edit/{id}` | EmployeesController | `Edit(int id, EmployeeViewModel)` | Validates + duplicate check (excluding itself), updates employee | Same URL as GET, different verb |
| POST | `/Employees/Delete/{id}` | EmployeesController | `Delete(int id)` | Deletes the employee with the given Id | No GET version — triggered by a form/button on Index with a JS confirm dialog |
| GET | `/Departments` | DepartmentsController | `Index()` | Lists departments with employee count (via `Include(d => d.Employees)`) | Default action for this controller |
| GET | `/Departments/Index` | DepartmentsController | `Index()` | Same as above | Explicit form of the same URL |
| GET | `/Departments/Create` | DepartmentsController | `Create()` | Displays the Add Department form | |
| POST | `/Departments/Create` | DepartmentsController | `Create(DepartmentViewModel)` | Validates + checks duplicate name (case-insensitive), saves new department | Same URL as GET, different verb |
| GET | `/Departments/Edit/{id}` | DepartmentsController | `Edit(int? id)` | Loads department by Id, displays pre-filled Edit form | |
| POST | `/Departments/Edit/{id}` | DepartmentsController | `Edit(int id, DepartmentViewModel)` | Validates + duplicate check (excluding itself), updates department | Same URL as GET, different verb |
| POST | `/Departments/Delete/{id}` | DepartmentsController | `Delete(int id)` | Deletes the department with the given Id | No GET version — same pattern as Employee delete |
| GET | `/Departments/Employees/{id}` | DepartmentsController | `Employees(int id)` | Shows all employees in that department (the "Show Employees" / Details page) | Also writes the department's name into the `RecentDepartments` session list (max 5, newest first) |

**Middleware note:** `RequestLoggerMiddleware` and `MaintenanceMiddleware` aren't tied to a single route — they run globally, in front of every route above, on every request.

### Route Flow Summary

- **Login → Session → Employees/Departments → Logout**
  `/Account/Login` (POST) stores the username in Session. That value is read in `_Layout.cshtml` on every page to show "Logged in as: X" in the nav bar. `/Account/Logout` clears the Session, so the nav falls back to showing "Login" again.

- **Employee Search → Cookie → Refresh → Search value restored**
  Submitting the search box does a GET to `/Employees?searchName=ahmed`. The controller writes `ahmed` into the `EmployeeSearch` cookie and filters the list. Visiting plain `/Employees` later (no query string) reads `searchName` back from that cookie and re-applies the same filter automatically.

- **Department → Show Employees → Session → Recently Visited Departments**
  Clicking "Show Employees" hits `/Departments/Employees/{id}`, which adds that department's name to the front of a Session list (`RecentDepartments`), trims it to 5 items, and displays it in a banner on the same page.

- **Theme Cookie → Every Page**
  `POST /Home/SetTheme` writes `Light`/`Dark` into the `Theme` cookie and redirects back to the referring page. `_Layout.cshtml` reads that cookie on every request and applies the matching CSS class site-wide.


## ----------------------------------------------------------------------------------

## Lab5
## Routes

All routes are generated by the default ASP.NET Core conventional route configured in `Program.cs`:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

No `[Route]` attributes are used anywhere in the project — every URL below is produced by this single pattern. Authentication is enforced via `[Authorize]` on the controller level (Cookie Authentication scheme), with `/Account/Login` set as the login redirect path.

### Home

| HTTP Method | Route | Controller / Action | Authentication | Description |
|-------------|-------|--------------------|----------------|-------------|
| GET | `/` | HomeController / Index() | Not required | Displays welcome message (from `UserName` cookie) and visit counter (from Session) |
| GET | `/Home` | HomeController / Index() | Not required | Same as above (default controller route) |
| GET | `/Home/Index` | HomeController / Index() | Not required | Same as above (explicit action name) |
| GET | `/Home/SetUserName` | HomeController / SetUserName() | Not required | Displays the form to set/change the display username |
| POST | `/Home/SetUserName` | HomeController / SetUserName(UserNameViewModel) | Not required | Validates input and saves the username into a Cookie (30-day expiry) |
| POST | `/Home/SetTheme` | HomeController / SetTheme(string theme) | Not required | Saves the selected theme (`Light`/`Dark`) into a Cookie, redirects back to the referring page |
| GET | `/Home/Privacy` | HomeController / Privacy() | Not required | Default scaffolded privacy page |
| GET | `/Home/Error` | HomeController / Error() | Not required | Displays a friendly error page; also invoked automatically by `UseExceptionHandler("/Home/Error")` on unhandled exceptions (Production mode) |

### Authentication

| HTTP Method | Route | Controller / Action | Authentication | Description |
|-------------|-------|--------------------|----------------|-------------|
| GET | `/Account/Register` | AccountController / Register() | Not required | Displays the registration form |
| POST | `/Account/Register` | AccountController / Register(RegisterViewModel) | Not required | Validates input, checks for duplicate username, hashes the password, creates a new `User` record |
| GET | `/Account/Login` | AccountController / Login() | Not required | Displays the login form |
| POST | `/Account/Login` | AccountController / Login(LoginViewModel) | Not required | Validates credentials against the hashed password, signs the user in via Cookie Authentication, stores username in Session |
| GET | `/Account/Logout` | AccountController / Logout() | Not required (accessible any time to clear state) | Signs the user out of the authentication cookie and clears the Session, redirects to Login |

### Dashboard

| HTTP Method | Route | Controller / Action | Authentication | Description |
|-------------|-------|--------------------|----------------|-------------|
| GET | `/Dashboard` | DashboardController / Index() | **Required** (`[Authorize]`) | Displays Total Employees, Total Departments, Average/Highest/Lowest Salary (all retrieved asynchronously via `IUnitOfWork`) |
| GET | `/Dashboard/Index` | DashboardController / Index() | **Required** (`[Authorize]`) | Same as above (explicit action name) |

### Departments

| HTTP Method | Route | Controller / Action | Authentication | Description |
|-------------|-------|--------------------|----------------|-------------|
| GET | `/Departments` | DepartmentsController / Index() | **Required** (`[Authorize]`) | Lists all departments with employee counts |
| GET | `/Departments/Index` | DepartmentsController / Index() | **Required** (`[Authorize]`) | Same as above (explicit action name) |
| GET | `/Departments/Create` | DepartmentsController / Create() | **Required** (`[Authorize]`) | Displays the Add Department form |
| POST | `/Departments/Create` | DepartmentsController / Create(DepartmentViewModel) | **Required** (`[Authorize]`) | Validates input, checks case-insensitive duplicate name, creates a new department |
| GET | `/Departments/Edit/{id}` | DepartmentsController / Edit(int? id) | **Required** (`[Authorize]`) | Loads the department by `{id}` and displays a pre-filled Edit form |
| POST | `/Departments/Edit/{id}` | DepartmentsController / Edit(int id, DepartmentViewModel) | **Required** (`[Authorize]`) | Validates input, checks duplicate name excluding itself, updates the department |
| POST | `/Departments/Delete/{id}` | DepartmentsController / Delete(int id) | **Required** (`[Authorize]`) | Deletes the department with the given `{id}` |
| GET | `/Departments/Employees/{id}` | DepartmentsController / Employees(int id) | **Required** (`[Authorize]`) | Displays all employees belonging to the department (`{id}`); also updates the "Recently Visited Departments" list in Session |

### Employees

| HTTP Method | Route | Controller / Action | Authentication | Description |
|-------------|-------|--------------------|----------------|-------------|
| GET | `/Employees` | EmployeesController / Index(string? searchName) | **Required** (`[Authorize]`) | Lists all employees with their department name |
| GET | `/Employees/Index` | EmployeesController / Index(string? searchName) | **Required** (`[Authorize]`) | Same as above (explicit action name) |
| GET | `/Employees?searchName={value}` | EmployeesController / Index(string? searchName) | **Required** (`[Authorize]`) | Filters employees by name (case-insensitive). If `searchName` is provided, it is saved to the `EmployeeSearch` cookie; if omitted, the last saved value is read back from that cookie so the filter persists across refreshes |
| GET | `/Employees/Create` | EmployeesController / Create() | **Required** (`[Authorize]`) | Displays the Add Employee form with the Department dropdown populated |
| POST | `/Employees/Create` | EmployeesController / Create(EmployeeViewModel) | **Required** (`[Authorize]`) | Validates input, checks duplicate name within the same department, creates a new employee |
| GET | `/Employees/Edit/{id}` | EmployeesController / Edit(int? id) | **Required** (`[Authorize]`) | Loads the employee by `{id}` and displays a pre-filled Edit form |
| POST | `/Employees/Edit/{id}` | EmployeesController / Edit(int id, EmployeeViewModel) | **Required** (`[Authorize]`) | Validates input, checks duplicate name excluding itself, updates the employee |
| POST | `/Employees/Delete/{id}` | EmployeesController / Delete(int id) | **Required** (`[Authorize]`) | Deletes the employee with the given `{id}` |

### Notes

- Middleware (`RequestLoggerMiddleware`, `MaintenanceMiddleware`) is not tied to specific routes — both run globally on every incoming request, including static assets, and are not represented as individual endpoints in the table above.
- All routes in **Dashboard**, **Departments**, and **Employees** require an authenticated user; unauthenticated requests are automatically redirected to `/Account/Login` via the configured Cookie Authentication `LoginPath`.
- No routing information was unclear or missing — every route above was confirmed directly against the current controller source code.


  
