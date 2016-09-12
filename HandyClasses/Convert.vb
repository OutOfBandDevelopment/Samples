Public Class Converters

#Region "Base64 Encoder/Decoder"
    Const base64 As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/="

    Public Function Base64Encoder(ByVal varInput As String) As String

        Dim InputPart As Int64, varOutput As String
        Dim varIn(0 To 2) As Byte, varOut(0 To 3) As Byte

        varOutput = ""

        Dim varProcess As Integer

        For InputPart = 1 To varInput.Length Step 3

            For varProcess = 0 To 2
                If InputPart + varProcess <= varInput.Length Then
                    varIn(varProcess) = Asc(Mid(varInput, InputPart + varProcess, 1))
                Else
                    varIn(varProcess) = 0
                End If
            Next

            varOut(0) = (varIn(0) And &HFC) / 4
            varOut(1) = (varIn(0) And &H3) * 16 + (varIn(1) And &HF0) / 16
            varOut(2) = (varIn(1) And &HF) * 4 + (varIn(2) And &HC0) / 64
            varOut(3) = (varIn(2) And &H3F)

            For varProcess = 0 To 3
                varOutput = varOutput & Mid(base64, varOut(varProcess) + 1, 1)
            Next

        Next

        If varOutput.Length <> 0 Then
            If Mid(varOutput, varOutput.Length, 1) = "A" Then
                Dim test As String
                test = Mid(varOutput, varOutput.Length - 2, 3)
                If Mid(varOutput, varOutput.Length - 1, 2) = "AA" Then
                    varOutput = Mid(varOutput, 1, varOutput.Length - 2) & "=="
                Else
                    varOutput = Mid(varOutput, 1, varOutput.Length - 1) & "="
                End If
            End If
        End If

        Base64Encoder = varOutput

        InputPart = Nothing
        varOutput = Nothing
        varIn = Nothing
        varOut = Nothing

    End Function

    Public Function Base64Decoder(ByVal varInput As String) As String

        Dim InputPart As Int64, varOutput As String, CleanUpNull As Boolean
        Dim varIn(0 To 3) As Byte, varOut(0 To 2) As Byte

        If InStr(varInput, "=") <> 0 Then
            CleanUpNull = True
        Else
            CleanUpNull = False
        End If
        varOutput = ""

        Dim varProcess As Integer

        For InputPart = 1 To varInput.Length Step 4

            For varProcess = 0 To 3
                If InputPart + varProcess + 1 <= varInput.Length Then
                    varIn(varProcess) = InStr(base64, Mid(varInput, InputPart + varProcess, 1)) - 1
                    If varIn(varProcess) = 64 Then varIn(varProcess) = 0
                Else
                    varIn(varProcess) = 0
                End If

            Next

            varOut(0) = varIn(0) * 4 + (varIn(1) And &H30) / 16
            varOut(1) = (varIn(1) And &HF) * 16 + (varIn(2) And &H3C) / 4
            varOut(2) = (varIn(2) And &H3) * 64 + varIn(3)

            For varProcess = 0 To 2
                varOutput = varOutput & Chr(varOut(varProcess))
            Next


        Next

        If CleanUpNull = True Then
            Dim doneWithIt As Boolean
            doneWithIt = False
            Do Until doneWithIt
                If InStr(varOutput.Length, varOutput, Chr(0)) Then
                    varOutput = Mid(varOutput, 1, varOutput.Length - 1)
                Else
                    doneWithIt = True
                End If
            Loop
        End If

        Base64Decoder = varOutput
        InputPart = Nothing
        varOutput = Nothing
        CleanUpNull = Nothing
        varIn = Nothing
        varOut = Nothing

    End Function

#End Region

