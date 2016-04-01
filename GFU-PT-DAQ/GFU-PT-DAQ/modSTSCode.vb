Module STSCode
    Public Const INVALID As Integer = -1

    Public totalTests As Integer = INVALID
    Public testCount As Integer = INVALID
    Public startSTSFrame As Integer = INVALID
    Public endSTSFrame As Integer = INVALID

    ' Bilateral variables.
    Public bilateralFirstMinimaFrame As Integer = INVALID
    Public bilateralPeakFrame As Integer = INVALID
    Public bilateralSecondMinimaFrame As Integer = INVALID

    ' Left leg variables.
    Public leftLegFirstMinimaFrame As Integer = INVALID
    Public leftLegPeakFrame As Integer = INVALID
    Public leftLegSecondMinimaFrame As Integer = INVALID

    ' Right leg variables.
    Public rightLegFirstMinimaFrame As Integer = INVALID
    Public rightLegPeakFrame As Integer = INVALID
    Public rightLegSecondMinimaFrame As Integer = INVALID

    ' Left arm variables.
    Public leftArmStartFrame As Integer = INVALID
    Public leftArmPeakFrame As Integer = INVALID
    Public leftArmEndFrame As Integer = INVALID

    ' Right arm variables.
    Public rightArmStartFrame As Integer = INVALID
    Public rightArmPeakFrame As Integer = INVALID
    Public rightArmEndFrame As Integer = INVALID

    ' Seat off variable.
    Public seatOffFrame As Integer = INVALID

    ' Average Forces
    Public leftLegAvgForce As Integer = INVALID
    Public rightLegAvgForce As Integer = INVALID

    Public lengthSTS As Integer = INVALID

    Public TimeArray(0 To 10000) As Double

    ' Right, Left Leg arrays.
    Public RFArray(0 To 10000) As Double
    Public LFArray(0 To 10000) As Double

    ' Right, Left Arm arrays.
    Public RAArray(0 To 10000) As Double
    Public LAArray(0 To 10000) As Double

    Public SeatArray(0 To 10000) As Double
    Public vGRFBilatArray(0 To 10000) As Double ' RFArray + LFArray
    Public ArmsBilatArray(0 To 10000) As Double ' RAArray + LAArray

    Public vGRFDeriv(0 To 10000) As Double
    Public ArmsvGRFsum(0 To 10000) As Double
    Public ArmsvGRFDeriv(0 To 10000) As Double

    ' Derivative arrays.
    Public RFDeriv(0 To 10000) As Double
    Public LFDeriv(0 To 10000) As Double
    Public RADeriv(0 To 10000) As Double
    Public LADeriv(0 To 10000) As Double

    Public FirstPeak As Double = INVALID
    Public SecondPeak As Double = INVALID
    Public ThirdPeak As Double = INVALID
    Public LFPeakForce As Double = INVALID
    Public RFPeakForce As Double = INVALID
    Public RAPeakFOrce As Double = INVALID
    Public LAPeakForce As Double = INVALID
    Public RDeltaTime25_50 As Double = INVALID
    Public LDeltaTime25_50 As Double = INVALID
    Public RDeltaForce As Double = INVALID
    Public LDeltaForce As Double = INVALID

    Public RAArea As Double = INVALID
    Public LAArea As Double = INVALID
    Public RFArea As Double = INVALID
    Public LFArea As Double = INVALID

    Public Sub main()
        ' Number of tests to run, probably used for various purposes.
        'Dim testCount As Integer = INVALID

        'totalTests = getTestCount()

        'For testCount = 0 To totalTests
        '    runTest()
        takeDerivativesOfvGRF()
        startSTSFrame = getStartSTSFrame()
        endSTSFrame = getEndSTSFrame(startSTSFrame)
        bilateralFirstMinimaFrame = getFirstPeakFrame(startSTSFrame)
        bilateralPeakFrame = getSecondPeakFrame(startSTSFrame)
        bilateralSecondMinimaFrame = getThirdPeakFrame(startSTSFrame)
        seatOffFrame = getSeatOffFrame() '<------------- @TODO Probably isn't correct, fix later.

        If (Not validData(startSTSFrame, endSTSFrame, seatOffFrame, bilateralPeakFrame)) Then
            MsgBox("Data invalid, go to manual pickoff.", vbApplicationModal + vbOKOnly + vbInformation, getAppTitle())
            clearProgramVariables()
        End If

        'Now clip the data and set the time to percent STS for plotting
        lengthSTS = endSTSFrame - startSTSFrame

        calculateLegDerivatives(bilateralFirstMinimaFrame, endSTSFrame)
        convertDataFromVoltagesToWeight()
        rightLegPeakFrame = getRightLegPeakFrame(bilateralFirstMinimaFrame, bilateralSecondMinimaFrame)
        leftLegPeakFrame = getLeftLegPeakFrame(bilateralFirstMinimaFrame, bilateralSecondMinimaFrame)
        rightLegAvgForce = getRightLegAvgForce(seatOffFrame, endSTSFrame)
        leftLegAvgForce = getLeftLegAvgForce(seatOffFrame, endSTSFrame)

        'Next testCount
    End Sub

    Public Sub clearProgramVariables()
        FirstPeak = INVALID
        SecondPeak = INVALID
        ThirdPeak = INVALID
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
        bilateralFirstMinimaFrame = INVALID
        bilateralPeakFrame = INVALID
        bilateralSecondMinimaFrame = INVALID

        ' Calculated Peak variables
        rightLegPeakFrame = INVALID
        leftLegPeakFrame = INVALID

        ' Average Forces
        leftLegAvgForce = INVALID
        rightLegAvgForce = INVALID

        seatOffFrame = INVALID
        lengthSTS = INVALID
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
            ' Vertical Ground Reaction Force.
            'vGRFDeriv(s) = Math.Abs((vGRFBilatArray(s + sampleCalcOffset) - vGRFBilatArray(s - sampleCalcOffset)) / (samplesPerDeriv * (1.0 / 10000.0)))
            vGRFDeriv(s) = (vGRFBilatArray(s + sampleCalcOffset) - vGRFBilatArray(s - sampleCalcOffset)) / (samplesPerDeriv * (1.0 / 10000.0))


            ArmsvGRFsum(s) = (RAArray(s) + LAArray(s))
            'ArmsvGRFDeriv(s) = Math.Abs((ArmsvGRFsum(s + sampleCalcOffset) - ArmsvGRFsum(s - sampleCalcOffset)) / (samplesPerDeriv * (1.0 / 10000.0)))
            ArmsvGRFDeriv(s) = (ArmsvGRFsum(s + sampleCalcOffset) - ArmsvGRFsum(s - sampleCalcOffset)) / (samplesPerDeriv * (1.0 / 10000.0))
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
        Dim sampleStartPoint As Integer = 100
        Dim sampleEndPoint As Integer = 300
        Dim maxSTSStartPoint As Integer = 90000
        Dim startSTSFrame As Integer = 0

        ' NEEDS TO BE SET APPROPRIATELY
        Dim footStdDiv As Double = 0.1

        ' Get the sum of all points between the start and end reference points.
        For i = sampleStartPoint To sampleEndPoint
            footRefSum = footRefSum + vGRFBilatArray(i)
        Next

        ' Get average foot weight by dividing our sum by the # of points.
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

        getStartSTSFrame = startSTSFrame
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
        'MsgBox("The start frame is " & startSTSframe)
        'MsgBox ("The first minima is " & FirstPeakFrame)
        'MsgBox("The seat off point is " & seatoffFrame)
        'MsgBox("The peak force after seat off is " & secondPeakFrame)
        'MsgBox ("The next minima after the peak is " & ThirdPeakFrame)
        'MsgBox("The end of the rising phase is " & endSTSFrame)

        ' @TODO Eventually we'll signal invalid data, but for now
        ' we'll just return true every time.
        validData = True
    End Function

    Public Sub convertDataFromVoltagesToWeight()
        ' Multiply data by regressions to convert voltage to force
        For i = 0 To 10000 - 1

            ' Convert the data to calculate stuff 
            RFArray(i) = (176.11 * RFArray(i))
            LFArray(i) = (180.77 * LFArray(i))
            RAArray(i) = (113.27 * RAArray(i))
            LAArray(i) = (234.71 * LAArray(i))

            ArmsBilatArray(i) = RAArray(i) + LAArray(i)
            vGRFBilatArray(i) = RFArray(i) + LFArray(i)
        Next
    End Sub

    Public Sub calculateLegDerivatives(ByVal firstPeakFrame As Integer, ByVal endSTSFrame As Integer)
        For i = firstPeakFrame To endSTSFrame
            'RFDeriv(i) = Math.Abs((RFArray_v(i + 40) - RFArray_v(i - 40)) / (80 * (1.0 / 10000.0)))
            'LFDeriv(i) = Math.Abs((LFArray_v(i + 40) - LFArray_v(i - 40)) / (80 * (1.0 / 10000.0)))
            RFDeriv(i) = (RFArray(i + 40) - RFArray(i - 40)) / (80 * (1.0 / 10000.0))
            LFDeriv(i) = (LFArray(i + 40) - LFArray(i - 40)) / (80 * (1.0 / 10000.0))
        Next
    End Sub

    Public Function getRightLegPeakFrame(ByVal firstPeakFrame As Integer, ByVal thirdPeakFrame As Integer) As Integer
        Dim firstPeakFrameOffset As Integer = 200
        Dim thirdPeakFrameOffset As Integer = 100

        ' If we return this value, then we know we have invalid data.
        getRightLegPeakFrame = INVALID

        For i = (firstPeakFrame + firstPeakFrameOffset) To (thirdPeakFrame + thirdPeakFrameOffset)
            If RFDeriv(i) < 1 Then ' @TODO: Maybe = 0?
                getRightLegPeakFrame = i
                Exit For
            End If
        Next

    End Function

    Public Function getLeftLegPeakFrame(ByVal firstMinima As Integer, ByVal secondMinima As Integer)
        Dim firstMinimaOffset As Integer = 200
        Dim secondMinimaOffset As Integer = 200

        ' If we return this value, then we know we have invalid data.
        getLeftLegPeakFrame = INVALID

        For i = (firstMinima + firstMinimaOffset) To (secondMinima + secondMinimaOffset)
            If LFDeriv(i) < 1 Then ' @TODO: Maybe = 0?
                getLeftLegPeakFrame = i
                Exit For
            End If
        Next

    End Function

    ' Calculate Average Force for the Left Leg.
    Public Function getLeftLegAvgForce(ByVal seatOffFrame As Integer, ByVal endSTSFrame As Integer) As Double

        Dim LFTotal As Double = 0

        ' First sum the forces.
        For i = seatOffFrame To endSTSFrame
            LFTotal += listLeftLeg(i)
        Next

        ' Then average them.
        getLeftLegAvgForce = LFTotal / CDbl((endSTSFrame - seatOffFrame))
    End Function

    ' Calculate Average Force for the Right Leg.
    Public Function getRightLegAvgForce(ByVal seatOffFrame As Integer, ByVal endSTSFrame As Integer) As Double

        Dim RFTotal As Double = 0

        ' First sum the forces.
        For i = seatOffFrame To endSTSFrame
            RFTotal += listRightLeg(i)
        Next

        ' Then average them.
        getRightLegAvgForce = RFTotal / CDbl((endSTSFrame - seatOffFrame))
    End Function

    Public Function getRFArea() As Double

        'Calculate area for rising phase.
        RFArea = 0

        For i = seatOffFrame To endSTSFrame
            RFArea += (RFArray(i) / (endSTSFrame - seatOffFrame))
        Next

        getRFArea = RFArea
    End Function

    Public Function getLFArea() As Double

        'Calculate area for rising phase.
        LFArea = 0

        For i = seatOffFrame To endSTSFrame
            LFArea += (LFArray(i) / (endSTSFrame - seatOffFrame))
        Next

        getLFArea = LFArea
    End Function
End Module
