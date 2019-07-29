Imports System.Data

Public Class Warehouse
    Private _AtomyDataSet As AtomyDataSet
    Public Property AtomyDataSet As AtomyDataSet
        Get
            Return _AtomyDataSet
        End Get
        Set(value As AtomyDataSet)

        End Set
    End Property
    Private _Mode As DataRowState
    Public Sub New()
        _AtomyDataSet = New AtomyDataSet()
        _Mode = DataRowState.Added
        ' This call is required by the designer.
        InitializeComponent()
        InitData()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub txtEmpCode_LostFocus(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub lnkCusCd_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim search As New Search()
            AddHandler search.SearchResult, AddressOf searchCus_SearchResult
            search.Kind = EnumSearch.SearchCustomer
            search.ShowDialog()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào link Mã sản phẩm.", ex)
        End Try
    End Sub

    Private Sub txtCusCd_LostFocus(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub txtWareCode_LostFocus(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub lnkEmpCode_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim search As New Search()
            AddHandler search.SearchResult, AddressOf searchEmp_SearchResult
            search.Kind = EnumSearch.SearchEmployee
            search.ShowDialog()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào link Mã sản phẩm.", ex)
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As RoutedEventArgs)

    End Sub
#Region "BUSINESS"
#Region "searchCus_SearchResult"
    Private Sub searchCus_SearchResult(sender As Object, e As SearchDataArgs)
        txtCusCd.Text = e.Code
        txtCusName.Text = e.Name
    End Sub
#End Region
    Private Sub InitData()
        _AtomyDataSet.Warehouse_Master.Clear()
        _AtomyDataSet.Warehouse.Clear()
        Dim newRow As AtomyDataSet.Warehouse_MasterRow = _AtomyDataSet.Warehouse_Master.NewWarehouse_MasterRow()
        _AtomyDataSet.Warehouse_Master.Rows.Add(newRow)
        Me.DataContext = _AtomyDataSet.Warehouse_Master.Rows(0)
        'Dim newRowD As AtomyDataSet.WarehouseRow = _AtomyDataSet.Warehouse.NewWarehouseRow()
        '_AtomyDataSet.Warehouse.Rows.Add(newRowD)
        grdWareHouse.ItemsSource = _AtomyDataSet.Warehouse.DefaultView
        _Mode = DataRowState.Added
    End Sub

#Region "searchEmp_SearchResult"
    Private Sub searchEmp_SearchResult(sender As Object, e As SearchDataArgs)
        txtEmpCode.Text = e.Code
        lblEmpName.Content = e.Name
    End Sub
#End Region
#End Region

End Class
