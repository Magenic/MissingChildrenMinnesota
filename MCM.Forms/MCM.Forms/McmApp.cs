using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using MCM.Core.Helpers;
using MCM.Core.Services;
using MCM.Forms.Helpers;

namespace MCM.Forms
{
    public class McmApp : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

			CreatableTypes()
				.EndingWith("ViewModel")
				.AsTypes()
				.RegisterAsDynamic();

			Mvx.RegisterSingleton<IUiContext>(new UiContext());
			Mvx.RegisterType(typeof(IConfiguration), typeof(Configuration));
			Mvx.RegisterType(typeof(IMobileService), typeof(MobileService));
				
            RegisterAppStart(new MvxAppStart());
        }
    }
}