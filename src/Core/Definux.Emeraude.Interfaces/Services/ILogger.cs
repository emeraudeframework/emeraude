﻿using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Definux.Emeraude.Interfaces.Services
{
    /// <summary>
    /// Definition of logger.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Save information (async execution) for thrown exception.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        Task LogErrorAsync(Exception exception, [CallerMemberName]string method = "");

        /// <summary>
        /// Save information for thrown exception.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="method"></param>
        void LogError(Exception exception, [CallerMemberName]string method = "");

        /// <summary>
        /// Save information for specific error without existing exception.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="method"></param>
        void LogErrorWithoutAnException(string source, string message, [CallerMemberName]string method = "");
    }
}
