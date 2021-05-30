﻿using System;
using System.Collections.Generic;
using Definux.Emeraude.Domain.Entities;

namespace Definux.Emeraude.Admin.UI.Adapters
{
    /// <summary>
    /// Emeraude context adapter that provide access of unavailable services to administration UI assembly.
    /// </summary>
    public interface IEmContextAdapter
    {
        /// <summary>
        /// Get all entities from the database based on the type of the desired entity.
        /// </summary>
        /// <param name="entityType">Type of the desired entity.</param>
        /// <returns></returns>
        IEnumerable<IEntity> GetAllEntitiesByType(Type entityType);

        /// <summary>
        /// Get all entities from the database based on the property name.
        /// </summary>
        /// <param name="name">Name of the database context property.</param>
        /// <returns></returns>
        IEnumerable<IEntity> GetAllEntitiesByPropertyName(string name);
    }
}
