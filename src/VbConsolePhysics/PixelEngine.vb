Imports Spectre.Console
Imports System.Runtime.CompilerServices
Imports System.Threading

' Allows drawing real-time pixel graphics
Module PixelEngine
    ' Clears screen
    <Extension()>
    Public Sub Clear(canvas As Canvas, color As Color)
        Dim maxy = canvas.Height - 1
        Dim maxx = canvas.Width - 1
        For y = 0 To maxy
            For x = 0 To maxx
                canvas.SetPixel(x, y, color)
            Next
        Next
    End Sub

    ' Draws a flat line
    <Extension()>
    Public Sub DrawFlatLine(canvas As Canvas, x As Integer, y As Integer, length As Integer, color As Color)
        If y >= 0 And y < canvas.Height Then
            Dim f = Math.Max(x, 0)
            Dim t = Math.Min(x + length, canvas.Width) - 1
            For i = f To t
                canvas.SetPixel(i, y, color)
            Next
        End If
    End Sub

    ' Draws a cirle
    <Extension()>
    Public Sub DrawCircle(canvas As Canvas, x As Integer, y As Integer, radius As Integer, color As Color)
        Dim r2 = radius * radius
        For i = y - radius To y + radius
            Dim d2 = r2 - (i - y) * (i - y)
            If d2 >= 0 Then
                Dim d = Math.Round(Math.Sqrt(d2))
                canvas.DrawFlatLine(x - d, i, 2 * d + 1, color)
            End If
        Next
    End Sub

    Sub PaintSomePixels(sleepFor As Integer, width As Integer, height As Integer, drawer As Func(Of Canvas, Boolean))
        Dim c = New Canvas(width, height)

        ' Uses live updates for flicker free experience
        AnsiConsole.Live(c).Start(
            Sub(ctx)
                While drawer(c)
                    ctx.Refresh()
                    Thread.Sleep(sleepFor)
                End While
            End Sub)
    End Sub
End Module
