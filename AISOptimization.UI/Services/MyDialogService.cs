using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

using AISOptimization.Utils.Dialog;

using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;


namespace AISOptimization.Services;

// todo wait until new version of WPF UI release and than replace it
/// <summary>
/// Сервис для отображения диалоговых окон
/// </summary>
public class MyDialogService
{
    private readonly IDialogService _dialogService;
    private readonly IFrameworkElementFactory _elementFactory;


    public MyDialogService(IDialogService dialogService, IFrameworkElementFactory elementFactory)
    {
        _dialogService = dialogService;
        _elementFactory = elementFactory;
    }

    /// <summary>
    /// Показать диалоговое окно
    /// </summary>
    /// <param name="data">Передаваемые данные</param>
    /// <typeparam name="T">Тип элемента для отображения в диалоговом окне</typeparam>
    /// <returns>Результат работы диалогового окна</returns>
    public Task<object?> ShowDialog<T>(object data = null) where T : FrameworkElement, IDialogAware
    {
        var tcs = new TaskCompletionSource<object?>();
        var e = _elementFactory.Create<T>();
        var dc = _dialogService.GetDialogControl();
        dc.DialogHeight = e.Height;
        dc.DialogWidth = e.Width;
        dc.Content = e;
        dc.Footer = e.Footer;

        //dc.Title = e.Title;
        var viewModel = e.ViewModelObject;

        if (viewModel is IDataHolder dataHolder)
        {
            dataHolder.Data = data;
        }

        if (viewModel is IInteractionAware interactionAware)
        {
            interactionAware.FinishInteraction = () =>
            {
                Debug.WriteLine("Диалог скрылся");
                dc.Hide();
                dc.Closed -= OnClosed;
            };
        }

        dc.Closed += OnClosed;

        void OnClosed(Dialog sender, RoutedEventArgs args)
        {
            if (viewModel is IResultHolder resultHolder)
            {
                Debug.WriteLine("Вернулся результат");
                tcs.SetResult(resultHolder.Result);
            }
        }

        dc.Show();

        return tcs.Task;
    }
}


