
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackOverflowAPI_DAL.Configration;
using StackOverflowAPI_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace StackOverflowAPI_DAL.Data
{
   public class ApplicationDbContext: IdentityDbContext<User>
    {
        public DbSet<Question>? Questions { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<LikeToComment>? LikesToComment { get; set; }
        public DbSet<LikeToQuestion>? LikesToQuestion { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<Reply>? Replies { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }
      
        protected override void OnModelCreating(ModelBuilder builder)
        {
          
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.Entity<Comment>()
                .HasOne<Question>(q => q.QuestionObject)
                .WithMany(q=>q.Comments).
                HasForeignKey(c=>c.QuestionId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Entity<Question>()
                .HasMany<LikeToQuestion>(q=>q.Likes).
                WithOne(l=>l.QuestionObject)
                .HasForeignKey(l=>l.QuestionId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();

            builder.Entity<Comment>().HasMany<Reply>(c => c.Replies).WithOne(r=>r.CommentObject)
                .HasForeignKey(r=>r.CommentId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();
            builder.Entity<Comment>().HasMany<LikeToComment>(c => c.Likes)
                .WithOne(l=>l.CommentObject)
                .HasForeignKey(l => l.CommentId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            builder.Entity<Tag>().HasMany<Question>(t => t.Questions).WithMany(q => q.Tags);
            
        }
    }
}
