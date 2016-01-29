VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
END
Attribute VB_Name = "Sheet1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = True
Sub AnalyzeSTSData()

'this program is being designed to analyze files from the portable STS device
'The program reads in data from a *.csv file, collected at 1000 hz for 10 seconds
'The program will not work for different data collection parameters
'also to use this program the last second of the 10 second trial the patient must be standing comfortably

'The output of the program is as follows
' Event points  - Start, seat off and end of STS trial
'removes offsets from data and calculates Bilateral vGRF
'calculates discreet variables like slope for the preparation phase and peak and average force for the rising phase


Dim NewFileName As String
Dim TimeArray(1 To 10000) As Single
Dim RFArray(1 To 10000) As Single
Dim LFArray(1 To 10000) As Single
Dim RAArray(1 To 10000) As Single
Dim LAArray(1 To 10000) As Single
Dim SeatArray(1 To 10000) As Single
Dim vGRFBilatArray(1 To 10000) As Single
Dim ArmsBilatArray(1 To 10000) As Single
Dim vGRFDeriv(1 To 10000) As Double
Dim ArmsvGRFsum(1 To 10000) As Single
Dim ArmsvGRFDeriv(1 To 10000) As Single
Dim RFDeriv(1 To 10000) As Single
Dim LFDeriv(1 To 10000) As Single
Dim RADeriv(1 To 10000) As Single
Dim LADeriv(1 To 10000) As Single
Dim PerSTS(1 To 10000), RFArray_v(1 To 10000), LFArray_v(1 To 10000), RAArray_v(1 To 10000), LAArray_v(1 To 10000) As Single
Dim Start50 As Integer
Dim Finish50 As Integer
Dim ws As Worksheet
Dim ChartData As Chart
Dim Msg, Style, Title, Response, MyString
Dim FirstPeak, SecondPeak, ThridPeak As Single
Dim LFPeakForce As Single
Dim RFPeakForce As Single
Dim RAPeakFOrce, LAPeakForce As Single
Dim RDeltaTime25_50, LDeltaTime25_50, RDeltaForce, LDeltaForce As Single

'First get file

'User types in file name here
'NewFileName = Inputbox("Type the exact file name")

'This opens the *.csv file and activates the spreadsheet
'Workbooks.Open Filename:="C:\Users\GFUPT\Desktop\Sit to Stand research Data Collection\" & NewFileName
'Windows(NewFileName).Activate

'For this program to work first open the 'template.xlsx' file
   Set Mybook = Workbooks.Open(Filename:="C:\Users\user\Desktop\Sit to Stand Data Collection\Programs\template.xlsx")
   NewName = InputBox("Type name of the file (eg. STSNH or STSH)")
   Mybook.SaveAs Filename:="C:\Users\user\Desktop\Sit to Stand Data Collection\" + NewName
   NewName = NewName + ".xlsx"
     
'NumberofSubjects = InputBox("Type the number of subjects or trials you will compile (eg. 10)")

'This first loop gets the entire program to loop
'determine the number of loops
NoLoops = InputBox("Number of trials to be processed")
q = 0
For q = 1 To NoLoops

'now open files that are going to be used for input
  MyFile = Application.GetOpenFilename("Text Files, *.csv")
    
    Workbooks.OpenText Filename:=MyFile, _
    Origin:= _
        xlWindows, StartRow:=1, DataType:=xlDelimited, TextQualifier:= _
        xlDoubleQuote, ConsecutiveDelimiter:=False, Tab:=True, Semicolon:=False, _
        Comma:=False, Space:=False, Other:=False, FieldInfo:=Array(Array(1, 1), _
        Array(2, 1), Array(3, 1), Array(4, 1), Array(5, 1), Array(6, 1), Array(7, 1), _
        Array(8, 1))
        
'This code copies the data from the spreadsheet
  SubjectName = ActiveWorkbook.Name
  ActiveSheet.Name = "RawData"
      
