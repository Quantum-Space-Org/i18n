using System.Collections.Generic;

namespace Quantum.I18n;

public class ApiResult
{
    public bool IsSucceeded { get; set; }
    public string Message { get; set; }
    public List<string> Messages { get; set; }
    public string Code { get; set; }
}