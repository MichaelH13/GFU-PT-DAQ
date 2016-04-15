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

        For i = fromIndex To toIndex - 1
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
        getSlopeForList = ((list(toIndex) - list(fromIndex)) / (toIndex - fromIndex))
    End Function

End Module
