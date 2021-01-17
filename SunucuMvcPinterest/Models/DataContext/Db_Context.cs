using SunucuMvcPinterest.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SunucuMvcPinterest.Models.DataContext
{
    public class Db_Context : DbContext
    {
        public Db_Context() : base("Db_Context")
        {

        }
        public DbSet<Uye> Uye { get; set; }


        public DbSet<Yayin> Yayin { get; set; }}
}