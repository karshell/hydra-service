using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json.Linq;
using Services.Hydra.WebApi.Configuration;
using Services.Hydra.WebApi.Models;

namespace Services.Hydra.WebApi.Services
{
    public class DocumentStorageService : IDocumentStorageService
    {
        private readonly DocumentClient _client;
        private readonly DataStorageOptions _dataStorageOptions;
        private readonly Uri _databaseUri;
        private readonly Uri _collectionUri;
        private readonly string _collectionId;
        private readonly PartitionKeyDefinition _partitionKeyDefinition;

        public DocumentStorageService(DataStorageOptions options, string collectionId)
        {
            _dataStorageOptions = options;
            _databaseUri = UriFactory.CreateDatabaseUri(options.DatabaseId);
            _collectionUri = UriFactory.CreateDocumentCollectionUri(options.DatabaseId, collectionId);
            _collectionId = collectionId;
            _client = new DocumentClient(new Uri(options.EndpointAddress), options.AuthorizationKey, ConnectionPolicy.Default);

            EnsureDatabaseExists();
            EnsureCollectionExists(out _partitionKeyDefinition);
        }

        public async Task<IEnumerable<TEntity>> GetManyAsync<TEntity>(Expression<Func<TEntity, bool>> predicate = null) where TEntity : Entity, new()
        {
            var feedOptions = new FeedOptions { EnableCrossPartitionQuery = true };

            try
            {
                IQueryable<TEntity> queryable = _client.CreateDocumentQuery<TEntity>(_collectionUri, feedOptions)
                    .Where(s => s.TypeName == new TEntity().TypeName);

                if (predicate != null)
                {
                    queryable = queryable.Where(predicate);
                }

                using (var documentQuery = queryable.AsDocumentQuery())
                {
                    var results = new List<TEntity>();
                    while (documentQuery.HasMoreResults)
                    {
                        var response = await documentQuery.ExecuteNextAsync<TEntity>();
                        results.AddRange(response);
                    }

                    return results;
                }
            }
            catch (DocumentClientException e)
            {
                Console.Write($"Unable to read details for document. {e.StatusCode}: {e.Message}");
                throw;
            }
        }

        public async Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : Entity, new()
        {
            IEnumerable<TEntity> documents = await GetManyAsync(predicate);

            try
            {
                foreach (var document in documents)
                {
                    object partitionKey = GetPartitionKey(document);

                    var documentUri = UriFactory.CreateDocumentUri(_dataStorageOptions.DatabaseId, _collectionId, document.Id);
                    await _client.DeleteDocumentAsync(documentUri, new RequestOptions { PartitionKey = new PartitionKey(partitionKey) });
                }
            }
            catch (DocumentClientException e)
            {
                Console.Write($"Unable to delete document. {e.StatusCode}: {e.Message}");
                throw;
            }
            catch (Exception e)
            {
                Console.Write($"Unable to delete document: {e}");
                throw;
            }
        }

        public async Task UpdateAsync(Entity document)
        {
            var documentUri = UriFactory.CreateDocumentUri(_dataStorageOptions.DatabaseId, _collectionId, document.Id);
            var response = await _client.ReplaceDocumentAsync(documentUri, document).ConfigureAwait(false);
        }

        public async Task InsertAsync<TEntity>(TEntity obj)
        {
            try
            {
                await _client.CreateDocumentAsync(_collectionUri, obj);
            }
            catch (DocumentClientException e)
            {
                Console.Write($"Unable to insert document. { e.StatusCode}: {e.Message}");
                throw;
            }
        }

        private object GetPartitionKey(Entity document)
        {
            string partitionKeyPath = _partitionKeyDefinition.Paths.FirstOrDefault();
            if (partitionKeyPath == null)
            {
                return Undefined.Value;
            }

            partitionKeyPath = XPathToJsonPath(partitionKeyPath);
            JToken value = JObject.FromObject(document).SelectToken(partitionKeyPath);

            return value.ToString();
        }

        private string XPathToJsonPath(string xPath)
        {
            return xPath.TrimStart('/').Replace('/', '.');
        }

        private void EnsureDatabaseExists()
        {
            try
            {
                _client.ReadDatabaseAsync(_databaseUri).GetAwaiter().GetResult();
                Console.Write($"Document storage database '{_databaseUri}' exists.");
            }
            catch (DocumentClientException e)
            {
                Console.Write($"Document storage database '{_databaseUri}' not found. {e.StatusCode}: {e.Message}");
                throw;
            }
        }

        private void EnsureCollectionExists(out PartitionKeyDefinition _partitionKeyDefinition)
        {
            IEnumerable<DocumentCollection> enumerable =
                _client.CreateDocumentCollectionQuery(_databaseUri).AsEnumerable();

            var collection = enumerable.FirstOrDefault(c => c.AltLink == _collectionUri.OriginalString);
            if (collection != null)
            {
                Console.Write($"Storage collection '{_collectionUri}' exists.");
                _partitionKeyDefinition = collection.PartitionKey;
                return;
            }

            Console.Write($"Storage collection '{_collectionUri}' not found.");
            _partitionKeyDefinition = null;
            throw new Exception($"Storage collection '{_collectionUri}' not found.");
        }
    }
}
