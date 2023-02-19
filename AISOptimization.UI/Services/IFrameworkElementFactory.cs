using System.Windows;


namespace AISOptimization.Services;

public interface IFrameworkElementFactory
{
    public T Create<T>() where T : FrameworkElement;
}
