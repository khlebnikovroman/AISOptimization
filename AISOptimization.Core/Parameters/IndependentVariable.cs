﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

using WPF.Base;


namespace AISOptimization.Core;

public class IndependentVariable:BaseVM, IVariable, ICloneable
{
    public string Key { get; set; }
    private double _value;

    public double Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            OnPropertyChanged();
        }
    }
    public FirstRoundRestriction FirstRoundRestriction { get; set; }

    public object Clone()
    {
        return new IndependentVariable() {Key = Key, Value = Value, FirstRoundRestriction = (FirstRoundRestriction) FirstRoundRestriction.Clone()};
    }
}
