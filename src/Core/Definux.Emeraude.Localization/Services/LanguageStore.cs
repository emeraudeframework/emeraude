﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Definux.Emeraude.Application.Localization;
using Definux.Emeraude.Application.Logger;
using Definux.Emeraude.Domain.Localization;
using Microsoft.EntityFrameworkCore;

namespace Definux.Emeraude.Localization.Services
{
    /// <summary>
    /// Storage implementation for all localization data - languages, translations.
    /// </summary>
    public class LanguageStore : ILanguageStore
    {
        private readonly ILocalizationContext context;
        private readonly IEmLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageStore"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public LanguageStore(ILocalizationContext context, IEmLogger logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public string[] GetAllLanguageCodes()
        {
            try
            {
                var languages = this.context.Languages.ToList();
                string[] languageCodes = languages.Select(x => x.Code).ToArray();

                return languageCodes;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex);
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<string[]> GetAllLanguageCodesAsync()
        {
            try
            {
                var languages = await this.context.Languages.AsQueryable().ToListAsync();
                string[] languageCodes = languages.Select(x => x.Code).ToArray();

                return languageCodes;
            }
            catch (Exception ex)
            {
                await this.logger.LogErrorAsync(ex);
                return null;
            }
        }

        /// <inheritdoc/>
        public Language GetDefaultLanguage()
        {
            try
            {
                return this.context.Languages.FirstOrDefault(x => x.IsDefault);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex);
                return default;
            }
        }

        /// <inheritdoc/>
        public async Task<Language> GetDefaultLanguageAsync()
        {
            try
            {
                return await this.context.Languages.AsQueryable().FirstOrDefaultAsync(x => x.IsDefault);
            }
            catch (Exception ex)
            {
                await this.logger.LogErrorAsync(ex);
                return default;
            }
        }

        /// <inheritdoc/>
        public List<string> GetTranslationsKeys()
        {
            try
            {
                return this.context
                    .Keys
                    .AsQueryable()
                    .Select(x => x.Key)
                    .ToList();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex);
                return null;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Language> GetLanguages()
        {
            try
            {
                return this.context.Languages.AsQueryable().ToList();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex);
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Language>> GetLanguagesAsync()
        {
            try
            {
                return await this.context.Languages.AsQueryable().ToListAsync();
            }
            catch (Exception ex)
            {
                await this.logger.LogErrorAsync(ex);
                return null;
            }
        }

        /// <inheritdoc/>
        public Dictionary<string, string> GetLanguageTranslationDictionary(int languageId)
        {
            try
            {
                var translations = this.context
                    .Values
                    .AsQueryable()
                    .Where(x => x.LanguageId == languageId)
                    .Include(x => x.TranslationKey)
                    .ToDictionary(k => k.TranslationKey.Key, v => v.Value);

                return translations;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex);
                return null;
            }
        }
    }
}
