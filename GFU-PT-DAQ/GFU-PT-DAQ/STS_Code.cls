Public Class STS_Code

    Public Const INVALID_FRAME = -1

    ' No idea what these are for.
    Dim MultiUse = True  'True
    Dim VB_Name = "Sheet1"
    Dim VB_GlobalNameSpace = False
    Dim VB_Creatable = False
    Dim VB_PredeclaredId = True
    Dim VB_Exposed = True
    ' Program variables, public for now.
    ' These will be renamed to use equivalent ArrayList later.
    Dim TimeArray(0 To 10000) As Single
    Dim RFArray(0 To 10000) As Single
    Dim LFArray(0 To 10000) As Single
    Dim RAArray(0 To 10000) As Single
    Dim LAArray(0 To 10000) As Single
    Dim SeatArray(0 To 10000) As Single
    Dim vGRFBilatArray(0 To 10000) As Single ' RFArray + LFArray
    Dim ArmsBilatArray(0 To 10000) As Single ' RAArray + LAArray

    ' We will not need these once the application is completed.
    Dim NewFileName As String
    'Dim ws As Worksheet
    'Dim ChartData As Chart

    Dim vGRFDeriv(0 To 10000) As Double
    Dim ArmsvGRFsum(0 To 10000) As Single
    Dim ArmsvGRFDeriv(0 To 10000) As Single
    Dim RFDeriv(0 To 10000) As Single
    Dim LFDeriv(0 To 10000) As Single
    Dim RADeriv(0 To 10000) As Single
    Dim LADeriv(0 To 10000) As Single
    Dim PerSTS(0 To 10000) As Single
    Dim RFArray_v(0 To 10000) As Single
    Dim LFArray_v(0 To 10000) As Single
    Dim RAArray_v(0 To 10000) As Single
    Dim LAArray_v(0 To 10000) As Single
    Dim Start50 As Integer
    Dim Finish50 As Integer
    Dim FirstPeak As Single
    Dim SecondPeak As Single
    Dim ThridPeak As Single
    Dim LFPeakForce As Single
    Dim RFPeakForce As Single
    Dim RAPeakFOrce, LAPeakForce As Single
    Dim RDeltaTime25_50 As Single
    Dim LDeltaTime25_50 As Single
    Dim RDeltaForce As Single
    Dim LDeltaForce As Single
    Dim Msg, Style, Title, Response, MyString



    Public Sub AnalyzeSTSData()
        'This first loop gets the entire program to loop
        'determine the number of loops
        Dim NoLoops As Integer
        Dim q As Integer
        NoLoops = InputBox("Number of trials to be processed")
        q = 0

        For q = 1 To NoLoops

            'get standard deviation from 9000 to 9500 frames of seat off data
            ' @TODO We may need this, but I'm not sure yet...
            'Workbooks(SubjectName).Sheets(1).Cells(1, 8).Formula = "=StDev(h9009:h9509)"
            'SDSeat = Workbooks(SubjectName).Sheets(1).Cells(1, 8).Value

            'Find the seat ref
            Dim s = 0
            Dim count = 0
            Dim seatoffFrame = 0
            Dim Seatoff = 0
            Dim EndSTS = 0
            Dim EndSTSFrame = 0
            Dim StartSTSFrame = 0
            Dim Manual = 0
            Dim StartSTS = 0
            Dim RAon = 0
            Dim RAoff = 0
            Dim LAon = 0
            Dim LAoff = 0
            Dim RAtotal = 0
            Dim LAtotal = 0
            Dim i = 0
            Dim RAOffset = 0
            Dim LAOffset = 0
            Dim LengthSTS = 0
            Dim g = 0
            Dim NewName = 0
            Dim SubjectName = 0
            Dim a = 0
            Dim j = 0
            Dim FirstPeakFrame = 0
            Dim ThirdPeakFrame = 0
            Dim RFPeakFrame = 0
            Dim LFPeakFrame = 0
            Dim RFTotal = 0
            Dim LFTotal = 0
            Dim RFAverage = 0
            Dim LFAverage = 0
            Dim RFArea = 0
            Dim LFArea = 0
            Dim Start25 = 0
            Dim RFSlope25_50 = 0
            Dim LFSlope25_50 = 0
            Dim RFSlope100 = 0
            Dim LFSlope100 = 0
            Dim RFFinish50 = 0
            Dim LFFinish50 = 0
            Dim RFStart25 = 0
            Dim LFStart25 = 0
            Dim DeltaTime100 = 0
            Dim BilatSlope100 = 0
            Dim ArmsIndicator = 0
            Dim RAPeakForceFrame = 0
            Dim LAPeakForceFrame = 0

            ' This section grabs the peak data from the trials
            ' we start with the same process as for the bilateral data which is used to detect the global events
            ' So we will take the derivative of each foot and arm and then get are points
            ' we start with the rising phase then do the preparation phase variables

            'Here we calculate the preparation phase variables
            'now we are going to get specific points and data
            Start25 = 0
            Finish50 = 0
            RFSlope25_50 = 0
            LFSlope25_50 = 0
            RFSlope100 = 0
            LFSlope100 = 0

            i = 0
            For i = FirstPeakFrame To RFPeakFrame
                If RFArray(i) > (RFArray(RFPeakFrame) * 0.5) Then
                    RFFinish50 = i
                    GoTo 80
                End If
            Next
