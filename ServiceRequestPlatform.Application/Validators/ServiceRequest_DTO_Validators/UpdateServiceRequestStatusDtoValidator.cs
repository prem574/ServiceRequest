using FluentValidation;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;

namespace ServiceRequestPlatform.Application.Validators.ServiceRequest_DTO_Validators
{
    public class UpdateServiceRequestStatusDtoValidator : AbstractValidator<UpdateServiceRequestStatusDto>
    {
        public UpdateServiceRequestStatusDtoValidator()
        {
            RuleFor(x => x.RequestId)
                .NotEmpty().WithMessage("Request ID is required.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => new[] { "Pending", "Assigned", "InProgress", "Completed", "Cancelled", "Rescheduled" }
                    .Contains(status))
                .WithMessage("Invalid status value.");

            // Only validate date/time if status is Rescheduled
            RuleFor(x => x.NewRequestedDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .When(x => x.Status == "Rescheduled")
                .WithMessage("New requested date cannot be in the past.");

            RuleFor(x => x.NewRequestedTime)
                .Must(time => time >= TimeSpan.Zero && time < TimeSpan.FromDays(1))
                .When(x => x.Status == "Rescheduled")
                .WithMessage("New requested time must be a valid time of day.");
        }
    }
}
