using System;
using System.Collections.ObjectModel;
using System.Linq;

using AISOptimization.Domain;
using AISOptimization.Domain.Parameters;
using AISOptimization.Utils.Dialog;
using AISOptimization.VMs;

using WPF.Base;

using Constant = AISOptimization.VMs.Constant;


namespace AISOptimization.Views.Pages;

public class CustomizableParameter : BaseVM, IVariable
{
    public bool IsVariable { get; set; }
    public string Key { get; set; }
    public double Value { get; set; }
}


public class SelectVariableParametersControlVM : BaseVM, IDataHolder, IResultHolder, IInteractionAware
{
    private string _function;

    private RelayCommand _onCancelCommand;
    private RelayCommand _onSelectCommand;


    private OptimizationProblemVM _optimizationProblem;

    public ObservableCollection<CustomizableParameter> AllParameters { get; set; }

    public RelayCommand OnSelectCommand
    {
        get
        {
            return _onSelectCommand ??= new RelayCommand(o =>
            {
                var decisionVariables = AllParameters
                                        .Where(p => p.IsVariable)
                                        .Select(p => new DecisionVariableVM
                                                    {Key = p.Key, FirstRoundConstraint = new FirstRoundConstraintVM(),});

                var constants = AllParameters
                                .Where(p => !p.IsVariable)
                                .Select(p => new Constant
                                            {Key = p.Key,});

                _optimizationProblem = new OptimizationProblemVM
                {
                    ObjectiveFunction = new FunctionExpressionVM
                        {Formula = _function,},
                    DecisionVariables = new ObservableCollection<DecisionVariableVM>(decisionVariables),
                    Constants = new ObservableCollection<Constant>(constants),
                };

                Result = _optimizationProblem;
                FinishInteraction();
            });
        }
    }

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

    public object Data
    {
        get => _function;
        set
        {
            _function = (string) value;
            AllParameters = new ObservableCollection<CustomizableParameter>();

            foreach (var variable in OptimizationProblem.GetVariables(_function))
            {
                AllParameters.Add(new CustomizableParameter
                                      {IsVariable = false, Key = variable,});
            }
        }
    }

    public Action FinishInteraction { get; set; }
    public object Result { get; set; }
}


