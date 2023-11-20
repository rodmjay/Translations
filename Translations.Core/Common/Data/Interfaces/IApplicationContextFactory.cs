#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Microsoft.EntityFrameworkCore.Design;
using Translations.Core.Data;

namespace Translations.Core.Common.Data.Interfaces;

public interface IApplicationContextFactory : IDesignTimeDbContextFactory<TranslationContext>
{
}