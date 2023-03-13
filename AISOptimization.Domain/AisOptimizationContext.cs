using AISOptimization.Core.Parameters;
using AISOptimization.Core.Restrictions;

using Microsoft.EntityFrameworkCore;


namespace AISOptimization.Core;

public class AisOptimizationContext : DbContext
{
    public string DbPath { get; }

    public DbSet<OptimizationProblem> OptimizationProblems { get; set; }
    public DbSet<FirstRoundRestriction> FirstRoundRestrictions { get; set; }
    public DbSet<SecondRoundRestriction> SecondRoundRestrictions { get; set; }
    public DbSet<StaticVariable> StaticVariables { get; set; }
    public DbSet<IndependentVariable> IndependentVariables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=AisOptimization.db");
    }
}

