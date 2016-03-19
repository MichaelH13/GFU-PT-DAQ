Public Class Task
    Private p_i As Integer
    Private p_theTickSize As MccDaq.CounterTickSize
    Private p_myDAQ As MccDaq.MccBoard

    Public Shared Function Task(ByVal i As Integer, ByVal myDaq As MccDaq.MccBoard, ByVal tickSize As MccDaq.CounterTickSize) As Task
        Dim newTask As New Task

        newTask.setTheTickSize(tickSize)
        newTask.setI(i)
        newTask.setBoard(myDaq)

        Task = newTask
    End Function

    Public Sub setI(ByVal i As Integer)
        p_i = i
    End Sub

    Public Sub setTheTickSize(ByVal tickSize As MccDaq.CounterTickSize)
        p_theTickSize = tickSize
    End Sub

    Public Sub setBoard(ByRef myDAQ As MccDaq.MccBoard)
        p_myDAQ = myDAQ
    End Sub

    Public Sub configureChannel()
        p_myDAQ.CConfigScan(p_i, MccDaq.CounterMode.StopAtMax, MccDaq.CounterDebounceTime.DebounceNone, MccDaq.CounterDebounceMode.TriggerAfterStable, MccDaq.CounterEdgeDetection.RisingEdge, p_theTickSize, vbNull)
    End Sub

    'Public Sub runScan()
    '    p_myDAQ.scan()(p_i, MccDaq.CounterMode.StopAtMax, MccDaq.CounterDebounceTime.DebounceNone, MccDaq.CounterDebounceMode.TriggerAfterStable, MccDaq.CounterEdgeDetection.RisingEdge, p_theTickSize, vbNull)
    'End Sub
End Class