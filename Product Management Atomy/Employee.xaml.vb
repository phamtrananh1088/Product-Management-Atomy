Imports System.Data.SqlClient
Imports System.Data
Imports System.Text

Public Class Employee

#Region "FIELD"
    Private AtomyDataSet As PMS_ATOMYDataSet
    Private Mode As DataRowState
#End Region

#Region "CONSTRUCTOR"
    Public Sub New()
        AtomyDataSet = New PMS_ATOMYDataSet()
        ' This call is required by the designer.
        InitializeComponent()
        cboProvince.ItemsSource = Province.GetAllProvinces()
        ProcessSelection.Mode = DataRowState.Added
        ' Add any initialization after the InitializeComponent() call.
    End Sub
#End Region

#Region "InitialControl"
    Private Sub InitialValue()
        txtEmpCode.Text = ""
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtDepartment.Text = ""
        txtPosition.Text = ""
        txtMobilePhone.Text = ""
        txtFacebookID.Text = ""
        txtEmailAddress.Text = ""
        txtAddress.Text = ""
        txtBusinessPhone.Text = ""
        txtHomePhone.Text = ""
        txtFaxNumber.Text = ""
        txtCity.Text = ""
        cboProvince.Text = ""
        txtZip.Text = ""
        txtCountry.Text = ""
        txtNotes.Text = ""
        lblRetiredDate.Visibility = Windows.Visibility.Hidden
        txtRetiredDate.Visibility = Windows.Visibility.Hidden
    End Sub
#End Region

#Region "LoadData"
    Private Sub LoadData(EmpCode As String)
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim sSQL As String = "select * from [Employee] where [EmpCode] = @EmpCode"
            Dim adapt As New SqlDataAdapter(sSQL, dbConn.Conn)
            adapt.SelectCommand.Parameters.AddWithValue("@EmpCode", EmpCode)
            AtomyDataSet.Employee.Clear()

            If adapt.Fill(AtomyDataSet, "Employee") > 0 Then
                Me.DataContext = AtomyDataSet.Employee.Rows(0)
            Else
                MessageBox.Show("Nhân viên [" + EmpCode + "] không tồn tại hoặc đã bị xóa.")
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
            txtDepartment.IsEnabled = True
            txtPosition.IsEnabled = True
            txtMobilePhone.IsEnabled = True
            txtFacebookID.IsEnabled = True
            txtEmailAddress.IsEnabled = True
            txtAddress.IsEnabled = True

            txtBusinessPhone.IsEnabled = True
            txtHomePhone.IsEnabled = True
            txtFaxNumber.IsEnabled = True
            txtCity.IsEnabled = True
            cboProvince.IsEnabled = True
            txtZip.IsEnabled = True
            txtCountry.IsEnabled = True

            txtNotes.IsEnabled = True
            lblRetiredDate.Visibility = Windows.Visibility.Hidden
            txtRetiredDate.Visibility = Windows.Visibility.Hidden
        ElseIf Mode = DataRowState.Modified Then
            txtFirstName.IsEnabled = True
            txtLastName.IsEnabled = True
            txtDepartment.IsEnabled = True
            txtPosition.IsEnabled = True
            txtMobilePhone.IsEnabled = True
            txtFacebookID.IsEnabled = True
            txtEmailAddress.IsEnabled = True
            txtAddress.IsEnabled = True

            txtBusinessPhone.IsEnabled = True
            txtHomePhone.IsEnabled = True
            txtFaxNumber.IsEnabled = True
            txtCity.IsEnabled = True
            cboProvince.IsEnabled = True
            txtZip.IsEnabled = True
            txtCountry.IsEnabled = True

            txtNotes.IsEnabled = True
            lblRetiredDate.Visibility = Windows.Visibility.Hidden
            txtRetiredDate.Visibility = Windows.Visibility.Hidden
        ElseIf Me.Mode = DataRowState.Deleted Then
            txtFirstName.IsEnabled = False
            txtLastName.IsEnabled = False
            txtDepartment.IsEnabled = False
            txtPosition.IsEnabled = False
            txtMobilePhone.IsEnabled = False
            txtFacebookID.IsEnabled = False
            txtEmailAddress.IsEnabled = False
            txtAddress.IsEnabled = False

            txtBusinessPhone.IsEnabled = False
            txtHomePhone.IsEnabled = False
            txtFaxNumber.IsEnabled = False
            txtCity.IsEnabled = False
            cboProvince.IsEnabled = False
            txtZip.IsEnabled = False
            txtCountry.IsEnabled = False

            txtNotes.IsEnabled = False
            lblRetiredDate.Visibility = Windows.Visibility.Visible
            txtRetiredDate.Visibility = Windows.Visibility.Visible
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
                    If Check.IsExisted("Employee", txtEmpCode.Text) Then
                        MessageBox.Show("Mã nhân viên đã tồn tại.")
                        HelpCreateEmpCode()
                        Return
                    End If

                    If InsertEmployee() Then
                        MessageBox.Show("Đã hoàn thành.")
                        lblEmpCodeHint.Content = ""
                        ProcessSelection.Mode = DataRowState.Modified
                        LoadData(txtEmpCode.Text)
                    Else
                        MessageBox.Show("Không thành công.")
                    End If
                Case DataRowState.Modified
                    If Not ValidateData(EnumAction.Update) Then
                        Return
                    End If
                    If UpdateEmployee() Then
                        MessageBox.Show("Đã hoàn thành.")
                        lblEmpCodeHint.Content = ""
                        LoadData(txtEmpCode.Text)
                    Else
                        MessageBox.Show("Không thành công.")
                    End If
                Case DataRowState.Deleted
                    If Not ValidateData(EnumAction.Delete) Then
                        Return
                    End If
                    Dim confirm As Boolean = (MessageBox.Show("Bạn có muốn xóa mặt hàng này không?", "Atomy", MessageBoxButton.YesNo) = MessageBoxResult.OK)
                    If confirm Then
                        If DeleteEmployee() Then
                            MessageBox.Show("Đã hoàn thành.")
                            lblEmpCodeHint.Content = ""
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
            AtomyDataSet.Employee.Clear()
            Dim newRow As PMS_ATOMYDataSet.EmployeeRow = AtomyDataSet.Employee.NewEmployeeRow()
            Utility.RowInit.InitEmployeeRow(newRow)
            AtomyDataSet.Employee.Rows.Add(newRow)
            Me.DataContext = AtomyDataSet.Employee.Rows(0)
            Mode = DataRowState.Added
            CtrEnable()
            HelpCreateEmpCode()
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

