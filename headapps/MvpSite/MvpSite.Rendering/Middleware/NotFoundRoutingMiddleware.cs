﻿using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using MvpSite.Rendering.AppSettings;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Exceptions;
using Sitecore.AspNetCore.SDK.LayoutService.Client.Interfaces;
using Sitecore.AspNetCore.SDK.RenderingEngine;
using Sitecore.AspNetCore.SDK.RenderingEngine.Configuration;
using Sitecore.AspNetCore.SDK.RenderingEngine.Extensions;
using Sitecore.AspNetCore.SDK.RenderingEngine.Interfaces;
using Sitecore.AspNetCore.SDK.RenderingEngine.Middleware;

namespace MvpSite.Rendering.Middleware;

public class NotFoundRoutingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<NotFoundRoutingMiddleware> _logger;

    private readonly MvpSiteSettings? _settings;

    private readonly RenderingEngineMiddleware _renderingEngine;

    public NotFoundRoutingMiddleware(
        RequestDelegate next,
        IConfiguration configuration,
        ILogger<NotFoundRoutingMiddleware> logger,
        ISitecoreLayoutRequestMapper requestMapper,
        ISitecoreLayoutClient layoutService,
        IOptions<RenderingEngineOptions> options)
    {
        _next = next;
        _settings = configuration.GetSection(MvpSiteSettings.Key).Get<MvpSiteSettings>();
        _logger = logger;
        _renderingEngine = new RenderingEngineMiddleware(_next, requestMapper, layoutService, options);
    }

    public async Task InvokeAsync(HttpContext context, IViewComponentHelper viewComponentHelper, IHtmlHelper htmlHelper)
    {
        ISitecoreRenderingContext? sitecoreContext = context.GetSitecoreRenderingContext();
        if (sitecoreContext != null && (sitecoreContext.Response?.HasErrors ?? false))
        {
            ItemNotFoundSitecoreLayoutServiceClientException? notFound = sitecoreContext.Response.Errors
                .OfType<ItemNotFoundSitecoreLayoutServiceClientException>().FirstOrDefault();
            if (notFound != null)
            {
                // [IVA] Keep track of not found pages in info logs
                _logger.LogInformation(notFound, notFound.Message);

                // [IVA] Change the request faking it towards the 404 page
                context.Request.Path = _settings?.NotFoundPage;

                // [IVA] Force the response to use 404 status code
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                // [IVA] Bust the RenderingEngine execution cache
                context.Items.Remove(nameof(RenderingEngineMiddleware));
                context.Features.Set<ISitecoreRenderingContext>(null);
                if (context.Request.RouteValues.ContainsKey(RenderingEngineConstants.RouteValues.SitecoreRoute))
                {
                    context.Request.RouteValues.Remove(RenderingEngineConstants.RouteValues.SitecoreRoute);
                }

                // [IVA] Finally we re-run the RenderingEngine
                await _renderingEngine.Invoke(context, viewComponentHelper, htmlHelper);
            }
            else
            {
                await _next(context);
            }
        }
        else
        {
            await _next(context);
        }
    }
}