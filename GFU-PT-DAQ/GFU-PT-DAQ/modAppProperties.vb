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

    Public samplingRate As Integer

    Public rightArmOffset As Double
    Public leftArmOffset As Double
    Public rightLegOffset As Double
    Public leftLegOffset As Double
    Public groundOffset As Double
    Public seatOffset As Double
    ' NOTE no bilateralOffset because we offset the legs appropriately.

    Public calibrateDevice As Boolean

    Public Sub mapLists()
        TimeArray = Array.ConvertAll(listTimes.ToArray, Function(s) Single.Parse(s))
        RFArray = Array.ConvertAll(listRightLeg.ToArray, Function(s) Single.Parse(s))
        LFArray = Array.ConvertAll(listLeftLeg.ToArray, Function(s) Single.Parse(s))
        RAArray = Array.ConvertAll(listRightArm.ToArray, Function(s) Single.Parse(s))
        LAArray = Array.ConvertAll(listLeftArm.ToArray, Function(s) Single.Parse(s))
        SeatArray = Array.ConvertAll(listSeat.ToArray, Function(s) Single.Parse(s))

    End Sub

    Public Function getAppTitle() As String
        Return "DAQ"
    End Function

    Public Function getDefaultErrorFormatting(ByVal errorMsg As String) As String
        getDefaultErrorFormatting = "Error " & errorMsg & ", please restart the system and try again. Contact a system administrator if the error occurs again."
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
        listDataPoints.Add(listBilateral)

        Return listDataPoints
    End Function


End Module
