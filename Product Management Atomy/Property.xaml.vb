Imports System.Data.OleDb
Imports System.Data
Imports System.Text

Public Class Property1
#Region "FIELD"
    Private AtomyDataSet As AtomyDataSet
    Private Mode As DataRowState
#End Region

#Region "CONSTRUCTOR"
    Public Sub New()
        AtomyDataSet = New AtomyDataSet()
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
            Dim sSQL As String = "select * from [Property] where [PropCode] = ?"
            Dim adapt As New OleDbDataAdapter(sSQL, dbConn.Conn)
            adapt.SelectCommand.Parameters.Add("@PropCode", OleDbType.VarChar).Value = PropCode
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

        End If

    End Sub
#End Region

#Region "btnProcess_Click"
    Private Sub btnProcess_Click(sender As Object, e As RoutedEventArgs)
        Try
            Select Case Mode
                Case DataRowState.Added
                    If Not ValidateData(EnumAction.Insert) Then
                        Return
                    End If
                    If Check.IsExisted("Property", txtPropCode.Text.Trim) Then
                        MessageBox.Show("Mã mặt hàng đã tồn tại.")
                        HelpCreateCode()
                        Return
                    End If

                    If Insert() Then
                        MessageBox.Show("Đã hoàn thành.")
                        lblPropCodeHint.Content = ""
                        LoadData(txtPropCode.Text.Trim)
                    Else
                        MessageBox.Show("Không thành công.")
                    End If
                Case DataRowState.Modified
                    If Not ValidateData(EnumAction.Update) Then
                        Return
                    End If
                    If Update() Then
                        MessageBox.Show("Đã hoàn thành.")
                        lblPropCodeHint.Content = ""
                        LoadData(txtPropCode.Text.Trim)
                    Else
                        MessageBox.Show("Không thành công.")
                    End If
                Case DataRowState.Deleted
                    If Not ValidateData(EnumAction.Delete) Then
                        Return
                    End If
                    Dim confirm As Boolean = (MessageBox.Show("Bạn có muốn xóa mặt hàng này không?", "Atomy", MessageBoxButton.YesNo) = MessageBoxResult.OK)
                    If confirm Then
                        If Delete() Then
                            MessageBox.Show("Đã hoàn thành.")
                            lblPropCodeHint.Content = ""
                            ProcessSelection.Mode = DataRowState.Added
                        End If
                    End If
            End Select
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào nút Cập nhật.", ex)
        End Try


    End Sub
#End Region

#Region "ProcessSelection_ValueChange"
    Private Sub ProcessSelection_ValueChange(sender As Object, e As EventArgs)
        If ProcessSelection.Mode = DataRowState.Added Then
            AtomyDataSet._Property.Clear()
            Dim newRow As AtomyDataSet.PropertyRow = AtomyDataSet._Property.NewPropertyRow()
            AtomyDataSet._Property.Rows.Add(newRow)
            Me.DataContext = AtomyDataSet._Property.Rows(0)
            Mode = DataRowState.Added
            CtrEnable()
            HelpCreateCode()
        ElseIf ProcessSelection.Mode = DataRowState.Modified Then
            Me.Mode = DataRowState.Modified
            CtrEnable()
        ElseIf ProcessSelection.Mode = DataRowState.Deleted Then
            Me.Mode = DataRowState.Deleted
            CtrEnable()
        End If
    End Sub
#End Region

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

