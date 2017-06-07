namespace RM.CommonLibrary.ConfigurationMiddleware
{
    public interface IConfigurationHelper
    {
        string ReadAppSettingsConfigurationValues(string key);
    }
}