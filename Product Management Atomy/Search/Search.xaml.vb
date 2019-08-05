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
                    Me.Content = Page
                    Me.Title = Page.Title
                Case EnumSearch.SearchCustomer
                    Dim page As SearchCustomer = New SearchCustomer(Me)
                    Me.Content = Page
                    Me.Title = page.Title
                Case EnumSearch.SearchWareHouse
                    Dim page As SearchWarehouse = New SearchWarehouse(Me)
                    Me.Content = page
                    Me.Title = page.Title
            End Select
        End Set
    End Property
    Public Event SearchResult(sender As Object, data As SearchDataArgs)
    Public Event SearchClose(sender As Object, data As EventArgs)

    Sub CloseF()
        Me.Close()
        RaiseEvent SearchClose(Me, New EventArgs)
    End Sub

    Sub ResultF(data As SearchDataArgs)
        Me.Close()
        RaiseEvent SearchResult(Me, data)
    End Sub

End Class
Public Enum EnumSearch
    None = 0
    SearchProperty = 1
    SearchEmployee = 2
    SearchCustomer = 3
    SearchWareHouse = 4

End Enum

Public MustInherit Class SearchDataArgs
    Inherits EventArgs
    Public Code As String
    Public Name As String
End Class


