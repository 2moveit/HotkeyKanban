using System;
using System.Collections.Generic;
using System.Windows.Input;
using Caliburn.Micro;
using KCT.HotkeyKanban.UI.ViewModels;

namespace KCT.HotkeyKanban.UI {
    public class AppBootstrapper : BootstrapperBase {
        SimpleContainer container;

        public AppBootstrapper() {
            Initialize(); 
        }

        protected override void Configure() {
            container = new SimpleContainer();

            container.Singleton<IWindowManager, AppWindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.PerRequest<IShell, ShellViewModel>();
            MessageBinder.SpecialValues.Add("$pressedkey", (context) =>
            {
                //http://stackoverflow.com/questions/16719496/caliburn-micro-enter-key-event
                // NOTE: IMPORTANT - you MUST add the dictionary key as lowercase as CM
                // does a ToLower on the param string you add in the action message, in fact ideally
                // all your param messages should be lowercase just in case. I don't really like this
                // behaviour but that's how it is!
                var keyArgs = context.EventArgs as KeyEventArgs;

                if (keyArgs != null)
                    return keyArgs.Key;

                return null;
            });
        }

        protected override object GetInstance(Type service, string key) {
            var instance = container.GetInstance(service, key);
            if (instance != null)
                return instance;

            throw new InvalidOperationException("Could not locate any instances.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service) {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance) {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e) {
            DisplayRootViewFor<IShell>();
        }
    }
}