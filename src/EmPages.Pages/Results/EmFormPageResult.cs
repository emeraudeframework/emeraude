﻿namespace EmPages.Pages.Results;

/// <summary>
/// Form page result.
/// </summary>
public class EmFormPageResult : IEmFormPageResult
{
    /// <inheritdoc/>
    public IEmPageModel Model { get; set; }
}