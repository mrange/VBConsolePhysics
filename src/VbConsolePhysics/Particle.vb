Imports System.Numerics

' A particle has a mass but also an inverted mass which is commonly
' used in many computations
' A particle also has a Current and Previous position
' Current - Previous position is the velocity of the Particle
Public Class Particle
    Public ReadOnly Mass As Single
    Public ReadOnly InvertedMass As Single

    Public Current As Vector2
    Public Previous As Vector2

    ' Creates particle with a mass, position and velocity
    Public Sub New(mass As Single, x As Single, y As Single, vx As Single, vy As Single)
        Dim c = New Vector2(x, y)
        Dim v = New Vector2(vx, vy)

        Me.Mass = mass
        InvertedMass = 1.0F / mass
        Current = c
        Previous = c - v
    End Sub

    ' Creates a fixed position that has infinite mass
    '   Used as anchor points
    Public Sub New(x As Single, y As Single)
        Dim c = New Vector2(x, y)

        Mass = Single.PositiveInfinity
        InvertedMass = 0.0F
        Current = c
        Previous = c
    End Sub

    ' Computes new Current position using the inertia and gravity
    Public Sub VerletStep(gravity As Single)
        If InvertedMass > 0 Then
            Dim c = Current
            Dim g = New Vector2(0, gravity)
            Current = g + c + (c - Previous)
            Previous = c
        End If
    End Sub
End Class
