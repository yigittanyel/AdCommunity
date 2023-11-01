using System;
using System.Collections.Generic;
using AdCommunity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Repository.Context;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Community> Communities { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCommunity> UserCommunities { get; set; }

    public virtual DbSet<UserEvent> UserEvents { get; set; }

    public virtual DbSet<UserTicket> UserTickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=Default");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Community>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Community_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Community_Id_seq\"'::regclass)");
            entity.Property(e => e.CreatedOn).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.Facebook).HasColumnType("character varying");
            entity.Property(e => e.Github).HasColumnType("character varying");
            entity.Property(e => e.Instagram).HasColumnType("character varying");
            entity.Property(e => e.Location).HasColumnType("character varying");
            entity.Property(e => e.Medium).HasColumnType("character varying");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.Organizators).HasColumnType("character varying");
            entity.Property(e => e.Tags).HasColumnType("character varying");
            entity.Property(e => e.Twitter).HasColumnType("character varying");
            entity.Property(e => e.Website).HasColumnType("character varying");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CommunityEvent_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"CommunityEvent_Id_seq\"'::regclass)");
            entity.Property(e => e.CreatedOn).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.EventDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EventName).HasColumnType("character varying");
            entity.Property(e => e.Location).HasColumnType("character varying");

            entity.HasOne(d => d.Community).WithMany(p => p.Events)
                .HasForeignKey(d => d.CommunityId)
                .HasConstraintName("FK_CommunityEvent_Community");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Tickets_pkey");

            entity.Property(e => e.CreatedOn).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.CommunityEvent).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.CommunityEventId)
                .HasConstraintName("FK_Tickets_CommunityEvent");

            entity.HasOne(d => d.Community).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.CommunityId)
                .HasConstraintName("FK_Tickets_Community");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.Property(e => e.CreatedOn).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Email).HasColumnType("character varying");
            entity.Property(e => e.Facebook).HasColumnType("character varying");
            entity.Property(e => e.FirstName).HasColumnType("character varying");
            entity.Property(e => e.Github).HasColumnType("character varying");
            entity.Property(e => e.HashedPassword)
                .HasColumnType("character varying")
                .HasColumnName("HashedPassword ");
            entity.Property(e => e.Instagram).HasColumnType("character varying");
            entity.Property(e => e.LastName).HasColumnType("character varying");
            entity.Property(e => e.Medium).HasColumnType("character varying");
            entity.Property(e => e.Password).HasColumnType("character varying");
            entity.Property(e => e.Phone).HasColumnType("character varying");
            entity.Property(e => e.Twitter).HasColumnType("character varying");
            entity.Property(e => e.Username).HasColumnType("character varying");
            entity.Property(e => e.Website).HasColumnType("character varying");
        });

        modelBuilder.Entity<UserCommunity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserCommunity_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"UserCommunity_Id_seq\"'::regclass)");
            entity.Property(e => e.CreatedOn).HasColumnType("timestamp without time zone");
            entity.Property(e => e.JoinDate).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Community).WithMany(p => p.UserCommunities)
                .HasForeignKey(d => d.CommunityId)
                .HasConstraintName("FK_UserCommunity_Community");

            entity.HasOne(d => d.User).WithMany(p => p.UserCommunities)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserCommunity_Users");
        });

        modelBuilder.Entity<UserEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserEvents_pkey");

            entity.Property(e => e.CreatedOn).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Event).WithMany(p => p.UserEvents)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_UserEvents_CommunityEvent");

            entity.HasOne(d => d.User).WithMany(p => p.UserEvents)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserEvents_Users");
        });

        modelBuilder.Entity<UserTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserTickets_pkey");

            entity.Property(e => e.CreatedOn).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Pnr).HasColumnType("character varying");

            entity.HasOne(d => d.Ticket).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.TicketId)
                .HasConstraintName("FK_UserTickets_Tickets");

            entity.HasOne(d => d.User).WithMany(p => p.UserTickets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserTickets_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
