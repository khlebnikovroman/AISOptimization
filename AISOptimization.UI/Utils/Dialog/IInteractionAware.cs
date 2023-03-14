using System;


namespace AISOptimization.Utils.Dialog;

public interface IInteractionAware
{
    Action FinishInteraction { get; set; }
}


