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

            entity.HasIndex(e => e.OrderId, "UQ__CardPaym__C3905BAE08388269").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CardLast4)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CardType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DateTimePayment)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Order).WithOne(p => p.CardPayment)
                .HasForeignKey<CardPayment>(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CardPayments_Orders");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cart__3214EC279A7183F5");

            entity.ToTable("Cart");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateTimeCreation)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateTimeUpdate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Users");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CartItem__3214EC275DC5A9A0");

            entity.HasIndex(e => new { e.CartId, e.GameId }, "UQ_Cart_Game").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CartId).HasColumnName("CartID");
            entity.Property(e => e.DateTimeAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GameId).HasColumnName("GameID");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("FK_CartItems_Cart");

            entity.HasOne(d => d.Game).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Games");
        });

        modelBuilder.Entity<Developer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Develope__3214EC27C9564D31");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Engine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Engines__3214EC27953C4571");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Games__3214EC2764629950");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price)
                .HasDefaultValueSql("((0.00))")
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SeriesId).HasColumnName("SeriesID");

            entity.HasOne(d => d.Series).WithMany(p => p.Games)
                .HasForeignKey(d => d.SeriesId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Games_Series");
        });

        modelBuilder.Entity<GameDeveloper>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameDeve__3214EC27C27F4E79");

            entity.HasIndex(e => new { e.GameId, e.DeveloperId }, "UQ_GameDeveloper").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DeveloperId).HasColumnName("DeveloperID");
            entity.Property(e => e.GameId).HasColumnName("GameID");

            entity.HasOne(d => d.Developer).WithMany(p => p.GameDevelopers)
                .HasForeignKey(d => d.DeveloperId)
                .HasConstraintName("FK_GameDevelopers_Developers");

            entity.HasOne(d => d.Game).WithMany(p => p.GameDevelopers)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_GameDevelopers_Games");
        });

        modelBuilder.Entity<GameEngine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameEngi__3214EC27407B4504");

            entity.HasIndex(e => new { e.GameId, e.EngineId }, "UQ_GameEngines").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EngineId).HasColumnName("EngineID");
            entity.Property(e => e.GameId).HasColumnName("GameID");

            entity.HasOne(d => d.Engine).WithMany(p => p.GameEngines)
                .HasForeignKey(d => d.EngineId)
                .HasConstraintName("FK_GameEngines_Engines");

            entity.HasOne(d => d.Game).WithMany(p => p.GameEngines)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_GameEngines_Games");
        });

        modelBuilder.Entity<GameGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameGenr__3214EC27CEA12E99");

            entity.HasIndex(e => new { e.GameId, e.GenreId }, "UQ_GameGenres").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.GenreId).HasColumnName("GenreID");

            entity.HasOne(d => d.Game).WithMany(p => p.GameGenres)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_GameGenres_Games");

            entity.HasOne(d => d.Genre).WithMany(p => p.GameGenres)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK_GameGenres_Genres");
        });

        modelBuilder.Entity<GameImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameImag__3214EC27DCF1C389");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.ImagePath).HasMaxLength(500);

            entity.HasOne(d => d.Game).WithMany(p => p.GameImages)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_GameImages_Games");
        });

        modelBuilder.Entity<GameMode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GameMode__3214EC270A27860E");

            entity.HasIndex(e => new { e.GameId, e.ModeId }, "UQ_GameModes").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.ModeId).HasColumnName("ModeID");

            entity.HasOne(d => d.Game).WithMany(p => p.GameModes)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_GameModes_Games");

            entity.HasOne(d => d.Mode).WithMany(p => p.GameModes)
                .HasForeignKey(d => d.ModeId)
                .HasConstraintName("FK_GameModes_Modes");
        });

        modelBuilder.Entity<GamePublisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GamePubl__3214EC27F14ACB93");

            entity.HasIndex(e => new { e.GameId, e.PublisherId }, "UQ_GamePublishers").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

            entity.HasOne(d => d.Game).WithMany(p => p.GamePublishers)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("FK_GamePublishers_Games");

            entity.HasOne(d => d.Publisher).WithMany(p => p.GamePublishers)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK_GamePublishers_Publishers");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genres__3214EC27CF0BEA60");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Mode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Modes__3214EC2792ED1690");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC27F26E5434");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateTimeOrder)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC2701EDAA42");

            entity.HasIndex(e => new { e.OrderId, e.GameId }, "UQ_Order_Game").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.PriceAtPurchase).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Game).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Games");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Orders");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Publishe__3214EC27034F18A8");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Series>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Series__3214EC27289A20FA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<SystemRequirement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SystemRe__3214EC27D8C6B86D");

            entity.HasIndex(e => new { e.GameId, e.RequirementType }, "UQ_RequirementType_Game").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Cpu)
                .HasMaxLength(150)
                .HasColumnName("CPU");
            entity.Property(e => e.DirectX).HasMaxLength(50);
            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.Gpu)
                .HasMaxLength(150)
                .HasColumnName("GPU");
            entity.Property(e => e.Os)
                .HasMaxLength(150)
                .HasColumnName("OS");
            entity.Property(e => e.RamGb).HasColumnName("RAM_GB");
            entity.Property(e => e.RequirementType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.StorageGb).HasColumnName("Storage_GB");

            entity.HasOne(d => d.Game).WithMany(p => p.SystemRequirements)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SystemRequirements_Games");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC275525C94A");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105346F83D4F9").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateTimeRegistration)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
