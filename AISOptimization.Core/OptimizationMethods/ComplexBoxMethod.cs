namespace AISOptimization.Core.OptimizationMethods;

public class ComplexBoxMethod
{
    private bool IsSecondRoundRestrictionsSatisfied()
    {
        throw new NotImplementedException();
    }
    private Point GETBEST()
    {
        var eps = 0.001;
        var complex = GetInitialComplex();
        // todo вычисление значений целевой функции
        do
        {
            var worstVertex = new Point();
            var bestVertex = new Point();
            var Ci = GetGravityCenterWithoutWorstVertex();

            if (GetB() < eps)
            {
                break;
            }
            var newVertex = GetNewVertexInsteadWorst();

            while (true) // пока ограничение нарушено
            {
                HalfShiftVertexToVertex(newVertex,Ci);
            }

            // while (newvertex.F<worstVertex.F)
            // {
            //     HalfShiftVertexToVertex(newvertex, bestVertex);
            // }
        } while (GetB()>=eps);


        return new Point(); //return Ci

    }
    /// <summary>
    /// Формирование исходного комплекса
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private List<Point> GetInitialComplex()
    {
        // Смещение вершины к центру вершин комплекса
        Point ShiftVertex(Point vertex)
        {
            throw new NotImplementedException();
        }
        
        throw new NotImplementedException();
    }
    /// <summary>
    /// Определение координат центра Комплекса с отброшенной «наихудшей» вершиной:
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private Point GetGravityCenterWithoutWorstVertex()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///  среднее расстояние от центра Комплекса до худшей (D) и лучшей (G) вершин
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private double GetB()
    {
        throw new NotImplementedException();
    }

    private Point GetNewVertexInsteadWorst()
    {
        throw new NotImplementedException();
    }

    private void HalfShiftVertexToVertex(Point vertex1, Point vertex2)
    {
        throw new NotImplementedException();
    }
    
}
