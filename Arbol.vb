Public Class Arbol
    Private Ecuacion As String
    Private Raiz As Nodo

    Sub New(ByVal Ecuacion)        
        Me.Ecuacion = Ecuacion
        setRaiz()
    End Sub

    Private Sub setRaiz(Optional ByVal Ecuacion As String = "")
        'Se configura la raíz, pero primero se debe generar la lista de nodos y de ahí crear el árbol
        If Ecuacion = "" Then Ecuacion = Me.Ecuacion
        Raiz = setArbol(setLista(Ecuacion), 1)
    End Sub

    ''' <summary>
    ''' Regresa una lista de nodos a partir de una cadena
    ''' </summary>
    ''' <param name="Ecuacion">Ecuación de tipo string, la cual se convertirá en una lista de nodos</param>
    ''' <returns>Regresa una lista de nodos ya acomodados a partir de una ecuación</returns>
    ''' <remarks></remarks>
    Private Function setLista(ByVal Ecuacion As String) As Nodo
        Dim nodo As New Nodo

        If Not IsNumeric(Ecuacion(0)) Then
            nodo.Valor = Ecuacion(0)
            Ecuacion = Mid(Ecuacion, 2, Ecuacion.Length - 1)
        Else
            While IsNumeric(Ecuacion(0))
                nodo.Valor &= Ecuacion(0)
                Ecuacion = Mid(Ecuacion, 2, Ecuacion.Length - 1)
                If Ecuacion = "" Then Exit While
            End While
        End If

        If Ecuacion <> "" Then nodo.Siguiente = setLista(Ecuacion)

        If nodo.Siguiente IsNot Nothing Then
            nodo.Siguiente.Anterior = nodo
        End If

        Return nodo
    End Function

    ''' <summary>
    ''' Proceso que se repite dentro de la función "setArbol" el cual crea los hijos del operador y elimina y modifica las referencias de los nodos
    ''' </summary>
    ''' <param name="raiz">Es el nodo en el que tratará de generar los hijos, dependiendo del nivel en el que este</param>
    ''' <param name="nivel">El nivel 1 busca parentesis, el nivel 2 busca multiplicaciones y divisiones, y el nivel 3 busca sumas y restas</param>
    ''' <remarks></remarks>
    Private Sub makeArbol(ByVal raiz As Nodo, ByVal nivel As Byte)
        Dim temp As Nodo = raiz
        Dim Entrar As Boolean = False

        If nivel = 2 Then
            If (temp.Valor = "*" Or temp.Valor = "/") And temp.Izquierdo Is Nothing And temp.Derecho Is Nothing Then Entrar = True
        ElseIf nivel = 3 Then
            If (temp.Valor = "+" Or temp.Valor = "-") And temp.Izquierdo Is Nothing And temp.Derecho Is Nothing Then Entrar = True
        End If

        If Entrar Then
            temp.Anterior.Siguiente = Nothing
            temp.Siguiente.Anterior = Nothing

            temp.Izquierdo = temp.Anterior
            temp.Anterior = temp.Anterior.Anterior
            If temp.Siguiente.Valor = "(" Then
                temp.Derecho = setArbol(temp.Siguiente, 1, True)
                temp.Siguiente = temp.Derecho.Siguiente
            Else
                temp.Derecho = temp.Siguiente
                temp.Siguiente = temp.Siguiente.Siguiente
            End If

            If temp.Izquierdo.Anterior IsNot Nothing Then temp.Izquierdo.Anterior = Nothing
            If temp.Izquierdo.Siguiente IsNot Nothing Then temp.Izquierdo.Siguiente = Nothing
            If temp.Derecho.Anterior IsNot Nothing Then temp.Derecho.Anterior = Nothing
            If temp.Derecho.Siguiente IsNot Nothing Then temp.Derecho.Siguiente = Nothing

            If temp.Siguiente IsNot Nothing Then
                temp.Siguiente.Anterior = temp
            End If
            If temp.Anterior IsNot Nothing Then
                temp.Anterior.Siguiente = temp
            End If
        End If
    End Sub

    ''' <summary>
    ''' Función que genera un árbol de una ecuación a partir de una lista de nodos
    ''' </summary>
    ''' <param name="raiz">Lista de nodos, con la cual se generará el árbol</param>
    ''' <param name="nivel">El nivel 1 busca parentesis, el nivel 2 busca multiplicaciones y divisiones, y el nivel 3 busca sumas y restas</param>
    ''' <param name="entrar">Si existen parentesis anidados dentro de la lista, la variable cambia a "True" para indicar que entro a parentesis anidados</param>
    ''' <returns>Regresa un nodo, el cual tendrá la configuración de todo el árbol (regresa la raíz del árbol)</returns>
    ''' <remarks></remarks>
    Private Function setArbol(ByVal raiz As Nodo, ByVal nivel As Byte, Optional ByVal entrar As Boolean = False) As Nodo
        Dim temp As Nodo = raiz

        If nivel = 1 Then
            While temp IsNot Nothing
                Dim nivel2 As Boolean = True
                Try
                    If temp.Anterior.Valor = "(" Then
                        While temp IsNot Nothing
                            If temp.Valor = "(" Then
                                temp = setArbol(temp.Siguiente, nivel, True)
                            End If

                            If nivel2 Then
                                makeArbol(temp, 2)
                            Else
                                makeArbol(temp, 3)
                            End If

                            If temp.Siguiente.Valor = ")" And nivel2 Then
                                While temp.Anterior.Valor <> "("
                                    temp = temp.Anterior
                                End While
                                nivel2 = False
                            End If
                            If temp.Siguiente.Valor = ")" And Not nivel2 Then
                                Exit While
                            End If
                            temp = temp.Siguiente
                        End While
                        temp.Anterior.Siguiente = Nothing
                        temp.Siguiente.Anterior = Nothing
                        temp.Anterior = temp.Anterior.Anterior
                        temp.Siguiente = temp.Siguiente.Siguiente
                        If temp.Siguiente IsNot Nothing Then
                            temp.Siguiente.Anterior = temp
                        End If
                        If temp.Anterior IsNot Nothing Then
                            temp.Anterior.Siguiente = temp
                        End If

                        If entrar Then Return temp
                    End If
                Catch ex As Exception
                End Try

                If temp.Siguiente Is Nothing Then Exit While
                temp = temp.Siguiente                
            End While
        Else
            While temp IsNot Nothing
                makeArbol(temp, nivel)
                If temp.Siguiente Is Nothing Then Exit While
                temp = temp.Siguiente
            End While
        End If

        If nivel < 3 Then
            While temp.Anterior IsNot Nothing
                temp = temp.Anterior
            End While
            temp = setArbol(temp, nivel + 1)
        End If

        Return temp
    End Function

    ''' <summary>
    ''' Función que resuelve la ecuación una vez que el árbol se ha configurado
    ''' </summary>
    ''' <param name="metodo">Con 1 se realizará el método en base al PreOrder, mientras que con el 2 resuelve el árbol mediante el método basado en PostOrder</param>
    ''' <returns>Regresa el resultado del árbol de ecuación en una cadena</returns>
    ''' <remarks></remarks>
    Public Function solveEq(ByVal metodo As Byte) As String
        Dim Eq As String = ""

        If metodo = 1 Then
            Eq = PreOrder(Me.Raiz)
            Dim valor As String = ""
            Dim pN, pO As New Stack
            Dim cN, cO As Byte
            For i As Integer = Eq.Length - 1 To 0 Step -1
                If Eq(i) <> "#" Then
                    If IsNumeric(Eq(i)) Then
                        valor = ""
                        While IsNumeric(Eq(i))
                            valor = Eq(i) & valor
                            i -= 1
                        End While
                        pN.Push(valor)
                        cN += 1
                    Else
                        valor = Eq(i)
                        pO.Push(valor)
                        cO += 1
                    End If
                End If

                If cN >= 2 And cO >= 1 Then
                    Dim izq, der As Double
                    izq = pN.Pop()
                    der = pN.Pop()
                    Select Case pO.Pop()
                        Case "+"
                            pN.Push(izq + der)
                        Case "-"
                            pN.Push(izq - der)
                        Case "*"
                            pN.Push(izq * der)
                        Case "/"
                            pN.Push(izq / der)
                    End Select
                    cN -= 1
                    cO -= 1
                End If
            Next
            Eq = pN.Peek()
        Else
            Eq = PostOrder(Me.Raiz)
            Dim valor As String = ""
            Dim pN, pO As New Stack
            Dim cN, cO As Byte

            For i As Byte = 0 To Eq.Length - 1
                If Eq(i) <> "#" Then
                    If IsNumeric(Eq(i)) Then
                        valor = ""
                        While IsNumeric(Eq(i))
                            valor = valor & Eq(i)
                            i += 1
                        End While
                        pN.Push(valor)
                        cN += 1
                    Else
                        valor = Eq(i)
                        pO.Push(valor)
                        cO += 1
                    End If
                End If

                If cN >= 2 And cO >= 1 Then
                    Dim izq, der As Double
                    der = pN.Pop()
                    izq = pN.Pop()
                    Select Case pO.Pop()
                        Case "+"
                            pN.Push(izq + der)
                        Case "-"
                            pN.Push(izq - der)
                        Case "*"
                            pN.Push(izq * der)
                        Case "/"
                            pN.Push(izq / der)
                    End Select
                    cN -= 1
                    cO -= 1
                End If
            Next
            Eq = pN.Peek()
        End If

        Return Eq
    End Function

    Private Function InOrder(ByVal n As Nodo) As String
        Dim cadena As String = ""

        If n.Izquierdo IsNot Nothing Then cadena &= InOrder(n.Izquierdo)

        cadena &= n.Valor & "#"

        If n.Derecho IsNot Nothing Then cadena &= InOrder(n.Derecho)

        Return cadena
    End Function

    Private Function PreOrder(ByVal n As Nodo) As String
        Dim cadena As String = n.Valor & "#"

        If n.Izquierdo IsNot Nothing Then cadena &= PreOrder(n.Izquierdo)
        If n.Derecho IsNot Nothing Then cadena &= PreOrder(n.Derecho)

        Return cadena
    End Function

    Private Function PostOrder(ByVal n As Nodo) As String
        Dim cadena As String = ""

        If n.Izquierdo IsNot Nothing Then cadena &= PostOrder(n.Izquierdo)
        If n.Derecho IsNot Nothing Then cadena &= PostOrder(n.Derecho)

        cadena &= n.Valor & "#"
        Return cadena
    End Function

    Public Function Ordenamientos() As String
        Dim cadena = "InOrder: " & InOrder(Raiz) & vbCrLf & "PreOrder: " & PreOrder(Raiz) & vbCrLf & "PostOrder: " & PostOrder(Raiz)
        Return cadena
    End Function
End Class
