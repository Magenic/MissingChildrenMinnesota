using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.MobileServices;

namespace MCM.Service.Tests.Integration
{
    [TestClass]
    public class LoginTests
    {
        private MobileServiceClient mobileService;
        [TestInitialize]
        public void TestInitialize()
        {
            mobileService = new MobileServiceClient("https://missingchildrenminnesota-dev.azure-mobile.net/", "jYPiRcGCAmwwqFbmGWhAVNdldYdnpo37");
        }
        
        [TestMethod]
        public async void MicrosoftLogin()
        {

            //var user = await this.mobileService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount, null);
            //Assert.IsNotNull(user);
        }
    }
}
