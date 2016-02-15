Module STSCode
    Public Const INVALID As Integer = -1

    Public totalTests As Integer = INVALID
    Public testCount As Integer = INVALID
    Public startSTSFrame As Integer = INVALID
    Public endSTSFrame As Integer = INVALID

    ' Selected/Calculated Peak variables.
    Public firstPeakFrame As Integer = INVALID
    Public secondPeakFrame As Integer = INVALID
    Public thirdPeakFrame As Integer = INVALID

    ' Calculated Peak variables
    Public rightLegPeakFrame As Integer = INVALID
    Public leftLegPeakFrame As Integer = INVALID

    ' Average Forces
    Public leftLegAvgForce As Integer = INVALID
    Public rightLegAvgForce As Integer = INVALID

    Public seatOffFrame As Integer = INVALID
    Public lengthSTS As Integer = INVALID

    Public TimeArray(0 To 10000) As Single
    Public RFArray(0 To 10000) As Single
    Public LFArray(0 To 10000) As Single
    Public RAArray(0 To 10000) As Single
    Public LAArray(0 To 10000) As Single
    Public SeatArray(0 To 10000) As Single
    Public vGRFBilatArray(0 To 10000) As Single ' RFArray + LFArray
    Public ArmsBilatArray(0 To 10000) As Single ' RAArray + LAArray

    ' We will not need these once the application is completed.
    Public NewFileName As String

    Public vGRFDeriv(0 To 10000) As Double
    Public ArmsvGRFsum(0 To 10000) As Single
    Public ArmsvGRFDeriv(0 To 10000) As Single
    Public RFDeriv(0 To 10000) As Single
    Public LFDeriv(0 To 10000) As Single
    Public RADeriv(0 To 10000) As Single
    Public LADeriv(0 To 10000) As Single
    Public PerSTS(0 To 10000) As Single
    Public RFArray_v(0 To 10000) As Single
    Public LFArray_v(0 To 10000) As Single
    Public RAArray_v(0 To 10000) As Single
    Public LAArray_v(0 To 10000) As Single

    Public Start50 As Integer = INVALID
    Public Finish50 As Integer = INVALID
    Public FirstPeak As Single = INVALID
    Public SecondPeak As Single = INVALID
    Public ThridPeak As Single = INVALID
    Public LFPeakForce As Single = INVALID
    Public RFPeakForce As Single = INVALID
    Public RAPeakFOrce As Single = INVALID
    Public LAPeakForce As Single = INVALID
    Public RDeltaTime25_50 As Single = INVALID
    Public LDeltaTime25_50 As Single = INVALID
    Public RDeltaForce As Single = INVALID
    Public LDeltaForce As Single = INVALID

    Public RAArea As Double = INVALID
    Public LAArea As Double = INVALID
    Public RFArea As Double = INVALID
    Public LFArea As Double = INVALID

    Public Msg, Style, Title, Response, MyString

    Public Sub main()
        ' Number of tests to run, probably used for various purposes.
        Dim testCount As Integer = INVALID

        totalTests = getTestCount()

        For testCount = 0 To totalTests
            runTest()
            takeDerivativesOfvGRF()
            startSTSFrame = getStartSTSFrame()
            endSTSFrame = getEndSTSFrame(startSTSFrame)
            firstPeakFrame = getFirstPeakFrame(startSTSFrame)
            secondPeakFrame = getSecondPeakFrame(startSTSFrame)
            thirdPeakFrame = getThirdPeakFrame(startSTSFrame)
            seatOffFrame = getSeatOffFrame() '<------------- @TODO Probably isn't correct, fix later.

            If (Not validData(startSTSFrame, endSTSFrame, seatOffFrame, secondPeakFrame)) Then
                MsgBox("Data invalid, go to manual pickoff.", vbApplicationModal + vbOKOnly + vbInformation, getAppTitle())
            End If

            'Now clip the data and set the time to percent STS for plotting
            lengthSTS = endSTSFrame - startSTSFrame

            convertDataFromVoltagesToWeight()
            calculateLegDerivatives(firstPeakFrame, endSTSFrame)
            rightLegPeakFrame = getRightLegPeakFrame(firstPeakFrame, thirdPeakFrame)
            leftLegPeakFrame = getLeftLegPeakFrame(firstPeakFrame, thirdPeakFrame)
            leftLegAvgForce = getLeftLegAvgForce(seatOffFrame, endSTSFrame)
            rightLegAvgForce = getRightLegAvgForce(seatOffFrame, endSTSFrame)

        Next testCount
    End Sub

    Public Sub clearProgramVariables()
        Start50 = INVALID
        Finish50 = INVALID
        FirstPeak = INVALID
        SecondPeak = INVALID
        ThridPeak = INVALID
        LFPeakForce = INVALID
        RFPeakForce = INVALID
        RAPeakFOrce = INVALID
        LAPeakForce = INVALID
        RDeltaTime25_50 = INVALID
        LDeltaTime25_50 = INVALID
        RDeltaForce = INVALID
        LDeltaForce = INVALID
        startSTSFrame = INVALID
        endSTSFrame = INVALID

        ' Selected/Calculated Peak variables.
        firstPeakFrame = INVALID
        secondPeakFrame = INVALID
        thirdPeakFrame = INVALID

        ' Calculated Peak variables
        rightLegPeakFrame = INVALID
        leftLegPeakFrame = INVALID

        ' Average Forces
        leftLegAvgForce = INVALID
        rightLegAvgForce = INVALID

        seatOffFrame = INVALID
        lengthSTS = INVALID
    End Sub
    ' Run STS, figure out how to call the form to get the data arrays later.
    Public Sub runTest()
        ' Do some test stuff...
    End Sub

    ' Get the number of STS tests to run.
    Public Function getTestCount() As Integer
        getTestCount = CInt(InputBox("Number of trials to be processed:"))
    End Function

    ' Take derivatives of vGRF
    Public Sub takeDerivativesOfvGRF()

        Dim s As Integer
        Dim samplesPerDeriv As Integer = 80
        Dim sampleCalcOffset As Integer = samplesPerDeriv / 2

        For s = 100 To 9000
            ' sVertical Ground Reaction Force.
            vGRFDeriv(s) = Math.Abs((vGRFBilatArray(s + sampleCalcOffset) - vGRFBilatArray(s - sampleCalcOffset)) / (samplesPerDeriv * 0.001))


            ArmsvGRFsum(s) = (vGRFBilatArray(s) + RAArray(s) + LAArray(s))
            ArmsvGRFDeriv(s) = Math.Abs((ArmsvGRFsum(s + sampleCalcOffset) - ArmsvGRFsum(s - sampleCalcOffset)) / (samplesPerDeriv * 0.001))
        Next
    End Sub

    ' Find the Start of the STS movement
    Public Function getStartSTSFrame() As Integer
        ' Get a moving average
        Dim i As Integer = 0
        Dim s As Integer = 0
        Dim footRefSum As Double = 0.0
        Dim footRef As Double
        Dim footAverageWeight As Double
        Dim footStdDiv As Double
        Dim sampleStartPoint As Integer = 100
        Dim sampleEndPoint As Integer = 300
        Dim maxSTSStartPoint As Integer = 90000
        Dim startSTSFrame As Integer = 0

        For i = sampleStartPoint To sampleEndPoint
            footRefSum = vGRFBilatArray(i) + footRefSum
        Next

        ' Get average foot weight.
        footAverageWeight = (footRefSum / (sampleEndPoint - sampleStartPoint))
        footRef = footAverageWeight - (footAverageWeight * footStdDiv)

        ' Find the start of the trial ref
        For s = sampleStartPoint To maxSTSStartPoint
            If vGRFBilatArray(s) <= footRef Then
                ' If we have found a point in the array that falls below 
                ' our average then save it as the start point.
                startSTSFrame = s
                Exit For
            End If
        Next

        ' @TODO Why is this divided by 1000?
        getStartSTSFrame = (startSTSFrame / 1000)
    End Function

    ' Find the END of the STS movement
    Public Function getEndSTSFrame(ByVal startSTSFrame As Integer) As Integer
        Dim count As Integer = 0
        Dim s As Integer = 0
        Dim endSTSSum As Double = 0
        Dim endSTSRef As Double = 0
        Dim endSTSFrame As Integer = 0
        Dim firstPeakFrame As Integer = 0
        Dim firstPeakFrameOffset As Integer = 200
        Dim secondPeakFrame As Integer = 0
        Dim secondPeakFrameOffset As Integer = 100
        Dim thirdPeakFrame As Integer = 0
        Dim thirdPeakFrameOffset As Integer = 100
        Dim sampleStartPoint As Integer = 9000
        Dim sampleEndPoint As Integer = 9500
        Dim startSTSFrameOffset As Integer = 100
        Dim totalSamplesInTest As Integer = getTotalSamplesInTest()

        For s = sampleStartPoint To sampleEndPoint
            endSTSSum = vGRFBilatArray(s) + endSTSSum
        Next

        endSTSSum = (endSTSSum / (sampleEndPoint - sampleStartPoint))

        ' Find first minima
        For s = startSTSFrame + startSTSFrameOffset To sampleStartPoint
            If (Math.Abs(vGRFDeriv(s))) <= 1 Then
                count = count + 1
                If vGRFDeriv(s) < vGRFDeriv(s + 1) Then
                    firstPeakFrame = s - count
                    Exit For
                End If
            End If
        Next

        ' Find minima - second 0 of the derivative
        For s = firstPeakFrame + firstPeakFrameOffset To totalSamplesInTest
            If (Math.Abs(vGRFDeriv(s))) <= 1 Then
                If vGRFDeriv(s) < vGRFDeriv(s + 1) Then
                    secondPeakFrame = s
                    Exit For
                End If
            End If
        Next

        ' Find the first peak
        For s = secondPeakFrame + secondPeakFrameOffset To totalSamplesInTest
            If (Math.Abs(vGRFDeriv(s))) <= 1 Then
                If vGRFDeriv(s) < vGRFDeriv(s + 1) Then
                    thirdPeakFrame = s
                    Exit For
                End If
            End If
        Next

        ' Now find the end of the STS trial
        For s = thirdPeakFrame + thirdPeakFrameOffset To totalSamplesInTest
            If vGRFBilatArray(s) >= endSTSRef Then
                endSTSFrame = s
                Exit For
            End If
        Next

        getEndSTSFrame = endSTSFrame
    End Function

    ' Find the First Peak Frame.
    Public Function getFirstPeakFrame(ByVal startSTSFrame As Integer) As Integer
        Dim count As Integer = 0
        Dim s As Integer = 0
        Dim firstPeakFrame As Integer = 0
        Dim sampleStartPoint As Integer = 9000
        Dim sampleEndPoint As Integer = 9500
        Dim startSTSFrameOffset As Integer = 100

        ' Find first minima
        For s = startSTSFrame + startSTSFrameOffset To sampleStartPoint
            If (Math.Abs(vGRFDeriv(s))) <= 1 Then
                count = count + 1
                If vGRFDeriv(s) < vGRFDeriv(s + 1) Then
                    firstPeakFrame = s - count
                    Exit For
                End If
            End If
        Next

        getFirstPeakFrame = firstPeakFrame
    End Function

    ' Find the Second Peak Frame.
    Public Function getSecondPeakFrame(ByVal startSTSFrame As Integer) As Integer
        Dim count As Integer = 0
        Dim s As Integer = 0
        Dim firstPeakFrame As Integer = 0
        Dim firstPeakFrameOffset As Integer = 200
        Dim secondPeakFrame As Integer = 0
        Dim sampleStartPoint As Integer = 9000
        Dim startSTSFrameOffset As Integer = 100
        Dim totalSamplesInTest As Integer = getTotalSamplesInTest()

        ' Find first minima
        For s = startSTSFrame + startSTSFrameOffset To sampleStartPoint
            If (Math.Abs(vGRFDeriv(s))) <= 1 Then
                count = count + 1
                If vGRFDeriv(s) < vGRFDeriv(s + 1) Then
                    firstPeakFrame = s - count
                    Exit For
                End If
            End If
        Next

        ' Find minima - second 0 of the derivative
        For s = firstPeakFrame + firstPeakFrameOffset To totalSamplesInTest
            If (Math.Abs(vGRFDeriv(s))) <= 1 Then
                If vGRFDeriv(s) < vGRFDeriv(s + 1) Then
                    secondPeakFrame = s
                    Exit For
                End If
            End If
        Next

        getSecondPeakFrame = secondPeakFrame
    End Function

    ' Find the Third Peak Frame.
    Public Function getThirdPeakFrame(ByVal startSTSFrame As Integer) As Integer
        Dim count As Integer = 0
        Dim s As Integer = 0
        Dim firstPeakFrame As Integer = 0
        Dim firstPeakFrameOffset As Integer = 200
        Dim secondPeakFrame As Integer = 0
        Dim secondPeakFrameOffset As Integer = 100
        Dim thirdPeakFrame As Integer = 0
        Dim sampleStartPoint As Integer = 9000
        Dim startSTSFrameOffset As Integer = 100
        Dim totalSamplesInTest As Integer = getTotalSamplesInTest()

        ' Find first minima
        For s = startSTSFrame + startSTSFrameOffset To sampleStartPoint
            If (Math.Abs(vGRFDeriv(s))) <= 1 Then
                count = count + 1
                If vGRFDeriv(s) < vGRFDeriv(s + 1) Then
                    firstPeakFrame = s - count
                    Exit For
                End If
            End If
        Next

        ' Find minima - second 0 of the derivative
        For s = firstPeakFrame + firstPeakFrameOffset To totalSamplesInTest
            If (Math.Abs(vGRFDeriv(s))) <= 1 Then
                If vGRFDeriv(s) < vGRFDeriv(s + 1) Then
                    secondPeakFrame = s
                    Exit For
                End If
            End If
        Next

        ' Find the first peak
        For s = secondPeakFrame + secondPeakFrameOffset To totalSamplesInTest
            If (Math.Abs(vGRFDeriv(s))) <= 1 Then
                If vGRFDeriv(s) < vGRFDeriv(s + 1) Then
                    thirdPeakFrame = s
                    Exit For
                End If
            End If
        Next

        getThirdPeakFrame = thirdPeakFrame
    End Function

    Public Function getSeatOffFrame() As Integer
        Dim seatSum As Double
        Dim seatOff As Double
        Dim seatRef As Double

        Dim totalSamplesInTest As Integer = getTotalSamplesInTest()
        Dim sampleStartPoint As Integer = 9000
        Dim sampleEndPoint As Integer = 9500
        Dim seatOffOffset As Integer = 100
        Dim seatoffFrame As Integer = 0
        Dim count As Integer = 0

        For s = sampleStartPoint To sampleEndPoint
            seatSum = vGRFBilatArray(s) + seatSum
        Next

        seatSum = (seatSum / (sampleEndPoint / sampleStartPoint))

        ' Get seat off from the seat sensor
        seatRef = SeatArray(9259) + (7 * seatSum) ' @TODO SEAT REF, may change later.

        For s = 1 To totalSamplesInTest
            If SeatArray(totalSamplesInTest - s) >= seatRef Then
                seatoffFrame = s
                count = count + 1
                If count = seatOffOffset Then
                    seatoffFrame = s - seatOffOffset
                    Exit For
                End If
            End If
        Next

        ' Here's seat off(start of rising phase)
        seatoffFrame = totalSamplesInTest - seatoffFrame
        seatOff = seatoffFrame / 1000 ' Divide by sampling rate.
        getSeatOffFrame = seatoffFrame
    End Function

    Public Function validData(ByVal startSTSframe As Integer, ByVal endSTSFrame As Integer, ByVal seatoffFrame As Integer, ByVal secondPeakFrame As Integer) As Boolean
        Dim endSTS As Double = 0
        endSTS = (endSTSFrame / 1000)
        MsgBox("The start frame is " & startSTSframe)
        'MsgBox ("The first minima is " & FirstPeakFrame)
        MsgBox("The seat off point is " & seatoffFrame)
        MsgBox("The peak force after seat off is " & secondPeakFrame)
        'MsgBox ("The next minima after the peak is " & ThirdPeakFrame)
        MsgBox("The end of the rising phase is " & endSTSFrame)

        ' @TODO Eventually we'll signal invalid data, but for now
        ' we'll just return true every time.
        validData = True
    End Function

    Private Sub convertDataFromVoltagesToWeight()
        Dim a As Integer = 0

        ' Multiply data by regressions to convert voltage to force
        For a = 1 To 10000

            ' Use the voltage data to find peaks
            ' @TODO These arrays are likely not necessary...
            RFArray_v(a) = RFArray(a)
            LFArray_v(a) = LFArray(a)
            RAArray_v(a) = RAArray(a)
            LAArray_v(a) = RAArray(a)

            ' Convert the data to calculate stuff 
            RFArray(a) = (176.11 * RFArray(a)) + 10.42
            LFArray(a) = (180.77 * LFArray(a)) - 9.21
            RAArray(a) = (113.27 * RAArray(a)) + 112.2
            LAArray(a) = (234.71 * LAArray(a)) - 18.6
            ArmsBilatArray(a) = RAArray(a) + LAArray(a)
            vGRFBilatArray(a) = RFArray(a) + LFArray(a)
        Next
    End Sub

    Public Sub calculateLegDerivatives(ByVal firstPeakFrame As Integer, ByVal endSTSFrame As Integer)
        ' Find first peak
        ' First take derivative
        ' @TODO Why is this done AFTER the VOLTAGES are converted to FORCES?
        For i = firstPeakFrame To endSTSFrame
            RFDeriv(i) = Math.Abs((RFArray_v(i + 40) - RFArray_v(i - 40)) / (80 * 0.001))
            LFDeriv(i) = Math.Abs((LFArray_v(i + 40) - LFArray_v(i - 40)) / (80 * 0.001))
        Next
    End Sub

    Public Function getRightLegPeakFrame(ByVal firstPeakFrame As Integer, ByVal thirdPeakFrame As Integer) As Integer
        Dim firstPeakFrameOffset As Integer = 200
        Dim thirdPeakFrameOffset As Integer = 100

        ' If we return this value, then we know we have invalid data.
        getRightLegPeakFrame = INVALID

        For i = (firstPeakFrame + firstPeakFrameOffset) To (thirdPeakFrame + thirdPeakFrameOffset)
            If RFDeriv(i) < 1 Then
                getRightLegPeakFrame = i
                Exit For
            End If
        Next

    End Function

    Public Function getLeftLegPeakFrame(ByVal firstPeakFrame As Integer, ByVal thirdPeakFrame As Integer)
        Dim firstPeakFrameOffset As Integer = 200
        Dim thirdPeakFrameOffset As Integer = 200

        ' If we return this value, then we know we have invalid data.
        getLeftLegPeakFrame = INVALID

        For i = (firstPeakFrame + firstPeakFrameOffset) To (thirdPeakFrame + thirdPeakFrameOffset)
            If LFDeriv(i) < 1 Then
                getLeftLegPeakFrame = i
                Exit For
            End If
        Next

    End Function

    ' Calculate average Force for the Left Leg.
    Public Function getLeftLegAvgForce(ByVal seatOffFrame As Integer, ByVal endSTSFrame As Integer) As Integer

        ' First sum the forces
        Dim LFTotal As Integer = 0
        For i = seatOffFrame To endSTSFrame
            LFTotal = LFTotal + LFArray(i)
        Next

        ' Then average them
        getLeftLegAvgForce = LFTotal / (endSTSFrame - seatOffFrame)
    End Function

    ' Calculate average Force for the Right Leg.
    Public Function getRightLegAvgForce(ByVal seatOffFrame As Integer, ByVal endSTSFrame As Integer) As Integer

        ' First sum the forces
        Dim RFTotal As Integer = 0
        For i = seatOffFrame To endSTSFrame
            RFTotal = RFTotal + RFArray(i)
        Next

        ' Then average them
        getRightLegAvgForce = RFTotal / (endSTSFrame - seatOffFrame)
    End Function

    Public Function getRFArea() As Integer

        'Calculate area for rising phase
        RFArea = 0

        For i = seatOffFrame To endSTSFrame
            RFArea = RFArea + (RFArray(i) / (endSTSFrame - seatOffFrame))
        Next

        getRFArea = LFArea
    End Function

    Public Function getLFArea() As Integer

        'Calculate area for rising phase
        LFArea = 0

        For i = seatOffFrame To endSTSFrame
            LFArea = RFArea + (LFArray(i) / (endSTSFrame - seatOffFrame))
        Next

        getLFArea = LFArea
    End Function
End Module
