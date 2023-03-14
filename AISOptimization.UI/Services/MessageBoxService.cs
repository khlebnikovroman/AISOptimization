using System;
using System.Windows;
using System.Windows.Controls;

using Button = Wpf.Ui.Controls.Button;
using MessageBox = Wpf.Ui.Controls.MessageBox;


namespace AISOptimization.Services;

public class MessageBoxService : IMessageBoxService
{
    public MessageBoxResult Show(string messageBoxText)
    {
        return Show(window:null, messageBoxText);
    }

    public MessageBoxResult Show(string messageBoxText, string caption)
    {
        return Show(null, messageBoxText, caption);
    }

    public MessageBoxResult Show(Window window, string messageBoxText)
    {
        return Show(window, messageBoxText, null);
    }

    public MessageBoxResult Show(Window window, string messageBoxText, string caption)
    {
        return Show(null, messageBoxText, caption, MessageBoxButton.OK);
    }

    public MessageBoxResult Show(Window window, string messageBoxText, string caption, MessageBoxButton button)
    {
        var mb = new MessageBox();
        mb.Owner = window;
        mb.Title = caption;
        mb.Content = messageBoxText;
        var result = MessageBoxResult.None;

        var footer = new StackPanel
            {HorizontalAlignment = HorizontalAlignment.Right,};

        void OnButtonClicked(MessageBoxResult buttonResult)
        {
            result = buttonResult;
            mb.Close();
        }

        var OkButton = new Button
            {Content = "ОК", Margin = new Thickness(5, 5, 5, 5),};

        var CancelButton = new Button
            {Content = "Отмена", Margin = new Thickness(5, 5, 5, 5),};

        var YesButton = new Button
            {Content = "Да", Margin = new Thickness(5, 5, 5, 5),};

        var NoButton = new Button
            {Content = "Нет", Margin = new Thickness(5, 5, 5, 5),};

        OkButton.Click += (sender, args) => OnButtonClicked(MessageBoxResult.OK);
        CancelButton.Click += (sender, args) => OnButtonClicked(MessageBoxResult.Cancel);
        YesButton.Click += (sender, args) => OnButtonClicked(MessageBoxResult.Yes);
        NoButton.Click += (sender, args) => OnButtonClicked(MessageBoxResult.No);

        switch (button)
        {
            case MessageBoxButton.OK:
                footer.Children.Add(OkButton);

                break;
            case MessageBoxButton.OKCancel:
                footer.Children.Add(CancelButton);
                footer.Children.Add(OkButton);

                break;
            case MessageBoxButton.YesNoCancel:
                footer.Children.Add(CancelButton);
                footer.Children.Add(NoButton);
                footer.Children.Add(YesButton);

                break;
            case MessageBoxButton.YesNo:
                footer.Children.Add(NoButton);
                footer.Children.Add(YesButton);

                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(button), button, null);
        }

        mb.Footer = footer;
        mb.Show();

        return result;
    }
}

