Imports System.Windows.Controls
Imports System.Globalization
Imports System.Text.RegularExpressions

Public Class CodeValidationRule
    Inherits ValidationRule

    Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult
        If value.ToString().Length = 0 Then
            Return ValidationResult.ValidResult
        End If
        Dim regex As Regex = New Regex("^[0-9]{8}$")
        If regex.IsMatch(value.ToString()) Then
            Return ValidationResult.ValidResult
        Else
            Return New ValidationResult(False, "Mã chỉ nhập được số từ 1 đến 9. Các số 0 sẽ tự động điền thêm cho đủ 8 ký tự.")
        End If
    End Function

End Class