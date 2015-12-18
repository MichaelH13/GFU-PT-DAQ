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
        Me.components = New System.ComponentModel.Container()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ch0 = New System.Windows.Forms.Label()
        Me.lbl0 = New System.Windows.Forms.Label()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.lbl2 = New System.Windows.Forms.Label()
        Me.ch1 = New System.Windows.Forms.Label()
        Me.lbl1 = New System.Windows.Forms.Label()
        Me.ch2 = New System.Windows.Forms.Label()
        Me.lbl3 = New System.Windows.Forms.Label()
        Me.ch3 = New System.Windows.Forms.Label()
        Me.lbl4 = New System.Windows.Forms.Label()
        Me.ch4 = New System.Windows.Forms.Label()
        Me.lbl5 = New System.Windows.Forms.Label()
        Me.ch5 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(112, 186)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Start"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ch0
        '
        Me.ch0.AutoSize = True
        Me.ch0.Location = New System.Drawing.Point(18, 63)
        Me.ch0.Name = "ch0"
        Me.ch0.Size = New System.Drawing.Size(130, 17)
        Me.ch0.TabIndex = 1
        Me.ch0.Text = "Channel0 RightArm"
        '
        'lbl0
        '
        Me.lbl0.AutoSize = True
        Me.lbl0.Location = New System.Drawing.Point(178, 63)
        Me.lbl0.Name = "lbl0"
        Me.lbl0.Size = New System.Drawing.Size(102, 17)
        Me.lbl0.TabIndex = 2
        Me.lbl0.Text = "Display Value0"
        '
        'Timer2
        '
        '
        'lbl2
        '
        Me.lbl2.AutoSize = True
        Me.lbl2.Location = New System.Drawing.Point(178, 97)
        Me.lbl2.Name = "lbl2"
        Me.lbl2.Size = New System.Drawing.Size(102, 17)
        Me.lbl2.TabIndex = 4
        Me.lbl2.Text = "Display Value2"
        '
        'ch1
        '
        Me.ch1.AutoSize = True
        Me.ch1.Location = New System.Drawing.Point(18, 80)
        Me.ch1.Name = "ch1"
        Me.ch1.Size = New System.Drawing.Size(121, 17)
        Me.ch1.TabIndex = 3
        Me.ch1.Text = "Channel1 LeftArm"
        '
        'lbl1
        '
        Me.lbl1.AutoSize = True
        Me.lbl1.Location = New System.Drawing.Point(178, 80)
        Me.lbl1.Name = "lbl1"
        Me.lbl1.Size = New System.Drawing.Size(102, 17)
        Me.lbl1.TabIndex = 6
        Me.lbl1.Text = "Display Value1"
        '
        'ch2
        '
        Me.ch2.AutoSize = True
        Me.ch2.Location = New System.Drawing.Point(18, 97)
        Me.ch2.Name = "ch2"
        Me.ch2.Size = New System.Drawing.Size(129, 17)
        Me.ch2.TabIndex = 5
        Me.ch2.Text = "Channel2 RightLeg"
        '
        'lbl3
        '
        Me.lbl3.AutoSize = True
        Me.lbl3.Location = New System.Drawing.Point(178, 114)
        Me.lbl3.Name = "lbl3"
        Me.lbl3.Size = New System.Drawing.Size(102, 17)
        Me.lbl3.TabIndex = 8
        Me.lbl3.Text = "Display Value3"
        '
        'ch3
        '
        Me.ch3.AutoSize = True
        Me.ch3.Location = New System.Drawing.Point(18, 114)
        Me.ch3.Name = "ch3"
        Me.ch3.Size = New System.Drawing.Size(120, 17)
        Me.ch3.TabIndex = 7
        Me.ch3.Text = "Channel3 LeftLeg"
        '
        'lbl4
        '
        Me.lbl4.AutoSize = True
        Me.lbl4.Location = New System.Drawing.Point(178, 131)
        Me.lbl4.Name = "lbl4"
        Me.lbl4.Size = New System.Drawing.Size(102, 17)
        Me.lbl4.TabIndex = 10
        Me.lbl4.Text = "Display Value4"
        '
        'ch4
        '
        Me.ch4.AutoSize = True
        Me.ch4.Location = New System.Drawing.Point(18, 131)
        Me.ch4.Name = "ch4"
        Me.ch4.Size = New System.Drawing.Size(120, 17)
        Me.ch4.TabIndex = 9
        Me.ch4.Text = "Channel4 Ground"
        '
        'lbl5
        '
        Me.lbl5.AutoSize = True
        Me.lbl5.Location = New System.Drawing.Point(178, 148)
        Me.lbl5.Name = "lbl5"
        Me.lbl5.Size = New System.Drawing.Size(102, 17)
        Me.lbl5.TabIndex = 12
        Me.lbl5.Text = "Display Value5"
        '
        'ch5
        '
        Me.ch5.AutoSize = True
        Me.ch5.Location = New System.Drawing.Point(18, 148)
        Me.ch5.Name = "ch5"
        Me.ch5.Size = New System.Drawing.Size(101, 17)
        Me.ch5.TabIndex = 11
        Me.ch5.Text = "Channel5 Seat"
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(315, 255)
        Me.Controls.Add(Me.lbl5)
        Me.Controls.Add(Me.ch5)
        Me.Controls.Add(Me.lbl4)
        Me.Controls.Add(Me.ch4)
        Me.Controls.Add(Me.lbl3)
        Me.Controls.Add(Me.ch3)
        Me.Controls.Add(Me.lbl1)
        Me.Controls.Add(Me.ch2)
        Me.Controls.Add(Me.lbl2)
        Me.Controls.Add(Me.ch1)
        Me.Controls.Add(Me.lbl0)
        Me.Controls.Add(Me.ch0)
        Me.Controls.Add(Me.Button1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ch0 As System.Windows.Forms.Label
    Friend WithEvents lbl0 As System.Windows.Forms.Label
    Friend WithEvents Timer2 As System.Windows.Forms.Timer
    Friend WithEvents lbl2 As System.Windows.Forms.Label
    Friend WithEvents ch1 As System.Windows.Forms.Label
    Friend WithEvents lbl1 As System.Windows.Forms.Label
    Friend WithEvents ch2 As System.Windows.Forms.Label
    Friend WithEvents lbl3 As System.Windows.Forms.Label
    Friend WithEvents ch3 As System.Windows.Forms.Label
    Friend WithEvents lbl4 As System.Windows.Forms.Label
    Friend WithEvents ch4 As System.Windows.Forms.Label
    Friend WithEvents lbl5 As System.Windows.Forms.Label
    Friend WithEvents ch5 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
