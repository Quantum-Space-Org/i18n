using Quantum.Configurator;

namespace Quantum.I18n.Configurator;

public static class ConfigI18NExtensions
{
    public static ConfigI18NBuilder ConfigI18n(this QuantumServiceCollection collection) =>
        new(collection);
}