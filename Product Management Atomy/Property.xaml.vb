Imports System.Data.SqlClient
Imports System.Data
Imports System.Text

Public Class Property1
#Region "FIELD"
    Private AtomyDataSet As PMS_ATOMYDataSet
    Private Mode As DataRowState
#End Region

#Region "CONSTRUCTOR"
    Public Sub New()
        AtomyDataSet = New PMS_ATOMYDataSet()
        Mode = DataRowState.Added
        ' This call is required by the designer.
        InitializeComponent()
        InitialValue()
        ProcessSelection.Mode = DataRowState.Added
        ' Add any initialization after the InitializeComponent() call.
    End Sub
#End Region

#Region "InitialControl"
    Private Sub InitialValue()
        txtPropCode.Text = ""
        txtPropName.Text = ""
        txtDescription.Text = ""
        txtCategory.Text = ""
        txtCondition.Text = ""
        txtPurchasePrice.Text = ""
        txtUnit.Text = ""
        txtSalesPrice.Text = ""
        txtCurrentValue.Text = ""
        txtLocation.Text = ""
        txtManufacturer.Text = ""
        txtModel.Text = ""
        txtComments.Text = ""
        txtAccquiredDate.Text = ""
    End Sub
#End Region

#Region "LoadData"
    Private Sub LoadData(PropCode As String)
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim sSQL As String = "select * from [Property] where [PropCode] = @PropCode"
            Dim adapt As New SqlDataAdapter(sSQL, dbConn.Conn)
            adapt.SelectCommand.Parameters.AddWithValue("@PropCode", PropCode)
            AtomyDataSet._Property.Clear()

            If adapt.Fill(AtomyDataSet, "Property") > 0 Then
                Me.DataContext = AtomyDataSet._Property.Rows(0)
            Else
                MessageBox.Show("Mặt hàng [" + PropCode + "] không tồn tại hoặc đã bị xóa.")
                InitialValue()
                CtrEnable()
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi lấy dữ liệu.", ex)
        Finally
            dbConn.Close()
        End Try

    End Sub
#End Region

#Region "CtrEnable"
    Private Sub CtrEnable()
        If Me.Mode = DataRowState.Added Then
            txtPropName.IsEnabled = True
            txtDescription.IsEnabled = True
            txtCategory.IsEnabled = True
            txtCondition.IsEnabled = True
            txtPurchasePrice.IsEnabled = True
            txtUnit.IsEnabled = True
            txtSalesPrice.IsEnabled = True
            txtCurrentValue.IsEnabled = True
            txtLocation.IsEnabled = True
            txtManufacturer.IsEnabled = True
            txtModel.IsEnabled = True
            txtComments.IsEnabled = True
            txtAccquiredDate.IsEnabled = True
            lblRetiredDate.Visibility = Windows.Visibility.Hidden
            txtRetiredDate.Visibility = Windows.Visibility.Hidden
        ElseIf Mode = DataRowState.Modified Then
            txtPropName.IsEnabled = True
            txtDescription.IsEnabled = True
            txtCategory.IsEnabled = True
            txtCondition.IsEnabled = True
            txtPurchasePrice.IsEnabled = True
            txtUnit.IsEnabled = True
            txtSalesPrice.IsEnabled = True
            txtCurrentValue.IsEnabled = True
            txtLocation.IsEnabled = True
            txtManufacturer.IsEnabled = True
            txtModel.IsEnabled = True
            txtComments.IsEnabled = True
            txtAccquiredDate.IsEnabled = True
            lblRetiredDate.Visibility = Windows.Visibility.Hidden
            txtRetiredDate.Visibility = Windows.Visibility.Hidden
        ElseIf Me.Mode = DataRowState.Deleted Then
            txtPropName.IsEnabled = False
            txtDescription.IsEnabled = False
            txtCategory.IsEnabled = False
            txtCondition.IsEnabled = False
            txtPurchasePrice.IsEnabled = False
            txtUnit.IsEnabled = False
            txtSalesPrice.IsEnabled = False
            txtCurrentValue.IsEnabled = False
            txtLocation.IsEnabled = False
            txtManufacturer.IsEnabled = False
            txtModel.IsEnabled = False
            txtComments.IsEnabled = False
            txtAccquiredDate.IsEnabled = False
            lblRetiredDate.Visibility = Windows.Visibility.Visible
            txtRetiredDate.Visibility = Windows.Visibility.Visible
        End If

    End Sub
