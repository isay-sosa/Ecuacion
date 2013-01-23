Public Class Nodo
    Private vValor
    Private vAnterior, vSiguiente, vDerecho, vIzquierdo As Nodo

    Sub New()

    End Sub

    Sub New(ByVal valor)
        vValor = valor
    End Sub

    Public Property Valor()
        Get
            Return vValor
        End Get
        Set(ByVal value)
            vValor = value
        End Set
    End Property

    Public Property Anterior()
        Get
            Return vAnterior
        End Get
        Set(ByVal value)
            vAnterior = value
        End Set
    End Property

    Public Property Siguiente()
        Get
            Return vSiguiente
        End Get
        Set(ByVal value)
            vSiguiente = value
        End Set
    End Property

    Public Property Izquierdo()
        Get
            Return vIzquierdo
        End Get
        Set(ByVal value)
            vIzquierdo = value
        End Set
    End Property

    Public Property Derecho()
        Get
            Return vDerecho
        End Get
        Set(ByVal value)
            vDerecho = value
        End Set
    End Property
End Class
