﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ANYI_Cafe.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class anyicafeEntities : DbContext
    {
        public anyicafeEntities()
            : base("name=anyicafeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Shippings> Shippings { get; set; }
        public virtual DbSet<Propertys> Propertys { get; set; }
        public virtual DbSet<ProductsProperty> ProductsProperty { get; set; }
        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<OrdersDetail> OrdersDetail { get; set; }
        public virtual DbSet<city> city { get; set; }
        public virtual DbSet<district> district { get; set; }
        public virtual DbSet<road> road { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<user> user { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Categorys> Categorys { get; set; }
        public virtual DbSet<product> product { get; set; }
        public virtual DbSet<vendor> vendor { get; set; }
    }
}
