using System;

namespace Quantum.I18n;

public class MessageKeyNotFoundInJsonFileException(string resultMessageKey) : Exception
{
    public string ResultMessageKey { get; } = resultMessageKey;
}