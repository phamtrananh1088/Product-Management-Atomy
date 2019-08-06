Imports System.Data.OleDb
Imports System.Data
Imports System.Text

Public Class Property1
    Private AtomyDataSet As AtomyDataSet
    Private Mode As DataRowState

    Public Sub New()
        AtomyDataSet = New AtomyDataSet()
        Mode = DataRowState.Added
        ' This call is required by the designer.
        InitializeComponent()

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
            AtomyDataSet._Property.Clear()
            adapt.Fill(AtomyDataSet, "Property")
            If AtomyDataSet._Property.Rows.Count > 0 Then
                Me.DataContext = AtomyDataSet._Property.Rows(0)
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

#Region "CtrEnable"
    Private Sub CtrEnable()
        If Mode = DataRowState.Modified Then

            txtPropCd.IsEnabled = False

     
        Else

            txtPropCd.IsEnabled = True
          
        End If

    End Sub
#End Region

#Region "formload"
    Private Sub formload(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

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
                    If Check.IsExisted("Property", txtPropCd.Text.Trim) Then
                        MessageBox.Show("Mã sản phẩm đã tồn tại.")
                        HelpCreateCode()
                        Return
                    End If

                    If Insert() Then
                        MessageBox.Show("Đã hoàn thành.")
                        lblPropCodeHint.Content = ""
                        LoadData(txtPropCd.Text.Trim)
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
                        LoadData(txtPropCd.Text.Trim)
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
                Dim confirm As Boolean = (MessageBox.Show("Bạn có muốn xóa mặt hàng này không?", "Atomy", MessageBoxButton.YesNo) = MessageBoxResult.OK)
                If confirm Then
                    If Delete() Then
                        MessageBox.Show("Đã hoàn thành.")
                        lblPropCodeHint.Content = ""

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
            AtomyDataSet._Property.Clear()
            Dim newRow As AtomyDataSet.PropertyRow = AtomyDataSet._Property.NewPropertyRow()
            AtomyDataSet._Property.Rows.Add(newRow)
            Me.DataContext = AtomyDataSet._Property.Rows(0)
            Mode = DataRowState.Added
            CtrEnable()
            HelpCreateCode()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào nút Thêm.", ex)
        End Try
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
            ErrorLog.SetError(Me, "Đã xảy ra lỗi khi nhấn vào link Mã sản phẩm.", ex)
        End Try
    End Sub
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

            End If
        Catch ex As Exception
            ErrorLog.SetError(Me, "Đã xảy ra lỗi ở ô mã.", ex)
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
                cmd.Parameters.Add("@3", OleDbType.VarChar).Value = IIf(row.Description Is Nothing, "", row.Description)
                cmd.Parameters.Add("@4", OleDbType.VarChar).Value = IIf(row.Category Is Nothing, "", row.Category)
                cmd.Parameters.Add("@5", OleDbType.VarChar).Value = IIf(row.Condition Is Nothing, "", row.Condition)
                cmd.Parameters.Add("@6", OleDbType.VarChar).Value = IIf(row.AcquiredDate Is Nothing, now.ToString("yyyy/MM/dd"), row.AcquiredDate)
                cmd.Parameters.Add("@7", OleDbType.VarChar).Value = IIf(row.Unit Is Nothing, "", row.Unit)
                cmd.Parameters.Add("@8", OleDbType.Currency).Value = row.PurchasePrice
                cmd.Parameters.Add("@9", OleDbType.Currency).Value = row.SalesPrice
                cmd.Parameters.Add("@10", OleDbType.Currency).Value = row.CurrentValue
                cmd.Parameters.Add("@11", OleDbType.VarChar).Value = IIf(row.Location Is Nothing, Utility.DefaultData.DefaultLocation, row.Location)
                cmd.Parameters.Add("@12", OleDbType.VarChar).Value = IIf(row.Manufacturer Is Nothing, Utility.DefaultData.DefaultManufacturer, row.Manufacturer)
                cmd.Parameters.Add("@13", OleDbType.VarChar).Value = IIf(row.Model Is Nothing, "", row.Model)
                cmd.Parameters.Add("@14", OleDbType.VarChar).Value = IIf(row.Comments Is Nothing, "", row.Comments)
                cmd.Parameters.Add("@15", OleDbType.VarChar).Value = IIf(row.RetiredDate Is Nothing, "", row.RetiredDate)
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
            ErrorLog.SetError(Me, "Đã sảy ra lỗi khi cập nhật sản phẩm.", ex)
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
            cmd.Parameters.Add("@Description", OleDbType.VarChar).Value = IIf(row.Description Is Nothing, "", row.Description)
            cmd.Parameters.Add("@Category", OleDbType.VarChar).Value = IIf(row.Category Is Nothing, "", row.Category)
            cmd.Parameters.Add("@Condition", OleDbType.VarChar).Value = IIf(row.Condition Is Nothing, "", row.Condition)
            cmd.Parameters.Add("@AcquiredDate", OleDbType.VarChar).Value = IIf(row.AcquiredDate Is Nothing, now.ToString("yyyy/MM/dd"), row.AcquiredDate)
            cmd.Parameters.Add("@Unit", OleDbType.VarChar).Value = IIf(row.Unit Is Nothing, "", row.Unit)
            cmd.Parameters.Add("@PurchasePrice", OleDbType.Currency).Value = row.PurchasePrice
            cmd.Parameters.Add("@SalesPrice", OleDbType.Currency).Value = row.SalesPrice
            cmd.Parameters.Add("@CurrentValue", OleDbType.Currency).Value = row.CurrentValue
            cmd.Parameters.Add("@Location", OleDbType.VarChar).Value = IIf(row.Location Is Nothing, Utility.DefaultData.DefaultLocation, row.Location)
            cmd.Parameters.Add("@Manufacturer", OleDbType.VarChar).Value = IIf(row.Manufacturer Is Nothing, Utility.DefaultData.DefaultManufacturer, row.Manufacturer)
            cmd.Parameters.Add("@Model", OleDbType.VarChar).Value = IIf(row.Model Is Nothing, "", row.Model)
            cmd.Parameters.Add("@Comments", OleDbType.VarChar).Value = IIf(row.Comments Is Nothing, "", row.Comments)
            cmd.Parameters.Add("@RetiredDate", OleDbType.VarChar).Value = IIf(row.RetiredDate Is Nothing, "", row.RetiredDate)
            cmd.Parameters.Add("@CreateDate", OleDbType.VarChar).Value = row.CreateDate
            cmd.Parameters.Add("@CreateTime", OleDbType.VarChar).Value = row.CreateTime
            cmd.Parameters.Add("@CreateUser", OleDbType.VarChar).Value = row.CreateUser
            cmd.Parameters.Add("@UpdateDate", OleDbType.VarChar).Value = row.UpdateDate
            cmd.Parameters.Add("@UpdateTime", OleDbType.VarChar).Value = row.UpdateTime
            cmd.Parameters.Add("@UpdateUser", OleDbType.VarChar).Value = row.UpdateUser
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

#Region "HelpCreateCode"
    Private Sub HelpCreateCode()
        lblPropCodeHint.Content = "Gợi ý: " + Utility.HelpCreateCode("Property")
    End Sub

#End Region
#End Region

#Region "☆ SQL"
#Region "InsertPropertySQL"
    Private Function InsertSQL() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("INSERT INTO [Property]                               ")
        sb.AppendLine("            ( [PropCode], [PropName], [Description], [Category], [Condition], [Acquired Date], [Unit], [Purchase Price], [Sales Price], [Current Value], [Location], [Manufacturer], [Model], [Comments], [Retired Date], [Create Date], [Create Time], [Create User], [Update Date], [Update Time], [Update User]) ")
        sb.AppendLine("     VALUES ( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)                                          ")
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

End Class
