using Inker.Services;
using Inker.Utilities;
using Inker.ViewModels;

namespace Inker
{
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;

    public class AppBootstrapper : BootstrapperBase
    {
        private SimpleContainer container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            container = new SimpleContainer();
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<BrushSettingsService>();
            container.Singleton<GridSettingsService>();
            container.PerRequest<CanvasViewModel>();
            container.PerRequest<IShell, ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            ThemeManagerHelper.CreateAppStyleBy(Colorization.Current, true);
            DisplayRootViewFor<IShell>();
            Colorization.ColorizationChanged += (o, color) => ThemeManagerHelper.CreateAppStyleBy(Colorization.Current, true);
        }
    }
}