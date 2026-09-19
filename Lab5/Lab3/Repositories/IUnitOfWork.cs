namespace Lab3.Repositories
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees { get; }
        IDepartmentRepository Departments { get; }
        IUserRepository Users { get; }
        Task<int> SaveChangesAsync();
    }
}