#Region "BUSINESS"
#Region "ValidateData"
    Private Function ValidateData(action As EnumAction) As Boolean
        Dim hasError As Boolean
        Select Case action
            Case EnumAction.Update
                hasError = Validation.GetHasError(txtPropCode)
                hasError = hasError OrElse Validation.GetHasError(txtPropName)
                hasError = hasError OrElse Validation.GetHasError(txtSalesPrice)
                hasError = hasError OrElse Validation.GetHasError(txtUnit)
                hasError = hasError OrElse Validation.GetHasError(txtPurchasePrice)
                hasError = hasError OrElse Validation.GetHasError(txtCurrentValue)
            Case EnumAction.Insert
                hasError = Validation.GetHasError(txtPropCode)
                hasError = hasError OrElse Validation.GetHasError(txtPropName)
                hasError = hasError OrElse Validation.GetHasError(txtSalesPrice)
                hasError = hasError OrElse Validation.GetHasError(txtUnit)
                hasError = hasError OrElse Validation.GetHasError(txtPurchasePrice)
                hasError = hasError OrElse Validation.GetHasError(txtCurrentValue)
            Case EnumAction.Delete
                hasError = Validation.GetHasError(txtPropCode)
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
            Using cmd As New OleDbCommand(sSQL, dbConn.Conn)
                cmd.Transaction = dbConn.Tran
                Dim row As AtomyDataSet.PropertyRow = AtomyDataSet._Property.Rows(0)
                Dim now As Date = Date.Now
                row.CreateDate = now.ToString("yyyy/MM/dd")
                row.CreateTime = now.ToString("HH:mm:ss")
                row.CreateUser = Utility.LoginUserCode
                row.UpdateDate = now.ToString("yyyy/MM/dd")
                row.UpdateTime = now.ToString("HH:mm:ss")
                row.UpdateUser = Utility.LoginUserCode

                cmd.Parameters.Add("@1", OleDbType.VarChar).Value = row.PropCode
                cmd.Parameters.Add("@2", OleDbType.VarChar).Value = row.PropName
                cmd.Parameters.Add("@3", OleDbType.VarChar).Value = row.Description
                cmd.Parameters.Add("@4", OleDbType.VarChar).Value = row.Category
                cmd.Parameters.Add("@5", OleDbType.VarChar).Value = row.Condition
                cmd.Parameters.Add("@6", OleDbType.VarChar).Value = row.AcquiredDate
                cmd.Parameters.Add("@7", OleDbType.VarChar).Value = row.Unit
                cmd.Parameters.Add("@8", OleDbType.Currency).Value = row.PurchasePrice
                cmd.Parameters.Add("@9", OleDbType.Currency).Value = row.SalesPrice
                cmd.Parameters.Add("@10", OleDbType.Currency).Value = row.CurrentValue
                cmd.Parameters.Add("@11", OleDbType.VarChar).Value = row.Location
                cmd.Parameters.Add("@12", OleDbType.VarChar).Value = row.Manufacturer
                cmd.Parameters.Add("@13", OleDbType.VarChar).Value = row.Model
                cmd.Parameters.Add("@14", OleDbType.VarChar).Value = row.Comments
                cmd.Parameters.Add("@16", OleDbType.VarChar).Value = row.CreateDate
                cmd.Parameters.Add("@17", OleDbType.VarChar).Value = row.CreateTime
                cmd.Parameters.Add("@18", OleDbType.VarChar).Value = row.CreateUser
                cmd.Parameters.Add("@19", OleDbType.VarChar).Value = row.UpdateDate
                cmd.Parameters.Add("@20", OleDbType.VarChar).Value = row.UpdateTime
                cmd.Parameters.Add("@21", OleDbType.VarChar).Value = row.UpdateUser

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
            Dim cmd As New OleDbCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            Dim row As AtomyDataSet.PropertyRow = AtomyDataSet._Property.Rows(0)
            Dim now As Date = Date.Now
            row.CreateDate = now.ToString("yyyy/MM/dd")
            row.CreateTime = now.ToString("HH:mm:ss")
            row.CreateUser = Utility.LoginUserCode
            row.UpdateDate = now.ToString("yyyy/MM/dd")
            row.UpdateTime = now.ToString("HH:mm:ss")
            row.UpdateUser = Utility.LoginUserCode

            cmd.Parameters.Add("@PropName", OleDbType.VarChar).Value = row.PropName
            cmd.Parameters.Add("@Description", OleDbType.VarChar).Value = row.Description
            cmd.Parameters.Add("@Category", OleDbType.VarChar).Value = row.Category
            cmd.Parameters.Add("@Condition", OleDbType.VarChar).Value = row.Condition
            cmd.Parameters.Add("@AcquiredDate", OleDbType.VarChar).Value = row.AcquiredDate
            cmd.Parameters.Add("@Unit", OleDbType.VarChar).Value = row.Unit
            cmd.Parameters.Add("@PurchasePrice", OleDbType.Currency).Value = row.PurchasePrice
            cmd.Parameters.Add("@SalesPrice", OleDbType.Currency).Value = row.SalesPrice
            cmd.Parameters.Add("@CurrentValue", OleDbType.Currency).Value = row.CurrentValue
            cmd.Parameters.Add("@Location", OleDbType.VarChar).Value = row.Location
            cmd.Parameters.Add("@Manufacturer", OleDbType.VarChar).Value = row.Manufacturer
            cmd.Parameters.Add("@Model", OleDbType.VarChar).Value = row.Model
            cmd.Parameters.Add("@Comments", OleDbType.VarChar).Value = row.Comments
            cmd.Parameters.Add("@UpdateDate", OleDbType.VarChar).Value = row.UpdateDate
            cmd.Parameters.Add("@UpdateTime", OleDbType.VarChar).Value = row.UpdateTime
            cmd.Parameters.Add("@UpdateUser", OleDbType.VarChar).Value = row.UpdateUser
            cmd.Parameters.Add("@PropCode", OleDbType.VarChar).Value = row.PropCode

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
            Dim cmd As New OleDbCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            Dim row As AtomyDataSet.PropertyRow = AtomyDataSet._Property.Rows(0)
            cmd.Parameters.Add("@1", OleDbType.Boolean).Value = True
            cmd.Parameters.Add("@2", OleDbType.VarChar).Value = New Date().ToString("yyyy/MM/dd")
            cmd.Parameters.Add("@3", OleDbType.VarChar).Value = row.PropCode

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
    Private Sub HelpCreateCode()
        lblPropCodeHint.Content = "Gợi ý: " + Utility.HelpCreateCode("Property")
    End Sub

