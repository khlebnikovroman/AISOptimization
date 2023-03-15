using System.Windows;


namespace AISOptimization.Services;

/// <summary>
/// Интерфейс для фабрики создания элементов WPF
/// </summary>
public interface IFrameworkElementFactory
{
    /// <summary>
    /// Сооздает элемент
    /// </summary>
    /// <typeparam name="T">Тип элемента для создания</typeparam>
    /// <returns>Созданный элемент</returns>
    public T Create<T>() where T : FrameworkElement;
}


