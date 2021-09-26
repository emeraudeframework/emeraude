﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Definux.Emeraude.Configuration.Extensions;
using Definux.Emeraude.Configuration.Options;
using Definux.Emeraude.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Definux.Emeraude.Admin.EmPages
{
    /// <inheritdoc />
    public class EmPageService : IEmPageService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly EmMainOptions mainOptions;

        private IEnumerable<EmPageSchemaDescription> schemaDescriptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmPageService"/> class.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="optionsProvider"></param>
        public EmPageService(IServiceProvider serviceProvider, IEmOptionsProvider optionsProvider)
        {
            this.serviceProvider = serviceProvider;
            this.mainOptions = optionsProvider.GetMainOptions();
        }

        /// <inheritdoc />
        public async Task<EmPageSchemaDescription> FindSchemaDescriptionAsync(string key)
        {
            await this.LoadSchemasIfEmptyAsync();
            return this.schemaDescriptions?.FirstOrDefault(x => x.Key?.Equals(key, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        /// <inheritdoc/>
        public async Task<EmPageSchemaDescription> FindSchemaDescriptionAsync(Type entityType, Type modelType)
        {
            await this.LoadSchemasIfEmptyAsync();
            return this.schemaDescriptions?.FirstOrDefault(x => x.EntityType == entityType && x.ModelType == modelType);
        }

        /// <inheritdoc/>
        public async Task ApplyValuePipesAsync<TEmPageModel>(IEnumerable<TEmPageModel> models, IEnumerable<IValuePipedViewItem> viewItems)
        {
            var modelType = typeof(TEmPageModel);
            var propertiesValuePipes = this.ExtractPropertiesValuePipes(modelType, viewItems);
            foreach (var propertyValuePipes in propertiesValuePipes)
            {
                var modelProperty = modelType.GetProperty(propertyValuePipes.PropertyName);
                var currentPropertyValues = models.Select(x => modelProperty?.GetValue(x));
                foreach (var (valuePipe, valuePipeParameters) in propertyValuePipes.ValuePipes)
                {
                    await valuePipe.PrepareAsync(currentPropertyValues, valuePipeParameters);
                }

                foreach (var model in models)
                {
                    foreach (var (valuePipe, _) in propertyValuePipes.ValuePipes)
                    {
                        var convertedValue = await valuePipe.ConvertAsync(modelProperty?.GetValue(model));
                        modelProperty.SetValue(model, convertedValue);
                    }
                }
            }
        }

        private async Task LoadSchemasIfEmptyAsync()
        {
            if (this.schemaDescriptions == null || !this.schemaDescriptions.Any())
            {
                this.schemaDescriptions = await this.FindAllSchemasDescriptionsAsync();
            }
        }

        private async Task<IEnumerable<EmPageSchemaDescription>> FindAllSchemasDescriptionsAsync()
        {
            var schemaType = typeof(IEmPageSchema<,>);

            var schemasImplementationsTypes = this.mainOptions.Assemblies
                .SelectMany(x => x
                    .GetExportedTypes()
                    .Where(y => y.IsClass && y.GetInterfaces()
                        .Any(z => z.IsGenericType && z.GetGenericTypeDefinition() == schemaType))
                    .ToList())
                .ToList();

            var foundSchemaDescriptions = new List<EmPageSchemaDescription>();
            var schemaContext = new EmPageSchemaContext();
            var scopedServiceProvider = this.serviceProvider.CreateScope().ServiceProvider;
            foreach (var schemasImplementationType in schemasImplementationsTypes)
            {
                var schema = scopedServiceProvider.GetService(schemasImplementationType);
                var schemaSetupMethod = schemasImplementationType.GetMethod("SetupAsync");
                var schemaSetupResultTask = (Task)schemaSetupMethod?.Invoke(schema, new object[] { schemaContext });
                if (schemaSetupResultTask != null)
                {
                    await schemaSetupResultTask.ConfigureAwait(false);
                    var userDefinedSchemaSettings = (object)((dynamic)schemaSetupResultTask).Result as IEmPageSchemaSettings;
                    var schemaDescription = userDefinedSchemaSettings?.BuildDescription(this.mainOptions.Assemblies);
                    if (schemaDescription != null)
                    {
                        foundSchemaDescriptions.Add(schemaDescription);
                    }
                }
            }

            return foundSchemaDescriptions;
        }

        private IEnumerable<PropertyValuePipes> ExtractPropertiesValuePipes(Type type, IEnumerable<IValuePipedViewItem> valuePipedViewItems)
        {
            if (valuePipedViewItems == null)
            {
                throw new ArgumentNullException(nameof(valuePipedViewItems));
            }

            var valuePipedViewItemsWithRegisteredPipes = valuePipedViewItems.Where(x => x.ValuePipes.Any());

            var result = new List<PropertyValuePipes>();
            var scopedServiceProvider = this.serviceProvider.CreateScope().ServiceProvider;
            foreach (var viewItem in valuePipedViewItemsWithRegisteredPipes)
            {
                var currentPropertyValuePipe = new PropertyValuePipes
                {
                    PropertyName = ((IViewItem)viewItem).SourceName,
                };

                foreach (var (valuePipeType, valuePipeParameters) in viewItem.ValuePipes)
                {
                    var currentValuePipe = scopedServiceProvider.GetService(valuePipeType) as IValuePipe;
                    currentPropertyValuePipe.ValuePipes.Add((currentValuePipe, valuePipeParameters));
                }

                result.Add(currentPropertyValuePipe);
            }

            return result;
        }
    }
}