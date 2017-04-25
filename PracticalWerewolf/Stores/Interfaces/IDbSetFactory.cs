using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace PracticalWerewolf.Stores.Interfaces
{
    // http://efpatterns.codeplex.com/SourceControl/changeset/view/7f1a9beddf25#Main/EntityFramework.Patterns/IObjectSetFactory.cs
    // https://github.com/icotting/SETwitter/blob/9bf60ffe26cacee4b44b4a9eae48156bf2145949/ASP.NET/Twitter_Shared/Data/IDbSetFactory.cs
    public interface IDbSetFactory : IDisposable
    {
        DbSet<T> CreateDbSet<T>() where T : class;
        void ChangeObjectState(object entity, EntityState state);
        void RefreshEntity<T>(ref T entity) where T : class;
    }
}