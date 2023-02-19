using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

using AISOptimization.Utils;


namespace AISOptimization.VIews.Pages;

public class CustomizableParameter
{
    public string ParameterName { get; set; }
    public bool IsVariable { get; set; }
}
public class SelectVariableParametersControlVM : BaseVM, IDataHolder, IResultHolder, IInteractionAware
{

    public ObservableCollection<CustomizableParameter> AllParameters { get; set; }

    private string function;
    
    public object Data { get; set; }
    public object Result { get; set; }
    public Action FinishInteraction { get; set; }
    private RelayCommand _onSelectCommand;

    public RelayCommand OnSelectCommand
    {
        get
        {
            return _onSelectCommand ??= new RelayCommand(o =>
            {
                Result = AllParameters;
                FinishInteraction();
            });
        }
    }

    private RelayCommand _onCancelCommand;

    public RelayCommand OnCancelCommand
    {
        get
        {
            return _onCancelCommand ??= new RelayCommand(o =>
            {
                Result = null;
                FinishInteraction();
            });
        }
    }

}
