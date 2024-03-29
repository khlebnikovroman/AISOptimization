﻿using System.Windows;
using System.Windows.Controls;


namespace AISOptimization.Controls;

public class LabeledControlPresenter : ContentControl
{
    /// <summary>
    ///     Property for <see cref="HeaderText" />.
    /// </summary>
    public static readonly DependencyProperty HeaderTextProperty =
        DependencyProperty.Register(nameof(HeaderText),
                                    typeof(string), typeof(LabeledControlPresenter), new PropertyMetadata(string.Empty));

    public static readonly DependencyProperty HeaderFontSizeProperty =
        DependencyProperty.Register(nameof(HeaderFontSize), typeof(double), typeof(LabeledControlPresenter), new PropertyMetadata(12d));

    public string HeaderText
    {
        get => (string) GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public double HeaderFontSize
    {
        get => (double) GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }
}

