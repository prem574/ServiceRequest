using FluentValidation;
using ServiceRequestPlatform.Application.DTOs.Rating;


namespace ServiceRequestPlatform.Application.Validators.Rating_DTO_Validators
{
    public class RatingDtoValidator : AbstractValidator<RatingDto>
    {
        public RatingDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Rating ID is required.");

            RuleFor(x => x.Score)
                .InclusiveBetween(1, 5).WithMessage("Rating score must be between 1 and 5.");
        }
    }
}
