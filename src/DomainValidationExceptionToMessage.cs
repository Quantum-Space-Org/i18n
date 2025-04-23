using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quantum.Core;

namespace Quantum.I18n;

public static class DomainValidationExceptionToMessage
{
    internal const string DefaultI18N = "i18n";

    private static readonly Dictionary<string, string> I18Ns = new();

    public static void SetI18nJson(string i18n)
        => I18Ns[DefaultI18N] = i18n;

    public static void SetI18nJson(string i18n, string @namespace)
        => I18Ns[@namespace] = i18n;

    public static string Farsi(this Result result)
    {
        var deserializeObject = DeserializeObject(result.Namespace);

        var message = JsonConvert.DeserializeObject<I18NEntryItem>(GetMessageValue(result, deserializeObject));

        var faEnParams =
            result.GetType() == typeof(ParameterBasedResult)
                ? result.ParameterKeys.Select(a => new I18NEntryItem { Code = "0", En = a, Fa = a }).ToList()
                : GetParameterKeys(result.ParameterKeys, deserializeObject);

        NormalizeParameters(message, faEnParams);

        return string.Format(message.Fa, faEnParams.Select(a => a.Fa).ToArray());
    }

    public static string English(this Result result)
    {
        var deserializeObject = DeserializeObject(result.Namespace);

        var message = JsonConvert.DeserializeObject<I18NEntryItem>(GetMessageValue(result, deserializeObject));

        var faEnParams =

             result.GetType() == typeof(ParameterBasedResult)
                 ? result.ParameterKeys.Select(a => new I18NEntryItem
                 {
                     Code = "0", 
                     En = a, 
                     Fa = a,
                     Ar= a,
                 }).ToList()
                 : GetParameterKeys(result.ParameterKeys, deserializeObject);

        NormalizeParameters(message, faEnParams);

        return string.Format(message.En, faEnParams.Select(a => a.En).ToArray());
    }
    
    public static string Arabic(this Result result)
    {
        var deserializeObject = DeserializeObject(result.Namespace);

        var message = JsonConvert.DeserializeObject<I18NEntryItem>(GetMessageValue(result, deserializeObject));

        var faEnParams =

            result.GetType() == typeof(ParameterBasedResult)
                ? result.ParameterKeys.Select(a => new I18NEntryItem
                {
                    Code = "0",
                    En = a,
                    Fa = a,
                    Ar= a,
                }).ToList()
                : GetParameterKeys(result.ParameterKeys, deserializeObject);

        NormalizeParameters(message, faEnParams);

        return string.Format(message.Ar, faEnParams.Select(a => a.Ar)
            .ToArray());
    }


    private static void NormalizeParameters(I18NEntryItem? message, ICollection<I18NEntryItem> faEnParams)
    {
        var length = message.Fa.Split("{").Length - 1;
        if (faEnParams.Count < length)
        {
            for (var i = 0; i < length - faEnParams.Count; i++)
            {
                faEnParams.Add(I18NEntryItem.NotFound());
            }
        }
    }

    private static string GetMessageValue(Result result, JObject? deserializeObject)
    {
        string messageValue;
        try
        {
            messageValue = deserializeObject[result.MessageKey].ToString();
        }
        catch (NullReferenceException e)
        {
            throw new MessageKeyNotFoundInJsonFileException(result.MessageKey);
        }

        return messageValue;
    }

    private static List<I18NEntryItem> GetParameterKeys(List<string> parametersKey,
        JObject? deserializeObject)
    {
        var result = new List<I18NEntryItem>();

        if (parametersKey is null)
            return result;
        if (deserializeObject is null)
            return result;

        result.AddRange(parametersKey.Select(resultParameterKey => ToI18NEntryItem(deserializeObject, resultParameterKey)));

        return result;
    }

    private static I18NEntryItem ToI18NEntryItem(JObject? deserializeObject, string resultParameterKey)
    {
        if (deserializeObject is null)
            return I18NEntryItem.Null();

        try
        {
            var s = deserializeObject[resultParameterKey].ToString();
            return JsonConvert.DeserializeObject<I18NEntryItem>(s);
        }
        catch (NullReferenceException e)
        {
            Console.WriteLine(e);
            return I18NEntryItem.Default(resultParameterKey);
        }
    }

    private static JObject DeserializeObject(string @namespace)
        => JsonConvert.DeserializeObject<JObject>(GetJson(@namespace));

    private static string GetJson(string ns)
    {
        if (I18Ns.TryGetValue(ns, out var res))
            return res;

        throw new I18nIsNotSetException();
    }
}