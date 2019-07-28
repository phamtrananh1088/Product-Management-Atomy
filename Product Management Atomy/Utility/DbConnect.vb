Imports System.Data.OleDb
Imports System.Data

Public Class DbConnect
    Private _connString As String = New MySettings().AtomyConnectionString
    Private _sensitiveData As String = ";Jet OLEDB:Database Password=net123aA@"
    Private _Conn As OleDbConnection
    Private _Tran As OleDbTransaction
    Public Property Conn As OleDbConnection

        Get
            Return _Conn
        End Get
        Set(ByVal value As OleDbConnection)
            _Conn = value
        End Set
    End Property

    Public ReadOnly Property Tran As OleDbTransaction
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
            _Conn = New OleDbConnection(_connString + _sensitiveData)
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
