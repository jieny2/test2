﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class XXXEntities : DbContext
    {
        public XXXEntities()
            : base("name=XXXEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<T_EMP_Employee> T_EMP_Employee { get; set; }
        public virtual DbSet<T_ORG_Department> T_ORG_Department { get; set; }
        public virtual DbSet<T_SYS_Group> T_SYS_Group { get; set; }
        public virtual DbSet<T_SYS_R_User_Role> T_SYS_R_User_Role { get; set; }
        public virtual DbSet<T_SYS_Role> T_SYS_Role { get; set; }
        public virtual DbSet<T_SYS_User> T_SYS_User { get; set; }
    }
}