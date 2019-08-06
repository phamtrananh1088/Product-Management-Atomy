Imports System.Data
Imports System.Data.OleDb
Imports System.Text

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

        ' Add any initialization after the InitializeComponent() call.
    End Sub
#End Region

#Region "BUSINESS"
#Region "LoadData"
    Private Sub LoadData(Cd As String)
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim sSQL As String = "select * from [WarehouseMaster] where [WareCode] = ?"
            Dim adapt As New OleDbDataAdapter(sSQL, dbConn.Conn)
            adapt.SelectCommand.Parameters.Add("@WareCode", OleDbType.VarChar).Value = Cd
            AtomyDataSet.WarehouseMaster.Clear()
            adapt.Fill(AtomyDataSet, "WarehouseMaster")
            If AtomyDataSet.WarehouseMaster.Rows.Count > 0 Then
                sSQL = "select * from [Warehouse] where [WareCode] = ?"
                adapt.SelectCommand.CommandText = sSQL
                AtomyDataSet.Warehouse.Clear()
                adapt.Fill(AtomyDataSet, "Warehouse")
                Me.DataContext = AtomyDataSet.WarehouseMaster.Rows(0)
                grdWareHouse.ItemsSource = AtomyDataSet.Warehouse.DefaultView
                Mode = DataRowState.Modified
                CtrEnable()
            Else

            End If

        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi lấy dữ liệu.", ex)
        Finally
            dbConn.Close()
        End Try

    End Sub
#End Region

#Region "ValidateData"
    Private Function ValidateData(action As EnumAction) As Boolean
        Dim hasError As Boolean
        Select Case action
            Case EnumAction.Update
                hasError = Validation.GetHasError(txtWareCode)
                hasError = hasError OrElse Validation.GetHasError(txtWareTitle)
                hasError = hasError OrElse Validation.GetHasError(txtCusCd)
                hasError = hasError OrElse Validation.GetHasError(txtEmpCode)
                hasError = hasError OrElse Validation.GetHasError(txtCusName)
                hasError = hasError OrElse Validation.GetHasError(grdWareHouse)
            Case EnumAction.Insert
                hasError = Validation.GetHasError(txtWareCode)
                hasError = hasError OrElse Validation.GetHasError(txtWareTitle)
                hasError = hasError OrElse Validation.GetHasError(txtCusCd)
                hasError = hasError OrElse Validation.GetHasError(txtEmpCode)
                hasError = hasError OrElse Validation.GetHasError(txtCusName)
                hasError = hasError OrElse Validation.GetHasError(grdWareHouse)
            Case EnumAction.Delete

        End Select
        Return Not hasError
    End Function
#End Region
#Region "CtrEnable"
    Private Sub CtrEnable()
        If Mode = DataRowState.Modified Then
            txtWareCode.IsEnabled = False

           
        Else
            txtWareCode.IsEnabled = True


        End If

    End Sub
#End Region

#Region "HelpCreateCode"
    Private Sub HelpCreateCode()
        lblWareCodeHint.Content = "Gợi ý: " + Utility.HelpCreateCode("Warehouse")
    End Sub

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
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi xóa sản phẩm.", ex)
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
        Return res
    End Function
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
#Region "txtEmpCodeLostFocus"
    Private Sub lnkCusCd_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim search As New Search()
            AddHandler search.SearchResult, AddressOf searchCusSearchResult
            search.Kind = EnumSearch.SearchCustomer
            search.ShowDialog()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào link Mã sản phẩm.", ex)
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
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào link Mã sản phẩm.", ex)
        End Try
    End Sub
#End Region

