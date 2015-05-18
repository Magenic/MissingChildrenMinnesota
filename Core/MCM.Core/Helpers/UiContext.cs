
namespace MCM.Core.Helpers
{
	public class UiContext : IUiContext
	{
#if ANDROID
		private static Android.App.Activity currentContext;

		public Android.App.Activity CurrentContext
		{
			get { return currentContext; }
			set { currentContext = value; }
		}
#elif IOS
		private static UIKit.UIViewController currentContext;

		public UIKit.UIViewController CurrentContext
		{
			get { return currentContext; }
			set { currentContext = value; }
		}
#endif
	}
}
