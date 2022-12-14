using InfoSecurity1.BusinessLogic;
using InfoSecurity1.BusinessLogic.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Lifetime;

namespace InfoSecurity1
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer currentContainer = BuildUnityContainer();

            var authWindow = currentContainer.Resolve<AuthWindow>();
            authWindow.Show();
        }


        private static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<UserLogic>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<CryptoHelper>(new HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}
