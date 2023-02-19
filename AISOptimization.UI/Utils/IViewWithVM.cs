namespace AISOptimization.Utils;

public interface IViewWithVM<ViewModelType> : IViewWithVM
{
    public ViewModelType ViewModel { get; set; }
}


public interface IViewWithVM
{
    public object ViewModelObject { get;  }
}
