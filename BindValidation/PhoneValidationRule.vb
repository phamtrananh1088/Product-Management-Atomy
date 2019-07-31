Imports System.Windows.Controls
Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class PhoneValidationRule
    Inherits ValidationRule

    Public Overrides Function Validate(ByVal value As Object, ByVal cultureInfo As CultureInfo) As ValidationResult
        Dim regex As Regex = New Regex("^0[0-9]{2}(\s)?[0-9]{7}$")
        Dim match As Match = regex.Match(value.ToString())

        If match Is Nothing Then
            Return New ValidationResult(False, "Số điện thoại không hợp lệ.")
        Else
            Return ValidationResult.ValidResult
        End If
    End Function
End Class
