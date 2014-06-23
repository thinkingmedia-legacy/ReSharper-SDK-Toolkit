﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
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
                _instance.Configure();
                return _instance;
            }
        }

        /// <summary>
        /// Configures the locator
        /// </summary>
        private void Configure()
        {
            Put<iNamingService>(new StandardNamingPattern());
            Put<iAppTheme>(new AppTheme());
            Put<iTreeNodeService>(new TreeNodeService());
            Put<iTestProjectService>(new TestProjectService(Get<iNamingService>()));
            Put<iProjectService>(new ProjectService());

            Put<iUnitTestService>(new UnitTestService(
                Get<iTestProjectService>(),
                Get<iProjectService>(),
                Get<iNamingService>()));

            Put<iElementEditorFactory>(new ElementEditorFactory());
        }

        /// <summary>
        /// Adds an object as a service. Will fail is service already exists for that type.
        /// </summary>
        private void Put<T>([NotNull] T pService) where T : class
        {
            if (pService == null)
            {
                throw new ArgumentNullException("pService");
            }

            _services.Add(typeof (T), pService);
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
    }
}