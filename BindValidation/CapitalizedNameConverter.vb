Imports System.Windows.Data
Imports System.Linq

<ValueConversion(GetType(String), GetType(String))>
Public Class CapitalizedNameConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return value
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        If TypeOf value Is String Then
            Dim castValue = CStr(value)
            Dim arName As New List(Of String)
            For Each item As String In castValue.Split(New Char() {" "}, System.StringSplitOptions.RemoveEmptyEntries)
                arName.Add(Char.ToUpper(item(0)) & item.Substring(1))
            Next
            Return arName.Aggregate(Function(a As String, b As String)
                                        Return a + " " + b
                                    End Function)
        Else
            Return value
        End If
    End Function
End Class