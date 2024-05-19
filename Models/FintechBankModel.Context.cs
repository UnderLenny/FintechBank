﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FintechBank.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FintechBankEntities2 : DbContext
    {
        public FintechBankEntities2()
            : base("name=FintechBankEntities2")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transactions>()
                .HasRequired(t => t.Accounts)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.SenderAccountID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Transactions>()
                .HasRequired(t => t.Accounts1)
                .WithMany(a => a.Transactions1)
                .HasForeignKey(t => t.ReceiverAccountID)
                .WillCascadeOnDelete(false);
        }

        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<Cards> Cards { get; set; }
        public virtual DbSet<CardStatuses> CardStatuses { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<TransactionTypes> TransactionTypes { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
