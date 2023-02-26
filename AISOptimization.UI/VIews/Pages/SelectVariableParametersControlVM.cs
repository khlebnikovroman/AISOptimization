using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using AISOptimization.Core;
using AISOptimization.Core.Collections;
using AISOptimization.Utils;

using WPF.Base;


namespace AISOptimization.VIews.Pages;

public class CustomizableParameter : BaseVM, IVariable
{
    public string Key { get; set; }
    public double Value { get; set; }
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
            AllParameters = new ObservableCollection<CustomizableParameter>();

            foreach (var variable in OptimizationProblem.GetVariables(_function))
            {
                AllParameters.Add(new CustomizableParameter(){IsVariable = false, Key = variable});
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
                                                       {Key = p.Key, FirstRoundRestriction = new FirstRoundRestriction()});
                var staticVariables = AllParameters
                                           .Where(p => !p.IsVariable)
                                           .Select(p => new StaticVariable()
                                                       {Key = p.Key});

                _optimizationProblem = new OptimizationProblem(_function, independentVariables, staticVariables);
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
