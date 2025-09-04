using FluentValidation;
using ServiceRequestPlatform.Application.DTOs.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Application.Validators.Worker_DTO_Validators
{
    public class WorkerDtoValidator : AbstractValidator<WorkerDto>
    {
        public WorkerDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Worker ID is required.");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }
}
