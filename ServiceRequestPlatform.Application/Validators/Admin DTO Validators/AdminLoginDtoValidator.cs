using FluentValidation;
using ServiceRequestPlatform.Application.DTOs.Admin;

namespace ServiceRequestPlatform.Application.Validators.Admin_DTO_Validators
{
    public class AdminLoginDtoValidator : AbstractValidator<AdminLoginDto>
    {
        public AdminLoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}