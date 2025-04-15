using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public partial class GameDbContext : DbContext
{
    public GameDbContext()
    {
    }

    public GameDbContext(DbContextOptions<GameDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Potrebitel> Potrebiteli { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseMySQL("Server=127.0.0.1;Database=GameDb;Uid=root;Pwd=root;");
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PRIMARY");

            entity.ToTable("game");

            entity.Property(e => e.GameId)
                .HasColumnType("int(11)")
                .HasColumnName("GameID");
            entity.Property(e => e.Title).HasMaxLength(20);

            entity.HasMany(d => d.Genres).WithMany(p => p.Games)
                .UsingEntity<Dictionary<string, object>>(
                    "Gamegenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("GenreId")
                        .HasConstraintName("gamegenre_ibfk_2"),
                    l => l.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .HasConstraintName("gamegenre_ibfk_1"),
                    j =>
                    {
                        j.HasKey("GameId", "GenreId").HasName("PRIMARY");
                        j.ToTable("gamegenre");
                        j.HasIndex(new[] { "GenreId" }, "GenreID");
                        j.IndexerProperty<int>("GameId")
                            .HasColumnType("int(11)")
                            .HasColumnName("GameID");
                        j.IndexerProperty<int>("GenreId")
                            .HasColumnType("int(11)")
                            .HasColumnName("GenreID");
                    });
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PRIMARY");

            entity.ToTable("genre");

            entity.Property(e => e.GenreId)
                .HasColumnType("int(11)")
                .HasColumnName("GenreID");
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Potrebitel>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("potrebitel");

            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("UserID");
            entity.Property(e => e.Age).HasColumnType("int(11)");
            entity.Property(e => e.Email).HasMaxLength(20);
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.Password).HasMaxLength(70);
            entity.Property(e => e.Username).HasMaxLength(20);

            entity.HasMany(d => d.Games).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Usergame",
                    r => r.HasOne<Game>().WithMany()
                        .HasForeignKey("GameId")
                        .HasConstraintName("usergame_ibfk_2"),
                    l => l.HasOne<Potrebitel>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("usergame_ibfk_1"),
                    j =>
                    {
                        j.HasKey("UserId", "GameId").HasName("PRIMARY");
                        j.ToTable("usergame");
                        j.HasIndex(new[] { "GameId" }, "GameID");
                        j.IndexerProperty<int>("UserId")
                            .HasColumnType("int(11)")
                            .HasColumnName("UserID");
                        j.IndexerProperty<int>("GameId")
                            .HasColumnType("int(11)")
                            .HasColumnName("GameID");
                    });

            entity.HasMany(d => d.UserId1s).WithMany(p => p.UserId2s)
                .UsingEntity<Dictionary<string, object>>(
                    "Friendship",
                    r => r.HasOne<Potrebitel>().WithMany()
                        .HasForeignKey("UserId1")
                        .HasConstraintName("friendship_ibfk_1"),
                    l => l.HasOne<Potrebitel>().WithMany()
                        .HasForeignKey("UserId2")
                        .HasConstraintName("friendship_ibfk_2"),
                    j =>
                    {
                        j.HasKey("UserId1", "UserId2").HasName("PRIMARY");
                        j.ToTable("friendship");
                        j.HasIndex(new[] { "UserId2" }, "UserID2");
                        j.IndexerProperty<int>("UserId1")
                            .HasColumnType("int(11)")
                            .HasColumnName("UserID1");
                        j.IndexerProperty<int>("UserId2")
                            .HasColumnType("int(11)")
                            .HasColumnName("UserID2");
                    });

            entity.HasMany(d => d.UserId2s).WithMany(p => p.UserId1s)
                .UsingEntity<Dictionary<string, object>>(
                    "Friendship",
                    r => r.HasOne<Potrebitel>().WithMany()
                        .HasForeignKey("UserId2")
                        .HasConstraintName("friendship_ibfk_2"),
                    l => l.HasOne<Potrebitel>().WithMany()
                        .HasForeignKey("UserId1")
                        .HasConstraintName("friendship_ibfk_1"),
                    j =>
                    {
                        j.HasKey("UserId1", "UserId2").HasName("PRIMARY");
                        j.ToTable("friendship");
                        j.HasIndex(new[] { "UserId2" }, "UserID2");
                        j.IndexerProperty<int>("UserId1")
                            .HasColumnType("int(11)")
                            .HasColumnName("UserID1");
                        j.IndexerProperty<int>("UserId2")
                            .HasColumnType("int(11)")
                            .HasColumnName("UserID2");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
