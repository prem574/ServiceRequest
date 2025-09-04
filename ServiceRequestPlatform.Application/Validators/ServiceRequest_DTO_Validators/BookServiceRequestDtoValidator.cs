using FluentValidation;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;

namespace ServiceRequestPlatform.Application.Validators.ServiceRequest_DTO_Validators
{
    public class BookServiceRequestDtoValidator : AbstractValidator<BookServiceRequestDto>
    {
        public BookServiceRequestDtoValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(x => x.ServiceId)
                .NotEmpty().WithMessage("Service ID is required.");

            RuleFor(x => x.RequestedDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Requested date cannot be in the past.");
            When(x => x.AvailabilitySlotId == null, () =>
        {
                RuleFor(x => x.RequestedDate)
                              .GreaterThanOrEqualTo(DateTime.Today)
                                .WithMessage("Requested date cannot be in the past.");
                        });

            RuleFor(x => x.RequestedTime)
                .Must(time => time >= TimeSpan.Zero && time < TimeSpan.FromDays(1))
                .WithMessage("Requested time must be a valid time of day.");
            When(x => x.AvailabilitySlotId == null, () =>
       {
                RuleFor(x => x.RequestedTime)
                                 .Must(time => time >= TimeSpan.Zero && time < TimeSpan.FromDays(1))
                                .WithMessage("Requested time must be a valid time of day.");
                        });

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Service address is required.")
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");
        }
    }
}
