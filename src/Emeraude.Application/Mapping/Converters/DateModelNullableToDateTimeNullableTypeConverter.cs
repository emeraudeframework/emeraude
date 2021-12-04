﻿using System;
using AutoMapper;
using Emeraude.Application.Models;

namespace Emeraude.Application.Mapping.Converters;

/// <inheritdoc />
public class DateModelNullableToDateTimeNullableTypeConverter : ITypeConverter<DateModel?, DateTime?>
{
    /// <inheritdoc />
    public DateTime? Convert(DateModel? source, DateTime? destination, ResolutionContext context)
        => source?.ToDateTime();
}