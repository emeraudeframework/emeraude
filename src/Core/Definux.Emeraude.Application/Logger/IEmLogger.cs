﻿using System.Threading.Tasks;
using Definux.Emeraude.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Definux.Emeraude.Application.Logger
{
    /// <summary>
    /// Emeraude default logger.
    /// </summary>
    public interface IEmLogger : ILogger
    {
        /// <summary>
        /// Register information about current request without saving.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="hideParameters"></param>
        void LogActivity(ActionExecutingContext context, bool hideParameters = false);

        /// <summary>
        /// Save information about sent/unsent email.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="receiver"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="sent"></param>
        /// <returns></returns>
        Task LogEmailAsync(string emailAddress, string receiver, string subject, string body, bool sent);
    }
}
