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

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=AisOptimization.db");
    }
    
}



