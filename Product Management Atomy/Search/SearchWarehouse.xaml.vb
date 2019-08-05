Imports System.Data.OleDb

Class SearchWarehouse
    Private _search As Search
    Private AtomyDataSet As AtomyDataSet
    Protected Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(search As Search)
        AtomyDataSet = New AtomyDataSet()
        _search = search
        ' This call is required by the designer.
        InitializeComponent()
    End Sub

    Private Sub SearchData()
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim sSQL As String = "select [WarehouseMaster].*,[Customer].[MobilePhone] from [WarehouseMaster] left join [Customer] on [WarehouseMaster].[CusCode] = [Customer].[CusCode] where [WareCode] like ?"
            Dim adapt As New OleDbDataAdapter()
            adapt.SelectCommand = New OleDbCommand()
            adapt.SelectCommand.Connection = dbConn.Conn
            adapt.SelectCommand.Parameters.Add("@WareCode", OleDbType.VarChar).Value = txtWareCode.Text.Trim + "%"
            If txtWareTitle.Text.Trim.Length > 0 Then
                sSQL = sSQL + " and [WareTitle] like ?"
                adapt.SelectCommand.Parameters.Add("@WareTitle", OleDbType.VarChar).Value = "%" + txtWareTitle.Text.Trim + "%"
            End If
            If txtWareDateF.Text.Trim.Length > 0 Then
                If txtWareDateT.Text.Trim.Length > 0 Then
                    sSQL = sSQL + " and [WareDate] >= ? and [WareDate] <= ?"
                    adapt.SelectCommand.Parameters.Add("@WareDateF", OleDbType.VarChar).Value = txtWareDateF.Text.Trim
                    adapt.SelectCommand.Parameters.Add("@WareDateT", OleDbType.VarChar).Value = txtWareDateT.Text.Trim
                Else
                    sSQL = sSQL + " and [WareDate] >= ?"
                    adapt.SelectCommand.Parameters.Add("@WareDateF", OleDbType.VarChar).Value = txtWareDateF.Text.Trim
                End If
            Else
                If txtWareDateT.Text.Trim.Length > 0 Then
                    sSQL = sSQL + " and [WareDate] <= ?"
                    adapt.SelectCommand.Parameters.Add("@WareDateT", OleDbType.VarChar).Value = txtWareDateT.Text.Trim
                End If
            End If
            If txtCusSearch.Text.Trim.Length > 0 Then
                sSQL = sSQL + " and ([CusCode] like ? or [CusName] like ? or [MobilePhone] like ?)"
                adapt.SelectCommand.Parameters.Add("@CusCode", OleDbType.VarChar).Value = ConvertCode(txtCusSearch.Text.Trim) + "%"
                adapt.SelectCommand.Parameters.Add("@CusName", OleDbType.VarChar).Value = "%" + ConvertCode(txtCusSearch.Text.Trim) + "%"
                adapt.SelectCommand.Parameters.Add("@MobilePhone", OleDbType.VarChar).Value = "%" + ConvertCode(txtCusSearch.Text.Trim) + "%"
            End If

            sSQL = sSQL + " order by [WarehouseMaster].[retired] desc"
            adapt.SelectCommand.CommandText = sSQL
            AtomyDataSet.WarehouseMaster.Clear()
            adapt.Fill(AtomyDataSet, "WarehouseMaster")

            grdData.ItemsSource = AtomyDataSet.WarehouseMaster.DefaultView
        Catch ex As Exception
            ErrorLog.SetError(_search, "WarehouseMaster: Load data error", ex)
        Finally
            dbConn.Close()
        End Try

    End Sub

    Private Function ConvertCode(code As String)
        If code.Length = 0 Then
        ElseIf code.Length < 8 Then
            Dim lead As String = New String("0", 8 - code.Length)
            code = lead + code
        End If
        Return code
    End Function
    Private Sub btnClose_Click(sender As Object, e As RoutedEventArgs)
        _search.CloseF()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As RoutedEventArgs)
        SearchData()
    End Sub

    Private Sub rowCode_Click(sender As Object, e As RoutedEventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim data As New SearchDataWarehouse() With {.Code = btn.Content.ToString, .Name = btn.Tag.ToString}
        _search.ResultF(data)
    End Sub
End Class
Public Class SearchDataWarehouse
    Inherits SearchDataArgs
End Class
