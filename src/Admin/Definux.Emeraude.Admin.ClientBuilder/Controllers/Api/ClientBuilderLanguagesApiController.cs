﻿using System.Threading.Tasks;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Commands.CreateContentKeyWithContent;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Commands.CreateKeyWithValues;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Commands.CreateLanguage;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Commands.DeleteContentKey;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Commands.DeleteKey;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Commands.DeleteLanguage;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Commands.EditContentKeyWithContent;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Commands.EditTranslation;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Commands.EditTranslationKey;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Commands.MakeLanguageDefault;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Queries.GetLanguages;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Queries.GetStaticContentKey;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Queries.GetStaticContentKeys;
using Definux.Emeraude.Admin.ClientBuilder.Requests.Queries.GetTranslationsGridData;
using Definux.Emeraude.Admin.ClientBuilder.Shared;
using Definux.Emeraude.Application.Exceptions;
using Definux.Emeraude.Configuration.Authorization;
using Definux.Emeraude.Presentation.Controllers;
using Definux.Utilities.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Definux.Emeraude.Admin.ClientBuilder.Controllers.Api
{
    /// <summary>
    /// Client builder controller that manages the languages, translations and static content items of the application.
    /// </summary>
    [Route("/api/client-builder/languages/")]
    [Authorize(AuthenticationSchemes = AuthenticationDefaults.AdminAuthenticationScheme)]
    public sealed class ClientBuilderLanguagesApiController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBuilderLanguagesApiController"/> class.
        /// </summary>
        /// <param name="hostEnvironment"></param>
        public ClientBuilderLanguagesApiController(IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                throw new DevelopmentOnlyException(ClientBuilderMessages.ProtectedControllerExceptionMessage);
            }
        }

        /// <summary>
        /// Get list of all supported languages.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetLanguages()
        {
            return this.Ok(await this.Mediator.Send(new GetLanguagesQuery()));
        }

        /// <summary>
        /// Get all language keys and their translations into grid format.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("grid-data")]
        public async Task<IActionResult> GetTranslationsGridData()
        {
            return this.Ok(await this.Mediator.Send(new GetTranslationGridDataQuery()));
        }

        /// <summary>
        /// Gets all static content keys.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("content-keys")]
        public async Task<IActionResult> GetContentKeys()
        {
            return this.Ok(await this.Mediator.Send(new GetStaticContentKeysQuery()));
        }

        /// <summary>
        /// Gets a specified static content key.
        /// </summary>
        /// <param name="keyId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("content-keys/{keyId}")]
        public async Task<IActionResult> GetContentKeys(int keyId)
        {
            return this.Ok(await this.Mediator.Send(new GetStaticContentKeyQuery { KeyId = keyId }));
        }

        /// <summary>
        /// Creates a translation key with values.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("keys")]
        public async Task<IActionResult> CreateKeyWithValues([FromBody]NewKeyWithValuesRequest request)
        {
            return this.Ok(await this.Mediator.Send(new CreateKeyWithValuesCommand(request)));
        }

        /// <summary>
        /// Creates a static content key with values.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("content-keys")]
        public async Task<IActionResult> CreateContentKeyWithValues([FromBody]NewContentKeyWithContentRequest request)
        {
            return this.Ok(await this.Mediator.Send(new CreateContentKeyWithContentCommand(request)));
        }

        /// <summary>
        /// Edits a translation key.
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("keys/{keyId}/edit")]
        public async Task<IActionResult> EditKey(int keyId, [FromBody]SingleValueObject<string> newValue)
        {
            return this.Ok(await this.Mediator.Send(new EditTranslationKeyCommand
            {
                KeyId = keyId,
                NewValue = newValue.Value,
            }));
        }

        /// <summary>
        /// Edits a static content key.
        /// </summary>
        /// <param name="keyId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("content-keys/{keyId}")]
        public async Task<IActionResult> EditContentKey(int keyId, [FromBody]ContentKeyWithContentRequest request)
        {
            return this.Ok(await this.Mediator.Send(new EditContentKeyWithContentCommand(keyId, request)));
        }

        /// <summary>
        /// Edits a translation value.
        /// </summary>
        /// <param name="translationId"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("translations/{translationId}/edit")]
        public async Task<IActionResult> EditTranslation(int translationId, [FromBody]SingleValueObject<string> newValue)
        {
            return this.Ok(await this.Mediator.Send(new EditTranslationCommand
            {
                TranslationId = translationId,
                NewValue = newValue.Value,
            }));
        }

        /// <summary>
        /// Deletes a translation key.
        /// </summary>
        /// <param name="keyId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("keys/{keyId}")]
        public async Task<IActionResult> DeleteKey(int keyId)
        {
            return this.Ok(await this.Mediator.Send(new DeleteKeyCommand
            {
                KeyId = keyId,
            }));
        }

        /// <summary>
        /// Deletes a static content key.
        /// </summary>
        /// <param name="keyId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("content-keys/{keyId}")]
        public async Task<IActionResult> DeleteContentKey(int keyId)
        {
            return this.Ok(await this.Mediator.Send(new DeleteContentKeyCommand
            {
                KeyId = keyId,
            }));
        }

        /// <summary>
        /// Makes a language default.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("default")]
        public async Task<IActionResult> MakeLanguageDefault([FromBody]SingleValueObject<int> request)
        {
            return this.Ok(await this.Mediator.Send(new MakeLanguageDefaultCommand
            {
                LanguageId = request.Value,
            }));
        }

        /// <summary>
        /// Creates a language.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateLanguage([FromBody]CreateLanguageRequest request)
        {
            return this.Ok(await this.Mediator.Send(new CreateLanguageCommand(request)));
        }

        /// <summary>
        /// Deletes a language.
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{languageId}")]
        public async Task<IActionResult> DeleteLanguage(int languageId)
        {
            return this.Ok(await this.Mediator.Send(new DeleteLanguageCommand
            {
                LanguageId = languageId,
            }));
        }
    }
}
