using ServiceRequestPlatform.Application.DTOs.AvailabilitySlot;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Application.Validators.AvailabilitySlot_DTO_Validators
{
    public class CreateAvailabilitySlotDtoValidator : AbstractValidator<CreateAvailabilitySlotDto>
    {
        public CreateAvailabilitySlotDtoValidator()
        {
            RuleFor(x => x.WorkerId)
                .NotEmpty().WithMessage("Worker ID is required.");

            RuleFor(x => x.AvailableDate)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Available date cannot be in the past.");

            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime).WithMessage("Start time must be before end time.");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");
        }
    }
}
