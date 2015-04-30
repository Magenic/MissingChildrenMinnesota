using Cirrious.MvvmCross.ViewModels;
using MCM.Forms.ViewModels;

namespace MCM.Forms.Helpers
{
	public sealed class MvxAppStart
		: MvxNavigatingObject, IMvxAppStart
	{
		public void Start(object hint = null)
		{
			ShowViewModel<LoginPageViewModel>();
		}
	}
}
