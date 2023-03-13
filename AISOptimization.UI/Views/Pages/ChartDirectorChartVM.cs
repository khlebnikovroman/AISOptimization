using System;
using System.Collections.Generic;
using System.Linq;

using AISOptimization.Core;
using AISOptimization.UI.VM.VMs;
using AISOptimization.Utils;

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
public class ChartDirectorChartVM: BaseVM, IInteractionAware, IDataHolder
{
    private OptimizationResultVM _optimizationResultVm;

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

    public IndependentVariableVM X { get; set; }
    public IndependentVariableVM Y { get; set; }

    private List<Point3D> Points;
    public double[] DataX => Points.Select(x => x.X).ToArray();
    public double[] DataY => Points.Select(x => x.Y).ToArray();
    public double[] DataZ => Points.Select(x => x.Z).ToArray();
    private void CalculatePoints()
    {
        Points = new List<Point3D>();
        var problem = ((OptimizationProblemVM) OptimizationResultVM).Adapt<OptimizationProblem>();
        var point = problem.CreatePoint();
        var countPerDimension = 200;
        X = problem.VectorX[0].Adapt<IndependentVariableVM>();
        Y = problem.VectorX[1].Adapt<IndependentVariableVM>();
        var xStep = (X.FirstRoundRestriction.Max - X.FirstRoundRestriction.Min)/countPerDimension;
        var yStep = (Y.FirstRoundRestriction.Max - Y.FirstRoundRestriction.Min)/countPerDimension;
        for (double x = X.FirstRoundRestriction.Min; x < X.FirstRoundRestriction.Max; x+=xStep)
        {
            for (double y = Y.FirstRoundRestriction.Min; y < Y.FirstRoundRestriction.Max; y+=xStep)
            {
                point.X[0].Value = x;
                point.X[1].Value = y;
                Points.Add(new Point3D(){X=x,Y=y,Z=problem.GetValueInPoint(point)});
            }
        }
    }
    
    public event EventHandler OptimizationResultVmChanged;
    public Action FinishInteraction { get; set; }
    public object Data
    {
        get => OptimizationResultVM;
        set => OptimizationResultVM =(OptimizationResultVM)value;
    }

    private RelayCommand _closeCommand;

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

}
