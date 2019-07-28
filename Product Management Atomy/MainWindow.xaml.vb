Class MainWindow 


    Private Sub MenuItemProp_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemProp.Click
        Dim p As New Property1
        p.ShowDialog()

    End Sub

    Private Sub MenuItemWareHouseIn_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemWareHouseIn.Click
        Dim p As New Warehouse
        p.ShowDialog()
    End Sub

    Private Sub MenuItemWareHouseOut_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemWareHouseOut.Click
        Dim p As New Warehouse
        p.ShowDialog()
    End Sub

    Private Sub MenuItemOrder_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemOrder.Click
        Dim p As New Order
        p.ShowDialog()
    End Sub

    Private Sub MenuItem5_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemExit.Click
        Dim confirm = MessageBox.Show("Bạn có muốn thoát khỏi ứng dụng không?", "Atomy", MessageBoxButton.YesNo)
        If confirm = MessageBoxResult.Yes Then
            Application.Current.Shutdown()
        End If
    End Sub

    Private Sub MenuItemCustomer_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemCustomer.Click
        Dim p As New Customer
        p.ShowDialog()
    End Sub
End Class