'activate the new workbook and worksheets
Workbooks(SubjectName).Activate
Workbooks(SubjectName).Worksheets(1).Select

'This puts the data into an array
i = 0
For i = 1 To 10000

    TimeArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 1).Value
    RAArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 3).Value
    LAArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 4).Value
    RFArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 5).Value
    LFArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 6).Value
    SeatArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 8).Value
    
Next

'Find data. There are two options
'1) the program will attempt to find the points
'2) if the program doesnt find the points then the user may hand enter the points

'1) the program will try to find the points
'Find seat off point
'Offest from the end of the data

'First calculate the bilateral vGRF data
d = 0
For d = 1 To 10000
    vGRFBilatArray(d) = RFArray(d) + LFArray(d)
Next

'Take derivatives of vGRF
s = 0
For s = 100 To 9000
    vGRFDeriv(s) = Abs((vGRFBilatArray(s + 40) - vGRFBilatArray(s - 40)) / (80 * 0.001))
    ArmsvGRFsum(s) = (vGRFBilatArray(s) + RAArray(s) + LAArray(s))
    ArmsvGRFDeriv(s) = Abs((ArmsvGRFsum(s + 40) - ArmsvGRFsum(s - 40)) / (80 * 0.001))
Next

'Find the Start of the STS movement
'get an average
i = 0
FootRefSum = 0
For i = 100 To 300
    FootRefSum = vGRFBilatArray(i) + FootRefSum
Next

FootAverageWt = FootRefSum / 200
FootRef = FootAverageWt - (FootAverageWt * 0.3)

'Find the start of the trial ref
s = 0
For s = 100 To 90000
    If vGRFBilatArray(s) <= FootRef Then
        StartSTSFrame = s
        GoTo 15
    End If
Next

15 StartSTS = (StartSTSFrame / 1000)

'Find the END of the STS movement
ArmsIndicator = InputBox("Is this an 'with arms trial'?", 1)

s = 0
EndSTSSum = 0
For s = 9000 To 9500
    EndSTSSum = vGRFBilatArray(s) + EndSTSSum
Next

EndSTSref = (EndSTSSum / 500)

'Find first minima
s = 0
Count = 0
For s = StartSTSFrame + 100 To 8990
        If (Abs(vGRFDeriv(s))) <= 1 Then
            Count = Count + 1
            If vGRFDeriv(s) < vGRFDeriv(s + 1) Then
                FirstPeakFrame = s - Count
                GoTo 40
            End If
        End If
Next
40

'Find minima - second 0 of the derivative
s = 0
Count = 0
For s = FirstPeakFrame + 200 To 10000
        If (Abs(vGRFDeriv(s))) <= 1 Then
            If vGRFDeriv(s) < vGRFDeriv(s + 1) Then
                SecondPeakFrame = s
                GoTo 42
            End If
        End If
Next
42

'Find the first peak
For s = SecondPeakFrame + 100 To 10000
    If (Abs(vGRFDeriv(s))) <= 1 Then
            If vGRFDeriv(s) < vGRFDeriv(s + 1) Then
                    ThirdPeakFrame = s
                    GoTo 44
            End If
    End If
Next
44

'now find the end of the sts trial
s = 0
For s = ThirdPeakFrame + 50 To 10000
    If vGRFBilatArray(s) >= EndSTSref Then
        EndSTSFrame = s
        GoTo 46
    End If
Next
46

'get standard deviation from 9000 to 9500 frames of seat off data
Workbooks(SubjectName).Sheets(1).Cells(1, 8).Formula = "=StDev(h9009:h9509)"
SDSeat = Workbooks(SubjectName).Sheets(1).Cells(1, 8).Value

'Find the seat ref
s = 0
'get seat off from the seat sensor
SeatRef = SeatArray(9259) + (7 * SDSeat)

