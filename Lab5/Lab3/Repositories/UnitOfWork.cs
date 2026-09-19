using Lab3.Data;

namespace Lab3.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IEmployeeRepository Employees { get; }
        public IDepartmentRepository Departments { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Employees = new EmployeeRepository(context);
            Departments = new DepartmentRepository(context);
            Users = new UserRepository(context);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}