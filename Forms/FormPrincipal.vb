Imports System.Windows.Forms
Imports System.Drawing

Public Class FormPrincipal
    Inherits Form

    Private dgvMercancias As DataGridView
    Private btnAgregar As Button
    Private btnEditar As Button
    Private btnEliminar As Button
    Private btnGenerarEtiqueta As Button
    Private btnRefrescar As Button
    Private lblTitulo As Label
    Private lblContador As Label
    Private panelBotones As Panel

    Public Sub New()
        InitializeComponent()
        DatabaseManager.InicializarBaseDatos()
        CargarMercancias()
    End Sub

    Private Sub InitializeComponent()
        ' Crear controles
        Me.dgvMercancias = New DataGridView()
        Me.btnAgregar = New Button()
        Me.btnEditar = New Button()
        Me.btnEliminar = New Button()
        Me.btnGenerarEtiqueta = New Button()
        Me.btnRefrescar = New Button()
        Me.lblTitulo = New Label()
        Me.lblContador = New Label()
        Me.panelBotones = New Panel()

        Me.SuspendLayout()

        ' Configurar formulario
        Me.Text = "Sistema de Gestión de Mercancías - Aduana"
        Me.Size = New Size(1000, 600)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(240, 248, 255)

        ' Configurar lblTitulo
        Me.lblTitulo.Text = "GESTIÓN DE MERCANCÍAS INCAUTADAS"
        Me.lblTitulo.Font = New Font("Segoe UI", 16.0!, FontStyle.Bold)
        Me.lblTitulo.ForeColor = Color.FromArgb(25, 25, 112)
        Me.lblTitulo.Location = New Point(20, 20)
        Me.lblTitulo.Size = New Size(600, 30)

        ' Configurar dgvMercancias
        Me.dgvMercancias.Location = New Point(20, 80)
        Me.dgvMercancias.Size = New Size(750, 400)
        Me.dgvMercancias.AllowUserToAddRows = False
        Me.dgvMercancias.AllowUserToDeleteRows = False
        Me.dgvMercancias.ReadOnly = True
        Me.dgvMercancias.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Me.dgvMercancias.MultiSelect = False
        Me.dgvMercancias.BackgroundColor = Color.White
        Me.dgvMercancias.BorderStyle = BorderStyle.Fixed3D

        ' Configurar panelBotones
        Me.panelBotones.Location = New Point(790, 80)
        Me.panelBotones.Size = New Size(180, 400)
        Me.panelBotones.BackColor = Color.FromArgb(230, 230, 250)
        Me.panelBotones.BorderStyle = BorderStyle.FixedSingle

        ' Configurar btnAgregar
        Me.btnAgregar.Text = "Agregar"
        Me.btnAgregar.Location = New Point(10, 10)
        Me.btnAgregar.Size = New Size(160, 40)
        Me.btnAgregar.BackColor = Color.FromArgb(50, 205, 50)
        Me.btnAgregar.ForeColor = Color.White
        Me.btnAgregar.FlatStyle = FlatStyle.Flat
        Me.btnAgregar.Font = New Font("Segoe UI", 10.0!, FontStyle.Bold)

        ' Configurar btnEditar
        Me.btnEditar.Text = "Editar"
        Me.btnEditar.Location = New Point(10, 60)
        Me.btnEditar.Size = New Size(160, 40)
        Me.btnEditar.BackColor = Color.FromArgb(255, 165, 0)
        Me.btnEditar.ForeColor = Color.White
        Me.btnEditar.FlatStyle = FlatStyle.Flat
        Me.btnEditar.Font = New Font("Segoe UI", 10.0!, FontStyle.Bold)
        Me.btnEditar.Enabled = False

        ' Configurar btnEliminar
        Me.btnEliminar.Text = "Eliminar"
        Me.btnEliminar.Location = New Point(10, 110)
        Me.btnEliminar.Size = New Size(160, 40)
        Me.btnEliminar.BackColor = Color.FromArgb(220, 20, 60)
        Me.btnEliminar.ForeColor = Color.White
        Me.btnEliminar.FlatStyle = FlatStyle.Flat
        Me.btnEliminar.Font = New Font("Segoe UI", 10.0!, FontStyle.Bold)
        Me.btnEliminar.Enabled = False

        ' Configurar btnGenerarEtiqueta
        Me.btnGenerarEtiqueta.Text = "Generar Etiqueta"
        Me.btnGenerarEtiqueta.Location = New Point(10, 160)
        Me.btnGenerarEtiqueta.Size = New Size(160, 40)
        Me.btnGenerarEtiqueta.BackColor = Color.FromArgb(70, 130, 180)
        Me.btnGenerarEtiqueta.ForeColor = Color.White
        Me.btnGenerarEtiqueta.FlatStyle = FlatStyle.Flat
        Me.btnGenerarEtiqueta.Font = New Font("Segoe UI", 10.0!, FontStyle.Bold)
        Me.btnGenerarEtiqueta.Enabled = False

        ' Configurar btnRefrescar
        Me.btnRefrescar.Text = "Refrescar"
        Me.btnRefrescar.Location = New Point(10, 210)
        Me.btnRefrescar.Size = New Size(160, 40)
        Me.btnRefrescar.BackColor = Color.FromArgb(105, 105, 105)
        Me.btnRefrescar.ForeColor = Color.White
        Me.btnRefrescar.FlatStyle = FlatStyle.Flat
        Me.btnRefrescar.Font = New Font("Segoe UI", 10.0!, FontStyle.Bold)

        ' Configurar lblContador
        Me.lblContador.Text = "Total de mercancías: 0"
        Me.lblContador.Font = New Font("Segoe UI", 10.0!)
        Me.lblContador.ForeColor = Color.FromArgb(25, 25, 112)
        Me.lblContador.Location = New Point(20, 500)
        Me.lblContador.Size = New Size(300, 20)

        ' Agregar controles al panel
        Me.panelBotones.Controls.Add(Me.btnAgregar)
        Me.panelBotones.Controls.Add(Me.btnEditar)
        Me.panelBotones.Controls.Add(Me.btnEliminar)
        Me.panelBotones.Controls.Add(Me.btnGenerarEtiqueta)
        Me.panelBotones.Controls.Add(Me.btnRefrescar)

        ' Agregar controles al formulario
        Me.Controls.Add(Me.lblTitulo)
        Me.Controls.Add(Me.dgvMercancias)
        Me.Controls.Add(Me.panelBotones)
        Me.Controls.Add(Me.lblContador)

        ' Configurar eventos
        AddHandler Me.btnAgregar.Click, AddressOf BtnAgregar_Click
        AddHandler Me.btnEditar.Click, AddressOf BtnEditar_Click
        AddHandler Me.btnEliminar.Click, AddressOf BtnEliminar_Click
        AddHandler Me.btnGenerarEtiqueta.Click, AddressOf BtnGenerarEtiqueta_Click
        AddHandler Me.btnRefrescar.Click, AddressOf BtnRefrescar_Click
        AddHandler Me.dgvMercancias.SelectionChanged, AddressOf DgvMercancias_SelectionChanged
        AddHandler Me.dgvMercancias.CellClick, AddressOf DgvMercancias_CellClick

        Me.ResumeLayout(False)
    End Sub

    Private Sub CargarMercancias()
        Try
            Dim mercancias = DatabaseManager.ObtenerTodasLasMercancias()
            dgvMercancias.DataSource = mercancias
            
            If dgvMercancias.Columns.Count > 0 Then
                dgvMercancias.Columns("Id").HeaderText = "ID"
                dgvMercancias.Columns("Id").Width = 50
                dgvMercancias.Columns("Documento").HeaderText = "Documento"
                dgvMercancias.Columns("Documento").Width = 120
                dgvMercancias.Columns("Fecha").HeaderText = "Fecha"
                dgvMercancias.Columns("Fecha").Width = 100
                dgvMercancias.Columns("Fiscalizador").HeaderText = "Fiscalizador"
                dgvMercancias.Columns("Fiscalizador").Width = 120
                dgvMercancias.Columns("Mercancia").HeaderText = "Mercancía"
                dgvMercancias.Columns("Mercancia").Width = 150
                dgvMercancias.Columns("Localidad").HeaderText = "Localidad"
                dgvMercancias.Columns("Localidad").Width = 100
                dgvMercancias.Columns("BGOrg").HeaderText = "BG/Org"
                dgvMercancias.Columns("BGOrg").Width = 100
                
                ' Ocultar la columna RutaPDF (no es necesario mostrarla)
                If dgvMercancias.Columns.Contains("RutaPDF") Then
                    dgvMercancias.Columns("RutaPDF").Visible = False
                End If
                
                ' Agregar columna de botón para abrir PDF
                If Not dgvMercancias.Columns.Contains("VerPDF") Then
                    Dim pdfColumn As New DataGridViewButtonColumn()
                    pdfColumn.Name = "VerPDF"
                    pdfColumn.HeaderText = "Ver PDF"
                    pdfColumn.Text = "Ver"
                    pdfColumn.UseColumnTextForButtonValue = True
                    pdfColumn.Width = 80
                    dgvMercancias.Columns.Add(pdfColumn)
                End If
            End If
            
            lblContador.Text = "Total de mercancías: " & mercancias.Count.ToString()
        Catch ex As Exception
            MessageBox.Show("Error al cargar mercancías: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs)
        Dim formAgregar As New FormAgregar()
        If formAgregar.ShowDialog() = DialogResult.OK Then
            CargarMercancias()
        End If
    End Sub

    Private Sub BtnEditar_Click(sender As Object, e As EventArgs)
        If dgvMercancias.SelectedRows.Count > 0 Then
            Dim mercanciaSeleccionada = CType(dgvMercancias.SelectedRows(0).DataBoundItem, Mercancia)
            Dim formAgregar As New FormAgregar(mercanciaSeleccionada)
            If formAgregar.ShowDialog() = DialogResult.OK Then
                CargarMercancias()
            End If
        End If
    End Sub

    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs)
        If dgvMercancias.SelectedRows.Count > 0 Then
            Dim mercanciaSeleccionada = CType(dgvMercancias.SelectedRows(0).DataBoundItem, Mercancia)
            Dim result = MessageBox.Show("¿Está seguro de eliminar esta mercancía?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            
            If result = DialogResult.Yes Then
                If DatabaseManager.EliminarMercancia(mercanciaSeleccionada.Id) Then
                    MessageBox.Show("Mercancía eliminada exitosamente")
                    CargarMercancias()
                End If
            End If
        End If
    End Sub

    Private Sub BtnGenerarEtiqueta_Click(sender As Object, e As EventArgs)
        If dgvMercancias.SelectedRows.Count > 0 Then
            Dim mercanciaSeleccionada = CType(dgvMercancias.SelectedRows(0).DataBoundItem, Mercancia)
            Dim rutaPDF As String = PDFGenerator.GenerarEtiquetaParaImpresion(mercanciaSeleccionada)
            
            ' Actualizar la ruta PDF en la base de datos si se generó correctamente
            If Not String.IsNullOrEmpty(rutaPDF) Then
                DatabaseManager.ActualizarRutaPDF(mercanciaSeleccionada.Id, rutaPDF)
                CargarMercancias() ' Refrescar la lista para mostrar el nuevo estado
            End If
        End If
    End Sub

    Private Sub BtnRefrescar_Click(sender As Object, e As EventArgs)
        CargarMercancias()
    End Sub

    Private Sub DgvMercancias_SelectionChanged(sender As Object, e As EventArgs)
        Dim haySeleccion = dgvMercancias.SelectedRows.Count > 0
        btnEditar.Enabled = haySeleccion
        btnEliminar.Enabled = haySeleccion
        btnGenerarEtiqueta.Enabled = haySeleccion
    End Sub

    Private Sub DgvMercancias_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        ' Verificar si se hizo clic en la columna "Ver PDF"
        If e.ColumnIndex >= 0 AndAlso e.RowIndex >= 0 Then
            If dgvMercancias.Columns(e.ColumnIndex).Name = "VerPDF" Then
                Dim mercanciaSeleccionada = CType(dgvMercancias.Rows(e.RowIndex).DataBoundItem, Mercancia)
                
                If Not String.IsNullOrEmpty(mercanciaSeleccionada.RutaPDF) Then
                    PDFGenerator.AbrirPDF(mercanciaSeleccionada.RutaPDF)
                Else
                    ' Si no hay PDF, generar uno nuevo
                    Dim result = MessageBox.Show("No hay etiqueta PDF para esta mercancía. ¿Desea generar una?", 
                                                "Sin Etiqueta", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If result = DialogResult.Yes Then
                        Dim rutaPDF As String = PDFGenerator.GenerarEtiquetaParaImpresion(mercanciaSeleccionada)
                        If Not String.IsNullOrEmpty(rutaPDF) Then
                            DatabaseManager.ActualizarRutaPDF(mercanciaSeleccionada.Id, rutaPDF)
                            CargarMercancias() ' Refrescar la lista
                        End If
                    End If
                End If
            End If
        End If
    End Sub
End Class