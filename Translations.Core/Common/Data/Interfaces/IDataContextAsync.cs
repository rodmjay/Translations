#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

namespace Translations.Core.Common.Data.Interfaces;

public interface IDataContextAsync : IDataContext
{
    //Task BeginTransactionAsync(DbIsolationLevel isolationLevel);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<int> SaveChangesAsync();
    Task SyncObjectsStatePostCommitAsync();
    Task<int> ExecuteSqlAsync(string query, params object[] parameters);
}