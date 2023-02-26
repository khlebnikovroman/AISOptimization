using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;


namespace AISOptimization.Utils;

public class EnumDisplayNameConverter : IValueConverter
{
    private string GetEnumDescription(Enum enumObj)
    {
        FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

        object[] attribArray = fieldInfo.GetCustomAttributes(false);

        if (attribArray.Length == 0)
        {
            return enumObj.ToString();
        }
        else
        {
            DescriptionAttribute attrib = attribArray[0] as DescriptionAttribute;
            return attrib.Description;
        }
    }

    object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Enum myEnum = (Enum)value;
        string description = GetEnumDescription(myEnum);
        return description;
    }

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }
    
}
