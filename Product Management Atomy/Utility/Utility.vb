Imports System.Data.SqlClient
Imports System.Threading.Tasks
Imports System.Data

Public Class Utility
#Region "Global"
    Public Shared Function GetUserInfo() As UserInfo
        Return DirectCast(Application.Current, Anh.Pmt.Atomy.Application).UserInfo
    End Function
    Public Shared Function GetCompanyInfo() As CompanyInfo
        Return DirectCast(Application.Current, Anh.Pmt.Atomy.Application).CompanyInfo
    End Function

    Public Shared Function LoginUserCode() As String
        Return DirectCast(Application.Current, Anh.Pmt.Atomy.Application).UserInfo.UserCd
    End Function
    Public Shared Function LoginUserName() As String
        Return DirectCast(Application.Current, Anh.Pmt.Atomy.Application).UserInfo.UserNm
    End Function

    Public Shared Function CompanyCode() As String
        Return DirectCast(Application.Current, Anh.Pmt.Atomy.Application).CompanyInfo.CompanyCd
    End Function

    Public Shared Function CompanyName() As String
        Return DirectCast(Application.Current, Anh.Pmt.Atomy.Application).CompanyInfo.CompanyNm
    End Function
    Public Shared Function AppCaption() As String
        Return DirectCast(Application.Current, Anh.Pmt.Atomy.Application).AppCaption
    End Function
#End Region

    Public Structure DefaultData
        Public Shared Function DefaultManufacturer() As Object
            Return "Atomy.Co.,Ld (Korea)"
        End Function
        Public Shared Function DefaultLocation() As Object
            Return "Korea"
        End Function
    End Structure

    Public Structure RowInit
        Shared Sub InitCustomerRow(newRow As AtomyDataSet.CustomerRow)
            newRow.CusCode = ""
            newRow.Company = ""
            newRow.LastName = ""
            newRow.FirstName = ""
            newRow.EmailAddress = ""
            newRow.JobTitle = ""
            newRow.BusinessPhone = ""
            newRow.HomePhone = ""
            newRow.MobilePhone = ""
            newRow.FaxNumber = ""
            newRow.Address = ""
            newRow.City = ""
            newRow.StateProvince = ""
            newRow.ZIPPostalCode = ""
            newRow.CountryRegion = ""
            newRow.WebPage = ""
            newRow.FacebookID = ""
            newRow.Notes = ""
            newRow.Retired = False
            newRow.RetiredDate = ""
        End Sub

        Shared Sub InitPropertyRow(newRow As AtomyDataSet.PropertyRow)
            newRow.PropCode = ""
            newRow.PropName = ""
            newRow.Description = ""
            newRow.Category = ""
            newRow.Condition = ""
            newRow.AcquiredDate = ""
            newRow.Unit = ""
            newRow.PurchasePrice = 0
            newRow.SalesPrice = 0
            newRow.CurrentValue = 0
            newRow.Location = ""
            newRow.Manufacturer = "Atomy.Co.,Ltd (Korea)"
            newRow.Model = ""
            newRow.Comments = ""
            newRow.Retired = False
            newRow.RetiredDate = ""
        End Sub

        Shared Sub InitEmployeeRow(newRow As AtomyDataSet.EmployeeRow)
            newRow.EmpCode = ""
            newRow.LastName = ""
            newRow.FirstName = ""
            newRow.Department = ""
            newRow.Position = ""
            newRow.EmailAddress = ""
            newRow.BusinessPhone = ""
            newRow.HomePhone = ""
            newRow.MobilePhone = ""
            newRow.FaxNumber = ""
            newRow.Address = ""
            newRow.City = ""
            newRow.StateProvince = ""
            newRow.ZIPPostalCode = ""
            newRow.CountryRegion = ""
            newRow.FacebookID = ""
            newRow.Notes = ""
            newRow.Retired = False
            newRow.RetiredDate = ""
        End Sub

        Shared Sub InitWarehouseMasterRow(newRow As AtomyDataSet.WarehouseMasterRow)
            newRow.WareCode = ""
            newRow.Type = 0
            newRow.WareDate = ""
            newRow.EmpCode = ""
            newRow.EmpName = ""
            newRow.CusCode = ""
            newRow.CusName = ""
            newRow.Status = 0
            newRow.WareTitle = ""
            newRow.Description = ""
            newRow.TotalAmount = 0
            newRow.Discount = 0
            newRow.SalesAmount = 0
            newRow.PaymentType = 0
            newRow.FinishFlag = 0
            newRow.PaymentDate = ""
            newRow.FinishDate = ""
            newRow.Comments = ""
            newRow.UpdateCount = 1
            newRow.Retired = False
            newRow.RetiredDate = ""
        End Sub

     

    End Structure
