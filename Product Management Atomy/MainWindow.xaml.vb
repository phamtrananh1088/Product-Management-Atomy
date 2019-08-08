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
        Dim pop As New PopupWindow()
        Select Case pageKey
            Case "Employee"
                Dim p As New Employee
                pop.Content = p
                pop.Title = p.Title
                pop.ShowDialog()
            Case "Property1"
                Dim p As New Property1
                pop.Content = p
                pop.Title = p.Title
                pop.ShowDialog()
            Case "WarehouseIn"
                Dim p As New Warehouse
                pop.Content = p
                pop.Title = p.Title
                pop.ShowDialog()
            Case "Warehouse"
                Dim p As New Warehouse
                pop.Content = p
                pop.Title = p.Title
                pop.ShowDialog()
            Case "Order"
                Dim p As New Order
                pop.Content = p
                pop.Title = p.Title
                pop.ShowDialog()
            Case "Customer"
                Dim p As New Customer
                pop.Content = p
                pop.Title = p.Title
                pop.ShowDialog()
            Case Else
                Dim p As New Home
                pop.Content = p
                pop.Title = p.Title
                pop.ShowDialog()
        End Select
    End Sub
    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Try
            Dim p As New Home
            Main.Content = p
            ShowPage(My.Settings.OpenForm)
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã có lỗi sảy ra khi mở ứng dụng", ex)
        End Try
    End Sub
End Class