Count = 0
For s = 1 To 10000
    If SeatArray(10000 - s) >= SeatRef Then
        seatoffFrame = s
        Count = Count + 1
            If Count = 100 Then
                seatoffFrame = s - 100
                GoTo 14
            End If
    End If
Next

'Here's seat off(start of rising phase)
14 seatoffFrame = 10000 - seatoffFrame
SeatOff = seatoffFrame / 1000

'Check the data
EndSTS = (EndSTSFrame / 1000)
MsgBox ("The start frame is " & StartSTSFrame)
'MsgBox ("The first minima is " & FirstPeakFrame)
MsgBox ("The seat off point is " & seatoffFrame)
MsgBox ("The peak force after seat off is " & SecondPeakFrame)
'MsgBox ("The next minima after the peak is " & ThirdPeakFrame)
MsgBox ("The end of the rising phase is " & EndSTSFrame)


'Set it up so manual inputs can also be used for timing
Manual = InputBox("If you want to Manually pick off times enter '1'")
If Manual = 1 Then
STStiming.Show
 StartSTSFrame = Sheets(1).Cells(2, 14).Value
 seatoffFrame = Sheets(1).Cells(2, 15).Value
 EndSTSFrame = Sheets(1).Cells(2, 16).Value
 
 StartSTS = StartSTSFrame / 1000
 SeatOff = seatoffFrame / 1000
 EndSTS = EndSTSFrame / 1000
 
 RAon = Sheets(1).Cells(2, 17).Value
 RAoff = Sheets(1).Cells(2, 18).Value
 LAon = Sheets(1).Cells(2, 19).Value
 LAoff = Sheets(1).Cells(2, 20).Value
End If

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

'Now clip the data and set the time to percent STS for plottin
LengthSTS = EndSTSFrame - StartSTSFrame
g = 0
For g = 1 To LengthSTS
    If g = 1 Then
        PerSTS(g) = 0
    End If
    If g < LengthSTS Then
        PerSTS(g) = ((g - 1) / LengthSTS)
    End If
    If g = (LengthSTS) Then
        PerSTS(g) = 1
    End If
Next

'Put the raw data into a worksheet and name a worksheet for the discreet variables
'WorkBooks(NewFileName)(.Name = "Raw data" & NewFileName
'Workbooks(NewFileName).Activate

Workbooks(NewName).Activate

'label Columns
'Cells(1, 1).Value = "Frame"
For i = 1 To 6
        Workbooks(NewName).Worksheets(i).Cells(1, q * 2 - 1).Value = "Percent STS"
        Workbooks(NewName).Worksheets(i).Cells(1, q * 2).Value = SubjectName
Next

If q = 1 Then
    Workbooks(NewName).Worksheets(1).Name = "vGRF Bilat"
    Workbooks(NewName).Worksheets(2).Name = "Right Foot"
    Workbooks(NewName).Worksheets(3).Name = "Left Foot"
    Workbooks(NewName).Worksheets(4).Name = "Right Arm"
    Workbooks(NewName).Worksheets(5).Name = "Left Arm"
    Workbooks(NewName).Worksheets(6).Name = "Bilat Arm"
    Workbooks(NewName).Worksheets(7).Name = "Data Points"
End If

'multiply data by regressions to convert voltage to force
a = 0
For a = 1 To 10000
    'use the voltage data to find peaks
    RFArray_v(a) = RFArray(a)
    LFArray_v(a) = LFArray(a)
    RAArray_v(a) = RAArray(a)
    LAArray_v(a) = RAArray(a)
    'convert the data to calculate stuff
    RFArray(a) = (176.11 * RFArray(a)) + 10.42
    LFArray(a) = (180.77 * LFArray(a)) - 9.21
    RAArray(a) = (113.27 * RAArray(a)) + 112.2 '- RAOffset
    LAArray(a) = (234.71 * LAArray(a)) - 18.6 ' - LAOffset))
    ArmsBilatArray(a) = RAArray(a) + LAArray(a)
    vGRFBilatArray(a) = RFArray(a) + LFArray(a)
