using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Edo.Data
{
    internal static class EdoContextModelExtensions
    {
        public static EntityDbMappingInfo GetEntityInfo<TEntity>(this EdoContext context)
        {
            var entityType = typeof(TEntity);
            return EntityInfos.GetOrAdd(entityType, (prop, dbContext) =>
                {
                    var entity = dbContext.Model.FindEntityType(entityType);
                    return new EntityDbMappingInfo()
                    {
                        Table = entity.Relational().TableName,
                        Schema = entity.Relational().Schema ?? DefaultSchema,
                        PropertyMapping = entity.GetProperties()
                            .ToDictionary(property => property.Name, property => property.Relational().ColumnName)
                    };
                },
                context);
        }
        
        private const string DefaultSchema = "public";

        private static readonly ConcurrentDictionary<Type, EntityDbMappingInfo> EntityInfos = new ConcurrentDictionary<Type, EntityDbMappingInfo>();
    }
}