Imports Microsoft.Data.Sqlite
Imports System.IO
Imports System.Windows.Forms

Public Class DatabaseManager
    Private Shared ReadOnly ConnectionString As String = "Data Source=aduana.db"

    ''' <summary>
    ''' Inicializa la base de datos y crea la tabla si no existe
    ''' </summary>
    Public Shared Sub InicializarBaseDatos()
        If Not File.Exists("aduana.db") Then
            Using connection As New SqliteConnection(ConnectionString)
                connection.Open()
                Dim createTableQuery As String = "
                    CREATE TABLE IF NOT EXISTS Mercancias (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Documento TEXT NOT NULL,
                        Fecha TEXT NOT NULL,
                        Fiscalizador TEXT NOT NULL,
                        Mercancia TEXT NOT NULL,
                        Localidad TEXT NOT NULL,
                        BGOrg TEXT NOT NULL,
                        RutaPDF TEXT
                    )"
                
                Using command As New SqliteCommand(createTableQuery, connection)
                    command.ExecuteNonQuery()
                End Using
            End Using
        Else
            ' Verificar y agregar columna RutaPDF si no existe en una base de datos existente
            Using connection As New SqliteConnection(ConnectionString)
                connection.Open()
                Try
                    Dim checkColumnQuery As String = "PRAGMA table_info(Mercancias)"
                    Using checkCommand As New SqliteCommand(checkColumnQuery, connection)
                        Using reader = checkCommand.ExecuteReader()
                            Dim tieneRutaPDF As Boolean = False
                            While reader.Read()
                                If reader("name").ToString() = "RutaPDF" Then
                                    tieneRutaPDF = True
                                    Exit While
                                End If
                            End While
                            reader.Close()
                            
                            If Not tieneRutaPDF Then
                                Dim addColumnQuery As String = "ALTER TABLE Mercancias ADD COLUMN RutaPDF TEXT"
                                Using addCommand As New SqliteCommand(addColumnQuery, connection)
                                    addCommand.ExecuteNonQuery()
                                End Using
                            End If
                        End Using
                    End Using
                Catch ex As Exception
                    ' Si hay error al agregar la columna, es probable que ya exista
                End Try
            End Using
        End If
    End Sub

    ''' <summary>
    ''' Obtiene todas las mercancías de la base de datos
    ''' </summary>
    ''' <returns>Lista de mercancías</returns>
    Public Shared Function ObtenerTodasLasMercancias() As List(Of Mercancia)
        Dim mercancias As New List(Of Mercancia)()
        
        Using connection As New SqliteConnection(ConnectionString)
            connection.Open()
            Dim query As String = "SELECT * FROM Mercancias ORDER BY Id DESC"
            
            Using command As New SqliteCommand(query, connection)
                Using reader As SqliteDataReader = command.ExecuteReader()
                    While reader.Read()
                        mercancias.Add(New Mercancia() With {
                            .Id = Convert.ToInt32(reader("Id")),
                            .Documento = reader("Documento").ToString(),
                            .Fecha = reader("Fecha").ToString(),
                            .Fiscalizador = reader("Fiscalizador").ToString(),
                            .Mercancia = reader("Mercancia").ToString(),
                            .Localidad = reader("Localidad").ToString(),
                            .BGOrg = reader("BGOrg").ToString(),
                            .RutaPDF = If(reader("RutaPDF") IsNot DBNull.Value, reader("RutaPDF").ToString(), "")
                        })
                    End While
                End Using
            End Using
        End Using
        
        Return mercancias
    End Function

    ''' <summary>
    ''' Inserta una nueva mercancía en la base de datos
    ''' </summary>
    ''' <param name="mercancia">Objeto mercancía a insertar</param>
    ''' <returns>True si se insertó correctamente</returns>
    Public Shared Function AgregarMercancia(mercancia As Mercancia) As Boolean
        Try
            Using connection As New SqliteConnection(ConnectionString)
                connection.Open()
                
                Dim query As String
                If mercancia.Id = 0 Then
                    ' Insertar nueva mercancía
                    query = "INSERT INTO Mercancias (Documento, Fecha, Fiscalizador, Mercancia, Localidad, BGOrg, RutaPDF) VALUES (@Documento, @Fecha, @Fiscalizador, @Mercancia, @Localidad, @BGOrg, @RutaPDF)"
                Else
                    ' Actualizar mercancía existente
                    query = "UPDATE Mercancias SET Documento = @Documento, Fecha = @Fecha, Fiscalizador = @Fiscalizador, Mercancia = @Mercancia, Localidad = @Localidad, BGOrg = @BGOrg, RutaPDF = @RutaPDF WHERE Id = @Id"
                End If
                
                Using command As New SqliteCommand(query, connection)
                    command.Parameters.AddWithValue("@Documento", mercancia.Documento)
                    command.Parameters.AddWithValue("@Fecha", mercancia.Fecha)
                    command.Parameters.AddWithValue("@Fiscalizador", mercancia.Fiscalizador)
                    command.Parameters.AddWithValue("@Mercancia", mercancia.Mercancia)
                    command.Parameters.AddWithValue("@Localidad", mercancia.Localidad)
                    command.Parameters.AddWithValue("@BGOrg", mercancia.BGOrg)
                    command.Parameters.AddWithValue("@RutaPDF", If(String.IsNullOrEmpty(mercancia.RutaPDF), DBNull.Value, mercancia.RutaPDF))
                    
                    If mercancia.Id > 0 Then
                        command.Parameters.AddWithValue("@Id", mercancia.Id)
                    End If
                    
                    command.ExecuteNonQuery()
                End Using
            End Using
            Return True
        Catch ex As Exception
            MessageBox.Show("Error al guardar mercancía: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Elimina una mercancía de la base de datos
    ''' </summary>
    ''' <param name="id">ID de la mercancía a eliminar</param>
    ''' <returns>True si se eliminó correctamente</returns>
    Public Shared Function EliminarMercancia(id As Integer) As Boolean
        Try
            Using connection As New SqliteConnection(ConnectionString)
                connection.Open()
                Dim query As String = "DELETE FROM Mercancias WHERE Id = @Id"
                
                Using command As New SqliteCommand(query, connection)
                    command.Parameters.AddWithValue("@Id", id)
                    command.ExecuteNonQuery()
                End Using
            End Using
            Return True
        Catch ex As Exception
            MessageBox.Show("Error al eliminar mercancía: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Actualiza la ruta del PDF de una mercancía específica
    ''' </summary>
    ''' <param name="id">ID de la mercancía</param>
    ''' <param name="rutaPDF">Ruta del archivo PDF</param>
    ''' <returns>True si se actualizó correctamente</returns>
    Public Shared Function ActualizarRutaPDF(id As Integer, rutaPDF As String) As Boolean
        Try
            Using connection As New SqliteConnection(ConnectionString)
                connection.Open()
                Dim query As String = "UPDATE Mercancias SET RutaPDF = @RutaPDF WHERE Id = @Id"
                
                Using command As New SqliteCommand(query, connection)
                    command.Parameters.AddWithValue("@RutaPDF", rutaPDF)
                    command.Parameters.AddWithValue("@Id", id)
                    command.ExecuteNonQuery()
                End Using
            End Using
            Return True
        Catch ex As Exception
            MessageBox.Show("Error al actualizar ruta PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
End Class