#Region "lnkEmpCode_Click"
    Private Sub lnkEmpCode_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim search As New Search()
            AddHandler search.SearchResult, AddressOf searchSearchResult
            search.Kind = EnumSearch.SearchEmployee
            search.ShowDialog()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào link Mã nhân viên.", ex)
        End Try
    End Sub
#End Region

#Region "BUSINESS"
#Region "ValidateData"
    Private Function ValidateData(action As EnumAction) As Boolean
        Dim hasError As Boolean
        Select Case action
            Case EnumAction.Update
                hasError = Validation.GetHasError(txtEmpCode)
                hasError = hasError OrElse Validation.GetHasError(txtFirstName)
                hasError = hasError OrElse Validation.GetHasError(txtLastName)
                hasError = hasError OrElse Validation.GetHasError(txtMobilePhone)
            Case EnumAction.Insert
                hasError = Validation.GetHasError(txtEmpCode)
                hasError = hasError OrElse Validation.GetHasError(txtFirstName)
                hasError = hasError OrElse Validation.GetHasError(txtLastName)
                hasError = hasError OrElse Validation.GetHasError(txtMobilePhone)
            Case EnumAction.Delete
                hasError = Validation.GetHasError(txtEmpCode)
                hasError = hasError OrElse Validation.GetHasError(txtRetiredDate)
        End Select
        Return Not hasError
    End Function
