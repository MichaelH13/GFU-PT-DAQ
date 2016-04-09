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
    Public arrayRightLegDerivative(0 To 10000) As Double
    Public arrayLeftLegDerivative(0 To 10000) As Double
    Public arrayBilateralLegsDerivative(0 To 10000) As Double
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
        startSTSFrame = getStartSTSFrame()
        endSTSFrame = getEndSTSFrame(startSTSFrame)
        bilateralFirstMinimaFrame = getBilateralFirstMinimaFrame(startSTSFrame)
        bilateralPeakFrame = getBilateralPeakFrame(startSTSFrame)
        bilateralSecondMinimaFrame = getBilateralSecondMinimaFrame(startSTSFrame)
        seatOffFrame = getSeatOffFrame() '<------------- @TODO Probably isn't correct, fix later.

        If (Not validData(startSTSFrame, endSTSFrame, seatOffFrame, bilateralPeakFrame)) Then
            MsgBox("Data invalid, go to manual pickoff.", vbApplicationModal + vbOKOnly + vbInformation, getAppTitle())
            clearProgramVariables()
        End If

        'Now clip the data and set the time to percent STS for plotting
        lengthSTS = endSTSFrame - startSTSFrame

        calculateLegDerivatives(startSTSFrame, endSTSFrame)
        convertDataFromVoltagesToWeight()
        rightLegPeakFrame = getRightLegPeakFrame(bilateralFirstMinimaFrame, bilateralSecondMinimaFrame)
        leftLegPeakFrame = getLeftLegPeakFrame(bilateralFirstMinimaFrame, bilateralSecondMinimaFrame)
        rightLegAvgForce = getRightLegAvgForce()
        leftLegAvgForce = getLeftLegAvgForce()

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
    Public Function getBilateralFirstMinimaFrame(ByVal startSTSFrame As Integer) As Integer
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

        getBilateralFirstMinimaFrame = firstPeakFrame
    End Function

    ' Find the Second Peak Frame.
    Public Function getBilateralPeakFrame(ByVal startSTSFrame As Integer) As Integer
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

        getBilateralPeakFrame = secondPeakFrame
    End Function

    ' Find the Third Peak Frame.
    Public Function getBilateralSecondMinimaFrame(ByVal startSTSFrame As Integer) As Integer
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

        getBilateralSecondMinimaFrame = thirdPeakFrame
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

    ''' <summary>
    ''' Conversions from Dr. Houck from 4/8/2016
    ''' Right Wii Plate y=0.0544(signal) +3.3621
    ''' Left Wii Plate y=0.0574(signal)-0.2194
    ''' Right Arm  y = 0.1878(signal)+56.123
    ''' Left Arm  y = 0.2222(signal)+32.94
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub convertDataFromVoltagesToWeight()
        ' Multiply data by regressions to convert voltage to force
        For i = 0 To 10000 - 1

            ' Convert the legs data
            listRightLeg(i) = (0.0544 * listRightLeg(i)) + 3.3621
            listLeftLeg(i) = (0.0574 * listLeftLeg(i)) - 0.2194

            ' Vertical ground reaction force for legs and bilateral.
            vGRFBilatArray(i) = listLeftLeg(i) + listRightLeg(i)
            listBilateralLegs(i) = listLeftLeg(i) + listRightLeg(i)

            ' Convert the arms data
            listRightArm(i) = (0.01878 * listRightArm(i)) + 56.123
            listLeftArm(i) = (0.02222 * listLeftArm(i)) + 32.94
            ArmsBilatArray(i) = listLeftArm(i) + listRightArm(i)
        Next
    End Sub

    ''' <summary>
    ''' Calculates the leg derivatives for the slope.
    ''' </summary>
    ''' <param name="startSTSFrame"></param>
    ''' <param name="endSTSFrame"></param>
    ''' <remarks></remarks>
    Public Sub calculateLegDerivatives(ByVal startSTSFrame As Integer, ByVal endSTSFrame As Integer)
        For i = startSTSFrame To endSTSFrame
            arrayRightLegDerivative(i) = ((listRightLeg(i + 40) - listRightLeg(i - 40)) / 80) * (1.0 / 10000.0)
            arrayLeftLegDerivative(i) = ((listLeftLeg(i + 40) - listLeftLeg(i - 40)) / 80) * (1.0 / 10000.0)
            arrayBilateralLegsDerivative(i) = ((listBilateralLegs(i + 40) - listBilateralLegs(i - 40)) / 80) * (1.0 / 10000.0)
        Next
    End Sub

    Public Function getRightLegPeakFrame(ByVal firstPeakFrame As Integer, ByVal thirdPeakFrame As Integer) As Integer
        Dim firstPeakFrameOffset As Integer = 200
        Dim thirdPeakFrameOffset As Integer = 100

        ' If we return this value, then we know we have invalid data.
        getRightLegPeakFrame = INVALID

        For i = (firstPeakFrame + firstPeakFrameOffset) To (thirdPeakFrame + thirdPeakFrameOffset)
            If arrayRightLegDerivative(i) < 1 Then ' @TODO: Maybe = 0?
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
            If arrayLeftLegDerivative(i) < 1 Then ' @TODO: Maybe = 0?
                getLeftLegPeakFrame = i
                Exit For
            End If
        Next

    End Function

    ''' <summary>
    ''' Gets the average force for the left leg from the the seat off frame 
    ''' to the end of the test.
    ''' </summary>
    ''' <returns>Returns the average force for the left leg from the the seat off frame 
    ''' to the end of the test as a Double. Units returned are Newtons.</returns>
    ''' <remarks></remarks>
    Public Function getLeftLegAvgForce() As Double
        getLeftLegAvgForce = getAverageForList(listLeftLeg, seatOffFrame, endSTSFrame)
    End Function

    ''' <summary>
    ''' Gets the average force for the right leg from the the seat off frame 
    ''' to the end of the test.
    ''' </summary>
    ''' <returns>Returns the average force for the right leg from the the seat off frame 
    ''' to the end of the test as a Double. Units returned are Newtons.</returns>
    ''' <remarks></remarks>
    Public Function getRightLegAvgForce() As Double
        getRightLegAvgForce = getAverageForList(listRightLeg, seatOffFrame, endSTSFrame)
    End Function

    ''' <summary>
    ''' Gets the bilateral legs slope from the first minima 
    ''' to the bilateral peak.
    ''' </summary>
    ''' <returns>Returns the bilateral legs slope from the first minima 
    ''' to the bilateral peak as a Double. Units returned are Newtons/Second.</returns>
    ''' <remarks></remarks>
    Public Function getBilateralSlope() As Double
        getBilateralSlope = getSlopeForList(listBilateralLegs, bilateralFirstMinimaFrame, bilateralPeakFrame) * 1000 ' Multiply by 1000 to convert to seconds.
    End Function

    ''' <summary>
    ''' Gets the bilateral legs slope from 25% to 50% of the magnitude of the
    ''' first minima to the bilateral peak.
    ''' </summary>
    ''' <returns>Returns the bilateral legs slope from 25% to 50% of the magnitude of the
    ''' first minima to the bilateral peak as a Double. Units returned are Newtons/Second.</returns>
    ''' <remarks></remarks>
    Public Function getBilateral25To50Slope() As Double
        ' Subtract the bilateral peak from the bilateral first minima to get the rise between the points.
        Dim bilateralRise As Double = listBilateralLegs(bilateralPeakFrame) - listBilateralLegs(bilateralFirstMinimaFrame)
        Dim twentyFivePercentValue As Double = (bilateralRise / 4) + listBilateralLegs(bilateralFirstMinimaFrame)
        Dim fiftyPercentValue As Double = (bilateralRise / 2) + listBilateralLegs(bilateralFirstMinimaFrame)
        Dim twentyFivePercentFrame As Integer
        Dim fiftyPercentFrame As Integer

        For i = bilateralFirstMinimaFrame To bilateralPeakFrame
            If (listBilateralLegs(i) > twentyFivePercentValue) Then
                twentyFivePercentFrame = i
                Exit For
            End If
        Next

        For i = bilateralFirstMinimaFrame To bilateralPeakFrame
            If (listBilateralLegs(i) > fiftyPercentValue) Then
                fiftyPercentFrame = i
                Exit For
            End If
        Next

        getBilateral25To50Slope = getSlopeForList(listBilateralLegs, twentyFivePercentFrame, fiftyPercentFrame) * 1000 ' Multiply by 1000 to convert to seconds.
    End Function

    ''' <summary>
    ''' Gets the right arm area from the start of the right arm movement 
    ''' to the end of the right arm movement.
    ''' </summary>
    ''' <returns>Returns the right arm area as a Double. Units returned are Newtons/Second.</returns>
    ''' <remarks></remarks>
    Public Function getRightArmArea() As Double
        getRightArmArea = getAreaForList(listRightArm, rightArmStartFrame, rightArmEndFrame) / 1000 ' Convert to seconds
    End Function

    ''' <summary>
    ''' Gets the left arm area from the start of the left arm movement 
    ''' to the end of the left arm movement.
    ''' </summary>
    ''' <returns>Returns the left arm area as a Double. Units returned are Newtons/Second.</returns>
    ''' <remarks></remarks>
    Public Function getLeftArmArea() As Double
        getLeftArmArea = getAreaForList(listLeftArm, leftArmStartFrame, leftArmEndFrame) / 1000 ' Convert to seconds
    End Function

    ''' <summary>
    ''' Gets the bilateral legs area from the seat off point to the 
    ''' end of the test.
    ''' </summary>
    ''' <returns>Returns the bilateral legs area from the seat off 
    ''' point to the end of the test as a Double. Units returned are Newtons/Second.</returns>
    ''' <remarks></remarks>
    Public Function getBilateralAreaSeatOffToEnd() As Double
        getBilateralAreaSeatOffToEnd = getAreaForList(listBilateralLegs, seatOffFrame, endSTSFrame) / 1000 ' Convert to seconds
    End Function

    ''' <summary>
    ''' Gets the bilateral legs average value from the seat off point to the 
    ''' end of the test.
    ''' </summary>
    ''' <returns>Returns the bilateral legs average value from the seat off 
    ''' point to the end of the test as a Double. Units returned are Newtons.</returns>
    ''' <remarks></remarks>
    Public Function getBilateralLegsAverageSeatOffToEnd() As Double
        getBilateralLegsAverageSeatOffToEnd = getAverageForList(listBilateralLegs, seatOffFrame, endSTSFrame)
    End Function

    ''' <summary>
    ''' Gets the area of a range (specified by the fromIndex, inclusive, and the 
    ''' toIndex, exclusive) of an array that is passed in the function by reference.
    ''' </summary>
    ''' <param name="array">The array to get the area of the range provided.</param>
    ''' <param name="fromIndex">The starting index of the range, inclusive.</param>
    ''' <param name="toIndex">The closing index of the range, exclusive.</param>
    ''' <returns>The area for the range specified in the array as a Double.</returns>
    ''' <remarks></remarks>
    Public Function getAreaForArray(ByRef array As Double(), ByVal fromIndex As Integer, ByVal toIndex As Integer) As Double
        Dim area As Double = 0

        For i = fromIndex To toIndex
            area += array(i)
        Next

        getAreaForArray = area
    End Function

    ''' <summary>
    ''' Gets the average of a range (specified by the fromIndex, inclusive,
    ''' and the toIndex, exclusive) of an array that is passed in the function 
    ''' by reference.
    ''' </summary>
    ''' <param name="array">The array to get the average of the range provided.</param>
    ''' <param name="fromIndex">The starting index of the range, inclusive.</param>
    ''' <param name="toIndex">The closing index of the range, exclusive.</param>
    ''' <returns>Returns the average of the range in the list specified as a Double.</returns>
    ''' <remarks></remarks>
    Public Function getAverageForArray(ByRef array As Double(), ByVal fromIndex As Integer, ByVal toIndex As Integer) As Double
        Dim sum As Double = 0

        For i = fromIndex To toIndex
            sum += array(i)
        Next

        getAverageForArray = sum / (toIndex - fromIndex)
    End Function

    ''' <summary>
    ''' Gets the area of a range (specified by the fromIndex, inclusive, and the 
    ''' toIndex, exclusive) of a list that is passed in the function by reference.
    ''' </summary>
    ''' <param name="list">The list to get the area of the range provided.</param>
    ''' <param name="fromIndex">The starting index of the range, inclusive.</param>
    ''' <param name="toIndex">The closing index of the range, exclusive.</param>
    ''' <returns>The area for the range specified in the list as a Double.</returns>
    ''' <remarks></remarks>
    Public Function getAreaForList(ByRef list As ArrayList, ByVal fromIndex As Integer, ByVal toIndex As Integer)
        Dim area As Double = 0

        For i = fromIndex To toIndex
            area += list(i)
        Next

        getAreaForList = area
    End Function

    ''' <summary>
    ''' Gets the average of a range (specified by the fromIndex, inclusive,
    ''' and the toIndex, exclusive) of a list that is passed in the function 
    ''' by reference.
    ''' </summary>
    ''' <param name="list">The list to get the average of the range provided.</param>
    ''' <param name="fromIndex">The starting index of the range, inclusive.</param>
    ''' <param name="toIndex">The closing index of the range, exclusive.</param>
    ''' <returns>Returns the average of the range in the list specified as a Double.</returns>
    ''' <remarks></remarks>
    Public Function getAverageForList(ByRef list As ArrayList, ByVal fromIndex As Integer, ByVal toIndex As Integer) As Double
        Dim sum As Double = 0

        For i = fromIndex To toIndex
            sum += list(i)
        Next

        getAverageForList = sum / (toIndex - fromIndex)
    End Function

    ''' <summary>
    ''' Gets the average of a list that is passed in the function by reference.
    ''' </summary>
    ''' <param name="list">The list to get the average value of.</param>
    ''' <returns>The average value of the listas a Double.</returns>
    ''' <remarks></remarks>
    Public Function getAverageForList(ByRef list As ArrayList) As Double
        getAverageForList = getAverageForList(list, 0, list.Count)
    End Function

    ''' <summary>
    ''' Gets the slope of a range (specified by the fromIndex, inclusive, and the 
    ''' toIndex, exclusive) of a list that is passed in the function by reference.
    ''' The slope is returned in Newtons/milliseconds.
    ''' </summary>
    ''' <param name="list">The list to get the average of the range provided.</param>
    ''' <param name="fromIndex">The starting index of the range, inclusive.</param>
    ''' <param name="toIndex">The closing index of the range, exclusive.</param>
    ''' <returns>Returns the slope (in Newtons/millisecond) of the range requested as a Double.</returns>
    ''' <remarks></remarks>
    Public Function getSlopeForList(ByRef list As ArrayList, ByVal fromIndex As Integer, ByVal toIndex As Integer) As Double
        getSlopeForList = ((list(toIndex) - list(fromIndex)) / (toIndex - fromIndex))
    End Function
End Module
