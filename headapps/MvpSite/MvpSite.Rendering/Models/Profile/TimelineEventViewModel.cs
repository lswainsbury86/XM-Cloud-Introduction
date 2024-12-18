﻿using Mvp.Selections.Domain;

namespace MvpSite.Rendering.Models.Profile;

public class TimelineEventViewModel
{
    public DateTime Date { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public Uri? Uri { get; set; }

    public ContributionType? ContributionType { get; set; }

    public List<string> RelatedProducts { get; init; } = [];
}