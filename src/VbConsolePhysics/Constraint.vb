' A constraint is either a stick or a rope constraint
' The stick constraint tries to make sure that the distance
' between the two particles are always the same
' The rope constraint allows the length to be shorter but not longer
Public Class Constraint
    Public ReadOnly IsStick As Boolean
    Public ReadOnly Length As Single

    Public ReadOnly Left As Particle
    Public ReadOnly Right As Particle

    ' Creates a stick constraint
    Public Sub New(left As Particle, right As Particle)
        IsStick = True
        Length = (left.Current - right.Current).Length()
        Me.Left = left
        Me.Right = right
    End Sub

    ' Creates a rope constraint, the extend ratio is used to 
    ' elongiate the rope
    Public Sub New(extendRatio As Single, left As Particle, right As Particle)
        IsStick = False
        Length = (left.Current - right.Current).Length() * (1.0F + Math.Max(extendRatio, 0.0F))
        Me.Left = left
        Me.Right = right
    End Sub

    ' After the verlet step the constraints are often "over-stretched"
    ' Relax makes sure that a constraint is then "relaxed" into a state
    ' where the two particles have a valid length
    ' This is a local relaxation but it turns out repeating this process
    ' over and over again tends towords a more "relaxed" system
    Public Sub Relax()
        ' Bunch of math but the idea is this
        ' 1. Compute the distance between the two particles
        ' 2. Check if length is valid
        ' 3. If length is too short or too long then push or pull the particles
        ' 4. Compare the masses of the two particles to ensure a smaller particle
        '    pulls a larger less and vice-versa
        Dim l = Left
        Dim r = Right
        Dim lc = l.Current
        Dim rc = r.Current

        Dim diff = lc - rc
        Dim len = diff.Length()
        Dim ldiff = len - Length
        Dim test = If(IsStick, Math.Abs(ldiff) > 0.0F, ldiff > 0.0F)
        If test Then
            Dim imass = 0.5F / (l.InvertedMass + r.InvertedMass)
            Dim mdiff = (imass * ldiff / len) * diff
            Dim loff = l.InvertedMass * mdiff
            Dim roff = r.InvertedMass * mdiff

            l.Current = lc - loff
            r.Current = rc + roff
        End If
    End Sub
End Class
