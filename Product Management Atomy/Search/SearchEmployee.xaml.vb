Imports System.Data.SqlClient

Class SearchEmployee
    Implements ISearch

    Private _search As Search
    Private _AtomyDataSet As PMS_ATOMYDataSet
    Public Property AtomyDataSet As PMS_ATOMYDataSet
        Get
            Return _AtomyDataSet
        End Get
        Set(value As PMS_ATOMYDataSet)

        End Set
    End Property
    Protected Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Sub New(search As Search)
        _AtomyDataSet = New PMS_ATOMYDataSet()
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
            Dim sSQL As String = "select *,[FirstName] + ' ' + [LastName] as FullName from [Employee] where [EmpCode] like @EmpCode"
            Dim adapt As New SqlDataAdapter()
            adapt.SelectCommand = New SqlCommand()
            adapt.SelectCommand.Connection = dbConn.Conn
            adapt.SelectCommand.Parameters.AddWithValue("@EmpCode", txtEmpCode.Text.Trim + "%")
            If txtFirstName.Text.Trim.Length > 0 Then
                sSQL = sSQL + " and [FirstName] like @FirstName"
                adapt.SelectCommand.Parameters.AddWithValue("@FirstName", "%" + txtFirstName.Text.Trim + "%")
            End If
            If txtLastName.Text.Trim.Length > 0 Then
                sSQL = sSQL + " and [LastName] like @LastName"
                adapt.SelectCommand.Parameters.AddWithValue("@LastName", "%" + txtFirstName.Text.Trim + "%")
            End If
            If txtMobilePhone.Text.Trim.Length > 0 Then
                sSQL = sSQL + " and [MobilePhone] like @MobilePhone"
                adapt.SelectCommand.Parameters.AddWithValue("@MobilePhone", "%" + txtFirstName.Text.Trim + "%")
            End If
            sSQL = sSQL + " order by [LastName],[FirstName]"
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

    Public Function SearchByKey() As SearchDataArgs Implements ISearch.SearchByKey
        Dim dbConn As New DbConnect
        Dim res As SearchDataEmployee = Nothing
        Try
            dbConn.Open()
            Dim sSQL As String = "select *,[FirstName] + ' ' + [LastName] as FullName from [Employee] where [EmpCode] = @EmpCode"
            Dim cmd As New SqlCommand(sSQL, dbConn.Conn)
            cmd.Parameters.AddWithValue("@EmpCode", Keycode)
            Dim read As SqlDataReader = cmd.ExecuteReader()
            If read.Read() Then
                res = New SearchDataEmployee() With {.Code = read("[EmpCode]").ToString, .Name = read("[FullName]").ToString()}
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã có lỗi khi tìm kiếm nhân viên.", ex)
        Finally
            dbConn.Close()
        End Try
        Return res
    End Function
End Class
Public Class SearchDataEmployee
    Inherits SearchDataArgs
End Class
