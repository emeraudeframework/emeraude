﻿using System;
using Definux.Emeraude.Interfaces.Services;

namespace Definux.Emeraude.Application.Persistence
{
    /// <summary>
    /// Database initializer manager that load and trigger all database initializers for the application.
    /// </summary>
    public interface IDatabaseInitializerManager : IDatabaseInitializer
    {
        /// <summary>
        /// Load database initializers types. Their execution is in the order of their adding.
        /// </summary>
        /// <param name="initializersTypes"></param>
        void LoadDatabaseInitializers(params Type[] initializersTypes);
    }
}
