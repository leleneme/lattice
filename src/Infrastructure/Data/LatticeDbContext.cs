namespace Lattice.Infrastructure.Data;

public class LatticeDbContext : DbContext
{
    public DbSet<UserAccount> Users => Set<UserAccount>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<UserTeam> UserTeams => Set<UserTeam>();
    public DbSet<Board> Boards => Set<Board>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<Card> Cards => Set<Card>();

    public LatticeDbContext(DbContextOptions options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserAccount>()
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Entity<UserAccount>()
            .HasIndex(u => u.Email)
            .IsUnique();

        builder.Entity<Team>()
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();

        builder.Entity<Board>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        builder.Entity<Section>()
            .Property(l => l.Id)
            .ValueGeneratedOnAdd();

        builder.Entity<Card>()
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();


        builder.Entity<Team>()
            .HasOne(t => t.Owner)
            .WithMany()
            .HasForeignKey(t => t.OwnerId);


        builder.Entity<UserTeam>()
            .HasOne(tu => tu.Team)
            .WithMany(t => t.UserTeams)
            .HasForeignKey(tu => tu.TeamId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<UserTeam>()
            .HasOne(tu => tu.User)
            .WithMany(u => u.UserTeams)
            .HasForeignKey(tu => tu.UserId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.Entity<Board>()
            .HasOne(b => b.Team)
            .WithMany(t => t.Boards)
            .HasForeignKey(b => b.TeamId);

        builder.Entity<Board>()
            .HasOne(b => b.Creator)
            .WithMany()
            .HasForeignKey(b => b.CreatedBy);


        builder.Entity<Card>()
            .HasOne(c => c.Assigned)
            .WithMany(u => u.Cards)
            .HasForeignKey(t => t.AssignedTo);

        builder.Entity<Card>()
            .HasOne(c => c.Section)
            .WithMany(s => s.Cards)
            .HasForeignKey(t => t.SectionId);

        builder.Entity<Card>()
            .HasOne(c => c.Creator)
            .WithMany()
            .HasForeignKey(t => t.CreatedBy);


        builder.Entity<Section>()
            .HasOne(l => l.Board)
            .WithMany(b => b.Sections)
            .HasForeignKey(l => l.BoardId);

        base.OnModelCreating(builder);
    }
}