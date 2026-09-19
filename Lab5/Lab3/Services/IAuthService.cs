using Lab3.Models;
using Lab3.ViewModels;

namespace Lab3.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterViewModel viewModel);
        Task<(bool Success, string? ErrorMessage, User? User)> ValidateLoginAsync(LoginViewModel viewModel);
    }
}