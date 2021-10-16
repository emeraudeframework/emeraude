﻿using System;
using System.Linq.Expressions;
using Definux.Emeraude.Admin.EmPages.Schema;
using Definux.Emeraude.Domain.Entities;

namespace Definux.Emeraude.Admin.EmPages.Data.Requests.EmPageDataFetch
{
    /// <inheritdoc cref="IEmPageDataFetchQuery{TEntity,TRequestModel}"/>
    public class EmPageDataFetchQuery<TEntity, TModel> : IEmPageDataFetchQuery<TEntity, TModel>
        where TEntity : class, IEntity, new()
        where TModel : class, IEmPageModel, new()
    {
        /// <inheritdoc/>
        public int Page { get; set; }

        /// <summary>
        /// Page size for the pagination. Default value is 25.
        /// </summary>
        public int PageSize { get; set; } = 25;

        /// <inheritdoc/>
        public string SearchQuery { get; set; }

        /// <inheritdoc/>
        public string OrderBy { get; set; }

        /// <inheritdoc/>
        public string OrderType { get; set; }

        /// <inheritdoc/>
        public Expression<Func<TEntity, bool>> FilterExpression { get; set; }
    }
}
