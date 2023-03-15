using System.Text;


namespace AISOptimization.Domain.Parameters;

/// <summary>
/// Точка
/// </summary>
public class Point
{
    public Point()
    {
        DecisionVariables = new List<DecisionVariable>();
    }

    public List<DecisionVariable> DecisionVariables { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder();

        foreach (var variable in DecisionVariables)
        {
            builder.Append($"{variable.Key} = {variable.Value}; ");
        }

        return builder.ToString();
    }
}



