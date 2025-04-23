using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Quantum.Core;

namespace Quantum.I18n;

public static class ApiResultExtensions
{
    public static ApiResult ToApiResult(this ValidationResult validationResult
        , HttpContext context)
    {
        var requestHeader = context.Request.Headers["Accept-Language"];

        if(requestHeader == "")
            return ApiResult(validationResult, Language.fa);

        return requestHeader.ToString().ToUpper() switch
        {
            "FA" => ApiResult(validationResult, Language.fa),
            "EN" => ApiResult(validationResult, Language.en),
            "AR" => ApiResult(validationResult, Language.ar),
            _ => ApiResult(validationResult, Language.fa)
        };
    }
    public static ApiResult ToApiResult(this ValidationResult validationResult)
    {
        return ApiResult(validationResult , Language.fa);
    }

    private static ApiResult ApiResult(ValidationResult validationResult
    , Language language)
    {
        if (validationResult.IsSucceeded)
            return new ApiResult { IsSucceeded = validationResult.IsSucceeded };

        List<string> messages = new();

        foreach (var validationResultResult in validationResult.Results)
        {
            try
            {
                if(language == Language.fa)
                    messages.Add(validationResultResult.Farsi());
                if (language == Language.en)
                    messages.Add(validationResultResult.English());

                if (language == Language.en)
                    messages.Add(validationResultResult.Arabic());
            }
            catch (I18nIsNotSetException exception)
            {
                messages.Add($"خطایی رخ داده است. فایل خطاها تنظیم نشده است.{validationResultResult.MessageKey}");
            }
            catch (MessageKeyNotFoundInJsonFileException exception)
            {
                messages.Add($"{validationResultResult.Message}");
            }
        }

        return new ApiResult
        {
            IsSucceeded = false,
            Messages = messages,
            Message = messages.Aggregate((a, b) => $"{a}{Environment.NewLine}{b}")
        };
    }
}

public enum Language
{
    fa,
    en,
    ar
}