﻿using Definux.Emeraude.Admin.ClientBuilder.Modules.Vue.Implementations.Constants;
using Definux.Emeraude.Admin.ClientBuilder.Modules.Vue.Implementations.EmPages;
using Definux.Emeraude.Admin.ClientBuilder.Modules.Vue.Implementations.Routes;
using Definux.Emeraude.Admin.ClientBuilder.Modules.Vue.Implementations.StaticContent;
using Definux.Emeraude.Admin.ClientBuilder.Modules.Vue.Implementations.TranslationsResources;
using Definux.Emeraude.Admin.ClientBuilder.Modules.Vue.Implementations.WebApi;
using Definux.Emeraude.Admin.ClientBuilder.Options;

namespace Definux.Emeraude.Admin.ClientBuilder.Modules.Vue.Extensions
{
    /// <summary>
    /// Extensions for <see cref="ClientBuilderOptions"/>.
    /// </summary>
    public static class ClientBuilderOptionsExtensions
    {
        /// <summary>
        /// Add default Vue modules.
        /// </summary>
        /// <param name="options"></param>
        public static void AddDefaultVueModules(this ClientBuilderOptions options)
        {
            options.AddModule<VueWebApiModule>();
            options.AddModule<VueEmPagesModule>();
            options.AddModule<VueRoutesModule>();
            options.AddModule<VueTranslationsResourcesModule>();
            options.AddModule<VueStaticContentModule>();
            options.AddModule<VueConstantsModule>();
        }
    }
}