#End Region

#Region "ProcessSelection_ValueChange"
    Private Sub ProcessSelection_ValueChange(sender As Object, e As EventArgs)
        If ProcessSelection.Mode = DataRowState.Added Then
            AtomyDataSet._Property.Clear()
            Dim newRow As PMS_ATOMYDataSet.PropertyRow = AtomyDataSet._Property.NewPropertyRow()
            Utility.RowInit.InitPropertyRow(newRow)
            AtomyDataSet._Property.Rows.Add(newRow)
            Me.DataContext = AtomyDataSet._Property.Rows(0)
            Mode = DataRowState.Added
            CtrEnable()
            HelpCreatePropCode()
        ElseIf ProcessSelection.Mode = DataRowState.Modified Then
            Me.Mode = DataRowState.Modified
            CtrEnable()
            HelpGetLastPropCode()
        ElseIf ProcessSelection.Mode = DataRowState.Deleted Then
            Me.Mode = DataRowState.Deleted
            CtrEnable()
            HelpGetLastPropCode()
        End If
    End Sub
#End Region

#Region "BUSINESS"
#Region "btnProcess_Click"
    Private Sub btnProcess_Click(sender As Object, e As RoutedEventArgs)
        Try
            Select Case Mode
                Case DataRowState.Added
                    If Not ValidateData(EnumAction.Insert) Then
                        Return
                    End If
                    If Insert() Then
                        MessageBox.Show("Thêm mới thành công.")
                        lblPropCodeHint.Content = ""
                        ProcessSelection.Mode = DataRowState.Modified
                        LoadData(txtPropCode.Text.Trim)
                    Else
                        MessageBox.Show("Thêm mới thất bại.")
                    End If
                Case DataRowState.Modified
                    If Not ValidateData(EnumAction.Update) Then
                        Return
                    End If
                    If Update() Then
                        MessageBox.Show("Sửa đổi thành công.")
                        lblPropCodeHint.Content = ""
                        LoadData(txtPropCode.Text.Trim)
                    Else
                        MessageBox.Show("Sửa đổi thất bại.")
                    End If
                Case DataRowState.Deleted
                    If Not ValidateData(EnumAction.Delete) Then
                        Return
                    End If
                    Dim confirm As Boolean = (MessageBox.Show("Bạn có muốn xóa mặt hàng này không?", Me.Title, MessageBoxButton.YesNo) = MessageBoxResult.Yes)
                    If confirm Then
                        If Delete() Then
                            MessageBox.Show("Xóa  thành công.")
                            lblPropCodeHint.Content = ""
                            ProcessSelection.Mode = DataRowState.Added
                        Else
                            MessageBox.Show("Xóa không thành công.", Me.Title, MessageBoxButton.OK)
                        End If
                    End If
            End Select
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào nút Cập nhật.", ex)
        End Try


    End Sub
#End Region

