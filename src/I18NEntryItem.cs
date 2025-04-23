namespace Quantum.I18n;

public class I18NEntryItem
{
    private const string Mark = "#";
    private const string NotFoundMark = "#";
    public static I18NEntryItem Null()
    {
        return new I18NEntryItem
        {
            Fa = Mark,
            En = Mark,
            Ar = Mark,
            Code = Mark
        };
    }

    public static I18NEntryItem Default(string resultParameterKey)
    {
        return new I18NEntryItem
        {
            Fa = resultParameterKey,
            En = resultParameterKey,
            Ar = resultParameterKey,
            Code = resultParameterKey
        };
    }

    public static I18NEntryItem NotFound()
    {
        return new I18NEntryItem
        {
            Fa = NotFoundMark,
            En = NotFoundMark,
            Ar = NotFoundMark,
            Code = NotFoundMark
        };
    }

    public string Fa { get; set; }
    public string En { get; set; }
    public string Ar { get; set; }
    public string Code { get; set; }

    public I18NEntryItem()
    {
        
    }
}