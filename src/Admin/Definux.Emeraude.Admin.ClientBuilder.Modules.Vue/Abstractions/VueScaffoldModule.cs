﻿using System;
using Definux.Emeraude.Admin.ClientBuilder.ScaffoldModules;
using Definux.Emeraude.Admin.ClientBuilder.Shared;

namespace Definux.Emeraude.Admin.ClientBuilder.Modules.Vue.Abstractions
{
    /// <summary>
    /// Abstract Vue module for generation of files supporting Vue applications.
    /// </summary>
    public abstract class VueScaffoldModule : ScaffoldModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VueScaffoldModule"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="locked"></param>
        public VueScaffoldModule(string name, bool locked)
            : base(name, InstanceType.WebModule, locked)
        {
            this.Icon = Convert.ToBase64String(Resources.vuejs);
            this.ScaffoldTypeName = "Vue";
            this.ParentModuleId = "vue.scaffold.module";
        }
    }
}
