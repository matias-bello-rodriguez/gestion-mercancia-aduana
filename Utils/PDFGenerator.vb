Imports iText.Kernel.Pdf
Imports iText.Layout
Imports iText.Layout.Element
Imports iText.Layout.Properties
Imports System.IO
Imports System.Windows.Forms

Public Class PDFGenerator
    Public Shared Function GenerarEtiquetaPDF(mercancia As Mercancia, Optional mostrarMensajes As Boolean = True) As String
        Try
            ' Crear carpeta Output si no existe
            If Not Directory.Exists("Output") Then
                Directory.CreateDirectory("Output")
            End If

            ' Generar nombre de archivo único
            Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
            Dim fileName As String = "Etiqueta_Mercancia_" & mercancia.Id.ToString() & "_" & timestamp & ".pdf"
            Dim fullPath As String = Path.Combine("Output", fileName)

            ' Crear PDF
            Using writer As New PdfWriter(fullPath)
                Using pdf As New PdfDocument(writer)
                    Using document As New Document(pdf)
                        ' Título principal
                        Dim titulo As New Paragraph("MERCANCIA INCAUTADA")
                        titulo.SetTextAlignment(TextAlignment.CENTER)
                        titulo.SetFontSize(20)
                        titulo.SetBold()
                        document.Add(titulo)

                        ' Línea separadora
                        document.Add(New Paragraph("═".PadRight(50, "═"c)).SetTextAlignment(TextAlignment.CENTER))
                        
                        ' Espaciado
                        document.Add(New Paragraph(" "))

                        ' Crear tabla para la información
                        Dim table As New Table(2)
                        table.SetWidth(UnitValue.CreatePercentValue(100))

                        ' Agregar datos a la tabla
                        table.AddCell(New Cell().Add(New Paragraph("ID:").SetBold()))
                        table.AddCell(New Cell().Add(New Paragraph(mercancia.Id.ToString())))

                        table.AddCell(New Cell().Add(New Paragraph("Documento:").SetBold()))
                        table.AddCell(New Cell().Add(New Paragraph(mercancia.Documento)))

                        table.AddCell(New Cell().Add(New Paragraph("Fecha:").SetBold()))
                        table.AddCell(New Cell().Add(New Paragraph(mercancia.Fecha)))

                        table.AddCell(New Cell().Add(New Paragraph("Fiscalizador:").SetBold()))
                        table.AddCell(New Cell().Add(New Paragraph(mercancia.Fiscalizador)))

                        table.AddCell(New Cell().Add(New Paragraph("Mercancía:").SetBold()))
                        table.AddCell(New Cell().Add(New Paragraph(mercancia.Mercancia)))

                        table.AddCell(New Cell().Add(New Paragraph("Localidad:").SetBold()))
                        table.AddCell(New Cell().Add(New Paragraph(mercancia.Localidad)))

                        table.AddCell(New Cell().Add(New Paragraph("BG/Org:").SetBold()))
                        table.AddCell(New Cell().Add(New Paragraph(mercancia.BGOrg)))

                        document.Add(table)

                        ' Espaciado
                        document.Add(New Paragraph(" "))

                        ' Datos para QR (texto plano por ahora)
                        Dim qrData As String = mercancia.Documento & "|" & mercancia.Fecha & "|" & mercancia.Fiscalizador
                        document.Add(New Paragraph("Código de verificación:").SetBold())
                        document.Add(New Paragraph(qrData).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER))

                        ' Pie de página
                        document.Add(New Paragraph(" "))
                        Dim piePagina As New Paragraph("Generado el: " & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
                        piePagina.SetTextAlignment(TextAlignment.CENTER)
                        piePagina.SetFontSize(10)
                        document.Add(piePagina)
                    End Using
                End Using
            End Using

            If mostrarMensajes Then
                MessageBox.Show("Etiqueta PDF generada exitosamente:" & Environment.NewLine & fullPath, 
                              "PDF Generado", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Preguntar si abrir el archivo
                Dim result As DialogResult = MessageBox.Show("¿Desea abrir el archivo PDF generado?", 
                                                            "Abrir PDF", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = DialogResult.Yes Then
                    AbrirPDF(fullPath)
                End If
            End If

            Return fullPath

        Catch ex As Exception
            If mostrarMensajes Then
                MessageBox.Show("Error al generar PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            Return ""
        End Try
    End Function

    Public Shared Sub AbrirPDF(rutaPDF As String)
        Try
            If File.Exists(rutaPDF) Then
                Dim psi As New System.Diagnostics.ProcessStartInfo()
                psi.FileName = rutaPDF
                psi.UseShellExecute = True
                System.Diagnostics.Process.Start(psi)
            Else
                MessageBox.Show("El archivo PDF no existe: " & rutaPDF, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("No se pudo abrir el archivo PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Shared Function GenerarEtiquetaParaImpresion(mercancia As Mercancia) As String
        Dim rutaPDF As String = GenerarEtiquetaPDF(mercancia, False)
        
        If Not String.IsNullOrEmpty(rutaPDF) Then
            Dim result As DialogResult = MessageBox.Show("Etiqueta generada. ¿Desea imprimirla ahora?", 
                                                        "Imprimir Etiqueta", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            
            Select Case result
                Case DialogResult.Yes
                    ' Abrir para imprimir
                    AbrirPDF(rutaPDF)
                Case DialogResult.No
                    ' Solo mostrar ubicación
                    MessageBox.Show("Etiqueta guardada en: " & rutaPDF, "Etiqueta Guardada", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Select
        End If
        
        Return rutaPDF
    End Function
End Class