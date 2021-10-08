﻿using System;
using Definux.Emeraude.Admin.EmPages.Schema;
using Definux.Emeraude.Domain.Entities;

namespace Definux.Emeraude.Admin.EmPages.Data.Requests.EmPageDataCreate
{
    /// <inheritdoc cref="IEmPageDataCreateCommand{TEntity,TModel}"/>
    public class EmPageDataCreateCommand<TEntity, TModel> : IEmPageDataCreateCommand<TEntity, TModel>
        where TEntity : class, IEntity, new()
        where TModel : class, IEmPageModel, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmPageDataCreateCommand{TEntity,TModel}"/> class.
        /// </summary>
        /// <param name="model"></param>
        public EmPageDataCreateCommand(TModel model)
        {
            this.Model = model;
        }

        /// <inheritdoc/>
        public TModel Model { get; set; }
    }
}
