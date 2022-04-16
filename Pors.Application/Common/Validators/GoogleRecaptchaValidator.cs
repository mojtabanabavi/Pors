using System;
using System.Net;
using System.Dynamic;
using System.Net.Http;
using Loby.Extensions;
using System.Text.Json;
using FluentValidation;
using System.Threading.Tasks;
using FluentValidation.Validators;
using Microsoft.Extensions.Options;
using Pors.Application.Common.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Pors.Application.Common.Validators
{
    public class GoogleRecaptchaValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        public override string Name => "GoogleRecaptchaValidator";
        private GoogleRecaptchaSettings _recaptchaSettings;
        private const string URL_TEMPLATE = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
        private const string DEFAULT_ERROR_MESSAGE = "لطفا از کپچا برای اعتبارسنجی استفاده کنید";
        private const string VALIDATION_ERROR_MESSAGE = "اعتبارسنجی کپچای شما معتبر نیست؛ لطفا دوباره امتحان کنید";

        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            _recaptchaSettings = context
                .GetServiceProvider()
                .GetService<IOptions<GoogleRecaptchaSettings>>().Value;

            var googleRecaptchaResponse = value as string;

            if (!googleRecaptchaResponse.HasValue())
            {
                context.AddFailure(DEFAULT_ERROR_MESSAGE);

                return false;
            }

            if (ValidateRecaptcha(googleRecaptchaResponse).Result)
            {
                return true;
            }

            context.AddFailure(VALIDATION_ERROR_MESSAGE);

            return false;
        }

        private async Task<bool> ValidateRecaptcha(string googleRecaptchaResponse)
        {
            var httpClient = new HttpClient();

            var apiUrl = string.Format(URL_TEMPLATE, _recaptchaSettings.SecretKey, googleRecaptchaResponse);

            var apiResponse = await httpClient.GetAsync(apiUrl);

            if (apiResponse.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            var apijsonResponse = await apiResponse.Content.ReadAsStringAsync();

            dynamic apiData = JsonSerializer.Deserialize<ExpandoObject>(apijsonResponse);

            if (apiData.success.ToString().ToLower() != "true")
            {
                return false;
            }

            return true;
        }
    }
}