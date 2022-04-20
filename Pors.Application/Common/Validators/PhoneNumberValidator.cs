using System;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace Pors.Application.Common.Validators
{
    public class PhoneNumberValidator<T> : PropertyValidator<T, string>, IRegularExpressionValidator
    {
        public string Expression => @"^0?9\d{9}$";
        public override string Name => "PhoneNumberValidator";

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            return Regex.IsMatch(value, Expression);
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return "'{PropertyName}' وارد شده قالب صحیح یک شماره تلفن را ندارد.";
        }
    }

    public static class PhoneNumberValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var validator = (PropertyValidator<T, string>)new PhoneNumberValidator<T>();

            return ruleBuilder.SetValidator(validator);
        }
    }
}
