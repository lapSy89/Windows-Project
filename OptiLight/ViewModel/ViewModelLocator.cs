using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using OptiLight.View;

namespace OptiLight.ViewModel {
    public class ViewModelLocator {

        public ViewModelLocator() {
            var container = new UnityContainer();
            var locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);

            if (ViewModelBase.IsInDesignModeStatic) {
            } else {
                container.RegisterType<DialogViews>();
            }

            container.RegisterType<MainViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    }
}