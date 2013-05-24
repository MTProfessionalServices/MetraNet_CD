Attribute VB_Name = "Module1"

Option Explicit
Option Base 0

' basTestBlowfish: Tests of Blowfish function

' New version: published January 2002

'************************* COPYRIGHT NOTICE*************************
' This code was originally written in Visual Basic by David Ireland
' and is copyright (c) 2000-2 D.I. Management Services Pty Limited,
' all rights reserved.

' You are free to use this code as part of your own applications
' provided you keep this copyright notice intact and acknowledge
' its authorship with the words:

'   "Contains cryptography software by David Ireland of
'   DI Management Services Pty Ltd <www.di-mgt.com.au>."

' If you use it as part of a web site, please include a link
' to our site in the form
' <A HREF="http://www.di-mgt.com.au/crypto.html">Cryptography
' Software Code</a>

' This code may only be used as part of an application. It may
' not be reproduced or distributed separately by any means without
' the express written permission of the author.

' David Ireland and DI Management Services Pty Limited make no
' representations concerning either the merchantability of this
' software or the suitability of this software for any particular
' purpose. It is provided "as is" without express or implied
' warranty of any kind.

' Please forward comments or bug reports to <code@di-mgt.com.au>.
' The latest version of this source code can be downloaded from
' www.di-mgt.com.au/crypto.html.
'****************** END OF COPYRIGHT NOTICE*************************

' Public Functions in this module are:
' TestStrEnc(): Encrypts and decrypts a standard hex block in ECB mode
' TestStrEncCBC(): ditto using CBC mode
' BlowfishTestSuite(): carries out Eric Young's Blowfish test suite
' TestFileEnc(): Encrypts a file using blf_FileEnc()
' TestFileDec(): Decrypts a file using blf_FileDec()
' TestFileEncViaString(): Encrypts a file via a string
' TestFileEncCBC(): Encrypts a file using CBC mode
' TestFileDecCBC(): Decrypts a file using CBC mode
' ReadFileIntoString(sFilePath): Returns string of file contents
' WriteFileFromString(sFilePath): Creates a file from a string

Private Type SuiteSet
' Used to store test suite
    Key As String * 16
    Plain As String * 16
    Cipher As String * 16
End Type

Public Sub main()
    
    'TestStrEnc
    'TestStrEncCBC
    Dim p As New CPoissonSuceur
    MsgBox p.UnitTest
End Sub

Public Function TestStrEnc() As Boolean
' Example showing encryption and decryption of standard hex block
' This should produce the output:
'   KY=FEDCBA9876543210
'   PT=0123456789ABCDEF
'   CT=0ACEAB0FC6A0A28D
'   OK=0ACEAB0FC6A0A28D
'   P '=0123456789ABCDEF

    Dim sData As String
    Dim sCipher As String
    Dim sPlain As String
    Dim aKey() As Byte

    ' Load test key and initialise
    aKey() = cv_BytesFromHex("FEDCBA9876543210")
    Call blf_KeyInit(aKey)
    
    ' Convert data in hex format to a string
    sData = cv_StringFromHex("0123456789ABCDEF")
    
    ' Encipher the string
    sCipher = blf_StringRaw(sData, True)
    ' Then decipher it
    sPlain = blf_StringRaw(sCipher, False)
    
    ' Output results
    Debug.Print "KY=" & cv_HexFromBytes(aKey)
    Debug.Print "PT=" & cv_HexFromString(sData)
    Debug.Print "CT=" & cv_HexFromString(sCipher)
    Debug.Print "OK=" & "0ACEAB0FC6A0A28D"  ' Correct answer
    Debug.Print "P'=" & cv_HexFromString(sPlain)
End Function

