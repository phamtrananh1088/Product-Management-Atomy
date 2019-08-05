Imports System.Windows.Controls
Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class EmailValidationRule
    Inherits ValidationRule

    Public Overrides Function Validate(ByVal value As Object, ByVal cultureInfo As CultureInfo) As ValidationResult
        If value.ToString().Length = 0 Then
            Return ValidationResult.ValidResult
        End If
        Dim regex As Regex = New Regex("^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")
        If regex.IsMatch(value.ToString()) Then
            Return ValidationResult.ValidResult
        Else
            Return New ValidationResult(False, "Email không hợp lệ.")
        End If
    End Function
End Class
