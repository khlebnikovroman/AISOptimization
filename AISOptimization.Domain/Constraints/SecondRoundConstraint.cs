using AISOptimization.Domain.Common;
using AISOptimization.Domain.Parameters;


namespace AISOptimization.Domain.Constraints;

/// <summary>
/// Ограничение второго рода
/// </summary>
public class SecondRoundConstraint : Entity
{
    public FuncExpression ConstraintFunction { get; set; }
    public long ConstraintFunctionId { get; set; }

    /// <summary>
    /// Проверяет, удовлетвоено ли ограничение ывторого рода в точке
    /// </summary>
    /// <param name="point">Точка, в которой необходимо проверить удовлетворение условий</param>
    /// <returns>Результат проверки</returns>
    public bool IsSatisfied(Point point)
    {
        var variables = ConstraintFunction.GetVariables();

        foreach (var xVariable in point.DecisionVariables)
        {
            if (variables.Contains(xVariable.Key))
            {
                ConstraintFunction.Bind(xVariable.Key, xVariable.Value);
            }
        }

        return ConstraintFunction.Eval<bool>();
    }
}



