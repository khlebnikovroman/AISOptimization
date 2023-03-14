using System.Windows;

using Autofac;


namespace AISOptimization.Services;

public class FrameworkElementFactory : IFrameworkElementFactory
{
    private readonly IComponentContext _container;

    public FrameworkElementFactory(IComponentContext container)
    {
        _container = container;
    }

    public T Create<T>() where T : FrameworkElement
    {
        return _container.Resolve<T>();
    }
}


