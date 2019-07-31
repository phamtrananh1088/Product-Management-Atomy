Imports System.Data.OleDb
Imports System.Data
Imports System.Text

Public Class Property1
    Private _AtomyDataSet As AtomyDataSet
    Private _Mode As DataRowState
    Public Property AtomyDataSet As AtomyDataSet
        Get
            Return _AtomyDataSet
        End Get
        Set(value As AtomyDataSet)

        End Set
    End Property

    Public Sub New()
        _AtomyDataSet = New AtomyDataSet()
        _Mode = DataRowState.Added
        ' This call is required by the designer.
        InitializeComponent()
        btnInsert_Click(btnInsert, New System.Windows.RoutedEventArgs)
        ' Add any initialization after the InitializeComponent() call.
    End Sub
#Region "LoadData"
    Private Sub LoadData(PropCd As String)
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim sSQL As String = "select * from [Property] where [PropCode] = ?"
            Dim adapt As New OleDbDataAdapter(sSQL, dbConn.Conn)
            adapt.SelectCommand.Parameters.Add("@PropCode", OleDbType.VarChar).Value = PropCd
            _AtomyDataSet._Property.Clear()
            adapt.Fill(_AtomyDataSet, "Property")
            If _AtomyDataSet._Property.Rows.Count > 0 Then
                Me.DataContext = _AtomyDataSet._Property.Rows(0)
                _Mode = DataRowState.Modified
                CtrEnable()
            Else
                btnInsert_Click(btnInsert, New System.Windows.RoutedEventArgs)
            End If

        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi lấy dữ liệu.", ex)
        Finally
            dbConn.Close()
        End Try

    End Sub
#End Region

#Region "EnableButton"
    Private Sub CtrEnable()
        If _Mode = DataRowState.Modified Then
            'btnDelete.Background = Brushes.Red
            'btnDelete.Foreground = Brushes.White
            'btnDelete.BorderBrush = Brushes.Red
            txtPropCd.IsEnabled = False
            btnDelete.IsEnabled = True
            If _AtomyDataSet._Property.Rows.Count > 0 Then
                Dim row As AtomyDataSet.PropertyRow = _AtomyDataSet._Property.Rows(0)
                cbRetired.IsEnabled = row.Retired
            End If
        Else
            'btnDelete.Background = New SolidColorBrush(Color.FromRgb(244, 244, 244))
            'btnDelete.Foreground = Brushes.Gray
            'btnDelete.BorderBrush = Brushes.Gray
            txtPropCd.IsEnabled = True
            btnDelete.IsEnabled = False
            cbRetired.IsEnabled = False
        End If

    End Sub
#End Region

