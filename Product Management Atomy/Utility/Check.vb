Imports System.Data.OleDb
Imports System.Data

Public Class Check
    Public Shared Function IsExisted(tableName As String, code As String) As Boolean
        Dim res As Boolean
        Dim sSQL As String = GetSelectCountSQL(tableName)

        Dim i As Integer = CountData(sSQL, code)
        If i > 0 Then
            res = True
        End If
        Return res
    End Function
    Private Shared Function GetSelectCountSQL(tableName As String) As String
        Dim sSQL As String
        Select Case tableName
            Case "Property"
                sSQL = "select count(*) from [Property] where [PropCode] = ?"
            Case "Customer"
                sSQL = "select count(*) from [Customer] where [CusCode] = ?"
            Case "Employee"
                sSQL = "select count(*) from [Employee] where [EmpCode] = ?"
            Case "Warehouse"
                sSQL = "select count(*) from [WarehouseMaster] where [WareCode] = ?"
            Case Else
                sSQL = ""
        End Select
        Return sSQL
    End Function
    Private Shared Function GetSelectSQL(tableName As String) As String
        Dim sSQL As String
        Select Case tableName
            Case "Property"
                sSQL = "select * from [Property] where [PropCode] = ?"
            Case "Customer"
                sSQL = "select * from [Customer] where [CusCode] = ?"
            Case "Employee"
                sSQL = "select * from [Employee] where [EmpCode] = ?"
            Case "Warehouse"
                sSQL = "select * from [Warehouse Master] where [WareCode] = ?"
            Case Else
                sSQL = ""
        End Select
        Return sSQL
    End Function
    Public Shared Function GetDataByCode(tableName As String, code As String) As DataRow
        Dim res As DataRow = Nothing
        Dim sSQL As String = GetSelectSQL(tableName)
        res = GetData(sSQL, code)
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

    Private Shared Function GetData(sSQL As String, code As String) As DataRow
        Dim ds As New DataSet()
        Dim dbConn As New DbConnect
        Dim res As DataRow = Nothing
        Try
            dbConn.Open()
            Dim adap As New OleDbDataAdapter(sSQL, dbConn.Conn)
            adap.SelectCommand.Parameters.Add("@1", OleDbType.VarChar).Value = code
            If adap.Fill(ds) > 0 Then
                res = ds.Tables(0).Rows(0)
            End If        
        Catch ex As Exception
            ErrorLog.SetError("", "Đã sảy ra lỗi khi tìm bản ghi theo mã.", ex)
        Finally
            dbConn.Close()
        End Try
        Return res
    End Function
End Class