Public Function TestStrEncCBC() As Boolean
' Example showing encryption and decryption in Cipher Block Chaining mode
' This is a standard test vector by Eric Young
    Dim sData As String
    Dim sCipher As String
    Dim sPlain As String
    Dim aKey() As Byte

    sData = "7654321 Now is the time for " & Chr$(0) & Chr$(0) & Chr$(0) & Chr$(0)
    
    ' NB blf_StringEncCBC will add an extra block of 8 bytes to the test vector
    ' because of padding.
    ' Correct output from this procedure in hex is:
' 6B77B4D63006DEE605B156E27403979358DEB9E7154616D959F1652BD5FF92CCEC0444132BC46E49
    ' whereas Eric Young's test vector produces:-
' 6B77B4D63006DEE605B156E27403979358DEB9E7154616D959F1652BD5FF92CC

    ' Load test key and initialise
    aKey() = cv_BytesFromHex("0123456789ABCDEFF0E1D2C3B4A59687")
    Call blf_KeyInit(aKey)
    
    ' Encipher the string
    sCipher = blf_StringEncCBC(sData, "FEDCBA9876543210")
    ' Then decipher it
    sPlain = blf_StringDecCBC(sCipher, "FEDCBA9876543210")
    
    ' Output results
    Debug.Print "KY="; cv_HexFromBytes(aKey)
    Debug.Print "IV="; "FEDCBA9876543210"
    Debug.Print "PT="; sData
    Debug.Print "CT="; cv_HexFromString(sCipher)
    Debug.Print "OK="; "6B77B4D63006DEE605B156E27403979358DEB9E7154616D959F1652BD5FF92CCEC0444132BC46E49"
    Debug.Print "P'="; sPlain
End Function

Public Function TestStrEncRawCBC() As Boolean
' Example showing encryption and decryption in Cipher Block Chaining mode
' without padding
' This is a standard test vector by Eric Young
    Dim sData As String
    Dim sCipher As String
    Dim sPlain As String
    Dim aKey() As Byte

    sData = "7654321 Now is the time for " & Chr$(0) & Chr$(0) & Chr$(0) & Chr$(0)
    
    ' Load test key and initialise
    aKey() = cv_BytesFromHex("0123456789ABCDEFF0E1D2C3B4A59687")
    Call blf_KeyInit(aKey)
    
    ' Encipher the string
    sCipher = blf_StringEncRawCBC(sData, "FEDCBA9876543210")
    ' Then decipher it
    sPlain = blf_StringDecRawCBC(sCipher, "FEDCBA9876543210")
    
    ' Output results
    Debug.Print "KY="; cv_HexFromBytes(aKey)
    Debug.Print "IV="; "FEDCBA9876543210"
    Debug.Print "PT="; sData
    Debug.Print "CT="; cv_HexFromString(sCipher)
    Debug.Print "OK="; "6B77B4D63006DEE605B156E27403979358DEB9E7154616D959F1652BD5FF92CC"
    Debug.Print "P'="; sPlain
End Function

Public Function BlowfishTestSuite()
    ' Data from test suite by Eric Young
    Const NTESTS As Integer = 34    ' # of sets in suite
    Dim aSuite(NTESTS - 1) As SuiteSet
    Dim vntA As Variant
    Dim i As Integer, j As Integer
    Dim aKey() As Byte
    Dim sPlain As String
    Dim sCipher As String
    Dim sBack As String
    
    ' Read in test suite as array of strings using array of variants
    ' These values are taken directly from the C code by Eric Young.
