﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Definux.Emeraude.Application.Identity;
using Definux.Emeraude.Application.Logger;
using Definux.Utilities.Objects;
using MediatR;
using Newtonsoft.Json;

namespace Definux.Emeraude.Application.Requests.Identity.Commands.RemoveExternalLoginProvider
{
    /// <summary>
    /// Command for remove external login provider from specified user.
    /// </summary>
    public class RemoveExternalLoginProviderCommand : IRequest<SimpleResult>
    {
        /// <summary>
        /// Id of the user.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// External login provider.
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Password of the specified user.
        /// </summary>
        public string Password { get; set; }

        /// <inheritdoc/>
        public class ResetTwoFactorAuthenticationCommandHandler : IRequestHandler<RemoveExternalLoginProviderCommand, SimpleResult>
        {
            private readonly IUserManager userManager;
            private readonly IEmLogger logger;

            /// <summary>
            /// Initializes a new instance of the <see cref="ResetTwoFactorAuthenticationCommandHandler"/> class.
            /// </summary>
            /// <param name="userManager"></param>
            /// <param name="logger"></param>
            public ResetTwoFactorAuthenticationCommandHandler(IUserManager userManager, IEmLogger logger)
            {
                this.userManager = userManager;
                this.logger = logger;
            }

            /// <inheritdoc/>
            public async Task<SimpleResult> Handle(RemoveExternalLoginProviderCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await this.userManager.FindUserByIdAsync(request.UserId ?? Guid.Empty);
                    if (user != null && await this.userManager.CheckPasswordAsync(user, request.Password))
                    {
                        var currentProviders = await this.userManager.GetLoginsAsync(user);
                        var currentProvider = currentProviders.FirstOrDefault(x => x.LoginProvider == request.Provider);
                        if (currentProvider != null)
                        {
                            await this.userManager.RemoveLoginAsync(user, currentProvider.LoginProvider, currentProvider.ProviderKey);
                            return SimpleResult.SuccessfulResult;
                        }
                    }

                    return SimpleResult.UnsuccessfulResult;
                }
                catch (Exception ex)
                {
                    await this.logger.LogErrorAsync(ex);
                    return SimpleResult.UnsuccessfulResult;
                }
            }
        }
    }
}