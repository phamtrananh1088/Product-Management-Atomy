Public Class PageTitle
    Public Shared ReadOnly LabelContentProperty As DependencyProperty =
          DependencyProperty.Register("LabelContent", GetType(String), GetType(PageTitle))

    Public Property LabelContent As String
        Get
            Return GetValue(LabelContentProperty).ToString()
        End Get
        Set(value As String)
            SetValue(LabelContentProperty, value)
        End Set
    End Property
    Public Property Title As String

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.DataContext = Me
    End Sub
 
End Class
