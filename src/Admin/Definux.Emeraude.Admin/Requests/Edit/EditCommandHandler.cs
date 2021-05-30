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

namespace Definux.Emeraude.Admin.Requests.Edit
{
    /// <inheritdoc cref="IEditCommandHandler{TEditCommand, TEntity, TRequestModel}"/>
    public class EditCommandHandler<TEntity, TRequestModel> : IEditCommandHandler<EditCommand<TEntity, TRequestModel>, TEntity, TRequestModel>
        where TEntity : class, IEntity, new()
        where TRequestModel : class, new()
    {
        private readonly IEmContext context;
        private readonly IMapper mapper;
        private readonly IEmLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler{TEntity, TRequestModel}"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public EditCommandHandler(IEmContext context, IMapper mapper, IEmLogger logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Guid?> Handle(EditCommand<TEntity, TRequestModel> request, CancellationToken cancellationToken)
        {
            try
            {
                var dbSet = this.context.Set<TEntity>();
                var requestExpression = request.ParentExpression ?? (x => true);
                var currentEntity = dbSet
                    .Where(ExpressionFunctions.AndAlso(requestExpression, x => x.Id == request.EntityId).Compile())
                    .FirstOrDefault();

                if (currentEntity != null)
                {
                    this.mapper.Map(request.Model, currentEntity);

                    currentEntity.Id = request.EntityId;
                    dbSet.Update(currentEntity);
                    await this.context.SaveChangesAsync(cancellationToken);

                    return request.EntityId;
                }

                return null;
            }
            catch (Exception ex)
            {
                await this.logger.LogErrorAsync(ex, nameof(EditCommandHandler<TEntity, TRequestModel>));
                return null;
            }
        }
    }
}
