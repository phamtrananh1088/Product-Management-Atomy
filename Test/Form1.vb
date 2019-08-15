Public Class Form1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TextBox2.Text = MoneyHelper.ReadMoney(CDec(TextBox1.Text))
    End Sub
End Class

Public Class MoneyHelper

    Public Shared Function ReadMoney(amount As Decimal) As String
        Dim res As String = ""
        If amount > 1000000000 Then
            Return res
        End If
        Dim m = amount Mod 1000000
        Dim r = (amount - m) / 1000000
        Dim c = If(r = 0, "", ReadSegment(r))
        Dim d = ReadSegment(m)
        res = If(c = "", "", c + " ") + d
        Return res

    End Function
    Private Shared liM1 As New List(Of Decimal) From {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}
    Private Shared liN1 As New List(Of String) From {"không", "một", "hai", "ba", "bốn", "năm", "sáu", "bẩy", "tám", "chín"}
    Private Shared liC1 As New List(Of String) From {"mười", "một trăm", "một nghìn", "một triệu"}

    Private Shared Function ReadSegment(amount As Decimal, Optional isStart As Boolean = True)
        Dim res As String = ""
        'hai trăm bốn mươi triệu không trăm hai lăm nghìn
        'hai trăm bốn mươi triệu không trăm linh năm nghìn
        If amount < 10 Then
            If isStart Then
                Return liN1(amount)
            Else
                Return "linh" + liN1(amount)
            End If
        ElseIf amount = 10 Then
            Return liC1(0)
        End If
        If amount < 20 Then
            Dim m = amount Mod 10
            Dim r = (amount - m) / 10
            Dim c = ReadSegment(10)
            Dim d = ReadSegment(m)
            Return c + " " + d
        ElseIf amount = 1000 Then
            Return liC1(2)
        End If

        If amount < 100 Then
            Dim m = amount Mod 10
            Dim r = (amount - m) / 10
            Dim c = ReadSegment(r)
            Dim d = ReadSegment(m)
            Return c + " " + d
        ElseIf amount = 1000 Then
            Return liC1(2)
        End If

        If amount < 1000 Then
            Dim m = amount Mod 100
            Dim r = (amount - m) / 100
            Dim c = ReadSegment(r) + " trăm"
            Dim d = ReadSegment(m)
            Return c + " " + d
        ElseIf amount = 1000 Then
            Return liC1(3)
        End If

        If amount < 1000000 Then
            Dim m = amount Mod 1000
            Dim r = (amount - m) / 1000
            Dim c = ReadSegment(r) + " nghìn"
            Dim d = ReadSegment(m)
            Return c + " " + d 
        ElseIf amount = 1000000 Then
            Return liC1(4)
        End If

        Return res
    End Function


    Public Shared Function BSearch(ByVal arr() As Decimal, ByVal target As Decimal) As Decimal
        Dim min As Decimal = 0
        Dim max As Decimal = arr.Length - 1
        Dim mid As Decimal = 0

        While min <= max
            mid = (min + max) / 2

            If arr(mid) = target Then
                Return mid
            ElseIf arr(mid) < target Then
                min = mid + 1
            Else
                max = mid - 1
            End If
        End While

        Return -1
    End Function
End Class