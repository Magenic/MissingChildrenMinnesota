using MCM.Core.Models;
using System;
using System.Threading.Tasks;

namespace MCM.Core.Services
{
	public enum AuthenticationProvider
	{
		Facebook,
		Google,
		Microsoft,
		Twitter
	}

	public interface IMobileService : IDisposable
	{
		Task<User> AuthenticateAsync(AuthenticationProvider provider);
	}
}
