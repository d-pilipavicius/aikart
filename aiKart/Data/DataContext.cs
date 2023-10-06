using Microsoft.EntityFrameworkCore;
using aiKart.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using aiKart.States;

namespace aiKart.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Card> Cards { get; set; }
    public DbSet<Deck> Decks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Card>()
            .HasKey(card => new { card.Id });
        modelBuilder.Entity<Deck>()
            .HasKey(deck => new { deck.Id });

        modelBuilder.Entity<Deck>()
            .HasMany(deck => deck.Cards)
            .WithOne(card => card.Deck)
            .HasForeignKey(card => card.DeckId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);          //When deck is deleted, its cards will be also deleted

        modelBuilder.Entity<Card>()
            .HasOne(card => card.Deck)
            .WithMany(deck => deck.Cards)
            .HasForeignKey(card => card.DeckId)
            .IsRequired();

        // Configure enum-to-string conversion for the 'State' property of 'Card'
        modelBuilder.Entity<Card>()
            .Property(card => card.State)
            .HasConversion(
                new EnumToStringConverter<CardState>()
            );
    }

}