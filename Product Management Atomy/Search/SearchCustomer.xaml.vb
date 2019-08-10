﻿Imports System.Data.OleDb

Class SearchCustomer
    Implements ISearch

    Private _search As Search
    Private _AtomyDataSet As AtomyDataSet
    Public Property AtomyDataSet As AtomyDataSet
        Get
            Return _AtomyDataSet
        End Get
        Set(value As AtomyDataSet)

        End Set
    End Property

    Protected Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(search As Search)
        _AtomyDataSet = New AtomyDataSet()
        _search = search
        ' This call is required by the designer.
        InitializeComponent()
    End Sub

    Public Property Keycode As String
    Public Sub New(code As String)
        Keycode = code
    End Sub

    Private Sub SearchData()
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim sSQL As String = "select *,[FirstName] + ' ' + [LastName] as FullName from [Customer] where [CusCode] like ?"
            Dim adapt As New OleDbDataAdapter()
            adapt.SelectCommand = New OleDbCommand()
            adapt.SelectCommand.Connection = dbConn.Conn
            adapt.SelectCommand.Parameters.Add("@CusCode", OleDbType.VarChar).Value = txtCusCode.Text.Trim + "%"
            If txtFirstName.Text.Trim.Length > 0 Then
                sSQL = sSQL + " and [FirstName] like ?"
                adapt.SelectCommand.Parameters.Add("@FirstName", OleDbType.VarChar).Value = "%" + txtFirstName.Text.Trim + "%"
            End If
            If txtLastName.Text.Trim.Length > 0 Then
                sSQL = sSQL + " and [LastName] like ?"
                adapt.SelectCommand.Parameters.Add("@LastName", OleDbType.VarChar).Value = "%" + txtLastName.Text.Trim + "%"
            End If

            sSQL = sSQL + " order by [LastName],[FirstName]"
            adapt.SelectCommand.CommandText = sSQL
            _AtomyDataSet.Customer.Clear()
            adapt.Fill(_AtomyDataSet, "Customer")

            grdData.ItemsSource = _AtomyDataSet.Customer.DefaultView
        Catch ex As Exception
            ErrorLog.SetError(_search, "Đã có lỗi sảy ra khi tìm kiếm khách hàng", ex)
        Finally
            dbConn.Close()
        End Try

    End Sub

    Private Sub btnClose_Click(sender As Object, e As RoutedEventArgs)
        _search.CloseF()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As RoutedEventArgs)
        SearchData()
    End Sub

    Private Sub rowCusCode_Click(sender As Object, e As RoutedEventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim data As New SearchDataCustomer() With {.Code = btn.Content.ToString, .Name = btn.Tag.ToString}
        _search.ResultF(data)
    End Sub

    Public Function SearchByKey() As SearchDataArgs Implements ISearch.SearchByKey
        Dim dbConn As New DbConnect
        Dim res As SearchDataCustomer = Nothing
        Try
            dbConn.Open()
            Dim sSQL As String = "select *,[FirstName] + ' ' + [LastName] as FullName from [Customer] where [CusCode] = ?"
            Dim cmd As New OleDbCommand(sSQL, dbConn.Conn)
            cmd.Parameters.Add("@CusCode", OleDbType.VarChar).Value = Keycode
            Dim read As OleDbDataReader = cmd.ExecuteReader()
            If read.Read() Then
                res = New SearchDataCustomer() With {.Code = read("[CusCode]").ToString, .Name = read("[FullName]").ToString()}
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã có lỗi khi tìm kiếm khách hàng.", ex)
        Finally
            dbConn.Close()
        End Try
        Return res
    End Function
End Class
Public Class SearchDataCustomer
    Inherits SearchDataArgs
End Class
