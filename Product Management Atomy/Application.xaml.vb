Class Application

    Public UserInfo As UserInfo
    Public CompanyInfo As CompanyInfo
    Public AppCaption As String
    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.
    Protected Overrides Sub OnStartup(e As StartupEventArgs)
        MyBase.OnStartup(e)
        SetupExceptionHandling()
    End Sub
    Private Sub SetupExceptionHandling()
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf LogUnhandledException

        AddHandler DispatcherUnhandledException, AddressOf LogDispatcherUnhandledException

        AddHandler System.Threading.Tasks.TaskScheduler.UnobservedTaskException, AddressOf LogUnobservedTaskException

    End Sub
    Private Sub LogUnhandledException(sender As Object, e As UnhandledExceptionEventArgs)
        ErrorLog.SetError(Me.MainWindow, "AppDomain.CurrentDomain.UnhandledException", DirectCast(e.ExceptionObject, Exception))
    End Sub

    Private Sub LogDispatcherUnhandledException(sender As Object, e As System.Windows.Threading.DispatcherUnhandledExceptionEventArgs)
        ErrorLog.SetError(Me.MainWindow, "Application.Current.DispatcherUnhandledException", e.Exception)
    End Sub

    Private Sub LogUnobservedTaskException(sender As Object, e As System.Threading.Tasks.UnobservedTaskExceptionEventArgs)
        ErrorLog.SetError(Me.MainWindow, "TaskScheduler.UnobservedTaskException", e.Exception)
    End Sub

    Private Sub app_Startup(sender As Object, e As StartupEventArgs)
        UserInfo = New UserInfo() With {.UserCd = "999999", .UserNm = "Trần Vũ Lan Anh"}
        CompanyInfo = New CompanyInfo() With {.CompanyCd = "001000", .CompanyNm = "Công ty TNHH Atomy Việt Nam"}
        AppCaption = "Atomy"
    End Sub

    Private Sub Application_Exit(sender As Object, e As ExitEventArgs)
        MySettings.Default.Save()
    End Sub
End Class
