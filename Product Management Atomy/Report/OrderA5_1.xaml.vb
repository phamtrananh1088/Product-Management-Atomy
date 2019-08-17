Imports unvell.ReoGrid

Public Class OrderA5_1

    Private _path As String
    Public Sub New(path As String)
        _path = path
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Initialize()
    End Sub

    Private Sub Initialize()
        grid.Load(_path, IO.FileFormat.Excel2007)

    End Sub

End Class