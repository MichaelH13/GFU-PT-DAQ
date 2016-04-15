Public Class frmViewOutputVariables

    Private Sub btnSaveVariables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveVariables.Click

        Dim variablesToWriteToFile As New ArrayList

        ' Write Left Leg Variables out to file.
        variablesToWriteToFile.Add("Left Leg First Minima Frame," & Me.txtLeftLegFirstMinima.Text)
        variablesToWriteToFile.Add("Left Leg First Peak Frame," & Me.txtLeftLegPeakFrame.Text)
        variablesToWriteToFile.Add("Left Leg Second Minima Frame," & Me.txtLeftLegSecondMinima.Text)
        variablesToWriteToFile.Add("Left Leg Average Force Seat Off to End of STS," & Me.txtLeftLegAvgForceSeatOffToEndOfSTS.Text)

        ' Write Right Leg Variables out to file.
        variablesToWriteToFile.Add("Right Leg First Minima Frame," & Me.txtRightLegFirstMinima.Text)
        variablesToWriteToFile.Add("Right Leg First Peak Frame," & Me.txtRightLegPeakFrame.Text)
        variablesToWriteToFile.Add("Right Leg Second Minima Frame," & Me.txtRightLegSecondMinima.Text)
        variablesToWriteToFile.Add("Right Leg Average Force Seat Off to End of STS," & Me.txtRightLegAvgForceSeatOffToEndOfSTS.Text)

        ' Write Left Arm Variables out to file.
        variablesToWriteToFile.Add("Left Arm Start Frame," & Me.txtLeftArmStartFrame.Text)
        variablesToWriteToFile.Add("Left Arm Peak Frame," & Me.txtLeftArmPeakFrame.Text)
        variablesToWriteToFile.Add("Left Arm End Frame," & Me.txtLeftArmEndFrame.Text)
        variablesToWriteToFile.Add("Left Arm Area Start Frame to End of Test Frame," & Me.txtLeftArmArea.Text)

        ' Write Right Arm Variables out to file.
        variablesToWriteToFile.Add("Right Arm Start Frame," & Me.txtRightArmStartFrame.Text)
        variablesToWriteToFile.Add("Right Arm Peak Frame," & Me.txtRightArmPeakFrame.Text)
        variablesToWriteToFile.Add("Right Arm End Frame," & Me.txtRightArmEndFrame.Text)
        variablesToWriteToFile.Add("Right Arm Area Start Frame to End Frame," & Me.txtRightArmArea.Text)

        ' Write Bilateral Leg Variables out to file.
        variablesToWriteToFile.Add("Bilateral Legs Start Frame," & Me.txtBilateralLegsStartFrame.Text)
        variablesToWriteToFile.Add("Bilateral Legs Peak Frame," & Me.txtBilateralLegsPeakFrame.Text)
        variablesToWriteToFile.Add("Bilateral Legs End Frame," & Me.txtBilateralLegsEndFrame.Text)
        variablesToWriteToFile.Add("Bilateral Legs First Minima Frame," & Me.txtBilateralLegsFirstMinima.Text)
        variablesToWriteToFile.Add("Bilateral Legs Second Minima Frame," & Me.txtBilateralLegsSecondMinima.Text)
        variablesToWriteToFile.Add("Bilateral Legs 25%-50% Slope," & Me.txtBilateralLegs25_50Slope.Text)
        variablesToWriteToFile.Add("Bilateral Legs Slope," & Me.txtBilateralLegsSlope.Text)
        variablesToWriteToFile.Add("Bilateral Legs Average Force Seat Off to End of STS," & Me.txtRightLegAvgForceSeatOffToEndOfSTS.Text)
        variablesToWriteToFile.Add("Bilateral Legs Area Seat Off to End of Test Frame," & Me.txtBilateralLegsAreaSeatOffToEndOfSTS.Text)
        variablesToWriteToFile.Add("Bilateral Legs Average Force Seat Off to End of Test Frame," & Me.txtBilateralLegsAvgForceSeatOffToEndOfSTS.Text)

        Dim strArrayToWriteToFile(variablesToWriteToFile.Count) As String

        For i = 0 To variablesToWriteToFile.Count - 1
            strArrayToWriteToFile(i) = variablesToWriteToFile(i)
        Next

        ' Show the dialog, save the file if user clicks "OK".
        If (frmSaveTest.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            IO.File.WriteAllLines(frmSaveTest.FileName, strArrayToWriteToFile)
            MsgBox("Saved file!", vbOKOnly, getAppTitle())
            Me.Close()
        End If
    End Sub
End Class