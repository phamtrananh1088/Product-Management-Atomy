Imports System.Data.OleDb
Imports System.Data
Imports System.Text

Public Class Employee
    Private AtomyDataSet As AtomyDataSet
    Private Mode As DataRowState

    Public Sub New()
        AtomyDataSet = New AtomyDataSet()
        ' This call is required by the designer.
        InitializeComponent()
        Mode = ProcessSelection.Mode
        cboProvince.DataContext = Province.GetAllProvinces()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
#Region "LoadData"
    Private Sub LoadData(EmpCode As String)
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim sSQL As String = "select * from [Employee] where [EmpCode] = ?"
            Dim adapt As New OleDbDataAdapter(sSQL, dbConn.Conn)
            adapt.SelectCommand.Parameters.Add("@EmpCode", OleDbType.VarChar).Value = EmpCode
            AtomyDataSet.Employee.Clear()
            adapt.Fill(AtomyDataSet, "Employee")
            Me.DataContext = AtomyDataSet.Employee.Rows(0)
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi lấy dữ liệu.", ex)
        Finally
            dbConn.Close()
        End Try

    End Sub
#End Region

#Region "EnableButton"
    Private Sub CtrEnable()
        If Me.Mode = DataRowState.Added Then
            txtFirstName.IsReadOnly = False
            txtLastName.IsReadOnly = False
            txtDepartment.IsReadOnly = False
            txtPosition.IsReadOnly = False
            txtMobilePhone.IsReadOnly = False
            txtFacebookID.IsReadOnly = False
            txtEmailAddress.IsReadOnly = False
            txtAddress.IsReadOnly = False

            txtBusinessPhone.IsReadOnly = False
            txtHomePhone.IsReadOnly = False
            txtFaxNumber.IsReadOnly = False
            txtCity.IsReadOnly = False
            cboProvince.IsReadOnly = False
            txtZip.IsReadOnly = False
            txtCountry.IsReadOnly = False

            txtNotes.IsReadOnly = False
            lblRetiredDate.Visibility = Windows.Visibility.Hidden
            txtRetiredDate.Visibility = Windows.Visibility.Hidden
        ElseIf Mode = DataRowState.Modified Then
            txtFirstName.IsReadOnly = False
            txtLastName.IsReadOnly = False
            txtDepartment.IsReadOnly = False
            txtPosition.IsReadOnly = False
            txtMobilePhone.IsReadOnly = False
            txtFacebookID.IsReadOnly = False
            txtEmailAddress.IsReadOnly = False
            txtAddress.IsReadOnly = False

            txtBusinessPhone.IsReadOnly = False
            txtHomePhone.IsReadOnly = False
            txtFaxNumber.IsReadOnly = False
            txtCity.IsReadOnly = False
            cboProvince.IsReadOnly = False
            txtZip.IsReadOnly = False
            txtCountry.IsReadOnly = False

            txtNotes.IsReadOnly = False
            lblRetiredDate.Visibility = Windows.Visibility.Hidden
            txtRetiredDate.Visibility = Windows.Visibility.Hidden
        ElseIf Me.Mode = DataRowState.Deleted Then
            txtFirstName.IsReadOnly = True
            txtLastName.IsReadOnly = True
            txtDepartment.IsReadOnly = True
            txtPosition.IsReadOnly = True
            txtMobilePhone.IsReadOnly = True
            txtFacebookID.IsReadOnly = True
            txtEmailAddress.IsReadOnly = True
            txtAddress.IsReadOnly = True

            txtBusinessPhone.IsReadOnly = True
            txtHomePhone.IsReadOnly = True
            txtFaxNumber.IsReadOnly = True
            txtCity.IsReadOnly = True
            cboProvince.IsReadOnly = True
            txtZip.IsReadOnly = True
            txtCountry.IsReadOnly = True

            txtNotes.IsReadOnly = True
            lblRetiredDate.Visibility = Windows.Visibility.Visible
            txtRetiredDate.Visibility = Windows.Visibility.Visible
        End If

    End Sub
#End Region

