Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Windows.Controls.Primitives
Imports BindValidation
Imports System.IO

Public Class Warehouse
#Region "FIELD"
    Private AtomyDataSet As AtomyDataSet
    Private Mode As DataRowState
    Private WareType As Int16 = 0
#End Region

#Region "CONSTRUCTOR"
    Public Sub New(wareType As Int16)
        AtomyDataSet = New AtomyDataSet()
        Me.WareType = wareType
        ' This call is required by the designer.
        InitializeComponent()
        InitialValue()
        ProcessSelection.Mode = DataRowState.Added
        ' Add any initialization after the InitializeComponent() call.
    End Sub
#End Region

#Region "InitialControl"
    Private Sub InitialValue()
        Select Case Me.WareType
            Case 0
                pgTitle.Title = "Phiếu nhập"
                Me.Title = "Phiếu nhập"
                lblWareDate.Content = "Ngày nhập:"
                grdWareHouse.Columns(4).Header = "Giá nhập"
                lblPayment.Visibility = Windows.Visibility.Hidden
                borderPaymentCash.Visibility = Windows.Visibility.Hidden
                rbPaymentCash.Visibility = Windows.Visibility.Hidden
                borderPaymentShipCode.Visibility = Windows.Visibility.Hidden
                rbPaymentShipCode.Visibility = Windows.Visibility.Hidden
                lblPaymentDate.Visibility = Windows.Visibility.Hidden
                txtPaymentDate.Visibility = Windows.Visibility.Hidden

                lblMarkCusCode.Visibility = Windows.Visibility.Hidden
                lblCusCode.Visibility = Windows.Visibility.Hidden
                txtCusCode.Visibility = Windows.Visibility.Hidden
                lblCusName.Visibility = Windows.Visibility.Hidden
                lblMarkEmpCode.Visibility = Windows.Visibility.Hidden
                lblEmpCode.Visibility = Windows.Visibility.Hidden
                txtEmpCode.Visibility = Windows.Visibility.Hidden
                lblEmpName.Visibility = Windows.Visibility.Hidden
                lblMarkCusName.Visibility = Windows.Visibility.Hidden
                lblCusName.Visibility = Windows.Visibility.Hidden
                txtCusName.Visibility = Windows.Visibility.Hidden

            Case 1
                pgTitle.Title = "Phiếu xuất"
                Me.Title = "Phiếu xuất"
                lblWareDate.Content = "Ngày xuất:"
                grdWareHouse.Columns(4).Header = "Giá bán"
                lblPayment.Visibility = Windows.Visibility.Visible
                borderPaymentCash.Visibility = Windows.Visibility.Visible
                rbPaymentCash.Visibility = Windows.Visibility.Visible
                borderPaymentShipCode.Visibility = Windows.Visibility.Visible
                rbPaymentShipCode.Visibility = Windows.Visibility.Visible
                lblPaymentDate.Visibility = Windows.Visibility.Visible
                txtPaymentDate.Visibility = Windows.Visibility.Visible
                lblMarkCusCode.Visibility = Windows.Visibility.Visible
                lblCusCode.Visibility = Windows.Visibility.Visible
                txtCusCode.Visibility = Windows.Visibility.Visible
                lblCusName.Visibility = Windows.Visibility.Visible
                lblMarkEmpCode.Visibility = Windows.Visibility.Visible
                lblEmpCode.Visibility = Windows.Visibility.Visible
                txtEmpCode.Visibility = Windows.Visibility.Visible
                lblEmpName.Visibility = Windows.Visibility.Visible
                lblMarkCusName.Visibility = Windows.Visibility.Visible
                lblCusName.Visibility = Windows.Visibility.Visible
                txtCusName.Visibility = Windows.Visibility.Visible
        End Select
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
            Dim sSQL As String = "select * from [WarehouseMaster] where [WareCode] = @WareCode and [Type] = @Type"
            Dim adapt As New SqlDataAdapter(sSQL, dbConn.Conn)
            adapt.SelectCommand.Parameters.AddWithValue("@WareCode", WareCode)
            adapt.SelectCommand.Parameters.AddWithValue("@Type", Me.WareType)
            AtomyDataSet.WarehouseMaster.Clear()
            AtomyDataSet.Warehouse.Clear()


            If adapt.Fill(AtomyDataSet, "WarehouseMaster") > 0 Then
                Me.DataContext = AtomyDataSet.WarehouseMaster.Rows(0)
                sSQL = "select * from [Warehouse] where [WareCode] = @WareCode"
                adapt.SelectCommand.CommandText = sSQL
                adapt.Fill(AtomyDataSet, "Warehouse")
                Dim tu = CalculateTotal()
                txtSumaryQuantity.Text = tu.Item1.ToString("#,##0")
                txtTotalAmount.Text = tu.Item2.ToString("#,##0")
                grdWareHouse.ItemsSource = AtomyDataSet.Warehouse.DefaultView
                If Me.WareType = 1 Then
                    btnPrint.Visibility = Windows.Visibility.Visible
                Else
                    btnPrint.Visibility = Windows.Visibility.Hidden
                End If
            Else
                MessageBox.Show("Phiếu " + If(Me.WareType = 0, "nhập", "xuất") + " [" + WareCode + "] không tồn tại hoặc đã bị xóa.")
                InitialValue()
                CtrEnable()
                btnPrint.Visibility = Windows.Visibility.Hidden
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
            btnPrint.Visibility = Windows.Visibility.Hidden
            grdWareHouse.ItemsSource = Nothing
            AtomyDataSet.WarehouseMaster.Clear()
            AtomyDataSet.Warehouse.Clear()
            Dim newRow As AtomyDataSet.WarehouseMasterRow = AtomyDataSet.WarehouseMaster.NewWarehouseMasterRow()
            Utility.RowInit.InitWarehouseMasterRow(newRow)
            AtomyDataSet.WarehouseMaster.Rows.Add(newRow)
            Me.DataContext = AtomyDataSet.WarehouseMaster.Rows(0)
            grdWareHouse.ItemsSource = AtomyDataSet.Warehouse.DefaultView
            Mode = DataRowState.Added
            CtrEnable()
            HelpCreateWareCode()
        ElseIf ProcessSelection.Mode = DataRowState.Modified Then
            btnPrint.Visibility = Windows.Visibility.Hidden
            grdWareHouse.ItemsSource = Nothing
            Me.Mode = DataRowState.Modified
            CtrEnable()
            HelpGetLastWareCode()
        ElseIf ProcessSelection.Mode = DataRowState.Deleted Then
            btnPrint.Visibility = Windows.Visibility.Hidden
            grdWareHouse.ItemsSource = Nothing
            Me.Mode = DataRowState.Deleted
            CtrEnable()
            HelpGetLastWareCode()
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
                        MessageBox.Show("Thêm mới thành công.", Me.Title, MessageBoxButton.OK)
                        lblWareCodeHint.Content = ""
                        ProcessSelection.Mode = DataRowState.Modified
                        LoadData(txtWareCode.Text.Trim)
                    Else
                        MessageBox.Show("Thêm mới thất bại.", Me.Title, MessageBoxButton.OK)
                    End If
                Case DataRowState.Modified
                    If Not ValidateData(EnumAction.Update) Then
                        Return
                    End If
                    If Update() Then
                        MessageBox.Show("Sửa đổi thành công.", Me.Title, MessageBoxButton.OK)
                        lblWareCodeHint.Content = ""
                        LoadData(txtWareCode.Text.Trim)
                    Else
                        MessageBox.Show("Sửa đổi thất bại.", Me.Title, MessageBoxButton.OK)
                    End If
                Case DataRowState.Deleted
                    If Not ValidateData(EnumAction.Delete) Then
                        Return
                    End If
                    Dim confirm As Boolean = (MessageBox.Show("Bạn có muốn xóa phiếu " + If(Me.WareType = 0, "nhập", "xuất") + " này không?", Me.Title, MessageBoxButton.YesNo) = MessageBoxResult.Yes)
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
                If Validation.GetHasError(txtWareCode) Then
                    MessageBox.Show("Vui lòng nhập số phiếu " + If(Me.WareType = 0, "nhập", "xuất") + ".", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareCode.Focus()
                    Return False
                End If
                If Check.IsExisted("Warehouse", txtWareCode.Text.Trim) Then
                    MessageBox.Show("Mã phiếu " + If(Me.WareType = 0, "nhập", "xuất") + " đã tồn tại.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareCode.Focus()
                    HelpCreateWareCode()
                    Return False
                End If
                If Validation.GetHasError(txtWareDate) Then
                    MessageBox.Show("Vui lòng nhập ngày " + If(Me.WareType = 0, "nhập", "xuất") + ".", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareDate.Focus()
                    Return False
                End If
                If Me.WareType = 1 AndAlso Validation.GetHasError(txtEmpCode) Then
                    MessageBox.Show("Vui lòng nhập mã nhân viên.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtEmpCode.Focus()
                    Return False
                End If
                If Me.WareType = 1 AndAlso Validation.GetHasError(txtCusCode) Then
                    MessageBox.Show("Vui lòng nhập mã khách hàng.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtCusCode.Focus()
                    Return False
                End If
                If Me.WareType = 1 AndAlso Validation.GetHasError(txtCusName) Then
                    MessageBox.Show("Vui lòng nhập tên khách hàng.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtCusName.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtWareCode) Then
                    MessageBox.Show("Vui lòng nhập số phiếu " + If(Me.WareType = 0, "nhập", "xuất") + ".", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareCode.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtWareTitle) Then
                    MessageBox.Show("Vui lòng nhập tiêu đề.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareTitle.Focus()
                    Return False
                End If
                If Validation.GetHasError(grdWareHouse) Then
                    MessageBox.Show("Vui lòng nhập chi tiết mặt hàng.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    grdWareHouse.Focus()
                    Return False
                End If
            Case EnumAction.Update
                If Validation.GetHasError(txtWareCode) Then
                    MessageBox.Show("Vui lòng nhập số phiếu " + If(Me.WareType = 0, "nhập", "xuất") + ".", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareCode.Focus()
                    Return False
                End If
                If Not Check.IsExisted("Warehouse", txtWareCode.Text.Trim) Then
                    MessageBox.Show("Mã phiếu " + If(Me.WareType = 0, "nhập", "xuất") + " chưa được đăng ký hoặc đã bị xóa.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareCode.Focus()
                    HelpGetLastWareCode()
                    Return False
                End If
                If Validation.GetHasError(txtWareDate) Then
                    MessageBox.Show("Vui lòng nhập ngày " + If(Me.WareType = 0, "nhập", "xuất") + ".", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareDate.Focus()
                    Return False
                End If
                If Me.WareType = 1 AndAlso Validation.GetHasError(txtEmpCode) Then
                    MessageBox.Show("Vui lòng nhập mã nhân viên.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtEmpCode.Focus()
                    Return False
                End If
                If Me.WareType = 1 AndAlso Validation.GetHasError(txtCusCode) Then
                    MessageBox.Show("Vui lòng nhập mã khách hàng.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtCusCode.Focus()
                    Return False
                End If
                If Me.WareType = 1 AndAlso txtCusName.Text.Trim.Length = 0 Then
                    MessageBox.Show("Vui lòng nhập tên khách hàng.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtCusName.Focus()
                    Return False
                End If
                If Validation.GetHasError(txtWareCode) Then
                    MessageBox.Show("Vui lòng nhập số phiếu.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareCode.Focus()
                    Return False
                End If

                If Validation.GetHasError(txtWareTitle) Then
                    MessageBox.Show("Vui lòng nhập tiêu đề.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareTitle.Focus()
                    Return False
                End If
                If Validation.GetHasError(grdWareHouse) Then
                    MessageBox.Show("Vui lòng nhập chi tiết mặt hàng.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    grdWareHouse.Focus()
                    Return False
                End If
                If Me.WareType = 1 AndAlso txtPaymentDate.Text < txtWareDate.Text Then
                    MessageBox.Show("Ngày thanh toán không được nhỏ hơn ngày xuất.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtPaymentDate.Focus()
                    Return False
                End If
                If Me.WareType = 1 AndAlso txtPaymentDate.Text = txtWareDate.Text AndAlso rbPaymentShipCode.IsChecked Then
                    MessageBox.Show("Phương thức thanh toán ship code thì ngày thanh toán phải sau ngày xuất ít nhất 1 ngày.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtPaymentDate.Focus()
                    Return False
                End If
            Case EnumAction.Delete
                If Validation.GetHasError(txtWareCode) Then
                    MessageBox.Show("Vui lòng nhập số phiếu " + If(Me.WareType = 0, "nhập", "xuất") + ".", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareCode.Focus()
                    Return False
                End If
                If Not Check.IsExisted("Warehouse", txtWareCode.Text.Trim) Then
                    MessageBox.Show("Mã phiếu " + If(Me.WareType = 0, "nhập", "xuất") + " chưa được đăng ký hoặc đã bị xóa.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                    txtWareCode.Focus()
                    HelpGetLastWareCode()
                    Return False
                End If
        End Select
        Return valid
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
                Dim row As AtomyDataSet.WarehouseMasterRow = AtomyDataSet.WarehouseMaster.Rows(0)
                row.Type = Me.WareType '0:mua hàng, 1: bán hàng
                Dim now As Date = Date.Now
                row.CreateDate = now.ToString("yyyy/MM/dd")
                row.CreateTime = now.ToString("HH:mm:ss")
                row.CreateUser = Utility.LoginUserCode
                row.UpdateDate = now.ToString("yyyy/MM/dd")
                row.UpdateTime = now.ToString("HH:mm:ss")
                row.UpdateUser = Utility.LoginUserCode

                cmd.Parameters.AddWithValue("@WareCode", row.WareCode)
                cmd.Parameters.AddWithValue("@Type", row.Type)
                cmd.Parameters.AddWithValue("@WareDate", row.WareDate)
                cmd.Parameters.AddWithValue("@EmpCode", row.EmpCode)
                cmd.Parameters.AddWithValue("@EmpName", row.EmpName)
                cmd.Parameters.AddWithValue("@CusCode", row.CusCode)
                cmd.Parameters.AddWithValue("@CusName", row.CusName)
                cmd.Parameters.AddWithValue("@Status", row.Status)
                cmd.Parameters.AddWithValue("@WareTitle", row.WareTitle)
                cmd.Parameters.AddWithValue("@Description", row.Description)
                cmd.Parameters.AddWithValue("@TotalAmount", row.TotalAmount)
                cmd.Parameters.AddWithValue("@Discount", row.Discount)
                cmd.Parameters.AddWithValue("@SalesAmount", row.SalesAmount)
                cmd.Parameters.AddWithValue("@PaymentType", If(rbPaymentCash.IsChecked, CShort(EnumPaymentType.Cash), If(rbPaymentShipCode.IsChecked, CShort(EnumPaymentType.ShipCode), 0)))
                cmd.Parameters.AddWithValue("@FinishFlag", row.FinishFlag)
                cmd.Parameters.AddWithValue("@PaymentDate", row.PaymentDate)
                cmd.Parameters.AddWithValue("@FinishDate", row.FinishDate)
                cmd.Parameters.AddWithValue("@Comments", row.Comments)
                cmd.Parameters.AddWithValue("@UpdateCount", row.UpdateCount)
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
            sSQL = InsertDetailSQL()
            grdWareHouse.EndInit()
            For index = 0 To AtomyDataSet.Warehouse.Rows.Count - 1
                Using cmd As New SqlCommand(sSQL, dbConn.Conn)
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

                    cmd.Parameters.AddWithValue("@WareCode", row.WareCode)
                    cmd.Parameters.AddWithValue("@Type", row.Type)
                    cmd.Parameters.AddWithValue("@WareDate", row.WareDate)
                    cmd.Parameters.AddWithValue("@PropCode", row.PropCode)
                    cmd.Parameters.AddWithValue("@PropName", row.PropName)
                    cmd.Parameters.AddWithValue("@Category", row.Category)
                    cmd.Parameters.AddWithValue("@Status", row.Status)
                    cmd.Parameters.AddWithValue("@Description", row.Description)
                    cmd.Parameters.AddWithValue("@Unit", row.Unit)
                    cmd.Parameters.AddWithValue("@UnitPrice", row.UnitPrice)
                    cmd.Parameters.AddWithValue("@CurrentPrice", row.CurrentPrice)
                    cmd.Parameters.AddWithValue("@Amount", row.Amount)
                    cmd.Parameters.AddWithValue("@Quantity", row.Quantity)
                    cmd.Parameters.AddWithValue("@Comments", row.Comments)
                    cmd.Parameters.AddWithValue("@UpdateCount", row.UpdateCount)
                    cmd.Parameters.AddWithValue("@CreateDate", row.CreateDate)
                    cmd.Parameters.AddWithValue("@CreateTime", row.CreateTime)
                    cmd.Parameters.AddWithValue("@CreateUser", row.CreateUser)
                    cmd.Parameters.AddWithValue("@UpdateDate", row.UpdateDate)
                    cmd.Parameters.AddWithValue("@UpdateTime", row.UpdateTime)
                    cmd.Parameters.AddWithValue("@UpdateUser", row.UpdateUser)

                    res = cmd.ExecuteNonQuery()

                End Using

            Next

            dbConn.CommitTran()
        Catch ex As Exception
            res = -1
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi cập nhật phiếu " + If(Me.WareType = 0, "nhập", "xuất") + ".", ex)
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
            Using cmd As New SqlCommand(sSQL, dbConn.Conn)
                Dim tu = CalculateTotal()
                cmd.Transaction = dbConn.Tran
                Dim row As AtomyDataSet.WarehouseMasterRow = AtomyDataSet.WarehouseMaster.Rows(0)
                row.TotalAmount = tu.Item2
                Dim now As Date = Date.Now
                row.UpdateDate = now.ToString("yyyy/MM/dd")
                row.UpdateTime = now.ToString("HH:mm:ss")
                row.UpdateUser = Utility.LoginUserCode

                cmd.Parameters.AddWithValue("@Type", row.Type)
                cmd.Parameters.AddWithValue("@WareDate", row.WareDate)
                cmd.Parameters.AddWithValue("@EmpCode", row.EmpCode)
                cmd.Parameters.AddWithValue("@EmpName", row.EmpName)
                cmd.Parameters.AddWithValue("@CusCode", row.CusCode)
                cmd.Parameters.AddWithValue("@CusName", row.CusName)
                cmd.Parameters.AddWithValue("@Status", row.Status)
                cmd.Parameters.AddWithValue("@WareTitle", row.WareTitle)
                cmd.Parameters.AddWithValue("@Description", row.Description)
                cmd.Parameters.AddWithValue("@TotalAmount", row.TotalAmount)
                cmd.Parameters.AddWithValue("@Discount", row.Discount)
                cmd.Parameters.AddWithValue("@SalesAmount", row.SalesAmount)
                cmd.Parameters.AddWithValue("@PaymentType", If(rbPaymentCash.IsChecked, CShort(EnumPaymentType.Cash), If(rbPaymentShipCode.IsChecked, CShort(EnumPaymentType.ShipCode), 0)))
                cmd.Parameters.AddWithValue("@FinishFlag", row.FinishFlag)
                cmd.Parameters.AddWithValue("@PaymentDate", row.PaymentDate)
                cmd.Parameters.AddWithValue("@FinishDate", row.FinishDate)
                cmd.Parameters.AddWithValue("@Comments", row.Comments)
                cmd.Parameters.AddWithValue("@UpdateCount", row.UpdateCount)
                cmd.Parameters.AddWithValue("@Retired", row.Retired)
                cmd.Parameters.AddWithValue("@RetiredDate", row.RetiredDate)
                cmd.Parameters.AddWithValue("@UpdateDate", row.UpdateDate)
                cmd.Parameters.AddWithValue("@UpdateTime", row.UpdateTime)
                cmd.Parameters.AddWithValue("@UpdateUser", row.UpdateUser)
                cmd.Parameters.AddWithValue("@WareCode", row.WareCode)
                res = cmd.ExecuteNonQuery()

            End Using
            Dim sSQLI = InsertDetailSQL()
            Dim sSQLU = UpdateDetailSQL()
            Dim sSQLD = DeleteDetailSQL()
            For index = 0 To AtomyDataSet.Warehouse.Rows.Count - 1
                Dim rowM As AtomyDataSet.WarehouseMasterRow = AtomyDataSet.WarehouseMaster.Rows(0)
                Dim row As AtomyDataSet.WarehouseRow = AtomyDataSet.Warehouse.Rows(index)
                If row.RowState = DataRowState.Added Then
                    Using cmd As New SqlCommand(sSQLI, dbConn.Conn)
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

                        cmd.Parameters.AddWithValue("@WareCode", row.WareCode)
                        cmd.Parameters.AddWithValue("@Type", row.Type)
                        cmd.Parameters.AddWithValue("@WareDate", row.WareDate)
                        cmd.Parameters.AddWithValue("@PropCode", row.PropCode)
                        cmd.Parameters.AddWithValue("@PropName", row.PropName)
                        cmd.Parameters.AddWithValue("@Category", row.Category)
                        cmd.Parameters.AddWithValue("@Status", row.Status)
                        cmd.Parameters.AddWithValue("@Description", row.Description)
                        cmd.Parameters.AddWithValue("@Unit", row.Unit)
                        cmd.Parameters.AddWithValue("@UnitPrice", row.UnitPrice)
                        cmd.Parameters.AddWithValue("@CurrentPrice", row.CurrentPrice)
                        cmd.Parameters.AddWithValue("@Amount", row.Amount)
                        cmd.Parameters.AddWithValue("@Quantity", row.Quantity)
                        cmd.Parameters.AddWithValue("@Comments", row.Comments)
                        cmd.Parameters.AddWithValue("@UpdateCount", row.UpdateCount)
                        cmd.Parameters.AddWithValue("@CreateDate", row.CreateDate)
                        cmd.Parameters.AddWithValue("@CreateTime", row.CreateTime)
                        cmd.Parameters.AddWithValue("@CreateUser", row.CreateUser)
                        cmd.Parameters.AddWithValue("@UpdateDate", row.UpdateDate)
                        cmd.Parameters.AddWithValue("@UpdateTime", row.UpdateTime)
                        cmd.Parameters.AddWithValue("@UpdateUser", row.UpdateUser)

                        res = cmd.ExecuteNonQuery()

                    End Using
                ElseIf row.RowState = DataRowState.Modified Then
                    Using cmd As New SqlCommand(sSQLU, dbConn.Conn)
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

                        cmd.Parameters.AddWithValue("@WareCode", row.WareCode)
                        cmd.Parameters.AddWithValue("@Type", row.Type)
                        cmd.Parameters.AddWithValue("@WareDate", row.WareDate)
                        cmd.Parameters.AddWithValue("@PropCode", row.PropCode)
                        cmd.Parameters.AddWithValue("@PropName", row.PropName)
                        cmd.Parameters.AddWithValue("@Category", row.Category)
                        cmd.Parameters.AddWithValue("@Status", row.Status)
                        cmd.Parameters.AddWithValue("@Description", row.Description)
                        cmd.Parameters.AddWithValue("@Unit", row.Unit)
                        cmd.Parameters.AddWithValue("@UnitPrice", row.UnitPrice)
                        cmd.Parameters.AddWithValue("@CurrentPrice", row.CurrentPrice)
                        cmd.Parameters.AddWithValue("@Amount", row.Amount)
                        cmd.Parameters.AddWithValue("@Quantity", row.Quantity)
                        cmd.Parameters.AddWithValue("@Comments", row.Comments)
                        cmd.Parameters.AddWithValue("@UpdateCount", row.UpdateCount + 1)
                        cmd.Parameters.AddWithValue("@UpdateDate", row.UpdateDate)
                        cmd.Parameters.AddWithValue("@UpdateTime", row.UpdateTime)
                        cmd.Parameters.AddWithValue("@UpdateUser", row.UpdateUser)
                        cmd.Parameters.AddWithValue("@ID", row("ID", DataRowVersion.Original))

                        res = cmd.ExecuteNonQuery()

                    End Using
                ElseIf row.RowState = DataRowState.Deleted Then
                    Using cmd As New SqlCommand(sSQLD, dbConn.Conn)
                        cmd.Transaction = dbConn.Tran

                        cmd.Parameters.AddWithValue("@ID", row("ID", DataRowVersion.Original))

                        res = cmd.ExecuteNonQuery()

                    End Using
                End If
            Next

            dbConn.CommitTran()
        Catch ex As Exception
            res = -1
            dbConn.RollbackTran()
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi cập nhật phiếu " + If(Me.WareType = 0, "nhập", "xuất") + ".", ex)
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
            Dim cmd As New SqlCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            Dim row As AtomyDataSet.PropertyRow = AtomyDataSet._Property.Rows(0)
            cmd.Parameters.AddWithValue("@Retired", True)
            cmd.Parameters.AddWithValue("@RetiredDate", New Date().ToString("yyyy/MM/dd"))
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
    Private Sub HelpCreateWareCode()
        lblWareCodeHint.Content = "Mã tiếp theo: " + Utility.HelpCreateCode("Warehouse")
    End Sub
#End Region

#Region "HelpGetLastWareCode"
    Private Sub HelpGetLastWareCode()
        lblWareCodeHint.Content = "Mã gần nhất: " + Utility.HelpGetLastCode("Warehouse", Me.WareType)
    End Sub
#End Region

#Region "CalculateTotal"
    Private Function CalculateTotal() As Tuple(Of Short, Decimal)
        Dim T1 As Short = 0
        Dim T2 As Decimal = 0

        For Each row As DataRow In AtomyDataSet.Warehouse.Rows
            If row.RowState = DataRowState.Added OrElse row.RowState = DataRowState.Unchanged OrElse row.RowState = DataRowState.Modified Then
                T1 += row("Quantity")
                T2 += row("Amount")
            End If
        Next
        Return New Tuple(Of Short, Decimal)(T1, T2)
    End Function
#End Region

#Region "CalculateSalesAmount"
    Private Function CalculateSalesAmount() As Decimal
        Dim decVal As Decimal = 0
        Dim canConvert As Boolean = Decimal.TryParse(txtDiscount.Text.Trim, decVal)
        If Not canConvert Then
            Return Nothing
        End If
        Dim Discount As Decimal = decVal
        canConvert = Decimal.TryParse(txtTotalAmount.Text.Trim, decVal)
        If Not canConvert Then
            Return Nothing
        End If
        Dim TotalAmount As Decimal = decVal
        Dim SalesAmount As Decimal = TotalAmount - Discount
        Return SalesAmount
    End Function
#End Region
#End Region

#Region "☆ SQL"
#Region "InsertSQL"
    Private Function InsertSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("INSERT INTO [WarehouseMaster]                               ")
        sb.AppendLine("            ( [WareCode],[Type],[WareDate],[EmpCode],[EmpName],[CusCode],[CusName],[Status],[WareTitle],[Description],[TotalAmount],[Discount],[SalesAmount],[PaymentType],[FinishFlag],[PaymentDate],[FinishDate],[Comments],[UpdateCount],[Retired],[RetiredDate],[CreateDate],[CreateTime],[CreateUser],[UpdateDate],[UpdateTime],[UpdateUser]) ")
        sb.AppendLine("     VALUES ( @WareCode,@Type,@WareDate,@EmpCode,@EmpName,@CusCode,@CusName,@Status,@WareTitle,@Description,@TotalAmount,@Discount,@SalesAmount,@PaymentType,@FinishFlag,@PaymentDate,@FinishDate,@Comments,@UpdateCount,@Retired,@RetiredDate,@CreateDate,@CreateTime,@CreateUser,@UpdateDate,@UpdateTime,@UpdateUser)")
        Return sb.ToString()
    End Function
#End Region
#Region "InsertDetailSQL"
    Private Function InsertDetailSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("INSERT INTO [Warehouse]                               ")
        sb.AppendLine("            ( [WareCode],[Type],[WareDate],[PropCode],[PropName],[Category],[Status],[Description],[Unit],[UnitPrice],[CurrentPrice],[Amount],[Quantity],[Comments],[UpdateCount],[CreateDate],[CreateTime],[CreateUser],[UpdateDate],[UpdateTime],[UpdateUser]) ")
        sb.AppendLine("     VALUES ( @WareCode,@Type,@WareDate,@PropCode,@PropName,@Category,@Status,@Description,@Unit,@UnitPrice,@CurrentPrice,@Amount,@Quantity,@Comments,@UpdateCount,@CreateDate,@CreateTime,@CreateUser,@UpdateDate,@UpdateTime,@UpdateUser) ")
        Return sb.ToString()
    End Function
#End Region

#Region "UpdateDetailSQL"
    Private Function UpdateDetailSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("Update [Warehouse]                                                                                                                                                                                                                                                                                                                                                                                                                               ")
        sb.AppendLine("   set [WareCode] = @WareCode,[Type] = @Type,[WareDate] = @WareDate,[PropCode] = @PropCode,[PropName] = @PropName,[Category] = @Category,[Status] = @Status,[Description] = @Description,[Unit] = @Unit,[UnitPrice] = @UnitPrice,[CurrentPrice] = @CurrentPrice,[Amount] = @Amount,[Quantity] = @Quantity,[Comments] = @Comments,[UpdateCount] = @UpdateCount,[UpdateDate] = @UpdateDate,[UpdateTime] = @UpdateTime,[UpdateUser] = @UpdateUser   ")
        sb.AppendLine(" where ID = @ID                                                                                                                                                                                                                                                                                                                                                                                                                                  ")
        Return sb.ToString()
    End Function
#End Region

#Region "DeleteDetailSQL"
    Private Function DeleteDetailSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("delete from [Warehouse] where ID = @ID   ")
        Return sb.ToString()
    End Function
#End Region

#Region "UpdatePropertySQL"
    Private Function UpdateSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("update [WarehouseMaster]                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             ")
        sb.AppendLine("   set [Type] = @Type,[WareDate] = @WareDate,[EmpCode] = @EmpCode,[EmpName] = @EmpName,[CusCode] = @CusCode,[CusName] = @CusName,[Status] = @Status,[WareTitle] = @WareTitle,[Description] = @Description,[TotalAmount] = @TotalAmount,[Discount] = @Discount,[SalesAmount] = @SalesAmount,[PaymentType] = @PaymentType,[FinishFlag] = @FinishFlag,[PaymentDate] = @PaymentDate,[FinishDate] = @FinishDate,[Comments] = @Comments,[UpdateCount] = @UpdateCount,[Retired] = @Retired,[RetiredDate] = @RetiredDate,[UpdateDate] = @UpdateDate,[UpdateTime] = @UpdateTime,[UpdateUser] = @UpdateUser    ")
        sb.AppendLine("     where [WareCode] = @WareCode                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    ")
        Return sb.ToString()
    End Function
#End Region

#Region "DeletePropertySQL"
    Private Function DeleteSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("UPDATE [Property]                                        ")
        sb.AppendLine("   SET [Retired] = @Retired                              ")
        sb.AppendLine("     , [RetiredDate] = @RetiredDate                      ")
        sb.AppendLine(" WHERE [PropCode] = @PropCode                            ")
        Return sb.ToString()
    End Function
#End Region
#End Region

#Region "EVENT"
#Region "searchWareHouseSearchResult"
    Private Sub searchWareHouseSearchResult(sender As Object, e As SearchDataArgs)
        Dim eW As SearchDataWarehouse = DirectCast(e, SearchDataWarehouse)
        If Me.WareType = 0 AndAlso eW.WareType = 1 Then
            If MessageBox.Show("Bạn đã chọn một phiếu xuất. Bạn có muốn chuyển sang phiếu xuất không?", Me.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.Yes Then
                Me.WareType = 1
                InitialValue()
            Else
                Return
            End If
        End If
        If Me.WareType = 1 AndAlso eW.WareType = 0 Then
            If MessageBox.Show("Bạn đã chọn một phiếu nhập. Bạn có muốn chuyển sang phiếu nhập không?", Me.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.Yes Then
                Me.WareType = 1
                InitialValue()
            Else
                Return
            End If
        End If
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
        txtCusCode.Focus()
        txtCusCode.Text = e.Code
        txtCusName.Focus()
        txtCusName.Text = e.Name
    End Sub
#End Region

#Region "searchEmpSearchResult"
    Private Sub searchEmpSearchResult(sender As Object, e As SearchDataArgs)
        txtEmpCode.Focus()
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
                End If
            ElseIf Mode = DataRowState.Modified OrElse Mode = DataRowState.Deleted Then
                LoadData(txtWareCode.Text.Trim)
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô mã.", ex)
        End Try
    End Sub
#End Region

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
                        MessageBox.Show("Mã khách hàng không tồn tại.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
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
                        MessageBox.Show("Mã nhân viên không tồn tại.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
                        txtEmpCode.Text = ""
                        lblEmpName.Content = ""
                    End If
                Else
                    lblEmpName.Content = ""
                End If
            End If

            If txtCode.Name.Equals("txtPropCode") Then
                If Not TypeOf grdWareHouse.SelectedItem Is DataRowView Then
                    Return
                End If
                Dim drv As DataRowView = grdWareHouse.SelectedItem
                If drv Is Nothing Then
                    Return
                End If

                If txtCode.Text.Trim.Length > 0 Then
                    Dim row As DataGridRow = Nothing
                    row = grdWareHouse.GetRow(grdWareHouse.SelectedIndex)
                    'Check whether value change or not
                    If String.Compare(drv.Row("PropCode").ToString, txtCode.Text.Trim) = 0 Then
                        Return
                    End If

                    Dim dr As DataRow = Check.GetDataByCode("Property", txtCode.Text.Trim)
                    If dr IsNot Nothing Then
                        drv.Row("PropName") = dr("PropName")
                        Dim cellName As DataGridCell = grdWareHouse.GetCell(row, 1)
                        cellName.SetTemplateTexBlockContent("lblPropName", dr("PropName"))

                        drv.Row("Unit") = dr("Unit")
                        Dim cellUnit As DataGridCell = grdWareHouse.GetCell(row, 2)
                        cellUnit.SetTemplateTexBlockContent("lblUnit", dr("Unit"))

                        drv.Row("UnitPrice") = If(Me.WareType = 0, dr("PurchasePrice"), dr("SalesPrice"))
                        Dim cellUnitPrice As DataGridCell = grdWareHouse.GetCell(row, 3)
                        cellUnitPrice.SetTemplateTexBlockContent("lblUnitPrice", If(Me.WareType = 0, dr("PurchasePrice"), dr("SalesPrice")))
                        drv.Row("CurrentPrice") = If(Me.WareType = 0, dr("PurchasePrice"), dr("SalesPrice"))
                        Dim cellCurrentPrice As DataGridCell = grdWareHouse.GetCell(row, 4)
                        cellCurrentPrice.SetTemplateTexBlockContent("lblCurrentPrice", If(Me.WareType = 0, dr("PurchasePrice"), dr("SalesPrice")))
                        Dim amount As Decimal = 0
                        Dim currentPrice As Decimal = If(Me.WareType = 0, dr("PurchasePrice"), dr("SalesPrice"))
                        Dim quantity As Int16 = drv.Row("Quantity")
                        amount = currentPrice * quantity
                        drv.Row("Amount") = amount
                        Dim cellAmount As DataGridCell = grdWareHouse.GetCell(row, 6)
                        cellAmount.SetTemplateTexBlockContent("lblAmount", amount)
                    Else
                        MessageBox.Show("Mã mặt hàng không tồn tại.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
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

#Region "txtCurrentPrice_LostKeyboardFocus"
    Private Sub txtCurrentPrice_LostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        Try
            If Not TypeOf grdWareHouse.SelectedItem Is DataRowView Then
                Return
            End If
            Dim drv As DataRowView = grdWareHouse.SelectedItem
            If drv Is Nothing Then
                Return
            End If
            Dim txtCurrentPrice = DirectCast(sender, TextBox)
            If txtCurrentPrice.Text.Trim.Length > 0 Then
                Dim row As DataGridRow = Nothing
                row = grdWareHouse.GetRow(grdWareHouse.SelectedIndex)
                'Check whether value change or not
                If String.Compare(drv.Row("CurrentPrice").ToString, txtCurrentPrice.Text.Trim) = 0 Then
                    Return
                End If
                Dim decVal As Decimal = 0
                Dim canConvert As Boolean = Decimal.TryParse(txtCurrentPrice.Text.Trim, decVal)
                If Not canConvert Then
                    Return
                End If
                Dim currentPrice As Decimal = CDec(txtCurrentPrice.Text.Trim)
                drv.Row("CurrentPrice") = currentPrice
                Dim amount As Decimal = 0
                Dim quantity As Int16 = drv.Row("Quantity")
                amount = currentPrice * quantity
                Dim cellAmount As DataGridCell = grdWareHouse.GetCell(row, 6)
                cellAmount.SetTemplateTexBlockContent("lblAmount", amount.ToString("N0"))
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô giá bán.", ex)
        End Try
    End Sub
#End Region

#Region "txtQuantity_LostKeyboardFocus"
    Private Sub txtQuantity_LostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        Try
            If Not TypeOf grdWareHouse.SelectedItem Is DataRowView Then
                Return
            End If
            Dim drv As DataRowView = grdWareHouse.SelectedItem
            If drv Is Nothing Then
                Return
            End If
            Dim txtQuantity = DirectCast(sender, TextBox)
            If txtQuantity.Text.Trim.Length > 0 Then
                Dim row As DataGridRow = Nothing
                row = grdWareHouse.GetRow(grdWareHouse.SelectedIndex)
                'Check whether value change or not
                If String.Compare(drv.Row("Quantity").ToString, txtQuantity.Text.Trim) = 0 Then
                    Return
                End If
                Dim intVal As Int16 = 0
                Dim canConvert As Boolean = Int16.TryParse(txtQuantity.Text.Trim, intVal)
                If Not canConvert Then
                    Return
                End If

                Dim currentPrice As Decimal = drv.Row("CurrentPrice")
                Dim amount As Decimal = 0
                Dim quantity As Int16 = CShort(txtQuantity.Text.Trim)
                drv.Row("Quantity") = quantity
                amount = currentPrice * quantity
                drv.Row("Amount") = amount
                Dim cellAmount As DataGridCell = grdWareHouse.GetCell(row, 6)
                cellAmount.SetTemplateTexBlockContent("lblAmount", amount.ToString("N0"))
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô số lượng.", ex)
        End Try
    End Sub
#End Region

#Region "TextBox_GotFocus"
    Private Sub TextBox_GotFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        Try
            CType(sender, TextBox).SelectAll()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi bắt đầu nhập liệu ô.", ex)
        End Try
    End Sub
#End Region

#Region "grdWareHouse_CellEditEnding"
    Private Sub grdWareHouse_CellEditEnding(sender As Object, e As DataGridCellEditEndingEventArgs)
        Try
            If Not TypeOf grdWareHouse.SelectedItem Is DataRowView Then
                Return
            End If
            Dim drv As DataRowView = grdWareHouse.SelectedItem
            If drv Is Nothing Then
                Return
            End If
            Dim row As DataGridRow = Nothing
            row = grdWareHouse.GetRow(grdWareHouse.SelectedIndex)
            Select Case e.Column.DisplayIndex
                Case 5
                    Dim cell4 As DataGridCell = grdWareHouse.GetCell(row, 5)
                    Dim textbox As TextBox = cell4.GetItem(Of TextBox)("txtQuantity")
                    Dim intVal As Int16 = 0
                    Dim intSum As Int16 = 0
                    Int16.TryParse(txtSumaryQuantity.Text, intSum)
                    If Int16.TryParse(textbox.Text, intVal) Then
                        intSum = intSum - drv.Row("Quantity") + intVal
                        txtSumaryQuantity.Text = intSum.ToString("#,##0")
                    End If

                Case 6
                    Dim cell4 As DataGridCell = grdWareHouse.GetCell(row, 6)
                    Dim textbox As TextBox = cell4.GetItem(Of TextBox)("txtAmount")
                    Dim decVal As Decimal = 0
                    Dim decSum As Decimal = 0
                    Decimal.TryParse(txtTotalAmount.Text, decSum)
                    If Decimal.TryParse(textbox.Text, decVal) Then
                        decSum = decSum - drv.Row("Amount") + decVal
                        txtTotalAmount.Text = decSum.ToString("#,##0")
                    End If
                Case Else

            End Select

        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhập liệu trên lưới.", ex)
        End Try
    End Sub
#End Region

#Region "grdWareHouse_PreparingCellForEdit"
    Private Sub grdWareHouse_PreparingCellForEdit(sender As Object, e As DataGridPreparingCellForEditEventArgs)
        Try
            If Not TypeOf grdWareHouse.SelectedItem Is DataRowView Then
                Return
            End If
            Dim drv As DataRowView = grdWareHouse.SelectedItem
            If drv Is Nothing Then
                Return
            End If
            Dim row As DataGridRow = Nothing
            row = grdWareHouse.GetRow(grdWareHouse.SelectedIndex)
            Select Case e.Column.DisplayIndex
                Case 0
                    Dim cell4 As DataGridCell = grdWareHouse.GetCell(row, 0)
                    Dim textbox As TextBox = cell4.GetItem(Of TextBox)("txtPropCode")
                    'textbox.SelectAll()
                    textbox.CaretIndex = textbox.Text.Length
                Case 2
                    Dim cell4 As DataGridCell = grdWareHouse.GetCell(row, 2)
                    Dim textbox As TextBox = cell4.GetItem(Of TextBox)("txtUnit")
                    'textbox.SelectAll()
                    textbox.CaretIndex = textbox.Text.Length
                Case 4
                    Dim cell4 As DataGridCell = grdWareHouse.GetCell(row, 4)
                    Dim textbox As TextBox = cell4.GetItem(Of TextBox)("txtCurrentPrice")
                    'textbox.SelectAll()
                    textbox.CaretIndex = textbox.Text.Length
                Case 5
                    Dim cell4 As DataGridCell = grdWareHouse.GetCell(row, 5)
                    Dim textbox As TextBox = cell4.GetItem(Of TextBox)("txtQuantity")
                    'textbox.SelectAll()
                    textbox.CaretIndex = textbox.Text.Length
                Case 6
                    Dim cell4 As DataGridCell = grdWareHouse.GetCell(row, 6)
                    Dim textbox As TextBox = cell4.GetItem(Of TextBox)("txtAmount")
                    'textbox.SelectAll()
                    textbox.CaretIndex = textbox.Text.Length
                Case Else

            End Select
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi chuẩn bị nhập liệu trên lưới.", ex)
        End Try
    End Sub
#End Region

#Region "lnkWareCode_Click"
    Private Sub lnkWareCode_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim search As New Search()
            AddHandler search.SearchResult, AddressOf searchWareHouseSearchResult
            search.Kind = If(Me.WareType = 0, EnumSearch.SearchWareHouseIn, EnumSearch.SearchWareHouse)
            search.ShowDialog()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào link Mã phiếu " + If(Me.WareType = 0, "nhập", "xuất") + ".", ex)
        End Try
    End Sub
#End Region

#Region "txtPropCode_KeyDown"
    Private Sub txtPropCode_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            If e.Key = Key.OemPlus AndAlso (Keyboard.IsKeyDown(Key.LeftShift) OrElse Keyboard.IsKeyDown(Key.RightShift)) Then
                Dim search As New Search()
                AddHandler search.SearchResult, AddressOf SearchPropCode_Result
                search.Kind = EnumSearch.SearchProperty
                search.ShowDialog()
                e.Handled = True
            End If

        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi chọn mặt hàng.", ex)
        End Try
    End Sub

    Private Sub SearchPropCode_Result(sender As Object, SearchDataArgs As SearchDataArgs)
        If SearchDataArgs Is Nothing Then
            MessageBox.Show("Mã mặt hàng không tồn tại", Me.Title, MessageBoxButton.OK)
        Else

            If Not TypeOf grdWareHouse.SelectedItem Is DataRowView Then
                Return
            End If
            Dim drv As DataRowView = grdWareHouse.SelectedItem
            If drv Is Nothing Then
                Return
            End If

            Dim row As DataGridRow = Nothing
            row = grdWareHouse.GetRow(grdWareHouse.SelectedIndex)

            Dim dr As DataRow = Check.GetDataByCode("Property", SearchDataArgs.Code)
            If dr IsNot Nothing Then
                drv.Row("PropCode") = dr("PropCode")
                Dim cellPropCode As DataGridCell = grdWareHouse.GetCell(row, 0)
                cellPropCode.SetTemplateTexContent("txtPropCode", dr("PropCode"))

                drv.Row("PropName") = dr("PropName")
                Dim cellName As DataGridCell = grdWareHouse.GetCell(row, 1)
                cellName.SetTemplateTexBlockContent("lblPropName", dr("PropName"))

                drv.Row("Unit") = dr("Unit")
                Dim cellUnit As DataGridCell = grdWareHouse.GetCell(row, 2)
                cellUnit.SetTemplateTexBlockContent("lblUnit", dr("Unit"))

                drv.Row("UnitPrice") = dr("SalesPrice")
                Dim cellUnitPrice As DataGridCell = grdWareHouse.GetCell(row, 3)
                cellUnitPrice.SetTemplateTexBlockContent("lblUnitPrice", dr("SalesPrice"))
                drv.Row("CurrentPrice") = dr("SalesPrice")
                Dim cellCurrentPrice As DataGridCell = grdWareHouse.GetCell(row, 4)
                cellCurrentPrice.SetTemplateTexBlockContent("lblCurrentPrice", dr("SalesPrice"))
                Dim amount As Decimal = 0
                Dim currentPrice As Decimal = dr("SalesPrice")
                Dim quantity As Int16 = drv.Row("Quantity")
                amount = currentPrice * quantity
                drv.Row("Amount") = amount
                Dim cellAmount As DataGridCell = grdWareHouse.GetCell(row, 6)
                cellAmount.SetTemplateTexBlockContent("lblAmount", amount)
            Else
                MessageBox.Show("Mã mặt hàng không tồn tại.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
            End If

        End If

    End Sub
#End Region

#Region "txtDiscount_LostKeyboardFocus"
    Private Sub txtDiscount_LostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        Try
            txtSalesAmount.Text = CalculateSalesAmount()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô chiết khấu.", ex)
        End Try
    End Sub
#End Region

#Region "txtWareDate_LostKeyboardFocus"
    Private Sub txtWareDate_LostKeyboardFocus(sender As Object, e As KeyboardFocusChangedEventArgs)
        Try
            If rbPaymentCash.IsChecked Then
                If String.Compare(txtWareDate.Text, Me.AtomyDataSet.WarehouseMaster.Rows(0)("WareDate").ToString) <> 0 Then
                    Dim dc As New DateConverter()
                    txtPaymentDate.Text = dc.ConvertBack(txtWareDate.Text, GetType(String), Nothing, System.Threading.Thread.CurrentThread.CurrentCulture)
                End If
            ElseIf rbPaymentShipCode.IsChecked Then
                If String.Compare(txtWareDate.Text, Me.AtomyDataSet.WarehouseMaster.Rows(0)("WareDate").ToString) <> 0 Then
                    Dim dc As New DateConverter()
                    txtPaymentDate.Text = dc.ConvertBack(txtWareDate.Text, GetType(String), Nothing, System.Threading.Thread.CurrentThread.CurrentCulture)
                End If
            End If

        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô ngày " + If(Me.WareType = 0, "nhập", "xuất") + ".", ex)
        End Try
    End Sub
#End Region

#Region "btnPrint_Click"
    Private Sub btnPrint_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            Dim applicationFolder = Path.Combine(appData, "Product Management Atomy")
            Dim dirI As DirectoryInfo = Nothing
            If Not Directory.Exists(applicationFolder) Then
                MessageBox.Show("Đây là lần in đầu tiên, phần mềm sẽ tự động tạo thư mục riêng và tạo shortcut [Product Management Atomy] trên desktop.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Information)
                dirI = Directory.CreateDirectory(applicationFolder)
                PrintWarehouseExcel.CreateShortCut()
            Else
                dirI = New DirectoryInfo(applicationFolder)
            End If
            Dim fileName As String = "PXK" + txtWareCode.Text.Trim + " " + DateTime.Now.ToString("yyyyMMdd") + ".xlsx"
            Dim print As New PrintWarehouseExcel()
            Dim filePath As String = Path.Combine(dirI.FullName, fileName)
            Dim res As Boolean = print.Print(txtWareCode.Text.Trim, filePath)
            If res Then
                'Dim rpt As New OrderA5_1(filePath)
                'rpt.ShowDialog()
                MessageBox.Show("Xuất in thành công ra file excel [" + fileName + "].", Me.Title, MessageBoxButton.OK, MessageBoxImage.Information)
                If My.Settings.AutoOpenExel Then
                    Process.Start(filePath)
                End If
            Else
                MessageBox.Show("Xuất in lỗi.", Me.Title, MessageBoxButton.OK, MessageBoxImage.Warning)
            End If

        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn nút In.", ex)
        End Try
    End Sub
#End Region
#End Region

End Class
