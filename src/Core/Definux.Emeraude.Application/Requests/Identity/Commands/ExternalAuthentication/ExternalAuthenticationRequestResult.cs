﻿using Definux.Emeraude.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Definux.Emeraude.Application.Requests.Identity.Commands.ExternalAuthentication
{
    /// <summary>
    /// Result of external authentication request.
    /// </summary>
    public class ExternalAuthenticationRequestResult
    {
        /// <summary>
        /// Result of the authentication.
        /// </summary>
        public SignInResult Result { get; set; }

        /// <summary>
        /// Authenticated user.
        /// </summary>
        public IUser User { get; set; }
    }
}
