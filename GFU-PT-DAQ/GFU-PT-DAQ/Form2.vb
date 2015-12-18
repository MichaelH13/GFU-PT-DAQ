Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form2

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '*********************************
        Dim dt0 As New DataTable
        Dim dt1 As New DataTable
        'Dim i As Integer
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

        ''Dim s As New Series
        ''s.Name = "aline"
        ' ''Change to a line graph.
        ''s.ChartType = SeriesChartType.Line
        ''s.Points.AddXY("1991", 15)
        ''s.Points.AddXY("1992", 17)
        ' ''Add the series to the Chart1 control.
        ''Chart1.Series.Add(s)

        ''dt0.Columns.Add("Time", GetType(Double))
        ''dt0.Columns.Add("Weight", GetType(Double))

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
            'If (lst = 3) Then 'Something wrong with the left leg capture.
            '    MsgBox("list3.Count: " & lists(3).Count & " sList(lst).Name: " & sList(lst).Name, , )
            'End If

            For idx As Integer = 0 To lists(lst).Count - 1 Step 1
                'MsgBox("list" & lst)
                'MsgBox("lists(lst)(idx): " & lists(lst)(idx))
                sList(lst).Points.AddXY(counter, lists(lst)(idx))
                counter += 100
            Next
            'MsgBox("Break 3")
            ' Add data to our chart.
            Chart1.Series.Add(sList(lst))

            ' Format the data in our chart.
            With Chart1.Series(lst)
                .ChartType = DataVisualization.Charting.SeriesChartType.Line
                .Color = colors(lst)
                .BorderWidth = 4
            End With
        Next

        ''dtTest.Columns.Add("TimePoint", GetType(Integer))
        ''dtTest.Columns.Add("Speed", GetType(Integer))

        'For lst As Integer = 0 To lists.Count - 1 Step 1
        '    sList(lst).Points.DataBindXY(listTimes, lists(lst))
        '    Chart1.Series(lst).Points.DataBindXY(listTimes, lists(lst))
        'Next

        ''counter = 0

        'For i = 0 To list0.Count - 1
        '    dt0.Rows.Add(counter, list0(i), list1(i), list2(i), list4(i), list5(i))
        '    counter += 100
        'Next
        'dt0.Columns.Add("Time", GetType(Integer))
        'dt0.Columns.Add("Channel0", GetType(Integer))
        'dt0.Columns.Add("Channel1", GetType(Integer))
        'dt0.Columns.Add("Channel2", GetType(Integer))
        'dt0.Columns.Add("Channel3", GetType(Integer))
        'dt0.Columns.Add("Channel4", GetType(Integer))
        'dt0.Columns.Add("Channel5", GetType(Integer))

        ' Add points to the graph.
        'counter = 0
        ' Debugging messages
        'MsgBox("list0.Count:" & list0.Count)
        'MsgBox("list1.Count:" & list1.Count)
        'MsgBox("list2.Count:" & list2.Count)
        'MsgBox("list3.Count:" & list3.Count)
        'MsgBox("list4.Count:" & list4.Count)
        'MsgBox("list5.Count:" & list5.Count)

        'For i = 0 To list0.Count - 1
        '    '***NOT WORKING FOR SOME REASON-list3 gets no data***'MsgBox("counter: " & counter & vbNewLine & "list0(i): " & list0(i) & vbNewLine & "list1(i): " & list1(i) & vbNewLine & "list2(i):" & list2(i) & vbNewLine & "list3(i):" & list3(i) & vbNewLine & "list4(i):" & list4(i) & vbNewLine & "list5(i):" & list5(i), vbOKOnly, getAppTitle())
        '    'MsgBox("counter: " & counter & vbNewLine & "list0(i): " & DirectCast(list0(i), Double) & vbNewLine & "list1(i): " & DirectCast(list0(i), Double) & vbNewLine & "list2(i):" & DirectCast(list0(i), Double) & vbNewLine & "list3(i):" & DirectCast(list0(i), Double) & vbNewLine & "list4(i):" & DirectCast(list0(i), Double) & vbNewLine & "list5(i):" & DirectCast(list0(i), Double), vbOKOnly, getAppTitle())
        '    'DOESN'T WORK TO DIRECT CAST INT TO DOUBLE MsgBox("counter: " & counter & "list0(i): " & DirectCast(list0(i), Double) & "list1(i): " & DirectCast(list1(i), Double) & "list2(i):" & DirectCast(list2(i), Double) & "list4(i):" & DirectCast(list4(i), Double) & "list5(i):" & DirectCast(list5(i), Double), vbOKOnly, getAppTitle())

        '    'dt0.Rows.Add(counter, DirectCast(list0(i), Double), DirectCast(list1(i), Double), DirectCast(list2(i), Double), DirectCast(list3(i), Double), DirectCast(list4(i), Double), DirectCast(list5(i), Double))
        '    'dt0.Rows.Add(counter, DirectCast(list0(i), Double), DirectCast(list1(i), Double), DirectCast(list2(i), Double), DirectCast(list4(i), Double), DirectCast(list5(i), Double))
        '    dt0.Rows.Add(counter, list0(i), list1(i), list2(i), list4(i), list5(i))
        '    counter += 100
        'Next

        ''For Each wUnit In list0
        ''    dt0.Rows.Add(counter, wUnit)
        ''    counter += 100
        ''Next
        ''counter = 0
        ''For Each wUnit In list2
        ''    dtTest.Rows.Add(counter, wUnit)
        ''    counter += 100
        ''Next
        ''counter = 0
        ''For Each wUnit In list3
        ''    dtTest.Rows.Add(counter, wUnit)
        ''    counter += 100
        ''Next
        ''counter = 0
        ''For Each wUnit In list4
        ''    dtTest.Rows.Add(counter, wUnit)
        ''    counter += 100
        ''Next
        ''counter = 0
        ''For Each wUnit In list5
        ''    dtTest.Rows.Add(counter, wUnit)
        ''    counter += 100
        ''Next

        ''dtTest.Rows.Add(0, 0)
        ''dtTest.Rows.Add(1000, 50)
        ''dtTest.Rows.Add(2000, 50)
        ''dtTest.Rows.Add(3000, 0)

        ' Set graph constraints
        'With Chart1.ChartAreas(0)
        '    .AxisX.Minimum = 0
        '    .AxisX.Maximum = 1000
        '    .AxisY.Minimum = -1
        '    .AxisY.Maximum = 3
        '    .AxisY.Interval = 0.25
        '    .AxisX.Title = "Time(ms)"
        '    .AxisY.Title = "Volts"
        'End With

        'With Chart1.ChartAreas(1)
        '    .AxisX.Minimum = 0
        '    .AxisX.Maximum = counter
        '    .AxisY.Minimum = -1
        '    .AxisY.Maximum = 3
        '    .AxisY.Interval = 0.25
        '    .AxisX.Title = "Time(ms)"
        '    .AxisY.Title = "Volts"
        'End With

        ' Bind the data and draw the chart.
        ''With Chart1.Series(0)
        ''    '.Points.DataBind(dt0.DefaultView, "Time", "Channel5", Nothing)
        ''    .ChartType = DataVisualization.Charting.SeriesChartType.Line
        ''    .Color = Color.Red
        ''    .BorderWidth = 4
        ''End With

        'With Chart1.Series(1)
        '    .Points.DataBind(dt1.DefaultView, "Time", "Channel5", Nothing)
        '    .ChartType = DataVisualization.Charting.SeriesChartType.Line
        '    .Color = Color.Red
        '    .BorderWidth = 4
        'End With
        '*********************************
    End Sub
End Class