Next

j = 0
For j = StartSTSFrame To (EndSTSFrame - 1)
        Workbooks(NewName).Worksheets(1).Cells(j - StartSTSFrame + 2, ((q * 2) - 1)).Value = PerSTS(j - StartSTSFrame + 1)
        Workbooks(NewName).Worksheets(1).Cells(j - StartSTSFrame + 2, (q * 2)).Value = vGRFBilatArray(j)
Next

For j = StartSTSFrame To (EndSTSFrame - 1)
        Workbooks(NewName).Worksheets(2).Cells(j - StartSTSFrame + 2, q * 2 - 1).Value = PerSTS(j - StartSTSFrame + 1)
        Workbooks(NewName).Worksheets(2).Cells(j - StartSTSFrame + 2, q * 2).Value = RFArray(j)
Next
        
For j = StartSTSFrame To (EndSTSFrame - 1)
        Workbooks(NewName).Worksheets(3).Cells(j - StartSTSFrame + 2, q * 2 - 1).Value = PerSTS(j - StartSTSFrame + 1)
        Workbooks(NewName).Worksheets(3).Cells(j - StartSTSFrame + 2, q * 2).Value = LFArray(j)
Next
        
For j = StartSTSFrame To (EndSTSFrame - 1)
        Workbooks(NewName).Worksheets(4).Cells(j - StartSTSFrame + 2, q * 2 - 1).Value = PerSTS(j - StartSTSFrame + 1)
        Workbooks(NewName).Worksheets(4).Cells(j - StartSTSFrame + 2, q * 2).Value = RAArray(j)
Next

For j = StartSTSFrame To (EndSTSFrame - 1)
        Workbooks(NewName).Worksheets(5).Cells(j - StartSTSFrame + 2, q * 2 - 1).Value = PerSTS(j - StartSTSFrame + 1)
        Workbooks(NewName).Worksheets(5).Cells(j - StartSTSFrame + 2, q * 2).Value = LAArray(j)
Next

For j = StartSTSFrame To (EndSTSFrame - 1)
        Workbooks(NewName).Worksheets(6).Cells(j - StartSTSFrame + 2, q * 2 - 1).Value = PerSTS(j - StartSTSFrame + 1)
        Workbooks(NewName).Worksheets(6).Cells(j - StartSTSFrame + 2, q * 2).Value = ArmsBilatArray(j)
Next

'This section grabs the peak data from the trials
'we start with the same process as for the bilateral data which is used to detect the global events
'So we will take the derivative of each foot and arm and then get are points
'we start with the rising phase then do the preparation phase variables

'Find first peak
'first take derivative
For i = FirstPeakFrame To EndSTSFrame
    RFDeriv(i) = Abs((RFArray_v(i + 40) - RFArray_v(i - 40)) / (80 * 0.001))
    LFDeriv(i) = Abs((LFArray_v(i + 40) - LFArray_v(i - 40)) / (80 * 0.001))
Next

'find 1st 0 point of derivative
RFPeakForce = 0
Count = 0
For i = (FirstPeakFrame + 200) To (ThirdPeakFrame + 100)
    If RFDeriv(i) < 1 Then
            RFPeakFrame = i
            GoTo 70
    End If
Next
70

LFPeakForce = 0
For i = (FirstPeakFrame + 200) To (ThirdPeakFrame + 200)
    If LFDeriv(i) < 1 Then
            LFPeakFrame = i
            GoTo 61
    End If
Next
61

'Calculate average Force
'first sum the forces
RFTotal = 0
LFTotal = 0
For i = seatoffFrame To EndSTSFrame
    RFTotal = RFTotal + RFArray(i)
    LFTotal = LFTotal + LFArray(i)
Next

'then average them
RFAverage = RFTotal / (EndSTSFrame - seatoffFrame)
LFAverage = LFTotal / (EndSTSFrame - seatoffFrame)

