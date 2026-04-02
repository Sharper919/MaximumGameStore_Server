using System;
using System.Collections.Generic;
using MaximumGameStore.Models;
using Microsoft.EntityFrameworkCore;

namespace MaximumGameStore.Data;

public partial class MaximumGameStoreContext : DbContext
{
    public MaximumGameStoreContext()
    {
    }

    public MaximumGameStoreContext(DbContextOptions<MaximumGameStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CardPayment> CardPayments { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Developer> Developers { get; set; }

    public virtual DbSet<Engine> Engines { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameDeveloper> GameDevelopers { get; set; }

    public virtual DbSet<GameEngine> GameEngines { get; set; }

    public virtual DbSet<GameGenre> GameGenres { get; set; }

    public virtual DbSet<GameImage> GameImages { get; set; }

    public virtual DbSet<GameMode> GameModes { get; set; }

    public virtual DbSet<GamePublisher> GamePublishers { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Mode> Modes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Series> Series { get; set; }

    public virtual DbSet<SystemRequirement> SystemRequirements { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CardPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CardPaym__3214EC27478CC0A3");

            entity.Property(e => e.CardLast4).IsFixedLength();
            entity.Property(e => e.DateTimePayment).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Order).WithOne(p => p.CardPayment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CardPayments_Orders");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3214EC279A7183F5");

            entity.Property(e => e.DateTimeCreation).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DateTimeUpdate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Active");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Users");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CartItem__3214EC275DC5A9A0");

            entity.Property(e => e.DateTimeAdded).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems).HasConstraintName("FK_CartItems_Cart");

            entity.HasOne(d => d.Game).WithMany(p => p.CartItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Games");
        });

        modelBuilder.Entity<Developer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Develope__3214EC27C9564D31");
        });

        modelBuilder.Entity<Engine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Engines__3214EC27953C4571");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Games__3214EC2764629950");

            entity.Property(e => e.Price).HasDefaultValueSql("((0.00))");

            entity.HasOne(d => d.Series).WithMany(p => p.Games)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Games_Series");
        });

        modelBuilder.Entity<GameDeveloper>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameDeve__3214EC27C27F4E79");

            entity.HasOne(d => d.Developer).WithMany(p => p.GameDevelopers).HasConstraintName("FK_GameDevelopers_Developers");

            entity.HasOne(d => d.Game).WithMany(p => p.GameDevelopers).HasConstraintName("FK_GameDevelopers_Games");
        });

        modelBuilder.Entity<GameEngine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameEngi__3214EC27407B4504");

            entity.HasOne(d => d.Engine).WithMany(p => p.GameEngines).HasConstraintName("FK_GameEngines_Engines");

            entity.HasOne(d => d.Game).WithMany(p => p.GameEngines).HasConstraintName("FK_GameEngines_Games");
        });

        modelBuilder.Entity<GameGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameGenr__3214EC27CEA12E99");

            entity.HasOne(d => d.Game).WithMany(p => p.GameGenres).HasConstraintName("FK_GameGenres_Games");

            entity.HasOne(d => d.Genre).WithMany(p => p.GameGenres).HasConstraintName("FK_GameGenres_Genres");
        });

        modelBuilder.Entity<GameImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameImag__3214EC27DCF1C389");

            entity.HasOne(d => d.Game).WithMany(p => p.GameImages).HasConstraintName("FK_GameImages_Games");
        });

        modelBuilder.Entity<GameMode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameMode__3214EC270A27860E");

            entity.HasOne(d => d.Game).WithMany(p => p.GameModes).HasConstraintName("FK_GameModes_Games");

            entity.HasOne(d => d.Mode).WithMany(p => p.GameModes).HasConstraintName("FK_GameModes_Modes");
        });

        modelBuilder.Entity<GamePublisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GamePubl__3214EC27F14ACB93");

            entity.HasOne(d => d.Game).WithMany(p => p.GamePublishers).HasConstraintName("FK_GamePublishers_Games");

            entity.HasOne(d => d.Publisher).WithMany(p => p.GamePublishers).HasConstraintName("FK_GamePublishers_Publishers");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genres__3214EC27CF0BEA60");
        });

        modelBuilder.Entity<Mode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Modes__3214EC2792ED1690");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC27F26E5434");

            entity.Property(e => e.DateTimeOrder).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC2701EDAA42");

            entity.HasOne(d => d.Game).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Games");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Orders");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publishe__3214EC27034F18A8");
        });

        modelBuilder.Entity<Series>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Series__3214EC27289A20FA");
        });

        modelBuilder.Entity<SystemRequirement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SystemRe__3214EC27D8C6B86D");

            entity.HasOne(d => d.Game).WithMany(p => p.SystemRequirements).HasConstraintName("FK_SystemRequirements_Games");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC275525C94A");

            entity.Property(e => e.DateTimeRegistration).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Role).HasDefaultValue("User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
