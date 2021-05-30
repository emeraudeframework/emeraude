﻿using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Definux.Emeraude.Application.EventHandlers;
using Definux.Emeraude.Application.Identity;
using Definux.Emeraude.Application.Localization;
using Definux.Emeraude.Interfaces.Requests;
using Definux.Utilities.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Definux.Emeraude.Application.Requests.Identity.Commands.ResetPassword
{
    /// <summary>
    /// Command for client reset password of user.
    /// </summary>
    public class ResetPasswordCommand : IRequest<ResetPasswordRequestResult>, IResetPasswordRequest
    {
        /// <inheritdoc/>
        public string Email { get; set; }

        /// <inheritdoc/>
        public string Password { get; set; }

        /// <inheritdoc/>
        public string ConfirmedPassword { get; set; }

        /// <inheritdoc/>
        public string Token { get; set; }

        /// <inheritdoc/>
        public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordRequestResult>
        {
            private readonly IUserManager userManager;
            private readonly IIdentityEventManager identityEventManager;
            private readonly UrlEncoder urlEncoder;
            private readonly IHttpContextAccessor httpContextAccessor;
            private readonly ICurrentLanguageProvider currentLanguageProvider;

            /// <summary>
            /// Initializes a new instance of the <see cref="ResetPasswordCommandHandler"/> class.
            /// </summary>
            /// <param name="userManager"></param>
            /// <param name="identityEventManager"></param>
            /// <param name="urlEncoder"></param>
            /// <param name="httpContextAccessor"></param>
            /// <param name="currentLanguageProvider"></param>
            public ResetPasswordCommandHandler(
                IUserManager userManager,
                IIdentityEventManager identityEventManager,
                UrlEncoder urlEncoder,
                IHttpContextAccessor httpContextAccessor,
                ICurrentLanguageProvider currentLanguageProvider)
            {
                this.userManager = userManager;
                this.identityEventManager = identityEventManager;
                this.urlEncoder = urlEncoder;
                this.httpContextAccessor = httpContextAccessor;
                this.currentLanguageProvider = currentLanguageProvider;
            }

            /// <inheritdoc/>
            public async Task<ResetPasswordRequestResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await this.userManager.FindUserByEmailAsync(request.Email);
                if (user != null)
                {
                    var resetPasswordResult = await this.userManager.ResetPasswordAsync(user, request.Token, request.Password);
                    if (resetPasswordResult.Succeeded)
                    {
                        var currentLanguage = await this.currentLanguageProvider.GetCurrentLanguageAsync();
                        string languageUrlPrefix = currentLanguage.IsDefault ? string.Empty : $"/{currentLanguage.Code.ToLower()}";
                        string passwordResetToken = this.urlEncoder.Encode(await this.userManager.GeneratePasswordResetTokenAsync(user));
                        string resetPasswordLink = this.httpContextAccessor.HttpContext.GetAbsoluteRoute($"{languageUrlPrefix}/reset-password?token={passwordResetToken}&email={user.Email}");
                        await this.identityEventManager.TriggerForgotPasswordEventAsync(user.Id, resetPasswordLink);

                        return new ResetPasswordRequestResult(true);
                    }
                }

                return new ResetPasswordRequestResult(false);
            }
        }
    }
}
