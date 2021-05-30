﻿using System.Collections.Generic;
using Definux.Emeraude.Admin.ClientBuilder.Models;
using Definux.Emeraude.Presentation.Attributes;

namespace Definux.Emeraude.Admin.ClientBuilder.Services
{
    /// <summary>
    /// Client builder service that scan and process assembly information about marked API endpoints into the application.
    /// </summary>
    public interface IEndpointService
    {
        /// <summary>
        /// Get list of all decorated (<see cref="EndpointAttribute"/>) endpoint actions from decorated controllers (<seealso cref="ApiEndpointsControllerAttribute"/>).
        /// </summary>
        /// <returns></returns>
        List<Endpoint> GetAllEndpoints();

        /// <summary>
        /// Get list of all used classes into the decorated endpoints (<see cref="EndpointAttribute"/>, <seealso cref="ApiEndpointsControllerAttribute"/>).
        /// </summary>
        /// <returns></returns>
        List<TypeDescription> GetAllEndpointsClasses();
    }
}
