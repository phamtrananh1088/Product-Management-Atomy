Imports System.Windows.Data

Public Class EnumBooleanConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim intVal As Int16 = CShort(parameter)
        Return Short.Equals(value, intVal)
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return If((CBool(value)), parameter, Binding.DoNothing)
    End Function
End Class