'       key bytes               clear bytes             cipher bytes
    vntA = Array( _
        "0000000000000000", "0000000000000000", "4EF997456198DD78", _
        "FFFFFFFFFFFFFFFF", "FFFFFFFFFFFFFFFF", "51866FD5B85ECB8A", _
        "3000000000000000", "1000000000000001", "7D856F9A613063F2", _
        "1111111111111111", "1111111111111111", "2466DD878B963C9D", _
        "0123456789ABCDEF", "1111111111111111", "61F9C3802281B096", _
        "1111111111111111", "0123456789ABCDEF", "7D0CC630AFDA1EC7", _
        "0000000000000000", "0000000000000000", "4EF997456198DD78", _
        "FEDCBA9876543210", "0123456789ABCDEF", "0ACEAB0FC6A0A28D", _
        "7CA110454A1A6E57", "01A1D6D039776742", "59C68245EB05282B", _
        "0131D9619DC1376E", "5CD54CA83DEF57DA", "B1B8CC0B250F09A0", _
        "07A1133E4A0B2686", "0248D43806F67172", "1730E5778BEA1DA4", _
        "3849674C2602319E", "51454B582DDF440A", "A25E7856CF2651EB", _
        "04B915BA43FEB5B6", "42FD443059577FA2", "353882B109CE8F1A", _
        "0113B970FD34F2CE", "059B5E0851CF143A", "48F4D0884C379918", _
        "0170F175468FB5E6", "0756D8E0774761D2", "432193B78951FC98", _
        "43297FAD38E373FE", "762514B829BF486A", "13F04154D69D1AE5", _
        "07A7137045DA2A16", "3BDD119049372802", "2EEDDA93FFD39C79")
        
    For i = 0 To 16
        aSuite(i).Key = vntA(i * 3)
        aSuite(i).Plain = vntA(i * 3 + 1)
        aSuite(i).Cipher = vntA(i * 3 + 2)
    Next
    
    vntA = Array( _
        "04689104C2FD3B2F", "26955F6835AF609A", "D887E0393C2DA6E3", _
        "37D06BB516CB7546", "164D5E404F275232", "5F99D04F5B163969", _
        "1F08260D1AC2465E", "6B056E18759F5CCA", "4A057A3B24D3977B", _
        "584023641ABA6176", "004BD6EF09176062", "452031C1E4FADA8E", _
        "025816164629B007", "480D39006EE762F2", "7555AE39F59B87BD", _
        "49793EBC79B3258F", "437540C8698F3CFA", "53C55F9CB49FC019", _
        "4FB05E1515AB73A7", "072D43A077075292", "7A8E7BFA937E89A3", _
        "49E95D6D4CA229BF", "02FE55778117F12A", "CF9C5D7A4986ADB5", _
        "018310DC409B26D6", "1D9D5C5018F728C2", "D1ABB290658BC778", _
        "1C587F1C13924FEF", "305532286D6F295A", "55CB3774D13EF201", _
        "0101010101010101", "0123456789ABCDEF", "FA34EC4847B268B2", _
        "1F1F1F1F0E0E0E0E", "0123456789ABCDEF", "A790795108EA3CAE", _
        "E0FEE0FEF1FEF1FE", "0123456789ABCDEF", "C39E072D9FAC631D", _
        "0000000000000000", "FFFFFFFFFFFFFFFF", "014933E0CDAFF6E4", _
        "FFFFFFFFFFFFFFFF", "0000000000000000", "F21E9A77B71C49BC", _
        "0123456789ABCDEF", "0000000000000000", "245946885754369A", _
        "FEDCBA9876543210", "FFFFFFFFFFFFFFFF", "6B5C5A9C5D9E0A5A")
        
    For i = 0 To 16
        aSuite(i + 17).Key = vntA(i * 3)
        aSuite(i + 17).Plain = vntA(i * 3 + 1)
        aSuite(i + 17).Cipher = vntA(i * 3 + 2)
    Next
  
    For i = 0 To NTESTS - 1
        Debug.Print "Test " & i + 1
        Debug.Print "Key=" & aSuite(i).Key & " Plain Data=" & aSuite(i).Plain
        
        ' Convert key to an array of 8 bytes, plain to an ascii string
        aKey() = cv_BytesFromHex(aSuite(i).Key)
        sPlain = cv_StringFromHex(aSuite(i).Plain)
        Debug.Print "Correct Cipher=" & aSuite(i).Cipher
        
        ' Encrypt just the raw block with new key
        Call blf_KeyInit(aKey)
        sCipher = blf_StringRaw(sPlain, bEncrypt:=True)
    
        Debug.Print "Calc'd Cipher =" & cv_HexFromString(sCipher)
        ' Check we did it
        If aSuite(i).Cipher <> cv_HexFromString(sCipher) Then
            MsgBox "Failed to get correct cipher on round " & i
        End If
    
        ' Now decipher back to plaintext, again just the raw block
        sBack = blf_StringRaw(sCipher, bEncrypt:=False)
        Debug.Print "Converted Back to Plain=        " & cv_HexFromString(sBack)
        
        If aSuite(i).Plain <> cv_HexFromString(sBack) Then
            MsgBox "Failed to get correct plain on round " & i
        End If
    Next
    
