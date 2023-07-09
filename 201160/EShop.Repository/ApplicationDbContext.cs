using EShop.Domain.DomainModels;
using EShop.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace EShop.Repository
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<TickedInCard> TickedInCards { get; set; }
        public virtual DbSet<TicketInOrder> TicketInOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ticket>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Card>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();
            /*builder.Entity<TickedInCard>()
                .HasKey(z => new { z.TicketId, z.CardId });*/
            builder.Entity<TickedInCard>()
                .HasOne(z => z.Ticket)
                .WithMany(z => z.TickedInCards)
                .HasForeignKey(z => z.CardId);
            builder.Entity<TickedInCard>()
                .HasOne(z => z.Card)
                .WithMany(z => z.TickedInCards)
                .HasForeignKey(z => z.TicketId);
            builder.Entity<Card>()
                .HasOne<User>(z => z.Owner)
                .WithOne(z => z.UserCard)
                .HasForeignKey<Card>(z => z.OwnerId);

           /* builder.Entity<TicketInOrder>()
               .HasKey(z => new { z.TicketId, z.OrderId });*/
            builder.Entity<TicketInOrder>()
                .HasOne(z => z.OrderTicket)
                .WithMany(z => z.TicketInOrders)
                .HasForeignKey(z => z.OrderId);
            builder.Entity<TicketInOrder>()
                .HasOne(z => z.UserOrder)
                .WithMany(z => z.TicketInOrders)
                .HasForeignKey(z => z.TicketId);
        }
    }
}
