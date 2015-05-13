using Cirrious.MvvmCross.ViewModels;
using System.Windows.Input;

namespace MCM.Forms.ViewModels
{
	public class MainPageViewModel : BaseViewModel
	{
		public ICommand MyChildren
		{
			get { return new MvxCommand(() => this.ShowViewModel<MyChildrenPageViewModel>()); }
		}

		public ICommand HomeDna
		{
			get { return new MvxCommand(() => this.ShowViewModel<HomeDnaPageViewModel>()); }
		}

		public ICommand SafetyForChildren
		{
			get { return new MvxCommand(() => this.ShowViewModel<SafetyForChildrenPageViewModel>()); }
		}

		public ICommand MissingChild
		{
			get { return new MvxCommand(() => this.ShowViewModel<MissingChildPageViewModel>()); }
		}

		public ICommand AboutMcm
		{
			get { return new MvxCommand(() => this.ShowViewModel<AboutMcmPageViewModel>()); }
		}
	}
}
