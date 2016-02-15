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
    Public Msg, Style, Title, Response, MyString

End Module
