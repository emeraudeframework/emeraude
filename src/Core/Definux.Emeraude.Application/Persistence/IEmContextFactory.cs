﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Definux.Emeraude.Application.Persistence
{
    /// <summary>
    /// Main database context factory of Emeraude application.
    /// </summary>
    public interface IEmContextFactory
    {
        /// <summary>
        /// Creates instance of database context.
        /// </summary>
        /// <returns></returns>
        DbContext CreateDbContext();

        /// <summary>
        /// Creates instance of database context.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DbContext> CreateDbContextAsync(CancellationToken cancellationToken = default);
    }
}