'Calculate area for rising phase
RFArea = 0
LFArea = 0
For i = seatoffFrame To EndSTSFrame
    RFArea = RFArray(i) * 0.001 + RFArea
    LFArea = LFArray(i) * 0.001 + RFArea
Next

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
80
i = 0
For i = FirstPeakFrame To LFPeakFrame
        If LFArray(i) > (LFArray(LFPeakFrame) * 0.5) Then
            LFFinish50 = i
            GoTo 81
        End If
Next
81

i = 0
For i = FirstPeakFrame To RFPeakFrame
        If RFArray(i) > (RFArray(RFPeakFrame) * 0.25) Then
            RFStart25 = i
            GoTo 82
        End If
Next
82
i = 0
For i = FirstPeakFrame To LFPeakFrame
        If LFArray(i) > (LFArray(LFPeakFrame) * 0.25) Then
            LFStart25 = i
            GoTo 83
        End If
Next
83

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
        
20
    s = 0
    For s = StartSTSFrame To 10000
        If RAArray(10000 - (RAon + s)) >= 10 Then
            RAoff = (10000 - (s + RAon))
            GoTo 21
        End If
    Next
    
21
    s = 0
    For s = StartSTSFrame To EndSTSFrame
        If LAArray(s) >= 10 Then
            LAon = s
            GoTo 22
        End If
    Next
22
    s = 0
    For s = StartSTSFrame To 10000
        If LAArray(10000 - (LAon + s)) >= 10 Then
            LAoff = (10000 - (LAon + s))
            GoTo 23
        End If
    Next
23
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
        RADeriv(i) = Abs((RAArray_v(i + 40) - RAArray_v(i - 40)) / (80 * 0.001))
        LADeriv(i) = Abs((LAArray_v(i + 40) - LAArray_v(i - 40)) / (80 * 0.001))
    Next
    
    'find 1st 0 point of derivative
    RAPeakFOrce = 0
    For i = RAon To RAon
        If RADeriv(i) < 1 Then
            RAPeakForceFrame = i
            GoTo 71
        End If
    Next
71
    
    LAPeakForce = 0
    For i = LAon To LAoff
        If LADeriv(i) < 1 Then
            LAPeakForceFrame = i
            GoTo 72
        End If
    Next
End If
72
50

'add discreet data to a sheet
'Labels
If q = 1 Then
Workbooks(NewName).Worksheets(7).Cells(1, q).Value = "StartSTS"
Workbooks(NewName).Worksheets(7).Cells(2, q).Value = "SeatOff"
Workbooks(NewName).Worksheets(7).Cells(3, q).Value = "EndSTS"
Workbooks(NewName).Worksheets(7).Cells(4, q).Value = "Right 25-50"
Workbooks(NewName).Worksheets(7).Cells(5, q).Value = "Left 25-50"
Workbooks(NewName).Worksheets(7).Cells(6, q).Value = "Right 0-100"
Workbooks(NewName).Worksheets(7).Cells(7, q).Value = "Left 0-100"
Workbooks(NewName).Worksheets(7).Cells(8, q).Value = "RF Peak Force"
Workbooks(NewName).Worksheets(7).Cells(9, q).Value = "Left Peak Force"
Workbooks(NewName).Worksheets(7).Cells(10, q).Value = "Left Average Force"
Workbooks(NewName).Worksheets(7).Cells(11, q).Value = "Left Average Force"
Workbooks(NewName).Worksheets(7).Cells(12, q).Value = "Left Area"
Workbooks(NewName).Worksheets(7).Cells(13, q).Value = "Right Area"
Workbooks(NewName).Worksheets(7).Cells(14, q).Value = "Right Peak Arm Force"
Workbooks(NewName).Worksheets(7).Cells(15, q).Value = "Left Peak Arm Force"
Workbooks(NewName).Worksheets(7).Cells(16, q).Value = "Right Arm Area"
Workbooks(NewName).Worksheets(7).Cells(17, q).Value = "Left Arm Area"
End If

