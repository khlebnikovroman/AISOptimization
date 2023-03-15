using System.Windows;

using Microsoft.EntityFrameworkCore.Query.Internal;


namespace AISOptimization.Services;

public interface IMessageBoxService
{
    public MessageBoxResult Show(string messageBoxText);

    public MessageBoxResult Show(string messageBoxText, string caption);
    public MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button);
    public MessageBoxResult Show(string messageBoxText,  MessageBoxButton button);
    public MessageBoxResult Show(Window window, string messageBoxText);

    public MessageBoxResult Show(Window window, string messageBoxText, string caption);

    public MessageBoxResult Show(Window window, string messageBoxText, string caption, MessageBoxButton button);
}
