﻿namespace EmPages.Pages.Results;

/// <summary>
/// Details page result.
/// </summary>
public class EmDetailsPageResult : IEmDetailsPageResult
{
    /// <inheritdoc/>
    public IEmPageModel Model { get; set; }
}