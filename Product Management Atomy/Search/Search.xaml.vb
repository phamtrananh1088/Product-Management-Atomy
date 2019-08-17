Public Class Search
    Private _Kind As EnumSearch
    Public Property Kind As EnumSearch
        Get
            Return _Kind
        End Get
        Set(value As EnumSearch)
            _Kind = value
            Select Case _Kind
                Case EnumSearch.SearchProperty
                    Dim page As SearchProperty = New SearchProperty(Me)
                    Me.Content = page
                    Me.Title = page.Title
                Case EnumSearch.SearchEmployee
                    Dim page As SearchEmployee = New SearchEmployee(Me)
                    Me.Content = page
                    Me.Title = page.Title
                Case EnumSearch.SearchCustomer
                    Dim page As SearchCustomer = New SearchCustomer(Me)
                    Me.Content = page
                    Me.Title = page.Title
                Case EnumSearch.SearchWareHouse
                    Dim page As SearchWarehouse = New SearchWarehouse(Me)
                    page.WareType = 1
                    Me.Content = page
                    Me.Title = page.Title
                Case EnumSearch.SearchWareHouseIn
                    Dim page As SearchWarehouse = New SearchWarehouse(Me)
                    page.WareType = 0
                    Me.Content = page
                    Me.Title = page.Title
            End Select
        End Set
    End Property
    Public Event SearchResult(sender As Object, data As SearchDataArgs)
    Public Event SearchClose(sender As Object, data As EventArgs)

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private _callback As Action(Of SearchDataArgs)
    Public Sub New(callback As Action(Of SearchDataArgs), ByRef caller As System.Windows.Controls.TextBox, searchKind As EnumSearch)
        _callback = callback
        SearchByKey(caller, searchKind)
    End Sub

    Sub CloseF()
        Me.Close()
        RaiseEvent SearchClose(Me, New EventArgs)
    End Sub

    Sub ResultF(data As SearchDataArgs)
        Me.Close()
        RaiseEvent SearchResult(Me, data)
    End Sub

    Private Sub SearchByKey(caller As TextBox, searchKind As EnumSearch)
        Select Case searchKind
            Case EnumSearch.SearchProperty
                Dim page As SearchProperty = New SearchProperty(caller.Text.Trim())
                Dim res As SearchDataArgs = page.SearchByKey()
                _callback(res)
            Case EnumSearch.SearchEmployee
                Dim page As SearchEmployee = New SearchEmployee(caller.Text.Trim())
                Dim res As SearchDataArgs = page.SearchByKey()
                _callback(res)
            Case EnumSearch.SearchCustomer
                Dim page As SearchCustomer = New SearchCustomer(caller.Text.Trim())
                Dim res As SearchDataArgs = page.SearchByKey()
                _callback(res)
            Case EnumSearch.SearchWareHouse, EnumSearch.SearchWareHouseIn
                Dim page As SearchWarehouse = New SearchWarehouse(caller.Text.Trim())
                page.WareType = If(searchKind = EnumSearch.SearchWareHouse, 1, 0)
                Dim res As SearchDataArgs = page.SearchByKey()
                _callback(res)
        End Select
    End Sub

End Class
Public Enum EnumSearch
    None = 0
    SearchProperty = 1
    SearchEmployee = 2
    SearchCustomer = 3
    SearchWareHouse = 4
    SearchWareHouseIn = 5

End Enum

Public MustInherit Class SearchDataArgs
    Inherits EventArgs
    Public Code As String
    Public Name As String
End Class