#End Region

    Private Function DeleteEmployee() As Boolean
        Dim dbConn As New DbConnect()
        Dim res As Integer

        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim sSQL As String = DeleteEmployeeSQL()
            Dim cmd As New SqlCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            Dim row As PMS_ATOMYDataSet.EmployeeRow = AtomyDataSet.Employee.Rows(0)
            cmd.Parameters.AddWithValue("@Retired", True)
            cmd.Parameters.AddWithValue("@RetiredDate", New Date().ToString("yyyy/MM/dd"))
            cmd.Parameters.AddWithValue("@EmpCode", row.EmpCode)

            res = cmd.ExecuteNonQuery()
            dbConn.CommitTran()
        Catch ex As Exception
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi xóa nhân viên.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res
    End Function

    Private Function InsertEmployee() As Boolean
        Dim dbConn As New DbConnect()
        Dim res As Integer

        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim sSQL As String = InsertEmployeeSQL()
            Using cmd As New SqlCommand(sSQL, dbConn.Conn)
                cmd.Transaction = dbConn.Tran
                Dim row As PMS_ATOMYDataSet.EmployeeRow = AtomyDataSet.Employee.Rows(0)
                Dim now As Date = Date.Now
                row.CreateDate = now.ToString("yyyy/MM/dd")
                row.CreateTime = now.ToString("HH:mm:ss")
                row.CreateUser = Utility.LoginUserCode
                row.UpdateDate = now.ToString("yyyy/MM/dd")
                row.UpdateTime = now.ToString("HH:mm:ss")
                row.UpdateUser = Utility.LoginUserCode

                cmd.Parameters.AddWithValue("@EmpCode", row.EmpCode)
                cmd.Parameters.AddWithValue("@LastName", row.LastName)
                cmd.Parameters.AddWithValue("@FirstName", row.FirstName)
                cmd.Parameters.AddWithValue("@Department", row.Department)
                cmd.Parameters.AddWithValue("@Position", row.Position)
                cmd.Parameters.AddWithValue("@EmailAddress", row.EmailAddress)
                cmd.Parameters.AddWithValue("@BusinessPhone", row.BusinessPhone)
                cmd.Parameters.AddWithValue("@HomePhone", row.HomePhone)
                cmd.Parameters.AddWithValue("@MobilePhone", row.MobilePhone)
                cmd.Parameters.AddWithValue("@FaxNumber", row.FaxNumber)
                cmd.Parameters.AddWithValue("@Address", row.Address)
                cmd.Parameters.AddWithValue("@City", row.City)
                cmd.Parameters.AddWithValue("@StateProvince", row.StateProvince)
                cmd.Parameters.AddWithValue("@ZIPPostalCode", row.ZIPPostalCode)
                cmd.Parameters.AddWithValue("@CountryRegion", row.CountryRegion)
                cmd.Parameters.AddWithValue("@FacebookID", row.FacebookID)
                cmd.Parameters.AddWithValue("@Notes", row.Notes)
                cmd.Parameters.AddWithValue("@Retired", row.Retired)
                cmd.Parameters.AddWithValue("@RetiredDate", row.RetiredDate)
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
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi cập nhật nhân viên.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res
    End Function

    Private Function UpdateEmployee() As Boolean
        Dim dbConn As New DbConnect()
        Dim res As Integer

        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim sSQL As String = UpdateEmployeeSQL()
            Dim cmd As New SqlCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            Dim row As PMS_ATOMYDataSet.EmployeeRow = AtomyDataSet.Employee.Rows(0)
            Dim now As Date = Date.Now
            row.CreateDate = now.ToString("yyyy/MM/dd")
            row.CreateTime = now.ToString("HH:mm:ss")
            row.CreateUser = Utility.LoginUserCode
            row.UpdateDate = now.ToString("yyyy/MM/dd")
            row.UpdateTime = now.ToString("HH:mm:ss")
            row.UpdateUser = Utility.LoginUserCode

            cmd.Parameters.AddWithValue("@LastName", row.LastName)
            cmd.Parameters.AddWithValue("@FirstName", row.FirstName)
            cmd.Parameters.AddWithValue("@Department", row.Department)
            cmd.Parameters.AddWithValue("@Position", row.Position)
            cmd.Parameters.AddWithValue("@EmailAddress", row.EmailAddress)
            cmd.Parameters.AddWithValue("@BusinessPhone", row.BusinessPhone)
            cmd.Parameters.AddWithValue("@HomePhone", row.HomePhone)
            cmd.Parameters.AddWithValue("@MobilePhone", row.MobilePhone)
            cmd.Parameters.AddWithValue("@FaxNumber", row.FaxNumber)
            cmd.Parameters.AddWithValue("@Address", row.Address)
            cmd.Parameters.AddWithValue("@City", row.City)
            cmd.Parameters.AddWithValue("@StateProvince", row.StateProvince)
            cmd.Parameters.AddWithValue("@ZIPPostalCode", row.ZIPPostalCode)
            cmd.Parameters.AddWithValue("@CountryRegion", row.CountryRegion)
            cmd.Parameters.AddWithValue("@FacebookID", row.FacebookID)
            cmd.Parameters.AddWithValue("@Notes", row.Notes)
            cmd.Parameters.AddWithValue("@Retired", row.Retired)
            cmd.Parameters.AddWithValue("@RetiredDate", row.RetiredDate)
            cmd.Parameters.AddWithValue("@UpdateDate", row.UpdateDate)
            cmd.Parameters.AddWithValue("@UpdateTime", row.UpdateTime)
            cmd.Parameters.AddWithValue("@UpdateUser", row.UpdateUser)
            cmd.Parameters.AddWithValue("@EmpCode", row.EmpCode)

            res = cmd.ExecuteNonQuery()
            dbConn.CommitTran()
        Catch ex As Exception
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi cập nhật nhân viên.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res
    End Function

