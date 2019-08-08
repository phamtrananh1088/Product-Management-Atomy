Imports System.Data.OleDb
Imports System.Data
Imports System.Text
Public Class Customer

#Region "FIELD"
    Private AtomyDataSet As AtomyDataSet
    Private Mode As DataRowState
#End Region

#Region "CONSTRUCTOR"
    Public Sub New()
        AtomyDataSet = New AtomyDataSet()
        ' This call is required by the designer.
        InitializeComponent()
        InitialValue()
        cboProvince.ItemsSource = Province.GetAllProvinces()
        ProcessSelection.Mode = DataRowState.Added
        ' Add any initialization after the InitializeComponent() call.
    End Sub
#End Region

#Region "InitialControl"
    Private Sub InitialValue()
        txtCusCode.Text = ""
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtMobilePhone.Text = ""
        txtFacebookID.Text = ""
        txtEmailAddress.Text = ""
        txtAddress.Text = ""
        txtCompany.Text = ""
        txtBusinessPhone.Text = ""
        txtHomePhone.Text = ""
        txtFaxNumber.Text = ""
        txtCity.Text = ""
        cboProvince.Text = ""
        txtZip.Text = ""
        txtCountry.Text = ""
        txtWebPage.Text = ""
        txtNotes.Text = ""
    End Sub
#End Region

#Region "LoadData"
    Private Sub LoadData(CusCode As String)
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim sSQL As String = "select * from [Customer] where [CusCode] = ?"
            Dim adapt As New OleDbDataAdapter(sSQL, dbConn.Conn)
            adapt.SelectCommand.Parameters.Add("@CusCode", OleDbType.VarChar).Value = CusCode
            AtomyDataSet.Customer.Clear()

            If adapt.Fill(AtomyDataSet, "Customer") > 0 Then
                Me.DataContext = AtomyDataSet.Customer.Rows(0)
            Else
                MessageBox.Show("Khách hàng [" + CusCode + "] không tồn tại hoặc đã bị xóa.")
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
            txtFirstName.IsEnabled = True
            txtLastName.IsEnabled = True
            txtMobilePhone.IsEnabled = True
            txtFacebookID.IsEnabled = True
            txtEmailAddress.IsEnabled = True
            txtAddress.IsEnabled = True
            txtCompany.IsEnabled = True
            txtBusinessPhone.IsEnabled = True
            txtHomePhone.IsEnabled = True
            txtFaxNumber.IsEnabled = True
            txtCity.IsEnabled = True
            cboProvince.IsEnabled = True
            txtZip.IsEnabled = True
            txtCountry.IsEnabled = True
            txtWebPage.IsEnabled = True
            txtNotes.IsEnabled = True

        ElseIf Mode = DataRowState.Modified Then
            txtFirstName.IsEnabled = True
            txtLastName.IsEnabled = True
            txtMobilePhone.IsEnabled = True
            txtFacebookID.IsEnabled = True
            txtEmailAddress.IsEnabled = True
            txtAddress.IsEnabled = True
            txtCompany.IsEnabled = True
            txtBusinessPhone.IsEnabled = True
            txtHomePhone.IsEnabled = True
            txtFaxNumber.IsEnabled = True
            txtCity.IsEnabled = True
            cboProvince.IsEnabled = True
            txtZip.IsEnabled = True
            txtCountry.IsEnabled = True
            txtWebPage.IsEnabled = True
            txtNotes.IsEnabled = True

        ElseIf Me.Mode = DataRowState.Deleted Then
            txtFirstName.IsEnabled = False
            txtLastName.IsEnabled = False
            txtMobilePhone.IsEnabled = False
            txtFacebookID.IsEnabled = False
            txtEmailAddress.IsEnabled = False
            txtAddress.IsEnabled = False
            txtCompany.IsEnabled = False
            txtBusinessPhone.IsEnabled = False
            txtHomePhone.IsEnabled = False
            txtFaxNumber.IsEnabled = False
            txtCity.IsEnabled = False
            cboProvince.IsEnabled = False
            txtZip.IsEnabled = False
            txtCountry.IsEnabled = False
            txtWebPage.IsEnabled = False
            txtNotes.IsEnabled = False

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
                    If Check.IsExisted("Customer", txtCusCode.Text) Then
                        MessageBox.Show("Mã khách hàng đã tồn tại.")
                        HelpCreateCusCode()
                        Return
                    End If

                    If InsertCustomer() Then
                        MessageBox.Show("Cập nhật thành công.", Me.Title, MessageBoxButton.OK)
                        lblCusCodeHint.Content = ""
                        ProcessSelection.Mode = DataRowState.Modified
                        LoadData(txtCusCode.Text.Trim)
                    Else
                        MessageBox.Show("Cập nhật không thành công.", Me.Title, MessageBoxButton.OK)
                    End If
                Case DataRowState.Modified
                    If Not ValidateData(EnumAction.Update) Then
                        Return
                    End If
                    If UpdateCustomer() Then
                        MessageBox.Show("Cập nhật thành công.", Me.Title, MessageBoxButton.OK)
                        lblCusCodeHint.Content = ""
                        LoadData(txtCusCode.Text.Trim)
                    Else
                        MessageBox.Show("Cập nhật không thành công.", Me.Title, MessageBoxButton.OK)
                    End If
                Case DataRowState.Deleted
                    If Not ValidateData(EnumAction.Delete) Then
                        Return
                    End If
                    Dim confirm As Boolean = (MessageBox.Show("Bạn có muốn xóa mặt hàng này không?", "Atomy", MessageBoxButton.YesNo) = MessageBoxResult.OK)
                    If confirm Then
                        If DeleteCustomer() Then
                            MessageBox.Show("Xóa thành công.", Me.Title, MessageBoxButton.OK)
                            lblCusCodeHint.Content = ""
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

