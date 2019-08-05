Imports System.Windows.Data

Public Class CodeConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        If value IsNot Nothing Then
            Dim s = value.ToString().Trim()
            If s.Length = 0 Then
                Return s
            ElseIf s.Length < 8 Then
                Dim lead As String = New String("0", 8 - s.Length)
                s = lead + s
                Return s
            Else
                Return s.Substring(0, 8)
            End If
        Else
            Return value
        End If
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return value
    End Function
End Class
