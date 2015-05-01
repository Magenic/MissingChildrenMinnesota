using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
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
				
            RegisterAppStart(new MvxAppStart());
        }
    }
}