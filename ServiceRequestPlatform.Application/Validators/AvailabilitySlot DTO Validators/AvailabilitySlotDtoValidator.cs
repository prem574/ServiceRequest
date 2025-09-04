using FluentValidation;
using ServiceRequestPlatform.Application.DTOs.AvailabilitySlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Application.Validators.AvailabilitySlot_DTO_Validators
{
    public class AvailabilitySlotDtoValidator : AbstractValidator<AvailabilitySlotDto>
    {
        public AvailabilitySlotDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Slot ID is required.");

            RuleFor(x => x.WorkerId)
                .NotEmpty().WithMessage("Worker ID is required.");

            RuleFor(x => x.AvailableDate)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Available date cannot be in the past.");
        }
    }
}
