using Microsoft.WindowsAzure.MobileServices;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace MCM.Service.TestHarness
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MobileServiceClient mobileService;

        public MainWindow()
        {
            InitializeComponent();
            mobileService = new MobileServiceClient("https://missingchildrenminnesota-dev.azure-mobile.net/", "Put Key Here");

        }

        private async void btnMicrosoft_Click(object sender, RoutedEventArgs e)
        {
            var token = new JObject();
            var user = await this.mobileService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount, token);

        }

        private async void btnChild_Click(object sender, RoutedEventArgs e)
        {
            var children = await mobileService.GetTable<Child>().Take(100).ToListAsync();
            MessageBox.Show(children.ToString());
        }

        private async void btnChildVitals_Click(object sender, RoutedEventArgs e)
        {
            var childVitals = await mobileService.GetTable<ChildVitals>().Take(100).ToListAsync();
            MessageBox.Show(childVitals.ToString());
        }

        private async void btnGoogle_Click(object sender, RoutedEventArgs e)
        {
            var token = new JObject();
            var user = await this.mobileService.LoginAsync(MobileServiceAuthenticationProvider.Google, token);
            MessageBox.Show(user.ToString());
        }
    }
}