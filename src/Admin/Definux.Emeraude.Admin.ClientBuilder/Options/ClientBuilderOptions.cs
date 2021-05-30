﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Definux.Emeraude.Admin.ClientBuilder.ScaffoldModules;

namespace Definux.Emeraude.Admin.ClientBuilder.Options
{
    /// <summary>
    /// Options for client builder.
    /// </summary>
    public class ClientBuilderOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBuilderOptions"/> class.
        /// </summary>
        public ClientBuilderOptions()
        {
            this.Assemblies = new List<Assembly>();
            this.ModulesTypes = new List<Type>();
            this.ConstantsTypes = new List<Type>();
        }

        /// <summary>
        /// Assemblies which will be scanned for the purposes of client builder.
        /// </summary>
        public List<Assembly> Assemblies { get; internal set; }

        /// <summary>
        /// List of all scaffold modules used for code generation from the client builder.
        /// </summary>
        public List<Type> ModulesTypes { get; internal set; }

        /// <summary>
        /// List of all classes types which will be scanned for constants.
        /// </summary>
        public List<Type> ConstantsTypes { get; private set; }

        /// <summary>
        /// Path of the web application used of the client builder.
        /// </summary>
        public string WebAppPath { get; set; }

        /// <summary>
        /// Path of the mobile application used of the client builder.
        /// </summary>
        public string MobileAppPath { get; set; }

        /// <summary>
        /// Method that set the web application path into the options.
        /// </summary>
        /// <param name="paths"></param>
        public void SetWebAppPath(params string[] paths)
        {
            this.WebAppPath = Path.Combine(paths);
        }

        /// <summary>
        /// Method that set the mobile application path into the options.
        /// </summary>
        /// <param name="paths"></param>
        public void SetMobileAppPath(params string[] paths)
        {
            this.MobileAppPath = Path.Combine(paths);
        }

        /// <summary>
        /// Add assembly which will be scan for the purposes of Emeraude Client Builder.
        /// </summary>
        /// <param name="assemblyString"></param>
        public void AddAssembly(string assemblyString)
        {
            this.Assemblies.Add(Assembly.Load(assemblyString));
        }

        /// <summary>
        /// Add module to Emeraude Client Builder.
        /// </summary>
        /// <typeparam name="TModule">Type of the module.</typeparam>
        public void AddModule<TModule>()
            where TModule : ScaffoldModule
        {
            this.ModulesTypes.Add(typeof(TModule));
        }
    }
}
