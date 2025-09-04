using FluentValidation;
using ServiceRequestPlatform.Application.DTOs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Application.Validators.Service_DTO_Validators
{
    public class ServiceDtoValidator : AbstractValidator<ServiceDto>
    {
        public ServiceDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Service ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Service name is required.");

           
        }
    }
}
