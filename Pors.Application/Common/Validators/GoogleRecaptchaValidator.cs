using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using FluentValidation;
using System.Threading.Tasks;
using FluentValidation.Validators;
using Microsoft.Extensions.Options;
using Pors.Application.Common.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Pors.Application.Common.Validators
{
    public class GoogleRecaptchaValidator<T> : PropertyValidator<T, string>
    {
        private GoogleRecaptchaSettings _recaptchaSettings;
        public override string Name => "GoogleRecaptchaValidator";
        private const string USE_CAPTCHA_ERROR_MESSAGE = "لطفا از کپچا استفاده کنید.";

        public override bool IsValid(ValidationContext<T> context, string value)
        {
            _recaptchaSettings = context
                .GetServiceProvider()
                .GetService<IOptions<GoogleRecaptchaSettings>>()?.Value;

            // Ignore captcha validation
            if (_recaptchaSettings == null)
            {
                return true;
            }

            if (value == null)
            {
                context.AddFailure(USE_CAPTCHA_ERROR_MESSAGE);
            }

            if (ValidateCaptcha(value).Result)
            {
                return true;
            }

            return false;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return "اعتبارسنجی '{PropertyName}' شما معتبر نیست؛ لطفا دوباره تلاش کنید.";
        }

        private async Task<bool> ValidateCaptcha(string googleResponse)
        {
            var apiUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={_recaptchaSettings.SecretKey}&response={googleResponse}";

            var apiResponse = await new HttpClient().GetAsync(apiUrl);

            if (apiResponse.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            var apijsonResponse = await apiResponse.Content.ReadAsStringAsync();

            var validationResult = JsonSerializer
                .Deserialize<GoogleRecaptchaResponse>(apijsonResponse);

            if (validationResult.Success)
            {
                return true;
            }

            return false;
        }
    }

    public static class GoogleRecaptchaValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidGoogleRecaptcha<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var validator = (PropertyValidator<T, string>)new GoogleRecaptchaValidator<T>();

            return ruleBuilder.SetValidator(validator);
        }
    }
}