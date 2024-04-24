using CantoLiterário___projeto_LivroOnline.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Emit;

namespace CantoLiterário___projeto_LivroOnline.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Book { get; set; }
        public DbSet<BookContent> BookContents { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Genders> Genders { get; set; }  
        public DbSet<CapContent> CapContents { get; set; }



    }


}
