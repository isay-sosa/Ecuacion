Public Class Form1
    Dim a As Arbol

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        a = New Arbol(Ecuacion.Text)
        Label3.Text = a.Ordenamientos()
        Label2.Text = "Método PreOrder" & vbCrLf & "R = " & a.solveEq(1)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        a = New Arbol(Ecuacion.Text)
        Label3.Text = a.Ordenamientos()
        Label2.Text = "Método PostOrder" & vbCrLf & "R = " & a.solveEq(2)
    End Sub
End Class
