Imports SpreadsheetGear
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports IWshRuntimeLibrary
Imports System.IO
Imports System.Threading

Public Class PrintWarehouseExcel
    Public Function Print(aWareCode As String, sFilePath As String) As Boolean
        Dim sTemplate_OrderA5_1 As String = ".\Excel\Template\OrderA5_1.xlsx"
        Dim xlBookDB As IWorkbook = SpreadsheetGear.Factory.GetWorkbook(sTemplate_OrderA5_1)
        Dim sSQL As String = GetPrintWarehouseExcelSQL()
        Dim ds As New DataSet()
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim adap As New SqlDataAdapter(sSQL, dbConn.Conn)
            adap.SelectCommand.Parameters.AddWithValue("@WareCode", aWareCode)
            If adap.Fill(ds) = 0 Then
                Return False
            End If
        Catch ex As Exception
            ErrorLog.SetError("", "Đã sảy ra lỗi lấy số liệu cho hóa đơn bán hàng.", ex)
            Return False
        Finally
            dbConn.Close()
        End Try
        Dim xlSheet As IWorksheet = xlBookDB.Worksheets("Data")
        Dim fi As Action(Of DataRow, Int32) = Sub(r As DataRow, i As Int32)
                                                  Dim j As Int32 = i + 2
                                                  xlSheet.Cells(String.Format("A{0}", j)).Value = "'" + r("WareCode")
                                                  xlSheet.Cells(String.Format("B{0}", j)).Value = r("WareDate")
                                                  xlSheet.Cells(String.Format("C{0}", j)).Value = r("Discount")
                                                  xlSheet.Cells(String.Format("D{0}", j)).Value = r("SalesAmount")
                                                  xlSheet.Cells(String.Format("E{0}", j)).Value = r("CusCode")
                                                  xlSheet.Cells(String.Format("F{0}", j)).Value = r("CusName")
                                                  xlSheet.Cells(String.Format("G{0}", j)).Value = "'" + r("MobilePhone")
                                                  xlSheet.Cells(String.Format("H{0}", j)).Value = r("Address")
                                                  xlSheet.Cells(String.Format("I{0}", j)).Value = r("PropName")
                                                  xlSheet.Cells(String.Format("J{0}", j)).Value = r("Unit")
                                                  xlSheet.Cells(String.Format("K{0}", j)).Value = r("Quantity")
                                                  xlSheet.Cells(String.Format("L{0}", j)).Value = r("CurrentPrice")
                                                  xlSheet.Cells(String.Format("M{0}", j)).Value = r("Amount")
                                              End Sub
        For index = 0 To ds.Tables(0).Rows.Count - 1
            Dim r As DataRow = ds.Tables(0).Rows(index)
            fi(r, index)
        Next

        Dim bW As Boolean = Helper.CanReadFile(sFilePath)

        If Not bW Then
            bW = MessageBox.Show(sFilePath & " đang mở. Bạn nên đóng vào trước khi tiếp tục ?", Utility.AppCaption, MessageBoxButton.YesNo) = MessageBoxResult.Yes

            If bW Then
                Dim pp As Integer = 10

                While (Not (bW = Helper.CanReadFile(sFilePath))) AndAlso pp > 0
                    pp -= 1
                    Thread.Sleep(5000)
                End While
            End If
        End If

        If bW Then
            xlBookDB.SaveAs(sFilePath, FileFormat.OpenXMLWorkbook)
            Return True
        Else
            Return False
        End If

        Return True
    End Function

    Private Function GetPrintWarehouseExcelSQL() As String
        Dim sb As New StringBuilder()

        sb.AppendLine("SELECT WarehouseMaster.WareCode                                                                   ")
        sb.AppendLine("     , WarehouseMaster.WareDate                                                                   ")
        sb.AppendLine("     , WarehouseMaster.Discount                                                                   ")
        sb.AppendLine("     , WarehouseMaster.SalesAmount                                                                ")
        sb.AppendLine("     , WarehouseMaster.CusCode                                                                    ")
        sb.AppendLine("     , WarehouseMaster.CusName                                                                    ")
        sb.AppendLine("     , Customer.MobilePhone                                                                       ")
        sb.AppendLine("     , Customer.Address + ' ' + Customer.City + ' ' + Customer.StateProvince AS  Address          ")
        sb.AppendLine("     , Warehouse.PropName                                                                         ")
        sb.AppendLine("     , Warehouse.Unit                                                                             ")
        sb.AppendLine("     , Warehouse.Quantity                                                                         ")
        sb.AppendLine("     , Warehouse.CurrentPrice                                                                     ")
        sb.AppendLine("     , Warehouse.Amount                                                                           ")
        sb.AppendLine("  FROM Warehouse inner join WarehouseMaster on WarehouseMaster.WareCode = Warehouse.WareCode      ")
        sb.AppendLine(" inner join Customer on WarehouseMaster.CusCode = Customer.CusCode                                ")
        sb.AppendLine("    where WarehouseMaster.WareCode = @WareCode order by Warehouse.ID                              ")
        Return sb.ToString()
    End Function

    Public Shared Sub CreateShortCut()
        Dim WshShell As WshShellClass = New WshShellClass()
        Dim MyShortcut As IWshRuntimeLibrary.IWshShortcut
        ' The shortcut will be created on the desktop
        Dim DesktopFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        MyShortcut = CType(WshShell.CreateShortcut(DesktopFolder & "\Product Management Atomy.lnk"), IWshRuntimeLibrary.IWshShortcut)
        MyShortcut.TargetPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Product Management Atomy")
        MyShortcut.Save()
    End Sub
