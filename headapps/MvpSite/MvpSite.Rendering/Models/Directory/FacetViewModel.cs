﻿using MvpSite.Rendering.ViewComponents;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Directory;

public class FacetViewModel
{
    public required string Identifier { get; set; }

    public TextField? Name { get; set; }

    public List<FacetOption> FacetOptions { get; set; } = [];

    public int SortOrder { get; set; }

    public string ToQueryString()
    {
        string result = FacetOptions
            .Where(fo => fo.IsActive)
            .Aggregate(string.Empty, (current, option) => current + $"{DirectoryViewComponent.FacetQuerystringPrefix}{Identifier}={option.Identifier}&");

        return !string.IsNullOrWhiteSpace(result) ? result[..^1] : string.Empty;
    }
}