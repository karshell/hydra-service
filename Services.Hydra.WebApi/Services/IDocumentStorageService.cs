using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Services.Hydra.WebApi.Models;

namespace Services.Hydra.WebApi.Services
{
    public interface IDocumentStorageService
    {
        Task<IEnumerable<TEntity>> GetManyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : Entity, new();

        Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : Entity, new();

        Task UpdateAsync(Entity document);

        Task InsertAsync<TEntity>(TEntity obj);
    }
}
