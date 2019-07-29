Imports System.Data.OleDb

Public Class Check
    Public Shared Function IsExisted(tableName As String, code As String)
        Dim res As Boolean
        Dim sSQL As String
        Select Case tableName
            Case "Property"
                sSQL = "select count(*) from [Property] where [PropCode] = ?"
            Case "Customer"
                sSQL = "select count(*) from [Customer] where [CusCode] = ?"
            Case "Employee"
                sSQL = "select count(*) from [Employee] where [EmpCode] = ?"
            Case "Warehouse"
                sSQL = "select count(*) from [Warehouse Master] where [WareCode] = ?"
            Case Else
                sSQL = ""
        End Select
        Dim i As Integer = CountData(sSQL, code)
        If i > 0 Then
            res = True
        End If
        Return res
    End Function

    Private Shared Function CountData(sSQL As String, code As String) As Integer
        Dim dbConn As New DbConnect
        Dim res As Integer
        Try
            dbConn.Open()
            Using cmd As New OleDbCommand(sSQL, dbConn.Conn)
                cmd.Parameters.Add("@1", OleDbType.VarChar).Value = code
                Dim read As OleDbDataReader = cmd.ExecuteReader()
                If read.Read() Then
                    res = read.GetInt32(0)
                Else
                    res = 0
                End If

            End Using
        Catch ex As Exception
            ErrorLog.SetError("", "Đã sảy ra lỗi khi kiểm tra trùng bản ghi.", ex)
        Finally
            dbConn.Close()
        End Try
        Return res
    End Function

End Class
