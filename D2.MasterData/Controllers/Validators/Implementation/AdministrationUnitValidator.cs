using D2.MasterData.Models;
using FluentValidation;

namespace D2.MasterData.Controllers.Validators.Implementation
{
    public class AdministrationUnitValidator : AbstractValidator<AdministrationUnit>, ModelValidator<AdministrationUnit>
    {
        public AdministrationUnitValidator()
        {
            RuleSet("Post", () => {
                RuleFor(unit => unit.UserKey).NotEmpty();
                RuleFor(unit => unit.Title).NotEmpty();
            });
        }

        public ValidationResult Validate(AdministrationUnit instance, params string[] ruleSets)
        {
            var result = DefaultValidatorExtensions.Validate(this, instance, ruleSet: string.Join(",", ruleSets));

            return new ValidationResultImpl(result);
        }
    }
}
