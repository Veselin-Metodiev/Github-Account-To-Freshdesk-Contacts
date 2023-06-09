namespace Github_Account_To_Freshdesk_Contacts.DbModels;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public partial class GithubAccountToFreshdeskContactContext : DbContext
{
	private readonly ConfigurationBuilder builder = null!;

    public GithubAccountToFreshdeskContactContext(ConfigurationBuilder builder)
    {
        this.builder = builder;
    }

    public GithubAccountToFreshdeskContactContext(DbContextOptions<GithubAccountToFreshdeskContactContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GithubAccountDb> GithubAccounts { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(builder.Build().GetSection("ConnectionString").Value);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
	    modelBuilder.Entity<GithubAccountDb>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GithubAc__3214EC07F343D077");

            entity.ToTable("GithubAccount");

            entity.HasIndex(e => e.Login, "UQ__GithubAc__5E55825BADA65470").IsUnique();

            entity.HasIndex(e => e.Name, "UQ__GithubAc__737584F603D83A56").IsUnique();

            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
