﻿using Sitecore.AspNetCore.SDK.LayoutService.Client.Response.Model.Fields;

namespace MvpSite.Rendering.Models.Admin;

public class ApplicationCommentModel : BaseModel
{
    public TextField? TitleLabel { get; set; }

    public TextField? CommentLabel { get; set; }

    public TextField? ConfirmLabel { get; set; }

    public Guid ApplicationId { get; set; }

    public string? Comment { get; set; }
}