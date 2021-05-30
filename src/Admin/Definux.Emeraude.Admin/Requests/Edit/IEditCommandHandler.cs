﻿using System;
using Definux.Emeraude.Domain.Entities;
using MediatR;

namespace Definux.Emeraude.Admin.Requests.Edit
{
    /// <inheritdoc/>
    public interface IEditCommandHandler<TEditCommand, TEntity, TRequestModel> : IRequestHandler<TEditCommand, Guid?>
        where TEntity : class, IEntity, new()
        where TRequestModel : class, new()
        where TEditCommand : IEditCommand<TEntity, TRequestModel>
    {
    }
}