End Class

Public Class Helper
    Const ERROR_SHARING_VIOLATION As Integer = 32
    Const ERROR_LOCK_VIOLATION As Integer = 33

    Public Shared Function IsFileLocked(ByVal exception As Exception) As Boolean
        Dim errorCode As Integer = System.Runtime.InteropServices.Marshal.GetHRForException(exception) And ((1 << 16) - 1)
        Return errorCode = ERROR_SHARING_VIOLATION OrElse errorCode = ERROR_LOCK_VIOLATION
    End Function

    Public Shared Function CanReadFile(ByVal filePath As String) As Boolean
        Try

            Using fileStream As FileStream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
                If fileStream IsNot Nothing Then fileStream.Close()
            End Using

        Catch ex As IOException

            If IsFileLocked(ex) Then
                Return False
            End If

        Finally
        End Try

        Return True
    End Function
End Class
Public Class MoneyHelper

    Public Shared Function ReadMoney(amount As Decimal) As String
        Dim res As String = ""
        If amount > 1000000000 Then
            Return res
        End If
        Dim m = amount Mod 1000000
        Dim r = amount - 1000000 * m
        Dim c = If(m = 0, "", ReadSegment(m))
        Dim d = ReadSegment(r)
        res = If(c = "", "", c + " ") + d
        Return res

    End Function
    Private Shared liM1 As New List(Of Decimal) From {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}
    Private Shared liN1 As New List(Of String) From {"không", "một", "hai", "ba", "bốn", "năm", "sáu", "bẩy", "tám", "chín"}
    Private Shared liC1 As New List(Of String) From {"mười", "một trăm", "một nghìn", "một triệu"}

    Private Shared Function ReadSegment(amount As Decimal, Optional isStart As Boolean = True)
        Dim res As String = ""
        'hai trăm bốn mươi triệu không trăm hai lăm nghìn
        'hai trăm bốn mươi triệu không trăm linh năm nghìn
        If amount < 10 Then
            If isStart Then
                Return liN1(amount)
            Else
                Return "linh" + liN1(amount)
            End If
        ElseIf amount = 10 Then
            Return liN1(1)
        End If

        If amount < 100 Then
            Dim m = amount Mod 10
            Dim r = amount - 10 * m
            Dim c = ReadSegment(m)
            Dim d = ReadSegment(r)
            Return c + " " + d
        ElseIf amount = 1000 Then
            Return liC1(2)
        End If

        If amount < 1000 Then
            Dim m = amount Mod 100
            Dim r = amount - 100 * m
            Dim h = ReadSegment(m) + " trăm"
            Dim d = ReadSegment(r)
            Return h + " " + d
        ElseIf amount = 1000 Then
            Return liC1(3)
        End If

        If amount < 1000000 Then
            Dim m = amount Mod 1000
            Dim r = amount - 1000 * m
            Dim h = ReadSegment(m)
            Dim d = ReadSegment(r) + " nghìn"
            Return h + " " + d
        ElseIf amount = 1000000 Then
            Return liC1(4)
        End If

        Return res
    End Function


    Public Shared Function BSearch(ByVal arr() As Decimal, ByVal target As Decimal) As Decimal
        Dim min As Decimal = 0
        Dim max As Decimal = arr.Length - 1
        Dim mid As Decimal = 0

        While min <= max
            mid = (min + max) / 2

            If arr(mid) = target Then
                Return mid
            ElseIf arr(mid) < target Then
                min = mid + 1
            Else
                max = mid - 1
            End If
        End While

        Return -1
    End Function
End Class