#Region "HelpCreateCode"
    Public Shared Function HelpCreateCode(tableName As String) As String
        Dim i = TaskHelpCreateCode(tableName)
        Return Task.WhenAny(i).Result.Result
    End Function
    Friend Shared Function TaskHelpCreateCode(tableName As String) As Task(Of String)
        Return Task.Run(Function()
                            Dim res As String = ""
                            Dim sSQL As String
                            Select Case tableName
                                Case "Property"
                                    sSQL = "select [PropCode] from [Property] order by [PropCode]"
                                Case "Customer"
                                    sSQL = "select [CusCode] from [Customer] order by [CusCode]"
                                Case "Employee"
                                    sSQL = "select [EmpCode] from [Employee] order by [EmpCode]"
                                Case "Warehouse"
                                    sSQL = "select [WareCode] from [WarehouseMaster] order by [WareCode]"
                                Case Else
                                    sSQL = ""
                            End Select
                            Dim dbConn As New DbConnect
                            Dim dataset As New DataSet
                            Try
                                dbConn.Open()
                                Dim adapt As New SqlDataAdapter(sSQL, dbConn.Conn)
                                Dim count As Integer = adapt.Fill(dataset)
                                If count > 0 Then
                                    Dim array(count) As String
                                    Dim arrayI(count) As Integer
                                    Dim f As Integer
                                    For index = 0 To count - 1
                                        array(index) = dataset.Tables(0).Rows(index)(0).ToString
                                        If Integer.TryParse(array(index).TrimStart("0"), f) Then
                                            arrayI(index) = f
                                        Else
                                            arrayI(index) = 0
                                        End If
                                    Next
                                    Dim max As Integer = arrayI(count - 1)
                                    Dim min As Integer = arrayI(0)
                                    Dim find As Integer
                                    If max - min + 1 > count Then
                                        For index = min To max
                                            find = BinarySearch.BSearch(arrayI, index)
                                            If find = -1 Then
                                                Return New String("0", IIf(8 > index.ToString.Length, 8 - index.ToString.Length, 0)) + index.ToString
                                            End If
                                        Next
                                    Else
                                        Dim n As String = (max + 1).ToString
                                        Return New String("0", IIf(8 > n.Length, 8 - n.Length, 0)) + n
                                    End If

                                Else
                                    Return "00000001"
                                End If
                            Catch ex As Exception
                                ErrorLog.SetError(Utility.Name, "Đã sảy ra lỗi khi lấy mã cho bản ghi thêm mới.", ex)
                            Finally
                                dbConn.Close()
                            End Try
                            Return res
                        End Function)
    End Function
#End Region

#Region "HelpGetLastCode"
    Public Shared Function HelpGetLastCode(tableName As String, Optional type As Integer = 1) As String
        Dim i = TaskHelpGetLastCode(tableName, type)
        Return Task.WhenAny(i).Result.Result
    End Function
    Friend Shared Function TaskHelpGetLastCode(tableName As String, Optional type As Integer = 1) As Task(Of String)
        Return Task.Run(Function()
                            Dim res As String = ""
                            Dim sSQL As String
                            Select Case tableName
                                Case "Property"
                                    sSQL = "select TOP 1 [PropCode] from [Property] where [Retired] = 0 order by [PropCode] DESC"
                                Case "Customer"
                                    sSQL = "select TOP 1 [CusCode] from [Customer] where [Retired] = 0 order by [CusCode] DESC"
                                Case "Employee"
                                    sSQL = "select TOP 1 [EmpCode] from [Employee] where [Retired] = 0 order by [EmpCode] DESC"
                                Case "Warehouse"
                                    sSQL = "select TOP 1 [WareCode] from [WarehouseMaster] where [Retired] = 0 and [Type] = @Type order by [WareCode] DESC"
                                Case Else
                                    sSQL = ""
                            End Select
                            Dim dbConn As New DbConnect
                            Dim dataset As New DataSet
                            Try
                                dbConn.Open()
                                Dim cmd As New SqlCommand(sSQL, dbConn.Conn)
                                If tableName = "Warehouse" Then
                                    cmd.Parameters.AddWithValue("@Type", type)
                                End If

                                Dim read As SqlDataReader = cmd.ExecuteReader()
                                If read.Read() Then
                                    Return read(0).ToString
                                Else
                                    Return ""
                                End If
                            Catch ex As Exception
                                ErrorLog.SetError(Utility.Name, "Đã sảy ra lỗi khi lấy mã cho bản ghi thêm mới.", ex)
                            Finally
                                dbConn.Close()
                            End Try
                            Return res
                        End Function)
    End Function
#End Region
    
    Private Shared Property Name = "Utility"
End Class

Public Class BinarySearch
    Public Shared Function BSearch(ByVal arr() As Integer, ByVal target As Integer) As Integer
        Dim min As Integer = 0
        Dim max As Integer = arr.Length - 1
        Dim mid As Integer = 0

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
