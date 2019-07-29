Imports System.Windows.Data
Imports System.Globalization

Public Class NameConverter
    Implements IMultiValueConverter

    Public Function Convert(values() As Object, targetType As Type, parameter As Object, culture As Globalization.CultureInfo) As Object Implements IMultiValueConverter.Convert
        Dim name As String

        Select Case CStr(parameter)
            Case "FormatLastFirst"
                name = values(1).ToString + ", " + values(0).ToString
            Case Else
                name = values(0).ToString + " " + values(1).ToString
        End Select

        Return name
    End Function

    Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object, culture As Globalization.CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
        Dim splitValues As String() = (CStr(value)).Split(" ")
        Return splitValues
    End Function
End Class
