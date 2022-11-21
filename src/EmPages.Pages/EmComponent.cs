﻿using System;
using System.Text.Json.Serialization;
using Essentials.Extensions;

namespace EmPages.Pages;

/// <summary>
/// Abstract implementation of component.
/// </summary>
public abstract class EmComponent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmComponent"/> class.
    /// </summary>
    /// <param name="type"></param>
    protected EmComponent(ComponentType type)
    {
        this.Type = type;
    }

    /// <summary>
    /// Name of the component in the UI provider source.
    /// </summary>
    public string SourceName => this.GetType().Name;

    /// <summary>
    /// Type of the component.
    /// </summary>
    public ComponentType Type { get; }

    /// <summary>
    /// Type of the source.
    /// </summary>
    public Type SourceType { get; set; }

    /// <summary>
    /// Full name of the source type.
    /// </summary>
    public string SourceTypeName => this.SourceType?.GetTypeByIgnoreTheNullable()?.FullName;

    /// <summary>
    /// Flag that indicates whether the source type is nullable or not.
    /// </summary>
    public bool IsNullable => Nullable.GetUnderlyingType(this.SourceType) != null;

    /// <summary>
    /// Gets parameters object based on the component.
    /// </summary>
    /// <returns></returns>
    public virtual object GetParametersObject() => new { };

    /// <summary>
    /// Validates setup of component. In order to provide a custom implementation for the component
    /// take into account that there is no base implementation of that validation method.
    /// </summary>
    public virtual void ValidateSetup()
    {
    }
}