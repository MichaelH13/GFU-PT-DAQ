Public Class Form1

    Dim myDAQ As New MccDaq.MccBoard(0)

    Dim dataValueC0 As System.Int16
    Dim engUnitC0 As Single
    Dim list0 As New ArrayList

    Dim dataValueC1 As System.Int16
    Dim engUnitC1 As Single
    Dim list1 As New ArrayList

    Dim dataValueC2 As System.Int16
    Dim engUnitC2 As Single
    Dim list2 As New ArrayList

    Dim dataValueC3 As System.Int16
    Dim engUnitC3 As Single
    Dim list3 As New ArrayList

    Dim dataValueC4 As System.Int16
    Dim engUnitC4 As Single
    Dim list4 As New ArrayList

    Dim dataValueC5 As System.Int16
    Dim engUnitC5 As Single
    Dim list5 As New ArrayList

    Dim timeCounter As Integer

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Timer1.Start()
        timeCounter = 0
        Timer2.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If (timeCounter = 10) Then
            Timer2.Stop()
            Timer1.Stop()
            MsgBox("DONE", vbOKOnly + vbSystemModal, getAppTitle())
            '*********************************
            'Dim dtTest As New DataTable

            'dtTest.Columns.Add("TimePoint", GetType(Integer))
            'dtTest.Columns.Add("Speed", GetType(Integer))

            'dtTest.Rows.Add(0, 0)
            'dtTest.Rows.Add(1000, 50)
            'dtTest.Rows.Add(2000, 50)
            'dtTest.Rows.Add(3000, 0)

            'With Chart1.ChartAreas(0)
            '    .AxisX.Minimum = 0
            '    .AxisX.Maximum = 3000
            '    .AxisY.Minimum = 0
            '    .AxisY.Maximum = 60
            '    .AxisY.Interval = 10
            '    .AxisX.Title = "Elapsed Time (ms)"
            '    .AxisY.Title = "Speed (km/hr)"
            'End With

            'With Chart1.Series(0)
            '    .Points.DataBind(dtTest.DefaultView, "TimePoint", "Speed", Nothing)
            '    .ChartType = DataVisualization.Charting.SeriesChartType.Line
            '    .BorderWidth = 4
            'End With
            '*********************************
        Else
            timeCounter = timeCounter + 1
        End If
    End Sub

    Private Sub Timer2_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        myDAQ.AIn(0, MccDaq.Range.Bip10Volts, dataValueC0)
        myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC0, engUnitC0)
        list0.Add(engUnitC0)
        Label2.Text = engUnitC0

        myDAQ.AIn(1, MccDaq.Range.Bip10Volts, dataValueC1)
        myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC1, engUnitC1)
        list1.Add(engUnitC1)
        Label5.Text = engUnitC1

        myDAQ.AIn(2, MccDaq.Range.Bip10Volts, dataValueC2)
        myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC2, engUnitC2)
        list2.Add(engUnitC2)
        Label3.Text = engUnitC2

        myDAQ.AIn(3, MccDaq.Range.Bip10Volts, dataValueC3)
        myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC3, engUnitC3)
        list3.Add(engUnitC3)
        Label7.Text = engUnitC3

        myDAQ.AIn(4, MccDaq.Range.Bip10Volts, dataValueC4)
        myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC4, engUnitC4)
        list4.Add(engUnitC4)
        Label9.Text = engUnitC4

        myDAQ.AIn(5, MccDaq.Range.Bip10Volts, dataValueC5)
        myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC5, engUnitC5)
        list5.Add(engUnitC5)
        Label11.Text = engUnitC5
    End Sub

End Class