#Region "HelpCreateEmpCode"
    Private Sub HelpCreateEmpCode()
        lblEmpCodeHint.Content = "Gợi ý: " + Utility.HelpCreateCode("Employee")
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
                If txtCode.Equals(txtEmpCode) AndAlso txtEmpCode.Text.Trim.Length > 0 AndAlso Check.IsExisted("Employee", txtEmpCode.Text.Trim) Then
                    MessageBox.Show("Mã nhân viên đã tồn tại.", Utility.AppCaption)
                    txtEmpCode.Text = ""
                End If
            ElseIf Mode = DataRowState.Modified OrElse Mode = DataRowState.Deleted Then
                LoadData(txtEmpCode.Text.Trim)
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô mã.", ex)
        End Try
    End Sub
#End Region

#Region "☆ SQL"
#Region "InsertEmployeeSQL"
    Private Function InsertEmployeeSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("INSERT INTO [Employee]                                                                                                                                                                                                                                                                                                                                   ")
        sb.AppendLine("            ( [EmpCode],[LastName],[FirstName],[Department],[Position], [EmailAddress],[BusinessPhone], [HomePhone], [MobilePhone], [FaxNumber], [Address], [City], [StateProvince], [ZipPostalCode], [CountryRegion],[FacebookID],[Notes],[Retired],[RetiredDate], [CreateDate], [CreateTime], [CreateUser], [UpdateDate], [UpdateTime], [UpdateUser])  ")
        sb.AppendLine("     VALUES ( @EmpCode,@LastName,@FirstName,@Department,@Position, @EmailAddress,@BusinessPhone, @HomePhone, @MobilePhone, @FaxNumber, @Address, @City, @StateProvince, @ZipPostalCode, @CountryRegion,@FacebookID,@Notes,@Retired,@RetiredDate, @CreateDate, @CreateTime, @CreateUser, @UpdateDate, @UpdateTime, @UpdateUser)                           ")
        Return sb.ToString()
    End Function
#End Region

#Region "UpdateEmployeeSQL"
    Private Function UpdateEmployeeSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Employee]                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        ")
        sb.AppendLine("   set [LastName] = @LastName,[FirstName] = @FirstName,[Department] = @Department,[Position] = @Position, [EmailAddress] = @EmailAddress,[BusinessPhone] = @BusinessPhone, [HomePhone] = @HomePhone, [MobilePhone] = @MobilePhone, [FaxNumber] = @FaxNumber, [Address] = @Address, [City] = @City, [StateProvince] = @StateProvince, [ZipPostalCode] = @ZipPostalCode, [CountryRegion] = @CountryRegion,[FacebookID] = @FacebookID,[Notes] = @Notes,[Retired] = @Retired,[RetiredDate] = @RetiredDate,[UpdateDate] = @UpdateDate,[UpdateTime] = @UpdateTime,[UpdateUser] = @UpdateUser   ")
        sb.AppendLine(" WHERE [EmpCode] = @EmpCode                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              ")
        Return sb.ToString()
    End Function
#End Region

#Region "DeleteEmployeeSQL"
    Private Function DeleteEmployeeSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Employee]                                        ")
        sb.AppendLine("   SET [Retired] = @Retired                              ")
        sb.AppendLine("     , [RetiredDate] = @RetiredDate                      ")
        sb.AppendLine(" WHERE [EmpCode] = @EmpCode                              ")
        Return sb.ToString()
    End Function
#End Region
#End Region

End Class

