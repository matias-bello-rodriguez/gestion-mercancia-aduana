Imports System.Windows.Forms
Imports System.Drawing
Imports System.Linq

Public Class FormAgregar
    Inherits Form

    Private lblDocumento As Label
    Private txtDocumento As TextBox
    Private lblFecha As Label
    Private dtpFecha As DateTimePicker
    Private lblFiscalizador As Label
    Private txtFiscalizador As TextBox
    Private lblMercancia As Label
    Private txtMercancia As TextBox
    Private lblLocalidad As Label
    Private txtLocalidad As TextBox
    Private lblBGOrg As Label
    Private txtBGOrg As TextBox
    Private btnGuardar As Button
    Private btnCancelar As Button
    Private btnLimpiar As Button

    Private mercanciaActual As Mercancia
    Private esEdicion As Boolean = False

    ''' <summary>
    ''' Constructor para modo agregar
    ''' </summary>
    Public Sub New()
        InitializeComponent()
        esEdicion = False
        Me.Text = "Agregar Nueva Mercancía"
    End Sub

    ''' <summary>
    ''' Constructor para modo editar
    ''' </summary>
    ''' <param name="mercancia">Objeto mercancía con los datos a editar</param>
    Public Sub New(mercancia As Mercancia)
        InitializeComponent()
        mercanciaActual = mercancia
        esEdicion = True
        Me.Text = "Editar Mercancía"
        CargarDatos()
    End Sub

    Private Sub InitializeComponent()
        ' Crear controles
        Me.lblDocumento = New Label()
        Me.txtDocumento = New TextBox()
        Me.lblFecha = New Label()
        Me.dtpFecha = New DateTimePicker()
        Me.lblFiscalizador = New Label()
        Me.txtFiscalizador = New TextBox()
        Me.lblMercancia = New Label()
        Me.txtMercancia = New TextBox()
        Me.lblLocalidad = New Label()
        Me.txtLocalidad = New TextBox()
        Me.lblBGOrg = New Label()
        Me.txtBGOrg = New TextBox()
        Me.btnGuardar = New Button()
        Me.btnCancelar = New Button()
        Me.btnLimpiar = New Button()

        Me.SuspendLayout()

        ' Configurar formulario
        Me.Size = New Size(500, 450)
        Me.StartPosition = FormStartPosition.CenterParent
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.BackColor = Color.FromArgb(245, 245, 250)

        ' lblDocumento
        Me.lblDocumento.Text = "Documento:"
        Me.lblDocumento.Location = New Point(30, 30)
        Me.lblDocumento.Size = New Size(100, 20)
        Me.lblDocumento.Font = New Font("Segoe UI", 10.0!, FontStyle.Bold)

        ' txtDocumento
        Me.txtDocumento.Location = New Point(140, 28)
        Me.txtDocumento.Size = New Size(300, 25)

        ' lblFecha
        Me.lblFecha.Text = "Fecha:"
        Me.lblFecha.Location = New Point(30, 70)
        Me.lblFecha.Size = New Size(100, 20)
        Me.lblFecha.Font = New Font("Segoe UI", 10.0!, FontStyle.Bold)

        ' dtpFecha
        Me.dtpFecha.Location = New Point(140, 68)
        Me.dtpFecha.Size = New Size(300, 25)
        Me.dtpFecha.Format = DateTimePickerFormat.Short

        ' lblFiscalizador
        Me.lblFiscalizador.Text = "Fiscalizador:"
        Me.lblFiscalizador.Location = New Point(30, 110)
        Me.lblFiscalizador.Size = New Size(100, 20)
        Me.lblFiscalizador.Font = New Font("Segoe UI", 10.0!, FontStyle.Bold)

        ' txtFiscalizador
        Me.txtFiscalizador.Location = New Point(140, 108)
        Me.txtFiscalizador.Size = New Size(300, 25)

        ' lblMercancia
        Me.lblMercancia.Text = "Mercancía:"
        Me.lblMercancia.Location = New Point(30, 150)
        Me.lblMercancia.Size = New Size(100, 20)
        Me.lblMercancia.Font = New Font("Segoe UI", 10.0!, FontStyle.Bold)

        ' txtMercancia
        Me.txtMercancia.Location = New Point(140, 148)
        Me.txtMercancia.Size = New Size(300, 60)
        Me.txtMercancia.Multiline = True

        ' lblLocalidad
        Me.lblLocalidad.Text = "Localidad:"
        Me.lblLocalidad.Location = New Point(30, 230)
        Me.lblLocalidad.Size = New Size(100, 20)
        Me.lblLocalidad.Font = New Font("Segoe UI", 10.0!, FontStyle.Bold)

        ' txtLocalidad
        Me.txtLocalidad.Location = New Point(140, 228)
        Me.txtLocalidad.Size = New Size(300, 25)

        ' lblBGOrg
        Me.lblBGOrg.Text = "BG/Org:"
        Me.lblBGOrg.Location = New Point(30, 270)
        Me.lblBGOrg.Size = New Size(100, 20)
        Me.lblBGOrg.Font = New Font("Segoe UI", 10.0!, FontStyle.Bold)

        ' txtBGOrg
        Me.txtBGOrg.Location = New Point(140, 268)
        Me.txtBGOrg.Size = New Size(300, 25)

        ' btnGuardar
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.Location = New Point(140, 320)
        Me.btnGuardar.Size = New Size(90, 35)
        Me.btnGuardar.BackColor = Color.FromArgb(50, 205, 50)
        Me.btnGuardar.ForeColor = Color.White
        Me.btnGuardar.FlatStyle = FlatStyle.Flat

        ' btnLimpiar
        Me.btnLimpiar.Text = "Limpiar"
        Me.btnLimpiar.Location = New Point(240, 320)
        Me.btnLimpiar.Size = New Size(90, 35)
        Me.btnLimpiar.BackColor = Color.FromArgb(255, 165, 0)
        Me.btnLimpiar.ForeColor = Color.White
        Me.btnLimpiar.FlatStyle = FlatStyle.Flat

        ' btnCancelar
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.Location = New Point(340, 320)
        Me.btnCancelar.Size = New Size(90, 35)
        Me.btnCancelar.BackColor = Color.FromArgb(220, 20, 60)
        Me.btnCancelar.ForeColor = Color.White
        Me.btnCancelar.FlatStyle = FlatStyle.Flat

        ' Agregar controles al formulario
        Me.Controls.Add(Me.lblDocumento)
        Me.Controls.Add(Me.txtDocumento)
        Me.Controls.Add(Me.lblFecha)
        Me.Controls.Add(Me.dtpFecha)
        Me.Controls.Add(Me.lblFiscalizador)
        Me.Controls.Add(Me.txtFiscalizador)
        Me.Controls.Add(Me.lblMercancia)
        Me.Controls.Add(Me.txtMercancia)
        Me.Controls.Add(Me.lblLocalidad)
        Me.Controls.Add(Me.txtLocalidad)
        Me.Controls.Add(Me.lblBGOrg)
        Me.Controls.Add(Me.txtBGOrg)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.btnLimpiar)
        Me.Controls.Add(Me.btnCancelar)

        ' Configurar eventos
        AddHandler Me.btnGuardar.Click, AddressOf BtnGuardar_Click
        AddHandler Me.btnLimpiar.Click, AddressOf BtnLimpiar_Click
        AddHandler Me.btnCancelar.Click, AddressOf BtnCancelar_Click

        Me.ResumeLayout(False)
    End Sub

    Private Sub CargarDatos()
        If mercanciaActual IsNot Nothing Then
            txtDocumento.Text = mercanciaActual.Documento
            dtpFecha.Value = DateTime.Parse(mercanciaActual.Fecha)
            txtFiscalizador.Text = mercanciaActual.Fiscalizador
            txtMercancia.Text = mercanciaActual.Mercancia
            txtLocalidad.Text = mercanciaActual.Localidad
            txtBGOrg.Text = mercanciaActual.BGOrg
        End If
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs)
        If ValidarCampos() Then
            Dim mercancia As New Mercancia()
            
            If esEdicion Then
                mercancia.Id = mercanciaActual.Id
            End If
            
            mercancia.Documento = txtDocumento.Text.Trim()
            mercancia.Fecha = dtpFecha.Value.ToShortDateString()
            mercancia.Fiscalizador = txtFiscalizador.Text.Trim()
            mercancia.Mercancia = txtMercancia.Text.Trim()
            mercancia.Localidad = txtLocalidad.Text.Trim()
            mercancia.BGOrg = txtBGOrg.Text.Trim()

            If DatabaseManager.AgregarMercancia(mercancia) Then
                MessageBox.Show("Mercancía guardada exitosamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                
                ' Si es una nueva mercancía, generar automáticamente la etiqueta PDF
                If Not esEdicion Then
                    ' Obtener el ID generado para la nueva mercancía
                    Dim mercanciaConId = DatabaseManager.ObtenerTodasLasMercancias().FirstOrDefault(Function(m) m.Documento = mercancia.Documento And m.Fecha = mercancia.Fecha And m.Fiscalizador = mercancia.Fiscalizador)
                    
                    If mercanciaConId IsNot Nothing Then
                        ' Generar etiqueta PDF para impresión
                        Dim rutaPDF As String = PDFGenerator.GenerarEtiquetaParaImpresion(mercanciaConId)
                        
                        ' Actualizar la ruta PDF en la base de datos
                        If Not String.IsNullOrEmpty(rutaPDF) Then
                            DatabaseManager.ActualizarRutaPDF(mercanciaConId.Id, rutaPDF)
                        End If
                    End If
                End If
                
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End If
        End If
    End Sub

    Private Sub BtnLimpiar_Click(sender As Object, e As EventArgs)
        txtDocumento.Text = ""
        dtpFecha.Value = DateTime.Now
        txtFiscalizador.Text = ""
        txtMercancia.Text = ""
        txtLocalidad.Text = ""
        txtBGOrg.Text = ""
        txtDocumento.Focus()
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs)
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Function ValidarCampos() As Boolean
        If String.IsNullOrWhiteSpace(txtDocumento.Text) Then
            MessageBox.Show("El documento es obligatorio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDocumento.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtFiscalizador.Text) Then
            MessageBox.Show("El fiscalizador es obligatorio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtFiscalizador.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtMercancia.Text) Then
            MessageBox.Show("La mercancía es obligatoria", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtMercancia.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtLocalidad.Text) Then
            MessageBox.Show("La localidad es obligatoria", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtLocalidad.Focus()
            Return False
        End If

        If String.IsNullOrWhiteSpace(txtBGOrg.Text) Then
            MessageBox.Show("BG/Org es obligatorio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtBGOrg.Focus()
            Return False
        End If

        Return True
    End Function
End Class