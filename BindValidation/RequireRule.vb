Imports System.Windows.Controls
Imports System.Globalization

Public Class RequireRule
    Inherits ValidationRule

    Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult

        Dim strValue As String = Convert.ToString(value)

        If String.IsNullOrEmpty(strValue) Then
            Return New ValidationResult(False, "Thông tin này là bắt buộc.")
        End If

        Return ValidationResult.ValidResult
    End Function
End Class
