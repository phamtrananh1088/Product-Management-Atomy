Imports System.Data.SqlClient
Imports System.Data

Public Class DbConnect
    Private _connString As String = New MySettings().PMS_ATOMYConnectionString
    Private _sensitiveData As String = ";Password=net123aA@;"
    Private _Conn As SqlConnection
    Private _Tran As SqlTransaction
    Public Property Conn As SqlConnection

        Get
            Return _Conn
        End Get
        Set(ByVal value As SqlConnection)
            _Conn = value
        End Set
    End Property

    Public ReadOnly Property Tran As SqlTransaction
        Get
            Return _Tran
        End Get
    End Property
    Public ReadOnly Property State As ConnectionState
        Get
            If _Conn Is Nothing Then
                Return Nothing
            End If
            Return _Conn.State
        End Get
    End Property
    Public Sub New()

    End Sub

    Public Sub Open()
        If _Conn Is Nothing Then
            _Conn = New SqlConnection(_connString + _sensitiveData)
        End If
        If State = ConnectionState.Closed Then
            _Conn.Open()
        End If

    End Sub

    Public Sub BeginTran()
        _Tran = _Conn.BeginTransaction()
    End Sub

    Public Sub CommitTran()
        _Tran.Commit()
    End Sub

    Public Sub RollbackTran()
        _Tran.Rollback()
    End Sub

    Public Sub DisposeTran()
        If Not IsDBNull(_Tran) Then
            _Tran.Dispose()
        End If
    End Sub

    Public Sub Close()
        If Not IsDBNull(_Conn) Then
            _Conn.Close()
        End If

    End Sub
End Class
