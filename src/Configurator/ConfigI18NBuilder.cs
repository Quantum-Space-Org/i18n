using Quantum.Configurator;

namespace Quantum.I18n.Configurator;

public class ConfigI18NBuilder
{
    private readonly QuantumServiceCollection _collection;

    public ConfigI18NBuilder(QuantumServiceCollection collection) 
        => _collection = collection;

    public ConfigI18NBuilder LoadI18n(string content, string @namespace = DomainValidationExceptionToMessage.DefaultI18N)
    {
        DomainValidationExceptionToMessage.SetI18nJson(content, @namespace);
        return this;
    }

    public QuantumServiceCollection and() 
        => _collection;
}