80:
            i = 0
            For i = FirstPeakFrame To LFPeakFrame
                If LFArray(i) > (LFArray(LFPeakFrame) * 0.5) Then
                    LFFinish50 = i
                    GoTo 81
                End If
            Next
81:

            i = 0
            For i = FirstPeakFrame To RFPeakFrame
                If RFArray(i) > (RFArray(RFPeakFrame) * 0.25) Then
                    RFStart25 = i
                    GoTo 82
                End If
            Next
82:
            i = 0
            For i = FirstPeakFrame To LFPeakFrame
                If LFArray(i) > (LFArray(LFPeakFrame) * 0.25) Then
                    LFStart25 = i
                    GoTo 83
                End If
            Next
83:

            'Start25 = FirstPeakFrame + (SeatOffFrame - FirstPeakFrame) * 0.25
            'Finish50 = FirstPeakFrame + (SeatOffFrame - FirstPeakFrame) * 0.5

            'Calculate the slopes
            RDeltaTime25_50 = (RFFinish50 - RFStart25) / 1000
            LDeltaTime25_50 = (LFFinish50 - LFStart25) / 1000
            RDeltaForce = RFArray(RFFinish50) - RFArray(RFStart25)
            LDeltaForce = LFArray(LFFinish50) - LFArray(LFStart25)

            RFSlope25_50 = RDeltaForce / RDeltaTime25_50
            LFSlope25_50 = LDeltaForce / LDeltaTime25_50

            DeltaTime100 = (seatoffFrame - FirstPeakFrame) / 1000

            RFSlope100 = (RFArray(seatoffFrame) - RFArray(StartSTSFrame)) / DeltaTime100
            LFSlope100 = (LFArray(seatoffFrame) - LFArray(StartSTSFrame)) / DeltaTime100
            BilatSlope100 = (vGRFBilatArray(seatoffFrame) - vGRFBilatArray(StartSTSFrame)) / DeltaTime100

            'Calculate area for arms
            'find start of arms
            'arms ref
            If Manual = 1 Then GoTo 50
            If ArmsIndicator = 1 Then
                s = 0
                For s = StartSTSFrame To EndSTSFrame
                    If RAArray(s) >= 10 Then
                        RAon = s
                        GoTo 20
                    End If
                Next

20:
                s = 0
                For s = StartSTSFrame To 10000
                    If RAArray(10000 - (RAon + s)) >= 10 Then
                        RAoff = (10000 - (s + RAon))
                        GoTo 21
                    End If
                Next

21:
                s = 0
                For s = StartSTSFrame To EndSTSFrame
                    If LAArray(s) >= 10 Then
                        LAon = s
                        GoTo 22
                    End If
                Next