'now put the data next to the label
Workbooks(NewName).Worksheets(7).Cells(1, q + 1).Value = StartSTS
Workbooks(NewName).Worksheets(7).Cells(2, q + 1).Value = SeatOff
Workbooks(NewName).Worksheets(7).Cells(3, q + 1).Value = EndSTS
Workbooks(NewName).Worksheets(7).Cells(4, q + 1).Value = RFSlope25_50
Workbooks(NewName).Worksheets(7).Cells(5, q + 1).Value = LFSlope25_50
Workbooks(NewName).Worksheets(7).Cells(6, q + 1).Value = RFSlope100
Workbooks(NewName).Worksheets(7).Cells(7, q + 1).Value = LFSlope100
Workbooks(NewName).Worksheets(7).Cells(8, q + 1).Value = RFArray(RFPeakFrame)
Workbooks(NewName).Worksheets(7).Cells(9, q + 1).Value = LFArray(RFPeakFrame)
Workbooks(NewName).Worksheets(7).Cells(10, q + 1).Value = RFAverage
Workbooks(NewName).Worksheets(7).Cells(11, q + 1).Value = LFAverage
Workbooks(NewName).Worksheets(7).Cells(12, q + 1).Value = LFArea
Workbooks(NewName).Worksheets(7).Cells(13, q + 1).Value = RFArea

If ArmsIndicator = 0 Then
    RAPeakForceFrame = 1
    LAPeakForceFrame = 1
End If
    
Workbooks(NewName).Worksheets(7).Cells(14, q + 1).Value = RAArray(RAPeakForceFrame)
Workbooks(NewName).Worksheets(7).Cells(15, q + 1).Value = LAArray(LAPeakForceFrame)
Workbooks(NewName).Worksheets(7).Cells(16, q + 1).Value = RAArea
Workbooks(NewName).Worksheets(7).Cells(17, q + 1).Value = LAArea

'This 'next' is for the end of the overall loop
Next q

'now average all the data
Workbooks(NewName).Worksheets(7).Cells(2, q + 3).Formula = "= b3-b1"
Workbooks(NewName).Worksheets(7).Cells(2, q + 4).Formula = "= c3-c1"
Workbooks(NewName).Worksheets(7).Cells(2, q + 5).Formula = "= d3-d1"
Workbooks(NewName).Worksheets(7).Cells(2, q + 6).Formula = "= AVERAGE(f2:H2)"
Workbooks(NewName).Worksheets(7).Cells(4, q + 2).Formula = "= AVERAGE(B4:D4)"
Workbooks(NewName).Worksheets(7).Cells(5, q + 2).Formula = "= average(b5:d5)"
Workbooks(NewName).Worksheets(7).Cells(6, q + 2).Formula = "= average(b6:d6)"
Workbooks(NewName).Worksheets(7).Cells(7, q + 2).Formula = "= average(b7:d7)"
Workbooks(NewName).Worksheets(7).Cells(8, q + 2).Formula = "= average(b8:d8)"
Workbooks(NewName).Worksheets(7).Cells(9, q + 2).Formula = "= average(b9:d9)"
Workbooks(NewName).Worksheets(7).Cells(10, q + 2).Formula = "= average(b10:d10)"
Workbooks(NewName).Worksheets(7).Cells(11, q + 2).Formula = "= average(b11:d11)"
Workbooks(NewName).Worksheets(7).Cells(12, q + 2).Formula = "= average(b12:d12)"
Workbooks(NewName).Worksheets(7).Cells(13, q + 2).Formula = "= average(b13:d13)"
Workbooks(NewName).Worksheets(7).Cells(14, q + 2).Formula = "= average(b14:d14)"
Workbooks(NewName).Worksheets(7).Cells(15, q + 2).Formula = "= average(b15:d15)"
Workbooks(NewName).Worksheets(7).Cells(16, q + 2).Formula = "= average(b16:d16)"
Workbooks(NewName).Worksheets(7).Cells(17, q + 2).Formula = "= average(b17:d17)"

