﻿using System;
using System.Threading.Tasks;
using Definux.Emeraude.Application.Exceptions;
using Definux.Emeraude.Application.Requests.Identity.Commands.Login;
using Definux.Emeraude.Locales.Attributes;
using Definux.Emeraude.Localization.Extensions;
using Definux.Emeraude.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Definux.Emeraude.Client.Controllers.Mvc
{
    /// <inheritdoc/>
    public sealed partial class ClientAuthenticationController : ClientController
    {
        private const string LoginRoute = "/login";

        /// <summary>
        /// Login action for GET request.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(LoginRoute)]
        [LanguageRoute(LoginRoute)]
        public IActionResult Login(string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToDefault();
            }

            string returnUrlLanguageCode = returnUrl.GetLanguageCodeFromUrl();
            var request = new LoginCommand();
            this.ViewData["ReturnUrl"] = returnUrl;
            if (!string.IsNullOrEmpty(returnUrlLanguageCode) && string.IsNullOrEmpty(this.HttpContext.GetLanguageCode()))
            {
                return this.LocalRedirect($"/{returnUrlLanguageCode}{LoginRoute}?returnUrl={this.urlEncoder.Encode(returnUrl)}");
            }
            else
            {
                return this.LoginView(request);
            }
        }

        /// <summary>
        /// Login action for GET request.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(LoginRoute)]
        [LanguageRoute(LoginRoute)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginCommand request, string returnUrl = "")
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToDefault();
            }

            try
            {
                var requestResult = await this.Mediator.Send(request);

                if (requestResult.Result.Succeeded)
                {
                    await this.SignInAsync(requestResult.User);
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return this.RedirectToDefault();
                    }

                    return this.LocalRedirect(returnUrl);
                }
                else if (requestResult.Result.IsLockedOut)
                {
                    return this.View("Lockout");
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, Strings.TheEmailAddressOrPasswordIsIncorrect);
                }
            }
            catch (ValidationException ex)
            {
                this.ModelState.ApplyValidationException(ex);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, Strings.YourLoginAttemptHasFailed);
            }

            this.ViewData["ReturnUrl"] = returnUrl;

            return this.LoginView(request);
        }

        private ViewResult LoginView(object model)
        {
            return this.View("Login", model);
        }
    }
}
