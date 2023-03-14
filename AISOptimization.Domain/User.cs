using AISOptimization.Domain.Common;


namespace AISOptimization.Domain;

public class User: Entity
{
    public string Role { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; } //todo to hashed password
    public List<OptimizationProblem> OptimizationProblems { get; set; }
}
