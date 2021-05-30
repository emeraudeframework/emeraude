﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Definux.Emeraude.Application.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Definux.Emeraude.Admin.ClientBuilder.Requests.Queries.GetLanguages
{
    /// <summary>
    /// Query that returns all languages.
    /// </summary>
    public class GetLanguagesQuery : IRequest<IEnumerable<LanguageResult>>
    {
        /// <inheritdoc/>
        public class GetLanguagesQueryHandler : IRequestHandler<GetLanguagesQuery, IEnumerable<LanguageResult>>
        {
            private readonly ILocalizationContext context;
            private readonly IMapper mapper;

            /// <summary>
            /// Initializes a new instance of the <see cref="GetLanguagesQueryHandler"/> class.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="mapper"></param>
            public GetLanguagesQueryHandler(ILocalizationContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            /// <inheritdoc/>
            public async Task<IEnumerable<LanguageResult>> Handle(GetLanguagesQuery request, CancellationToken cancellationToken)
            {
                return await this.context
                    .Languages
                    .AsQueryable()
                    .ProjectTo<LanguageResult>(this.mapper.ConfigurationProvider)
                    .ToListAsync();
            }
        }
    }
}
