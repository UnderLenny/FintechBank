﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FintechBank
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FintechBankEntities : DbContext
    {
        public FintechBankEntities()
            : base("name=FintechBankEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<Cards> Cards { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