#Region "ProcessSelection_ValueChange"
    Private Sub ProcessSelection_ValueChange(sender As Object, e As EventArgs)
        If ProcessSelection.Mode = DataRowState.Added Then
            AtomyDataSet.Customer.Clear()
            Dim newRow As AtomyDataSet.CustomerRow = AtomyDataSet.Customer.NewCustomerRow()
            AtomyDataSet.Customer.Rows.Add(newRow)
            Me.DataContext = AtomyDataSet.Customer.Rows(0)
            Mode = DataRowState.Added
            CtrEnable()
            HelpCreateCusCode()
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

#Region "lnkCusCode_Click"
    Private Sub lnkCusCode_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim search As New Search()
            AddHandler search.SearchResult, AddressOf searchSearchResult
            search.Kind = EnumSearch.SearchCustomer
            search.ShowDialog()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào link Mã khách hàng.", ex)
        End Try
    End Sub
#End Region

#Region "BUSINESS"
#Region "ValidateData"
    Private Function ValidateData(action As EnumAction) As Boolean
        Dim hasError As Boolean
        Select Case action
            Case EnumAction.Update
                hasError = Validation.GetHasError(txtCusCode)
                hasError = hasError OrElse Validation.GetHasError(txtFirstName)
                hasError = hasError OrElse Validation.GetHasError(txtLastName)
                hasError = hasError OrElse Validation.GetHasError(txtMobilePhone)
            Case EnumAction.Insert
                hasError = Validation.GetHasError(txtCusCode)
                hasError = hasError OrElse Validation.GetHasError(txtFirstName)
                hasError = hasError OrElse Validation.GetHasError(txtLastName)
                hasError = hasError OrElse Validation.GetHasError(txtMobilePhone)
            Case EnumAction.Delete
                hasError = Validation.GetHasError(txtCusCode)
        End Select
        Return Not hasError
    End Function
#End Region

#Region "DeleteCustomer"
    Private Function DeleteCustomer() As Boolean
        Dim dbConn As New DbConnect()
        Dim res As Integer

        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim sSQL As String = DeleteCustomerSQL()
            Dim cmd As New OleDbCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            Dim row As AtomyDataSet.CustomerRow = AtomyDataSet.Customer.Rows(0)
            cmd.Parameters.Add("@1", OleDbType.Boolean).Value = True
            cmd.Parameters.Add("@2", OleDbType.VarChar).Value = New Date().ToString("yyyy/MM/dd")
            cmd.Parameters.Add("@3", OleDbType.VarChar).Value = row.CusCode

            res = cmd.ExecuteNonQuery()
            dbConn.CommitTran()
        Catch ex As Exception
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi xóa khách hàng.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res
    End Function
#End Region

