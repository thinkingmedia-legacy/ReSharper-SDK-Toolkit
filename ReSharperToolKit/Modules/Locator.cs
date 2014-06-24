using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using JetBrains.Util;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Modules.Factories;
using ReSharperToolKit.Modules.Factories.Impl;
using ReSharperToolKit.Modules.Services;
using ReSharperToolKit.Modules.Services.Impl;

namespace ReSharperToolKit.Modules
{
    /// <summary>
    /// A very basic service locator.
    /// </summary>
    public class Locator
    {
        /// <summary>
        /// Singleton
        /// </summary>
        private static Locator _instance;

        /// <summary>
        /// Storage of all the service objects.
        /// </summary>
        private readonly Dictionary<Type, object> _services;

        /// <summary>
        /// Global access
        /// </summary>
        [NotNull]
        private static Locator Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = new Locator();
                Configure();
                return _instance;
            }
        }

        /// <summary>
        /// Configures the locator
        /// </summary>
        private static void Configure()
        {
            Put<iNamingService>(new StandardNamingPattern());
            Put<iTreeNodeService>(new TreeNodeService());
            Put<iProjectService>(new ProjectService());
            Put<iElementEditorFactory>(new ElementEditorFactory());

            // will execute all static methods that have the LocatorConfig attribute assigned to it.
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                IEnumerable<Type> types;

                try
                {
                    types = from type in assembly.GetTypes()
                            where type.IsClass && type.IsPublic && type.IsVisible
                            select type;
                }
                catch (ReflectionTypeLoadException)
                {
                    // some assemblies might not load, so ignore them.
                    continue;
                }

                IEnumerable<MethodInfo> configs = from t in types
                                                  from m in t.GetMethods()
                                                  where !m.IsConstructor
                                                        && m.IsPublic
                                                        && m.IsStatic
                                                        && m.ReturnType == typeof (void)
                                                        && m.GetCustomAttributes(false)
                                                            .ToList()
                                                            .Any(pObj=>pObj is LocatorConfigAttribute)
                                                  select m;

                configs.ForEach(pConfig=>pConfig.Invoke(null, null));
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private Locator()
        {
            _services = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Gets a service from the container.
        /// </summary>
        [NotNull]
        public static T Get<T>() where T : class
        {
            if (!Instance._services.ContainsKey(typeof (T)))
            {
                throw new ServiceNotFoundException(typeof (T));
            }
            return (T)Instance._services[typeof (T)];
        }

        /// <summary>
        /// Adds an object as a service. Will fail is service already exists for that type.
        /// </summary>
        public static void Put<T>([NotNull] T pService) where T : class
        {
            if (pService == null)
            {
                throw new ArgumentNullException("pService");
            }

            Instance._services.Add(typeof (T), pService);
        }
    }
}