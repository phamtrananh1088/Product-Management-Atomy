Imports System.Threading.Tasks
Imports System.Data.OleDb
Imports System.Text

Public Class ErrorLog
    Private Shared _ILogging As New DbLogging
    Public Shared Property ILogging As ILogging
        Get
            Return _ILogging
        End Get
        Set(value As ILogging)
            _ILogging = value
        End Set
    End Property

    Public Shared Sub SetError(window As Window, errorText As String, ex As Exception)
        Dim screenName As String = IIf(window.Name = "", window.ToString, window.Name)
        Dim i = LogError(screenName, errorText, ex)
        Task.WhenAny(i)
    End Sub
    Public Shared Sub SetError(page As Page, errorText As String, ex As Exception)
        Dim screenName As String = IIf(page.Name = "", page.ToString, page.Name)
        Dim i = LogError(screenName, errorText, ex)
        Task.WhenAny(i)
    End Sub
    Public Shared Sub SetError(windowName As String, errorText As String, ex As Exception)
        Dim i = LogError(windowName, errorText, ex)
        Task.WhenAny(i)
    End Sub
    Friend Shared Function LogError(windowName As String, errorText As String, ex As Exception) As Task
        Return Task.Run(Function()
                            Return _ILogging.LogError(windowName:=windowName, errorText:=errorText, ex:=ex)
                        End Function)
    End Function
End Class

Public Interface ILogging
    Function LogError(windowName As String, errorText As String, ex As Exception) As Integer
End Interface

Friend Class DbLogging
    Implements ILogging

    Public Function LogError(windowName As String, errorText As String, ex As Exception) As Integer Implements ILogging.LogError
        Dim sb As New StringBuilder()
        sb.AppendLine("INSERT INTO [Error Log]              ")
        sb.AppendLine("          ( [Title]                  ")
        sb.AppendLine("          , [Message]                ")
        sb.AppendLine("          , [Source]                 ")
        sb.AppendLine("          , [StackTrace]             ")
        sb.AppendLine("          , [InnerException]         ")
        sb.AppendLine("          , [Window]                 ")
        sb.AppendLine("          , [Create Date]            ")
        sb.AppendLine("          , [Create Time]            ")
        sb.AppendLine("          , [Create User]            ")
        sb.AppendLine("          )                          ")
        sb.AppendLine("     VALUES                          ")
        sb.AppendLine("          ( ?                        ")
        sb.AppendLine("          , ?                        ")
        sb.AppendLine("          , ?                        ")
        sb.AppendLine("          , ?                        ")
        sb.AppendLine("          , ?                        ")
        sb.AppendLine("          , ?                        ")
        sb.AppendLine("          , ?                        ")
        sb.AppendLine("          , ?                        ")
        sb.AppendLine("          , ?                        ")
        sb.AppendLine("          )                          ")

        Dim sSQL As String = sb.ToString()
        Dim dbConn As New DbConnect()
        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim cmd As New OleDbCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            cmd.Parameters.Add("@Title", OleDbType.VarChar).Value = errorText
            cmd.Parameters.Add("@Message", OleDbType.VarChar).Value = ex.Message
            cmd.Parameters.Add("@Source", OleDbType.VarChar).Value = ex.Source
            cmd.Parameters.Add("@StackTrace", OleDbType.VarChar).Value = ex.StackTrace
            If ex.InnerException Is Nothing Then
                cmd.Parameters.Add("@InnerException", OleDbType.VarChar).Value = ""
            Else
                cmd.Parameters.Add("@InnerException", OleDbType.VarChar).Value = ex.InnerException.Message
            End If
            cmd.Parameters.Add("@Window", OleDbType.VarChar).Value = windowName
            Dim d As Date = Date.Now
            cmd.Parameters.Add("@CreateDate", OleDbType.VarChar).Value = d.ToString("yyyy/MM/dd")
            cmd.Parameters.Add("@CreateTime", OleDbType.VarChar).Value = d.ToString("HH:mm:ss")
            cmd.Parameters.Add("@CreateUser", OleDbType.VarChar).Value = Utility.LoginUserCode
            LogError = cmd.ExecuteNonQuery()
            dbConn.CommitTran()
        Catch oleDbEx As OleDbException
            dbConn.RollbackTran()
            Console.WriteLine(oleDbEx.ToString())
            LogError = -1
        Catch pEx As Exception
            dbConn.RollbackTran()
            Console.WriteLine(pEx.ToString())
            LogError = -2
        Finally
            dbConn.DisposeTran()
            dbConn.Close()
        End Try
    End Function
End Class
