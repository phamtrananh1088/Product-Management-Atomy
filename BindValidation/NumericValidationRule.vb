Imports System.Windows.Controls
Imports System.Globalization

Public Class NumericValidationRule
    Inherits ValidationRule

    Public Property TypeName As String
    Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult

        Dim strValue As String = Convert.ToString(value)

        If String.IsNullOrEmpty(strValue) Then
            Return New ValidationResult(False, "Thông tin này là bắt buộc.")
        End If

        Dim canConvert As Boolean = False
        Select Case TypeName
            Case "Boolean"
                Dim boolVal As Boolean = False
                canConvert = Boolean.TryParse(strValue, boolVal)
                If Not canConvert Then
                    Return New ValidationResult(False, "Input should be type of boolean")
                End If
            Case "Int32"
                Dim intVal As Integer = 0
                canConvert = Integer.TryParse(strValue, intVal)
                If Not canConvert Then
                    Return New ValidationResult(False, "Input should be type of Int32")
                End If
            Case "Double"
                Dim doubleVal As Double = 0
                canConvert = Double.TryParse(strValue, doubleVal)
                If Not canConvert Then
                    Return New ValidationResult(False, "Input should be type of Double")
                End If
            Case "Int64"
                Dim longVal As Long = 0
                canConvert = Long.TryParse(strValue, longVal)
                If Not canConvert Then
                    Return New ValidationResult(False, "Input should be type of Int64")
                End If
            Case "Decimal"
                Dim decVal As Decimal = 0
                canConvert = Decimal.TryParse(strValue, decVal)
                If Not canConvert Then
                    Return New ValidationResult(False, "Bạn phải nhập số tiền.")
                End If
            Case "LeadingZeroNumber"
                Dim intVal As Integer = 0
                canConvert = Integer.TryParse(strValue, intVal)
                If Not canConvert Then
                    Return New ValidationResult(False, "Bạn phải nhập số, các số 0 sẽ tự động thêm vào trước.")
                End If
            Case Else
                Throw New InvalidCastException("{ValidationType.Name} is not supported")
        End Select

        Return ValidationResult.ValidResult
    End Function

End Class