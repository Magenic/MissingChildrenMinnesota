using Xamarin.Forms;

namespace MCM.Forms.Helpers
{
	// Borrowed from: https://github.com/Cheesebaron/Xam.Forms.Mvx/blob/master/Movies/Movies.Android/MvxDroidAdaptation/IMvxPageNavigationHost.cs
	public interface IMvxPageNavigationProvider
	{
		void Push(Page page);
		void Pop();
	}
}