Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Runtime.InteropServices
Imports System.IO

Public Class Form2

    Public Enum FORM_STATE As Integer
        NEW_TEST
        RUN_TEST
        SELECT_START_FRAME
        SELECT_END_FRAME
        SELECT_BILATERAL_FIRST_MINIMA
        SELECT_BILATERAL_PEAK
        SELECT_BILATERAL_SECOND_MINIMA
        SELECT_LEFT_LEG_FIRST_MINIMA
        SELECT_LEFT_LEG_PEAK
        SELECT_LEFT_LEG_SECOND_MINIMA
        SELECT_RIGHT_LEG_FIRST_MINIMA
        SELECT_RIGHT_LEG_PEAK
        SELECT_RIGHT_LEG_SECOND_MINIMA
        SELECT_LEFT_ARM_START
        SELECT_LEFT_ARM_END
        SELECT_LEFT_ARM_PEAK
        SELECT_RIGHT_ARM_START
        SELECT_RIGHT_ARM_END
        SELECT_RIGHT_ARM_PEAK
        SELECT_SEAT_OFF
        SHOW_POINTS
        SAVE_TEST
    End Enum

    Public frmState As Integer = FORM_STATE.NEW_TEST
    Public xStart As Double
    Public xEnd As Double
    Public yStart As Double
    Public yEnd As Double

    Public xchartcoord As Integer
    Public ychartcoord As Integer

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        frmState = FORM_STATE.NEW_TEST
        drawChart()
        samplingRate = 1000 / Me.clkSamplingRate.Interval
        Me.pgbTestStatus.Visible = False
        totalSamples = getTotalSamplesInTest()
    End Sub

    Dim myDAQ As New MccDaq.MccBoard(0)
    Dim voltageRange As MccDaq.Range = MccDaq.Range.Bip10Volts

    Dim timeCounter As Double
    Dim totalSamples As Double

    ''' <summary>
    ''' Sub that is to be called to draw the chart when the data is updated.
    ''' </summary>
    ''' <remarks>
    ''' All series are derived from the lists of data points (Legs, arms, seat), 
    ''' except for the listTimes list, which is created based on the 
    ''' length of the aforementioned lists.
    ''' </remarks>
    Private Sub drawChart()
        ' List of data points we got from the DAQ.
        Dim listOfDataPoints As New ArrayList

        ' List of series we are going to plot.
        Dim listOfSeries As New ArrayList

        ' List of colors we'll use to plot the chart.
        Dim colors As New ArrayList

        ' Declare our series to add to our chart.
        Dim seriesRightArm As New Series
        Dim seriesLeftArm As New Series
        Dim seriesRightLeg As New Series
        Dim seriesLeftLeg As New Series
        Dim seriesGround As New Series
        Dim seriesSeat As New Series
        Dim seriesBilateral As New Series

        ' Put our lists of points in a list (of lists).
        listOfDataPoints.Add(listRightArm)
        listOfDataPoints.Add(listLeftArm)
        listOfDataPoints.Add(listRightLeg)
        listOfDataPoints.Add(listLeftLeg)
        listOfDataPoints.Add(listGround)
        listOfDataPoints.Add(listSeat)
        listOfDataPoints.Add(listBilateral)

        ' Set the name appropriately for each series.
        seriesRightArm.Name = "Right Arm"
        seriesLeftArm.Name = "Left Arm"
        seriesRightLeg.Name = "Right Leg"
        seriesLeftLeg.Name = "Left Leg"
        seriesGround.Name = "Ground"
        seriesSeat.Name = "Seat"
        seriesBilateral.Name = "Bilateral"

        ' Add in our timing information.
        For i As Integer = 1 To listOfDataPoints(0).Count Step 1
            listTimes.Add(i)
        Next

        ' Add each series to our list of series.
        listOfSeries.Add(seriesRightArm)
        listOfSeries.Add(seriesLeftArm)
        listOfSeries.Add(seriesRightLeg)
        listOfSeries.Add(seriesLeftLeg)
        listOfSeries.Add(seriesGround)
        listOfSeries.Add(seriesSeat)
        listOfSeries.Add(seriesBilateral)

        ' Clear any previously charted information.
        STSChart.Series.Clear()

        ' Add each of the colors to our colors list (for displaying on the graph).
        ' NOTE: To change a color for a line, just change the color, 
        ' not the comment or the order in which the colors are added to the list.
        colors.Add(Color.Red)           ' Right Arm
        colors.Add(Color.Blue)          ' Left Arm
        colors.Add(Color.Orange)        ' Right Leg
        colors.Add(Color.Pink)          ' Left Leg
        colors.Add(Color.Green)         ' Ground
        colors.Add(Color.Black)         ' Seat
        colors.Add(Color.DarkMagenta)   ' Leg Bilateral (Right Leg + Left Leg)

        ' Iterate over each one of our lists.
        For lst As Integer = 0 To listOfSeries.Count - 1 Step 1

            ' Add all of the points to our series.
            For idx As Integer = 0 To listOfDataPoints(lst).Count - 1 Step 1
                ' Add a point from our listsTimes array and the curren idx 
                ' for the lst that we are adding points for.
                listOfSeries(lst).Points.AddXY(listTimes(idx), listOfDataPoints(lst)(idx))
            Next

            ' Add data series to our chart.
            STSChart.Series.Add(listOfSeries(lst))

            ' Format the data in our chart.
            With STSChart.Series(lst)                                             ' Select the current Series to format.
                .ChartType = DataVisualization.Charting.SeriesChartType.Line    ' Set the format for the series.
                .Color = colors(lst)                                            ' Set the color for the line.
                .BorderWidth = 3                                                ' Set the Line width.
            End With
        Next

    End Sub

    ''' <summary>
    ''' Sub that is triggered by a click event on the chart.
    ''' Used to input program variables to calculate various slopes and forces.
    ''' </summary>
    ''' <remarks>
    ''' @input xchartcoord
    ''' This variable is set in the selection function "Chart1_MouseMove"
    ''' </remarks>
    Private Sub STSChart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles STSChart.Click

        If (frmState >= FORM_STATE.SELECT_START_FRAME And frmState <= FORM_STATE.SELECT_SEAT_OFF) Then
            ' Now confirm the selection with the user.
            Select Case (frmState)
                Case FORM_STATE.SELECT_START_FRAME
                    If (confirmPoint("Start of Test")) Then
                        startSTSFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_END_FRAME
                        selectPoint("End of Test")
                    End If

                Case FORM_STATE.SELECT_END_FRAME
                    If (confirmPoint("End of Test")) Then
                        endSTSFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_BILATERAL_FIRST_MINIMA
                        selectPoint("Bilateral First Minima")
                    End If

                Case FORM_STATE.SELECT_BILATERAL_FIRST_MINIMA
                    If (confirmPoint("Bilateral First Minima")) Then
                        bilateralFirstMinimaFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_BILATERAL_PEAK
                        selectPoint("Bilateral Peak")
                    End If

                Case FORM_STATE.SELECT_BILATERAL_PEAK
                    If (confirmPoint("Bilateral Peak")) Then
                        bilateralPeakFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_BILATERAL_SECOND_MINIMA
                        selectPoint("Second Minima")
                    End If

                Case FORM_STATE.SELECT_BILATERAL_SECOND_MINIMA
                    If (confirmPoint("Bilateral Second Minima")) Then
                        bilateralSecondMinimaFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_LEFT_LEG_FIRST_MINIMA
                        selectPoint("Left Leg First Minima")
                    End If

                Case FORM_STATE.SELECT_LEFT_LEG_FIRST_MINIMA
                    If (confirmPoint("Left Leg First Minima")) Then
                        leftLegFirstMinimaFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_LEFT_LEG_PEAK
                        selectPoint("Left Leg Peak")
                    End If
                Case FORM_STATE.SELECT_LEFT_LEG_PEAK
                    If (confirmPoint("Left Leg Peak")) Then
                        leftLegPeakFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_LEFT_LEG_SECOND_MINIMA
                        selectPoint("Left Leg Second Minima")
                    End If
                Case FORM_STATE.SELECT_LEFT_LEG_SECOND_MINIMA
                    If (confirmPoint("Left Leg Second Minima")) Then
                        leftLegSecondMinimaFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_RIGHT_LEG_FIRST_MINIMA
                        selectPoint("Right Leg First Minima")
                    End If
                Case FORM_STATE.SELECT_RIGHT_LEG_FIRST_MINIMA
                    If (confirmPoint("Right Leg First Minima")) Then
                        rightLegFirstMinimaFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_RIGHT_LEG_PEAK
                        selectPoint("Right Leg Peak")
                    End If
                Case FORM_STATE.SELECT_RIGHT_LEG_PEAK
                    If (confirmPoint("Right Leg Peak")) Then
                        rightLegPeakFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_RIGHT_LEG_SECOND_MINIMA
                        selectPoint("Right Leg Second Minima")
                    End If
                Case FORM_STATE.SELECT_RIGHT_LEG_SECOND_MINIMA
                    If (confirmPoint("Right Leg Second Minima")) Then
                        rightLegSecondMinimaFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_LEFT_ARM_START
                        selectPoint("Left Arm Start")
                    End If
                Case FORM_STATE.SELECT_LEFT_ARM_START
                    If (confirmPoint("Left Arm Start")) Then
                        leftArmStartFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_LEFT_ARM_END
                        selectPoint("Left Arm End")
                    End If
                Case FORM_STATE.SELECT_LEFT_ARM_END
                    If (confirmPoint("Left Arm End")) Then
                        leftArmEndFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_LEFT_ARM_PEAK
                        selectPoint("Left Arm Peak")
                    End If
                Case FORM_STATE.SELECT_LEFT_ARM_PEAK
                    If (confirmPoint("Left Arm Peak")) Then
                        leftArmPeakFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_RIGHT_ARM_START
                        selectPoint("Right Arm Start")
                    End If
                Case FORM_STATE.SELECT_RIGHT_ARM_START
                    If (confirmPoint("Right Arm Start")) Then
                        rightArmStartFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_RIGHT_ARM_END
                        selectPoint("Right Arm End")
                    End If
                Case FORM_STATE.SELECT_RIGHT_ARM_END
                    If (confirmPoint("Right Arm End")) Then
                        rightArmEndFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_RIGHT_ARM_PEAK
                        selectPoint("Right Arm Peak")
                    End If
                Case FORM_STATE.SELECT_RIGHT_ARM_PEAK
                    If (confirmPoint("Right Arm Peak")) Then
                        rightArmPeakFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_SEAT_OFF
                        selectPoint("Seat Off")
                    End If
                Case FORM_STATE.SELECT_SEAT_OFF
                    If (confirmPoint("Seat Off")) Then
                        seatOffFrame = xchartcoord
                        frmState = FORM_STATE.SHOW_POINTS
                    End If
            End Select
        End If
    End Sub

    Private Function confirmPoint(ByVal pointName As String) As Boolean
        confirmPoint = MsgBox("Are you sure the point X=" & Me.xCoord.Text & " Y=" & Me.yCoord.Text & " is your desired " & pointName & " point?", vbYesNo + vbApplicationModal, getAppTitle()) = vbYes
    End Function

    Private Sub selectPoint(ByVal pointName As String)
        MsgBox("Select the " & pointName & " point for the test...", vbOKOnly + vbApplicationModal, getAppTitle())
    End Sub

    Private Sub STSChart_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles STSChart.MouseMove
        Dim result As HitTestResult = STSChart.HitTest(e.X, e.Y)
        Dim dp As DataPoint

        'Only trigger event if we are picking points, otherwise ignore, and reset values on our textboxes and what not.
        If (frmState >= FORM_STATE.SELECT_START_FRAME And frmState <= FORM_STATE.SAVE_TEST) Then

            ' If the click was successful, then figure out where it was.
            If result.PointIndex > 0 Then

                dp = STSChart.Series(0).Points(result.PointIndex)
                ToolTip1.SetToolTip(STSChart, "X:" & dp.XValue & " Y:" & dp.YValues(0))
                Me.xCoord.Text = dp.XValue
                Me.yCoord.Text = dp.YValues(0)
                xchartcoord = dp.XValue
                ychartcoord = dp.YValues(0)

                ' Now confirm the selection with the user.
                Select Case (frmState)
                    Case FORM_STATE.SELECT_START_FRAME
                    Case FORM_STATE.SELECT_END_FRAME
                    Case FORM_STATE.SELECT_BILATERAL_FIRST_MINIMA
                    Case FORM_STATE.SELECT_BILATERAL_PEAK
                    Case FORM_STATE.SELECT_BILATERAL_SECOND_MINIMA
                    Case FORM_STATE.SELECT_LEFT_LEG_FIRST_MINIMA
                    Case FORM_STATE.SELECT_LEFT_LEG_PEAK
                    Case FORM_STATE.SELECT_LEFT_LEG_SECOND_MINIMA
                    Case FORM_STATE.SELECT_RIGHT_LEG_FIRST_MINIMA
                    Case FORM_STATE.SELECT_RIGHT_LEG_PEAK
                    Case FORM_STATE.SELECT_RIGHT_LEG_SECOND_MINIMA
                    Case FORM_STATE.SELECT_LEFT_ARM_START
                    Case FORM_STATE.SELECT_LEFT_ARM_END
                    Case FORM_STATE.SELECT_LEFT_ARM_PEAK
                    Case FORM_STATE.SELECT_RIGHT_ARM_START
                    Case FORM_STATE.SELECT_RIGHT_ARM_END
                    Case FORM_STATE.SELECT_RIGHT_ARM_PEAK
                    Case FORM_STATE.SELECT_SEAT_OFF
                    Case FORM_STATE.SHOW_POINTS

                        'Now clip the data and set the time to percent STS for plotting
                        lengthSTS = endSTSFrame - startSTSFrame

                        takeDerivativesOfvGRF()
                        calculateLegDerivatives(bilateralFirstMinimaFrame, endSTSFrame)
                        'convertDataFromVoltagesToWeight()

                        MsgBox("Left Leg Peak Frame: " & leftLegPeakFrame, vbInformation + vbSystemModal, getAppTitle())
                        rightLegAvgForce = getRightLegAvgForce(seatOffFrame, endSTSFrame)
                        MsgBox("Right Leg Avg Force: " & rightLegAvgForce, vbInformation + vbSystemModal, getAppTitle())
                        leftLegAvgForce = getLeftLegAvgForce(seatOffFrame, endSTSFrame)
                        MsgBox("Left Leg Avg Force: " & leftLegAvgForce, vbInformation + vbSystemModal, getAppTitle())
                        frmState = FORM_STATE.SAVE_TEST
                End Select
            End If


        End If
    End Sub

    Private Sub btnCalibrateDevice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalibrateDevice.Click

        ' Only allow user to calibrate the device if we are ready to run a new test.
        If (frmState = FORM_STATE.NEW_TEST) Then

            runScanAndPopulateLists(False, 500, "calibrate.txt")

            rightArmOffset = getAvgFor(listRightArm)
            leftArmOffset = getAvgFor(listLeftArm)
            rightLegOffset = getAvgFor(listRightLeg)
            leftLegOffset = getAvgFor(listLeftLeg)
            seatOffset = getAvgFor(listSeat)
            groundOffset = getAvgFor(listGround)

            drawChart()
            MsgBox("Done")
        End If
    End Sub

    Private Sub btnRunTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunTest.Click
        If (frmState = FORM_STATE.NEW_TEST) Then

            runScanAndPopulateLists(True, 10000, "test.txt")

            drawChart()

            If (MsgBox("Would you like to select your own points?", vbYesNo, getAppTitle()) = vbYes) Then
                frmState = FORM_STATE.SELECT_START_FRAME
                selectPoint("Start of Test")
            Else
                frmState = FORM_STATE.SAVE_TEST
            End If
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim listDataPoints As ArrayList = getDataPoints()
        Dim listsToWriteToFile(listDataPoints.Count) As String
        Dim idx As Integer = 0

        ReDim listsToWriteToFile(listDataPoints(0).Count)

        ' Iterate over the data points.
        For i As Integer = 0 To listDataPoints(0).Count - 1 Step 1

            ' Write data points for time i at idx in our listsToWriteToFile string Array.
            For j As Integer = 0 To listDataPoints.Count - 1 Step 1
                listsToWriteToFile(idx) = listsToWriteToFile(idx) & listDataPoints(j)(i) & IIf(j + 1 = listDataPoints.Count, "", ",")
            Next

            ' Iterate our idx so we will write the next i sample time on a new line.
            idx += 1
        Next

        ' Save the Test to file.
        If (frmSaveTest.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            IO.File.WriteAllLines(frmSaveTest.FileName, listsToWriteToFile)
            MsgBox("Saved file!", vbOKOnly, getAppTitle())
        End If


        clearLists()
        frmState = FORM_STATE.NEW_TEST
    End Sub

    Private Sub runScanAndPopulateLists(ByVal applyOffsets As Boolean, ByVal totalPoints As Integer, ByVal fileName As String)
        Dim theTickSize As MccDaq.CounterTickSize = MccDaq.CounterTickSize.Tick20pt83ns
        Dim numberOfChannels As Integer = 6
        Dim ptr As IntPtr = MccDaq.MccService.WinBufAlloc32Ex(totalPoints * numberOfChannels)
        Dim sampleRate As Integer = 1000
        Dim er As MccDaq.ErrorInfo

        ' Run the scan. If we get an error, report it and cancel. 
        ' Otherwise continue to analyze the data.
        er = myDAQ.FileAInScan(0, numberOfChannels - 1, totalPoints * numberOfChannels, 1000, voltageRange, fileName, MccDaq.ScanOptions.Default)
        If (er.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors) Then
            MsgBox(er.Message, vbOKOnly + vbApplicationModal + vbExclamation, getAppTitle())
        Else
            readInSTSTest(applyOffsets, totalPoints, numberOfChannels, fileName)
        End If
    End Sub

    Private Sub readInSTSTest(ByVal applyOffsets As Boolean, ByVal totalPoints As Integer, ByVal numberOfChannels As Integer, ByVal fileName As String)

        ' Make sure the lists are clear before we gather data.
        clearLists()

        ' Open the file, and get a byte stream ready.
        Dim fStream As New FileStream(fileName, FileMode.Open, FileAccess.Read)
        Dim br As New BinaryReader(fStream)
        Dim value As UInt16

        Try

            ' Throw away first bit of crap
            br.ReadBytes(60)

            ' Read all values in the data file, putting each one 
            ' in its appropriate place.
            For i = 1 To (totalPoints * numberOfChannels)
                value = br.ReadUInt16()
                If (i Mod 6 = 0) Then ' Seat
                    If (applyOffsets) Then listSeat.Add(value - seatOffset) Else listSeat.Add(value)
                ElseIf (i Mod 6 = 1) Then ' Right Arm
                    If (applyOffsets) Then listRightArm.Add(value - rightArmOffset) Else listRightArm.Add(value)
                ElseIf (i Mod 6 = 2) Then ' Left Arm
                    If (applyOffsets) Then listLeftArm.Add(value - leftArmOffset) Else listLeftArm.Add(value)
                ElseIf (i Mod 6 = 3) Then ' Right Leg
                    If (applyOffsets) Then listRightLeg.Add(value - rightLegOffset) Else listRightLeg.Add(value)
                ElseIf (i Mod 6 = 4) Then ' Left Leg
                    If (applyOffsets) Then listLeftLeg.Add(value - leftLegOffset) Else listLeftLeg.Add(value)
                ElseIf (i Mod 6 = 5) Then ' Ground
                    If (applyOffsets) Then listGround.Add(value - groundOffset) Else listGround.Add(value)

                    ' Add Bilateral
                    listBilateral.Add(listRightLeg(listRightLeg.Count - 1) + listLeftLeg(listLeftLeg.Count - 1))
                End If
            Next i
        Catch e As Exception
            'MsgBox(e.Message, vbApplicationModal + vbExclamation, getAppTitle())
            fStream.Close()
            br.Close()
        End Try
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

    Private Sub btnCancelTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelTest.Click
        frmState = FORM_STATE.NEW_TEST
    End Sub

    Private Sub btnResetPickoff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetPickoff.Click
        readInSTSTest(True, 60000, 6, "test.txt")
        drawChart()
        frmState = FORM_STATE.SELECT_START_FRAME
        selectPoint("Start of Test")
    End Sub
End Class