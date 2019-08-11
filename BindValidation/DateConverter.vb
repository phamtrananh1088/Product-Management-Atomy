Imports System.Windows.Data
Imports System.Text.RegularExpressions

Public Class DateConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return value
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Dim n As Date = Date.Now
        Dim dteVal As Date = Nothing
        Dim regexMonth As Regex = New Regex("^[0-9]{1,2}$")
        If regexMonth.IsMatch(value.ToString()) Then
            Dim intVal As Int16 = 0
            intVal = CShort(value.ToString())
            If intVal >= 1 AndAlso intVal <= 12 Then
                Return New Date(n.Year, intVal, 1).ToString("yyyy/MM/dd")
            Else
                Return n.ToString("yyyy/MM/dd")
            End If
        Else
            Dim regex As Regex = New Regex("^[0-9]{4}\/[0-9]{1,2}\/[0-9]{1,2}$")
            If Regex.IsMatch(value.ToString()) Then
                If Date.TryParse(value.ToString(), dteVal) Then
                    Return dteVal.ToString("yyyy/MM/dd")
                Else
                    Return n.ToString("yyyy/MM/dd")
                End If
            Else
                Return n.ToString("yyyy/MM/dd")
            End If
        End If

       
    End Function
End Class
