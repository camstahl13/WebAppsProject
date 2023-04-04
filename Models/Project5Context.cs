using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ljcProject5.Models;

public partial class Project5Context : DbContext
{
    public Project5Context()
    {
    }

    public Project5Context(DbContextOptions<Project5Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Aspnetrole> Aspnetroles { get; set; }

    public virtual DbSet<Aspnetroleclaim> Aspnetroleclaims { get; set; }

    public virtual DbSet<Aspnetuser> Aspnetusers { get; set; }

    public virtual DbSet<Aspnetuserclaim> Aspnetuserclaims { get; set; }

    public virtual DbSet<Aspnetuserlogin> Aspnetuserlogins { get; set; }

    public virtual DbSet<Aspnetusertoken> Aspnetusertokens { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<LjcCatalog> LjcCatalogs { get; set; }

    public virtual DbSet<LjcCatayear> LjcCatayears { get; set; }

    public virtual DbSet<LjcCourse> LjcCourses { get; set; }

    public virtual DbSet<LjcMajor> LjcMajors { get; set; }

    public virtual DbSet<LjcMajorRequirement> LjcMajorRequirements { get; set; }

    public virtual DbSet<LjcMinor> LjcMinors { get; set; }

    public virtual DbSet<LjcMinorRequirement> LjcMinorRequirements { get; set; }

    public virtual DbSet<LjcPlan> LjcPlans { get; set; }

    public virtual DbSet<LjcPlannedCourse> LjcPlannedCourses { get; set; }

    public virtual DbSet<LjcPlannedMajor> LjcPlannedMajors { get; set; }

    public virtual DbSet<LjcPlannedMinor> LjcPlannedMinors { get; set; }

    public virtual DbSet<LjcUser> LjcUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=163.11.236.98;database=project5;username=webapps;password=Kp!gLkfCTNRjh8KG", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.27-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Aspnetrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetroles");

            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<Aspnetroleclaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetroleclaims");

            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.Property(e => e.Id).HasColumnType("int(11)");

            entity.HasOne(d => d.Role).WithMany(p => p.Aspnetroleclaims)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_AspNetRoleClaims_AspNetRoles_RoleId");
        });

        modelBuilder.Entity<Aspnetuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetusers");

            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.AccessFailedCount).HasColumnType("int(11)");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.LockoutEnd).HasMaxLength(6);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Aspnetuserrole",
                    r => r.HasOne<Aspnetrole>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_AspNetUserRoles_AspNetRoles_RoleId"),
                    l => l.HasOne<Aspnetuser>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_AspNetUserRoles_AspNetUsers_UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("aspnetuserroles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<Aspnetuserclaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetuserclaims");

            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.Property(e => e.Id).HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserclaims)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserClaims_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Aspnetuserlogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("aspnetuserlogins");

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserlogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserLogins_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Aspnetusertoken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("aspnetusertokens");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetusertokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserTokens_AspNetUsers_UserId");
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__efmigrationshistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<LjcCatalog>(entity =>
        {
            entity.HasKey(e => new { e.CatalogYear, e.CourseId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("ljc_catalog")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.HasIndex(e => e.CourseId, "course_id");

            entity.Property(e => e.CatalogYear)
                .HasColumnType("int(11)")
                .HasColumnName("catalog_year");
            entity.Property(e => e.CourseId)
                .HasMaxLength(9)
                .HasColumnName("course_id");
        });

        modelBuilder.Entity<LjcCatayear>(entity =>
        {
            entity.HasKey(e => e.CatalogYear).HasName("PRIMARY");

            entity
                .ToTable("ljc_catayear")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.Property(e => e.CatalogYear)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("catalog_year");
        });

        modelBuilder.Entity<LjcCourse>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PRIMARY");

            entity
                .ToTable("ljc_course")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.Property(e => e.CourseId)
                .HasMaxLength(9)
                .HasColumnName("course_id");
            entity.Property(e => e.Credits)
                .HasColumnType("int(11)")
                .HasColumnName("credits");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(40)
                .HasColumnName("title");
        });

        modelBuilder.Entity<LjcMajor>(entity =>
        {
            entity.HasKey(e => e.MajorId).HasName("PRIMARY");

            entity
                .ToTable("ljc_major")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.Property(e => e.MajorId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("major_id");
            entity.Property(e => e.Major)
                .HasMaxLength(60)
                .HasColumnName("major");
        });

        modelBuilder.Entity<LjcMajorRequirement>(entity =>
        {
            entity.HasKey(e => new { e.MajorId, e.CatalogYear, e.CourseId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("ljc_major_requirements")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.HasIndex(e => new { e.CatalogYear, e.CourseId }, "catalog_year");

            entity.HasIndex(e => e.CourseId, "course_id");

            entity.Property(e => e.MajorId)
                .HasColumnType("int(11)")
                .HasColumnName("major_id");
            entity.Property(e => e.CatalogYear)
                .HasColumnType("int(11)")
                .HasColumnName("catalog_year");
            entity.Property(e => e.CourseId)
                .HasMaxLength(9)
                .HasColumnName("course_id");
            entity.Property(e => e.Category)
                .HasMaxLength(15)
                .HasColumnName("category");
        });

        modelBuilder.Entity<LjcMinor>(entity =>
        {
            entity.HasKey(e => e.MinorId).HasName("PRIMARY");

            entity
                .ToTable("ljc_minor")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.Property(e => e.MinorId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("minor_id");
            entity.Property(e => e.Minor)
                .HasMaxLength(60)
                .HasColumnName("minor");
        });

        modelBuilder.Entity<LjcMinorRequirement>(entity =>
        {
            entity.HasKey(e => new { e.MinorId, e.CatalogYear, e.CourseId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity
                .ToTable("ljc_minor_requirements")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.HasIndex(e => new { e.CatalogYear, e.CourseId }, "catalog_year");

            entity.HasIndex(e => e.CourseId, "course_id");

            entity.Property(e => e.MinorId)
                .HasColumnType("int(11)")
                .HasColumnName("minor_id");
            entity.Property(e => e.CatalogYear)
                .HasColumnType("int(11)")
                .HasColumnName("catalog_year");
            entity.Property(e => e.CourseId)
                .HasMaxLength(9)
                .HasColumnName("course_id");
        });

        modelBuilder.Entity<LjcPlan>(entity =>
        {
            entity.HasKey(e => e.PlanId).HasName("PRIMARY");

            entity
                .ToTable("ljc_plan")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.HasIndex(e => e.CatalogYear, "catalog_year");

            entity.HasIndex(e => e.Username, "ljc_plan_ibfk_1");

            entity.Property(e => e.PlanId)
                .HasColumnType("int(11)")
                .HasColumnName("plan_id");
            entity.Property(e => e.CatalogYear)
                .HasColumnType("int(11)")
                .HasColumnName("catalog_year");
            entity.Property(e => e.Default).HasColumnName("default_");
            entity.Property(e => e.Planname)
                .HasMaxLength(30)
                .HasColumnName("planname");
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .HasColumnName("username");
        });

        modelBuilder.Entity<LjcPlannedCourse>(entity =>
        {
            entity.HasKey(e => new { e.PlanId, e.CourseId, e.Year, e.Term })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0, 0 });

            entity
                .ToTable("ljc_planned_courses")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.HasIndex(e => e.CourseId, "ljc_planned_courses_ibfk_2");

            entity.Property(e => e.PlanId)
                .HasColumnType("int(11)")
                .HasColumnName("plan_id");
            entity.Property(e => e.CourseId)
                .HasMaxLength(9)
                .HasColumnName("course_id");
            entity.Property(e => e.Year)
                .HasColumnType("int(11)")
                .HasColumnName("year");
            entity.Property(e => e.Term)
                .HasMaxLength(2)
                .HasColumnName("term");
        });

        modelBuilder.Entity<LjcPlannedMajor>(entity =>
        {
            entity.HasKey(e => new { e.PlanId, e.MajorId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("ljc_planned_majors")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.HasIndex(e => e.MajorId, "ljc_planned_majors_ibfk_2");

            entity.Property(e => e.PlanId)
                .HasColumnType("int(11)")
                .HasColumnName("plan_id");
            entity.Property(e => e.MajorId)
                .HasColumnType("int(11)")
                .HasColumnName("major_id");
        });

        modelBuilder.Entity<LjcPlannedMinor>(entity =>
        {
            entity.HasKey(e => new { e.PlanId, e.MinorId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("ljc_planned_minors")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.HasIndex(e => e.MinorId, "ljc_planned_minors_ibfk_2");

            entity.Property(e => e.PlanId)
                .HasColumnType("int(11)")
                .HasColumnName("plan_id");
            entity.Property(e => e.MinorId)
                .HasColumnType("int(11)")
                .HasColumnName("minor_id");
        });

        modelBuilder.Entity<LjcUser>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PRIMARY");

            entity
                .ToTable("ljc_user")
                .HasCharSet("latin1")
                .UseCollation("latin1_swedish_ci");

            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .HasColumnName("username");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
