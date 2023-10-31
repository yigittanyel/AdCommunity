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

    public virtual DbSet<Communityevent> Communityevents { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Social> Socials { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Usercommunity> Usercommunities { get; set; }

    public virtual DbSet<Userevent> Userevents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=CnnStr");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Community>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("community_pkey");

            entity.ToTable("community");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Location)
                .HasColumnType("character varying")
                .HasColumnName("location");
            entity.Property(e => e.Membercount).HasColumnName("membercount");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Organizators)
                .HasColumnType("character varying")
                .HasColumnName("organizators");
            entity.Property(e => e.Social)
                .HasColumnType("jsonb")
                .HasColumnName("social");
            entity.Property(e => e.Tags)
                .HasColumnType("character varying")
                .HasColumnName("tags");
        });

        modelBuilder.Entity<Communityevent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("communityevents_pkey");

            entity.ToTable("communityevents");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Attendingmembercount).HasColumnName("attendingmembercount");
            entity.Property(e => e.Communityid).HasColumnName("communityid");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Eventdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("eventdate");
            entity.Property(e => e.Eventname)
                .HasColumnType("character varying")
                .HasColumnName("eventname");
            entity.Property(e => e.Location)
                .HasColumnType("character varying")
                .HasColumnName("location");

            entity.HasOne(d => d.Community).WithMany(p => p.Communityevents)
                .HasForeignKey(d => d.Communityid)
                .HasConstraintName("communityevents_communityid_fkey");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("invoice_pkey");

            entity.ToTable("invoice");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Invoiceno)
                .HasColumnType("character varying")
                .HasColumnName("invoiceno");
        });

        modelBuilder.Entity<Social>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("social");

            entity.Property(e => e.Facebook)
                .HasColumnType("character varying")
                .HasColumnName("facebook");
            entity.Property(e => e.Github)
                .HasColumnType("character varying")
                .HasColumnName("github");
            entity.Property(e => e.Instagram)
                .HasColumnType("character varying")
                .HasColumnName("instagram");
            entity.Property(e => e.Medium)
                .HasColumnType("character varying")
                .HasColumnName("medium");
            entity.Property(e => e.Twitch)
                .HasColumnType("character varying")
                .HasColumnName("twitch");
            entity.Property(e => e.Twitter)
                .HasColumnType("character varying")
                .HasColumnName("twitter");
            entity.Property(e => e.Website)
                .HasColumnType("character varying")
                .HasColumnName("website");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tickets_pkey");

            entity.ToTable("tickets");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Pnr)
                .HasColumnType("character varying")
                .HasColumnName("pnr");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("fk_tickets_users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasColumnType("character varying")
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasColumnType("character varying")
                .HasColumnName("lastname");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasColumnType("character varying")
                .HasColumnName("phone");
            entity.Property(e => e.Username)
                .HasColumnType("character varying")
                .HasColumnName("username");
        });

        modelBuilder.Entity<Usercommunity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usercommunity_pkey");

            entity.ToTable("usercommunity");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Communityid).HasColumnName("communityid");
            entity.Property(e => e.Joindate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("joindate");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Community).WithMany(p => p.Usercommunities)
                .HasForeignKey(d => d.Communityid)
                .HasConstraintName("usercommunity_communityid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Usercommunities)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("usercommunity_userid_fkey");
        });

        modelBuilder.Entity<Userevent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("userevents_pkey");

            entity.ToTable("userevents");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Eventid).HasColumnName("eventid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Event).WithMany(p => p.Userevents)
                .HasForeignKey(d => d.Eventid)
                .HasConstraintName("userevents_eventid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Userevents)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("userevents_userid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
