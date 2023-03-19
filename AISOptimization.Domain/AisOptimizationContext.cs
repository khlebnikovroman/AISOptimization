using AISOptimization.Domain.Constraints;
using AISOptimization.Domain.Parameters;

using Microsoft.EntityFrameworkCore;


namespace AISOptimization.Domain;

public class AisOptimizationContext : DbContext
{
    public string DbPath { get; }

    public DbSet<OptimizationProblem> OptimizationProblems { get; set; }
    public DbSet<FirstRoundConstraint> FirstRoundConstraints { get; set; }
    public DbSet<SecondRoundConstraint> SecondRoundConstraints { get; set; }
    public DbSet<Constant> Constants { get; set; }
    public DbSet<DecisionVariable> DecisionVariables { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UsersRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options
            //.UseLazyLoadingProxies()
            .UseSqlite("Data Source=AisOptimization.db")
            .EnableSensitiveDataLogging(true);
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var adminRole = new UserRole() {Id = 1, RoleType = "Admin"};
        var userRole = new UserRole() {Id = 2, RoleType = "User"};
        var admin = new User() {Id = 1, UserName = "a", Password = "a", RoleId = adminRole.Id};
        var researcher = new User() {Id = 2, UserName = "r", Password = "r", RoleId = userRole.Id};

        modelBuilder.Entity<UserRole>().HasData(adminRole, userRole);
        modelBuilder.Entity<User>().HasData(admin, researcher);
        
    }
}