#Region "btnUpdate_Click"
    Private Sub btnUpdate_Click(sender As Object, e As RoutedEventArgs)
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
            Dim newRow As AtomyDataSet.EmployeeRow = AtomyDataSet.Employee.NewEmployeeRow()
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
            Dim cmd As New OleDbCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            Dim row As AtomyDataSet.EmployeeRow = AtomyDataSet.Employee.Rows(0)
            cmd.Parameters.Add("@1", OleDbType.Boolean).Value = True
            cmd.Parameters.Add("@2", OleDbType.VarChar).Value = New Date().ToString("yyyy/MM/dd")
            cmd.Parameters.Add("@3", OleDbType.VarChar).Value = row.EmpCode

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
            Using cmd As New OleDbCommand(sSQL, dbConn.Conn)
                cmd.Transaction = dbConn.Tran
                Dim row As AtomyDataSet.EmployeeRow = AtomyDataSet.Employee.Rows(0)
                Dim now As Date = Date.Now
                row.CreateDate = now.ToString("yyyy/MM/dd")
                row.CreateTime = now.ToString("HH:mm:ss")
                row.CreateUser = Utility.LoginUserCode
                row.UpdateDate = now.ToString("yyyy/MM/dd")
                row.UpdateTime = now.ToString("HH:mm:ss")
                row.UpdateUser = Utility.LoginUserCode

                cmd.Parameters.Add("@EmpCode", OleDbType.VarChar).Value = row.EmpCode
                cmd.Parameters.Add("@LastName", OleDbType.VarChar).Value = row.LastName
                cmd.Parameters.Add("@FirstName", OleDbType.VarChar).Value = row.FirstName
                cmd.Parameters.Add("@Deparment", OleDbType.VarChar).Value = row.Deparment
                cmd.Parameters.Add("@Position", OleDbType.VarChar).Value = row.Position
                cmd.Parameters.Add("@EmailAddress", OleDbType.VarChar).Value = row.EmailAddress
                cmd.Parameters.Add("@BusinessPhone", OleDbType.VarChar).Value = row.BusinessPhone
                cmd.Parameters.Add("@HomePhone", OleDbType.VarChar).Value = row.HomePhone
                cmd.Parameters.Add("@MobilePhone", OleDbType.VarChar).Value = row.MobilePhone
                cmd.Parameters.Add("@FaxNumber", OleDbType.VarChar).Value = row.FaxNumber
                cmd.Parameters.Add("@Address", OleDbType.VarChar).Value = row.Address
                cmd.Parameters.Add("@City", OleDbType.VarChar).Value = row.City
                cmd.Parameters.Add("@StateProvince", OleDbType.VarChar).Value = row.StateProvince
                cmd.Parameters.Add("@ZIPPostalCode", OleDbType.VarChar).Value = row.ZIPPostalCode
                cmd.Parameters.Add("@CountryRegion", OleDbType.VarChar).Value = row.CountryRegion
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
            Dim cmd As New OleDbCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            Dim row As AtomyDataSet.EmployeeRow = AtomyDataSet.Employee.Rows(0)
            Dim now As Date = Date.Now
            row.CreateDate = now.ToString("yyyy/MM/dd")
            row.CreateTime = now.ToString("HH:mm:ss")
            row.CreateUser = Utility.LoginUserCode
            row.UpdateDate = now.ToString("yyyy/MM/dd")
            row.UpdateTime = now.ToString("HH:mm:ss")
            row.UpdateUser = Utility.LoginUserCode

            cmd.Parameters.Add("@LastName", OleDbType.VarChar).Value = row.LastName
            cmd.Parameters.Add("@FirstName", OleDbType.VarChar).Value = row.FirstName
            cmd.Parameters.Add("@Deparment", OleDbType.VarChar).Value = row.Deparment
            cmd.Parameters.Add("@Position", OleDbType.VarChar).Value = row.Position
            cmd.Parameters.Add("@EmailAddress", OleDbType.VarChar).Value = row.EmailAddress
            cmd.Parameters.Add("@BusinessPhone", OleDbType.VarChar).Value = row.BusinessPhone
            cmd.Parameters.Add("@HomePhone", OleDbType.VarChar).Value = row.HomePhone
            cmd.Parameters.Add("@MobilePhone", OleDbType.VarChar).Value = row.MobilePhone
            cmd.Parameters.Add("@FaxNumber", OleDbType.VarChar).Value = row.FaxNumber
            cmd.Parameters.Add("@Address", OleDbType.VarChar).Value = row.Address
            cmd.Parameters.Add("@City", OleDbType.VarChar).Value = row.City
            cmd.Parameters.Add("@StateProvince", OleDbType.VarChar).Value = row.StateProvince
            cmd.Parameters.Add("@ZIPPostalCode", OleDbType.VarChar).Value = row.ZIPPostalCode
            cmd.Parameters.Add("@CountryRegion", OleDbType.VarChar).Value = row.CountryRegion
            cmd.Parameters.Add("@FacebookID", OleDbType.VarChar).Value = row.FacebookID
            cmd.Parameters.Add("@Notes", OleDbType.VarChar).Value = row.Notes
            cmd.Parameters.Add("@Retired", OleDbType.Boolean).Value = row.Retired
            cmd.Parameters.Add("@RetiredDate", OleDbType.VarChar).Value = row.RetiredDate
            cmd.Parameters.Add("@UpdateDate", OleDbType.VarChar).Value = row.UpdateDate
            cmd.Parameters.Add("@UpdateTime", OleDbType.VarChar).Value = row.UpdateTime
            cmd.Parameters.Add("@UpdateUser", OleDbType.VarChar).Value = row.UpdateUser
            cmd.Parameters.Add("@EmpCode", OleDbType.VarChar).Value = row.EmpCode

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

