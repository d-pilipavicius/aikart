using Microsoft.EntityFrameworkCore;
using aiKart.Models;
using System.Reflection;

namespace aikart.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base (options)
    {
    }

    public DbSet<Card> Cards {get; set;}
    public DbSet<Card> Decks {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>()
            .HasKey(card => new {card.Id});
        modelBuilder.Entity<Deck>()
            .HasKey(deck => new {deck.Id});

        modelBuilder.Entity<Deck>()
            .HasMany(deck => deck.Cards)
            .WithOne(card => card.Deck)
            .HasForeignKey(card => card.DeckId)
            .IsRequired();

        modelBuilder.Entity<Card>()
            .HasOne(card => card.Deck)
            .WithMany(deck => deck.Cards)
            .HasForeignKey(card => card.DeckId)
            .IsRequired();
    }

}