﻿using AISOptimization.Domain.Common;


namespace AISOptimization.Domain;

/// <summary>
/// Пользователь
/// </summary>
public class User: Entity
{
    public UserRole Role { get; set; }
    public long RoleId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; } //todo to hashed password
    public List<OptimizationProblem> OptimizationProblems { get; set; }
}