'graph data vGRF Bil, seat, RA and LA arrays= "=AVERAGE(RC[-1])"
For i = 1 To 7
Workbooks(NewName).Worksheets(i).Activate
Worksheets(i).Range("b1:f1600").Select

Charts.Add
 ActiveChart.ChartType = xlXYScatterSmoothNoMarkers
 ActiveChart.Location Where:=xlLocationAsNewSheet
Next

Sheets("Chart1").Select
MsgBox ("Oh la la! All done!  Ready for another!")

End Sub
Private Sub ManFindSTS_Times()
'this program is being designed to analyze files from the portable STS device
'The program reads in data from a *.csv file, collected at 1000 hz for 10 seconds
'The program will not work for different data collection parameters
'also to use this program the last second of the 10 second trial the patient must be standing comfortably

'The output of the program is as follows
' Event points  - Start, seat off and end of STS trial
'removes offsets from data and calculates Bilateral vGRF
'calculates discreet variables like slope for the preparation phase and peak and average force for the rising phase


Dim NewFileName As String
Dim TimeArray(1 To 10000) As Single
Dim RFArray(1 To 10000) As Single
Dim LFArray(1 To 10000) As Single
Dim RAArray(1 To 10000) As Single
Dim LAArray(1 To 10000) As Single
Dim SeatArray(1 To 10000) As Single
Dim vGRFBilatArray(1 To 10000) As Single
Dim ArmsBilatArray(1 To 10000) As Single
Dim PerSTS(1 To 10000) As Single
Dim Start50 As Integer
Dim Finish50 As Integer
Dim ws As Worksheet
Dim ChartData As Chart
Dim Msg, Style, Title, Response, MyString

'Allow the program to loop through some number of trials
'NumberofTrails = InputBox("How many trials do you want to work on?")

'First get file

'User types in file name here
'NewFileName = Inputbox("Type the exact file name")

'This opens the *.csv file and activates the spreadsheet
'Workbooks.Open Filename:="C:\Users\GFUPT\Desktop\Sit to Stand research Data Collection\" & NewFileName
'Windows(NewFileName).Activate

'For this program to work first open the 'template.xlsx' file
  ' Set Mybook = Workbooks.Open(Filename:="C:\Users\GFUPT\Desktop\Sit to Stand research Data Collection\programs\" + "template.xlsx")
  ' NewName = InputBox("Type name of the file (eg. STSNH or STSH)")
  ' Mybook.SaveAs Filename:="C:\Users\GFUPT\Desktop\Sit to Stand research Data Collection\" + NewName
     
Set Mybook = Workbooks.Open(Filename:="C:\Users\user\Desktop\Sit to Stand Data Collection\Programs\template.xlsx")
   NewName = InputBox("Type name of the file (eg. STSNH or STSH)")
   Mybook.SaveAs Filename:="C:\Users\user\Desktop\Sit to Stand Data Collection\" + NewName
   NewName = NewName + ".xlsx"
     

'NumberofSubjects = InputBox("Type the number of subjects or trials you will compile (eg. 10)")

'now open files that are going to be used for input
  MyFile = Application.GetOpenFilename("Text Files, *.csv")
    
    Workbooks.OpenText Filename:=MyFile, _
    Origin:= _
        xlWindows, StartRow:=1, DataType:=xlDelimited, TextQualifier:= _
        xlDoubleQuote, ConsecutiveDelimiter:=False, Tab:=True, Semicolon:=False, _
        Comma:=False, Space:=False, Other:=False, FieldInfo:=Array(Array(1, 1), _
        Array(2, 1), Array(3, 1), Array(4, 1), Array(5, 1), Array(6, 1), Array(7, 1), _
        Array(8, 1))
        
