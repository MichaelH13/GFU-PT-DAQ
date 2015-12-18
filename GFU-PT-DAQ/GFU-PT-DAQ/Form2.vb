Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form2

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        reloadChart()
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
        Timer1.Start()
        timeCounter = 0
        Timer2.Start()
        list0.Clear()
        list1.Clear()
        list2.Clear()
        list3.Clear()
        list4.Clear()
        list5.Clear()
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

End Class