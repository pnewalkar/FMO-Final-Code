﻿using System;
using System.Data.Entity;

namespace RM.DataManagement.DeliveryRoute.WebAPI.DataService
{
    public class DatabaseFactory<TContext> : IDatabaseFactory<TContext>
        where TContext : DbContext
    {
        private TContext dataContext;

        public TContext Get()
        {
            return dataContext ?? (dataContext = (TContext)Activator.CreateInstance(typeof(TContext)));
        }
    }
}