#End Region
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

#Region "☆ SQL"
#Region "InsertPropertySQL"
    Private Function InsertSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("INSERT INTO [Property]                               ")
        sb.AppendLine("            ( [PropCode], [PropName], [Description], [Category], [Condition], [AcquiredDate], [Unit], [PurchasePrice], [SalesPrice], [CurrentValue], [Location], [Manufacturer], [Model], [Comments], [CreateDate], [CreateTime], [CreateUser], [UpdateDate], [UpdateTime], [UpdateUser]) ")
        sb.AppendLine("     VALUES ( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)                                          ")
        Return sb.ToString()
    End Function
#End Region

#Region "UpdatePropertySQL"
    Private Function UpdateSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Property]                                ")
        sb.AppendLine("   SET [PropName] = ?                            ")
        sb.AppendLine("     , [Description] = ?                         ")
        sb.AppendLine("     , [Category] = ?                            ")
        sb.AppendLine("     , [Condition] = ?                           ")
        sb.AppendLine("     , [AcquiredDate] = ?                       ")
        sb.AppendLine("     , [Unit] = ?                                ")
        sb.AppendLine("     , [PurchasePrice] = ?                      ")
        sb.AppendLine("     , [SalesPrice] = ?                         ")
        sb.AppendLine("     , [CurrentValue] = ?                       ")
        sb.AppendLine("     , [Location] = ?                            ")
        sb.AppendLine("     , [Manufacturer] = ?                        ")
        sb.AppendLine("     , [Model] = ?                               ")
        sb.AppendLine("     , [Comments] = ?                            ")
        sb.AppendLine("     , [UpdateDate] = ?                         ")
        sb.AppendLine("     , [UpdateTime] = ?                         ")
        sb.AppendLine("     , [UpdateUser] = ?                         ")
        sb.AppendLine(" WHERE [PropCode] = ?                            ")
        Return sb.ToString()
    End Function
#End Region

#Region "DeletePropertySQL"
    Private Function DeleteSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Property]                                ")
        sb.AppendLine("   SET [Retired] = ?                             ")
        sb.AppendLine("     , [RetiredDate] = ?                        ")
        sb.AppendLine(" WHERE [PropCode] = ?                            ")
        Return sb.ToString()
    End Function
#End Region
#End Region

End Class
