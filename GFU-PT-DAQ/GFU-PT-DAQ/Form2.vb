Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form2

    Public frmState As Integer
    Public xStart As Double
    Public xEnd As Double
    Public yStart As Double
    Public yEnd As Double

    Public Enum FORM_STATE As Integer
        RUN_TEST = 0
        DECIDE_AUTO_POINTS = 1
        PICK_START_POINT = 2
        PICK_END_POINT = 3
        NEW_TEST = 4
        SAVE_TEST = 5
    End Enum

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        frmState = FORM_STATE.NEW_TEST
        reloadChart()
        Me.pgbTestStatus.Visible = False
    End Sub

    Dim myDAQ As New MccDaq.MccBoard(0)

    Dim dataValueC0 As System.Int16
    Dim engUnitC0 As Single

    Dim dataValueC1 As System.Int16
    Dim engUnitC1 As Single

    Dim dataValueC2 As System.Int16
    Dim engUnitC2 As Single

    Dim dataValueC3 As System.Int16
    Dim engUnitC3 As Single

    Dim dataValueC4 As System.Int16
    Dim engUnitC4 As Single

    Dim dataValueC5 As System.Int16
    Dim engUnitC5 As Single

    Dim timeCounter As Integer

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If (frmState = FORM_STATE.NEW_TEST) Then
            frmState = FORM_STATE.RUN_TEST
            Me.pgbTestStatus.Visible = True
            Timer1.Start()
            timeCounter = 0
            Timer2.Start()
            list0.Clear()
            list1.Clear()
            list2.Clear()
            list3.Clear()
            list4.Clear()
            list5.Clear()
            Me.pgbTestStatus.Value = 0
            Me.pgbTestStatus.Minimum = 0
            Me.pgbTestStatus.Maximum = 100
            Me.pgbTestStatus.Visible = True
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If (timeCounter = 10) Then
            Timer1.Stop()
            Timer2.Stop()
            Timer1.Dispose()
            Timer2.Dispose()

            reloadChart()

            Dim strArray As String()
            ReDim strArray(list0.Count)

            For i As Integer = 0 To list0.Count - 1 Step 1
                strArray(i) = list0(i).ToString
            Next

            Dim FileName As String = "C:\DAQ\test_run.txt"
            IO.File.WriteAllLines(FileName, strArray)
        Else
            listTimes.Add(timeCounter * 100)
            timeCounter = timeCounter + 1
            Me.pgbTestStatus.Value = timeCounter * 10

            'Hide the progress bar and see if the user wants to select their own points.
            If (Me.pgbTestStatus.Value = 100) Then
                Me.pgbTestStatus.Visible = False
                frmState = FORM_STATE.DECIDE_AUTO_POINTS
            End If
        End If
    End Sub

    Private Sub Timer2_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer2.Tick

        myDAQ.AIn(0, MccDaq.Range.Bip10Volts, dataValueC0)
        myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC0, engUnitC0)
        list0.Add(engUnitC0)
        'lbl0.Text = engUnitC0

        myDAQ.AIn(1, MccDaq.Range.Bip10Volts, dataValueC1)
        myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC1, engUnitC1)
        list1.Add(engUnitC1)
        'lbl1.Text = engUnitC1

        myDAQ.AIn(2, MccDaq.Range.Bip10Volts, dataValueC2)
        myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC2, engUnitC2)
        list2.Add(engUnitC2)
        'lbl2.Text = engUnitC2

        myDAQ.AIn(3, MccDaq.Range.Bip10Volts, dataValueC3)
        myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC3, engUnitC3)
        list3.Add(engUnitC3)
        'lbl3.Text = engUnitC3

        myDAQ.AIn(4, MccDaq.Range.Bip10Volts, dataValueC4)
        myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC4, engUnitC4)
        list4.Add(engUnitC4)
        'lbl4.Text = engUnitC4

        myDAQ.AIn(5, MccDaq.Range.Bip10Volts, dataValueC5)
        myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC5, engUnitC5)
        list5.Add(engUnitC5)
        'lbl5.Text = engUnitC5
    End Sub

    Private Sub reloadChart()
        Dim counter As Integer
        Dim lists As New ArrayList
        Dim sList As New ArrayList

        ' Put our lists of points in a list.
        lists.Add(list0)
        lists.Add(list1)
        lists.Add(list2)
        lists.Add(list3)
        lists.Add(list4)
        lists.Add(list5)
        Dim s0 As New Series()
        Dim s1 As New Series()
        Dim s2 As New Series()
        Dim s3 As New Series()
        Dim s4 As New Series()
        Dim s5 As New Series()
        s0.Name = "Channel0"
        s1.Name = "Channel1"
        s2.Name = "Channel2"
        s3.Name = "Channel3"
        s4.Name = "Channel4"
        s5.Name = "Channel5"
        sList.Add(s0)
        sList.Add(s1)
        sList.Add(s2)
        sList.Add(s3)
        sList.Add(s4)
        sList.Add(s5)

        Chart1.Series.Clear()

        Dim colors As New ArrayList
        colors.Add(Color.Red)
        colors.Add(Color.Blue)
        colors.Add(Color.Orange)
        colors.Add(Color.Pink)
        colors.Add(Color.Green)
        colors.Add(Color.Black)

        ' Iterate over each one of our lists.
        For lst As Integer = 0 To sList.Count - 1 Step 1
            counter = 100

            For idx As Integer = 0 To lists(lst).Count - 1 Step 1
                sList(lst).Points.AddXY(counter, lists(lst)(idx))
                counter += 100
            Next

            ' Add data to our chart.
            Chart1.Series.Add(sList(lst))

            ' Format the data in our chart.
            With Chart1.Series(lst)
                .ChartType = DataVisualization.Charting.SeriesChartType.Line
                .Color = colors(lst)
                .BorderWidth = 4
            End With
        Next

    End Sub

    Private Sub Chart1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chart1.Click
        If (frmState = FORM_STATE.RUN_TEST) Then frmState = FORM_STATE.PICK_START_POINT
        If (frmState = FORM_STATE.PICK_START_POINT) Then
            If (MsgBox("Are you sure the point X=" & Me.xCoord.Text & " Y=" & Me.yCoord.Text & " is your desired start point?", vbYesNo, getAppTitle()) = vbYes) Then
                frmState = FORM_STATE.PICK_END_POINT
                xStart = CDbl(Me.xCoord.Text)
                yStart = CDbl(Me.yCoord.Text)
            End If
        ElseIf (frmState = FORM_STATE.PICK_END_POINT) Then
            If (MsgBox("Are you sure the point X=" & Me.xCoord.Text & " Y=" & Me.yCoord.Text & " is your desired end point?", vbYesNo, getAppTitle()) = vbYes) Then
                frmState = FORM_STATE.SAVE_TEST
                xEnd = CDbl(Me.xCoord.Text)
                yEnd = CDbl(Me.yCoord.Text)
                MsgBox("Final Points: " & vbNewLine & "Start: X=" & xStart & " Y=" & yStart & vbNewLine & "End: X=" & xEnd & " Y=" & yEnd)
            End If
        End If

    End Sub

    Private Sub Chart1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Chart1.MouseMove
        If (frmState = FORM_STATE.DECIDE_AUTO_POINTS) Then
            If (MsgBox("Would you like to select your own points?", vbYesNo, getAppTitle()) = vbYes) Then
                frmState = FORM_STATE.PICK_START_POINT
            Else
                frmState = FORM_STATE.SAVE_TEST
            End If
        End If

        'Only trigger event if we are picking points, otherwise ignore, and reset values on our textboxes and what not.
        If (frmState = FORM_STATE.PICK_END_POINT Or frmState = FORM_STATE.PICK_START_POINT) Then
            Dim result As HitTestResult = Chart1.HitTest(e.X, e.Y)
            Dim xchartcoord As Integer
            Dim ychartcoord As Integer

            If result.PointIndex > 0 Then
                Dim dp As DataPoint = Chart1.Series(0).Points(result.PointIndex)
                ToolTip1.SetToolTip(Chart1, "X:" & dp.XValue & " Y:" & dp.YValues(0))
                Me.xCoord.Text = dp.XValue
                Me.yCoord.Text = dp.YValues(0)
                xchartcoord = dp.XValue
                ychartcoord = dp.YValues(0)
            End If
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If (frmState = FORM_STATE.SAVE_TEST) Then
            frmState = FORM_STATE.NEW_TEST
        End If
    End Sub
End Class