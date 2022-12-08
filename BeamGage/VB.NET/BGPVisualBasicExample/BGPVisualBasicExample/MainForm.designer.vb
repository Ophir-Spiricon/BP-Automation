<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button
        Me.Total = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Row1Col1 = New System.Windows.Forms.TextBox
        Me.Row1Col2 = New System.Windows.Forms.TextBox
        Me.Row2Col2 = New System.Windows.Forms.TextBox
        Me.Row2Col1 = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TimeStamp = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Start/Stop"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Total
        '
        Me.Total.Location = New System.Drawing.Point(203, 14)
        Me.Total.Name = "Total"
        Me.Total.ReadOnly = True
        Me.Total.Size = New System.Drawing.Size(109, 20)
        Me.Total.TabIndex = 1
        Me.Total.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(158, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(31, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Total"
        '
        'Row1Col1
        '
        Me.Row1Col1.Location = New System.Drawing.Point(156, 99)
        Me.Row1Col1.Name = "Row1Col1"
        Me.Row1Col1.Size = New System.Drawing.Size(77, 20)
        Me.Row1Col1.TabIndex = 3
        Me.Row1Col1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Row1Col2
        '
        Me.Row1Col2.Location = New System.Drawing.Point(239, 99)
        Me.Row1Col2.Name = "Row1Col2"
        Me.Row1Col2.Size = New System.Drawing.Size(73, 20)
        Me.Row1Col2.TabIndex = 4
        Me.Row1Col2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Row2Col2
        '
        Me.Row2Col2.Location = New System.Drawing.Point(239, 125)
        Me.Row2Col2.Name = "Row2Col2"
        Me.Row2Col2.Size = New System.Drawing.Size(73, 20)
        Me.Row2Col2.TabIndex = 6
        Me.Row2Col2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Row2Col1
        '
        Me.Row2Col1.Location = New System.Drawing.Point(156, 125)
        Me.Row2Col1.Name = "Row2Col1"
        Me.Row2Col1.Size = New System.Drawing.Size(77, 20)
        Me.Row2Col1.TabIndex = 5
        Me.Row2Col1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(21, 84)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Pixel values:"
        '
        'TimeStamp
        '
        Me.TimeStamp.AutoSize = True
        Me.TimeStamp.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.TimeStamp.Location = New System.Drawing.Point(200, 45)
        Me.TimeStamp.Name = "TimeStamp"
        Me.TimeStamp.Size = New System.Drawing.Size(60, 13)
        Me.TimeStamp.TabIndex = 8
        Me.TimeStamp.Text = "TimeStamp"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(134, 45)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "TimeStamp"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(12, 41)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "Ultracal"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 67)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(0, 13)
        Me.Label4.TabIndex = 11
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(100, 99)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(32, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Row:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(158, 70)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(45, 13)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Column:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(134, 102)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(13, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "1"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(134, 128)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(13, 13)
        Me.Label8.TabIndex = 15
        Me.Label8.Text = "2"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(181, 83)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(13, 13)
        Me.Label9.TabIndex = 16
        Me.Label9.Text = "1"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(265, 83)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(13, 13)
        Me.Label10.TabIndex = 17
        Me.Label10.Text = "2"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(334, 159)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TimeStamp)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Row2Col2)
        Me.Controls.Add(Me.Row2Col1)
        Me.Controls.Add(Me.Row1Col2)
        Me.Controls.Add(Me.Row1Col1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Total)
        Me.Controls.Add(Me.Button1)
        Me.Name = "MainForm"
        Me.Text = "BGP Visual Basic Example"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Total As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Row1Col1 As System.Windows.Forms.TextBox
    Friend WithEvents Row1Col2 As System.Windows.Forms.TextBox
    Friend WithEvents Row2Col2 As System.Windows.Forms.TextBox
    Friend WithEvents Row2Col1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TimeStamp As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label

End Class
