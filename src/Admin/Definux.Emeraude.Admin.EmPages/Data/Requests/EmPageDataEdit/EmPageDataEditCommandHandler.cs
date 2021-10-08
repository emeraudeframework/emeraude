﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Definux.Emeraude.Admin.EmPages.Schema;
using Definux.Emeraude.Application.Logger;
using Definux.Emeraude.Application.Persistence;
using Definux.Emeraude.Domain.Entities;
using Definux.Utilities.Functions;
using Microsoft.EntityFrameworkCore;

namespace Definux.Emeraude.Admin.EmPages.Data.Requests.EmPageDataEdit
{
    /// <inheritdoc cref="IEmPageDataEditCommandHandler{TEditCommand,TEntity,TModel}"/>
    public class EmPageDataEditCommandHandler<TEntity, TModel> : IEmPageDataEditCommandHandler<EmPageDataEditCommand<TEntity, TModel>, TEntity, TModel>
        where TEntity : class, IEntity, new()
        where TModel : class, IEmPageModel, new()
    {
        private readonly IEmContext context;
        private readonly IMapper mapper;
        private readonly IEmLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmPageDataEditCommandHandler{TEntity,TModel}"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public EmPageDataEditCommandHandler(IEmContext context, IMapper mapper, IEmLogger logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<Guid?> Handle(EmPageDataEditCommand<TEntity, TModel> request, CancellationToken cancellationToken)
        {
            try
            {
                var dbSet = this.context.Set<TEntity>();
                var currentEntity = await dbSet.FirstOrDefaultAsync(x => x.Id == request.EntityId, cancellationToken);

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
                await this.logger.LogErrorAsync(ex, nameof(EmPageDataEditCommandHandler<TEntity, TModel>));
                return null;
            }
        }
    }
}
