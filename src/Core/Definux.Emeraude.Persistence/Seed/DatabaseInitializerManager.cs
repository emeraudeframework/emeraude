﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Definux.Emeraude.Application.Persistence;
using Definux.Emeraude.Interfaces.Services;

namespace Definux.Emeraude.Persistence.Seed
{
    /// <inheritdoc cref="IDatabaseInitializerManager"/>
    public class DatabaseInitializerManager : IDatabaseInitializerManager
    {
        private readonly IServiceProvider serviceProvider;
        private List<IDatabaseInitializer> databaseInitializers;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseInitializerManager"/> class.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public DatabaseInitializerManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.databaseInitializers = new List<IDatabaseInitializer>();
        }

        /// <inheritdoc/>
        public void LoadDatabaseInitializers(params Type[] initializersTypes)
        {
            foreach (var initializerType in initializersTypes)
            {
                this.databaseInitializers.Add(this.serviceProvider.GetService(initializerType) as IDatabaseInitializer);
            }
        }

        /// <inheritdoc/>
        public async Task SeedAsync()
        {
            if (this.databaseInitializers == null || this.databaseInitializers.Count == 0)
            {
                throw new InvalidOperationException("To seed data into database please load database initializers from the Emeraude options.");
            }

            foreach (var initializer in this.databaseInitializers)
            {
                await initializer.SeedAsync();
            }
        }
    }
}
