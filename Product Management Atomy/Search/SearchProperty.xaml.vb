Imports System.Data.OleDb

Class SearchProperty
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
    Private Sub SearchData(PropCd As String)
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim sSQL As String = "select * from [Property] where [PropCode] like ?"
            Dim adapt As New OleDbDataAdapter()
            adapt.SelectCommand = New OleDbCommand()
            adapt.SelectCommand.Connection = dbConn.Conn
            adapt.SelectCommand.Parameters.Add("@PropCode", OleDbType.VarChar).Value = PropCd + "%"
            If txtPropName.Text.Trim.Length > 0 Then
                sSQL = sSQL + " and [PropName] like ?"
                adapt.SelectCommand.Parameters.Add("@PropName", OleDbType.VarChar).Value = "%" + txtPropName.Text.Trim + "%"
            End If
            If txtAcquiredDateF.Text.Trim.Length > 0 Then
                If txtAcquiredDateT.Text.Trim.Length > 0 Then
                    sSQL = sSQL + " and [Acquired Date] >= ? and [Acquired Date] <= ?"
                    adapt.SelectCommand.Parameters.Add("@AcquiredDateF", OleDbType.VarChar).Value = txtAcquiredDateF.Text.Trim
                    adapt.SelectCommand.Parameters.Add("@AcquiredDateT", OleDbType.VarChar).Value = txtAcquiredDateT.Text.Trim
                Else
                    sSQL = sSQL + " and [Acquired Date] >= ?"
                    adapt.SelectCommand.Parameters.Add("@AcquiredDateF", OleDbType.VarChar).Value = txtAcquiredDateF.Text.Trim
                End If
            Else
                If txtAcquiredDateT.Text.Trim.Length > 0 Then
                    sSQL = sSQL + " and [Acquired Date] <= ?"
                    adapt.SelectCommand.Parameters.Add("@AcquiredDateT", OleDbType.VarChar).Value = txtAcquiredDateT.Text.Trim
                End If
            End If
            If txtCategory.Text.Trim.Length > 0 Then
                sSQL = sSQL + " and [Category] like ?"
                adapt.SelectCommand.Parameters.Add("@Category", OleDbType.VarChar).Value = "%" + txtCategory.Text.Trim + "%"
            End If

            sSQL = sSQL + " order by retired desc"
            adapt.SelectCommand.CommandText = sSQL
            _AtomyDataSet._Property.Clear()
            adapt.Fill(_AtomyDataSet, "Property")

            grdData.ItemsSource = _AtomyDataSet._Property.DefaultView
        Catch ex As Exception
            ErrorLog.SetError(_search, "Property: Load data error", ex)
        Finally
            dbConn.Close()
        End Try

    End Sub

    Private Sub btnClose_Click(sender As Object, e As RoutedEventArgs)
        _search.CloseF()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As RoutedEventArgs)
        SearchData(txtPropCd.Text)
    End Sub

    Private Sub rowPropCd_Click(sender As Object, e As RoutedEventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim data As New SearchDataProperty() With {.Code = btn.Content.ToString, .Name = btn.Tag.ToString}
        _search.ResultF(data)
    End Sub

    Public Function SearchByKey() As SearchDataArgs Implements ISearch.SearchByKey
        Dim dbConn As New DbConnect
        Dim res As SearchDataProperty = Nothing
        Try
            dbConn.Open()
            Dim sSQL As String = "select * from [Property] where [PropCode] = ?"
            Dim cmd As New OleDbCommand(sSQL, dbConn.Conn)
            cmd.Parameters.Add("@PropCode", OleDbType.VarChar).Value = Keycode

            Dim read As OleDbDataReader = cmd.ExecuteReader()
            If read.Read() Then
                res = New SearchDataProperty() With {.Code = read("[PropCode]").ToString, .Name = read("[PropName]").ToString()}
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã có lỗi khi tìm kiếm mặt hàng.", ex)
        Finally
            dbConn.Close()
        End Try
        Return res
    End Function
End Class
Public Class SearchDataProperty
    Inherits SearchDataArgs
End Class
