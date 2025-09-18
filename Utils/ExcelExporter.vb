Imports ClosedXML.Excel
Imports System.IO
Imports System.Windows.Forms
Imports QRCoder
Imports System.Drawing
Imports System.Drawing.Imaging
' Asegurar que estamos importando correctamente los tipos específicos de ClosedXML
Imports IXLWorksheet = ClosedXML.Excel.IXLWorksheet
Imports XLWorkbook = ClosedXML.Excel.XLWorkbook
Imports XLColor = ClosedXML.Excel.XLColor
Imports XLBorderStyleValues = ClosedXML.Excel.XLBorderStyleValues
Imports XLAlignmentHorizontalValues = ClosedXML.Excel.XLAlignmentHorizontalValues

Public Class ExcelExporter
    
    ''' <summary>
    ''' Genera un código QR como imagen en memoria
    ''' </summary>
    ''' <param name="texto">Texto a codificar en el QR</param>
    ''' <returns>Imagen del código QR</returns>
    Private Shared Function GenerarQRParaExcel(texto As String) As System.Drawing.Image
        Try
            Dim qrGenerator As New QRCodeGenerator()
            Dim qrCodeData As QRCodeData = qrGenerator.CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q)
            Dim qrCode As New QRCode(qrCodeData)
            Return qrCode.GetGraphic(10, Color.Black, Color.White, True)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    
    ''' <summary>
    ''' Exporta todas las mercancías a un archivo Excel con códigos QR
    ''' </summary>
    ''' <param name="mercancias">Lista de mercancías a exportar</param>
    ''' <returns>True si la exportación fue exitosa</returns>
    Public Shared Function ExportarMercanciasAExcel(mercancias As List(Of Mercancia)) As Boolean
        Try
            ' Crear diálogo para guardar archivo
            Using saveDialog As New SaveFileDialog()
                saveDialog.Filter = "Archivos Excel (*.xlsx)|*.xlsx"
                saveDialog.Title = "Exportar Mercancías a Excel"
                saveDialog.FileName = "Mercancias_Aduanas_" & DateTime.Now.ToString("yyyyMMdd_HHmmss") & ".xlsx"
                
                If saveDialog.ShowDialog() = DialogResult.OK Then
                    ' Crear libro de Excel con ClosedXML
                    Using workbook As New XLWorkbook()
                        ' Crear hoja de trabajo
                        Dim worksheet = workbook.Worksheets.Add("Mercancías Incautadas")
                        
                        ' Configurar encabezados
                        ConfigurarEncabezados(worksheet)
                        
                        ' Llenar datos
                        LlenarDatosMercancias(worksheet, mercancias)
                        
                        ' Ajustar columnas
                        AjustarColumnas(worksheet)
                        
                        ' Guardar archivo
                        workbook.SaveAs(saveDialog.FileName)
                        
                        MessageBox.Show("Exportación exitosa. Archivo guardado en:" & Environment.NewLine & saveDialog.FileName, 
                                      "Exportación Completa", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        
                        ' Preguntar si abrir el archivo
                        Dim result = MessageBox.Show("¿Desea abrir el archivo Excel generado?", 
                                                   "Abrir Excel", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If result = DialogResult.Yes Then
                            Try
                                System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo() With {
                                    .FileName = saveDialog.FileName,
                                    .UseShellExecute = True
                                })
                            Catch ex As Exception
                                MessageBox.Show("No se pudo abrir el archivo: " & ex.Message)
                            End Try
                        End If
                        
                        Return True
                    End Using
                End If
            End Using
            
            Return False
            
        Catch ex As Exception
            MessageBox.Show("Error al exportar a Excel: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    
    ''' <summary>
    ''' Configura los encabezados del archivo Excel
    ''' </summary>
    Private Shared Sub ConfigurarEncabezados(worksheet As IXLWorksheet)
        ' Título principal
        worksheet.Cell("A1").Value = "MERCANCÍAS INCAUTADAS - ADUANAS CHILE"
        worksheet.Range("A1:H1").Merge()
        worksheet.Cell("A1").Style.Font.SetBold(True).Font.SetFontSize(16)
        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
        
        ' Fecha de exportación
        worksheet.Cell("A2").Value = "Exportado el: " & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
        worksheet.Range("A2:H2").Merge()
        worksheet.Cell("A2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center)
        
        ' Encabezados de columnas (fila 4)
        worksheet.Cell("A4").Value = "ID"
        worksheet.Cell("B4").Value = "Documento"
        worksheet.Cell("C4").Value = "Fecha"
        worksheet.Cell("D4").Value = "Fiscalizador"
        worksheet.Cell("E4").Value = "Mercancía"
        worksheet.Cell("F4").Value = "Localidad"
        worksheet.Cell("G4").Value = "BG/Org"
        worksheet.Cell("H4").Value = "Código QR"
        
        ' Estilo de encabezados
        Dim headerRange = worksheet.Range("A4:H4")
        headerRange.Style.Font.SetBold(True)
        headerRange.Style.Fill.SetBackgroundColor(XLColor.LightBlue)
        headerRange.Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
        headerRange.Style.Border.SetInsideBorder(XLBorderStyleValues.Thin)
    End Sub
    
    ''' <summary>
    ''' Llena los datos de las mercancías en el Excel
    ''' </summary>
    Private Shared Sub LlenarDatosMercancias(worksheet As IXLWorksheet, mercancias As List(Of Mercancia))
        Dim fila As Integer = 5 ' Empezar en fila 5
        
        For Each mercancia In mercancias
            ' Datos básicos
            worksheet.Cell(fila, 1).Value = mercancia.Id
            worksheet.Cell(fila, 2).Value = mercancia.Documento
            worksheet.Cell(fila, 3).Value = mercancia.Fecha
            worksheet.Cell(fila, 4).Value = mercancia.Fiscalizador
            worksheet.Cell(fila, 5).Value = mercancia.Mercancia
            worksheet.Cell(fila, 6).Value = mercancia.Localidad
            worksheet.Cell(fila, 7).Value = mercancia.BGOrg
            
            ' Generar QR para esta mercancía
            Dim qrImage = GenerarQRParaExcel(mercancia.Documento)
            
            If qrImage IsNot Nothing Then
                Try
                    ' Crear carpeta temporal si no existe
                    Dim tempFolder = Path.Combine(Path.GetTempPath(), "AduanaExcelTemp")
                    If Not Directory.Exists(tempFolder) Then
                        Directory.CreateDirectory(tempFolder)
                    End If
                    
                    ' Guardar QR como imagen temporal
                    Dim qrImagePath = Path.Combine(tempFolder, $"qr_temp_{mercancia.Id}_{Guid.NewGuid()}.png")
                    qrImage.Save(qrImagePath, ImageFormat.Png)
                    
                    ' Colocar QR DENTRO de la celda usando celdas contiguas para fijar
                    ' En ClosedXML tenemos que usar una técnica diferente para asegurar que la imagen quede fija a la celda
                    Dim altoEnPuntos As Double = 4.00 * 28.3 ' 113.2 puntos
                    Dim anchoEnPuntos As Double = 4.53 * 28.3 ' 128.2 puntos
                    
                    ' Obtener la celda para el QR
                    Dim cell = worksheet.Cell(fila, 8)
                    
                    ' Añadir la imagen a la celda
                    Dim picture = worksheet.AddPicture(qrImagePath)
                    
                    ' Anclar a la celda específica (método 1)
                    picture.MoveTo(cell, 0, 0)
                    
                    ' Aseguramos que la imagen tenga el tamaño adecuado
                    picture.WithSize(altoEnPuntos, anchoEnPuntos)
                    
                    ' Establecemos un valor de celda vacío para asegurar que la imagen se asocie con la celda
                    cell.Value = "" ' Asegura que la celda existe y tiene un valor
                    
                    ' Eliminar imagen temporal después de usarla
                    Try
                        File.Delete(qrImagePath)
                    Catch
                        ' Ignorar errores al eliminar archivos temporales
                    End Try
                Catch ex As Exception
                    ' Si falla al agregar la imagen, mostrar texto alternativo
                    worksheet.Cell(fila, 8).Value = "QR: " & mercancia.Documento
                End Try
            Else
                worksheet.Cell(fila, 8).Value = "QR: " & mercancia.Documento
            End If
            
            ' Establecer altura de la fila para el QR con base en las dimensiones requeridas
            ' La altura debe ser al menos 2,59 cm (convertidos a puntos) + un pequeño margen
            worksheet.Row(fila).Height = 2.59 * 28.3 + 5 ' 73.3 puntos + margen
            
            ' Bordes para las celdas de datos
            worksheet.Range(fila, 1, fila, 8).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            worksheet.Range(fila, 1, fila, 8).Style.Border.SetInsideBorder(XLBorderStyleValues.Thin)
            
            fila += 1
        Next
    End Sub
    
    ''' <summary>
    ''' Ajusta el ancho de las columnas
    ''' </summary>
    Private Shared Sub AjustarColumnas(worksheet As IXLWorksheet)
        worksheet.Column(1).Width = 8   ' ID
        worksheet.Column(2).Width = 20  ' Documento
        worksheet.Column(3).Width = 15  ' Fecha
        worksheet.Column(4).Width = 20  ' Fiscalizador
        worksheet.Column(5).Width = 25  ' Mercancía
        worksheet.Column(6).Width = 18  ' Localidad
        worksheet.Column(7).Width = 12  ' BG/Org
        ' Configurar ancho para el QR con base en las dimensiones requeridas (4,53 cm)
        ' El ancho en ClosedXML se mide en unidades diferentes, necesitamos convertir
        ' Aproximadamente, un ancho de columna de 20 equivale a unos 4,5 cm
        worksheet.Column(8).Width = 20  ' Ajustado para que sea de 4,53 cm de ancho
    End Sub
End Class