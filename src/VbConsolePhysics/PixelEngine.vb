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

    ' Draws a horizontal line
    <Extension()>
    Public Sub DrawHorizontalLine(canvas As Canvas, x As Integer, y As Integer, length As Integer, color As Color)
        If y >= 0 And y < canvas.Height Then
            Dim f = Math.Max(x, 0)
            Dim t = Math.Min(x + length, canvas.Width) - 1
            For i = f To t
                canvas.SetPixel(i, y, color)
            Next
        End If
    End Sub

    ' Draws a vertical line
    <Extension()>
    Public Sub DrawVerticalLine(canvas As Canvas, x As Integer, y As Integer, length As Integer, color As Color)
        If x >= 0 And x < canvas.Width Then
            Dim f = Math.Max(y, 0)
            Dim t = Math.Min(y + length, canvas.Height) - 1
            For i = f To t
                canvas.SetPixel(x, i, color)
            Next
        End If
    End Sub

    ' Draws a line
    <Extension()>
    Public Sub DrawLine(canvas As Canvas, x0 As Integer, y0 As Integer, x1 As Integer, y1 As Integer, color As Color)
        Dim left = Math.Min(x0, x1)
        Dim top = Math.Min(y0, y1)
        Dim right = Math.Max(x0, x1)
        Dim bottom = Math.Max(y0, y1)
        Dim diffX As Double = right - left
        Dim diffY As Double = bottom - top
        If diffY = 0 Then
            canvas.DrawHorizontalLine(x0, y0, diffX, color)
        ElseIf diffX = 0 Then
            canvas.DrawVerticalLine(x0, y0, diffY, color)
        ElseIf diffY < diffX Then
            Dim stepX = diffX / (diffY + 1)
            Dim currentX As Double = left
            For i = top To bottom
                Dim nextX = currentX + stepX
                Dim x = Math.Round(currentX)
                Dim l = Math.Round(nextX) - x + 1
                canvas.DrawHorizontalLine(x, i, l, color)
                currentX = nextX
            Next
        Else
            Dim stepY = diffY / (diffX + 1)
            Dim currentY As Double = top
            For i = left To right
                Dim nextY = currentY + stepY
                Dim y = Math.Round(currentY)
                Dim l = Math.Round(nextY) - y + 1
                canvas.DrawVerticalLine(i, y, l, color)
                currentY = nextY
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
                canvas.DrawHorizontalLine(x - d, i, 2 * d + 1, color)
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
