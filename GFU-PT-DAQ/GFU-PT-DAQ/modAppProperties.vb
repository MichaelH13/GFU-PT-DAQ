Module modAppProperties
    ' ArrayLists to hold our data.
    Public listTimes As New ArrayList
    Public listRightArm As New ArrayList
    Public listLeftArm As New ArrayList
    Public listRightLeg As New ArrayList
    Public listLeftLeg As New ArrayList
    Public listGround As New ArrayList
    Public listSeat As New ArrayList
    Public listBilateral As New ArrayList

    Public rightArmOffset As Double
    Public leftArmOffset As Double
    Public rightLegOffset As Double
    Public leftLegOffset As Double
    Public groundOffset As Double
    Public seatOffset As Double
    ' NOTE no bilateralOffset because we offset the legs appropriately.

    Public calibrateDevice As Boolean

    Public Function getAppTitle() As String
        Return "DAQ"
    End Function

    Public Function getSamplingRate() As Double
        Return 100 'sample every 10 ms (1/100 second)
    End Function

    Public Function getSecondsPerTest() As Double
        Return 1 'take N seconds to sample the data.
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
        listDataPoints.Add(listBilateral)

        Return listDataPoints
    End Function


End Module