#Region "InsertCustomer"
    Private Function InsertCustomer() As Boolean
        Dim dbConn As New DbConnect()
        Dim res As Integer

        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim sSQL As String = InsertCustomerSQL()
            Using cmd As New OleDbCommand(sSQL, dbConn.Conn)
                cmd.Transaction = dbConn.Tran
                Dim row As AtomyDataSet.CustomerRow = AtomyDataSet.Customer.Rows(0)
                Dim now As Date = Date.Now
                row.CreateDate = now.ToString("yyyy/MM/dd")
                row.CreateTime = now.ToString("HH:mm:ss")
                row.CreateUser = Utility.LoginUserCode
                row.UpdateDate = now.ToString("yyyy/MM/dd")
                row.UpdateTime = now.ToString("HH:mm:ss")
                row.UpdateUser = Utility.LoginUserCode

                cmd.Parameters.Add("@CusCode", OleDbType.VarChar).Value = row.CusCode
                cmd.Parameters.Add("@Company", OleDbType.VarChar).Value = row.Company
                cmd.Parameters.Add("@LastName", OleDbType.VarChar).Value = row.LastName
                cmd.Parameters.Add("@FirstName", OleDbType.VarChar).Value = row.FirstName
                cmd.Parameters.Add("@EmailAddress", OleDbType.VarChar).Value = row.EmailAddress
                cmd.Parameters.Add("@JobTitle", OleDbType.VarChar).Value = row.JobTitle
                cmd.Parameters.Add("@BusinessPhone", OleDbType.VarChar).Value = row.BusinessPhone
                cmd.Parameters.Add("@HomePhone", OleDbType.VarChar).Value = row.HomePhone
                cmd.Parameters.Add("@MobilePhone", OleDbType.VarChar).Value = row.MobilePhone
                cmd.Parameters.Add("@FaxNumber", OleDbType.VarChar).Value = row.FaxNumber
                cmd.Parameters.Add("@Address", OleDbType.VarChar).Value = row.Address
                cmd.Parameters.Add("@City", OleDbType.VarChar).Value = row.City
                cmd.Parameters.Add("@StateProvince", OleDbType.VarChar).Value = row.StateProvince
                cmd.Parameters.Add("@ZIPPostalCode", OleDbType.VarChar).Value = row.ZIPPostalCode
                cmd.Parameters.Add("@CountryRegion", OleDbType.VarChar).Value = row.CountryRegion
                cmd.Parameters.Add("@WebPage", OleDbType.VarChar).Value = row.WebPage
                cmd.Parameters.Add("@FacebookID", OleDbType.VarChar).Value = row.FacebookID
                cmd.Parameters.Add("@Notes", OleDbType.VarChar).Value = row.Notes
                cmd.Parameters.Add("@Retired", OleDbType.Boolean).Value = row.Retired
                cmd.Parameters.Add("@RetiredDate", OleDbType.VarChar).Value = row.RetiredDate
                cmd.Parameters.Add("@CreateDate", OleDbType.VarChar).Value = row.CreateDate
                cmd.Parameters.Add("@CreateTime", OleDbType.VarChar).Value = row.CreateTime
                cmd.Parameters.Add("@CreateUser", OleDbType.VarChar).Value = row.CreateUser
                cmd.Parameters.Add("@UpdateDate", OleDbType.VarChar).Value = row.UpdateDate
                cmd.Parameters.Add("@UpdateTime", OleDbType.VarChar).Value = row.UpdateTime
                cmd.Parameters.Add("@UpdateUser", OleDbType.VarChar).Value = row.UpdateUser

                res = cmd.ExecuteNonQuery()

            End Using

            dbConn.CommitTran()
        Catch ex As Exception
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi cập nhật khách hàng.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res
    End Function
#End Region

