using FluentValidation;
using ServiceRequestPlatform.Application.DTOs.Admin;

namespace ServiceRequestPlatform.Application.Validators.Admin_DTO_Validators
{
    public class AdminDtoValidator : AbstractValidator<AdminDto>
    {
        public AdminDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Admin ID is required.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}

