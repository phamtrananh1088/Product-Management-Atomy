Option Explicit On
Option Infer On

Imports System.Runtime.CompilerServices
Imports System.Windows.Controls.Primitives

Module DataGridExtensions

    ''' <summary>
    ''' Get DataGridRow using index
    ''' </summary>
    <Extension()>
    Function GetRow(ByVal grid As DataGrid, ByVal index As Integer) As DataGridRow

        Dim row As DataGridRow = DirectCast(grid.ItemContainerGenerator.ContainerFromIndex(index), DataGridRow)
        If row Is Nothing Then
            grid.UpdateLayout()
            grid.ScrollIntoView(grid.Items(index))
            row = DirectCast(grid.ItemContainerGenerator.ContainerFromIndex(index), DataGridRow)
        End If

        Return row
    End Function

    ''' <summary>
    ''' Return Selected Item
    ''' </summary>
    <Extension()>
    Function GetSelectedRow(ByVal grid As DataGrid) As DataGridRow
        Return DirectCast(grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem), DataGridRow)
    End Function

    ''' <summary>
    ''' pass datagridrow and column index
    ''' </summary>
    <Extension()>
    Public Function GetCell(grid As DataGrid, row As DataGridRow, column As Integer) As DataGridCell
        If row IsNot Nothing Then
            Dim presenter As DataGridCellsPresenter = GetVisualChild(Of DataGridCellsPresenter)(row)

            If presenter Is Nothing Then
                grid.ScrollIntoView(row, grid.Columns(column))
                presenter = GetVisualChild(Of DataGridCellsPresenter)(row)
            End If

            Dim cell As DataGridCell = DirectCast(presenter.ItemContainerGenerator.ContainerFromIndex(column), DataGridCell)
            Return cell
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Pass row and column index
    ''' </summary>
    <Extension()>
    Public Function GetCell(grid As DataGrid, row As Integer, column As Integer) As DataGridCell
        Dim rowContainer As DataGridRow = grid.GetRow(row)
        Return grid.GetCell(rowContainer, column)
    End Function

    ''' <summary>
    ''' Return DataGrid Rows 
    ''' </summary>
    <Extension()>
    Public Function GetDataGridRows(grid As DataGrid) As List(Of DataGridRow)
        Dim rows As New List(Of DataGridRow)
        Dim itemsSource = TryCast(grid.ItemsSource, IEnumerable)

        If itemsSource Is Nothing Then
            Return Nothing
        End If

        For Each item In itemsSource
            Dim row = TryCast(grid.ItemContainerGenerator.ContainerFromItem(item), DataGridRow)

            If row Is Nothing Then

                'bring into view and get row
                grid.UpdateLayout()
                grid.ScrollIntoView(item)
                row = TryCast(grid.ItemContainerGenerator.ContainerFromItem(item), DataGridRow)

            End If

            If row IsNot Nothing Then
                rows.Add(row)
            End If
        Next

        Return rows

    End Function

    ''' <summary>
    ''' Set selection based on indexes
    ''' </summary>
    <Extension()>
    Public Sub SelectDataGridRowByIndexes(dataGrid As DataGrid, ByVal ParamArray rowIndexes As Integer())
        If Not dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow) Then
            Throw New ArgumentException("Change selection unit of the DataGrid to FullRow.")
        End If

        If Not dataGrid.SelectionMode.Equals(DataGridSelectionMode.Extended) Then
            Throw New ArgumentException("Change selectionMode of the DataGrid to Extended.")
        End If

        If rowIndexes.Length.Equals(0) OrElse rowIndexes.Length > dataGrid.Items.Count Then
            Throw New ArgumentException("Invalid number of indexes.")
        End If

        dataGrid.SelectedItems.Clear()
        dataGrid.UpdateLayout()

        For Each rowIndex As Integer In rowIndexes
        
            If rowIndex < 0 OrElse rowIndex > (dataGrid.Items.Count - 1) Then
                Throw New ArgumentException(String.Format("{0} is an invalid row index.", rowIndex))
            End If

            Dim item As Object = dataGrid.Items(rowIndex)

            Try
                dataGrid.SelectedItems.Add(item)
            Catch ex As Exception

            End Try

            Dim row As DataGridRow = TryCast(dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex), DataGridRow)

            If row Is Nothing Then
                dataGrid.ScrollIntoView(item)
                row = TryCast(dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex), DataGridRow)
            End If

            If row IsNot Nothing Then
                Dim cell As DataGridCell = GetCell(dataGrid, row, 0)
                If cell IsNot Nothing Then
                    cell.Focus()
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Traverse Visual Tree 
    ''' </summary>
    <Extension()>
    Public Function GetVisualChild(Of T As Visual)(parent As Visual) As T
        Dim child As T = Nothing
        Dim numVisuals As Integer = VisualTreeHelper.GetChildrenCount(parent)

        For i As Integer = 0 To numVisuals - 1
            Dim v As Visual = DirectCast(VisualTreeHelper.GetChild(parent, i), Visual)
            child = TryCast(v, T)
            If child Is Nothing Then
                child = GetVisualChild(Of T)(v)
            End If

            If child IsNot Nothing Then
                Exit For
            End If
        Next

        Return child
    End Function

End Module

Module DataGridCellExtensions
    ''' <summary>
    ''' Get Control template
    ''' </summary>
    <Extension()>
    Function GetItem(Of T)(ByVal cell As DataGridCell, ByVal name As String) As T
        Dim cp As ContentPresenter = cell.Content
        Dim item As T = cp.ContentTemplate.FindName(name, cp)
        Return item
    End Function

    ''' <summary>
    ''' Get Control template
    ''' </summary>
    <Extension()>
    Sub SetTemplateLabelContent(ByVal cell As DataGridCell, ByVal name As String, value As Object)
        Dim itemName As Label = cell.GetItem(Of Label)(name)
        itemName.Content = value
    End Sub
End Module
