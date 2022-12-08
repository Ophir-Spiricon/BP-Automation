<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.initServer_Button = New System.Windows.Forms.Button()
        Me.beamWidthX_TextBox = New System.Windows.Forms.TextBox()
        Me.beamWidthX_Label = New System.Windows.Forms.Label()
        Me.beamWidthY_Label = New System.Windows.Forms.Label()
        Me.beamWidthY_TextBox = New System.Windows.Forms.TextBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.status_ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.startDAQ_Button = New System.Windows.Forms.Button()
        Me.profileChart = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.profileChart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'initServer_Button
        '
        Me.initServer_Button.Location = New System.Drawing.Point(13, 13)
        Me.initServer_Button.Name = "initServer_Button"
        Me.initServer_Button.Size = New System.Drawing.Size(75, 23)
        Me.initServer_Button.TabIndex = 0
        Me.initServer_Button.Text = "Start Server"
        Me.initServer_Button.UseVisualStyleBackColor = True
        '
        'beamWidthX_TextBox
        '
        Me.beamWidthX_TextBox.Location = New System.Drawing.Point(136, 42)
        Me.beamWidthX_TextBox.Name = "beamWidthX_TextBox"
        Me.beamWidthX_TextBox.Size = New System.Drawing.Size(100, 20)
        Me.beamWidthX_TextBox.TabIndex = 1
        '
        'beamWidthX_Label
        '
        Me.beamWidthX_Label.AutoSize = True
        Me.beamWidthX_Label.Location = New System.Drawing.Point(136, 22)
        Me.beamWidthX_Label.Name = "beamWidthX_Label"
        Me.beamWidthX_Label.Size = New System.Drawing.Size(75, 13)
        Me.beamWidthX_Label.TabIndex = 2
        Me.beamWidthX_Label.Text = "Beam Width X"
        '
        'beamWidthY_Label
        '
        Me.beamWidthY_Label.AutoSize = True
        Me.beamWidthY_Label.Location = New System.Drawing.Point(242, 22)
        Me.beamWidthY_Label.Name = "beamWidthY_Label"
        Me.beamWidthY_Label.Size = New System.Drawing.Size(75, 13)
        Me.beamWidthY_Label.TabIndex = 4
        Me.beamWidthY_Label.Text = "Beam Width Y"
        '
        'beamWidthY_TextBox
        '
        Me.beamWidthY_TextBox.Location = New System.Drawing.Point(242, 42)
        Me.beamWidthY_TextBox.Name = "beamWidthY_TextBox"
        Me.beamWidthY_TextBox.Size = New System.Drawing.Size(100, 20)
        Me.beamWidthY_TextBox.TabIndex = 3
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.status_ToolStripStatusLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 496)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(729, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'status_ToolStripStatusLabel
        '
        Me.status_ToolStripStatusLabel.Name = "status_ToolStripStatusLabel"
        Me.status_ToolStripStatusLabel.Size = New System.Drawing.Size(121, 17)
        Me.status_ToolStripStatusLabel.Text = "ToolStripStatusLabel1"
        '
        'startDAQ_Button
        '
        Me.startDAQ_Button.Location = New System.Drawing.Point(13, 65)
        Me.startDAQ_Button.Name = "startDAQ_Button"
        Me.startDAQ_Button.Size = New System.Drawing.Size(75, 23)
        Me.startDAQ_Button.TabIndex = 6
        Me.startDAQ_Button.Text = "Start DAQ"
        Me.startDAQ_Button.UseVisualStyleBackColor = True
        '
        'profileChart
        '
        ChartArea1.Name = "ChartArea1"
        Me.profileChart.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.profileChart.Legends.Add(Legend1)
        Me.profileChart.Location = New System.Drawing.Point(136, 68)
        Me.profileChart.Name = "profileChart"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series1.Legend = "Legend1"
        Series1.Name = "Profile X"
        Me.profileChart.Series.Add(Series1)
        Me.profileChart.Size = New System.Drawing.Size(581, 416)
        Me.profileChart.TabIndex = 7
        Me.profileChart.Text = "Profile X"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(729, 518)
        Me.Controls.Add(Me.profileChart)
        Me.Controls.Add(Me.startDAQ_Button)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.beamWidthY_Label)
        Me.Controls.Add(Me.beamWidthY_TextBox)
        Me.Controls.Add(Me.beamWidthX_Label)
        Me.Controls.Add(Me.beamWidthX_TextBox)
        Me.Controls.Add(Me.initServer_Button)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.profileChart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents initServer_Button As System.Windows.Forms.Button
    Friend WithEvents beamWidthX_TextBox As System.Windows.Forms.TextBox
    Friend WithEvents beamWidthX_Label As System.Windows.Forms.Label
    Friend WithEvents beamWidthY_Label As System.Windows.Forms.Label
    Friend WithEvents beamWidthY_TextBox As System.Windows.Forms.TextBox
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents status_ToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents startDAQ_Button As System.Windows.Forms.Button
    Friend WithEvents profileChart As System.Windows.Forms.DataVisualization.Charting.Chart

End Class
