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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.btnRunTest = New System.Windows.Forms.Button()
        Me.clkSamplingRate = New System.Windows.Forms.Timer(Me.components)
        Me.btnSave = New System.Windows.Forms.Button()
        Me.xCoord = New System.Windows.Forms.Label()
        Me.yCoord = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.pgbTestStatus = New System.Windows.Forms.ProgressBar()
        Me.btnCalibrateDevice = New System.Windows.Forms.Button()
        Me.btnSelectStartFrame = New System.Windows.Forms.Button()
        Me.btnSelectEndFrame = New System.Windows.Forms.Button()
        Me.btnSelectFirstMinima = New System.Windows.Forms.Button()
        Me.btnSelectBilateralPeak = New System.Windows.Forms.Button()
        Me.btnSelectSecondMinima = New System.Windows.Forms.Button()
        Me.btnSelectSeatOff = New System.Windows.Forms.Button()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Chart1
        '
        ChartArea1.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.Chart1.Legends.Add(Legend1)
        Me.Chart1.Location = New System.Drawing.Point(1, -1)
        Me.Chart1.Name = "Chart1"
        Series1.ChartArea = "ChartArea1"
        Series1.Legend = "Legend1"
        Series1.Name = "Series1"
        Me.Chart1.Series.Add(Series1)
        Me.Chart1.Size = New System.Drawing.Size(1070, 555)
        Me.Chart1.TabIndex = 1
        Me.Chart1.Text = "Chart1"
        '
        'btnRunTest
        '
        Me.btnRunTest.Location = New System.Drawing.Point(438, 604)
        Me.btnRunTest.Name = "btnRunTest"
        Me.btnRunTest.Size = New System.Drawing.Size(136, 34)
        Me.btnRunTest.TabIndex = 2
        Me.btnRunTest.Text = "Run"
        Me.btnRunTest.UseVisualStyleBackColor = True
        '
        'clkSamplingRate
        '
        Me.clkSamplingRate.Interval = 10
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(654, 604)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(136, 34)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'xCoord
        '
        Me.xCoord.AutoSize = True
        Me.xCoord.Location = New System.Drawing.Point(893, 274)
        Me.xCoord.Name = "xCoord"
        Me.xCoord.Size = New System.Drawing.Size(51, 17)
        Me.xCoord.TabIndex = 4
        Me.xCoord.Text = "Label1"
        '
        'yCoord
        '
        Me.yCoord.AutoSize = True
        Me.yCoord.Location = New System.Drawing.Point(1008, 274)
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
        'btnCalibrateDevice
        '
        Me.btnCalibrateDevice.Location = New System.Drawing.Point(218, 604)
        Me.btnCalibrateDevice.Name = "btnCalibrateDevice"
        Me.btnCalibrateDevice.Size = New System.Drawing.Size(136, 34)
        Me.btnCalibrateDevice.TabIndex = 7
        Me.btnCalibrateDevice.Text = "Calibrate Chair"
        Me.btnCalibrateDevice.UseVisualStyleBackColor = True
        '
        'btnSelectStartFrame
        '
        Me.btnSelectStartFrame.Location = New System.Drawing.Point(896, 320)
        Me.btnSelectStartFrame.Name = "btnSelectStartFrame"
        Me.btnSelectStartFrame.Size = New System.Drawing.Size(163, 34)
        Me.btnSelectStartFrame.TabIndex = 8
        Me.btnSelectStartFrame.Text = "Select Start Frame"
        Me.btnSelectStartFrame.UseVisualStyleBackColor = True
        '
        'btnSelectEndFrame
        '
        Me.btnSelectEndFrame.Location = New System.Drawing.Point(896, 360)
        Me.btnSelectEndFrame.Name = "btnSelectEndFrame"
        Me.btnSelectEndFrame.Size = New System.Drawing.Size(163, 34)
        Me.btnSelectEndFrame.TabIndex = 9
        Me.btnSelectEndFrame.Text = "Select End Frame"
        Me.btnSelectEndFrame.UseVisualStyleBackColor = True
        '
        'btnSelectFirstMinima
        '
        Me.btnSelectFirstMinima.Location = New System.Drawing.Point(896, 400)
        Me.btnSelectFirstMinima.Name = "btnSelectFirstMinima"
        Me.btnSelectFirstMinima.Size = New System.Drawing.Size(163, 34)
        Me.btnSelectFirstMinima.TabIndex = 10
        Me.btnSelectFirstMinima.Text = "Select First Minima"
        Me.btnSelectFirstMinima.UseVisualStyleBackColor = True
        '
        'btnSelectBilateralPeak
        '
        Me.btnSelectBilateralPeak.Location = New System.Drawing.Point(896, 440)
        Me.btnSelectBilateralPeak.Name = "btnSelectBilateralPeak"
        Me.btnSelectBilateralPeak.Size = New System.Drawing.Size(163, 34)
        Me.btnSelectBilateralPeak.TabIndex = 11
        Me.btnSelectBilateralPeak.Text = "Select Bilateral Peak"
        Me.btnSelectBilateralPeak.UseVisualStyleBackColor = True
        '
        'btnSelectSecondMinima
        '
        Me.btnSelectSecondMinima.Location = New System.Drawing.Point(896, 480)
        Me.btnSelectSecondMinima.Name = "btnSelectSecondMinima"
        Me.btnSelectSecondMinima.Size = New System.Drawing.Size(163, 34)
        Me.btnSelectSecondMinima.TabIndex = 12
        Me.btnSelectSecondMinima.Text = "Select Second Minima"
        Me.btnSelectSecondMinima.UseVisualStyleBackColor = True
        '
        'btnSelectSeatOff
        '
        Me.btnSelectSeatOff.Location = New System.Drawing.Point(896, 520)
        Me.btnSelectSeatOff.Name = "btnSelectSeatOff"
        Me.btnSelectSeatOff.Size = New System.Drawing.Size(163, 34)
        Me.btnSelectSeatOff.TabIndex = 13
        Me.btnSelectSeatOff.Text = "Select Seat Off"
        Me.btnSelectSeatOff.UseVisualStyleBackColor = True
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1071, 739)
        Me.Controls.Add(Me.btnSelectSeatOff)
        Me.Controls.Add(Me.btnSelectSecondMinima)
        Me.Controls.Add(Me.btnSelectBilateralPeak)
        Me.Controls.Add(Me.btnSelectFirstMinima)
        Me.Controls.Add(Me.btnSelectEndFrame)
        Me.Controls.Add(Me.btnSelectStartFrame)
        Me.Controls.Add(Me.btnCalibrateDevice)
        Me.Controls.Add(Me.pgbTestStatus)
        Me.Controls.Add(Me.yCoord)
        Me.Controls.Add(Me.xCoord)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnRunTest)
        Me.Controls.Add(Me.Chart1)
        Me.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Name = "Form2"
        Me.Text = "Sit-To-Stand 5000"
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents btnRunTest As System.Windows.Forms.Button
    Friend WithEvents clkSamplingRate As System.Windows.Forms.Timer
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents xCoord As System.Windows.Forms.Label
    Friend WithEvents yCoord As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents pgbTestStatus As System.Windows.Forms.ProgressBar
    Friend WithEvents btnCalibrateDevice As System.Windows.Forms.Button
    Friend WithEvents btnSelectStartFrame As System.Windows.Forms.Button
    Friend WithEvents btnSelectEndFrame As System.Windows.Forms.Button
    Friend WithEvents btnSelectFirstMinima As System.Windows.Forms.Button
    Friend WithEvents btnSelectBilateralPeak As System.Windows.Forms.Button
    Friend WithEvents btnSelectSecondMinima As System.Windows.Forms.Button
    Friend WithEvents btnSelectSeatOff As System.Windows.Forms.Button
End Class
