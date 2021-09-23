﻿using System;
using System.Collections.Generic;

namespace Definux.Emeraude.Admin.UI.Models
{
    /// <summary>
    /// Abstract class that defines the general entities view schema implementation.
    /// </summary>
    public abstract class EntityViewSchema
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityViewSchema"/> class.
        /// </summary>
        /// <param name="context"></param>
        public EntityViewSchema(EntityViewSchemaContext context)
        {
            this.NavbarActions = new List<ActionModel>();
            this.Context = context;
        }

        /// <inheritdoc cref="EntityViewSchemaContext"/>
        public EntityViewSchemaContext Context { get; set; }

        /// <summary>
        /// Actions placed in the navbar.
        /// </summary>
        public IList<ActionModel> NavbarActions { get; }

        /// <summary>
        /// Breadcrumbs list.
        /// </summary>
        public IList<BreadcrumbItemModel> Breadcrumbs { get; set; }

        /// <summary>
        /// Component type for over the main.
        /// </summary>
        public Type ComponentTypeForOverTheMain { get; set; }

        /// <summary>
        /// Component type for below the main.
        /// </summary>
        public Type ComponentTypeForBelowTheMain { get; set; }
    }
}