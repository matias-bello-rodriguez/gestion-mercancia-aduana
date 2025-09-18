Imports System
Imports System.Windows.Forms

Module Module1
    <STAThread>
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New FormPrincipal())
    End Sub
End Module