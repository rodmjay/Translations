#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Translations.Core.Common.Settings;

namespace Translations.Common.Bases;

[ApiController]
[Produces("application/json")]
[Route("v1.0/[controller]")]
public class BaseController : ControllerBase
{
    protected readonly AppSettings AppSettings;
    protected readonly IDistributedCache Cache;

    /// <param name="serviceProvider"></param>
    protected BaseController(IServiceProvider serviceProvider)
    {
        Cache = serviceProvider.GetRequiredService<IDistributedCache>();
        AppSettings = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
    }
    
}