End Function

Public Function TestFileEnc() As Boolean
' Example encrypting file using blf_FileEnc()
' Input file is the 13 bytes:-
' 000000  68 65 6c 6c 6f 20 77 6f 72 6c 64 0d 0a           hello world..
' Output file should be the 16 bytes:-
' 000000  1a a1 51 b7 7a 5a 33 5c 4e 7e dc 84 a3 86 dc 96  .�Q�zZ3\N~�.�.�.

    Dim sFileIn As String
    Dim sFileOut As String
    Dim aKey() As Byte

    sFileIn = "C:\test\hello.txt"
    sFileOut = "C:\test\hello.enc"
    
    Debug.Print "Processing..."
    ' Load test key and initialise
    aKey() = cv_BytesFromHex("FEDCBA9876543210")
    Call ap_StartTimer
    Call blf_KeyInit(aKey)
    
    ' Encipher the file
    Call blf_FileEnc(sFileIn, sFileOut)
    Debug.Print "By blf_FileEnc: " & ap_EndTimer & " millisecs"
End Function

Public Function TestFileDec() As Boolean
' Decrypting an encrypted file
' This should produce an identical file to "hello.txt"
    Dim sFileIn As String
    Dim sFileOut As String
    Dim aKey() As Byte

    sFileIn = "C:\test\hello.enc"
    sFileOut = "C:\test\hello.dec"
    
    Debug.Print "Processing..."
    ' Load test key and initialise
    aKey() = cv_BytesFromHex("FEDCBA9876543210")
    Call ap_StartTimer
    Call blf_KeyInit(aKey)
    
    ' Encipher the file
    Call blf_FileDec(sFileIn, sFileOut)
    Debug.Print "By blf_FileDec: " & ap_EndTimer & " millisecs"
End Function

Public Function TestFileEncViaString()
' Shows how a file could be read into a string and then encrypted
' Try timing tests on your computer to see which is faster
' blf_FileEnc or blf_StringEnc
    Dim sFilePath As String
    Dim sFileOut As String
    Dim sFileDec As String
    Dim strIn As String
    Dim strOut As String
    Dim aKey() As Byte
    Dim i As Long
    Dim lTime As Long
    
    sFilePath = "C:\test\sonnets.txt"
    sFileOut = "C:\test\sonnets.enc"
    sFileDec = "C:\test\sonnets.dec"
    
    ' Start the timer
    Call ap_StartTimer
    
    ' Read the file into a string ready to process
    strIn = ReadFileIntoString(sFilePath)
    Debug.Print "File is " & Len(strIn) & " bytes"
    
    ' Now set up key (=0x3132333435)
    aKey() = StrConv("12345", vbFromUnicode)
        
    ' And initialise Blowfish
    Call blf_KeyInit(aKey())
    
    ' Encrypt it
    strOut = blf_StringEnc(strIn)
    ' Save as a file
    Call WriteFileFromString(sFileOut, strOut)
    
    ' Decrypt it
    strOut = blf_StringDec(strOut)
    
    ' Now write to a file
    Call WriteFileFromString(sFileDec, strOut)
    
    ' This decrypted file should be identical to the original
    ' Use the DOS file compare command (fc) to check:-
    '   C:\Test>fc sonnets.txt sonnets.dec
    '   Comparing files sonnets.txt and sonnets.dec
    '   FC: no differences encountered
    
    ' Get the time
    lTime = ap_EndTimer()
    Debug.Print Len(strIn), lTime & " ms"
    
    ' Make sure we did it correctly
    If strIn <> strOut Then
        MsgBox "Encrypt error"
    End If
    
    ' Now do the same test but using the direct file encryption fns
    Call ap_StartTimer
    aKey() = StrConv("12345", vbFromUnicode)
    Call blf_KeyInit(aKey())
    ' Encrypt it
    Call blf_FileEnc(sFilePath, sFileOut)
    ' Decrypt it
    Call blf_FileDec(sFileOut, sFileDec)
    lTime = ap_EndTimer()
    Debug.Print "Using blf_FileEnc: ", lTime & " ms"
    
    