#Region "btnInsert_Click"
    Private Sub btnInsert_Click(sender As Object, e As RoutedEventArgs)
        Try
            AtomyDataSet.WarehouseMaster.Clear()
            AtomyDataSet.Warehouse.Clear()
            Dim newRow As AtomyDataSet.WarehouseMasterRow = AtomyDataSet.WarehouseMaster.NewWarehouseMasterRow()
            AtomyDataSet.WarehouseMaster.Rows.Add(newRow)
            Me.DataContext = AtomyDataSet.WarehouseMaster.Rows(0)
            grdWareHouse.ItemsSource = AtomyDataSet.Warehouse.DefaultView
            Mode = DataRowState.Added
            CtrEnable()
            HelpCreateCode()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào nút Thêm.", ex)
        End Try
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
                    If Check.IsExisted("Warehouse", txtWareCode.Text.Trim) Then
                        MessageBox.Show("Mã phiếu xuất đã tồn tại.")
                        HelpCreateCode()
                        Return
                    End If

                    If Insert() Then
                        MessageBox.Show("Đã hoàn thành.")
                        lblWareCodeHint.Content = ""
                        LoadData(txtWareCode.Text.Trim)
                    Else
                        MessageBox.Show("Không thành công.")
                    End If
                Case DataRowState.Modified
                    If Not ValidateData(EnumAction.Update) Then
                        Return
                    End If
                    If Update() Then
                        MessageBox.Show("Đã hoàn thành.")
                        lblWareCodeHint.Content = ""
                        LoadData(txtWareCode.Text.Trim)
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
            If Mode = DataRowState.Modified Then
                Dim confirm As Boolean = (MessageBox.Show("Bạn có muốn xóa phiếu này không?", "Atomy", MessageBoxButton.YesNo) = MessageBoxResult.OK)
                If confirm Then
                    If Delete() Then
                        MessageBox.Show("Đã hoàn thành.")

                    End If
                End If
            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào nút Xóa.", ex)
        End Try

    End Sub
#End Region

#Region "searchCusSearchResult"
    Private Sub searchCusSearchResult(sender As Object, e As SearchDataArgs)
        txtCusCd.Text = e.Code
        txtCusName.Text = e.Name
    End Sub
#End Region

#Region "searchEmpSearchResult"
    Private Sub searchEmpSearchResult(sender As Object, e As SearchDataArgs)
        txtEmpCode.Text = e.Code
        lblEmpName.Content = e.Name
    End Sub
#End Region

#Region "searchWareHouseSearchResult"
    Private Sub searchWareHouseSearchResult(sender As Object, e As SearchDataArgs)
         LoadData(e.Code)
    End Sub
#End Region

#Region "txtWareCode_LostFocus"
    Private Sub txtWareCode_LostFocus(sender As Object, e As RoutedEventArgs)
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

                If txtWareCode.Text.Trim.Length > 0 AndAlso Check.IsExisted("Warehouse", txtWareCode.Text.Trim) Then
                    MessageBox.Show("Mã phiếu bán hàng đã tồn tại.", Utility.AppCaption)
                    txtWareCode.Text = ""
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
            If txtCode.Equals(txtCusCd) Then
                If txtCode.Text.Trim.Length > 0 Then
                    Dim dr As DataRow = Check.GetDataByCode("Customer", txtCusCd.Text.Trim)
                    If dr IsNot Nothing Then
                        txtCusName.Text = dr("FirstName").ToString() + " " + dr("LastName").ToString()
                    Else
                        MessageBox.Show("Mã khách hàng không tồn tại.", Utility.AppCaption)
                        txtCusCd.Text = ""
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
                    row = grdWareHouse.ItemContainerGenerator.ContainerFromIndex(grdWareHouse.SelectedIndex)
                    Dim ctr = row.FindName("lblPropName")
                    Dim dr As DataRow = Check.GetDataByCode("Property", txtCode.Text.Trim)
                    If dr IsNot Nothing Then
                        drv.Row("PropName") = dr("PropName").ToString()
                    Else
                        MessageBox.Show("Mã sản phẩm không tồn tại.", Utility.AppCaption)
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

#Region "txtCode_LostKeyboardFocus"
    Private Sub grdWareHouse_CellEditEnding(sender As Object, e As DataGridCellEditEndingEventArgs)
        Try
            Dim el As FrameworkElement = e.Column.GetCellContent(e.Row)
            If TypeOf el Is TextBox Then
                Dim tEl As TextBox = DirectCast(el, TextBox)
                If tEl.Name.StartsWith("txtPropCode") Then

                End If

            End If

        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở lưới.", ex)
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

#End Region

End Class
