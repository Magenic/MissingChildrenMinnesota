
namespace MCM.Core.Helpers
{
    public interface IConfiguration
    {
		T GetValue<T>(string key);
    }
}
