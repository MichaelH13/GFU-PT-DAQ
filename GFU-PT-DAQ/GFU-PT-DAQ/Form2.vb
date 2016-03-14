Imports System.Windows.Forms.DataVisualization.Charting

Public Class Form2

    Public Enum FORM_STATE As Integer
        NEW_TEST
        RUN_TEST
        DECIDE_AUTO_POINTS
        SELECT_START_FRAME
        SELECT_END_FRAME
        SELECT_FIRST_MINIMA
        SELECT_BILATERAL_PEAK
        SELECT_SECOND_MINIMA
        SELECT_SEAT_OFF
        SHOW_POINTS
        SAVE_TEST
    End Enum

    Public arrays As Short()()
    Public frmState As Integer = FORM_STATE.NEW_TEST
    Public xStart As Double
    Public xEnd As Double
    Public yStart As Double
    Public yEnd As Double

    Public xchartcoord As Integer
    Public ychartcoord As Integer

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        frmState = FORM_STATE.NEW_TEST
        reloadChart()
        samplingRate = 1000 / Me.clkSamplingRate.Interval
        Me.pgbTestStatus.Visible = False
        totalSamples = getTotalSamplesInTest()
    End Sub

    Dim myDAQ As New MccDaq.MccBoard(0)
    Dim voltageRange As MccDaq.Range = MccDaq.Range.Bip10Volts

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

    Dim timeCounter As Double
    Dim totalSamples As Double

    Private Sub clkSamplingRate_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles clkSamplingRate.Tick
        If (timeCounter >= totalSamples) Then
            clkSamplingRate.Stop()
            clkSamplingRate.Dispose()

            For i = 1 To listLeftLeg.Count
                listBilateral.Add(listRightLeg(i - 1) + listLeftLeg(i - 1))
            Next

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
            If (timeCounter >= totalSamples) Then
                Me.pgbTestStatus.Visible = False
                If Not calibrateDevice Then frmState = FORM_STATE.DECIDE_AUTO_POINTS Else frmState = FORM_STATE.NEW_TEST
            End If
        End If
    End Sub

    Public Sub getVoltages(ByRef theBoard As MccDaq.MccBoard, ByVal channel As Integer, ByRef voltages As Short(), ByVal time As Integer, ByVal voltagesToGet As Integer)
        Dim voltageRange As MccDaq.Range = MccDaq.Range.Bip10Volts

        Dim dataValue As System.Int16
        Dim engUnit As Single

        For i = 0 To voltagesToGet
            theBoard.AIn(channel, voltageRange, dataValue)
            voltages(i) = engUnit
        Next i

    End Sub


    Private Sub getSample()


        ' If we are doing a calibration, then simply take whatever values we get.
        ' Otherwise use the established offsets that we have previously obtained.
        If (calibrateDevice) Then
            myDAQ.AIn(0, voltageRange, dataValueC0)
            myDAQ.ToEngUnits(voltageRange, dataValueC0, engUnitC0)
            listRightArm.Add(engUnitC0)

            myDAQ.AIn(1, voltageRange, dataValueC1)
            myDAQ.ToEngUnits(voltageRange, dataValueC1, engUnitC1)
            listLeftArm.Add(engUnitC1)

            myDAQ.AIn(2, voltageRange, dataValueC2)
            myDAQ.ToEngUnits(voltageRange, dataValueC2, engUnitC2)
            listRightLeg.Add(engUnitC2)

            myDAQ.AIn(3, voltageRange, dataValueC3)
            myDAQ.ToEngUnits(voltageRange, dataValueC3, engUnitC3)
            listLeftLeg.Add(engUnitC3)

            myDAQ.AIn(4, voltageRange, dataValueC4)
            myDAQ.ToEngUnits(voltageRange, dataValueC4, engUnitC4)
            listGround.Add(engUnitC4)

            myDAQ.AIn(5, voltageRange, dataValueC5)
            myDAQ.ToEngUnits(voltageRange, dataValueC5, engUnitC5)
            listSeat.Add(engUnitC5)
        Else
            myDAQ.AIn(0, voltageRange, dataValueC0)
            myDAQ.ToEngUnits(voltageRange, dataValueC0, engUnitC0)
            listRightArm.Add(engUnitC0 - rightArmOffset)

            myDAQ.AIn(1, voltageRange, dataValueC1)
            myDAQ.ToEngUnits(voltageRange, dataValueC1, engUnitC1)
            listLeftArm.Add(engUnitC1 - leftArmOffset)

            myDAQ.AIn(2, voltageRange, dataValueC2)
            myDAQ.ToEngUnits(voltageRange, dataValueC2, engUnitC2)
            listRightLeg.Add(engUnitC2 - rightLegOffset)

            myDAQ.AIn(3, voltageRange, dataValueC3)
            myDAQ.ToEngUnits(voltageRange, dataValueC3, engUnitC3)
            listLeftLeg.Add(engUnitC3 - leftLegOffset)

            myDAQ.AIn(4, voltageRange, dataValueC4)
            myDAQ.ToEngUnits(voltageRange, dataValueC4, engUnitC4)
            listGround.Add(engUnitC4 - groundOffset)

            myDAQ.AIn(5, voltageRange, dataValueC5)
            myDAQ.ToEngUnits(voltageRange, dataValueC5, engUnitC5)
            listSeat.Add(engUnitC5 - seatOffset)
        End If
    End Sub

    Private Sub reloadChart()

        Dim lists As New ArrayList 'list of data points we got from the DAQ.
        Dim sList As New ArrayList 'list of series we are going to plot.

        ' Declare our series to add to our graph.
        Dim seriesRightArm As New Series
        Dim seriesLeftArm As New Series
        Dim seriesRightLeg As New Series
        Dim seriesLeftLeg As New Series
        Dim seriesGround As New Series
        Dim seriesSeat As New Series
        Dim seriesBilateral As New Series

        ' Put our lists of points in a list (of lists).
        lists.Add(listRightArm)
        lists.Add(listLeftArm)
        lists.Add(listRightLeg)
        lists.Add(listLeftLeg)
        lists.Add(listGround)
        lists.Add(listSeat)
        lists.Add(listBilateral)

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
        colors.Add(Color.DarkMagenta)   ' Leg Bilateral (Right Leg + Left Leg)

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

        ' Now confirm the selection with the user.
        Select Case (frmState)
            Case FORM_STATE.SELECT_START_FRAME
                If (confirmPoint("Start of Test")) Then
                    startSTSFrame = xchartcoord
                    frmState = FORM_STATE.SELECT_END_FRAME
                End If

            Case FORM_STATE.SELECT_END_FRAME
                If (confirmPoint("End of Test")) Then
                    endSTSFrame = xchartcoord
                    frmState = FORM_STATE.SELECT_FIRST_MINIMA
                End If

            Case FORM_STATE.SELECT_FIRST_MINIMA
                If (confirmPoint("First Minima")) Then
                    firstPeakFrame = xchartcoord
                    frmState = FORM_STATE.SELECT_BILATERAL_PEAK
                End If

            Case FORM_STATE.SELECT_BILATERAL_PEAK
                If (confirmPoint("Bilateral Peak")) Then
                    secondPeakFrame = xchartcoord
                    frmState = FORM_STATE.SELECT_SECOND_MINIMA
                End If

            Case FORM_STATE.SELECT_SECOND_MINIMA
                If (confirmPoint("Second Minima")) Then
                    thirdPeakFrame = xchartcoord
                    frmState = FORM_STATE.SELECT_SEAT_OFF
                End If

            Case FORM_STATE.SELECT_SEAT_OFF
                If (confirmPoint("Seat Off")) Then
                    seatOffFrame = xchartcoord
                    frmState = FORM_STATE.SHOW_POINTS
                End If

            Case Else
                MsgBox(getDefaultErrorFormatting("Click-state"), vbOKOnly + vbExclamation, getAppTitle())
        End Select

    End Sub

    Private Function confirmPoint(ByVal pointName As String) As Boolean
        confirmPoint = MsgBox("Are you sure the point X=" & Me.xCoord.Text & " Y=" & Me.yCoord.Text & " is your desired " & pointName & " point?", vbYesNo, getAppTitle()) = vbYes
    End Function

    Private Sub Chart1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Chart1.MouseMove
        Dim result As HitTestResult = Chart1.HitTest(e.X, e.Y)

        If (frmState = FORM_STATE.DECIDE_AUTO_POINTS) Then
            If (MsgBox("Would you like to select your own points?", vbYesNo, getAppTitle()) = vbYes) Then
                frmState = FORM_STATE.SELECT_START_FRAME
            Else
                frmState = FORM_STATE.SAVE_TEST
            End If
        End If

        'Only trigger event if we are picking points, otherwise ignore, and reset values on our textboxes and what not.
        If (frmState >= FORM_STATE.SELECT_START_FRAME And frmState <= FORM_STATE.SAVE_TEST) Then

            ' If the click was successful, then figure out where it was.
            If result.PointIndex > 0 Then

                Dim dp As DataPoint = Chart1.Series(6).Points(result.PointIndex)
                ToolTip1.SetToolTip(Chart1, "X:" & dp.XValue & " Y:" & dp.YValues(0))
                Me.xCoord.Text = dp.XValue
                Me.yCoord.Text = dp.YValues(0)
                xchartcoord = dp.XValue
                ychartcoord = dp.YValues(0)

                ' Now confirm the selection with the user.
                Select Case (frmState)
                    Case FORM_STATE.SELECT_START_FRAME
                    Case FORM_STATE.SELECT_END_FRAME
                    Case FORM_STATE.SELECT_FIRST_MINIMA
                    Case FORM_STATE.SELECT_BILATERAL_PEAK
                    Case FORM_STATE.SELECT_SECOND_MINIMA
                    Case FORM_STATE.SELECT_SEAT_OFF
                    Case FORM_STATE.SHOW_POINTS

                        'Now clip the data and set the time to percent STS for plotting
                        lengthSTS = endSTSFrame - startSTSFrame

                        takeDerivativesOfvGRF()
                        calculateLegDerivatives(firstPeakFrame, endSTSFrame)
                        convertDataFromVoltagesToWeight()

                        rightLegPeakFrame = getRightLegPeakFrame(firstPeakFrame, thirdPeakFrame)
                        MsgBox("Right Leg Peak Frame: " & rightLegPeakFrame, vbInformation + vbSystemModal, getAppTitle())
                        leftLegPeakFrame = getLeftLegPeakFrame(firstPeakFrame, thirdPeakFrame)
                        MsgBox("Left Leg Peak Frame: " & leftLegPeakFrame, vbInformation + vbSystemModal, getAppTitle())
                        rightLegAvgForce = getRightLegAvgForce(seatOffFrame, endSTSFrame)
                        MsgBox("Right Leg Avg Force: " & rightLegAvgForce, vbInformation + vbSystemModal, getAppTitle())
                        leftLegAvgForce = getLeftLegAvgForce(seatOffFrame, endSTSFrame)
                        MsgBox("Left Leg Avg Force: " & leftLegAvgForce, vbInformation + vbSystemModal, getAppTitle())
                        frmState = FORM_STATE.SAVE_TEST

                        ''Dim FileName As String = "C:\DAQ\test_run.txt"
                        ''Dim listsToWriteToFile As ArrayList = New ArrayList

                        ''listsToWriteToFile.Add("Right Leg Peak Frame: " & rightLegPeakFrame)
                        ''listsToWriteToFile.Add("Left Leg Peak Frame: " & leftLegPeakFrame)
                        ''listsToWriteToFile.Add("Right Leg Avg Force: " & rightLegAvgForce)
                        ''listsToWriteToFile.Add("Left Leg Avg Force: " & leftLegAvgForce)

                        ''IO.File.WriteAllLines(FileName, listsToWriteToFile)
                End Select
                'Else
                '    MsgBox(getDefaultErrorFormatting("selecting point on chart"), vbExclamation + vbSystemModal, getAppTitle())
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
            Dim theTickSize As MccDaq.CounterTickSize = MccDaq.CounterTickSize.Tick20pt83ns
            Dim ptr As IntPtr = MccDaq.MccService.WinBufAlloc32Ex(10000)

            ' Set up the Daq to scan the channels appropriately.
            For i = 1 To 6
                myDAQ.CConfigScan(i, MccDaq.CounterMode.StopAtMax, MccDaq.CounterDebounceTime.DebounceNone, MccDaq.CounterDebounceMode.TriggerAfterStable, MccDaq.CounterEdgeDetection.RisingEdge, theTickSize, vbNull)
            Next i

            timeCounter = 1
            initProgressBar()                       ' Initialize our progress bar (i.e. set min/max values and make it visible
            clkSamplingRate.Start()
        End If
    End Sub

    'Private Sub btnRunTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunTest.Click
    '    If (frmState = FORM_STATE.NEW_TEST) Then
    '        timeCounter = 1                         ' Initialize our timeCounter.
    '        clearLists()                            ' Clear our list of datapoints so we don't get inaccurate data.
    '        initProgressBar()                       ' Initialize our progress bar (i.e. set min/max values and make it visible).
    '        clkSamplingRate.Start()                 ' Finally, start our sampling timer.
    '    End If
    'End Sub

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

    Private Sub btnCancelTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelTest.Click
        frmState = FORM_STATE.NEW_TEST
    End Sub
End Class