using System;


namespace AISOptimization.Utils;

public interface IInteractionAware
{
    Action FinishInteraction { get; set; }
}
