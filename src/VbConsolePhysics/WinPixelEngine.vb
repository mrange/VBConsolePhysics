Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.Threading

Module WinPixelEngine
    Sub PaintSomePixels(sleepFor As Integer, width As Integer, height As Integer, painter As Func(Of Integer, Integer, Graphics, Boolean))
        Dim c = New Spectre.Console.Canvas(width, height)
        Dim bz = 2
        Dim sbz = 1.0 / (bz * bz)
        Dim bwidth = width * bz
        Dim bheight = height * bz

        Using bi = New Bitmap(bwidth, bheight, PixelFormat.Format24bppRgb)
            ' Uses live updates for flicker free experience
            Spectre.Console.AnsiConsole.Live(c).Start(
                    Sub(ctx)

                        Dim cont = True
                        Do
                            Using g = Graphics.FromImage(bi)
                                cont = painter(bwidth, bheight, g)
                            End Using

                            If cont Then
                                Dim bd = bi.LockBits(New Rectangle(0, 0, bwidth, bheight), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb)
                                Try
                                    Dim sz = Math.Abs(bd.Stride) * bd.Height
                                    Dim rgb(sz) As Byte
                                    Marshal.Copy(bd.Scan0, rgb, 0, sz)

                                    For y = 0 To height - 1
                                        Dim yoff = y * bd.Stride * bz
                                        For x = 0 To width - 1
                                            Dim r = 0.0
                                            Dim g = 0.0
                                            Dim b = 0.0
                                            Dim xoff = x * bz
                                            For yy = 0 To bz - 1
                                                Dim yyoff = yy * bd.Stride
                                                For xx = 0 To bz - 1
                                                    Dim off = yoff + 3 * (xoff + xx)
                                                    b += rgb(off + 0)
                                                    g += rgb(off + 1)
                                                    r += rgb(off + 2)
                                                Next
                                            Next
                                            Dim color = New Spectre.Console.Color(Math.Round(r * sbz), Math.Round(g * sbz), Math.Round(b * sbz))
                                            c.SetPixel(x, y, color)
                                        Next
                                    Next
                                Finally
                                    bi.UnlockBits(bd)
                                End Try
                                ctx.Refresh()

                                Thread.Sleep(sleepFor)
                            End If

                        Loop While cont
                    End Sub)
        End Using
    End Sub
End Module
