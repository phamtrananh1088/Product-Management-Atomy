Imports System.Windows.Controls
Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class FaxValidationRule
    Inherits ValidationRule

    Public Overrides Function Validate(ByVal value As Object, ByVal cultureInfo As CultureInfo) As ValidationResult
        Dim regex As Regex = New Regex("^(\+?\d{1,}(\s?|\-?)\d*(\s?|\-?)\(?\d{2,}\)?(\s?|\-?)\d{3,}\s?\d{3,})$")
        Dim match As Match = regex.Match(value.ToString())

        If match Is Nothing Then
            Return New ValidationResult(False, "Số fax không hợp lệ.")
        Else
            Return ValidationResult.ValidResult
        End If
    End Function
End Class
