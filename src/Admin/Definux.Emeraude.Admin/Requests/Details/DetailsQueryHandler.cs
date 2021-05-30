﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Definux.Emeraude.Admin.Utilities;
using Definux.Emeraude.Application.Logger;
using Definux.Emeraude.Application.Persistence;
using Definux.Emeraude.Domain.Entities;
using Definux.Utilities.Functions;

namespace Definux.Emeraude.Admin.Requests.Details
{
    /// <inheritdoc cref="IDetailsQueryHandler{TDetailsQuery, TEntity, TRequestModel}"/>
    public class DetailsQueryHandler<TEntity, TRequestModel> : IDetailsQueryHandler<DetailsQuery<TEntity, TRequestModel>, TEntity, TRequestModel>
        where TEntity : class, IEntity, new()
        where TRequestModel : class, new()
    {
        private readonly IEmContext context;
        private readonly IMapper mapper;
        private readonly IEmLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DetailsQueryHandler{TEntity, TRequestModel}"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public DetailsQueryHandler(IEmContext context, IMapper mapper, IEmLogger logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<TRequestModel> Handle(DetailsQuery<TEntity, TRequestModel> request, CancellationToken cancellationToken)
        {
            try
            {
                var requestExpression = request.ParentExpression ?? (x => true);
                var entity = this.context
                    .Set<TEntity>()
                    .Where(ExpressionFunctions.AndAlso(requestExpression, x => x.Id == request.EntityId).Compile())
                    .FirstOrDefault();

                var requestModel = this.mapper.Map<TRequestModel>(entity);

                return requestModel;
            }
            catch (Exception ex)
            {
                await this.logger.LogErrorAsync(ex, nameof(DetailsQueryHandler<TEntity, TRequestModel>));
                return null;
            }
        }
    }
}
