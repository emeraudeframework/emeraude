﻿using System;
using System.Threading.Tasks;
using Definux.Emeraude.Application.Exceptions;
using Definux.Emeraude.Application.Requests.Identity.Commands.ForgotPassword;
using Definux.Emeraude.Presentation.Controllers;
using Definux.Emeraude.Presentation.Extensions;
using Definux.Emeraude.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Definux.Emeraude.Client.Controllers.Api
{
    /// <inheritdoc/>
    public sealed partial class ClientAuthenticationApiController : EmApiController
    {
        /// <summary>
        /// Forgot password action.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordCommand request)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.BadRequest();
            }

            try
            {
                var requestResult = await this.Mediator.Send(request);

                if (requestResult.Succeeded)
                {
                    return this.Ok();
                }
                else
                {
                    await this.Logger.LogErrorAsync(new ArgumentException($"Invalid email ({request.Email}) from reset password form."));
                    return this.Ok();
                }
            }
            catch (ValidationException ex)
            {
                this.ModelState.ApplyValidationException(ex);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, Messages.YourRequestCannotBeExecuted);
            }

            return this.BadRequestWithModelErrors();
        }
    }
}
