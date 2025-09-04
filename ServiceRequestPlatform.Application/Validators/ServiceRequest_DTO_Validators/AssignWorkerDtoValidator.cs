using FluentValidation;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;

namespace ServiceRequestPlatform.Application.Validators.ServiceRequest_DTO_Validators
{
    public class AssignWorkerDtoValidator : AbstractValidator<AssignWorkerDto>
    {
        public AssignWorkerDtoValidator()
        {
            RuleFor(x => x.RequestId)
                .NotEmpty().WithMessage("Request ID is required.");

            RuleFor(x => x.WorkerId)
                .NotEmpty().WithMessage("Worker ID is required.");
        }
    }
}
