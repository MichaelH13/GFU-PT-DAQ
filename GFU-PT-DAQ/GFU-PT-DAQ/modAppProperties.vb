''' <summary>
''' This module holds core variables and some utility functions.
''' </summary>
''' <remarks></remarks>
Module modAppProperties

    Public Const INVALID As Integer = -1

    ' ArrayLists to hold our data.
    Public listTimes As New ArrayList
    Public listRightArm As New ArrayList
    Public listLeftArm As New ArrayList
    Public listRightLeg As New ArrayList
    Public listLeftLeg As New ArrayList
    Public listGround As New ArrayList
    Public listSeat As New ArrayList
    Public listBilateralLegs As New ArrayList

    Public samplingRate As Double

    ' NOTE: 
    ' No bilateralOffset because we have already offset the legs appropriately.
    Public rightArmOffset As Double
    Public leftArmOffset As Double
    Public rightLegOffset As Double
    Public leftLegOffset As Double
    Public groundOffset As Double
    Public seatOffset As Double

    ' True if we are running a calibration right now.
    Public calibrateDevice As Boolean

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

    ' Average Forces
    Public leftLegAvgForce As Integer = INVALID
    Public rightLegAvgForce As Integer = INVALID

    ' Seat off variable.
    Public seatOffFrame As Integer = INVALID

    Public Function getAppTitle() As String
        Return "DAQ"
    End Function

    Public Function getSamplingRate() As Double
        Return samplingRate 'sample every 10 ms (1/100 second)
    End Function

    Public Function getSecondsPerTest() As Double
        Return 10 'take N seconds to sample the data.
    End Function

    Public Function getTotalSamplesInTest() As Integer
        getTotalSamplesInTest = getSecondsPerTest() * getSamplingRate()
    End Function

    Public Function getDataPoints() As ArrayList
        Dim listDataPoints As New ArrayList
        listDataPoints.Add(listTimes)
        listDataPoints.Add(listRightArm)
        listDataPoints.Add(listLeftArm)
        listDataPoints.Add(listRightLeg)
        listDataPoints.Add(listLeftLeg)
        listDataPoints.Add(listGround)
        listDataPoints.Add(listSeat)
        listDataPoints.Add(listBilateralLegs)

        For i = 0 To 10000 - 1
            listDataPoints(0)(i) = i
        Next

        Return listDataPoints
    End Function

End Module
