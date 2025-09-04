using FluentValidation;
using ServiceRequestPlatform.Application.DTOs.Rating;

namespace ServiceRequestPlatform.Application.Validators.Rating_DTO_Validators
{
    public class CreateRatingDtoValidator : AbstractValidator<CreateRatingDto>
    {
        public CreateRatingDtoValidator()
        {
            RuleFor(x => x.ServiceRequestId)
                .NotEmpty().WithMessage("Service request ID is required.");

            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(x => x.WorkerId)
                .NotEmpty().WithMessage("Worker ID is required.");

            RuleFor(x => x.Score)
                .InclusiveBetween(1, 5).WithMessage("Rating score must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.");
        }
    }
}