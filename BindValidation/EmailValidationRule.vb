Imports System.Windows.Controls
Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class EmailValidationRule
    Inherits ValidationRule

    Public Overrides Function Validate(ByVal value As Object, ByVal cultureInfo As CultureInfo) As ValidationResult
        Dim regex As Regex = New Regex("^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")
        Dim match As Match = regex.Match(value.ToString())

        If match Is Nothing Then
            Return New ValidationResult(False, "Email không hợp lệ.")
        Else
            Return ValidationResult.ValidResult
        End If
    End Function
End Class
