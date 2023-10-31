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

    public virtual DbSet<CommunityEvent> CommunityEvents { get; set; }

    public virtual DbSet<Social> Socials { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCommunity> UserCommunities { get; set; }

    public virtual DbSet<UserEvent> UserEvents { get; set; }

    public virtual DbSet<UserTicket> UserTickets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=AdCommunityDb;User Id=postgres;Password=YgtTnyl2000.com.tr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Community>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Community_pkey");

            entity.ToTable("Community");

            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.Location).HasColumnType("character varying");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.Organizators).HasColumnType("character varying");
            entity.Property(e => e.Tags).HasColumnType("character varying");

            entity.HasOne(d => d.Social).WithMany(p => p.Communities)
                .HasForeignKey(d => d.SocialId)
                .HasConstraintName("FK_Community_Social");
        });

        modelBuilder.Entity<CommunityEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CommunityEvent_pkey");

            entity.ToTable("CommunityEvent");

            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.EventDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.EventName).HasColumnType("character varying");
            entity.Property(e => e.Location).HasColumnType("character varying");

            entity.HasOne(d => d.Community).WithMany(p => p.CommunityEvents)
                .HasForeignKey(d => d.CommunityId)
                .HasConstraintName("FK_CommunityEvent_Community");
        });

        modelBuilder.Entity<Social>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Social_pkey");

            entity.ToTable("Social");

            entity.Property(e => e.Facebook).HasColumnType("character varying");
            entity.Property(e => e.Github).HasColumnType("character varying");
            entity.Property(e => e.Instagram).HasColumnType("character varying");
            entity.Property(e => e.Medium).HasColumnType("character varying");
            entity.Property(e => e.Twitch).HasColumnType("character varying");
            entity.Property(e => e.Twitter).HasColumnType("character varying");
            entity.Property(e => e.website).HasColumnType("character varying");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Tickets_pkey");

            entity.Property(e => e.Pnr).HasColumnType("character varying");

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

            entity.Property(e => e.Email).HasColumnType("character varying");
            entity.Property(e => e.FirstName).HasColumnType("character varying");
            entity.Property(e => e.LastName).HasColumnType("character varying");
            entity.Property(e => e.Password).HasColumnType("character varying");
            entity.Property(e => e.Phone).HasColumnType("character varying");
            entity.Property(e => e.Username).HasColumnType("character varying");

            entity.HasOne(d => d.Social).WithMany(p => p.Users)
                .HasForeignKey(d => d.SocialId)
                .HasConstraintName("FK_Users_Social");
        });

        modelBuilder.Entity<UserCommunity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserCommunity_pkey");

            entity.ToTable("UserCommunity");

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
