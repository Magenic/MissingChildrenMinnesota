using MCM.Core.Helpers;
using MCM.Core.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace MCM.Core.Services
{
	[SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
	public sealed class MobileService : IMobileService
	{
		private readonly MobileServiceClient mobileService;
		private readonly IConfiguration configuration;
#if ANDROID || IOS
		private readonly IUiContext currentUiContext;

		public MobileService(IUiContext uiContext, IConfiguration configuration)
		{
			this.currentUiContext = uiContext;
			this.configuration = configuration;

			mobileService = new MobileServiceClient(
				configuration.GetValue<string>(Constants.ConfigurationKeys.MobileServiceUrl),
				configuration.GetValue<string>(Constants.ConfigurationKeys.MobileServiceKey));
		}
#else
		public MobileService(IConfiguration configuration)
		{
			this.configuration = configuration;

			mobileService = new MobileServiceClient(
				configuration.GetValue<string>(Constants.ConfigurationKeys.MobileServiceUrl),
				configuration.GetValue<string>(Constants.ConfigurationKeys.MobileServiceKey));
		}
#endif // ANDROID || IOS

		public async Task<User> AuthenticateAsync(AuthenticationProvider provider)
		{
#if ANDROID || IOS
			var user = await this.mobileService.LoginAsync(currentUiContext.CurrentContext, ToMobileServiceProvider(provider));
#else
			var user = await this.mobileService.LoginAsync(ToMobileServiceProvider(provider));
#endif // ANDROID || IOS
			return new User
			{
				MobileServiceAuthenticationToken = user.MobileServiceAuthenticationToken,
				UserId = user.UserId
			};
		}

		private static MobileServiceAuthenticationProvider ToMobileServiceProvider(AuthenticationProvider provider)
		{
			switch (provider)
			{
				case AuthenticationProvider.Facebook:
					return MobileServiceAuthenticationProvider.Facebook;

				case AuthenticationProvider.Google:
					return MobileServiceAuthenticationProvider.Google;

				case AuthenticationProvider.Twitter:
					return MobileServiceAuthenticationProvider.Twitter;

				case AuthenticationProvider.Microsoft:
				default:
					return MobileServiceAuthenticationProvider.MicrosoftAccount;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.mobileService != null)
				{
					this.mobileService.Dispose();
				}
			}
		}
	}
}
