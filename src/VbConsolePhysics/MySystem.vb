Public Class MySystem
    Public ReadOnly Particles() As Particle
    Public ReadOnly Constraints() As Constraint

    Public Sub New()
        Particles =
        {
            New Particle(0, -40), 'Fix point
            New Particle(10, 40, -40, 0, 0), ' Particle right of fix point
            New Particle(20, 80, -40, 0, 0)  ' Particle right of particle above
        }

        Constraints =
        {
            New Constraint(Particles(0), Particles(1)), ' Connect fix point and first particle
            New Constraint(Particles(1), Particles(2))  ' Connect first particle and second particle
        }
    End Sub

    Public Sub TimeStep()
        ' By repeating a few times with smaller steps
        ' we get better precision. Otherwise the "energy" leaks
        ' out of the system too fast
        For i = 1 To 5
            For Each p In Particles
                ' Change this factor to speed up or slow down
                p.VerletStep(0.0125)
            Next

            ' Relax entire system 5 times
            ' More iterations makes system more stiff
            ' Less makes system more "bouncy"
            For j = 1 To 5
                For Each c In Constraints
                    c.Relax()
                Next
            Next
        Next
    End Sub
End Class
