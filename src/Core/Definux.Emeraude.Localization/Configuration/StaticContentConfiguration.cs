﻿using System;
using Definux.Emeraude.Domain.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Definux.Emeraude.Localization.Configuration
{
    /// <inheritdoc/>
    public class StaticContentConfiguration : IEntityTypeConfiguration<StaticContent>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<StaticContent> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.Content)
                .HasColumnName("content");

            builder
                .Property(x => x.ContentKeyId)
                .HasColumnName("content_key_id");

            builder
                .HasOne(x => x.ContentKey)
                .WithMany(x => x.StaticContentList)
                .HasForeignKey(x => x.ContentKeyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(x => x.LanguageId)
                .HasColumnName("language_id");

            builder
                .HasOne(x => x.Language)
                .WithMany(x => x.StaticContentList)
                .HasForeignKey(x => x.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
