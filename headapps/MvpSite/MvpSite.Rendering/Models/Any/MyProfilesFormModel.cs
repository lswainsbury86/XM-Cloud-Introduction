﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Mvp.Selections.Domain;
using MvpSite.Rendering.Attributes;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Any;

public class MyProfilesFormModel : BaseModel
{
    public TextField? TitleLabel { get; set; }

    [FromForm(Name = $"{nameof(MyProfilesFormModel)}.{nameof(IsEdit)}")]
    public bool IsEdit { get; set; }

    public TextField? NameLabel { get; set; }

    [Required]
    [FromForm(Name = $"{nameof(MyProfilesFormModel)}.{nameof(Name)}")]
    public string? Name { get; set; }

    public TextField? LinkLabel { get; set; }

    [Required]
    [HttpsUrl]
    [FromForm(Name = $"{nameof(MyProfilesFormModel)}.{nameof(Link)}")]
    public Uri? Link { get; set; }

    public TextField? TypeLabel { get; set; }

    [Required]
    [FromForm(Name = $"{nameof(MyProfilesFormModel)}.{nameof(Type)}")]
    public ProfileLinkType Type { get; set; }

    public TextField? SubmitLabel { get; set; }

    public List<ProfileLink> Links { get; init; } = [];

    public Guid? RemoveProfileLinkId { get; set; }

    public ProfileLink? RemoveProfileLink { get; set; }

    public TextField? ConfirmMessageLabelFormat { get; set; }

    public TextField? ConfirmLabel { get; set; }

    public bool RemoveConfirmed { get; set; }
}