﻿using System;
using System.Threading.Tasks;
using Definux.Emeraude.Application.Exceptions;
using Definux.Emeraude.Application.Requests.Identity.Commands.RefreshAccessToken;
using Definux.Emeraude.Presentation.Controllers;
using Definux.Emeraude.Presentation.Extensions;
using Definux.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Definux.Emeraude.Client.Controllers.Api
{
    /// <inheritdoc/>
    public sealed partial class ClientAuthenticationApiController : EmApiController
    {
        /// <summary>
        /// Refresh JWT token action.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshAccessToken([FromBody]RefreshAccessTokenCommand request)
        {
            try
            {
                request.UserId = this.HttpContext.GetJwtUserId();
                var requestResult = await this.Mediator.Send(request);

                if (requestResult.Succeeded)
                {
                    return this.Ok(requestResult);
                }
            }
            catch (ValidationException ex)
            {
                this.ModelState.ApplyValidationException(ex);
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, Strings.YourRequestCannotBeExecuted);
            }

            return this.BadRequestWithModelErrors();
        }
    }
}
