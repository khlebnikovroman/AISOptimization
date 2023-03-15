using AISOptimization.Domain.Constraints;
using AISOptimization.Domain.Parameters;


namespace AISOptimization.Domain.OptimizationMethods;

/// <summary>
/// Метод Бокса
/// </summary>
public class ComplexBoxMethod : IOptimizationMethod
{
    private readonly OptimizationProblem _optimizationProblem;
    private readonly int vertexCount;
    private Point _bestVertex;
    private Point _center;
    private List<Point> _complex;
    private double _eps;
    private Point _worstVertex;

    public ComplexBoxMethod(OptimizationProblem optimizationProblem, double eps)
    {
        _optimizationProblem = optimizationProblem;
        _eps = eps;
        var varCount = _optimizationProblem.CreatePoint().DecisionVariables.Count;
        vertexCount = varCount <= 5 ? 2 * varCount : varCount + 1;
    }

    /// <summary>
    /// Находит оптимальное значение функции
    /// </summary>
    /// <returns></returns>
    public Point SolveProblem()
    {
        _eps = 0.001;
        _complex = GetInitialComplex();

        do
        {
            _worstVertex = _complex.MinBy(p => _optimizationProblem.GetValueInPoint(p));
            _bestVertex = _complex.MaxBy(p => _optimizationProblem.GetValueInPoint(p));
            _center = GetGravityCenterWithoutWorstVertex();
            var b = GetB();

            if (b < _eps)
            {
                break;
            }

            var newVertex = GetNewVertexInsteadWorst();

            while (!_optimizationProblem.IsSecondRoundConstraintsSatisfied(newVertex)) // пока ограничение нарушено
            {
                HalfShiftVertex1ToVertex2(newVertex, _center);
            }

            while (_optimizationProblem.GetValueInPoint(newVertex) < _optimizationProblem.GetValueInPoint(_worstVertex))
            {
                HalfShiftVertex1ToVertex2(newVertex, _bestVertex);
            }

            _complex.Remove(_worstVertex);
            _complex.Add(newVertex);
        } while (GetB() >= _eps);


        return _center;
    }

    /// <summary>
    ///     Формирует исходного комплекса
    /// </summary>
    /// <returns>Исходный комплекс</returns>
    private List<Point> GetInitialComplex()
    {
        List<Point> GetRandomPoints()
        {
            var rnd = new Random();
            var complex = new List<Point>();

            for (var i = 0; i < vertexCount; i++)
            {
                var vertex = _optimizationProblem.CreatePoint();

                foreach (var variable in vertex.DecisionVariables)
                {
                    variable.Value = variable.FirstRoundConstraint.Min +
                                     rnd.NextDouble() * (variable.FirstRoundConstraint.Max - variable.FirstRoundConstraint.Min);
                }

                complex.Add(vertex);
            }

            return complex;
        }

        var allPoints = GetRandomPoints();

        while (allPoints.All(p => !_optimizationProblem.IsSecondRoundConstraintsSatisfied(p)))
        {
            allPoints = GetRandomPoints();
        }

        var fixedPoints = allPoints.Where(p => _optimizationProblem.IsSecondRoundConstraintsSatisfied(p)).ToList();
        var unfixedPoints = allPoints.Where(p => !fixedPoints.Contains(p)).ToList();

        while (unfixedPoints.Count != 0)
        {
            var unfixedPoint = unfixedPoints[0];

            while (!_optimizationProblem.IsSecondRoundConstraintsSatisfied(unfixedPoint)
                   || unfixedPoint.DecisionVariables.Any(p => p.FirstRoundConstraint.IsSatisfied(p.Value) != FirstRoundConstraintSatisfactory.OK))
            {
                ShiftVertex(unfixedPoint);
            }

            unfixedPoints.Remove(unfixedPoint);
            fixedPoints.Add(unfixedPoint);
        }

        return fixedPoints;

        // Смещение вершины к центру вершин комплекса
        void ShiftVertex(Point vertex)
        {
            for (var i = 0; i < vertex.DecisionVariables.Count; i++)
            {
                var variable = vertex.DecisionVariables[i];
                variable.Value = 0.5 * (variable.Value + 1.0 / fixedPoints.Count * fixedPoints.Sum(p => p.DecisionVariables[i].Value));
            }
        }
    }

    /// <summary>
    ///     Определение координат центра Комплекса с отброшенной «наихудшей» вершиной:
    /// </summary>
    /// <returns>Координаты</returns>
    private Point GetGravityCenterWithoutWorstVertex()
    {
        var center = _optimizationProblem.CreatePoint();

        for (var i = 0; i < center.DecisionVariables.Count; i++)
        {
            center.DecisionVariables[i].Value = 1.0 / (vertexCount - 1) *
                                                (_complex.Sum(p => p.DecisionVariables[i].Value) - _worstVertex.DecisionVariables[i].Value);
        }

        return center;
    }

    /// <summary>
    ///     Находит среднее расстояние от центра Комплекса до худшей (D) и лучшей (G) вершин
    /// </summary>
    /// <returns>Среднее расстояние от центра Комплекса до худшей (D) и лучшей (G) вершин</returns>
    private double GetB()
    {
        double sum = 0;

        for (var i = 0; i < _center.DecisionVariables.Count; i++)
        {
            sum += Math.Abs(_center.DecisionVariables[i].Value - _worstVertex.DecisionVariables[i].Value) +
                   Math.Abs(_center.DecisionVariables[i].Value - _bestVertex.DecisionVariables[i].Value);
        }

        return 1.0 / (2 * _center.DecisionVariables.Count) * sum;
    }

    /// <summary>
    /// Получает новую точку взамен наихудшей
    /// </summary>
    /// <returns>Новая точка</returns>
    private Point GetNewVertexInsteadWorst()
    {
        var newPoint = _optimizationProblem.CreatePoint();

        for (var i = 0; i < _center.DecisionVariables.Count; i++)
        {
            newPoint.DecisionVariables[i].Value = 2.3 * _center.DecisionVariables[i].Value - 1.3 * _worstVertex.DecisionVariables[i].Value;
        }

        foreach (var variable in newPoint.DecisionVariables)
        {
            var compareResult = variable.FirstRoundConstraint.IsSatisfied(variable.Value);

            switch (compareResult)
            {
                case FirstRoundConstraintSatisfactory.LessThanMin:
                    variable.Value = variable.FirstRoundConstraint.Min;

                    break;
                case FirstRoundConstraintSatisfactory.LessOrEqualMin:
                    variable.Value = variable.FirstRoundConstraint.Min + _eps;

                    break;
                case FirstRoundConstraintSatisfactory.OK:
                    break;
                case FirstRoundConstraintSatisfactory.BiggerOrEqualMax:
                    variable.Value = variable.FirstRoundConstraint.Max;

                    break;
                case FirstRoundConstraintSatisfactory.BiggerThanMax:
                    variable.Value = variable.FirstRoundConstraint.Max - _eps;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return newPoint;
    }

    /// <summary>
    /// Наполовину сдвигает <paramref name="vertex1" к <paramref name="vertex2"/>/>
    /// </summary>
    /// <param name="vertex1"></param>
    /// <param name="vertex2"></param>
    private void HalfShiftVertex1ToVertex2(Point vertex1, Point vertex2)
    {
        for (var i = 0; i < vertex1.DecisionVariables.Count; i++)
        {
            vertex1.DecisionVariables[i].Value = 0.5 * (vertex1.DecisionVariables[i].Value + vertex2.DecisionVariables[i].Value);
        }
    }
}




