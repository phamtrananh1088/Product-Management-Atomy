Imports System.Data

Public Class ProcessSelection
    Private _Mode As DataRowState = DataRowState.Detached
    Public Property Mode As DataRowState
        Get
            Return _Mode
        End Get
        Set(value As DataRowState)
            _Mode = value
            If _Mode = DataRowState.Added Then
                rbAdd.IsChecked = True
            ElseIf _Mode = DataRowState.Modified Then
                rbUpdate.IsChecked = True
            ElseIf _Mode = DataRowState.Deleted Then
                rbDelete.IsChecked = True
            Else
                rbAdd.IsChecked = False
                rbUpdate.IsChecked = False
                rbDelete.IsChecked = False
                borAdd.Background = Brushes.White
                borUpdate.Background = Brushes.White
                borDelete.Background = Brushes.White
                RaiseEvent ValueChange(Me, New EventArgs)
            End If
        End Set
    End Property
    Public Event ValueChange(sender As Object, e As EventArgs)
    Private Sub RadioButton_Checked(sender As Object, e As RoutedEventArgs)
        Try
            Dim rb As RadioButton = DirectCast(sender, RadioButton)
            If rb.Equals(rbAdd) Then
                Mode = DataRowState.Added
                borAdd.Background = Brushes.Yellow
                borUpdate.Background = Brushes.White
                borDelete.Background = Brushes.White
                RaiseEvent ValueChange(Me, New EventArgs)
            ElseIf rb.Equals(rbUpdate) Then
                Mode = DataRowState.Modified
                borAdd.Background = Brushes.White
                borUpdate.Background = Brushes.Yellow
                borDelete.Background = Brushes.White
                RaiseEvent ValueChange(Me, New EventArgs)
            ElseIf rb.Equals(rbDelete) Then
                Mode = DataRowState.Deleted
                borAdd.Background = Brushes.White
                borUpdate.Background = Brushes.White
                borDelete.Background = Brushes.Yellow
                RaiseEvent ValueChange(Me, New EventArgs)
            Else
                Mode = DataRowState.Detached
                borAdd.Background = Brushes.White
                borUpdate.Background = Brushes.White
                borDelete.Background = Brushes.White
                RaiseEvent ValueChange(Me, New EventArgs)
            End If
        Catch ex As Exception
            ErrorLog.SetError(DirectCast(Me.Parent, Page), "Đã xảy ra lỗi khi chọn chế độ Thêm/Sửa/Xóa.", ex)
        End Try
    End Sub
End Class
