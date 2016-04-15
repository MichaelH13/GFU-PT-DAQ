<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Dim ChartArea3 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend3 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series3 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.STSChart = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.btnRunTest = New System.Windows.Forms.Button()
        Me.clkSamplingRate = New System.Windows.Forms.Timer(Me.components)
        Me.btnSave = New System.Windows.Forms.Button()
        Me.chartSTSToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.pgbTestStatus = New System.Windows.Forms.ProgressBar()
        Me.btnCalibrateDevice = New System.Windows.Forms.Button()
        Me.btnCancelTest = New System.Windows.Forms.Button()
        Me.btnResetPickoff = New System.Windows.Forms.Button()
        Me.frmSaveTest = New System.Windows.Forms.SaveFileDialog()
        Me.btnViewVariables = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.xCoord = New System.Windows.Forms.Label()
        Me.yCoord = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        CType(Me.STSChart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'STSChart
        '
        ChartArea3.Name = "ChartArea1"
        Me.STSChart.ChartAreas.Add(ChartArea3)
        Legend3.Name = "Legend1"
        Me.STSChart.Legends.Add(Legend3)
        Me.STSChart.Location = New System.Drawing.Point(23, -1)
        Me.STSChart.Name = "STSChart"
        Series3.ChartArea = "ChartArea1"
        Series3.Legend = "Legend1"
        Series3.Name = "Series1"
        Me.STSChart.Series.Add(Series3)
        Me.STSChart.Size = New System.Drawing.Size(1055, 555)
        Me.STSChart.TabIndex = 1
        Me.STSChart.Text = "STS Chart"
        '
        'btnRunTest
        '
        Me.btnRunTest.Location = New System.Drawing.Point(479, 589)
        Me.btnRunTest.Name = "btnRunTest"
        Me.btnRunTest.Size = New System.Drawing.Size(136, 34)
        Me.btnRunTest.TabIndex = 2
        Me.btnRunTest.Text = "Run Test"
        Me.btnRunTest.UseVisualStyleBackColor = True
        '
        'clkSamplingRate
        '
        Me.clkSamplingRate.Interval = 10
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(695, 589)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(136, 34)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "Save Test"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'pgbTestStatus
        '
        Me.pgbTestStatus.Location = New System.Drawing.Point(100, 531)
        Me.pgbTestStatus.Name = "pgbTestStatus"
        Me.pgbTestStatus.Size = New System.Drawing.Size(791, 23)
        Me.pgbTestStatus.TabIndex = 6
        '
        'btnCalibrateDevice
        '
        Me.btnCalibrateDevice.Location = New System.Drawing.Point(259, 589)
        Me.btnCalibrateDevice.Name = "btnCalibrateDevice"
        Me.btnCalibrateDevice.Size = New System.Drawing.Size(136, 34)
        Me.btnCalibrateDevice.TabIndex = 7
        Me.btnCalibrateDevice.Text = "Calibrate Chair"
        Me.btnCalibrateDevice.UseVisualStyleBackColor = True
        '
        'btnCancelTest
        '
        Me.btnCancelTest.Location = New System.Drawing.Point(479, 654)
        Me.btnCancelTest.Name = "btnCancelTest"
        Me.btnCancelTest.Size = New System.Drawing.Size(136, 34)
        Me.btnCancelTest.TabIndex = 8
        Me.btnCancelTest.Text = "Cancel Test"
        Me.btnCancelTest.UseVisualStyleBackColor = True
        '
        'btnResetPickoff
        '
        Me.btnResetPickoff.Location = New System.Drawing.Point(695, 654)
        Me.btnResetPickoff.Name = "btnResetPickoff"
        Me.btnResetPickoff.Size = New System.Drawing.Size(136, 34)
        Me.btnResetPickoff.TabIndex = 9
        Me.btnResetPickoff.Text = "Restore Last Test"
        Me.btnResetPickoff.UseVisualStyleBackColor = True
        '
        'frmSaveTest
        '
        Me.frmSaveTest.DefaultExt = "csv"
        Me.frmSaveTest.FileName = "sts_raw_data"
        '
        'btnViewVariables
        '
        Me.btnViewVariables.Location = New System.Drawing.Point(259, 654)
        Me.btnViewVariables.Name = "btnViewVariables"
        Me.btnViewVariables.Size = New System.Drawing.Size(136, 34)
        Me.btnViewVariables.TabIndex = 10
        Me.btnViewVariables.Text = "View Variables"
        Me.btnViewVariables.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(905, 310)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(17, 17)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "X"
        '
        'xCoord
        '
        Me.xCoord.AutoSize = True
        Me.xCoord.Location = New System.Drawing.Point(928, 310)
        Me.xCoord.Name = "xCoord"
        Me.xCoord.Size = New System.Drawing.Size(0, 17)
        Me.xCoord.TabIndex = 12
        '
        'yCoord
        '
        Me.yCoord.AutoSize = True
        Me.yCoord.Location = New System.Drawing.Point(928, 327)
        Me.yCoord.Name = "yCoord"
        Me.yCoord.Size = New System.Drawing.Size(0, 17)
        Me.yCoord.TabIndex = 14
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(905, 327)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(17, 17)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Y"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1078, 739)
        Me.Controls.Add(Me.yCoord)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.xCoord)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnViewVariables)
        Me.Controls.Add(Me.btnResetPickoff)
        Me.Controls.Add(Me.btnCancelTest)
        Me.Controls.Add(Me.btnCalibrateDevice)
        Me.Controls.Add(Me.pgbTestStatus)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnRunTest)
        Me.Controls.Add(Me.STSChart)
        Me.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "GFU-Physical-Therapy STS"
        CType(Me.STSChart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents STSChart As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents btnRunTest As System.Windows.Forms.Button
    Friend WithEvents clkSamplingRate As System.Windows.Forms.Timer
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents chartSTSToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents pgbTestStatus As System.Windows.Forms.ProgressBar
    Friend WithEvents btnCalibrateDevice As System.Windows.Forms.Button
    Friend WithEvents btnCancelTest As System.Windows.Forms.Button
    Friend WithEvents btnResetPickoff As System.Windows.Forms.Button
    Friend WithEvents frmSaveTest As System.Windows.Forms.SaveFileDialog
    Friend WithEvents btnViewVariables As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents xCoord As System.Windows.Forms.Label
    Friend WithEvents yCoord As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
