﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Definux.Emeraude.Client.EmPages.Models;
using Definux.Emeraude.Presentation.Controllers;
using Definux.Seo.Attributes;
using Definux.Seo.Extensions;
using Definux.Seo.Models;
using Definux.Seo.Options;
using Definux.Utilities.Functions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Definux.Emeraude.Client.EmPages.Abstractions
{
    /// <summary>
    /// EmPage parent class that initialize a ASP.NET Core MVC page related with Vue.js page with custom predefined initial state.
    /// </summary>
    /// <typeparam name="TViewModel">Type of the returning data transfer object (ViewModel).</typeparam>
    /// <typeparam name="TRequest">Request that compute and return the ViewModel.</typeparam>
    [MetaTag(MainMetaTags.Title, EmPagesConstants.PageMetaTagTitleKey, true)]
    [MetaTag(MainMetaTags.Description, EmPagesConstants.PageMetaTagDescriptionKey, true)]
    [Canonical]
    public abstract class EmPage<TViewModel, TRequest> : PublicController, IEmPage
        where TViewModel : class, IEmViewModel, new()
        where TRequest : class, IRequest<TViewModel>, new()
    {
        /// <summary>
        /// Path and name to the view that will be used for the current EmPage.
        /// </summary>
        public string EmPageViewName { get; protected set; } = "EmPages/Index";

        /// <inheritdoc cref="IInitialStateModel{TViewModel}"/>
        public InitialStateModel<TViewModel> InitialStateModel { get; protected set; }

        /// <summary>
        /// Data transfer object for pass strong-typed data to the initial state of the page.
        /// </summary>
        public TViewModel ViewModel { get; protected set; }

        /// <summary>
        /// Request that compute and return the <see cref="ViewModel"/>.
        /// </summary>
        public TRequest InitialStateRequest { get; protected set; }

        /// <summary>
        /// GET request that returns the <see cref="EmPageView(InitialStateModel{TViewModel})"/> with the page initial state.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<IActionResult> Index()
        {
            var model = await this.BuildInitialStateModel();
            return this.EmPageView(model);
        }

        /// <summary>
        /// POST request for the page initial state without HTML rendering.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<IActionResult> RequestInitialState()
        {
            var model = await this.BuildInitialStateModel();
            return this.Ok(model);
        }

        /// <summary>
        /// Method that contains the implementation of creation of initial state model. It is usefull if you want to pass some routing data to the page ViewModel request.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<InitialStateModel<TViewModel>> BuildInitialStateModel()
        {
            return await this.CreateInitialStateModelAsync(new TRequest());
        }

        /// <summary>
        /// Method that create the initial state model - the customized data + all generic data. Overriding is not recommended.
        /// </summary>
        /// <param name="dataQuery"></param>
        /// <returns></returns>
        protected virtual async Task<InitialStateModel<TViewModel>> CreateInitialStateModelAsync(IRequest<TViewModel> dataQuery)
        {
            var model = new InitialStateModel<TViewModel>(this.GetType().Name);

            var currentLanguage = await this.CurrentLanguageProvider.GetCurrentLanguageAsync();
            var currentUser = await this.CurrentUserProvider.GetCurrentUserAsync();

            model.User.IsAuthenticated = this.User.Identity.IsAuthenticated;
            model.LanguageCode = currentLanguage?.Code;
            model.LanguageId = currentLanguage?.Id ?? -1;

            if (model.User.IsAuthenticated && currentUser != null)
            {
                model.User.Roles = this.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray();
                model.User.Email = currentUser.Email;
                model.User.Name = currentUser.Name;
            }

            model.ViewModel = dataQuery is null ? null : await this.Mediator.Send(dataQuery);

            var viewDataItems = await this.InitializeViewDataAsync(model);
            if (viewDataItems != null && viewDataItems.Count > 0)
            {
                foreach (var (key, value) in viewDataItems)
                {
                    model.AddViewDataItem(key, value);
                }
            }

            model.MetaTags = await this.InitializeMetaTagsAsync(model);

            return model;
        }

        /// <summary>
        /// Method that returns the action result of the EmPage. To work properly the existence of Views/Client/Shared/EmPages/Index.cshtml is a must.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [NonAction]
        protected IActionResult EmPageView(InitialStateModel<TViewModel> model)
        {
            return this.View(this.EmPageViewName, model);
        }

        /// <summary>
        /// This method initializes the key-value pair used as view data for the initial state after the page view model is computed but before the page sending action result to the view.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual async Task<Dictionary<string, object>> InitializeViewDataAsync(InitialStateModel<TViewModel> model)
        {
            return new Dictionary<string, object>();
        }

        /// <summary>
        /// This method initialize the meta tags properties definition for current initial state of the current page.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="disableDefaultDecoratedTags">Disable default decorated meta tags for the parent controller.</param>
        /// <returns></returns>
        protected virtual async Task<MetaTagsModel> InitializeMetaTagsAsync(InitialStateModel<TViewModel> model, bool disableDefaultDecoratedTags = false)
        {
            try
            {
                var seoOptionsAccessor = (IOptions<DefinuxSeoOptions>)this.HttpContext.RequestServices.GetService(typeof(IOptions<DefinuxSeoOptions>));
                var seoOptions = seoOptionsAccessor?.Value;
                var metaTagsModel = this.ViewData.GetMetaTagsModelOrDefault();
                if (metaTagsModel == null)
                {
                    metaTagsModel = new MetaTagsModel();
                }

                metaTagsModel.ApplyStaticTags(seoOptions.DefaultMetaTags);
                metaTagsModel.OpenGraphImage.Value = seoOptions.DefaultMetaTags.OpenGraphImage.Value;
                metaTagsModel.TwitterImage.Value = seoOptions.DefaultMetaTags.TwitterImage.Value;

                if (!disableDefaultDecoratedTags)
                {
                    string pageKey = StringFunctions.ConvertToKey(this.GetType().Name);
                    this.AddTranslatedValueIntoViewData(EmPagesConstants.PageMetaTagTitleKey, $"{pageKey}_META_TITLE");
                    this.AddTranslatedValueIntoViewData(EmPagesConstants.PageMetaTagDescriptionKey, $"{pageKey}_META_DESCRIPTION");
                }

                return metaTagsModel;
            }
            catch (Exception ex)
            {
                await this.Logger.LogErrorAsync(ex);
                return new MetaTagsModel();
            }
        }
    }
}
