﻿using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HappyTravel.Edo.Data.Infrastructure.DatabaseExtensions
{
    public static class EdoContextExtensions
    {
        public static void Detach<TEntity>(this EdoContext context, int id)
            where TEntity : class, IEntity
        {
            var local = context.Set<TEntity>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(id));
            if (local != null)
                context.Entry(local).State = EntityState.Detached;
        }


        public static void Detach<TEntity>(this EdoContext context, TEntity entity)
            where TEntity : class, IEntity
        {
            Detach<TEntity>(context, entity.Id);
        }
    }
}
