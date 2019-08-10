Imports System.Data
Imports System.Data.OleDb
Imports System.Text
Imports System.Windows.Controls.Primitives

Public Class Warehouse
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
        txtEmpCode.Text = ""
        lblEmpName.Content = ""
        txtCusCode.Text = ""
        txtCusName.Text = ""
        txtWareCode.Text = ""
        txtWareDate.Text = ""
        txtWareTitle.Text = ""
        txtTotalAmount.Text = "0"
        txtDiscount.Text = "0"
        txtDescription.Text = ""
        txtComment.Text = ""
    End Sub
#End Region

#Region "LoadData"
    Private Sub LoadData(WareCode As String)
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim sSQL As String = "select * from [WarehouseMaster] where [WareCode] = ?"
            Dim adapt As New OleDbDataAdapter(sSQL, dbConn.Conn)
            adapt.SelectCommand.Parameters.Add("@WareCode", OleDbType.VarChar).Value = WareCode
            AtomyDataSet.WarehouseMaster.Clear()
            AtomyDataSet.Warehouse.Clear()


            If adapt.Fill(AtomyDataSet, "WarehouseMaster") > 0 Then
                Me.DataContext = AtomyDataSet.WarehouseMaster.Rows(0)
                sSQL = "select * from [Warehouse] where [WareCode] = ?"
                adapt.SelectCommand.CommandText = sSQL
                adapt.Fill(AtomyDataSet, "Warehouse")
                grdWareHouse.ItemsSource = AtomyDataSet.Warehouse.DefaultView
            Else
                MessageBox.Show("Phiếu xuất [" + WareCode + "] không tồn tại hoặc đã bị xóa.")
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
            txtEmpCode.IsEnabled = True
            txtCusCode.IsEnabled = True
            txtCusName.IsEnabled = True
            txtWareDate.IsEnabled = True
            txtWareTitle.IsEnabled = True
            txtDiscount.IsEnabled = True
            txtDescription.IsEnabled = True
            txtComment.IsEnabled = True
            grdWareHouse.IsEnabled = True
        ElseIf Mode = DataRowState.Modified Then
            txtEmpCode.IsEnabled = True
            txtCusCode.IsEnabled = True
            txtCusName.IsEnabled = True
            txtWareDate.IsEnabled = True
            txtWareTitle.IsEnabled = True
            txtDiscount.IsEnabled = True
            txtDescription.IsEnabled = True
            txtComment.IsEnabled = True
            grdWareHouse.IsEnabled = True
        ElseIf Me.Mode = DataRowState.Deleted Then
            txtEmpCode.IsEnabled = False
            txtCusCode.IsEnabled = False
            txtCusName.IsEnabled = False
            txtWareDate.IsEnabled = False
            txtWareTitle.IsEnabled = False
            txtDiscount.IsEnabled = False
            txtDescription.IsEnabled = False
            txtComment.IsEnabled = False
            grdWareHouse.IsEnabled = False
        End If
    End Sub
#End Region


