using System.Text;


namespace AISOptimization.Core.Parameters;

//todo delete
public class Point
{
    public Point()
    {
        X = new List<IndependentVariable>();
    }

    public List<IndependentVariable> X { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder();

        foreach (var variable in X)
        {
            builder.Append($"{variable.Key} = {variable.Value}; ");
        }

        return builder.ToString();
    }
}

