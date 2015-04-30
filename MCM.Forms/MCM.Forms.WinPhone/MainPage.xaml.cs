using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MCM.Forms.WinPhone.Helpers;
using Xamarin.Forms;

namespace MCM.Forms.WinPhone
{
	public partial class MainPage : PhoneApplicationPage
	{
		public MainPage()
		{
			InitializeComponent();
			SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

			global::Xamarin.Forms.Forms.Init();

			var navigationPage = MvxFormsWindowsPhonePagePresenter.NavigationPage;
			Content = navigationPage.ConvertPageToUIElement(this);
		}
	}
}