﻿using Definux.Emeraude.Admin.EmPages.Schema;
using Definux.Emeraude.Domain.Entities;
using Definux.Emeraude.Essentials.Models;
using MediatR;

namespace Definux.Emeraude.Admin.EmPages.Data.Requests.EmPageDataFetch
{
    /// <summary>
    /// Interface that wraps get all request handler.
    /// </summary>
    /// <typeparam name="TFetchQuery">Fetch query.</typeparam>
    /// <typeparam name="TEntity">Target entity.</typeparam>
    /// <typeparam name="TModel">EmPage model.</typeparam>
    public interface IEmPageDataFetchQueryHandler<in TFetchQuery, TEntity, TModel> : IRequestHandler<TFetchQuery, PaginatedList<TModel>>
        where TEntity : class, IEntity, new()
        where TModel : class, IEmPageModel, new()
        where TFetchQuery : IEmPageDataFetchQuery<TEntity, TModel>
    {
    }
}
