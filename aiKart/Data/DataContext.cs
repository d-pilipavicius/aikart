using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using aiKart.Models;
using aiKart.States;

namespace aiKart.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    public DataContext()
    {
    }

    public virtual DbSet<Card> Cards { get; set; }
    public virtual DbSet<Deck> Decks { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserDeck> UserDecks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserDeck>()
            .HasKey(userDeck => new {userDeck.UserId, userDeck.DeckId});
        modelBuilder.Entity<UserDeck>()
            .HasOne(deck => deck.Deck)
            .WithMany(userDeck => userDeck.UserDecks)
            .HasForeignKey(deck => deck.DeckId);
        modelBuilder.Entity<UserDeck>()
            .HasOne(user => user.User)
            .WithMany(userDeck => userDeck.UserDecks)
            .HasForeignKey(user => user.UserId);
        
        modelBuilder.Entity<Deck>()
            .HasKey(deck => deck.Id);
        modelBuilder.Entity<Deck>()
            .HasMany(deck => deck.Cards)
            .WithOne(card => card.Deck)
            .HasForeignKey(card => card.DeckId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);          //When deck is deleted, its cards will be also deleted

        modelBuilder.Entity<Card>()
            .HasKey(card => card.Id);
        modelBuilder.Entity<Card>()
            .HasOne(card => card.Deck)
            .WithMany(deck => deck.Cards)
            .HasForeignKey(card => card.DeckId)
            .IsRequired();
        modelBuilder.Entity<Card>()
            .Property(card => card.State)
            .HasConversion(
                new EnumToStringConverter<CardState>() // Configure enum-to-string conversion for the 'State' property of 'Card'
            );
    }

}

