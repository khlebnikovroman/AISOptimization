using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using AISOptimization.Core;
using AISOptimization.Utils;

using WPF.Base;


namespace AISOptimization.VIews.Pages;

public class CustomizableParameter : BaseVM
{
    public string ParameterName { get; set; }
    public bool IsVariable { get; set; }
}
public class SelectVariableParametersControlVM : BaseVM, IDataHolder, IResultHolder, IInteractionAware
{

    public ObservableCollection<CustomizableParameter> AllParameters { get; set; }

    private string _function;
    

    private OptimizationProblem _optimizationProblem;
    public object Data
    {
        get => _function;
        set
        {
            _function = (string) value;
            _optimizationProblem = new OptimizationProblem(_function);
            AllParameters = new ObservableCollection<CustomizableParameter>();

            foreach (var variable in _optimizationProblem.Function.Expression.getVariables())
            {
                AllParameters.Add(new CustomizableParameter(){IsVariable = false, ParameterName = variable});
            }
        }
    }
    public object Result { get; set; }
    public Action FinishInteraction { get; set; }
    private RelayCommand _onSelectCommand;

    public RelayCommand OnSelectCommand
    {
        get
        {
            return _onSelectCommand ??= new RelayCommand(o =>
            {
                var independentVariables = AllParameters
                                           .Where(p => p.IsVariable)
                                           .Select(p => new IndependentVariable()
                                                       {Key = p.ParameterName, FirstRoundRestriction = new FirstRoundRestriction()});
                var staticVariables = AllParameters
                                           .Where(p => !p.IsVariable)
                                           .Select(p => new StaticVariable()
                                                       {Key = p.ParameterName});

                _optimizationProblem.IndependentVariables = new ObservableCollection<IndependentVariable>(independentVariables);
                _optimizationProblem.StaticVariables = new ObservableCollection<StaticVariable>(staticVariables);
                Result = _optimizationProblem;
                FinishInteraction();
            });
        }
    }

    private RelayCommand _onCancelCommand;

    public RelayCommand OnCancelCommand
    {
        get
        {
            return _onCancelCommand ??= new RelayCommand(o =>
            {
                Result = null;
                FinishInteraction();
            });
        }
    }

}
