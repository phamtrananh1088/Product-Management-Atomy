Imports System.Windows.Controls
Imports System.Globalization

Public Class NumericValidationRule
    Inherits ValidationRule

    Public Property TypeName As String
    Public Overrides Function Validate(value As Object, cultureInfo As CultureInfo) As ValidationResult

        Dim strValue As String = Convert.ToString(value)

        If String.IsNullOrEmpty(strValue) Then
            Return ValidationResult.ValidResult
        End If

        Dim canConvert As Boolean = False
        Select Case TypeName
            Case "Boolean"
                Dim boolVal As Boolean = False
                canConvert = Boolean.TryParse(strValue, boolVal)
                If Not canConvert Then
                    Return New ValidationResult(False, "Đầu vào phải là <Đúng> hoặc <Sai>.")
                End If
            Case "Int16"
                Dim intVal As Int16 = 0
                canConvert = Int16.TryParse(strValue, intVal)
                If Not canConvert Then
                    Return New ValidationResult(False, "Đầu vào phải là số nhỏ.")
                End If
            Case "Int32"
                Dim intVal As Integer = 0
                canConvert = Integer.TryParse(strValue, intVal)
                If Not canConvert Then
                    Return New ValidationResult(False, "Đầu vào phải là số.")
                End If
            Case "Double"
                Dim doubleVal As Double = 0
                canConvert = Double.TryParse(strValue, doubleVal)
                If Not canConvert Then
                    Return New ValidationResult(False, "Đầu vào phải là số (có phần thập phân).")
                End If
            Case "Int64"
                Dim longVal As Long = 0
                canConvert = Long.TryParse(strValue, longVal)
                If Not canConvert Then
                    Return New ValidationResult(False, "Đầu vào phải là số (có thể nhận giá trị lớn)")
                End If
            Case "Decimal"
                Dim decVal As Decimal = 0
                canConvert = Decimal.TryParse(strValue, decVal)
                If Not canConvert Then
                    Return New ValidationResult(False, "Đầu vào phải là số tiền.")
                End If
            Case Else
                Throw New InvalidCastException("{ValidationType.Name} is not supported")
        End Select

        Return ValidationResult.ValidResult
    End Function

End Class