#Region "ValidateData"
    Private Function ValidateData(action As EnumAction) As Boolean
        Dim hasError As Boolean
        Select Case action
            Case EnumAction.Insert
                If Validation.GetHasError(txtPropCode) Then
                    MessageBox.Show("Vui lòng nhập mã mặt hàng.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtPropCode.Focus()
                    Return False
                End If
                If Check.IsExisted("Property", txtPropCode.Text.Trim) Then
                    MessageBox.Show("Mã mặt hàng đã tồn tại.")
                    HelpCreatePropCode()
                    Return False
                End If
                If Validation.GetHasError(txtPropName) Then
                    MessageBox.Show("Vui lòng nhập tên mặt hàng.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtPropName.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtPurchasePrice) Then
                    MessageBox.Show("Vui lòng nhập giá mua.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtPurchasePrice.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtUnit) Then
                    MessageBox.Show("Vui lòng nhập đơn vị tính.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtUnit.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtSalesPrice) Then
                    MessageBox.Show("Vui lòng nhập giá bán.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtSalesPrice.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtCurrentValue) Then
                    MessageBox.Show("Vui lòng nhập giá hiện tại.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtCurrentValue.Focus()
                    Return False
                End If
            Case EnumAction.Update
                If Validation.GetHasError(txtPropCode) Then
                    MessageBox.Show("Vui lòng nhập mã mặt hàng.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtPropCode.Focus()
                    Return False
                End If
                If Not Check.IsExisted("Property", txtPropCode.Text.Trim) Then
                    MessageBox.Show("Mã mặt hàng chưa được đăng ký hoặc đã bị xóa.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtPropCode.Focus()
                    HelpGetLastPropCode()
                    Return False
                End If
                If Validation.GetHasError(txtPropName) Then
                    MessageBox.Show("Vui lòng nhập tên mặt hàng.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtPropName.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtPurchasePrice) Then
                    MessageBox.Show("Vui lòng nhập giá mua.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtPurchasePrice.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtUnit) Then
                    MessageBox.Show("Vui lòng nhập đơn vị tính.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtUnit.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtSalesPrice) Then
                    MessageBox.Show("Vui lòng nhập giá bán.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtSalesPrice.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtCurrentValue) Then
                    MessageBox.Show("Vui lòng nhập giá hiện tại.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtCurrentValue.Focus()
                    Return False
                End If
            Case EnumAction.Delete
                If Validation.GetHasError(txtPropCode) Then
                    MessageBox.Show("Vui lòng nhập mã mặt hàng.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtPropCode.Focus()
                    Return False
                End If
                If Not Check.IsExisted("Property", txtPropCode.Text.Trim) Then
                    MessageBox.Show("Mã mặt hàng chưa được đăng ký hoặc đã bị xóa.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtPropCode.Focus()
                    HelpGetLastPropCode()
                    Return False
                End If
                If Validation.GetHasError(txtRetiredDate) Then
                    MessageBox.Show("Vui lòng nhập ngày xóa.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtRetiredDate.Focus()
                    Return False
                End If

        End Select
        Return Not hasError
    End Function
#End Region

#Region "INSERT"
    Private Function Insert() As Boolean
        Dim dbConn As New DbConnect()
        Dim res As Integer

        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim sSQL As String = InsertSQL()
            Using cmd As New SqlCommand(sSQL, dbConn.Conn)
                cmd.Transaction = dbConn.Tran
                Dim row As PMS_ATOMYDataSet.PropertyRow = AtomyDataSet._Property.Rows(0)
                Dim now As Date = Date.Now
                row.CreateDate = now.ToString("yyyy/MM/dd")
                row.CreateTime = now.ToString("HH:mm:ss")
                row.CreateUser = Utility.LoginUserCode
                row.UpdateDate = now.ToString("yyyy/MM/dd")
                row.UpdateTime = now.ToString("HH:mm:ss")
                row.UpdateUser = Utility.LoginUserCode

                cmd.Parameters.AddWithValue("@PropCode", row.PropCode)
                cmd.Parameters.AddWithValue("@PropName", row.PropName)
                cmd.Parameters.AddWithValue("@Description", row.Description)
                cmd.Parameters.AddWithValue("@Category", row.Category)
                cmd.Parameters.AddWithValue("@Condition", row.Condition)
                cmd.Parameters.AddWithValue("@AcquiredDate", row.AcquiredDate)
                cmd.Parameters.AddWithValue("@Unit", row.Unit)
                cmd.Parameters.AddWithValue("@PurchasePrice", row.PurchasePrice)
                cmd.Parameters.AddWithValue("@SalesPrice", row.SalesPrice)
                cmd.Parameters.AddWithValue("@CurrentValue", row.CurrentValue)
                cmd.Parameters.AddWithValue("@Location", row.Location)
                cmd.Parameters.AddWithValue("@Manufacturer", row.Manufacturer)
                cmd.Parameters.AddWithValue("@Model", row.Model)
                cmd.Parameters.AddWithValue("@Comments", row.Comments)
                cmd.Parameters.AddWithValue("@CreateDate", row.CreateDate)
                cmd.Parameters.AddWithValue("@CreateTime", row.CreateTime)
                cmd.Parameters.AddWithValue("@CreateUser", row.CreateUser)
                cmd.Parameters.AddWithValue("@UpdateDate", row.UpdateDate)
                cmd.Parameters.AddWithValue("@UpdateTime", row.UpdateTime)
                cmd.Parameters.AddWithValue("@UpdateUser", row.UpdateUser)

                res = cmd.ExecuteNonQuery()

            End Using

            dbConn.CommitTran()
        Catch ex As Exception
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi cập nhật mặt hàng.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res
    End Function
#End Region

#Region "UPDATE"
    Private Function Update() As Boolean
        Dim dbConn As New DbConnect()
        Dim res As Integer

        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim sSQL As String = UpdateSQL()
            Dim cmd As New SqlCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            Dim row As PMS_ATOMYDataSet.PropertyRow = AtomyDataSet._Property.Rows(0)
            Dim now As Date = Date.Now
            row.CreateDate = now.ToString("yyyy/MM/dd")
            row.CreateTime = now.ToString("HH:mm:ss")
            row.CreateUser = Utility.LoginUserCode
            row.UpdateDate = now.ToString("yyyy/MM/dd")
            row.UpdateTime = now.ToString("HH:mm:ss")
            row.UpdateUser = Utility.LoginUserCode

            cmd.Parameters.AddWithValue("@PropName", row.PropName)
            cmd.Parameters.AddWithValue("@Description", row.Description)
            cmd.Parameters.AddWithValue("@Category", row.Category)
            cmd.Parameters.AddWithValue("@Condition", row.Condition)
            cmd.Parameters.AddWithValue("@AcquiredDate", row.AcquiredDate)
            cmd.Parameters.AddWithValue("@Unit", row.Unit)
            cmd.Parameters.AddWithValue("@PurchasePrice", row.PurchasePrice)
            cmd.Parameters.AddWithValue("@SalesPrice", row.SalesPrice)
            cmd.Parameters.AddWithValue("@CurrentValue", row.CurrentValue)
            cmd.Parameters.AddWithValue("@Location", row.Location)
            cmd.Parameters.AddWithValue("@Manufacturer", row.Manufacturer)
            cmd.Parameters.AddWithValue("@Model", row.Model)
            cmd.Parameters.AddWithValue("@Comments", row.Comments)
            cmd.Parameters.AddWithValue("@UpdateDate", row.UpdateDate)
            cmd.Parameters.AddWithValue("@UpdateTime", row.UpdateTime)
            cmd.Parameters.AddWithValue("@UpdateUser", row.UpdateUser)
            cmd.Parameters.AddWithValue("@PropCode", row.PropCode)

            res = cmd.ExecuteNonQuery()
            dbConn.CommitTran()
        Catch ex As Exception
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi cập nhật mặt hàng.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res
    End Function
#End Region

#Region "DELETE"
    Private Function Delete() As Boolean
        Dim dbConn As New DbConnect()
        Dim res As Integer

        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim sSQL As String = DeleteSQL()
            Dim cmd As New SqlCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            Dim row As PMS_ATOMYDataSet.PropertyRow = AtomyDataSet._Property.Rows(0)
            cmd.Parameters.AddWithValue("@Retired", True)
            cmd.Parameters.AddWithValue("@RetiredDate", row.RetiredDate)
            cmd.Parameters.AddWithValue("@PropCode", row.PropCode)

            res = cmd.ExecuteNonQuery()
            dbConn.CommitTran()
        Catch ex As Exception
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi xóa mặt hàng.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res
    End Function
#End Region

#Region "HelpCreateCode"
    Private Sub HelpCreatePropCode()
        lblPropCodeHint.Content = "Gợi ý: " + Utility.HelpCreateCode("Property")
    End Sub
#End Region

#Region "HelpGetLastPropCode"
    Private Sub HelpGetLastPropCode()
        lblPropCodeHint.Content = "Mã gần nhất: " + Utility.HelpGetLastCode("Property")
    End Sub
#End Region
#End Region



#Region "☆ SQL"
#Region "InsertPropertySQL"
    Private Function InsertSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("INSERT INTO [Property]                               ")
        sb.AppendLine("            ( [PropCode], [PropName], [Description], [Category], [Condition], [AcquiredDate], [Unit], [PurchasePrice], [SalesPrice], [CurrentValue], [Location], [Manufacturer], [Model], [Comments], [CreateDate], [CreateTime], [CreateUser], [UpdateDate], [UpdateTime], [UpdateUser]) ")
        sb.AppendLine("     VALUES ( @PropCode, @PropName, @Description, @Category, @Condition, @AcquiredDate, @Unit, @PurchasePrice, @SalesPrice, @CurrentValue, @Location, @Manufacturer, @Model, @Comments, @CreateDate, @CreateTime, @CreateUser, @UpdateDate, @UpdateTime, @UpdateUser) ")
        Return sb.ToString()
    End Function
#End Region

#Region "UpdatePropertySQL"
    Private Function UpdateSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Property]                                            ")
        sb.AppendLine("   SET [PropName] = @PropName                                ")
        sb.AppendLine("     , [Description] = @Description                          ")
        sb.AppendLine("     , [Category] = @Category                                ")
        sb.AppendLine("     , [Condition] = @Condition                              ")
        sb.AppendLine("     , [AcquiredDate] = @AcquiredDate                        ")
        sb.AppendLine("     , [Unit] = @Unit                                        ")
        sb.AppendLine("     , [PurchasePrice] = @PurchasePrice                      ")
        sb.AppendLine("     , [SalesPrice] = @SalesPrice                            ")
        sb.AppendLine("     , [CurrentValue] = @CurrentValue                        ")
        sb.AppendLine("     , [Location] = @Location                                ")
        sb.AppendLine("     , [Manufacturer] = @Manufacturer                        ")
        sb.AppendLine("     , [Model] = @Model                                      ")
        sb.AppendLine("     , [Comments] = @Comments                                ")
        sb.AppendLine("     , [UpdateDate] = @UpdateDate                            ")
        sb.AppendLine("     , [UpdateTime] = @UpdateTime                            ")
        sb.AppendLine("     , [UpdateUser] = @UpdateUser                            ")
        sb.AppendLine(" WHERE [PropCode] = @PropCode                                ")
        Return sb.ToString()
    End Function
#End Region

#Region "DeletePropertySQL"
    Private Function DeleteSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Property]                                            ")
        sb.AppendLine("   SET [Retired] = @Retired                                  ")
        sb.AppendLine("     , [RetiredDate] = @RetiredDate                          ")
        sb.AppendLine(" WHERE [PropCode] = @PropCode                                ")
        Return sb.ToString()
    End Function
#End Region
#End Region

#Region "EVENT"
#Region "searchSearchResult"
    Private Sub searchSearchResult(sender As Object, e As SearchDataArgs)
        LoadData(e.Code)
    End Sub
#End Region

#Region "lnkPropCd_Click"
    Private Sub lnkPropCd_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim search As New Search()
            AddHandler search.SearchResult, AddressOf searchSearchResult
            search.Kind = EnumSearch.SearchProperty
            search.ShowDialog()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào link Mã mặt hàng.", ex)
        End Try
    End Sub
#End Region

#Region "txtCode_LostFocus"
    Private Sub txtCode_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            Dim txtCode = DirectCast(sender, TextBox)
            Dim s = txtCode.Text.Trim()
            If s.Length = 0 Then
                Return
            End If
            If s.Length < 8 Then
                Dim lead As String = New String("0", 8 - s.Length)
                s = lead + s
                txtCode.Text = s
            End If
            If Mode = DataRowState.Added Then
                If txtCode.Equals(txtPropCode) AndAlso txtPropCode.Text.Trim.Length > 0 AndAlso Check.IsExisted("Property", txtPropCode.Text.Trim) Then
                    MessageBox.Show("Mã mặt hàng đã tồn tại.", Utility.AppCaption)
                    txtPropCode.Text = ""
                End If
            ElseIf Mode = DataRowState.Modified OrElse Mode = DataRowState.Deleted Then
                LoadData(txtPropCode.Text.Trim)
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô mã.", ex)
        End Try
    End Sub
#End Region
#End Region
End Class