#Region "Base32 Encoder/Decoder"
    Const base32 As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567="

    Public Function Base32Encoder(ByVal varInput As String) As String

        Dim InputPart As Int64, varOutput As String
        Dim varIn(0 To 4) As Byte, varOut(0 To 7) As Byte

        varOutput = ""

        Dim varProcess As Integer

        For InputPart = 1 To varInput.Length Step 5

            For varProcess = 0 To 4
                If InputPart + (varProcess) <= varInput.Length Then
                    varIn(varProcess) = Asc(Mid(varInput, InputPart + varProcess, 1))
                Else
                    varIn(varProcess) = 0
                End If
            Next

            varOut(0) = (varIn(0) And &HF8) / 8
            varOut(1) = (varIn(0) And &H7) * 4 + (varIn(1) And &HC0) / 64
            varOut(2) = (varIn(1) And &H3E) / 2
            varOut(3) = (varIn(1) And &H1) * 16 + (varIn(2) And &HF0) / 16
            varOut(4) = (varIn(2) And &HF) * 2 + (varIn(3) And &H80) / 128
            varOut(5) = (varIn(3) And &H7E) / 4
            varOut(6) = (varIn(3) And &H3) * 8 + (varIn(4) And &HE0) / 32
            varOut(7) = (varIn(4) And &H1F)

            For varProcess = 0 To 7
                varOutput = varOutput & Mid(base32, varOut(varProcess) + 1, 1)
            Next

        Next

        If varOutput <> "" Then
            If Mid(varOutput, varOutput.Length, 1) = "A" Then
                Dim test As String
                test = Mid(varOutput, varOutput.Length - 2, 3)
                If Mid(varOutput, varOutput.Length - 1, 2) = "AA" Then
                    If Mid(varOutput, varOutput.Length - 2, 3) = "AAA" Then
                        varOutput = Mid(varOutput, 1, varOutput.Length - 3) & "==="
                    Else
                        varOutput = Mid(varOutput, 1, varOutput.Length - 2) & "=="
                    End If
                Else
                    varOutput = Mid(varOutput, 1, varOutput.Length - 1) & "="
                End If
            End If
        End If

        Base32Encoder = varOutput

        InputPart = Nothing
        varOutput = Nothing
        varProcess = Nothing
        varIn = Nothing
        varOut = Nothing

    End Function

    Public Function Base32Decoder(ByVal varInput As String) As String

        Dim InputPart As Int64, varOutput As String, CleanUpNull As Boolean
        Dim varIn(0 To 7) As Byte, varOut(0 To 4) As Byte

        varInput = UCase(varInput)

        If InStr(varInput, "=") <> 0 Then
            CleanUpNull = True
        Else
            CleanUpNull = False
        End If
        varOutput = ""

        Dim varProcess As Integer

        For InputPart = 1 To varInput.Length Step 8

            For varProcess = 0 To 7
                If InputPart + (varProcess) <= varInput.Length Then
                    varIn(varProcess) = InStr(base32, Mid(varInput, InputPart + (varProcess), 1)) - 1
                    If varIn(varProcess) = 32 Then varIn(varProcess) = 0
                Else
                    varIn(varProcess) = 0
                End If
            Next

            varOut(0) = (varIn(0)) * 8 + (varIn(1) And &H1C) / 4
            varOut(1) = (varIn(1) And &H3) * 64 + (varIn(2)) * 2 + (varIn(3) And &H10) / 16
            varOut(2) = (varIn(3) And &HF) * 16 + (varIn(4) And &H1E) / 2
            varOut(3) = (varIn(4) And &H1) * 128 + (varIn(5)) * 4 + (varIn(6) And &H18) / 8
            varOut(4) = (varIn(6) And &H7) * 32 + (varIn(7))

            For varProcess = 0 To 4
                varOutput = varOutput & Chr(varOut(varProcess))
            Next

        Next

        If CleanUpNull = True Then

            Dim doneWithIt As Boolean
            doneWithIt = False

            Do Until doneWithIt
                If InStr(varOutput.Length, varOutput, Chr(0)) Then
                    varOutput = Mid(varOutput, 1, varOutput.Length - 1)
                Else
                    doneWithIt = True
                End If
            Loop

        End If

        Base32Decoder = varOutput

        InputPart = Nothing
        varOutput = Nothing
        CleanUpNull = Nothing
        varProcess = Nothing
        varIn = Nothing
        varOut = Nothing

    End Function

#End Region

#Region "Base16 Encoder/Decoder"

    Public Function Base16Encoder(ByVal varInput As String) As String

        Dim varOutput As String
        Dim varIn As Byte

        Dim varInputPart As Integer

        varOutput = ""

        For varInputPart = 1 To varInput.Length

            varIn = Asc(Mid(varInput, varInputPart, 1))
            varOutput = varOutput & Hex(varIn)

        Next

        Base16Encoder = varOutput

    End Function

    Public Function Base16Decoder(ByVal varInput As String) As String

        Dim varOutput As String
        Dim varIn As String

        Dim varInputPart As Integer

        varOutput = ""

        For varInputPart = 1 To varInput.Length Step 2

            varIn = Mid(varInput, varInputPart, 2)
            varOutput = varOutput & Chr(Val("&h" & varIn))

        Next

        Base16Decoder = varOutput

    End Function

#End Region

#Region "Base8 Encoder/Decoder"

    Public Function Base8Encoder(ByVal varInput As String) As String

        Dim varOutput As String
        Dim varIn As Byte

        Dim varInputPart As Integer

        varOutput = ""

        For varInputPart = 1 To varInput.Length

            varIn = Asc(Mid(varInput, varInputPart, 1))
            varOutput = varOutput & Oct(varIn)

        Next

        Base8Encoder = (varOutput)

    End Function

    Public Function Base8Decoder(ByVal varInput As String) As String

        Dim varOutput As String
        Dim varIn As String

        Dim varInputPart As Integer

        varOutput = ""

        For varInputPart = 1 To varInput.Length Step 3

            varIn = Mid(varInput, varInputPart, 3)
            varOutput = varOutput & Chr(Val("&o" & varIn))

        Next

        Base8Decoder = varOutput

    End Function

#End Region

End Class
