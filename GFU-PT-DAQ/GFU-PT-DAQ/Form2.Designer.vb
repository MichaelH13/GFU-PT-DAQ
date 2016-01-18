<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Me.components = New System.ComponentModel.Container()
        Dim ChartArea7 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend7 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series7 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Button2 = New System.Windows.Forms.Button()
        Me.xCoord = New System.Windows.Forms.Label()
        Me.yCoord = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pgbTestStatus = New System.Windows.Forms.ProgressBar()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Chart1
        '
        ChartArea7.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea7)
        Legend7.Name = "Legend1"
        Me.Chart1.Legends.Add(Legend7)
        Me.Chart1.Location = New System.Drawing.Point(1, -1)
        Me.Chart1.Name = "Chart1"
        Series7.ChartArea = "ChartArea1"
        Series7.Legend = "Legend1"
        Series7.Name = "Series1"
        Me.Chart1.Series.Add(Series7)
        Me.Chart1.Size = New System.Drawing.Size(1070, 555)
        Me.Chart1.TabIndex = 1
        Me.Chart1.Text = "Chart1"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(335, 604)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(136, 34)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Run"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'Timer2
        '
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(551, 604)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(136, 34)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "Save"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'xCoord
        '
        Me.xCoord.AutoSize = True
        Me.xCoord.Location = New System.Drawing.Point(934, 371)
        Me.xCoord.Name = "xCoord"
        Me.xCoord.Size = New System.Drawing.Size(51, 17)
        Me.xCoord.TabIndex = 4
        Me.xCoord.Text = "Label1"
        '
        'yCoord
        '
        Me.yCoord.AutoSize = True
        Me.yCoord.Location = New System.Drawing.Point(934, 407)
        Me.yCoord.Name = "yCoord"
        Me.yCoord.Size = New System.Drawing.Size(51, 17)
        Me.yCoord.TabIndex = 5
        Me.yCoord.Text = "Label2"
        '
        'pgbTestStatus
        '
        Me.pgbTestStatus.Location = New System.Drawing.Point(100, 531)
        Me.pgbTestStatus.Name = "pgbTestStatus"
        Me.pgbTestStatus.Size = New System.Drawing.Size(791, 23)
        Me.pgbTestStatus.TabIndex = 6
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1071, 739)
        Me.Controls.Add(Me.pgbTestStatus)
        Me.Controls.Add(Me.yCoord)
        Me.Controls.Add(Me.xCoord)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Chart1)
        Me.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Name = "Form2"
        Me.Text = "Sit-To-Stand 5000"
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents xCoord As System.Windows.Forms.Label
    Friend WithEvents yCoord As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents pgbTestStatus As System.Windows.Forms.ProgressBar
End Class
