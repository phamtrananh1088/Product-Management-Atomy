Imports System.Data.OleDb

Class SearchEmployee
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

    Private Sub SearchData()
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim sSQL As String = "select * from [Employee] where [EmpCode] like ?"
            Dim adapt As New OleDbDataAdapter()
            adapt.SelectCommand = New OleDbCommand()
            adapt.SelectCommand.Connection = dbConn.Conn
            adapt.SelectCommand.Parameters.Add("@EmpCode", OleDbType.VarChar).Value = txtEmpCode.Text.Trim + "%"
            If txtFirstName.Text.Trim.Length > 0 Then
                sSQL = sSQL + " and [First Name] like ?"
                adapt.SelectCommand.Parameters.Add("@FirstName", OleDbType.VarChar).Value = "%" + txtFirstName.Text.Trim + "%"
            End If
            If txtLastName.Text.Trim.Length > 0 Then
                sSQL = sSQL + " and [Last Name] like ?"
                adapt.SelectCommand.Parameters.Add("@LastName", OleDbType.VarChar).Value = "%" + txtFirstName.Text.Trim + "%"
            End If
            If txtMobilePhone.Text.Trim.Length > 0 Then
                sSQL = sSQL + " and [Mobile Phone] like ?"
                adapt.SelectCommand.Parameters.Add("@MobilePhone", OleDbType.VarChar).Value = "%" + txtFirstName.Text.Trim + "%"
            End If
            sSQL = sSQL + " order by [Last Name],[First Name]"
            adapt.SelectCommand.CommandText = sSQL
            _AtomyDataSet.Employee.Clear()
            adapt.Fill(_AtomyDataSet, "Employee")

            grdData.ItemsSource = _AtomyDataSet.Employee.DefaultView
        Catch ex As Exception
            ErrorLog.SetError(_search, "Đã có lỗi sảy ra khi tìm kiếm nhân viên", ex)
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

    Private Sub rowEmpCode_Click(sender As Object, e As RoutedEventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim data As New SearchDataEmployee() With {.Code = btn.Content.ToString, .Name = btn.Tag.ToString}
        _search.ResultF(data)
    End Sub
End Class
Public Class SearchDataEmployee
    Inherits SearchDataArgs
End Class
