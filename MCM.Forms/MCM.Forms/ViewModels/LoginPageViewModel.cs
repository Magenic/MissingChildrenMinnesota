using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MCM.Core.Models;
using MCM.Core.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MCM.Forms.ViewModels
{
	public class LoginPageViewModel : BaseViewModel
	{
		private readonly IMobileService mobileService;

		public LoginPageViewModel(IMobileService mobileService)
		{
			this.mobileService = mobileService;
		}

		public void GoToMainPage()
		{
			this.ShowViewModel<MainPageViewModel>();
		}

		public ICommand LogInWithMicrosoft
		{
			get { return new MvxCommand(async () => { await AuthenticateAndGo(AuthenticationProvider.Microsoft); }); }
		}

		public ICommand LogInWithGoogle
		{
			get { return new MvxCommand(async () => { await AuthenticateAndGo(AuthenticationProvider.Google); }); }
		}

		public ICommand LogInWithTwitter
		{
			get { return new MvxCommand(async () => { await AuthenticateAndGo(AuthenticationProvider.Twitter); }); }
		}

		private async Task AuthenticateAndGo(AuthenticationProvider provider)
		{
			var result = await Authenticate(provider);

			if (result.Error != null)
			{
				if (result.Error.HResult != -2146233079) // Cancelled by user
				{
					Mvx.Error(result.Error.ToString());
					//await this.messageBox.ShowAsync("Error authenticating.", "Error");
				}
			}
			else
			{
				GoToMainPage();
			}
		}

		private async Task<AuthenticationResult> Authenticate(AuthenticationProvider provider)
		{
			var result = new AuthenticationResult();

			try
			{
				result.User = await this.mobileService.AuthenticateAsync(provider);
			}
			catch (InvalidOperationException ex)
			{
				Mvx.Error(ex.ToString());
				result.Error = ex;
			}

			return result;
		}

		private class AuthenticationResult
		{
			public User User { get; set; }
			public Exception Error { get; set; }
		}
	}
}