22:
                s = 0
                For s = StartSTSFrame To 10000
                    If LAArray(10000 - (LAon + s)) >= 10 Then
                        LAoff = (10000 - (LAon + s))
                        GoTo 23
                    End If
                Next
23:
                'Calculate area for arms
                i = 0
                RAArea = 0
                For i = RAon To RAoff
                    RAArea = (RAArray(i) * 0.001) + RAArea
                Next

                i = 0
                LAArea = 0
                For i = LAon To LAoff
                    LAArea = (LAArray(i) * 0.001) + LAArea
                Next

                'peak force and average from each ARM from seat off to end of STS
                'Find first peak
                'first take derivative
                For i = FirstPeakFrame To EndSTSFrame
                    RADeriv(i) = Math.Abs((RAArray_v(i + 40) - RAArray_v(i - 40)) / (80 * 0.001))
                    LADeriv(i) = Math.Abs((LAArray_v(i + 40) - LAArray_v(i - 40)) / (80 * 0.001))
                Next

                ' Arm peak
                'find 1st 0 point of derivative
                RAPeakFOrce = 0
                For i = RAon To RAon
                    If RADeriv(i) < 1 Then
                        RAPeakForceFrame = i
                        GoTo 71
                    End If
                Next
71:

                LAPeakForce = 0
                For i = LAon To LAoff
                    If LADeriv(i) < 1 Then
                        LAPeakForceFrame = i
                        GoTo 72
                    End If
                Next
            End If
