using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using Autofac;
using Lianyun.UST.Infrastructure.Dependency;
using Autofac.Configuration;

namespace Lianyun.UST.Infrastructure.Core
{
    public static class Engine
    {
        private static readonly IContainer _container;
        private static readonly IEngine _engine;

        [MethodImpl(MethodImplOptions.Synchronized)]
        static Engine()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new ConfigurationSettingsReader("autofac"));

            builder.Register(o => Engine._container).As<IContainer>().SingleInstance();

            _container = builder.Build();

            var registersBuilder = new ContainerBuilder();

            InitializeRegistration(registersBuilder);

            registersBuilder.Update(_container);

            _engine = _container.Resolve<IEngine>();

            InitializeStartUpTasks();
        }

        public static IEngine Current
        {
            get
            {
                return _engine;
            }
        }

        private static void InitializeRegistration(ContainerBuilder builder)
        {
            var registers = new List<IDependencyRegistration>();

            using (var scope = _container.BeginLifetimeScope())
            {
                var typeFinder = scope.Resolve<ITypeFinder>();

                var registersTypes = typeFinder.FindClassesOfType<IDependencyRegistration>();

                foreach (var type in registersTypes)
                {
                    var register = (IDependencyRegistration)Activator.CreateInstance(type);

                    registers.Add(register);
                }
            }

            foreach (var register in registers.OrderBy(o => o.Order))
            {
                register.Regist(builder);

                var disposable = register as IDisposable;

                if (disposable != null)
                    disposable.Dispose();
            }
        }

        private static void InitializeStartUpTasks()
        {
            var startUpTasks = new List<IStartUpTask>();

            using (var scope = _container.BeginLifetimeScope())
            {
                var typeFinder = scope.Resolve<ITypeFinder>();

                var startUpTasksTypes = typeFinder.FindClassesOfType<IStartUpTask>();

                foreach (var type in startUpTasksTypes)
                {
                    var task = (IStartUpTask)Activator.CreateInstance(type);

                    startUpTasks.Add(task);
                }
            }

            foreach (var task in startUpTasks.OrderBy(o => o.Order))
            {
                task.OnStartUp();

                var disposable = task as IDisposable;

                if (disposable != null)
                    disposable.Dispose();
            }
        }
    }
}
