Imports System.Data.OleDb

Public Class Property1
    Private _AtomyDataSet As AtomyDataSet.PropertyDataTable
    Public Property AtomyDataSet As AtomyDataSet.PropertyDataTable
        Get
            Return _AtomyDataSet
        End Get
        Set(value As AtomyDataSet.PropertyDataTable)

        End Set
    End Property

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        LoadData()
    End Sub
#Region "LoadData"
    Private Sub LoadData()
        Dim dbConn As New DbConnect

        Try
            dbConn.Open()
            Dim sSQL As String = "select * from "
            Dim cmd As New OleDbCommand(sSQL, dbConn.Conn)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            ErrorLog.SetError(Me, "Property: Load data error", ex)
        Finally
            dbConn.Close()
        End Try

    End Sub
#End Region

    Private Sub form_load(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

    End Sub
    Private Sub btnUpdate_Click(sender As Object, e As RoutedEventArgs) Handles btnUpdate.Click

    End Sub

    Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs) Handles btnDelete.Click

    End Sub

    Private Sub btnInsert_Click(sender As Object, e As RoutedEventArgs) Handles btnInsert.Click

    End Sub



End Class