'This code copies the data from the spreadsheet
  SubjectName = ActiveWorkbook.Name
  ActiveSheet.Name = "RawData"
      
'activate the new workbook and worksheets
Workbooks(SubjectName).Activate
Workbooks(SubjectName).Worksheets(1).Select

'This puts the data into an array
i = 0
For i = 1 To 10000

    TimeArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 1).Value
    RAArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 3).Value
    LAArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 4).Value
    RFArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 5).Value
    LFArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 6).Value
    SeatArray(i) = Workbooks(SubjectName).Worksheets("RawData").Cells(i + 8, 8).Value
    
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

Windows(NewName).Activate

'label Columns
'Cells(1, 1).Value = "Frame"
'For i = 1 To 6
    Workbooks(NewName).Worksheets(1).Cells(1, 1).Value = "Frame(ms) STS"
    Workbooks(NewName).Worksheets(1).Cells(1, 2).Value = "vGRF"
    Workbooks(NewName).Worksheets(1).Cells(1, 3).Value = "Seat"
    
    Workbooks(NewName).Worksheets(1).Cells(1, 5).Value = "Frame(ms) STS"
    Workbooks(NewName).Worksheets(1).Cells(1, 6).Value = "RArm"
    Workbooks(NewName).Worksheets(1).Cells(1, 7).Value = "LArm"

    Workbooks(NewName).Worksheets(1).Name = "Data"
    
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
        
    Workbooks(NewName).Worksheets(1).Cells(j, 1).Value = j
    Workbooks(NewName).Worksheets(1).Cells(j, 2).Value = vGRFBilatArray(j)
    Workbooks(NewName).Worksheets(1).Cells(j, 3).Value = SeatArray(j)
    
    Workbooks(NewName).Worksheets(1).Cells(j, 5).Value = j
    Workbooks(NewName).Worksheets(1).Cells(j, 6).Value = RAArray(j)
    Workbooks(NewName).Worksheets(1).Cells(j, 7).Value = LAArray(j)

Next

'graph data vGRF Bil, seat, RA and LA arrays
Workbooks(NewName).Worksheets(1).Activate
ActiveSheet.Range("b1:b10000").Select

Charts.Add
 ActiveChart.ChartType = xlXYScatterSmoothNoMarkers
 ActiveChart.Location Where:=xlLocationAsNewSheet
 
 Workbooks(NewName).Worksheets(1).Activate
 ActiveSheet.Range("c1:c10000").Select

Charts.Add
 ActiveChart.ChartType = xlXYScatterSmoothNoMarkers
 ActiveChart.Location Where:=xlLocationAsNewSheet
    
 Workbooks(NewName).Worksheets(1).Activate
 ActiveSheet.Range("f1:f10000").Select

Charts.Add
 ActiveChart.ChartType = xlXYScatterSmoothNoMarkers
 ActiveChart.Location Where:=xlLocationAsNewSheet
       
  Workbooks(NewName).Worksheets(1).Activate
 ActiveSheet.Range("g1:g10000").Select

Charts.Add
 ActiveChart.ChartType = xlXYScatterSmoothNoMarkers
 ActiveChart.Location Where:=xlLocationAsNewSheet

Sheets("Chart1").Select
 
End Sub
Sub maxima()

'peak force and average from each leg from seat off to end of STS
'Find first peak
i = 0
For i = SeatOff To EndSTSFrame
    If RFArray(i) > RFArray(i + 1) Then
            RFPeakForce = RFArray(i)
                If RFArray(i) < RFArray(i + 1) Then
                   GoTo 9
                End If
    End If
 Next
 
9 i = 0
 For i = SeatOff To EndSTSFrame
    If LFArray(i) > LFArray(i + 1) Then
            LFPeakForce = LFArray(i)
                If LFArray(i) < LFArray(i + 1) Then
                GoTo 10
                End If
    End If
Next
End Sub
Sub junk()

End Sub
