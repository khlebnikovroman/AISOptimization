using System.Collections.Generic;

using AISOptimization.Domain;


namespace AISOptimization.VMs;

/// <summary>
/// VM для <see cref="User"/>
/// </summary>
public class UserVM
{
    public long Id { get; set; }
    public UserRoleVM Role { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; } //todo to hashed password
    public List<OptimizationProblemVM> OptimizationProblems { get; set; }
}
