namespace AISOptimization.Core.OptimizationMethods;

public class ComplexBoxMethod : IOptimizationMethod
{
    public ComplexBoxMethod(OptimizationProblem optimizationProblem, double eps)
    {
        _optimizationProblem = optimizationProblem;
        _eps = eps;
        var varCount = _optimizationProblem.CreatePoint().X.Count;
        vertexCount = varCount <= 5 ? 2 * varCount : varCount + 1;
    }
    private Point _bestVertex;
    private Point _center;
    private List<Point> _complex;
    private double _eps;
    private OptimizationProblem _optimizationProblem;
    private Point _worstVertex;
    private int vertexCount;

    public Point GetBestXPoint()
    {
        _eps = 0.001;
        _complex = GetInitialComplex();

        // todo вычисление значений целевой функции
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

            while (!_optimizationProblem.IsSecondRoundRestrictionsSatisfied(newVertex)) // пока ограничение нарушено
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


        return _center; //return Ci
    }

    /// <summary>
    ///     Формирование исходного комплекса
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private List<Point> GetInitialComplex()
    {
        List<Point> GetRandomPoints()
        {
            var rnd = new Random();
            var complex = new List<Point>();

            for (var i = 0; i < vertexCount; i++)
            {
                var vertex = _optimizationProblem.CreatePoint();

                foreach (var variable in vertex.X)
                {
                    variable.Value = variable.FirstRoundRestriction.Min +
                                     rnd.NextDouble() * (variable.FirstRoundRestriction.Max - variable.FirstRoundRestriction.Min);
                }

                complex.Add(vertex);
            }

            return complex;
        }

        var allPoints = GetRandomPoints();

        while (allPoints.All(p => !_optimizationProblem.IsSecondRoundRestrictionsSatisfied(p)))
        {
            allPoints = GetRandomPoints();
        }

        var fixedPoints = allPoints.Where(p => _optimizationProblem.IsSecondRoundRestrictionsSatisfied(p)).ToList();
        var unfixedPoints = allPoints.Where(p => !fixedPoints.Contains(p)).ToList();

        while (unfixedPoints.Count != 0)
        {
            var unfixedPoint = unfixedPoints[0];

            while (_optimizationProblem.IsSecondRoundRestrictionsSatisfied(unfixedPoint) &&
                   unfixedPoint.X.All(p => p.FirstRoundRestriction.IsSatisfied(p.Value) == FirstRoundRestrictionSatisfactory.OK))
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
            for (var i = 0; i < vertex.X.Count; i++)
            {
                var variable = vertex.X[i];
                variable.Value = 0.5 * (variable.Value + 1 / fixedPoints.Count * fixedPoints.Sum(p => p.X[i].Value));
            }
        }
    }

    /// <summary>
    ///     Определение координат центра Комплекса с отброшенной «наихудшей» вершиной:
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private Point GetGravityCenterWithoutWorstVertex()
    {
        var center = _optimizationProblem.CreatePoint();

        for (var i = 0; i < center.X.Count; i++)
        {
            center.X[i].Value = 1.0 / (vertexCount-1) * (_complex.Sum(p => p.X[i].Value) - _worstVertex.X[i].Value);
        }

        return center;
    }

    /// <summary>
    ///     среднее расстояние от центра Комплекса до худшей (D) и лучшей (G) вершин
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private double GetB()
    {
        double sum = 0;

        for (var i = 0; i < _center.X.Count; i++)
        {
            sum += Math.Abs(_center.X[i].Value - _worstVertex.X[i].Value) + Math.Abs(_center.X[i].Value - _bestVertex.X[i].Value);
        }

        return (1.0 / (2 * _center.X.Count)) * sum;
    }

    private Point GetNewVertexInsteadWorst()
    {
        var newPoint = _optimizationProblem.CreatePoint();

        for (var i = 0; i < _center.X.Count; i++)
        {
            newPoint.X[i].Value = 2.3 * _center.X[i].Value - 1.3 * _worstVertex.X[i].Value;
        }

        foreach (var variable in newPoint.X)
        {
            var compareResult = variable.FirstRoundRestriction.IsSatisfied(variable.Value);

            switch (compareResult)
            {
                case FirstRoundRestrictionSatisfactory.LessThanMin:
                case FirstRoundRestrictionSatisfactory.LessOrEqualMin:
                    variable.Value = variable.FirstRoundRestriction.Min + _eps;

                    break;
                case FirstRoundRestrictionSatisfactory.OK:
                    break;
                case FirstRoundRestrictionSatisfactory.BiggerOrEqualMax:
                case FirstRoundRestrictionSatisfactory.BiggerThanMax:
                    variable.Value = variable.FirstRoundRestriction.Max - _eps;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return newPoint;
    }

    private void HalfShiftVertex1ToVertex2(Point vertex1, Point vertex2)
    {
        for (var i = 0; i < vertex1.X.Count; i++)
        {
            vertex1.X[i].Value = 0.5 * (vertex1.X[i].Value + vertex2.X[i].Value);
        }
    }
}

