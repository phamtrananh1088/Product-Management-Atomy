Imports System.Configuration

Class MainWindow

    Private Sub MenuItemEmployee_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemEmployee.Click
        My.Settings.OpenForm = "Employee"
        ShowPage(My.Settings.OpenForm)
    End Sub

    Private Sub MenuItemProp_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemProp.Click
        My.Settings.OpenForm = "Property1"
        ShowPage(My.Settings.OpenForm)
    End Sub

    Private Sub MenuItemWareHouseIn_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemWareHouseIn.Click
        My.Settings.OpenForm = "WarehouseIn"
        ShowPage(My.Settings.OpenForm)
    End Sub

    Private Sub MenuItemWareHouseOut_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemWareHouseOut.Click
        My.Settings.OpenForm = "Warehouse"
        ShowPage(My.Settings.OpenForm)
    End Sub

    Private Sub MenuItemOrder_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemOrder.Click
        My.Settings.OpenForm = "Order"
        ShowPage(My.Settings.OpenForm)
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim confirm = MessageBox.Show("Bạn có muốn thoát khỏi ứng dụng không?", "Atomy", MessageBoxButton.YesNo)
        If confirm = MessageBoxResult.Yes Then
            Application.Current.Shutdown()
        End If
    End Sub

    Private Sub MenuItemCustomer_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemCustomer.Click
        My.Settings.OpenForm = "Customer"
        ShowPage(My.Settings.OpenForm)
    End Sub

    Private Sub ShowPage(pageKey As String)
        Select Case pageKey
            Case "Employee"
                Dim p As New Employee
                Main.Content = p
            Case "Property1"
                Dim p As New Property1
                Main.Content = p
            Case "WarehouseIn"
                Dim p As New Warehouse
                Main.Content = p
            Case "Warehouse"
                Dim p As New Warehouse
                Main.Content = p
            Case "Order"
                Dim p As New Order
                Main.Content = p
            Case "Customer"
                Dim p As New Customer
                Main.Content = p
            Case Else
                Dim p As New Home
                Main.Content = p
        End Select
    End Sub
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Try
            ShowPage(My.Settings.OpenForm)
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã có lỗi sảy ra khi mở ứng dụng", ex)
        End Try
    End Sub
End Class
