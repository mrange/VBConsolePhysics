Imports System.Drawing
' Imports Spectre.Console

Module Program
    Dim timer As Stopwatch = Stopwatch.StartNew()
    Dim mySystem As MySystem = New MySystem()

    Dim constraintPen As Pen = New Pen(Brushes.Red, 3.0)
    Dim font = New Font("Arial", 24.0)

    ' Draws MySystem
    Function DrawMySystem2(width As Integer, height As Integer, g As Graphics) As Boolean
        Dim now = timer.ElapsedMilliseconds / 1000.0
        Dim cx = width / 2
        Dim cy = height / 2

        g.Clear(Color.Black)

        mySystem.TimeStep()

        g.DrawString("Powered by", font, Brushes.White, New PointF(0.0, 0.0))
        g.DrawString("Spectre.Console", font, Brushes.Purple, New PointF(0.0, height / 2.0))

        For Each s In mySystem.Constraints
            Dim x0 As Integer = s.Left.Current.X + cx
            Dim y0 As Integer = s.Left.Current.Y + cy

            Dim x1 As Integer = s.Right.Current.X + cx
            Dim y1 As Integer = s.Right.Current.Y + cy

            g.DrawLine(constraintPen, x0, y0, x1, y1)
        Next

        For Each p In mySystem.Particles
            Dim radius As Integer = 10
            Dim x As Integer = p.Current.X + cx - radius
            Dim y As Integer = p.Current.Y + cy - radius

            g.FillEllipse(Brushes.Yellow, x, y, 2 * radius, 2 * radius)
        Next

        Return True
    End Function

    Sub Main(args As String())
        Dim w = Spectre.Console.AnsiConsole.Profile.Width
        Dim h = Spectre.Console.AnsiConsole.Profile.Height

        PaintSomePixels(25, w, h * 2, Function(g, bw, bh) DrawMySystem2(g, bw, bh))
    End Sub
End Module