#Region "UpdateCustomer"
    Private Function UpdateCustomer() As Boolean
        Dim dbConn As New DbConnect()
        Dim res As Integer

        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim sSQL As String = UpdateCustomerSQL()
            Dim cmd As New OleDbCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            Dim row As AtomyDataSet.CustomerRow = AtomyDataSet.Customer.Rows(0)
            Dim now As Date = Date.Now
            row.CreateDate = now.ToString("yyyy/MM/dd")
            row.CreateTime = now.ToString("HH:mm:ss")
            row.CreateUser = Utility.LoginUserCode
            row.UpdateDate = now.ToString("yyyy/MM/dd")
            row.UpdateTime = now.ToString("HH:mm:ss")
            row.UpdateUser = Utility.LoginUserCode

            cmd.Parameters.Add("@Company", OleDbType.VarChar).Value = row.Company
            cmd.Parameters.Add("@LastName", OleDbType.VarChar).Value = row.LastName
            cmd.Parameters.Add("@FirstName", OleDbType.VarChar).Value = row.FirstName
            cmd.Parameters.Add("@EmailAddress", OleDbType.VarChar).Value = row.EmailAddress
            cmd.Parameters.Add("@JobTitle", OleDbType.VarChar).Value = row.JobTitle
            cmd.Parameters.Add("@BusinessPhone", OleDbType.VarChar).Value = row.BusinessPhone
            cmd.Parameters.Add("@HomePhone", OleDbType.VarChar).Value = row.HomePhone
            cmd.Parameters.Add("@MobilePhone", OleDbType.VarChar).Value = row.MobilePhone
            cmd.Parameters.Add("@FaxNumber", OleDbType.VarChar).Value = row.FaxNumber
            cmd.Parameters.Add("@Address", OleDbType.VarChar).Value = row.Address
            cmd.Parameters.Add("@City", OleDbType.VarChar).Value = row.City
            cmd.Parameters.Add("@StateProvince", OleDbType.VarChar).Value = row.StateProvince
            cmd.Parameters.Add("@ZIPPostalCode", OleDbType.VarChar).Value = row.ZIPPostalCode
            cmd.Parameters.Add("@CountryRegion", OleDbType.VarChar).Value = row.CountryRegion
            cmd.Parameters.Add("@WebPage", OleDbType.VarChar).Value = row.WebPage
            cmd.Parameters.Add("@FacebookID", OleDbType.VarChar).Value = row.FacebookID
            cmd.Parameters.Add("@Notes", OleDbType.VarChar).Value = row.Notes
            cmd.Parameters.Add("@Retired", OleDbType.Boolean).Value = row.Retired
            cmd.Parameters.Add("@RetiredDate", OleDbType.VarChar).Value = row.RetiredDate
            cmd.Parameters.Add("@UpdateDate", OleDbType.VarChar).Value = row.UpdateDate
            cmd.Parameters.Add("@UpdateTime", OleDbType.VarChar).Value = row.UpdateTime
            cmd.Parameters.Add("@UpdateUser", OleDbType.VarChar).Value = row.UpdateUser
            cmd.Parameters.Add("@CusCode", OleDbType.VarChar).Value = row.CusCode

            res = cmd.ExecuteNonQuery()
            dbConn.CommitTran()
        Catch ex As Exception
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi cập nhật khách hàng.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res
    End Function
#End Region

#Region "HelpCreateCusCode"
    Private Sub HelpCreateCusCode()
        lblCusCodeHint.Content = "Gợi ý: " + Utility.HelpCreateCode("Customer")
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
                If txtCode.Equals(txtCusCode) AndAlso txtCusCode.Text.Trim.Length > 0 AndAlso Check.IsExisted("Customer", txtCusCode.Text.Trim) Then
                    MessageBox.Show("Mã khách hàng đã tồn tại.", Me.Title)
                    txtCusCode.Text = ""
                End If
            ElseIf Mode = DataRowState.Modified OrElse Mode = DataRowState.Deleted Then
                LoadData(txtCusCode.Text.Trim)
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô mã.", ex)
        End Try
    End Sub
#End Region

#Region "☆ SQL"
#Region "InsertCustomerSQL"
    Private Function InsertCustomerSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("INSERT INTO [Customer]                               ")
        sb.AppendLine("            ( [CusCode],[Company],[LastName],[FirstName], [EmailAddress], [JobTitle], [BusinessPhone], [HomePhone], [MobilePhone], [FaxNumber], [Address], [City], [StateProvince], [ZipPostalCode], [CountryRegion], [WebPage], [FacebookID],[Notes],[Retired],[RetiredDate], [CreateDate], [CreateTime], [CreateUser], [UpdateDate], [UpdateTime], [UpdateUser]) ")
        sb.AppendLine("     VALUES ( ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)                                          ")
        Return sb.ToString()
    End Function
#End Region

#Region "UpdateCustomerSQL"
    Private Function UpdateCustomerSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Customer]                                ")
        sb.AppendLine("   set [Company] = ?,[LastName] = ?,[FirstName] = ?, [EmailAddress] = ?, [JobTitle] = ?, [BusinessPhone] = ?, [HomePhone] = ?, [MobilePhone] = ?, [FaxNumber] = ?, [Address] = ?, [City] = ?, [StateProvince] = ?, [ZipPostalCode] = ?, [CountryRegion] = ?, [WebPage] = ?, [FacebookID] = ?,[Notes] = ?,[Retired] = ?,[RetiredDate] = ?,[UpdateDate] = ?,[UpdateTime] = ?,[UpdateUser] = ? ")
        sb.AppendLine(" WHERE [CusCode] = ?                            ")
        Return sb.ToString()
    End Function
#End Region

#Region "DeleteCustomerSQL"
    Private Function DeleteCustomerSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Customer]                                ")
        sb.AppendLine("   SET [Retired] = ?                             ")
        sb.AppendLine("     , [RetiredDate] = ?                        ")
        sb.AppendLine(" WHERE [CusCode] = ?                            ")
        Return sb.ToString()
    End Function
#End Region
#End Region

End Class


