Imports System.Threading.Tasks
Imports System.Data.SqlClient
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
        If String.IsNullOrEmpty(windowName) Then
            windowName = Utility.AppCaption
        End If

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
        sb.AppendLine("INSERT INTO [ErrorLog]                                                                                                           ")
        sb.AppendLine("          ( [Title], [Message], [Source], [StackTrace], [InnerException], [Window], [CreateDate], [CreateTime], [CreateUser])    ")
        sb.AppendLine("     VALUES                                                                                                                      ")
        sb.AppendLine("          ( @Title, @Message, @Source, @StackTrace, @InnerException, @Window, @CreateDate, @CreateTime, @CreateUser)             ")
     
        Dim sSQL As String = sb.ToString()
        Dim dbConn As New DbConnect()
        Try
            dbConn.Open()
            dbConn.BeginTran()
            Dim cmd As New SqlCommand(sSQL, dbConn.Conn)
            cmd.Transaction = dbConn.Tran
            cmd.Parameters.AddWithValue("@Title", errorText)
            cmd.Parameters.AddWithValue("@Message", ex.Message)
            cmd.Parameters.AddWithValue("@Source", ex.Source)
            cmd.Parameters.AddWithValue("@StackTrace", ex.StackTrace)
            If ex.InnerException Is Nothing Then
                cmd.Parameters.AddWithValue("@InnerException", "")
            Else
                cmd.Parameters.AddWithValue("@InnerException", ex.InnerException.Message)
            End If
            cmd.Parameters.AddWithValue("@Window", windowName)
            Dim d As Date = Date.Now
            cmd.Parameters.AddWithValue("@CreateDate", d.ToString("yyyy/MM/dd"))
            cmd.Parameters.AddWithValue("@CreateTime", d.ToString("HH:mm:ss"))
            cmd.Parameters.AddWithValue("@CreateUser", Utility.LoginUserCode)
            LogError = cmd.ExecuteNonQuery()
            dbConn.CommitTran()
        Catch oleDbEx As SqlException
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