#Region "☆ SQL"
#Region "InsertEmployeeSQL"
    Private Function InsertEmployeeSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("INSERT INTO [Employee]                               ")
        sb.AppendLine("            ( [EmpCode],[LastName],[FirstName],[Department],[Position], [EmailAddress],[BusinessPhone], [HomePhone], [MobilePhone], [FaxNumber], [Address], [City], [StateProvince], [ZipPostalCode], [CountryRegion],[FacebookID],[Notes],[Retired],[RetiredDate], [CreateDate], [CreateTime], [CreateUser], [UpdateDate], [UpdateTime], [UpdateUser]) ")
        sb.AppendLine("     VALUES ( ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)                                          ")
        Return sb.ToString()
    End Function
#End Region

#Region "UpdateEmployeeSQL"
    Private Function UpdateEmployeeSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Employee]                                ")
        sb.AppendLine("   set [LastName] = ?,[FirstName] = ?,[Department] = ?,[Position] = ?, [EmailAddress] = ?,[BusinessPhone] = ?, [HomePhone] = ?, [MobilePhone] = ?, [FaxNumber] = ?, [Address] = ?, [City] = ?, [StateProvince] = ?, [ZipPostalCode] = ?, [CountryRegion] = ?,[FacebookID] = ?,[Notes] = ?,[Retired] = ?,[RetiredDate] = ?,[UpdateDate] = ?,[UpdateTime] = ?,[UpdateUser] = ? ")
        sb.AppendLine(" WHERE [EmpCode] = ?                            ")
        Return sb.ToString()
    End Function
#End Region

#Region "DeleteEmployeeSQL"
    Private Function DeleteEmployeeSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Employee]                                ")
        sb.AppendLine("   SET [Retired] = ?                             ")
        sb.AppendLine("     , [RetiredDate] = ?                        ")
        sb.AppendLine(" WHERE [EmpCode] = ?                            ")
        Return sb.ToString()
    End Function
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
            Else
                If txtCode.Equals(txtEmpCode) AndAlso txtEmpCode.Text.Trim.Length > 0 AndAlso (Not Check.IsExisted("Employee", txtEmpCode.Text.Trim)) Then
                    MessageBox.Show("Mã nhân viên không tồn tại.", Utility.AppCaption)
                    txtEmpCode.Text = ""
                Else
                    LoadData(txtEmpCode.Text.Trim)
                End If
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô mã.", ex)
        End Try
    End Sub
#End Region

End Class

