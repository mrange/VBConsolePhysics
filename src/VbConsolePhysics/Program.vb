Imports Spectre.Console

Module Program
    Dim timer As Stopwatch = Stopwatch.StartNew()
    Dim mySystem As MySystem = New MySystem()

    ' Draws a circle traversing around in a circle
    Function DrawACircle(c As Canvas) As Boolean
        Dim now = timer.ElapsedMilliseconds / 1000.0
        Dim cx = c.Width / 2
        Dim cy = c.Height / 2

        Dim a = now
        Dim x = cx + Math.Cos(a) * 20
        Dim y = cy + Math.Sin(a) * 20

        c.Clear(Color.Black)
        c.DrawCircle(x, y, 3, Color.Yellow)

        Return True
    End Function

    ' Draws MySystem
    Function DrawMySystem(c As Canvas) As Boolean
        Dim now = timer.ElapsedMilliseconds / 1000.0
        Dim cx = c.Width / 2
        Dim cy = c.Height / 2

        c.Clear(Color.Black)

        mySystem.TimeStep()

        For Each s In mySystem.Constraints
            Dim x0 = s.Left.Current.X + cx
            Dim y0 = s.Left.Current.Y + cy

            Dim x1 = s.Right.Current.X + cx
            Dim y1 = s.Right.Current.Y + cy

            c.DrawLine(x0, y0, x1, y1, Color.Red)
        Next

        For Each p In mySystem.Particles
            Dim x = p.Current.X + cx
            Dim y = p.Current.Y + cy

            c.DrawCircle(x, y, 5, Color.Yellow)
        Next

        Return True
    End Function

    Sub Main(args As String())
        Dim w = AnsiConsole.Profile.Width
        Dim h = AnsiConsole.Profile.Height

        PaintSomePixels(50, w, h * 2, Function(c) DrawMySystem(c))
    End Sub
End Module