#Region "form_load"
    Private Sub form_load(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

    End Sub
#End Region

#Region "btnUpdate_Click"
    Private Sub btnUpdate_Click(sender As Object, e As RoutedEventArgs)
        Try
            Select Case _Mode
                Case DataRowState.Added
                    If Not ValidateData(EnumAction.Insert) Then
                        Return
                    End If
                    If Check.IsExisted("Property", txtPropCd.Text) Then
                        MessageBox.Show("Mã sản phẩm đã tồn tại.")
                        HelpCreatePropCode()
                        Return
                    End If

                    If InsertProperty() Then
                        MessageBox.Show("Đã hoàn thành.")
                        lblPropCodeHint.Content = ""
                        LoadData(txtPropCd.Text)
                    Else
                        MessageBox.Show("Không thành công.")
                    End If
                Case DataRowState.Modified
                    If Not ValidateData(EnumAction.Update) Then
                        Return
                    End If
                    If UpdateProperty() Then
                        MessageBox.Show("Đã hoàn thành.")
                        lblPropCodeHint.Content = ""
                        LoadData(txtPropCd.Text)
                    Else
                        MessageBox.Show("Không thành công.")
                    End If
            End Select
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào nút Cập nhật.", ex)
        End Try


    End Sub
#End Region

#Region "btnDelete_Click"
    Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not ValidateData(EnumAction.Delete) Then
                Return
            End If
            If _Mode = DataRowState.Modified Then
                Dim confirm As Boolean = (MessageBox.Show("Bạn có muốn xóa mặt hàng này không?", "Atomy", MessageBoxButton.YesNo) = MessageBoxResult.OK)
                If confirm Then
                    If DeleteProperty() Then
                        MessageBox.Show("Đã hoàn thành.")
                        lblPropCodeHint.Content = ""
                        btnInsert_Click(btnInsert, New System.Windows.RoutedEventArgs)
                    End If
                End If
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào nút Xóa.", ex)
        End Try

    End Sub
#End Region

#Region "btnInsert_Click"
    Private Sub btnInsert_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not ValidateData(EnumAction.Insert) Then
                Return
            End If
            _AtomyDataSet._Property.Clear()
            Dim newRow As AtomyDataSet.PropertyRow = _AtomyDataSet._Property.NewPropertyRow()
            _AtomyDataSet._Property.Rows.Add(newRow)
            Me.DataContext = _AtomyDataSet._Property.Rows(0)
            _Mode = DataRowState.Added
            CtrEnable()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào nút Thêm.", ex)
        End Try
    End Sub
#End Region

#Region "search_SearchResult"
    Private Sub search_SearchResult(sender As Object, e As SearchDataArgs)
        LoadData(e.Code)

    End Sub
#End Region

#Region "lnkPropCd_Click"
    Private Sub lnkPropCd_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim search As New Search()
            AddHandler search.SearchResult, AddressOf search_SearchResult
            search.Kind = EnumSearch.SearchProperty
            search.ShowDialog()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào link Mã sản phẩm.", ex)
        End Try
    End Sub
#End Region

#Region "txtPropCd_LostFocus"
    Private Sub txtPropCd_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If _Mode = DataRowState.Added Then
                Dim s = txtPropCd.Text.Trim()
                If s.Length < 8 Then
                    Dim lead As String = New String("0", 8 - s.Length)
                    s = lead + s
                    txtPropCd.Text = s
                End If

            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô mã sản phẩm.", ex)
        End Try


    End Sub
#End Region

#Region "BUSINESS"
#Region "ValidateData"
    Private Function ValidateData(action As EnumAction) As Boolean
        Dim hasError As Boolean
        Select Case action
            Case EnumAction.Update
                hasError = Validation.GetHasError(txtPropCd)
                hasError = hasError OrElse Validation.GetHasError(txtPropName)
                hasError = hasError OrElse Validation.GetHasError(txtSalesPrice)
                hasError = hasError OrElse Validation.GetHasError(txtUnit)
                hasError = hasError OrElse Validation.GetHasError(txtPurchasePrice)
                hasError = hasError OrElse Validation.GetHasError(txtCurrentValue)
            Case EnumAction.Insert
                hasError = Validation.GetHasError(txtPropCd)
                hasError = hasError OrElse Validation.GetHasError(txtPropName)
                hasError = hasError OrElse Validation.GetHasError(txtSalesPrice)
                hasError = hasError OrElse Validation.GetHasError(txtUnit)
                hasError = hasError OrElse Validation.GetHasError(txtPurchasePrice)
                hasError = hasError OrElse Validation.GetHasError(txtCurrentValue)
            Case EnumAction.Delete

        End Select
        Return Not hasError
    End Function
#End Region

    Private Function DeleteProperty() As Boolean
        Dim dbConn As New DbConnect()
        Dim res As Integer

        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim sSQL As String = DeletePropertySQL()
            Dim cmd As New OleDbCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            Dim row As AtomyDataSet.PropertyRow = _AtomyDataSet._Property.Rows(0)
            cmd.Parameters.Add("@1", OleDbType.Boolean).Value = True
            cmd.Parameters.Add("@2", OleDbType.VarChar).Value = New Date().ToString("yyyy/MM/dd")
            cmd.Parameters.Add("@3", OleDbType.VarChar).Value = row.PropCode

            res = cmd.ExecuteNonQuery()
            dbConn.CommitTran()
        Catch ex As Exception
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi xóa sản phẩm.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res
    End Function

    Private Function InsertProperty() As Boolean
        Dim dbConn As New DbConnect()
        Dim res As Integer
      
        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim sSQL As String = InsertPropertySQL()
            Using cmd As New OleDbCommand(sSQL, dbConn.Conn)
                cmd.Transaction = dbConn.Tran
                Dim row As AtomyDataSet.PropertyRow = _AtomyDataSet._Property.Rows(0)
                Dim now As Date = Date.Now
                row.Create_Date = now.ToString("yyyy/MM/dd")
                row.Create_Time = now.ToString("HH:mm:ss")
                row.Create_User = Utility.LoginUserCode
                row.Update_Date = now.ToString("yyyy/MM/dd")
                row.Update_Time = now.ToString("HH:mm:ss")
                row.Update_User = Utility.LoginUserCode

                cmd.Parameters.Add("@1", OleDbType.VarChar).Value = row.PropCode
                cmd.Parameters.Add("@2", OleDbType.VarChar).Value = row.PropName
                cmd.Parameters.Add("@3", OleDbType.VarChar).Value = IIf(row.Description Is Nothing, "", row.Description)
                cmd.Parameters.Add("@4", OleDbType.VarChar).Value = IIf(row.Category Is Nothing, "", row.Category)
                cmd.Parameters.Add("@5", OleDbType.VarChar).Value = IIf(row.Condition Is Nothing, "", row.Condition)
                cmd.Parameters.Add("@6", OleDbType.VarChar).Value = IIf(row.Acquired_Date Is Nothing, now.ToString("yyyy/MM/dd"), row.Acquired_Date)
                cmd.Parameters.Add("@7", OleDbType.VarChar).Value = IIf(row.Unit Is Nothing, "", row.Unit)
                cmd.Parameters.Add("@8", OleDbType.Currency).Value = row.Purchase_Price
                cmd.Parameters.Add("@9", OleDbType.Currency).Value = row.Sales_Price
                cmd.Parameters.Add("@10", OleDbType.Currency).Value = row.Current_Value
                cmd.Parameters.Add("@11", OleDbType.VarChar).Value = IIf(row.Location Is Nothing, Utility.DefaultData.DefaultLocation, row.Location)
                cmd.Parameters.Add("@12", OleDbType.VarChar).Value = IIf(row.Manufacturer Is Nothing, Utility.DefaultData.DefaultManufacturer, row.Manufacturer)
                cmd.Parameters.Add("@13", OleDbType.VarChar).Value = IIf(row.Model Is Nothing, "", row.Model)
                cmd.Parameters.Add("@14", OleDbType.VarChar).Value = IIf(row.Comments Is Nothing, "", row.Comments)
                cmd.Parameters.Add("@15", OleDbType.VarChar).Value = IIf(row.Retired_Date Is Nothing, "", row.Retired_Date)
                cmd.Parameters.Add("@16", OleDbType.VarChar).Value = row.Create_Date
                cmd.Parameters.Add("@17", OleDbType.VarChar).Value = row.Create_Time
                cmd.Parameters.Add("@18", OleDbType.VarChar).Value = row.Create_User
                cmd.Parameters.Add("@19", OleDbType.VarChar).Value = row.Update_Date
                cmd.Parameters.Add("@20", OleDbType.VarChar).Value = row.Update_Time
                cmd.Parameters.Add("@21", OleDbType.VarChar).Value = row.Update_User

                res = cmd.ExecuteNonQuery()

            End Using

            dbConn.CommitTran()
        Catch ex As Exception
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi cập nhật sản phẩm.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res
    End Function

    Private Function UpdateProperty() As Boolean
        Dim dbConn As New DbConnect()
        Dim res As Integer

        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim sSQL As String = UpdatePropertySQL()
            Dim cmd As New OleDbCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            Dim row As AtomyDataSet.PropertyRow = _AtomyDataSet._Property.Rows(0)
            Dim now As Date = Date.Now
            row.Create_Date = now.ToString("yyyy/MM/dd")
            row.Create_Time = now.ToString("HH:mm:ss")
            row.Create_User = Utility.LoginUserCode
            row.Update_Date = now.ToString("yyyy/MM/dd")
            row.Update_Time = now.ToString("HH:mm:ss")
            row.Update_User = Utility.LoginUserCode
          
            cmd.Parameters.Add("@PropName", OleDbType.VarChar).Value = row.PropName
            cmd.Parameters.Add("@Description", OleDbType.VarChar).Value = IIf(row.Description Is Nothing, "", row.Description)
            cmd.Parameters.Add("@Category", OleDbType.VarChar).Value = IIf(row.Category Is Nothing, "", row.Category)
            cmd.Parameters.Add("@Condition", OleDbType.VarChar).Value = IIf(row.Condition Is Nothing, "", row.Condition)
            cmd.Parameters.Add("@Acquired_Date", OleDbType.VarChar).Value = IIf(row.Acquired_Date Is Nothing, now.ToString("yyyy/MM/dd"), row.Acquired_Date)
            cmd.Parameters.Add("@Unit", OleDbType.VarChar).Value = IIf(row.Unit Is Nothing, "", row.Unit)
            cmd.Parameters.Add("@Purchase_Price", OleDbType.Currency).Value = row.Purchase_Price
            cmd.Parameters.Add("@Sales_Price", OleDbType.Currency).Value = row.Sales_Price
            cmd.Parameters.Add("@Current_Value", OleDbType.Currency).Value = row.Current_Value
            cmd.Parameters.Add("@Location", OleDbType.VarChar).Value = IIf(row.Location Is Nothing, Utility.DefaultData.DefaultLocation, row.Location)
            cmd.Parameters.Add("@Manufacturer", OleDbType.VarChar).Value = IIf(row.Manufacturer Is Nothing, Utility.DefaultData.DefaultManufacturer, row.Manufacturer)
            cmd.Parameters.Add("@Model", OleDbType.VarChar).Value = IIf(row.Model Is Nothing, "", row.Model)
            cmd.Parameters.Add("@Comments", OleDbType.VarChar).Value = IIf(row.Comments Is Nothing, "", row.Comments)
            cmd.Parameters.Add("@Retired_Date", OleDbType.VarChar).Value = IIf(row.Retired_Date Is Nothing, "", row.Retired_Date)
            cmd.Parameters.Add("@Create_Date", OleDbType.VarChar).Value = row.Create_Date
            cmd.Parameters.Add("@Create_Time", OleDbType.VarChar).Value = row.Create_Time
            cmd.Parameters.Add("@Create_User", OleDbType.VarChar).Value = row.Create_User
            cmd.Parameters.Add("@Update_Date", OleDbType.VarChar).Value = row.Update_Date
            cmd.Parameters.Add("@Update_Time", OleDbType.VarChar).Value = row.Update_Time
            cmd.Parameters.Add("@Update_User", OleDbType.VarChar).Value = row.Update_User
            cmd.Parameters.Add("@PropCode", OleDbType.VarChar).Value = row.PropCode

            res = cmd.ExecuteNonQuery()
            dbConn.CommitTran()
        Catch ex As Exception
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi cập nhật sản phẩm.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res
    End Function

#Region "HelpCreatePropCode"
    Private Sub HelpCreatePropCode()
        lblPropCodeHint.Content = "Gợi ý: " + Utility.HelpCreateCode("Property")
    End Sub

#End Region
#End Region

#Region "☆ SQL"
#Region "InsertPropertySQL"
    Private Function InsertPropertySQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("INSERT INTO [Property]                               ")
        sb.AppendLine("            ( [PropCode], [PropName], [Description], [Category], [Condition], [Acquired Date], [Unit], [Purchase Price], [Sales Price], [Current Value], [Location], [Manufacturer], [Model], [Comments], [Retired Date], [Create Date], [Create Time], [Create User], [Update Date], [Update Time], [Update User]) ")
        sb.AppendLine("     VALUES ( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)                                          ")
        Return sb.ToString()
    End Function
#End Region

#Region "UpdatePropertySQL"
    Private Function UpdatePropertySQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Property]                                ")
        sb.AppendLine("   SET [PropName] = ?                            ")
        sb.AppendLine("     , [Description] = ?                         ")
        sb.AppendLine("     , [Category] = ?                            ")
        sb.AppendLine("     , [Condition] = ?                           ")
        sb.AppendLine("     , [Acquired Date] = ?                       ")
        sb.AppendLine("     , [Unit] = ?                                ")
        sb.AppendLine("     , [Purchase Price] = ?                      ")
        sb.AppendLine("     , [Sales Price] = ?                         ")
        sb.AppendLine("     , [Current Value] = ?                       ")
        sb.AppendLine("     , [Location] = ?                            ")
        sb.AppendLine("     , [Manufacturer] = ?                        ")
        sb.AppendLine("     , [Model] = ?                               ")
        sb.AppendLine("     , [Comments] = ?                            ")
        sb.AppendLine("     , [Create Date] = ?                         ")
        sb.AppendLine("     , [Create Time] = ?                         ")
        sb.AppendLine("     , [Create User] = ?                         ")
        sb.AppendLine("     , [Update Date] = ?                         ")
        sb.AppendLine("     , [Update Time] = ?                         ")
        sb.AppendLine("     , [Update User] = ?                         ")
        sb.AppendLine(" WHERE [PropCode] = ?                            ")
        Return sb.ToString()
    End Function
#End Region

#Region "DeletePropertySQL"
    Private Function DeletePropertySQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Property]                                ")
        sb.AppendLine("   SET [Retired] = ?                             ")
        sb.AppendLine("     , [Retired Date] = ?                        ")
        sb.AppendLine(" WHERE [PropCode] = ?                            ")
        Return sb.ToString()
    End Function
#End Region
#End Region


    Private Sub txtSalesPrice_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            '   DirectCast(sender, Control).GetBindingExpression(TextBox.TextProperty).UpdateSource()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô giá bán.", ex)
        End Try
    End Sub

End Class
