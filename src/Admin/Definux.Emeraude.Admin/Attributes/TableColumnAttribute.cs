﻿using System;
using System.Runtime.CompilerServices;
using Definux.Emeraude.Admin.UI.UIElements.Table;
using Definux.Utilities.Functions;

namespace Definux.Emeraude.Admin.Attributes
{
    /// <summary>
    /// Attribute used for rendering the table of the get all action of admin entity controller.
    /// </summary>
    public class TableColumnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumnAttribute"/> class.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="name"></param>
        /// <param name="uiElementType"></param>
        public TableColumnAttribute(int order, string name, Type uiElementType)
        {
            this.Order = order;
            this.Name = name;
            this.UIElementType = uiElementType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumnAttribute"/> class.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="uiElementType"></param>
        /// <param name="propertyName"></param>
        public TableColumnAttribute(int order, Type uiElementType, [CallerMemberName] string propertyName = "")
        {
            this.Order = order;
            this.Name = StringFunctions.SplitWordsByCapitalLetters(propertyName);
            this.UIElementType = uiElementType;
        }

        /// <summary>
        /// Order of the column.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Name of the column.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// UI element type that implemented <see cref="ITableElement"/>.
        /// </summary>
        public Type UIElementType { get; }
    }
}