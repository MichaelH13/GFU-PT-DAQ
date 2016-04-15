﻿''' <summary>
''' Functions are stored here that get various program variables as 
''' determined by the data provided.
''' </summary>
''' <remarks></remarks>
Module ProgramVariableFunctions

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

End Module