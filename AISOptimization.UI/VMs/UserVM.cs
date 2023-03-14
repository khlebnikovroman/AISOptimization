using System.Collections.Generic;


namespace AISOptimization.VMs;

public class UserVM
{
    public long Id { get; set; }
    public UserRoleVM UserRole { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; } //todo to hashed password
    public List<OptimizationProblemVM> OptimizationProblems { get; set; }
}
