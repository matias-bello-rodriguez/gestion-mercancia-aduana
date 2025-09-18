Public Class Mercancia
    ''' <summary>
    ''' ID único de la mercancía (clave primaria)
    ''' </summary>
    Public Property Id As Integer

    ''' <summary>
    ''' Número de documento asociado a la mercancía
    ''' </summary>
    Public Property Documento As String

    ''' <summary>
    ''' Fecha de registro de la mercancía
    ''' </summary>
    Public Property Fecha As String

    ''' <summary>
    ''' Nombre del fiscalizador responsable
    ''' </summary>
    Public Property Fiscalizador As String

    ''' <summary>
    ''' Descripción de la mercancía incautada
    ''' </summary>
    Public Property Mercancia As String

    ''' <summary>
    ''' Localidad donde se incautó la mercancía
    ''' </summary>
    Public Property Localidad As String

    ''' <summary>
    ''' BGOrg (Organización de destino o código)
    ''' </summary>
    Public Property BGOrg As String

    ''' <summary>
    ''' Ruta del archivo PDF de etiqueta generado para esta mercancía
    ''' </summary>
    Public Property RutaPDF As String

    ''' <summary>
    ''' Constructor vacío
    ''' </summary>
    Public Sub New()
        Id = 0
        Documento = ""
        Fecha = ""
        Fiscalizador = ""
        Mercancia = ""
        Localidad = ""
        BGOrg = ""
        RutaPDF = ""
    End Sub

    ''' <summary>
    ''' Constructor con parámetros
    ''' </summary>
    Public Sub New(documento As String, fecha As String, fiscalizador As String, 
                   mercancia As String, localidad As String, bgOrg As String)
        Me.Id = 0
        Me.Documento = documento
        Me.Fecha = fecha
        Me.Fiscalizador = fiscalizador
        Me.Mercancia = mercancia
        Me.Localidad = localidad
        Me.BGOrg = bgOrg
        Me.RutaPDF = ""
    End Sub

    ''' <summary>
    ''' Valida que todos los campos obligatorios estén completos
    ''' </summary>
    ''' <returns>True si todos los campos están completos</returns>
    Public Function IsValid() As Boolean
        Return Not String.IsNullOrWhiteSpace(Documento) AndAlso
               Not String.IsNullOrWhiteSpace(Fecha) AndAlso
               Not String.IsNullOrWhiteSpace(Fiscalizador) AndAlso
               Not String.IsNullOrWhiteSpace(Mercancia) AndAlso
               Not String.IsNullOrWhiteSpace(Localidad) AndAlso
               Not String.IsNullOrWhiteSpace(BGOrg)
    End Function

    ''' <summary>
    ''' Genera el texto para el código QR
    ''' </summary>
    ''' <returns>Cadena con formato para QR</returns>
    Public Function GetQRText() As String
        Return $"{Documento}|{Fecha}|{Fiscalizador}"
    End Function

    ''' <summary>
    ''' Representación en cadena del objeto
    ''' </summary>
    ''' <returns>Descripción de la mercancía</returns>
    Public Overrides Function ToString() As String
        Return $"ID: {Id} - {Mercancia} ({Documento})"
    End Function
End Class