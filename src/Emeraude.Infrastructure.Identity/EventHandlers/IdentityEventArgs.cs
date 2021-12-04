﻿using System;
using Microsoft.AspNetCore.Http;

namespace Emeraude.Infrastructure.Identity.EventHandlers;

/// <summary>
/// Base class that implements the identity event arguments.
/// </summary>
public abstract class IdentityEventArgs
{
    /// <summary>
    /// User id.
    /// </summary>
    public Guid UserId { get; set; }

    /// <inheritdoc cref="HttpContext"/>
    public HttpContext HttpContext { get; set; }
}