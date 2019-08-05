Imports System.Windows.Controls
Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class PhoneValidationRule
    Inherits ValidationRule

    Public Overrides Function Validate(ByVal value As Object, ByVal cultureInfo As CultureInfo) As ValidationResult
        If value.ToString().Length = 0 Then
            Return ValidationResult.ValidResult
        End If
        Dim regex As Regex = New Regex("^0[0-9]{2}(\s)?[0-9]{7}$")
        If regex.IsMatch(value.ToString()) Then
            Return ValidationResult.ValidResult
        Else
            Return New ValidationResult(False, "Số điện thoại không hợp lệ.")
        End If
    End Function
End Class
