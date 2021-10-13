﻿using System;
using System.Collections.Generic;
using Definux.Emeraude.Admin.UI.Models;

namespace Definux.Emeraude.Admin.EmPages.UI.Models
{
    /// <summary>
    /// Abstract class that defines the general entities view schema implementation.
    /// </summary>
    public abstract class EmPageViewSchema
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmPageViewSchema"/> class.
        /// </summary>
        /// <param name="context"></param>
        public EmPageViewSchema(EmPageViewContext context)
        {
            this.PropertyComponentPair = new Dictionary<string, Type>();
            this.PropertyTypePair = new Dictionary<string, Type>();
            this.PropertyParametersPair = new Dictionary<string, IDictionary<string, object>>();
            this.Context = context;
        }

        /// <inheritdoc cref="EmPageViewContext"/>
        public EmPageViewContext Context { get; set; }

        /// <summary>
        /// Component type for over the main.
        /// </summary>
        public Type ComponentTypeForOverTheMain { get; set; }

        /// <summary>
        /// Component type for below the main.
        /// </summary>
        public Type ComponentTypeForBelowTheMain { get; set; }

        /// <summary>
        /// Pair that contains corresponding component for each property of the model.
        /// </summary>
        public IDictionary<string, Type> PropertyComponentPair { get; set; }

        /// <summary>
        /// Pair that contains corresponding type for each property of the model.
        /// </summary>
        public IDictionary<string, Type> PropertyTypePair { get; set; }

        /// <summary>
        /// Pair that contains corresponding parameters for each property of the model.
        /// </summary>
        public IDictionary<string, IDictionary<string, object>> PropertyParametersPair { get; set; }
    }
}