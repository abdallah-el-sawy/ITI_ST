using Microsoft.AspNetCore.Identity;
using Lab3.Models;
using Lab3.Repositories;
using Lab3.ViewModels;

namespace Lab3.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterViewModel viewModel)
        {
            if (await _unitOfWork.Users.IsDuplicateUserNameAsync(viewModel.UserName))
            {
                return (false, "This username is already taken.");
            }

            var user = new User { UserName = viewModel.UserName };
            user.PasswordHash = _passwordHasher.HashPassword(user, viewModel.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool Success, string? ErrorMessage, User? User)> ValidateLoginAsync(LoginViewModel viewModel)
        {
            var user = await _unitOfWork.Users.GetByUserNameAsync(viewModel.UserName);
            if (user == null)
            {
                return (false, "Invalid username or password.", null);
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, viewModel.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return (false, "Invalid username or password.", null);
            }

            return (true, null, user);
        }
    }
}