Imports System.Windows.Data

Public Class EmailConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If value IsNot Nothing Then
            Return "mailto:" + value.ToString()
        Else
            Dim email As String = ""
            Return email
        End If
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Dim email As Uri = New Uri(CStr(value))
        Return email
    End Function
End Class
