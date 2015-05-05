using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;

namespace MCM.Forms.ViewModels
{
	public class LoginPageViewModel : BaseViewModel
	{
		public void GoToMainPage()
		{
			this.ShowViewModel<MainPageViewModel>();
		}

		public ICommand LogInWithMicrosoft
		{
			// TODO: Implement auth
			get { return new MvxCommand(this.GoToMainPage); }
		}

		public ICommand LogInWithGoogle
		{
			// TODO: Implement auth
			get { return new MvxCommand(this.GoToMainPage); }
		}

		public ICommand LogInWithTwitter
		{
			// TODO: Implement auth
			get { return new MvxCommand(this.GoToMainPage); }
		}
	}
}
