using FluentValidation;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;

namespace ServiceRequestPlatform.Application.Validators.ServiceRequest_DTO_Validators
{
    public class ServiceRequestDtoValidator : AbstractValidator<ServiceRequestDto>
    {
        public ServiceRequestDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Service request ID is required.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(x => x.ServiceId)
                .NotEmpty().WithMessage("Service ID is required.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => new[] { "Pending", "Assigned", "InProgress", "Completed", "Cancelled", "Rescheduled" }
                    .Contains(status))
                .WithMessage("Invalid status value.");
        }
    }
}