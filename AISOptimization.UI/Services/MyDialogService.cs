﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using AISOptimization.Utils;
using AISOptimization.VIews.Pages;

using Wpf.Ui.Contracts;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Window;
using Wpf.Ui.Services;


namespace AISOptimization.Services;

// todo wait until new version of WPF UI release and than replace it
public class MyDialogService 
{
    private readonly IDialogService _dialogService;
    private readonly IFrameworkElementFactory _elementFactory;


    public MyDialogService(IDialogService dialogService, IFrameworkElementFactory elementFactory)
    {
        _dialogService = dialogService;
        _elementFactory = elementFactory;
    }

    public Task<object?> ShowDialog<T>(object data = null) where T: FrameworkElement, IViewWithVM
    {
        var tcs = new TaskCompletionSource<object?>();
        var e = _elementFactory.Create<T>();
        var dc = _dialogService.GetDialogControl();
        //dc.DialogHeight = e.Height;
        //dc.DialogWidth = e.Width;
        dc.Content = e;
        dc.Footer = new UserControl();
        
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
            };
        }
        dc.Show();

        
        dc.Closed += (sender, args) =>
        {
            if (viewModel is IResultHolder resultHolder)
            {
                Debug.WriteLine("Вернулся результат");
                tcs.SetResult(resultHolder.Result);
            }
        };

        
        return tcs.Task;
    }




}