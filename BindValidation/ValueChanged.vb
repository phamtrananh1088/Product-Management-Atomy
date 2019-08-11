'Imports System.Windows
'Imports System.Windows.Controls

'Class SurroundingClass
'    Public Shared ReadOnly ValueProperty As DependencyProperty = DependencyProperty.Register("Value", GetType(String), GetType(TextBox), New FrameworkPropertyMetadata("", New PropertyChangedCallback(AddressOf OnValueChanged), New CoerceValueCallback(AddressOf CoerceValue)))

'    Private Shared Function CoerceValue(ByVal element As DependencyObject, ByVal value As Object) As Object
'        Return value
'    End Function

'    Private Shared Sub OnValueChanged(ByVal obj As DependencyObject, ByVal args As DependencyPropertyChangedEventArgs)
'        Dim control As TextBox = CType(obj, TextBox)
'        Dim e As RoutedPropertyChangedEventArgs(Of String) = New RoutedPropertyChangedEventArgs(Of String)(CStr(args.OldValue), CStr(args.NewValue), ValueChangedEvent)
'        control.OnValueChanged(e)
'    End Sub

'    Public Shared ReadOnly ValueChangedEvent As RoutedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, GetType(RoutedPropertyChangedEventHandler(Of String)), GetType(TextBox))

'    Public Custom Event ValueChanged As RoutedPropertyChangedEventHandler(Of String)
'        AddHandler(ByVal value As RoutedPropertyChangedEventHandler(Of String))
'            AddHandler(ValueChangedEvent, value)
'        End AddHandler
'        RemoveHandler(ByVal value As RoutedPropertyChangedEventHandler(Of String))
'            RemoveHandler ValueChangedEvent, AddressOf OnValueChanged
'        End RemoveHandler
'    End Event

'    Protected Overridable Sub OnValueChanged(ByVal args As RoutedPropertyChangedEventArgs(Of String))
'        RaiseEvent ValueChanged(args)
'    End Sub
'End Class