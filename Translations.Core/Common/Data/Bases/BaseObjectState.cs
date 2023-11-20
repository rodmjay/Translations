#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Translations.Core.Common.Data.Enums;
using Translations.Core.Common.Data.Interfaces;

namespace Translations.Core.Common.Data.Bases;

public abstract class BaseObjectState : IObjectState
{
    [NotMapped][IgnoreDataMember] public ObjectState ObjectState { get; set; }
}