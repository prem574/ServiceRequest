using FluentValidation;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;

namespace ServiceRequestPlatform.Application.Validators.ServiceRequest_DTO_Validators
{
    public class RescheduleServiceRequestDtoValidator : AbstractValidator<RescheduleServiceRequestDto>
    {
        public RescheduleServiceRequestDtoValidator()
        {
            RuleFor(x => x.RequestId)
                .NotEmpty().WithMessage("Request ID is required.");

            RuleFor(x => x.NewDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("New date cannot be in the past.");

            RuleFor(x => x.NewTime)
                .Must(time => time >= TimeSpan.Zero && time < TimeSpan.FromDays(1))
                .WithMessage("New time must be a valid time of day.");
        }
    }
}