End Function

Public Function TestFileEncCBC() As Boolean
' Encrypts a file using CBC mode
' Input file is the 32 bytes:-
' 000000  37 36 35 34 33 32 31 20 4e 6f 77 20 69 73 20 74  7654321 Now is t
' 000010  68 65 20 74 69 6d 65 20 66 6f 72 20 00 00 00 00  he time for ....

' This should produce a file containing the 40 bytes
' 000000  6b 77 b4 d6 30 06 de e6 05 b1 56 e2 74 03 97 93  kw��0.��.�V�t...
' 000010  58 de b9 e7 15 46 16 d9 59 f1 65 2b d5 ff 92 cc  X޹�.F.�Y�e+����
' 000020  ec 04 44 13 2b c4 6e 49                          �.D.+�nI

' (there are 8 extra bytes because of padding)

    Dim sFileIn  As String
    Dim sFileOut As String
    Dim sCipher As String
    Dim sPlain As String
    Dim aKey() As Byte

    sFileIn = "C:\test\Nowis.txt"
    sFileOut = "C:\test\Nowis.enc"
    
    ' Load test key and initialise
    aKey() = cv_BytesFromHex("0123456789ABCDEFF0E1D2C3B4A59687")
    Call blf_KeyInit(aKey)
    
    ' Encipher the file in CBC mode
    Call blf_FileEncCBC(sFileIn, sFileOut, "FEDCBA9876543210")
    
    MsgBox "done"
End Function

Public Function TestFileDecCBC() As Boolean
' This should produce a file identical to what we started with
    Dim sFileIn As String
    Dim sFileOut As String
    Dim aKey() As Byte

    sFileIn = "C:\test\Nowis.enc"
    sFileOut = "C:\test\NowisDec.txt"
    
    ' Load test key and initialise
    aKey() = cv_BytesFromHex("0123456789ABCDEFF0E1D2C3B4A59687")
    Call blf_KeyInit(aKey)
    
    ' Decipher the file in CBC mode
    Call blf_FileDecCBC(sFileIn, sFileOut, "FEDCBA9876543210")
    MsgBox "done"
End Function

' ---- File read and write functions

Public Function ReadFileIntoString(sFilePath As String) As String
' Reads file (if it exists) into a string.
    Dim strIn As String
    Dim hFile As Integer
    
    ' Check if file exists
    If Len(Dir(sFilePath)) = 0 Then
        Exit Function
    End If
    hFile = FreeFile
    Open sFilePath For Binary Access Read As #hFile
    strIn = Input(LOF(hFile), #hFile)
    Close #hFile
    ReadFileIntoString = strIn
    
End Function

Public Function WriteFileFromString(sFilePath As String, strIn As String) As Boolean
' Creates a file from a string. Clobbers any existing file.
On Error GoTo OnError
    Dim hFile As Integer
    
    If Len(Dir(sFilePath)) > 0 Then
        Kill sFilePath
    End If
    hFile = FreeFile
    Open sFilePath For Binary Access Write As #hFile
    Put #hFile, , strIn
    Close #hFile
    WriteFileFromString = True
Done:
    Exit Function
OnError:
    Resume Done
    
End Function


