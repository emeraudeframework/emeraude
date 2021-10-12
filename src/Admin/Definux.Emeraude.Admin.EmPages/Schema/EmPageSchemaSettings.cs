﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Definux.Emeraude.Admin.EmPages.Data;
using Definux.Emeraude.Admin.EmPages.Schema.DetailsView;
using Definux.Emeraude.Admin.EmPages.Schema.FormView;
using Definux.Emeraude.Admin.EmPages.Schema.TableView;
using Definux.Emeraude.Admin.EmPages.UI.Utilities;
using Definux.Emeraude.Domain.Entities;
using Definux.Emeraude.Essentials.Helpers;

namespace Definux.Emeraude.Admin.EmPages.Schema
{
    /// <summary>
    /// Settings implementation for entity EmPage schema.
    /// </summary>
    /// <typeparam name="TModel">EmPage model.</typeparam>
    public class EmPageSchemaSettings<TModel> : IEmPageSchemaSettings
        where TModel : class, IEmPageModel, new()
    {
        private readonly TableViewConfigurationBuilder<TModel> tableViewConfigurationBuilder;
        private readonly DetailsViewConfigurationBuilder<TModel> detailsViewConfigurationBuilder;
        private readonly FormViewConfigurationBuilder<TModel> formViewConfigurationBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmPageSchemaSettings{TModel}"/> class.
        /// </summary>
        public EmPageSchemaSettings()
        {
            this.tableViewConfigurationBuilder = new TableViewConfigurationBuilder<TModel>();
            this.detailsViewConfigurationBuilder = new DetailsViewConfigurationBuilder<TModel>();
            this.formViewConfigurationBuilder = new FormViewConfigurationBuilder<TModel>();

            this.ModelActions = new List<EmPageAction>();
        }

        /// <summary>
        /// Model route used for identification (normally in in plural format). Example: 'dogs'.
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// Title of the model in plural format.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Defines model actions that can be executed.
        /// </summary>
        /// <returns></returns>
        public IList<EmPageAction> ModelActions { get; }

        /// <summary>
        /// Set defaults of the current schema. It must be executed as a last method of the schema because it is
        /// using the already defined setup.
        /// </summary>
        public void BuildDefaults()
        {
            this.ModelActions.Add(new EmPageAction()
            {
                Name = "Details",
                RelativeUrlFormat = "/{0}",
            });

            this.ModelActions.Add(new EmPageAction()
            {
                Order = 1,
                Name = "Edit",
                RelativeUrlFormat = "/{0}/edit",
            });

            this.ModelActions.Add(new EmPageAction()
            {
                Order = 2,
                Name = "Delete",
                RelativeUrlFormat = "/{0}",
                Method = HttpMethod.Delete,
                ConfirmationMessage = "Are you sure you want to delete this entity?",
            });

            this.tableViewConfigurationBuilder.Breadcrumbs.Add(new EmPageBreadcrumb
            {
                Title = this.Title,
            });

            this.detailsViewConfigurationBuilder.Breadcrumbs.Add(new EmPageBreadcrumb
            {
                Title = this.Title,
                IsActive = true,
                Href = $"/admin/{this.Route}",
            });

            this.detailsViewConfigurationBuilder.Breadcrumbs.Add(new EmPageBreadcrumb
            {
                Title = "Details",
                Order = 1,
                IsActive = false,
            });

            this.formViewConfigurationBuilder.Breadcrumbs.Add(new EmPageBreadcrumb
            {
                Title = this.Title,
                IsActive = true,
                Href = $"/admin/{this.Route}",
            });

            this.formViewConfigurationBuilder.Breadcrumbs.Add(new EmPageBreadcrumb
            {
                Title = EmPagesPlaceholders.FormAction,
                Order = 1,
                IsActive = false,
            });

            if (this.formViewConfigurationBuilder.ViewItems.Any(x => x.Type == FormViewItemType.CreateEdit || x.Type == FormViewItemType.EditOnly))
            {
                this.detailsViewConfigurationBuilder.PageActions.Add(new EmPageAction
                {
                    Order = 1,
                    Name = "Edit",
                    RelativeUrlFormat = "/{0}/edit",
                });
            }
        }

        /// <summary>
        /// Table view configuration builder action.
        /// </summary>
        /// <param name="configurationBuilderAction"></param>
        /// <returns></returns>
        public EmPageSchemaSettings<TModel> ConfigureTableView(
            Action<TableViewConfigurationBuilder<TModel>> configurationBuilderAction)
        {
            configurationBuilderAction.Invoke(this.tableViewConfigurationBuilder);
            return this;
        }

        /// <summary>
        /// Details view configuration builder action.
        /// </summary>
        /// <param name="configurationBuilderAction"></param>
        /// <returns></returns>
        public EmPageSchemaSettings<TModel> ConfigureDetailsView(
            Action<DetailsViewConfigurationBuilder<TModel>> configurationBuilderAction)
        {
            configurationBuilderAction.Invoke(this.detailsViewConfigurationBuilder);
            return this;
        }

        /// <summary>
        /// Form view configuration builder action.
        /// </summary>
        /// <param name="configurationBuilderAction"></param>
        /// <returns></returns>
        public EmPageSchemaSettings<TModel> ConfigureFormView(
            Action<FormViewConfigurationBuilder<TModel>> configurationBuilderAction)
        {
            configurationBuilderAction.Invoke(this.formViewConfigurationBuilder);
            return this;
        }

        /// <inheritdoc/>
        public EmPageSchemaDescription BuildDescription(IEnumerable<Assembly> targetAssemblies)
        {
            var dataManagerType = AssemblyHelpers
                .GetClassesThatImplements<IEmPageDataManager>(targetAssemblies)
                .FirstOrDefault(x =>
                    !x.IsAbstract &&
                    x.BaseType != null &&
                    x.BaseType?.GetGenericArguments().ElementAt(0) == typeof(TModel));

            var description = new EmPageSchemaDescription()
            {
                Route = this.Route,
                Title = this.Title,
                ModelType = typeof(TModel),
                DataManagerType = dataManagerType,
                ModelActions = this.ModelActions,
                TableView = new TableViewDescription
                {
                    ViewItems = this.tableViewConfigurationBuilder.ViewItems,
                    PageActions = this.tableViewConfigurationBuilder.PageActions.OrderBy(x => x.Order).ToList(),
                    Breadcrumbs = this.tableViewConfigurationBuilder.Breadcrumbs.OrderBy(x => x.Order).ToList(),
                },
                DetailsView = new DetailsViewDescription
                {
                    ViewItems = this.detailsViewConfigurationBuilder.ViewItems,
                    PageActions = this.detailsViewConfigurationBuilder.PageActions.OrderBy(x => x.Order).ToList(),
                    Breadcrumbs = this.detailsViewConfigurationBuilder.Breadcrumbs.OrderBy(x => x.Order).ToList(),
                },
                FormView = new FormViewDescription
                {
                    ViewItems = this.formViewConfigurationBuilder.ViewItems,
                    PageActions = this.formViewConfigurationBuilder.PageActions.OrderBy(x => x.Order).ToList(),
                    Breadcrumbs = this.formViewConfigurationBuilder.Breadcrumbs.OrderBy(x => x.Order).ToList(),
                },
            };

            description.FormView.SetModelValidatorAction(this.formViewConfigurationBuilder.ModelValidatorAction);

            return description;
        }
    }
}