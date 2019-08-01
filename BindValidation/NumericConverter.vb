Imports System.Windows.Data

Public Class NumericConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return value
    End Function

    Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        If TypeOf value Is String AndAlso CStr(value) = "" Then
            Return 0
        End If

        Return value
    End Function
End Class
