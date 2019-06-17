using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ReservaSala.Models
{
    public class ReservaSalaContext : DbContext
    {
        
    /*name=ReservaSalaContext*/
        public ReservaSalaContext() : base("dbSalas")
        {
        }

        public DbSet<Salas> Salas { get; set; }

        public DbSet<ReservaSala> ReservaSalas { get; set; }

        public DbSet<UsuarioSala> UsuarioSalas { get; set; }

    }
}
