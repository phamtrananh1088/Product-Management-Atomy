Imports System.Windows
Imports System.Windows.Interactivity
Imports System.Windows.Controls

Public Class PutCursorAtEndTextBoxBehavior
    Inherits Behavior(Of UIElement)

    Private _textBox As TextBox

    Protected Overrides Sub OnAttached()
        MyBase.OnAttached()
        _textBox = TryCast(AssociatedObject, TextBox)

        If _textBox Is Nothing Then
            Return
        End If

        AddHandler _textBox.GotFocus, AddressOf TextBoxGotFocus
    End Sub

    Protected Overrides Sub OnDetaching()
        If _textBox Is Nothing Then
            Return
        End If

        RemoveHandler _textBox.GotFocus, AddressOf TextBoxGotFocus
        MyBase.OnDetaching()
    End Sub

    Private Sub TextBoxGotFocus(ByVal sender As Object, ByVal routedEventArgs As RoutedEventArgs)
        _textBox.CaretIndex = _textBox.Text.Length
    End Sub
End Class
