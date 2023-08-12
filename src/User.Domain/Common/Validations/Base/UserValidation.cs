using FluentValidation;
using User.Domain.Service.User.Entities;

namespace User.Domain.Common.Validations.Base
{
    public class UserValidation : AbstractValidator<UserEntity>
    {
        public UserValidation()
        {
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .Length(3, 30);

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}
