using AISOptimization.Domain.Common;


namespace AISOptimization.Domain;

public class User: Entity
{
    public virtual UserRole Role { get; set; }
    public long RoleId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; } //todo to hashed password
    public virtual List<OptimizationProblem> OptimizationProblems { get; set; }
}
