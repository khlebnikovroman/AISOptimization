using System.ComponentModel.DataAnnotations.Schema;

using AISOptimization.Domain.Common;

using org.matheval;


namespace AISOptimization.Domain;

public class FuncExpression : Entity
{
    [NotMapped]
    private readonly Expression _expression;

    private string _formula;
    private List<string> _notAllowedFunctions;

    public FuncExpression(string formula)
    {
        _formula = formula;
        _expression = new Expression(formula);
    }

    public string Formula
    {
        get => _formula;
        set
        {
            _formula = value;
            _expression.SetFomular(_formula);
        }
    }

    public List<string> GetVariables()
    {
        return _expression.getVariables();
    }

    public void Bind(string key, object value)
    {
        _expression.Bind(key, value);
    }

    public T Eval<T>()
    {
        return _expression.Eval<T>();
    }

    public void DisableFunction(string functionName)
    {
        _expression.DisableFunction(functionName);
    }

    public void DisableFunction(string[] functionName)
    {
        _expression.DisableFunction(functionName);
    }

    public List<string> GetError()
    {
        return _expression.GetError();
    }
}



