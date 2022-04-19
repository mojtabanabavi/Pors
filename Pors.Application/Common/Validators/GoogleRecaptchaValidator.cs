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
    public class GoogleRecaptchaValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        private GoogleRecaptchaSettings _recaptchaSettings;
        public override string Name => "GoogleRecaptchaValidator";

        private const string URL_TEMPLATE = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
        private const string USE_CAPTCHA_ERROR_MESSAGE = "لطفا از کپچا برای اعتبارسنجی استفاده کنید";
        private const string CAPTCHA_VALIDATION_ERROR_MESSAGE = "اعتبارسنجی کپچای شما معتبر نیست؛ لطفا دوباره امتحان کنید";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
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

            if (ValidateRecaptcha(value).Result)
            {
                return true;
            }

            context.AddFailure(CAPTCHA_VALIDATION_ERROR_MESSAGE);

            return false;
        }

        private async Task<bool> ValidateRecaptcha(TProperty googleResponse)
        {
            var httpClient = new HttpClient();

            var apiUrl = string.Format(URL_TEMPLATE, _recaptchaSettings.SecretKey, googleResponse);

            var apiResponse = await httpClient.GetAsync(apiUrl);

            if (apiResponse.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            var apijsonResponse = await apiResponse.Content
                .ReadAsStringAsync();

            var validationResult = JsonSerializer
                .Deserialize<GoogleRecaptchaResponse>(apijsonResponse);

            if (validationResult.Success)
            {
                return true;
            }

            return false;
        }
    }
}