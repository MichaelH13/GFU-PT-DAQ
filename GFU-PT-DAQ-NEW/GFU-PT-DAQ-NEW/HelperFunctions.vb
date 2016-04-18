''' <summary>
''' Short, general helper functions to calculate areas, averages, and slopes.
''' </summary>
''' <remarks></remarks>
Module HelperFunctions

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

        If (toIndex > 0 And fromIndex >= 0) Then
            For i = fromIndex To toIndex
                area += array(i)
            Next
            getAreaForArray = area
        Else
            getAreaForArray = INVALID
        End If

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

        If (toIndex > 0 And fromIndex >= 0) Then
            For i = fromIndex To toIndex - 1
                sum += array(i)
            Next
            getAverageForArray = sum / (toIndex - fromIndex)
        Else
            getAverageForArray = INVALID
        End If

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

        If (toIndex > 0 And fromIndex >= 0) Then
            For i = fromIndex To toIndex
                area += list(i)
            Next
            getAreaForList = area
        Else
            getAreaForList = INVALID
        End If

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

        If (toIndex > 0 And fromIndex >= 0) Then
            For i = fromIndex To toIndex - 1
                sum += list(i)
            Next
            getAverageForList = sum / (toIndex - fromIndex)
        Else
            getAverageForList = INVALID
        End If

    End Function

    ''' <summary>
    ''' Gets the average of a list that is passed in the function by reference.
    ''' </summary>
    ''' <param name="list">The list to get the average value of.</param>
    ''' <returns>The average value of the listas a Double.</returns>
    ''' <remarks></remarks>
    Public Function getAverageForList(ByRef list As ArrayList) As Double
        getAverageForList = getAverageForList(list, 0, list.Count - 1)
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
        If (toIndex > 0 And fromIndex >= 0) Then
            getSlopeForList = ((list(toIndex) - list(fromIndex)) / (toIndex - fromIndex))
        Else
            getSlopeForList = INVALID
        End If
    End Function

    ''' <summary>
    ''' Gets the slope from 25% to 50% of the magnitude of the
    ''' first minima to the peak.
    ''' </summary>
    ''' <param name="list">The list to get the slope from.</param>
    ''' <param name="firstMinimaFrame">The first minima for the data in list.</param>
    ''' <param name="peakFrame">The peak frame for the data in list.</param>
    ''' <returns>
    ''' Returns the slope from 25% to 50% of the magnitude of the
    ''' first minima to the peak as a Double. Units returned are Newtons/Second.</returns>
    ''' <remarks></remarks>
    Public Function get25_50Slope(ByRef list As ArrayList, ByVal peakFrame As Integer, ByVal firstMinimaFrame As Integer) As Double
        If (firstMinimaFrame <> INVALID And peakFrame <> INVALID And Not list Is Nothing And list.Count > 0) Then
            ' Subtract the bilateral peak from the bilateral first minima to get the rise between the points.
            Dim rise As Double = list(peakFrame) - list(firstMinimaFrame)
            Dim twentyFivePercentValue As Double = (rise / 4) + list(firstMinimaFrame)
            Dim fiftyPercentValue As Double = (rise / 2) + list(firstMinimaFrame)
            Dim twentyFivePercentFrame As Integer
            Dim fiftyPercentFrame As Integer

            For i = firstMinimaFrame To peakFrame
                If (list(i) > twentyFivePercentValue) Then
                    twentyFivePercentFrame = i
                    Exit For
                End If
            Next

            For i = firstMinimaFrame To peakFrame
                If (list(i) > fiftyPercentValue) Then
                    fiftyPercentFrame = i
                    Exit For
                End If
            Next

            get25_50Slope = getSlopeForList(list, twentyFivePercentFrame, fiftyPercentFrame) * 1000 ' Multiply by 1000 to convert to seconds.
        Else
            get25_50Slope = INVALID
        End If
    End Function

    ''' <summary>
    ''' Removes a range of indexes from a given ArrayList of Arraylists.
    ''' </summary>
    ''' <param name="lists">The list to remove the indexes from.</param>
    ''' <param name="fromIndex">The index to begin the index removing.</param>
    ''' <param name="countToRemove">The number of indexes to remove from the list (including the starting index).</param>
    ''' <remarks></remarks>
    Public Sub removeRangeFromLists(ByRef lists As ArrayList, ByVal fromIndex As Integer, ByVal countToRemove As Integer)
        For i = 0 To lists.Count - 1
            lists(i).RemoveRange(fromIndex, countToRemove)
        Next
    End Sub

End Module