#Region "ProcessSelection_ValueChange"
    Private Sub ProcessSelection_ValueChange(sender As Object, e As EventArgs)
        If ProcessSelection.Mode = DataRowState.Added Then
            AtomyDataSet.WarehouseMaster.Clear()
            AtomyDataSet.Warehouse.Clear()
            Dim newRow As AtomyDataSet.WarehouseMasterRow = AtomyDataSet.WarehouseMaster.NewWarehouseMasterRow()
            AtomyDataSet.WarehouseMaster.Rows.Add(newRow)
            Me.DataContext = AtomyDataSet.WarehouseMaster.Rows(0)
            grdWareHouse.ItemsSource = AtomyDataSet.Warehouse.DefaultView
            Mode = DataRowState.Added
            CtrEnable()
            HelpCreateWareCode()
        ElseIf ProcessSelection.Mode = DataRowState.Modified Then
            Me.Mode = DataRowState.Modified
            CtrEnable()
        ElseIf ProcessSelection.Mode = DataRowState.Deleted Then
            Me.Mode = DataRowState.Deleted
            CtrEnable()
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
                        MessageBox.Show("Cập nhật thành công.", Me.Title, MessageBoxButton.OK)
                        lblWareCodeHint.Content = ""
                        ProcessSelection.Mode = DataRowState.Modified
                        LoadData(txtCusCode.Text.Trim)
                    Else
                        MessageBox.Show("Cập nhật không thành công.", Me.Title, MessageBoxButton.OK)
                    End If
                Case DataRowState.Modified
                    If Not ValidateData(EnumAction.Update) Then
                        Return
                    End If
                    If Update() Then
                        MessageBox.Show("Cập nhật thành công.", Me.Title, MessageBoxButton.OK)
                        lblWareCodeHint.Content = ""
                        LoadData(txtWareCode.Text.Trim)
                    Else
                        MessageBox.Show("Cập nhật không thành công.", Me.Title, MessageBoxButton.OK)
                    End If
                Case DataRowState.Deleted
                    If Not ValidateData(EnumAction.Delete) Then
                        Return
                    End If
                    Dim confirm As Boolean = (MessageBox.Show("Bạn có muốn xóa mặt hàng này không?", "Atomy", MessageBoxButton.YesNo) = MessageBoxResult.OK)
                    If confirm Then
                        If Delete() Then
                            MessageBox.Show("Xóa thành công.", Me.Title, MessageBoxButton.OK)
                            lblWareCodeHint.Content = ""
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
        Dim valid As Boolean = True
        Select Case action
            Case EnumAction.Insert
                If Validation.GetHasError(txtEmpCode) Then
                    MessageBox.Show("Vui lòng nhập mã nhân viên", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtEmpCode.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtCusCode) Then
                    MessageBox.Show("Vui lòng nhập mã khách hàng", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtCusCode.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtCusName) Then
                    MessageBox.Show("Vui lòng nhập tên khách hàng", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtCusName.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtWareCode) Then
                    MessageBox.Show("Vui lòng nhập số phiếu", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareCode.Focus()
                    Return False
                End If
                If Check.IsExisted("Warehouse", txtWareCode.Text.Trim) Then
                    MessageBox.Show("Mã phiếu xuất đã tồn tại.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareCode.Focus()
                    HelpCreateWareCode()
                    Return False
                End If
                If Validation.GetHasError(txtWareDate) Then
                    MessageBox.Show("Vui lòng nhập ngày xuất", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareDate.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtWareTitle) Then
                    MessageBox.Show("Vui lòng nhập tiêu đề", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareTitle.Focus()
                    Return False
                End If
                If Validation.GetHasError(grdWareHouse) Then
                    MessageBox.Show("Vui lòng nhập chi tiết mặt hàng", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    grdWareHouse.Focus()
                    Return False
                End If
            Case EnumAction.Update
                If Validation.GetHasError(txtEmpCode) Then
                    MessageBox.Show("Vui lòng nhập mã nhân viên", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtEmpCode.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtCusCode) Then
                    MessageBox.Show("Vui lòng nhập mã khách hàng", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtCusCode.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtCusName) Then
                    MessageBox.Show("Vui lòng nhập tên khách hàng", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtCusName.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtWareCode) Then
                    MessageBox.Show("Vui lòng nhập số phiếu", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareCode.Focus()
                    Return False
                End If
                If Not Check.IsExisted("Warehouse", txtWareCode.Text.Trim) Then
                    MessageBox.Show("Mã phiếu xuất chưa được đăng ký hoặc đã bị xóa.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareCode.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtWareDate) Then
                    MessageBox.Show("Vui lòng nhập ngày xuất", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareDate.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtWareTitle) Then
                    MessageBox.Show("Vui lòng nhập tiêu đề", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareTitle.Focus()
                    Return False
                End If
                If Validation.GetHasError(grdWareHouse) Then
                    MessageBox.Show("Vui lòng nhập chi tiết mặt hàng", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    grdWareHouse.Focus()
                    Return False
                End If
            Case EnumAction.Delete

        End Select
        Return Not valid
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
                Dim row As AtomyDataSet.WarehouseMasterRow = AtomyDataSet.WarehouseMaster.Rows(0)
                row.Type = 1 'bán hàng
                Dim now As Date = Date.Now
                row.CreateDate = now.ToString("yyyy/MM/dd")
                row.CreateTime = now.ToString("HH:mm:ss")
                row.CreateUser = Utility.LoginUserCode
                row.UpdateDate = now.ToString("yyyy/MM/dd")
                row.UpdateTime = now.ToString("HH:mm:ss")
                row.UpdateUser = Utility.LoginUserCode

                cmd.Parameters.Add("@1", OleDbType.VarChar).Value = row.WareCode
                cmd.Parameters.Add("@2", OleDbType.SmallInt).Value = row.Type
                cmd.Parameters.Add("@3", OleDbType.VarChar).Value = row.WareDate
                cmd.Parameters.Add("@4", OleDbType.VarChar).Value = row.EmpCode
                cmd.Parameters.Add("@5", OleDbType.VarChar).Value = row.EmpName
                cmd.Parameters.Add("@6", OleDbType.VarChar).Value = row.CusCode
                cmd.Parameters.Add("@7", OleDbType.VarChar).Value = row.CusName
                cmd.Parameters.Add("@8", OleDbType.SmallInt).Value = row.Status
                cmd.Parameters.Add("@9", OleDbType.VarChar).Value = row.WareTitle
                cmd.Parameters.Add("@10", OleDbType.VarChar).Value = row.Description
                cmd.Parameters.Add("@11", OleDbType.Currency).Value = row.TotalAmount
                cmd.Parameters.Add("@12", OleDbType.Currency).Value = row.Discount
                cmd.Parameters.Add("@13", OleDbType.VarChar).Value = row.Comments
                cmd.Parameters.Add("@14", OleDbType.SmallInt).Value = row.UpdateCount
                cmd.Parameters.Add("@15", OleDbType.Boolean).Value = row.Retired
                cmd.Parameters.Add("@16", OleDbType.VarChar).Value = row.RetiredDate
                cmd.Parameters.Add("@17", OleDbType.VarChar).Value = row.CreateDate
                cmd.Parameters.Add("@18", OleDbType.VarChar).Value = row.CreateTime
                cmd.Parameters.Add("@19", OleDbType.VarChar).Value = row.CreateUser
                cmd.Parameters.Add("@20", OleDbType.VarChar).Value = row.UpdateDate
                cmd.Parameters.Add("@21", OleDbType.VarChar).Value = row.UpdateTime
                cmd.Parameters.Add("@22", OleDbType.VarChar).Value = row.UpdateUser

                res = cmd.ExecuteNonQuery()

            End Using
            sSQL = InsertDetailSQL()
            For index = 0 To AtomyDataSet.Warehouse.Rows.Count - 1
                Using cmd As New OleDbCommand(sSQL, dbConn.Conn)
                    cmd.Transaction = dbConn.Tran
                    Dim rowM As AtomyDataSet.WarehouseMasterRow = AtomyDataSet.WarehouseMaster.Rows(0)
                    Dim row As AtomyDataSet.WarehouseRow = AtomyDataSet.Warehouse.Rows(index)
                    row.Type = rowM.Type
                    row.WareCode = rowM.WareCode
                    row.WareDate = rowM.WareDate
                    row.Status = rowM.Status
                    row.Description = rowM.Description
                    Dim now As Date = Date.Now
                    row.CreateDate = now.ToString("yyyy/MM/dd")
                    row.CreateTime = now.ToString("HH:mm:ss")
                    row.CreateUser = Utility.LoginUserCode
                    row.UpdateDate = now.ToString("yyyy/MM/dd")
                    row.UpdateTime = now.ToString("HH:mm:ss")
                    row.UpdateUser = Utility.LoginUserCode

                    cmd.Parameters.Add("@1", OleDbType.VarChar).Value = row.WareCode
                    cmd.Parameters.Add("@2", OleDbType.SmallInt).Value = row.Type
                    cmd.Parameters.Add("@3", OleDbType.VarChar).Value = row.WareDate
                    cmd.Parameters.Add("@4", OleDbType.VarChar).Value = row.PropCode
                    cmd.Parameters.Add("@5", OleDbType.VarChar).Value = row.PropName
                    cmd.Parameters.Add("@6", OleDbType.VarChar).Value = row.Category
                    cmd.Parameters.Add("@7", OleDbType.SmallInt).Value = row.Status
                    cmd.Parameters.Add("@8", OleDbType.VarChar).Value = row.Description
                    cmd.Parameters.Add("@9", OleDbType.VarChar).Value = row.Unit
                    cmd.Parameters.Add("@10", OleDbType.Currency).Value = row.UnitPrice
                    cmd.Parameters.Add("@11", OleDbType.Currency).Value = row.CurrentPrice
                    cmd.Parameters.Add("@12", OleDbType.Currency).Value = row.Amount
                    cmd.Parameters.Add("@13", OleDbType.SmallInt).Value = row.Quantity
                    cmd.Parameters.Add("@14", OleDbType.VarChar).Value = row.Comments
                    cmd.Parameters.Add("@15", OleDbType.SmallInt).Value = row.UpdateCount
                    cmd.Parameters.Add("@16", OleDbType.VarChar).Value = row.CreateDate
                    cmd.Parameters.Add("@17", OleDbType.VarChar).Value = row.CreateTime
                    cmd.Parameters.Add("@18", OleDbType.VarChar).Value = row.CreateUser
                    cmd.Parameters.Add("@19", OleDbType.VarChar).Value = row.UpdateDate
                    cmd.Parameters.Add("@20", OleDbType.VarChar).Value = row.UpdateTime
                    cmd.Parameters.Add("@21", OleDbType.VarChar).Value = row.UpdateUser

                    res = cmd.ExecuteNonQuery()

                End Using

            Next

            dbConn.CommitTran()
        Catch ex As Exception
            res = -1
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi cập nhật phiếu xuất.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res > 0
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
            Using cmd As New OleDbCommand(sSQL, dbConn.Conn)
                cmd.Transaction = dbConn.Tran
                Dim row As AtomyDataSet.WarehouseMasterRow = AtomyDataSet.WarehouseMaster.Rows(0)
                Dim now As Date = Date.Now
                row.UpdateDate = now.ToString("yyyy/MM/dd")
                row.UpdateTime = now.ToString("HH:mm:ss")
                row.UpdateUser = Utility.LoginUserCode

                cmd.Parameters.Add("@1", OleDbType.SmallInt).Value = row.Type
                cmd.Parameters.Add("@2", OleDbType.VarChar).Value = row.WareDate
                cmd.Parameters.Add("@3", OleDbType.VarChar).Value = row.EmpCode
                cmd.Parameters.Add("@4", OleDbType.VarChar).Value = row.EmpName
                cmd.Parameters.Add("@5", OleDbType.VarChar).Value = row.CusCode
                cmd.Parameters.Add("@6", OleDbType.VarChar).Value = row.CusName
                cmd.Parameters.Add("@7", OleDbType.SmallInt).Value = row.Status
                cmd.Parameters.Add("@8", OleDbType.VarChar).Value = row.WareTitle
                cmd.Parameters.Add("@9", OleDbType.VarChar).Value = row.Description
                cmd.Parameters.Add("@10", OleDbType.Currency).Value = row.TotalAmount
                cmd.Parameters.Add("@11", OleDbType.Currency).Value = row.Discount
                cmd.Parameters.Add("@12", OleDbType.VarChar).Value = row.Comments
                cmd.Parameters.Add("@13", OleDbType.SmallInt).Value = row.UpdateCount
                cmd.Parameters.Add("@14", OleDbType.Boolean).Value = row.Retired
                cmd.Parameters.Add("@15", OleDbType.VarChar).Value = row.RetiredDate
                cmd.Parameters.Add("@26", OleDbType.VarChar).Value = row.UpdateDate
                cmd.Parameters.Add("@17", OleDbType.VarChar).Value = row.UpdateTime
                cmd.Parameters.Add("@18", OleDbType.VarChar).Value = row.UpdateUser
                cmd.Parameters.Add("@19", OleDbType.VarChar).Value = row.WareCode
                res = cmd.ExecuteNonQuery()

            End Using
            Dim sSQLI = InsertDetailSQL()
            Dim sSQLU = UpdateDetailSQL()
            Dim sSQLD = DeleteDetailSQL()
            For index = 0 To AtomyDataSet.Warehouse.Rows.Count - 1
                Dim rowM As AtomyDataSet.WarehouseMasterRow = AtomyDataSet.WarehouseMaster.Rows(0)
                Dim row As AtomyDataSet.WarehouseRow = AtomyDataSet.Warehouse.Rows(index)
                If row.RowState = DataRowState.Added Then
                    Using cmd As New OleDbCommand(sSQLI, dbConn.Conn)
                        cmd.Transaction = dbConn.Tran
                        row.Type = rowM.Type
                        row.WareCode = rowM.WareCode
                        row.WareDate = rowM.WareDate
                        row.Status = rowM.Status
                        row.Description = rowM.Description
                        Dim now As Date = Date.Now
                        row.CreateDate = now.ToString("yyyy/MM/dd")
                        row.CreateTime = now.ToString("HH:mm:ss")
                        row.CreateUser = Utility.LoginUserCode
                        row.UpdateDate = now.ToString("yyyy/MM/dd")
                        row.UpdateTime = now.ToString("HH:mm:ss")
                        row.UpdateUser = Utility.LoginUserCode

                        cmd.Parameters.Add("@1", OleDbType.VarChar).Value = row.WareCode
                        cmd.Parameters.Add("@2", OleDbType.SmallInt).Value = row.Type
                        cmd.Parameters.Add("@3", OleDbType.VarChar).Value = row.WareDate
                        cmd.Parameters.Add("@4", OleDbType.VarChar).Value = row.PropCode
                        cmd.Parameters.Add("@5", OleDbType.VarChar).Value = row.PropName
                        cmd.Parameters.Add("@6", OleDbType.VarChar).Value = row.Category
                        cmd.Parameters.Add("@7", OleDbType.SmallInt).Value = row.Status
                        cmd.Parameters.Add("@8", OleDbType.VarChar).Value = row.Description
                        cmd.Parameters.Add("@9", OleDbType.VarChar).Value = row.Unit
                        cmd.Parameters.Add("@10", OleDbType.Currency).Value = row.UnitPrice
                        cmd.Parameters.Add("@11", OleDbType.Currency).Value = row.CurrentPrice
                        cmd.Parameters.Add("@12", OleDbType.Currency).Value = row.Amount
                        cmd.Parameters.Add("@13", OleDbType.SmallInt).Value = row.Quantity
                        cmd.Parameters.Add("@14", OleDbType.VarChar).Value = row.Comments
                        cmd.Parameters.Add("@15", OleDbType.SmallInt).Value = row.UpdateCount
                        cmd.Parameters.Add("@16", OleDbType.VarChar).Value = row.CreateDate
                        cmd.Parameters.Add("@17", OleDbType.VarChar).Value = row.CreateTime
                        cmd.Parameters.Add("@18", OleDbType.VarChar).Value = row.CreateUser
                        cmd.Parameters.Add("@19", OleDbType.VarChar).Value = row.UpdateDate
                        cmd.Parameters.Add("@20", OleDbType.VarChar).Value = row.UpdateTime
                        cmd.Parameters.Add("@21", OleDbType.VarChar).Value = row.UpdateUser

                        res = cmd.ExecuteNonQuery()

                    End Using
                ElseIf row.RowState = DataRowState.Modified Then
                    Using cmd As New OleDbCommand(sSQLU, dbConn.Conn)
                        cmd.Transaction = dbConn.Tran
                        row.Type = rowM.Type
                        row.WareCode = rowM.WareCode
                        row.WareDate = rowM.WareDate
                        row.Status = rowM.Status
                        row.Description = rowM.Description
                        Dim now As Date = Date.Now
                        row.UpdateDate = now.ToString("yyyy/MM/dd")
                        row.UpdateTime = now.ToString("HH:mm:ss")
                        row.UpdateUser = Utility.LoginUserCode

                        cmd.Parameters.Add("@1", OleDbType.VarChar).Value = row.WareCode
                        cmd.Parameters.Add("@2", OleDbType.SmallInt).Value = row.Type
                        cmd.Parameters.Add("@3", OleDbType.VarChar).Value = row.WareDate
                        cmd.Parameters.Add("@4", OleDbType.VarChar).Value = row.PropCode
                        cmd.Parameters.Add("@5", OleDbType.VarChar).Value = row.PropName
                        cmd.Parameters.Add("@6", OleDbType.VarChar).Value = row.Category
                        cmd.Parameters.Add("@7", OleDbType.SmallInt).Value = row.Status
                        cmd.Parameters.Add("@8", OleDbType.VarChar).Value = row.Description
                        cmd.Parameters.Add("@9", OleDbType.VarChar).Value = row.Unit
                        cmd.Parameters.Add("@10", OleDbType.Currency).Value = row.UnitPrice
                        cmd.Parameters.Add("@11", OleDbType.Currency).Value = row.CurrentPrice
                        cmd.Parameters.Add("@12", OleDbType.Currency).Value = row.Amount
                        cmd.Parameters.Add("@13", OleDbType.SmallInt).Value = row.Quantity
                        cmd.Parameters.Add("@14", OleDbType.VarChar).Value = row.Comments
                        cmd.Parameters.Add("@15", OleDbType.SmallInt).Value = row.UpdateCount + 1
                        cmd.Parameters.Add("@19", OleDbType.VarChar).Value = row.UpdateDate
                        cmd.Parameters.Add("@20", OleDbType.VarChar).Value = row.UpdateTime
                        cmd.Parameters.Add("@21", OleDbType.VarChar).Value = row.UpdateUser
                        cmd.Parameters.Add("@22", OleDbType.BigInt).Value = row("ID", DataRowVersion.Original)

                        res = cmd.ExecuteNonQuery()

                    End Using
                ElseIf row.RowState = DataRowState.Deleted Then
                    Using cmd As New OleDbCommand(sSQLD, dbConn.Conn)
                        cmd.Transaction = dbConn.Tran

                        cmd.Parameters.Add("@1", OleDbType.BigInt).Value = row("ID", DataRowVersion.Original)

                        res = cmd.ExecuteNonQuery()

                    End Using
                End If
            Next

            dbConn.CommitTran()
        Catch ex As Exception
            res = -1
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi cập nhật phiếu xuất.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res > 0
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
    Private Sub HelpCreateWareCode()
        lblWareCodeHint.Content = "Gợi ý: " + Utility.HelpCreateCode("Warehouse")
    End Sub

#End Region
#End Region

#Region "☆ SQL"
#Region "InsertSQL"
    Private Function InsertSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("INSERT INTO [WarehouseMaster]                               ")
        sb.AppendLine("            ( [WareCode],[Type],[WareDate],[EmpCode],[EmpName],[CusCode],[CusName],[Status],[WareTitle],[Description],[TotalAmount],[Discount],[Comments],[UpdateCount],[Retired],[RetiredDate],[CreateDate],[CreateTime],[CreateUser],[UpdateDate],[UpdateTime],[UpdateUser]) ")
        sb.AppendLine("     VALUES ( ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)                                          ")
        Return sb.ToString()
    End Function
#End Region
#Region "InsertDetailSQL"
    Private Function InsertDetailSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("INSERT INTO [Warehouse]                               ")
        sb.AppendLine("            ( [WareCode],[Type],[WareDate],[PropCode],[PropName],[Category],[Status],[Description],[Unit],[UnitPrice],[CurrentPrice],[Amount],[Quantity],[Comments],[UpdateCount],[CreateDate],[CreateTime],[CreateUser],[UpdateDate],[UpdateTime],[UpdateUser]) ")
        sb.AppendLine("     VALUES ( ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)                                          ")
        Return sb.ToString()
    End Function
#End Region
#Region "UpdateDetailSQL"
    Private Function UpdateDetailSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("Update [Warehouse]                               ")
        sb.AppendLine("   set [WareCode] = ?,[Type] = ?,[WareDate] = ?,[PropCode] = ?,[PropName] = ?,[Category] = ?,[Status] = ?,[Description] = ?,[Unit] = ?,[UnitPrice] = ?,[CurrentPrice] = ?,[Amount] = ?,[Quantity] = ?,[Comments] = ?,[UpdateCount] = ?,[UpdateDate] = ?,[UpdateTime] = ?,[UpdateUser] = ? ")
        sb.AppendLine(" where ID = ?                             ")
        Return sb.ToString()
    End Function
#End Region
#Region "DeleteDetailSQL"
    Private Function DeleteDetailSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("delete from [Warehouse] where ID = ?                             ")
        Return sb.ToString()
    End Function
#End Region

#Region "UpdatePropertySQL"
    Private Function UpdateSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("update [WarehouseMaster]                               ")
        sb.AppendLine("   set [Type] = ?,[WareDate] = ?,[EmpCode] = ?,[EmpName] = ?,[CusCode] = ?,[CusName] = ?,[Status] = ?,[WareTitle] = ?,[Description] = ?,[TotalAmount] = ?,[Discount] = ?,[Comments] = ?,[UpdateCount] = ?,[Retired] = ?,[RetiredDate] = ?,[UpdateDate] = ?,[UpdateTime] = ?,[UpdateUser] = ? ")
        sb.AppendLine("     where [WareCode] = ?")
        Return sb.ToString()
    End Function
#End Region

#Region "DeletePropertySQL"
    Private Function DeleteSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Property]                                ")
        sb.AppendLine("   SET [Retired] = ?                             ")
        sb.AppendLine("     , [Retired Date] = ?                        ")
        sb.AppendLine(" WHERE [PropCode] = ?                            ")
        Return sb.ToString()
    End Function
#End Region
#End Region

#Region "EVENT"

#Region "searchWareHouseSearchResult"
    Private Sub searchWareHouseSearchResult(sender As Object, e As SearchDataArgs)
        LoadData(e.Code)
    End Sub
#End Region

#Region "lnkCusCode_Click"
    Private Sub lnkCusCode_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim search As New Search()
            AddHandler search.SearchResult, AddressOf searchCusSearchResult
            search.Kind = EnumSearch.SearchCustomer
            search.ShowDialog()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào link Mã mặt hàng.", ex)
        End Try
    End Sub
#End Region

#Region "lnkEmpCode_Click"
    Private Sub lnkEmpCode_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim search As New Search()
            AddHandler search.SearchResult, AddressOf searchEmpSearchResult
            search.Kind = EnumSearch.SearchEmployee
            search.ShowDialog()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào link Mã mặt hàng.", ex)
        End Try
    End Sub
#End Region


#Region "searchCusSearchResult"
    Private Sub searchCusSearchResult(sender As Object, e As SearchDataArgs)
        txtCusCode.Text = e.Code
        txtCusName.Text = e.Name
    End Sub
#End Region

#Region "searchEmpSearchResult"
    Private Sub searchEmpSearchResult(sender As Object, e As SearchDataArgs)
        txtEmpCode.Text = e.Code
        lblEmpName.Content = e.Name
    End Sub
#End Region

#Region "txtWareCode_LostFocus"
    Private Sub txtWareCode_LostFocus(sender As Object, e As RoutedEventArgs)
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
                If txtWareCode.Text.Trim.Length > 0 AndAlso Check.IsExisted("Warehouse", txtWareCode.Text.Trim) Then
                    MessageBox.Show("Mã phiếu bán hàng đã tồn tại.", Me.Title)
                    txtWareCode.Text = ""
                ElseIf Mode = DataRowState.Modified OrElse Mode = DataRowState.Deleted Then
                    LoadData(txtWareCode.Text.Trim)
                End If
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô mã.", ex)
        End Try
    End Sub

#Region "txtCode_LostKeyboardFocus"
    Private Sub txtCode_LostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        Try
            Dim txtCode = DirectCast(sender, TextBox)

            Dim s = txtCode.Text.Trim()
            If s.Length = 0 Then
            ElseIf s.Length < 8 Then
                Dim lead As String = New String("0", 8 - s.Length)
                s = lead + s
                txtCode.Text = s
            End If
            If txtCode.Equals(txtCusCode) Then
                If txtCode.Text.Trim.Length > 0 Then
                    Dim dr As DataRow = Check.GetDataByCode("Customer", txtCusCode.Text.Trim)
                    If dr IsNot Nothing Then
                        txtCusName.Text = dr("FirstName").ToString() + " " + dr("LastName").ToString()
                    Else
                        MessageBox.Show("Mã khách hàng không tồn tại.", Utility.AppCaption)
                        txtCusCode.Text = ""
                        txtCusName.Text = ""
                    End If
                Else
                    txtCusName.Text = ""
                End If

            End If

            If txtCode.Equals(txtEmpCode) Then
                If txtCode.Text.Trim.Length > 0 Then
                    Dim dr As DataRow = Check.GetDataByCode("Employee", txtEmpCode.Text.Trim)
                    If dr IsNot Nothing Then
                        lblEmpName.Content = dr("FirstName").ToString() + " " + dr("LastName").ToString()
                    Else
                        MessageBox.Show("Mã nhân viên không tồn tại.", Utility.AppCaption)
                        txtEmpCode.Text = ""
                        lblEmpName.Content = ""
                    End If
                Else
                    lblEmpName.Content = ""
                End If
            End If

            If txtCode.Name.Equals("txtPropCode") Then
                Dim drv As DataRowView = grdWareHouse.SelectedItem
                If txtCode.Text.Trim.Length > 0 Then
                    Dim row As DataGridRow = Nothing
                    row = grdWareHouse.GetRow(grdWareHouse.SelectedIndex)
                    Dim dr As DataRow = Check.GetDataByCode("Property", txtCode.Text.Trim)
                    If dr IsNot Nothing Then
                        drv.Row("PropName") = dr("PropName")
                        Dim cellName As DataGridCell = grdWareHouse.GetCell(row, 1)
                        cellName.SetTemplateLabelContent("lblPropName", dr("PropName"))

                        drv.Row("Unit") = dr("Unit")
                        Dim cellUnit As DataGridCell = grdWareHouse.GetCell(row, 2)
                        cellUnit.SetTemplateLabelContent("lblUnit", dr("Unit"))

                        drv.Row("UnitPrice") = dr("SalesPrice")
                        drv.Row("CurrentPrice") = dr("SalesPrice")
                        Dim cellCurrentPrice As DataGridCell = grdWareHouse.GetCell(row, 4)
                        cellCurrentPrice.SetTemplateLabelContent("lblCurrentPrice", dr("SalesPrice"))
                    Else
                        MessageBox.Show("Mã mặt hàng không tồn tại.", Utility.AppCaption)
                        txtCode.Text = ""
                        drv.Row("PropName") = ""
                    End If
                Else
                    drv.Row("PropName") = ""
                End If
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô mã.", ex)
        End Try
    End Sub
#End Region

#End Region

#Region "lnkWareCode_Click"
    Private Sub lnkWareCode_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim search As New Search()
            AddHandler search.SearchResult, AddressOf searchWareHouseSearchResult
            search.Kind = EnumSearch.SearchWareHouse
            search.ShowDialog()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào link Mã phiếu xuất.", ex)
        End Try
    End Sub
#End Region

#Region "txtPropCode_KeyDown"
    Private Sub txtPropCode_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            If e.Key = Key.OemPlus Then
                Dim search As New Search(AddressOf SearchPropCode_Result, sender, EnumSearch.SearchProperty)
                e.Handled = True
            End If

        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi chọn mặt hàng.", ex)
        End Try
    End Sub

    Private Sub SearchPropCode_Result(SearchDataArgs As SearchDataArgs)
        If SearchDataArgs Is Nothing Then
            MessageBox.Show("Mã mặt hàng không tồn tại", Me.Title, MessageBoxButton.OK)
        Else

        End If

    End Sub
#End Region
#End Region

End Class
