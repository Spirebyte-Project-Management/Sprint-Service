using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.EFCore.Extensions;
using Convey.MessageBrokers.Outbox.Messages;
using Microsoft.EntityFrameworkCore;
using Partytitan.Convey.MessageBrokers.Outbox.EntityFramework.Outbox.Entities;
using Spirebyte.Services.Sprints.Core.Entities;
using Spirebyte.Services.Sprints.Infrastructure.EntityFramework.Tables;

namespace Spirebyte.Services.Sprints.Infrastructure.EntityFramework
{
    public class SprintsDbContext : DbContext
    {
        public SprintsDbContext(DbContextOptions<SprintsDbContext> opt) : base(opt) { }

        public DbSet<InboxMessage> InboxMessages { get; set; }
        public DbSet<OutboxEntity> OutboxMessages { get; set; }
        public DbSet<SprintTable> Sprints { get; set; }
        public DbSet<IssueTable> Issues { get; set; }
        public DbSet<ProjectTable> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyAllConfigurationsFromCurrentAssembly();
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
