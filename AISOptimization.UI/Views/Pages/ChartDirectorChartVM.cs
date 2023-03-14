using System;
using System.Collections.Generic;
using System.Linq;

using AISOptimization.Domain;
using AISOptimization.Utils.Dialog;
using AISOptimization.VMs;

using Mapster;

using WPF.Base;


namespace AISOptimization.Views.Pages;

//todo change to filescoped
public class Point3D
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
}


public class ChartDirectorChartVM : BaseVM, IInteractionAware, IDataHolder
{
    private RelayCommand _closeCommand;
    private OptimizationResultVM _optimizationResultVm;

    private List<Point3D> Points;

    public OptimizationResultVM OptimizationResultVM
    {
        get => _optimizationResultVm;
        set
        {
            _optimizationResultVm = value;
            CalculatePoints();
            OptimizationResultVmChanged?.Invoke(this, null);
        }
    }

    public DecisionVariableVM X { get; set; }
    public DecisionVariableVM Y { get; set; }
    public double[] DataX => Points.Select(x => x.X).ToArray();
    public double[] DataY => Points.Select(x => x.Y).ToArray();
    public double[] DataZ => Points.Select(x => x.Z).ToArray();

    public RelayCommand CloseCommand
    {
        get
        {
            return _closeCommand ??= new RelayCommand(o =>
            {
                FinishInteraction();
            });
        }
    }

    public object Data
    {
        get => OptimizationResultVM;
        set => OptimizationResultVM = (OptimizationResultVM) value;
    }

    public Action FinishInteraction { get; set; }

    private void CalculatePoints()
    {
        Points = new List<Point3D>();
        var problem = OptimizationResultVM.Adapt<OptimizationProblem>();
        var point = problem.CreatePoint();
        var countPerDimension = 200;
        X = problem.DecisionVariables[0].Adapt<DecisionVariableVM>();
        Y = problem.DecisionVariables[1].Adapt<DecisionVariableVM>();
        var xStep = (X.FirstRoundConstraint.Max - X.FirstRoundConstraint.Min) / countPerDimension;
        var yStep = (Y.FirstRoundConstraint.Max - Y.FirstRoundConstraint.Min) / countPerDimension;

        for (var x = X.FirstRoundConstraint.Min; x < X.FirstRoundConstraint.Max; x += xStep)
        {
            for (var y = Y.FirstRoundConstraint.Min; y < Y.FirstRoundConstraint.Max; y += xStep)
            {
                point.DecisionVariables[0].Value = x;
                point.DecisionVariables[1].Value = y;

                Points.Add(new Point3D
                               {X = x, Y = y, Z = problem.GetValueInPoint(point),});
            }
        }
    }

    public event EventHandler OptimizationResultVmChanged;
}


