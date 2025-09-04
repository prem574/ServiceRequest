using FluentValidation;
using ServiceRequestPlatform.Application.DTOs.AvailabilitySlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Application.Validators.AvailabilitySlot_DTO_Validators
{
    public class UpdateAvailabilitySlotDtoValidator : AbstractValidator<UpdateAvailabilitySlotDto>
    {
        public UpdateAvailabilitySlotDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Slot ID is required.");

            RuleFor(x => x.AvailableDate)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Available date cannot be in the past.");

            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime).WithMessage("Start time must be before end time.");
        }
    }
}