72:
50:

            'add discrete data to a sheet
            'Labels
            If q = 1 Then
                ''    Workbooks(NewName).Worksheets(7).Cells(1, q).Value = "StartSTS"
                ''    Workbooks(NewName).Worksheets(7).Cells(2, q).Value = "SeatOff"
                ''    Workbooks(NewName).Worksheets(7).Cells(3, q).Value = "EndSTS"
                ''    Workbooks(NewName).Worksheets(7).Cells(4, q).Value = "Right 25-50"
                ''    Workbooks(NewName).Worksheets(7).Cells(5, q).Value = "Left 25-50"
                ''    Workbooks(NewName).Worksheets(7).Cells(6, q).Value = "Right 0-100"
                ''    Workbooks(NewName).Worksheets(7).Cells(7, q).Value = "Left 0-100"
                ''    Workbooks(NewName).Worksheets(7).Cells(8, q).Value = "RF Peak Force"
                ''    Workbooks(NewName).Worksheets(7).Cells(9, q).Value = "Left Peak Force"
                ''    Workbooks(NewName).Worksheets(7).Cells(10, q).Value = "Left Average Force"
                ''    Workbooks(NewName).Worksheets(7).Cells(11, q).Value = "Left Average Force"
                ''    Workbooks(NewName).Worksheets(7).Cells(12, q).Value = "Left Area"
                ''    Workbooks(NewName).Worksheets(7).Cells(13, q).Value = "Right Area"
                ''    Workbooks(NewName).Worksheets(7).Cells(14, q).Value = "Right Peak Arm Force"
                ''    Workbooks(NewName).Worksheets(7).Cells(15, q).Value = "Left Peak Arm Force"
                ''    Workbooks(NewName).Worksheets(7).Cells(16, q).Value = "Right Arm Area"
                ''    Workbooks(NewName).Worksheets(7).Cells(17, q).Value = "Left Arm Area"
            End If

            'now put the data next to the label
            ''Workbooks(NewName).Worksheets(7).Cells(1, q + 1).Value = StartSTS
            ''Workbooks(NewName).Worksheets(7).Cells(2, q + 1).Value = Seatoff
            ''Workbooks(NewName).Worksheets(7).Cells(3, q + 1).Value = EndSTS
            ''Workbooks(NewName).Worksheets(7).Cells(4, q + 1).Value = RFSlope25_50
            ''Workbooks(NewName).Worksheets(7).Cells(5, q + 1).Value = LFSlope25_50
            ''Workbooks(NewName).Worksheets(7).Cells(6, q + 1).Value = RFSlope100
            ''Workbooks(NewName).Worksheets(7).Cells(7, q + 1).Value = LFSlope100
            ''Workbooks(NewName).Worksheets(7).Cells(8, q + 1).Value = RFArray(RFPeakFrame)
            ''Workbooks(NewName).Worksheets(7).Cells(9, q + 1).Value = LFArray(RFPeakFrame)
            ''Workbooks(NewName).Worksheets(7).Cells(10, q + 1).Value = RFAverage
            ''Workbooks(NewName).Worksheets(7).Cells(11, q + 1).Value = LFAverage
            ''Workbooks(NewName).Worksheets(7).Cells(12, q + 1).Value = LFArea
            ''Workbooks(NewName).Worksheets(7).Cells(13, q + 1).Value = RFArea

            If ArmsIndicator = 0 Then
                RAPeakForceFrame = 1
                LAPeakForceFrame = 1
            End If

            ''Workbooks(NewName).Worksheets(7).Cells(14, q + 1).Value = RAArray(RAPeakForceFrame)
            ''Workbooks(NewName).Worksheets(7).Cells(15, q + 1).Value = LAArray(LAPeakForceFrame)
            ''Workbooks(NewName).Worksheets(7).Cells(16, q + 1).Value = RAArea
            ''Workbooks(NewName).Worksheets(7).Cells(17, q + 1).Value = LAArea

            'This 'next' is for the end of the overall loop
        Next q

        'now average all the data
        'Workbooks(NewName).Worksheets(7).Cells(2, q + 3).Formula = "= b3-b1"
        'Workbooks(NewName).Worksheets(7).Cells(2, q + 4).Formula = "= c3-c1"
        'Workbooks(NewName).Worksheets(7).Cells(2, q + 5).Formula = "= d3-d1"
        'Workbooks(NewName).Worksheets(7).Cells(2, q + 6).Formula = "= AVERAGE(f2:H2)"
        'Workbooks(NewName).Worksheets(7).Cells(4, q + 2).Formula = "= AVERAGE(B4:D4)"
        'Workbooks(NewName).Worksheets(7).Cells(5, q + 2).Formula = "= average(b5:d5)"
        'Workbooks(NewName).Worksheets(7).Cells(6, q + 2).Formula = "= average(b6:d6)"
        'Workbooks(NewName).Worksheets(7).Cells(7, q + 2).Formula = "= average(b7:d7)"
        'Workbooks(NewName).Worksheets(7).Cells(8, q + 2).Formula = "= average(b8:d8)"
        'Workbooks(NewName).Worksheets(7).Cells(9, q + 2).Formula = "= average(b9:d9)"
        'Workbooks(NewName).Worksheets(7).Cells(10, q + 2).Formula = "= average(b10:d10)"
        'Workbooks(NewName).Worksheets(7).Cells(11, q + 2).Formula = "= average(b11:d11)"
        'Workbooks(NewName).Worksheets(7).Cells(12, q + 2).Formula = "= average(b12:d12)"
        'Workbooks(NewName).Worksheets(7).Cells(13, q + 2).Formula = "= average(b13:d13)"
        'Workbooks(NewName).Worksheets(7).Cells(14, q + 2).Formula = "= average(b14:d14)"
        'Workbooks(NewName).Worksheets(7).Cells(15, q + 2).Formula = "= average(b15:d15)"
        'Workbooks(NewName).Worksheets(7).Cells(16, q + 2).Formula = "= average(b16:d16)"
        'Workbooks(NewName).Worksheets(7).Cells(17, q + 2).Formula = "= average(b17:d17)"

        ''graph data vGRF Bil, seat, RA and LA arrays= "=AVERAGE(RC[-1])"
        'For i = 1 To 7
        '    Workbooks(NewName).Worksheets(i).Activate()
        '    Worksheets(i).Range("b1:f1600").Select()

        '    Charts.Add()
        '    ActiveChart.ChartType = xlXYScatterSmoothNoMarkers
        '    ActiveChart.Location(Where:=xlLocationAsNewSheet)
        'Next

        'Sheets("Chart1").Select()
        'MsgBox("Oh la la! All done!  Ready for another!")

    End Sub

    Private Sub ManualFindSTS_Times()
        'this program is being designed to analyze files from the portable STS device
        'The program reads in data from a *.csv file, collected at 1000 hz for 10 seconds
        'The program will not work for different data collection parameters
        'also to use this program the last second of the 10 second trial the patient must be standing comfortably

        'The output of the program is as follows
        ' Event points  - Start, seat off and end of STS trial
        'removes offsets from data and calculates Bilateral vGRF
        'calculates discreet variables like slope for the preparation phase and peak and average force for the rising phase


        Dim TimeArray(0 To 10000) As Single
        Dim RFArray(0 To 10000) As Single
        Dim LFArray(0 To 10000) As Single
        Dim RAArray(0 To 10000) As Single
        Dim LAArray(0 To 10000) As Single
        Dim SeatArray(0 To 10000) As Single
        Dim vGRFBilatArray(0 To 10000) As Single
        Dim ArmsBilatArray(0 To 10000) As Single
        Dim PerSTS(0 To 10000) As Single
        Dim s = 0
        Dim count = 0
        Dim seatoffFrame = 0
        Dim SeatRef = 0
        Dim Seatoff = 0
        Dim SDSeat = 0
        Dim EndSTS = 0
        Dim EndSTSFrame = 0
        Dim StartSTSFrame = 0
        Dim SecondPeakFrame = 0
        Dim Manual = 0
        Dim STSTiming = 0
        Dim StartSTS = 0
        Dim RAon = 0
        Dim RAoff = 0
        Dim LAon = 0
        Dim LAoff = 0
        Dim RAtotal = 0
        Dim LAtotal = 0
        Dim i = 0
        Dim RAOffset = 0
        Dim LAOffset = 0
        Dim LengthSTS = 0
        Dim g = 0
        Dim NewName = 0
        Dim SubjectName = 0
        Dim a = 0
        Dim j = 0
        Dim FirstPeakFrame = 0
        Dim ThirdPeakFrame = 0
        Dim RFPeakFrame = 0
        Dim LFPeakFrame = 0
        Dim RFTotal = 0
        Dim LFTotal = 0
        Dim RFAverage = 0
        Dim LFAverage = 0
        Dim RFArea = 0
        Dim LFArea = 0
        Dim Start25 = 0
        Dim RFSlope25_50 = 0
        Dim LFSlope25_50 = 0
        Dim RFSlope100 = 0
        Dim LFSlope100 = 0
        Dim RFFinish50 = 0
        Dim LFFinish50 = 0
        Dim RFStart25 = 0
        Dim LFStart25 = 0
        Dim DeltaTime100 = 0
        Dim BilatSlope100 = 0
        Dim ArmsIndicator = 0
        Dim RAArea = 0
        Dim LAArea = 0
        Dim RAPeakForceFrame = 0
        Dim LAPeakForceFrame = 0
        Dim Mybook = 0
        Dim d = 0
        Dim xlXYScatterSmoothNoMarkers = 0
        Dim xlLocationAsNewSheet = 0

        'This puts the data into an array
        i = 0
        For i = 1 To 10000

            ''TimeArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 1).Value
            ''RAArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 3).Value
            ''LAArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 4).Value
            ''RFArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 5).Value
            ''LFArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 6).Value
            ''SeatArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 8).Value

        Next

        'Remove Offsets in Armas Data
        RAtotal = 0
        LAtotal = 0
        i = 0
        For i = 9500 To 10000
            RAtotal = RAtotal + RAArray(i)
            LAtotal = LAtotal + LAArray(i)
        Next

        RAOffset = RAtotal / 500
        LAOffset = LAtotal / 500

        'Put the raw data into a worksheet and name a worksheet for the discreet variables
        'WorkBooks(NewFileName)(.Name = "Raw data" & NewFileName
        'Workbooks(NewFileName).Activate

        'Windows(NewName).Activate()

        'label Columns
        'Cells(1, 1).Value = "Frame"
        'For i = 1 To 6
        ''Workbooks(NewName).Worksheets(1).Cells(1, 1).Value = "Frame(ms) STS"
        ''Workbooks(NewName).Worksheets(1).Cells(1, 2).Value = "vGRF"
        ''Workbooks(NewName).Worksheets(1).Cells(1, 3).Value = "Seat"

        ''Workbooks(NewName).Worksheets(1).Cells(1, 5).Value = "Frame(ms) STS"
        ''Workbooks(NewName).Worksheets(1).Cells(1, 6).Value = "RArm"
        ''Workbooks(NewName).Worksheets(1).Cells(1, 7).Value = "LArm"

        ''Workbooks(NewName).Worksheets(1).Name = "Data"

        'Next

        'multiply data by regressions to convert voltage to force
        a = 0
        For a = 1 To 10000
            RFArray(a) = (165.45 * RFArray(a)) - 1.9287
            LFArray(a) = (169.18 * LFArray(a)) + 13.799
            RAArray(a) = (112.72 * (RAArray(a) - RAOffset))
            LAArray(a) = (255.06 * (LAArray(a) - LAOffset))
            ArmsBilatArray(a) = RAArray(a) + LAArray(a)
        Next

        'calculate the bilateral vGRF data
        d = 0
        For d = 1 To 10000
            vGRFBilatArray(d) = RFArray(d) + LFArray(d)
        Next


        j = 0
        For j = 1 To 10000

            ''Workbooks(NewName).Worksheets(1).Cells(j, 1).Value = j
            ''Workbooks(NewName).Worksheets(1).Cells(j, 2).Value = vGRFBilatArray(j)
            ''Workbooks(NewName).Worksheets(1).Cells(j, 3).Value = SeatArray(j)

            ''Workbooks(NewName).Worksheets(1).Cells(j, 5).Value = j
            ''Workbooks(NewName).Worksheets(1).Cells(j, 6).Value = RAArray(j)
            ''Workbooks(NewName).Worksheets(1).Cells(j, 7).Value = LAArray(j)

        Next

        'graph data vGRF Bil, seat, RA and LA arrays
        ''Workbooks(NewName).Worksheets(1).Activate()
        ''ActiveSheet.Range("b1:b10000").Select()

        ''Charts.Add()
        ''ActiveChart.ChartType = xlXYScatterSmoothNoMarkers
        ''ActiveChart.Location(Where:=xlLocationAsNewSheet)

        ''Workbooks(NewName).Worksheets(1).Activate()
        ''ActiveSheet.Range("c1:c10000").Select()

        ''Charts.Add()
        ''ActiveChart.ChartType = xlXYScatterSmoothNoMarkers
        ''ActiveChart.Location(Where:=xlLocationAsNewSheet)

        ''Workbooks(NewName).Worksheets(1).Activate()
        ''ActiveSheet.Range("f1:f10000").Select()

        ''Charts.Add()
        ''ActiveChart.ChartType = xlXYScatterSmoothNoMarkers
        ''ActiveChart.Location(Where:=xlLocationAsNewSheet)

        ''Workbooks(NewName).Worksheets(1).Activate()
        ''ActiveSheet.Range("g1:g10000").Select()

        ''Charts.Add()
        ''ActiveChart.ChartType = xlXYScatterSmoothNoMarkers
        ''ActiveChart.Location(Where:=xlLocationAsNewSheet)

        ''Sheets("Chart1").Select()

    End Sub
    Sub maxima()
        Dim i = 0
        Dim seatoff = 0
        Dim EndSTSFrame = 0
        'peak force and average from each leg from seat off to end of STS
        'Find first peak
        i = 0
        For i = seatoff To EndSTSFrame
            If RFArray(i) > RFArray(i + 1) Then
                RFPeakForce = RFArray(i)
                If RFArray(i) < RFArray(i + 1) Then
                    GoTo 9
                End If
            End If
        Next

9:      i = 0
        For i = seatoff To EndSTSFrame
            If LFArray(i) > LFArray(i + 1) Then
                LFPeakForce = LFArray(i)
                If LFArray(i) < LFArray(i + 1) Then
                    Exit For
                End If
            End If
        Next
    End Sub

End Class