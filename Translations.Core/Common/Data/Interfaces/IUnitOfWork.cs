#region Header Info

// Copyright 2023 Rod Johnson.  All rights reserved

#endregion

using Translations.Core.Common.Data.Enums;

namespace Translations.Core.Common.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
    event EventHandler OnSaveChanges;
    int SaveChanges();
    void Dispose(bool disposing);
    IRepository<TEntity> Repository<TEntity>() where TEntity : class, IObjectState;
    void BeginTransaction(DbIsolationLevel isolationLevel = DbIsolationLevel.Unspecified);
    bool Commit();
    void Rollback();
}