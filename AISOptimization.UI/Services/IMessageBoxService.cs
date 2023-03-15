using System.Threading.Tasks;
using System.Windows;

using Microsoft.EntityFrameworkCore.Query.Internal;


namespace AISOptimization.Services;

/// <summary>
/// Интерфейс для сервиса вызова MessageBox
/// </summary>
public interface IMessageBoxService
{
    /// <summary>
    /// Показать MessageBox
    /// </summary>
    /// <param name="messageBoxText">Текст для MessageBox</param>
    /// <returns>Результат работы MessageBox</returns>
    public Task<MessageBoxResult> Show(string messageBoxText);

    /// <summary>
    /// Показать MessageBox
    /// </summary>
    /// <param name="messageBoxText">Текст для MessageBox</param>
    /// <param name="caption">Заголовок </param>
    /// <returns></returns>
    public Task<MessageBoxResult> Show(string messageBoxText, string caption);
    /// <summary>
    /// Показать MessageBox
    /// </summary>
    /// <param name="messageBoxText">Отображаемый текст</param>
    /// <param name="caption">Заголовок</param>
    /// <param name="button">Задаваемые кнопки</param>
    /// <returns>Результат выбора пользователя</returns>
    public Task<MessageBoxResult> Show(string messageBoxText, string caption, MessageBoxButton button);
    /// <summary>
    /// Показать MessageBox
    /// </summary>
    /// <param name="messageBoxText">Отображаемый текст</param>
    /// <param name="button">Задаваемые кнопки</param>
    /// <returns>Результат выбора пользователя</returns>
    public Task<MessageBoxResult> Show(string messageBoxText, MessageBoxButton button);
    /// <summary>
    /// Показать MessageBox
    /// </summary>
    /// <param name="window">Окно-владелец MessageBox</param>
    /// <param name="messageBoxText">Отображаемый текст</param>
    /// <returns>Результат выбора пользователя</returns>
    public Task<MessageBoxResult> Show(Window window, string messageBoxText);

    /// <summary>
    /// Показать MessageBox
    /// </summary>
    /// <param name="window">Окно-владелец MessageBox</param>
    /// <param name="messageBoxText">Отображаемый текст</param>
    /// <param name="caption">Заголовок</param>
    /// <returns>Результат выбора пользователя</returns>
    public Task<MessageBoxResult> Show(Window window, string messageBoxText, string caption);

    /// <summary>
    /// Показать MessageBox
    /// </summary>
    /// <param name="window">Окно-владелец MessageBox</param>
    /// <param name="messageBoxText">Отображаемый текст</param>
    /// <param name="caption">Заголовок</param>
    /// <param name="button">Задаваемые кнопки</param>
    /// <returns>Результат выбора пользователя</returns>
    public Task<MessageBoxResult> Show(Window window, string messageBoxText, string caption, MessageBoxButton button);
}
