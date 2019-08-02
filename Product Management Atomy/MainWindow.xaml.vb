Imports System.Configuration

Class MainWindow


    Private Sub MenuItemProp_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemProp.Click
        My.Settings.OpenForm = "Property1"
        Dim p As New Property1
        Main.Content = p

    End Sub

    Private Sub MenuItemWareHouseIn_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemWareHouseIn.Click
        My.Settings.OpenForm = "WarehouseIn"
        Dim p As New Warehouse
        Main.Content = p
    End Sub

    Private Sub MenuItemWareHouseOut_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemWareHouseOut.Click
        My.Settings.OpenForm = "Warehouse"
        Dim p As New Warehouse
        Main.Content = p
    End Sub

    Private Sub MenuItemOrder_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemOrder.Click
        My.Settings.OpenForm = "Order"
        Dim p As New Order
        Main.Content = p
    End Sub

    Private Sub MenuItem5_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemExit.Click
        Dim confirm = MessageBox.Show("Bạn có muốn thoát khỏi ứng dụng không?", "Atomy", MessageBoxButton.YesNo)
        If confirm = MessageBoxResult.Yes Then
            Application.Current.Shutdown()
        End If
    End Sub

    Private Sub MenuItemCustomer_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MenuItemCustomer.Click
        My.Settings.OpenForm = "Customer"
        Dim p As New Customer
        Main.Content = p
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Try
            Select Case My.Settings.OpenForm
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

            End Select

        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã có lỗi sảy ra khi mở ứng dụng", ex)
        End Try
    End Sub
End Class
