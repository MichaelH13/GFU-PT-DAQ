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

    Dim samplingRate As Double = getSamplingRate()
    Dim secondsPerTest As Double = getSecondsPerTest()
    Dim timeCounter As Double
    Dim totalSamples As Double = samplingRate * secondsPerTest

    Private Sub clkSamplingRate_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles clkSamplingRate.Tick
        If (timeCounter >= totalSamples) Then
            clkSamplingRate.Stop()
            clkSamplingRate.Dispose()
            reloadChart()

            ' Update the calibration values if we are doing a calibration run.
            If (calibrateDevice) Then
                rightArmOffset = getAvgFor(listRightArm)
                leftArmOffset = getAvgFor(listLeftArm)
                rightLegOffset = getAvgFor(listRightLeg)
                leftLegOffset = getAvgFor(listLeftLeg)
                seatOffset = getAvgFor(listSeat)
                groundOffset = getAvgFor(listGround)
                calibrateDevice = False
            End If
        Else
            getSample()
            listTimes.Add(timeCounter)
            timeCounter += 1
            Me.pgbTestStatus.Value = CInt((timeCounter / totalSamples) * 100.0)

            ' Hide the progress bar and see if the user wants to select their own points.
            If (Me.pgbTestStatus.Value >= totalSamples) Then
                Me.pgbTestStatus.Visible = False
                If Not calibrateDevice Then frmState = FORM_STATE.DECIDE_AUTO_POINTS Else frmState = FORM_STATE.NEW_TEST
            End If
        End If
    End Sub

    Private Sub getSample()
        ' If we are doing a calibration, then simply take whatever values we get.
        ' Otherwise use the established offsets that we have previously obtained.
        If (calibrateDevice) Then
            myDAQ.AIn(0, MccDaq.Range.Bip10Volts, dataValueC0)
            myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC0, engUnitC0)
            listRightArm.Add(engUnitC0)

            myDAQ.AIn(1, MccDaq.Range.Bip10Volts, dataValueC1)
            myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC1, engUnitC1)
            listLeftArm.Add(engUnitC1)

            myDAQ.AIn(2, MccDaq.Range.Bip10Volts, dataValueC2)
            myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC2, engUnitC2)
            listRightLeg.Add(engUnitC2)

            myDAQ.AIn(3, MccDaq.Range.Bip10Volts, dataValueC3)
            myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC3, engUnitC3)
            listLeftLeg.Add(engUnitC3)

            myDAQ.AIn(4, MccDaq.Range.Bip10Volts, dataValueC4)
            myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC4, engUnitC4)
            listGround.Add(engUnitC4)

            myDAQ.AIn(5, MccDaq.Range.Bip10Volts, dataValueC5)
            myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC5, engUnitC5)
            listSeat.Add(engUnitC5)

            listBilateral.Add(listRightLeg(timeCounter - 1) + listLeftLeg(timeCounter - 1))
        Else
            myDAQ.AIn(0, MccDaq.Range.Bip10Volts, dataValueC0)
            myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC0, engUnitC0)
            listRightArm.Add(engUnitC0 - rightArmOffset)

            myDAQ.AIn(1, MccDaq.Range.Bip10Volts, dataValueC1)
            myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC1, engUnitC1)
            listLeftArm.Add(engUnitC1 - leftArmOffset)

            myDAQ.AIn(2, MccDaq.Range.Bip10Volts, dataValueC2)
            myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC2, engUnitC2)
            listRightLeg.Add(engUnitC2 - rightLegOffset)

            myDAQ.AIn(3, MccDaq.Range.Bip10Volts, dataValueC3)
            myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC3, engUnitC3)
            listLeftLeg.Add(engUnitC3 - leftLegOffset)

            myDAQ.AIn(4, MccDaq.Range.Bip10Volts, dataValueC4)
            myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC4, engUnitC4)
            listGround.Add(engUnitC4 - groundOffset)

            myDAQ.AIn(5, MccDaq.Range.Bip10Volts, dataValueC5)
            myDAQ.ToEngUnits(MccDaq.Range.Bip10Volts, dataValueC5, engUnitC5)
            listSeat.Add(engUnitC5 - seatOffset)

            listBilateral.Add(listRightLeg(timeCounter - 1) + listLeftLeg(timeCounter - 1))
        End If
    End Sub

    Private Sub reloadChart()

        Dim lists As New ArrayList 'list of data points we got from the DAQ.
        Dim sList As New ArrayList 'list of series we are going to plot.

        ' Put our lists of points in a list (of lists).
        lists.Add(listRightArm)
        lists.Add(listLeftArm)
        lists.Add(listRightLeg)
        lists.Add(listLeftLeg)
        lists.Add(listGround)
        lists.Add(listSeat)
        lists.Add(listBilateral)

        ' Declare our series to add to our graph.
        Dim seriesRightArm As New Series
        Dim seriesLeftArm As New Series
        Dim seriesRightLeg As New Series
        Dim seriesLeftLeg As New Series
        Dim seriesGround As New Series
        Dim seriesSeat As New Series
        Dim seriesBilateral As New Series

        ' Set the name appropriately for each series.
        seriesRightArm.Name = "Right Arm"
        seriesLeftArm.Name = "Left Arm"
        seriesRightLeg.Name = "Right Leg"
        seriesLeftLeg.Name = "Left Leg"
        seriesGround.Name = "Ground"
        seriesSeat.Name = "Seat"
        seriesBilateral.Name = "Bilateral"

        ' Add each series to our list of series.
        sList.Add(seriesRightArm)
        sList.Add(seriesLeftArm)
        sList.Add(seriesRightLeg)
        sList.Add(seriesLeftLeg)
        sList.Add(seriesGround)
        sList.Add(seriesSeat)
        sList.Add(seriesBilateral)

        Chart1.Series.Clear()

        Dim colors As New ArrayList

        ' Add each of the colors to our colors list (for displaying on the graph).
        colors.Add(Color.Red)           ' Right Arm
        colors.Add(Color.Blue)          ' Left Arm
        colors.Add(Color.Orange)        ' Right Leg
        colors.Add(Color.Pink)          ' Left Leg
        colors.Add(Color.Green)         ' Ground
        colors.Add(Color.Black)         ' Seat
        colors.Add(Color.DarkMagenta)   ' Bilateral (Right Leg + Left Leg)

        ' Iterate over each one of our lists.
        For lst As Integer = 0 To sList.Count - 1 Step 1

            ' Add all of the points to our series.
            For idx As Integer = 0 To lists(lst).Count - 1 Step 1
                ' Add a point from our listsTimes array and the curren idx 
                ' for the lst that we are adding points for.
                sList(lst).Points.AddXY(listTimes(idx), lists(lst)(idx))
            Next

            ' Add data series to our chart.
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


    Private Sub btnCalibrateDevice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalibrateDevice.Click

        ' Only allow user to calibrate the device if we are ready to run a new test.
        If (frmState = FORM_STATE.NEW_TEST) Then
            calibrateDevice = True
            timeCounter = 1                         ' Initialize our timeCounter.
            clearLists()                            ' Clear our list of datapoints so we don't get inaccurate data.
            initProgressBar()                       ' Initialize our progress bar (i.e. set min/max values and make it visible).
            clkSamplingRate.Start()                 ' Finally, start our sampling timer.
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim listDataPoints As ArrayList = getDataPoints()
        Dim listsToWriteToFile As String()
        Dim idx As Integer

        ' Don't save data if we are doing a calibration.
        If calibrateDevice Then Exit Sub

        ReDim listsToWriteToFile(listDataPoints(0).Count)

        If (frmState = FORM_STATE.SAVE_TEST) Then

            ' Iterate over the data points.
            For i As Integer = 0 To listDataPoints(0).Count - 1 Step 1

                ' Write data points for time i at idx in our listsToWriteToFile string Array.
                For j As Integer = 0 To listDataPoints.Count - 1 Step 1
                    listsToWriteToFile(idx) = listsToWriteToFile(idx) & listDataPoints(j)(i) & IIf(j + 1 = listDataPoints.Count, "", ",")
                Next

                ' Iterate our idx so we will write the next i sample time on a new line.
                idx += 1
            Next


            Dim FileName As String = "C:\DAQ\test_run.txt"
            IO.File.WriteAllLines(FileName, listsToWriteToFile)
            MsgBox("Saved file!", vbOKOnly, getAppTitle())


            clearLists()
            frmState = FORM_STATE.NEW_TEST
        End If
    End Sub

    Private Sub btnRunTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunTest.Click
        If (frmState = FORM_STATE.NEW_TEST) Then
            timeCounter = 1                         ' Initialize our timeCounter.
            clearLists()                            ' Clear our list of datapoints so we don't get inaccurate data.
            initProgressBar()                       ' Initialize our progress bar (i.e. set min/max values and make it visible).
            clkSamplingRate.Start()                 ' Finally, start our sampling timer.
        End If
    End Sub

    Private Sub clearLists()
        listTimes.Clear()
        listRightArm.Clear()
        listLeftArm.Clear()
        listRightLeg.Clear()
        listLeftLeg.Clear()
        listGround.Clear()
        listSeat.Clear()
        listBilateral.Clear()
    End Sub

    Private Sub initProgressBar()
        Me.pgbTestStatus.Value = 0
        Me.pgbTestStatus.Minimum = 0
        Me.pgbTestStatus.Maximum = totalSamples
        Me.pgbTestStatus.Visible = True
    End Sub

    Private Function getAvgFor(ByRef list As ArrayList) As Double
        Dim sum As Double

        For Each item As Double In list
            sum += item
        Next

        Return sum / list.Count
    End Function
End Class