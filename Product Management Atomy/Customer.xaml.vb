Imports System.Data.OleDb
Imports System.Data
Imports System.Text
Public Class Customer
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
    Private Sub LoadData(CusCode As String)
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim sSQL As String = "select * from [Customer] where [CusCode] = ?"
            Dim adapt As New OleDbDataAdapter(sSQL, dbConn.Conn)
            adapt.SelectCommand.Parameters.Add("@CusCode", OleDbType.VarChar).Value = CusCode
            AtomyDataSet.Customer.Clear()
            adapt.Fill(AtomyDataSet, "Customer")
            Me.DataContext = AtomyDataSet.Customer.Rows(0)
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
            txtMobilePhone.IsReadOnly = False
            txtFacebookID.IsReadOnly = False
            txtEmailAddress.IsReadOnly = False
            txtAddress.IsReadOnly = False
            txtCompany.IsReadOnly = False
            txtBusinessPhone.IsReadOnly = False
            txtHomePhone.IsReadOnly = False
            txtFaxNumber.IsReadOnly = False
            txtCity.IsReadOnly = False
            cboProvince.IsReadOnly = False
            txtZip.IsReadOnly = False
            txtCountry.IsReadOnly = False
            txtWebPage.IsReadOnly = False
            txtNotes.IsReadOnly = False

        ElseIf Mode = DataRowState.Modified Then
            txtFirstName.IsReadOnly = False
            txtLastName.IsReadOnly = False
            txtMobilePhone.IsReadOnly = False
            txtFacebookID.IsReadOnly = False
            txtEmailAddress.IsReadOnly = False
            txtAddress.IsReadOnly = False
            txtCompany.IsReadOnly = False
            txtBusinessPhone.IsReadOnly = False
            txtHomePhone.IsReadOnly = False
            txtFaxNumber.IsReadOnly = False
            txtCity.IsReadOnly = False
            cboProvince.IsReadOnly = False
            txtZip.IsReadOnly = False
            txtCountry.IsReadOnly = False
            txtWebPage.IsReadOnly = False
            txtNotes.IsReadOnly = False

        ElseIf Me.Mode = DataRowState.Deleted Then
            txtFirstName.IsReadOnly = True
            txtLastName.IsReadOnly = True
            txtMobilePhone.IsReadOnly = True
            txtFacebookID.IsReadOnly = True
            txtEmailAddress.IsReadOnly = True
            txtAddress.IsReadOnly = True
            txtCompany.IsReadOnly = True
            txtBusinessPhone.IsReadOnly = True
            txtHomePhone.IsReadOnly = True
            txtFaxNumber.IsReadOnly = True
            txtCity.IsReadOnly = True
            cboProvince.IsReadOnly = True
            txtZip.IsReadOnly = True
            txtCountry.IsReadOnly = True
            txtWebPage.IsReadOnly = True
            txtNotes.IsReadOnly = True

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
                    If Check.IsExisted("Customer", txtCusCode.Text) Then
                        MessageBox.Show("Mã khách hàng đã tồn tại.")
                        HelpCreateCusCode()
                        Return
                    End If

                    If InsertCustomer() Then
                        MessageBox.Show("Đã hoàn thành.")
                        lblCusCodeHint.Content = ""
                        ProcessSelection.Mode = DataRowState.Modified
                        LoadData(txtCusCode.Text)
                    Else
                        MessageBox.Show("Không thành công.")
                    End If
                Case DataRowState.Modified
                    If Not ValidateData(EnumAction.Update) Then
                        Return
                    End If
                    If UpdateCustomer() Then
                        MessageBox.Show("Đã hoàn thành.")
                        lblCusCodeHint.Content = ""
                        LoadData(txtCusCode.Text)
                    Else
                        MessageBox.Show("Không thành công.")
                    End If
                Case DataRowState.Deleted
                    If Not ValidateData(EnumAction.Delete) Then
                        Return
                    End If
                    Dim confirm As Boolean = (MessageBox.Show("Bạn có muốn xóa mặt hàng này không?", "Atomy", MessageBoxButton.YesNo) = MessageBoxResult.OK)
                    If confirm Then
                        If DeleteCustomer() Then
                            MessageBox.Show("Đã hoàn thành.")
                            lblCusCodeHint.Content = ""
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

        End Select
        Return Not hasError
    End Function
#End Region

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

#Region "HelpCreateCusCode"
    Private Sub HelpCreateCusCode()
        lblCusCodeHint.Content = "Gợi ý: " + Utility.HelpCreateCode("Customer")
    End Sub

#End Region
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

#Region "txtCode_LostFocus"
    Private Sub txtCode_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            Dim txtCode = DirectCast(sender, TextBox)
            If Mode = DataRowState.Added Then
                Dim s = txtCode.Text.Trim()
                If s.Length = 0 Then
                    Return
                End If
                If s.Length < 8 Then
                    Dim lead As String = New String("0", 8 - s.Length)
                    s = lead + s
                    txtCode.Text = s
                End If
                If txtCode.Equals(txtCusCode) AndAlso txtCusCode.Text.Trim.Length > 0 AndAlso Check.IsExisted("Customer", txtCusCode.Text.Trim) Then
                    MessageBox.Show("Mã khách hàng đã tồn tại.", Utility.AppCaption)
                    txtCusCode.Text = ""
                End If
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô mã.", ex)
        End Try
    End Sub
#End Region

End Class

