using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

using AISOptimization.Core;
using AISOptimization.Core.Parameters;
using AISOptimization.Core.Restrictions;
using AISOptimization.UI.VM.VMs;
using AISOptimization.Utils;

using WPF.Base;


namespace AISOptimization.Views.Pages;

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
    

    private OptimizationProblemVM _optimizationProblem;
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
                                           .Select(p => new IndependentVariableVM()
                                                       {Key = p.Key, FirstRoundRestriction = new FirstRoundRestrictionVM()});
                var staticVariables = AllParameters
                                           .Where(p => !p.IsVariable)
                                           .Select(p => new StaticVariableVM()
                                                       {Key = p.Key});

                _optimizationProblem = new OptimizationProblemVM()
                {
                    FunctionExpression = new FunctionExpressionVM(){Formula = _function},
                    IndependentVariables = new ObservableCollection<IndependentVariableVM>(independentVariables),
                    StaticVariables = new ObservableCollection<StaticVariableVM>(staticVariables)
                };
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
