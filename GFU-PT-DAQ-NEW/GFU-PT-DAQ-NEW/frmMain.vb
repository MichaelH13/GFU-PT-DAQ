Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Runtime.InteropServices
Imports System.IO

Public Class frmMain

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

    Dim myDAQ As New MccDaq.MccBoard(0)
    Dim voltageRange As MccDaq.Range = MccDaq.Range.Bip10Volts

    ''' <summary>
    ''' Initializes the form state, draws the chart, sets the sampling rate, 
    ''' hides the progress bar, and gets the total samples in the test.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        frmState = FORM_STATE.NEW_TEST
        drawChart()
        samplingRate = 1000 / Me.clkSamplingRate.Interval
        Me.pgbTestStatus.Visible = False
    End Sub

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
        Dim seriesSeat As New Series
        Dim seriesRightArm As New Series
        Dim seriesLeftArm As New Series
        Dim seriesRightLeg As New Series
        Dim seriesLeftLeg As New Series
        Dim seriesGround As New Series
        Dim seriesBilateral As New Series

        ' Put our lists of points in a list (of lists).
        listOfDataPoints.Add(listSeat)
        listOfDataPoints.Add(listRightArm)
        listOfDataPoints.Add(listLeftArm)
        listOfDataPoints.Add(listRightLeg)
        listOfDataPoints.Add(listLeftLeg)
        listOfDataPoints.Add(listGround)
        listOfDataPoints.Add(listBilateralLegs)

        ' Set the name appropriately for each series.
        seriesSeat.Name = "Seat"
        seriesRightArm.Name = "Right Arm"
        seriesLeftArm.Name = "Left Arm"
        seriesRightLeg.Name = "Right Leg"
        seriesLeftLeg.Name = "Left Leg"
        seriesGround.Name = "Ground"
        seriesBilateral.Name = "Bilateral"

        ' Make sure we have cleared our timing list before
        ' we refill it.
        listTimes.Clear()

        ' Add in our timing information.
        For i As Integer = 1 To listOfDataPoints(0).Count Step 1
            listTimes.Add(i)
        Next

        ' Add each series to our list of series.
        listOfSeries.Add(seriesSeat)
        listOfSeries.Add(seriesRightArm)
        listOfSeries.Add(seriesLeftArm)
        listOfSeries.Add(seriesRightLeg)
        listOfSeries.Add(seriesLeftLeg)
        listOfSeries.Add(seriesGround)
        listOfSeries.Add(seriesBilateral)

        ' Clear any previously charted information.
        STSChart.Series.Clear()

        ' Add each of the colors to our colors list (for displaying on the graph).
        ' NOTE: To change a color for a line, just change the color, 
        ' not the comment or the order in which the colors are added to the list.
        colors.Add(Color.Black)         ' Seat
        colors.Add(Color.Red)           ' Right Arm
        colors.Add(Color.Blue)          ' Left Arm
        colors.Add(Color.Orange)        ' Right Leg
        colors.Add(Color.Pink)          ' Left Leg
        colors.Add(Color.Green)         ' Ground
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
            With STSChart.Series(lst)                                           ' Select the current Series to format.
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
                        bilateralLegsFirstMinimaFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_BILATERAL_PEAK
                        selectPoint("Bilateral Peak")
                    End If

                Case FORM_STATE.SELECT_BILATERAL_PEAK
                    If (confirmPoint("Bilateral Peak")) Then
                        bilateralLegsPeakFrame = xchartcoord
                        frmState = FORM_STATE.SELECT_BILATERAL_SECOND_MINIMA
                        selectPoint("Second Minima")
                    End If

                Case FORM_STATE.SELECT_BILATERAL_SECOND_MINIMA
                    If (confirmPoint("Bilateral Second Minima")) Then
                        bilateralLegsSecondMinimaFrame = xchartcoord
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

                        viewOutput()

                        frmState = FORM_STATE.SAVE_TEST
                    End If
            End Select
        End If
    End Sub

    ''' <summary>
    ''' Displays form to view program variable output from the 
    ''' last complete STS test.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub viewOutput()

        '******************************************************
        ' Now that we are ready to show the points, calculate
        ' them and then display them to the user.
        '******************************************************

        ' Then get averages, areas, and slopes.
        leftLegAvgForce = getLeftLegAvgForce()
        leftLegAreaSeatOffToEnd = getLeftLegAreaSeatOffToEnd()
        leftLeg25To50Slope = getLeftLeg25To50Slope()
        leftLegSlope = getLeftLegSlope()
        leftLegAreaSeatOffToEnd = getLeftLegAreaSeatOffToEnd()
        leftLegSeatOffForceValue = listLeftLeg(seatOffFrame)
        leftLegPeakForceValue = listLeftLeg(leftLegPeakFrame)

        rightLegAvgForce = getRightLegAvgForce()
        rightLegAreaSeatOffToEnd = getRightLegAreaSeatOffToEnd()
        rightLeg25To50Slope = getRightLeg25To50Slope()
        rightLegSlope = getRightLegSlope()
        rightLegAreaSeatOffToEnd = getRightLegAreaSeatOffToEnd()
        rightLegSeatOffForceValue = listRightLeg(seatOffFrame)
        rightLegPeakForceValue = listRightLeg(rightLegPeakFrame)

        bilateralLegs25To50Slope = getBilateralLegs25To50Slope()
        bilateralLegsSlope = getBilateralLegsSlope()
        bilateralLegsAreaSeatOffToEnd = getBilateralLegsAreaSeatOffToEnd()
        bilateralLegsAverageSeatOffToEnd = getBilateralLegsAverageSeatOffToEnd()

        bilateralLegsSeatOffForceValue = listBilateralLegs(seatOffFrame)
        bilateralLegsPeakForceValue = listBilateralLegs(bilateralLegsPeakFrame)

        leftArmArea = getLeftArmArea()
        leftArmSeatOffForceValue = listLeftArm(seatOffFrame)
        leftArmPeakForceValue = listLeftArm(leftArmPeakFrame)

        rightArmArea = getRightArmArea()
        rightArmSeatOffForceValue = listRightArm(seatOffFrame)
        rightArmPeakForceValue = listRightArm(rightArmPeakFrame)


        Dim viewOutput As New frmViewOutputVariables

        ' Write Left Leg Variables out to the viewing form.
        viewOutput.txtLeftLegFirstMinima.Text = leftLegFirstMinimaFrame
        viewOutput.txtLeftLegPeakFrame.Text = leftLegPeakFrame
        viewOutput.txtLeftLegSecondMinima.Text = leftLegSecondMinimaFrame
        viewOutput.txtLeftLegAvgForceSeatOffToEndOfSTS.Text = Math.Round(leftLegAvgForce, 2)

        viewOutput.txtLeftLeg25_50Slope.Text = Math.Round(leftLeg25To50Slope, 2)
        viewOutput.txtLeftLegSlope.Text = Math.Round(leftLegSlope, 2)
        viewOutput.txtLeftLegAreaSeatOffToEndOfSTS.Text = Math.Round(leftLegAreaSeatOffToEnd, 2)
        viewOutput.txtLeftLegSeatOffForce.Text = Math.Round(leftLegSeatOffForceValue, 2)
        viewOutput.txtLeftLegPeakForce.Text = Math.Round(leftLegPeakForceValue, 2)

        ' Write Right Leg Variables out to the viewing form.
        viewOutput.txtRightLegFirstMinima.Text = rightLegFirstMinimaFrame
        viewOutput.txtRightLegPeakFrame.Text = rightLegPeakFrame
        viewOutput.txtRightLegSecondMinima.Text = rightLegSecondMinimaFrame
        viewOutput.txtRightLegAvgForceSeatOffToEndOfSTS.Text = Math.Round(rightLegAvgForce, 2)

        viewOutput.txtRightLeg25_50Slope.Text = Math.Round(rightLeg25To50Slope, 2)
        viewOutput.txtRightLegSlope.Text = Math.Round(rightLegSlope, 2)
        viewOutput.txtRightLegAreaSeatOffToEndOfSTS.Text = Math.Round(rightLegAreaSeatOffToEnd, 2)
        viewOutput.txtRightLegSeatOffForce.Text = Math.Round(rightLegSeatOffForceValue, 2)
        viewOutput.txtRightLegPeakForce.Text = Math.Round(rightLegPeakForceValue, 2)

        ' Write Left Arm Variables out to the viewing form.
        viewOutput.txtLeftArmStartFrame.Text = leftArmStartFrame
        viewOutput.txtLeftArmPeakFrame.Text = leftArmPeakFrame
        viewOutput.txtLeftArmEndFrame.Text = leftArmEndFrame

        viewOutput.txtLeftArmArea.Text = Math.Round(leftArmArea, 2)
        viewOutput.txtLeftArmPeakForce.Text = Math.Round(leftArmPeakForceValue, 2)
        viewOutput.txtLeftArmSeatOffForce.Text = Math.Round(leftArmSeatOffForceValue, 2)

        ' Write Right Arm Variables out to the viewing form.
        viewOutput.txtRightArmStartFrame.Text = rightArmStartFrame
        viewOutput.txtRightArmPeakFrame.Text = rightArmPeakFrame
        viewOutput.txtRightArmEndFrame.Text = rightArmEndFrame

        viewOutput.txtRightArmArea.Text = Math.Round(rightArmArea, 2)
        viewOutput.txtRightArmPeakForce.Text = Math.Round(rightArmPeakForceValue, 2)
        viewOutput.txtRightArmSeatOffForce.Text = Math.Round(rightArmSeatOffForceValue, 2)

        ' Write Bilateral Leg Variables out to the viewing form.
        viewOutput.txtBilateralLegsStartFrame.Text = startSTSFrame
        viewOutput.txtBilateralLegsPeakFrame.Text = bilateralLegsPeakFrame
        viewOutput.txtBilateralLegsEndFrame.Text = endSTSFrame
        viewOutput.txtBilateralLegsFirstMinima.Text = bilateralLegsFirstMinimaFrame
        viewOutput.txtBilateralLegsSecondMinima.Text = bilateralLegsSecondMinimaFrame
        viewOutput.txtSeatOffFrame.Text = seatOffFrame

        viewOutput.txtBilateralLegs25_50Slope.Text = Math.Round(bilateralLegs25To50Slope, 2)
        viewOutput.txtBilateralLegsSlope.Text = Math.Round(bilateralLegsSlope, 2)
        viewOutput.txtBilateralLegsAreaSeatOffToEndOfSTS.Text = Math.Round(bilateralLegsAreaSeatOffToEnd, 2)
        viewOutput.txtBilateralLegsAvgForceSeatOffToEndOfSTS.Text = Math.Round(bilateralLegsAverageSeatOffToEnd, 2)
        viewOutput.txtBilateralLegsSeatOffForce.Text = Math.Round(bilateralLegsSeatOffForceValue, 2)
        viewOutput.txtBilateralLegsPeakForce.Text = Math.Round(bilateralLegsPeakForceValue, 2)

        viewOutput.txtTotalFramesInTest.Text = (endSTSFrame - startSTSFrame)

        ' Finally, show the form to the user.
        viewOutput.Show()
    End Sub

    ''' <summary>
    ''' Short function to confirm the user's point choice.
    ''' </summary>
    ''' <param name="pointName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function confirmPoint(ByVal pointName As String) As Boolean
        confirmPoint = MsgBox("Are you sure the point X = " & Me.xCoord.Text & " is your desired " & pointName & " point?", vbYesNo + vbApplicationModal, getAppTitle()) = vbYes
    End Function

    ''' <summary>
    ''' Short function to create a general message box for selecting a point.
    ''' </summary>
    ''' <param name="pointName"></param>
    ''' <remarks></remarks>
    Private Sub selectPoint(ByVal pointName As String)
        MsgBox("Select the " & pointName & " point for the test...", vbOKOnly + vbApplicationModal, getAppTitle())
    End Sub

    ''' <summary>
    ''' Triggered when the mouse moves on the chart.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub STSChart_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles STSChart.MouseMove
        Dim result As HitTestResult = STSChart.HitTest(e.X, e.Y)
        Dim dp As DataPoint

        'Only trigger event if we are picking points, otherwise ignore, and reset values on our textboxes and what not.
        If (frmState >= FORM_STATE.SELECT_START_FRAME And frmState <= FORM_STATE.SAVE_TEST) Then

            ' If the click was successful, then figure out where it was.
            If result.PointIndex > 0 Then

                dp = STSChart.Series(0).Points(result.PointIndex)

                ' Show both X and Y coordinates for the first series.
                'ToolTip1.SetToolTip(STSChart, "X:" & dp.XValue & " Y:" & dp.YValues(0))

                ' Show just the X coordinate for any given series.
                chartSTSToolTip.SetToolTip(STSChart, "X: " & dp.XValue)

                Me.xCoord.Text = dp.XValue
                Me.yCoord.Text = dp.YValues(0)
                xchartcoord = dp.XValue
                ychartcoord = dp.YValues(0)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Subroutine to handle when the user clicks the "Calibrate" button.
    ''' We save off sets for each known channel to be applied when we 
    ''' read in the values of a new test.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCalibrateDevice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalibrateDevice.Click

        ' Only allow user to calibrate the device if we are ready to run a new test.
        If (frmState = FORM_STATE.NEW_TEST) Then

            runScanAndPopulateLists(False, 500, 6, "calibrate.txt")

            rightArmOffset = getAverageForList(listRightArm)
            leftArmOffset = getAverageForList(listLeftArm)
            rightLegOffset = getAverageForList(listRightLeg)
            leftLegOffset = getAverageForList(listLeftLeg)
            seatOffset = getAverageForList(listSeat)
            groundOffset = getAverageForList(listGround)

            drawChart()
            MsgBox("Done")
        End If
    End Sub

    ''' <summary>
    ''' Subroutine to run a new STS test.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRunTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunTest.Click
        If (frmState = FORM_STATE.NEW_TEST) Then

            runScanAndPopulateLists(True, 10000, 6, "test.txt")

            drawChart()

            If (MsgBox("Would you like to select your own points?", vbYesNo, getAppTitle()) = vbYes) Then
                frmState = FORM_STATE.SELECT_START_FRAME
                selectPoint("Start of Test")
            Else
                frmState = FORM_STATE.NEW_TEST
            End If
        Else
            MsgBox("Please click 'Cancel Test' to reset the software and begin a new test.", vbApplicationModal + vbOKOnly, getAppTitle())
        End If
    End Sub

    ''' <summary>
    ''' Saves the most recent test to a file that the user specifies.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim listDataPoints As ArrayList = getDataPoints()
        Dim listsToWriteToFile(listDataPoints(1).Count + 1) As String
        Dim idx As Integer = 0

        listDataPoints(0).clear()

        For i = 0 To listDataPoints(1).Count - 1
            listDataPoints(0).add(i)
        Next

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
            IO.File.WriteAllLines(frmSaveTest.FileName, {"Milliseconds,Right Arm,Left Arm,Right Leg,Left Leg,Ground,Seat,Bilateral Legs"})
            IO.File.AppendAllLines(frmSaveTest.FileName, listsToWriteToFile)
            MsgBox("Saved file!", vbOKOnly, getAppTitle())
        End If

        clearLists()
        frmState = FORM_STATE.NEW_TEST
    End Sub

    ''' <summary>
    ''' Runs a scan using the DAQ and populate the lists of values 
    ''' for each channel.
    ''' </summary>
    ''' <param name="isCalibrated"></param>
    ''' Should be true if we have calibrated the device already, 
    ''' false if we are not calibrated yet.
    ''' 
    ''' <param name="totalPoints"></param>
    ''' The total number of data points we need to get for EACH 
    ''' channel in numberOfChannels.
    ''' 
    ''' <param name="numberOfChannels"></param>
    ''' The total number of channels we are reading in from 
    ''' (starting at channel 0 and going to channel (numberOfChannels - 1)).
    ''' 
    ''' <param name="fileName"></param>
    ''' The name of the file we need to read the data in from.
    ''' <remarks></remarks>
    Private Sub runScanAndPopulateLists(ByVal isCalibrated As Boolean, ByVal totalPoints As Integer, ByVal numberOfChannels As Integer, ByVal fileName As String)
        Dim theTickSize As MccDaq.CounterTickSize = MccDaq.CounterTickSize.Tick20pt83ns
        Dim ptr As IntPtr = MccDaq.MccService.WinBufAlloc32Ex(totalPoints * numberOfChannels)
        Dim sampleRate As Integer = 1000
        Dim er As MccDaq.ErrorInfo

        ' Run the scan. If we get an error, report it and cancel. 
        ' Otherwise continue to analyze the data.
        er = myDAQ.FileAInScan(0, numberOfChannels - 1, totalPoints * numberOfChannels, 1000, voltageRange, fileName, MccDaq.ScanOptions.Default)
        If (er.Value <> MccDaq.ErrorInfo.ErrorCode.NoErrors) Then
            MsgBox(er.Message, vbOKOnly + vbApplicationModal + vbExclamation, getAppTitle())
        Else
            readInSTSTest(isCalibrated, totalPoints, numberOfChannels, fileName)
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' This function reads in raw 16-bit data values as generated from 
    ''' the <see cref="runScanAndPopulateLists"></see> function. If the 
    ''' <paramref name="totalPoints"></paramref> is equal to 10000, then 
    ''' this function will convert the voltages to Newtons as determined
    ''' by the <see cref="convertDataFromVoltagesToWeight"></see> function.
    ''' 
    ''' </summary>
    ''' <param name="isCalibrated"></param>
    ''' Should be true if we have calibrated the device already, 
    ''' false if we are not calibrated yet.
    ''' 
    ''' <param name="totalPoints"></param>
    ''' The total number of data points we need to get for EACH 
    ''' channel in numberOfChannels.
    ''' 
    ''' <param name="numberOfChannels"></param>
    ''' The total number of channels we are reading in from 
    ''' (starting at channel 0 and going to channel (numberOfChannels - 1)).
    ''' 
    ''' <param name="fileName"></param>
    ''' The name of the file we need to read the data in from.
    ''' 
    ''' <remarks></remarks>
    Private Function readInSTSTest(ByVal isCalibrated As Boolean, ByVal totalPoints As Integer, ByVal numberOfChannels As Integer, ByVal fileName As String) As Boolean

        ' Make sure the lists are clear before we gather data.
        clearLists()

        ' Open the file, and get a byte stream ready.
        Dim fStream As New FileStream(fileName, FileMode.Open, FileAccess.Read)
        Dim br As New BinaryReader(fStream)
        Dim value As UInt16

        Try

            ' Throw away first bit of crap. This is just the file header that
            ' the DAQ generates. It contains date information, which we don't
            ' actually care about.
            br.ReadBytes(60)

            '**************************************************************
            ' Summary of how we determine to run the loop:
            '**************************************************************
            ' We multiply the totalPoints by the numberOfChannels to get 
            ' the total data points in the test. Then we iterate i from
            ' 1 to the total number of points in the test itself to get
            ' each sample as described below.
            '**************************************************************
            For i = 1 To (totalPoints * numberOfChannels)

                ' Read in the next unsigned-16-bit (2 byte) value.
                value = br.ReadUInt16()

                '**************************************************************
                ' Summary of how the voltages are read in from the file:
                '**************************************************************
                ' 1) For each i, we'll have read in a new 2 byte value.
                ' So, we'll be checking to see what the remainder is
                ' of the current i divided by numberOfChannels (which is 
                ' accomplished by using "i Mod numberOfChannels = n").
                ' 
                ' 2) The general method of getting each voltage is to check if
                ' we have calibrated the device, then to add the raw value if
                ' we have NOT calibrated the device yet 
                ' (i.e isCalibrated = false) or add the raw value minus the 
                ' calibration offset (which is just the average offset from 0
                ' that the channel generates when running a calibration cycle
                ' where no force is applied to any part of the device).
                '
                ' 3) If we encounter an error, we'll be sure to close our file
                ' and carefully exit, and inform the user that an error occured
                ' while reading in the file.
                '
                ' 4) Final note: if we want to add more channels later, we just 
                ' have to add another if() statement below to include 
                ' i Mod numberOfChannels = numberOfChannels - 1
                ' Such that we add the points to a list that can be manipulated
                ' by the rest of the program.
                '**************************************************************
                If (i Mod numberOfChannels = 0) Then ' Seat
                    ' NOTE: we are using a division by 8 to scale down relative to the rest of the graph,
                    ' while still being able to pickoff the seat off point.
                    If (isCalibrated) Then listSeat.Add((value - seatOffset) / 8) Else listSeat.Add(value)
                ElseIf (i Mod numberOfChannels = 1) Then ' Right Arm
                    If (isCalibrated) Then listRightArm.Add(value - rightArmOffset) Else listRightArm.Add(value)
                ElseIf (i Mod numberOfChannels = 2) Then ' Left Arm
                    If (isCalibrated) Then listLeftArm.Add(value - leftArmOffset) Else listLeftArm.Add(value)
                ElseIf (i Mod numberOfChannels = 3) Then ' Right Leg
                    If (isCalibrated) Then listRightLeg.Add(value - rightLegOffset) Else listRightLeg.Add(value)
                ElseIf (i Mod numberOfChannels = 4) Then ' Left Leg
                    If (isCalibrated) Then listLeftLeg.Add(value - leftLegOffset) Else listLeftLeg.Add(value)
                ElseIf (i Mod numberOfChannels = 5) Then ' Ground
                    If (isCalibrated) Then listGround.Add(value - groundOffset) Else listGround.Add(value)

                    ' Add Bilateral
                    listBilateralLegs.Add(listRightLeg(listRightLeg.Count - 1) + listLeftLeg(listLeftLeg.Count - 1))
                End If
            Next i
        Catch e As Exception
            ' We encountered some error, so close our IO streams, 
            ' return false, and exit.
            fStream.Close()
            br.Close()
            readInSTSTest = False
            Exit Function
        End Try

        ' If we are doing a 10-second STS test, then convert the voltages to Newtons.
        If (totalPoints = 10000) Then convertDataFromVoltagesToWeight()
        readInSTSTest = True
    End Function

    ''' <summary>
    ''' Clears all arraylists we use to keep track of our datapoints.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub clearLists()
        listTimes.Clear()
        listRightArm.Clear()
        listLeftArm.Clear()
        listRightLeg.Clear()
        listLeftLeg.Clear()
        listGround.Clear()
        listSeat.Clear()
        listBilateralLegs.Clear()
    End Sub

    ''' <summary>
    ''' Function to initialize our progress bar to run a test.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub initializeProgressBar(ByVal totalMilliseconds)
        Me.pgbTestStatus.Minimum = 0
        Me.pgbTestStatus.Maximum = totalMilliseconds
        Me.pgbTestStatus.Value = 0
        Me.pgbTestStatus.Step = 1
        Me.pgbTestStatus.Visible = True
    End Sub

    Private Sub btnCancelTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelTest.Click
        frmState = FORM_STATE.NEW_TEST
    End Sub

    Private Sub btnReloadLastTestToPickoff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetPickoff.Click
        readInSTSTest(True, 10000, 6, "test.txt")
        drawChart()
        frmState = FORM_STATE.SELECT_START_FRAME
        selectPoint("Start of Test")
    End Sub

    Private Sub btnViewVariables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewVariables.Click
        viewOutput()
    End Sub

    Private Sub btnClipData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClipData.Click
        Dim listDataPoints As ArrayList = getDataPoints()

        listDataPoints(0).clear()

        For i = 0 To listDataPoints(1).Count - 1
            listDataPoints(0).add(i)
        Next

        removeRangeFromLists(listDataPoints, endSTSFrame, (10000 - endSTSFrame))
        removeRangeFromLists(listDataPoints, 0, startSTSFrame)
        drawChart()
        frmState = FORM_STATE.SELECT_START_FRAME
        selectPoint("Start of Test")
    End Sub

    Private Sub btnImportOld_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportOld.Click

        Dim fileContents() As String = Nothing
        Dim currentLine() As String

        ' If the user selects a file, then read the file in.
        If (frmLoadOldTest.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then

            Try
                fileContents = IO.File.ReadAllLines(frmLoadOldTest.FileName)
            Catch ex As Exception
                MsgBox("Error: File may be missing or corrupted. Please verify the file and try again.", vbOKOnly, getAppTitle())
            End Try

            If (Not fileContents Is Nothing) Then
                clearLists()

                For i = 8 To fileContents.Count - 1

                    currentLine = fileContents(i).Split(",")

                    If (currentLine.Count < 8) Then
                        If (i <> fileContents.Count - 1) Then MsgBox("Error: File is formatted incorrectly!", vbApplicationModal + vbExclamation + vbOKOnly, getAppTitle())
                        Exit For
                    End If

                    listTimes.Add(currentLine(0))
                    ' Skip index 1 (one) because its just Date/Time info.
                    listRightArm.Add(CDbl(currentLine(2)))
                    listLeftArm.Add(CDbl(currentLine(3)))
                    listRightLeg.Add(CDbl(currentLine(4)))
                    listLeftLeg.Add(CDbl(currentLine(5)))
                    listGround.Add(CDbl(currentLine(6)))
                    listSeat.Add(CDbl(currentLine(7)))
                    listBilateralLegs.Add(listRightLeg(listRightLeg.Count - 1) + listLeftLeg(listLeftLeg.Count - 1))
                Next
                convertDataFromVoltagesToWeight(True)
                drawChart()
                MsgBox("Test imported successfully!", vbOKOnly, getAppTitle())
                frmState = FORM_STATE.SELECT_START_FRAME
                selectPoint("Start of Test")
            Else
                MsgBox("Error: File may be formatted incorrectly. Please verify the file and try again.", vbOKOnly, getAppTitle())
            End If
        Else
            MsgBox("Error: File may be missing, corrupted, or formatted incorrectly. Please verify the file and try again.", vbOKOnly, getAppTitle())
        End If
    End Sub

    Private Sub btnImportNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportNew.Click

        Dim fileContents() As String = Nothing
        Dim currentLine() As String

        ' If the user selects a file, then read the file in.
        If (frmLoadOldTest.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then

            Try
                fileContents = IO.File.ReadAllLines(frmLoadOldTest.FileName)
            Catch ex As Exception
                MsgBox("Error: File may be missing or corrupted. Please verify the file and try again.", vbOKOnly, getAppTitle())
            End Try

            If (Not fileContents Is Nothing) Then
                
                clearLists()

                For i = 1 To fileContents.Count - 3

                    currentLine = fileContents(i).Split(",")

                    If (currentLine.Count <> 8) Then
                        If (i <> fileContents.Count - 3) Then MsgBox("Error: Line in file is formatted incorrectly! " & i, vbApplicationModal + vbExclamation + vbOKOnly, getAppTitle())
                        Exit For
                    End If

                    listTimes.Add(currentLine(0))
                    listRightArm.Add(CDbl(currentLine(1)))
                    listLeftArm.Add(CDbl(currentLine(2)))
                    listRightLeg.Add(CDbl(currentLine(3)))
                    listLeftLeg.Add(CDbl(currentLine(4)))
                    listGround.Add(CDbl(currentLine(5)))
                    listSeat.Add(CDbl(currentLine(6)))

                    listBilateralLegs.Add(listRightLeg(listRightLeg.Count - 1) + listLeftLeg(listLeftLeg.Count - 1))
                Next

                convertDataFromVoltagesToWeight()

                drawChart()

                MsgBox("Test imported successfully!", vbOKOnly, getAppTitle())

                frmState = FORM_STATE.SELECT_START_FRAME
                selectPoint("Start of Test")

            Else
                MsgBox("Error: File may be formatted incorrectly. Please verify the file and try again.", vbOKOnly, getAppTitle())
            End If
        Else
            MsgBox("Error: File may be missing, corrupted, or formatted incorrectly. Please verify the file and try again.", vbOKOnly, getAppTitle())
        End If
    End Sub
End Class