Public Class Province
    Public Property ZipCode As String
    Public Property Province As String
    Public Property Country As String
    Public Property IsCapital As String
    Public Sub New(a As String, b As String, c As String, d As Boolean)
        Me.ZipCode = a
        Me.Province = b
        Me.Country = c
        Me.IsCapital = d
    End Sub
    Private Shared ListProvince As List(Of Province)
    Public Shared Function GetAllProvinces() As List(Of Province)
        If ListProvince Is Nothing Then
            ListProvince = New List(Of Province)
            ListProvince.Add(New Province("100000", "Hà Nội", "Việt Nam", True))
            ListProvince.Add(New Province("700000", "Thành phố Hồ Chí Minh", "Việt Nam", False))
            ListProvince.Add(New Province("880000", "An Giang", "Việt Nam", False))
            ListProvince.Add(New Province("790000", "Bà Rịa-Vũng Tàu", "Việt Nam", False))
            ListProvince.Add(New Province("960000", "Bạc Liêu", "Việt Nam", False))
            ListProvince.Add(New Province("260000", "Bắc Kạn", "Việt Nam", False))
            ListProvince.Add(New Province("230000", "Bắc Giang", "Việt Nam", False))
            ListProvince.Add(New Province("220000", "Bắc Ninh", "Việt Nam", False))
            ListProvince.Add(New Province("930000", "Bến Tre", "Việt Nam", False))
            ListProvince.Add(New Province("820000", "Bình Dương", "Việt Nam", False))
            ListProvince.Add(New Province("590000", "Bình Định", "Việt Nam", False))
            ListProvince.Add(New Province("830000", "Bình Phước", "Việt Nam", False))
            ListProvince.Add(New Province("800000", "Bình Thuận", "Việt Nam", False))
            ListProvince.Add(New Province("970000", "Cà Mau", "Việt Nam", False))
            ListProvince.Add(New Province("270000", "Cao Bằng", "Việt Nam", False))
            ListProvince.Add(New Province("900000", "Cần Thơ", "Việt Nam", False))
            ListProvince.Add(New Province("550000", "Đà Nẵng", "Việt Nam", False))
            ListProvince.Add(New Province("630000", "Đắk Lắk", "Việt Nam", False))
            ListProvince.Add(New Province("640000", "Đắk Nông", "Việt Nam", False))
            ListProvince.Add(New Province("380000", "Điện Biên", "Việt Nam", False))
            ListProvince.Add(New Province("810000", "Đồng Nai", "Việt Nam", False))
            ListProvince.Add(New Province("870000", "Đồng Tháp", "Việt Nam", False))
            ListProvince.Add(New Province("600000", "Gia Lai", "Việt Nam", False))
            ListProvince.Add(New Province("310000", "Hà Giang", "Việt Nam", False))
            ListProvince.Add(New Province("400000", "Hà Nam", "Việt Nam", False))
            ListProvince.Add(New Province("480000", "Hà Tĩnh", "25", False))
            ListProvince.Add(New Province("170000", "Hải Dương", "26", False))
            ListProvince.Add(New Province("180000", "Hải Phòng", "27", False))
            ListProvince.Add(New Province("910000", "Hậu Giang", "28", False))
            ListProvince.Add(New Province("350000", "Hòa Bình", "29", False))
            ListProvince.Add(New Province("160000", "Hưng Yên", "31", False))
            ListProvince.Add(New Province("650000", "Khánh Hoà", "32", False))
            ListProvince.Add(New Province("920000", "Kiên Giang", "33", False))
            ListProvince.Add(New Province("580000", "Kon Tum", "34", False))
            ListProvince.Add(New Province("390000", "Lai Châu", "35", False))
            ListProvince.Add(New Province("240000", "Lạng Sơn", "36", False))
            ListProvince.Add(New Province("330000", "Lào Cai", "37", False))
            ListProvince.Add(New Province("670000", "Lâm Đồng", "38", False))
            ListProvince.Add(New Province("850000", "Long An", "39", False))
            ListProvince.Add(New Province("420000", "Nam Định", "40", False))
            ListProvince.Add(New Province("460000", "Nghệ An", "41", False))
            ListProvince.Add(New Province("430000", "Ninh Bình", "42", False))
            ListProvince.Add(New Province("660000", "Ninh Thuận", "43", False))
            ListProvince.Add(New Province("290000", "Phú Thọ", "44", False))
            ListProvince.Add(New Province("620000", "Phú Yên", "45", False))
            ListProvince.Add(New Province("510000", "Quảng Bình", "46", False))
            ListProvince.Add(New Province("560000", "Quảng Nam", "47", False))
            ListProvince.Add(New Province("570000", "Quảng Ngãi", "48", False))
            ListProvince.Add(New Province("200000", "Quảng Ninh", "49", False))
            ListProvince.Add(New Province("520000", "Quảng Trị", "50", False))
            ListProvince.Add(New Province("950000", "Sóc Trăng", "51", False))
            ListProvince.Add(New Province("360000", "Sơn La", "52", False))
            ListProvince.Add(New Province("840000", "Tây Ninh", "53", False))
            ListProvince.Add(New Province("410000", "Thái Bình", "54", False))
            ListProvince.Add(New Province("250000", "Thái Nguyên", "55", False))
            ListProvince.Add(New Province("440000", "Thanh Hoá", "56", False))
            ListProvince.Add(New Province("530000", "Thừa Thiên-Huế", "57", False))
            ListProvince.Add(New Province("860000", "Tiền Giang", "58", False))
            ListProvince.Add(New Province("940000", "Trà Vinh", "59", False))
            ListProvince.Add(New Province("300000", "Tuyên Quang", "60", False))
            ListProvince.Add(New Province("890000", "Vĩnh Long", "61", False))
            ListProvince.Add(New Province("280000", "Vĩnh Phúc", "62", False))
            ListProvince.Add(New Province("320000", "Yên Bái", "63", False))


        End If

        Return ListProvince
    End Function

End Class

