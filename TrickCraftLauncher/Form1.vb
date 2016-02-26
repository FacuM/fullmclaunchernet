Imports System.Net.Sockets
Imports System.Net
Imports System.IO
Imports System.Text

Public Class Form1
    Dim sourceString As String
    Dim TiempoMin As Integer
    Dim TiempoSeg As Integer
    Dim IP As String
    Dim PingInMS As Integer
    Dim Alertas As String
    Dim Veces As Integer
    Dim Version As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Colocar las variable "veces" en 0
        Veces = 0
        ' Resolver la IP actual de trickcraft.servegame.com
        Try
            Dim myIPHostEntry As IPHostEntry = Dns.Resolve("trickcraft.servegame.com")
            Dim myIPAddresses() As IPAddress = myIPHostEntry.AddressList
            Dim myIPAddress As IPAddress

            For Each myIPAddress In myIPAddresses

                IP = myIPAddress.ToString

            Next
        Catch ex As SocketException
            Console.WriteLine(ex.Message)
        End Try
        ' Colocar el temporizador en 4:59 minutos.
        TiempoMin = 5
        TiempoSeg = 0
        ' Obtener las alertas y convertilas a texto.
        Dim address As String = "http://trickcraft.servegame.com/information.txt"
        Try
            Dim client As WebClient = New WebClient()
            Dim reader As StreamReader = New StreamReader(client.OpenRead(address))
            Label1.Text = reader.ReadToEnd
        Catch ex As Exception
            Label1.Text = "Error de conexion."
        End Try
        ' Guardar las alertas en la variable "Alertas" para uso global alterno de la aplicacion
        Alertas = Label1.Text
        ' Intentar conectarse con el servidor de Minecraft.
        Dim MinecraftServer As New TcpClient
        Try
            MinecraftServer.Connect("trickcraft.servegame.com", 25565)
            Label2.Text = "IP: trickcraft.servegame.com" & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & "El servidor esta funcionando."
        Catch Excep As Exception
            Label2.Text = "IP: trickcraft.servegame.com" & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & "El servidor esta funcionando."
        End Try
        ' Verificar el estado del servidor web.
        Dim str As String = New WebClient().DownloadString(("http://downforeveryoneorjustme.com/" + "trickcraft.servegame.com"))
        If str.Contains("It's not just you!") Then
            Label3.Text = "Fue imposible conectarse con el servidor, por favor inténtalo nuevamente más tarde." & vbCrLf & vbCrLf & "(se intentó con 'trickcraft.servegame.com')"
        Else
            Label3.Text = "El servidor está funcionando, si no puedes conectarte al servidor de Minecraft por favor verifica la información del cuadro inferior." & vbCrLf & vbCrLf & "(se intentó con 'trickcraft.servegame.com')"
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' Obtener las alertas y convertilas a texto.
        Dim address As String = "http://trickcraft.servegame.com/information.txt"
        Try
            Dim client As WebClient = New WebClient()
            Dim reader As StreamReader = New StreamReader(client.OpenRead(address))
            Label1.Text = reader.ReadToEnd
        Catch ex As Exception
            Label1.Text = "Error de conexion."
        End Try
        ' Comprobar la presencia de cambios en las alertas descargadas, si los hay, mostrar un popup advirtiendolo [...]
        If Alertas <> Label1.Text Then
            MessageBox.Show("Hay nuevas alertas publicadas, por favor leelas.", _
            "Cambio en las alertas")
            ' y ademas, indicarlo parpadeando el icono en la barra de tareas.
            Dim res = WindowsApi.FlashWindow(Process.GetCurrentProcess().MainWindowHandle, True, True, 5)
            Alertas = Label1.Text
        End If
        ' Intentar conectarse con el servidor de Minecraft.
        Dim MinecraftServer As New TcpClient
        Try
            MinecraftServer.Connect("trickcraft.servegame.com", 25565)
            Label2.Text = "IP: trickcraft.servegame.com" & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & "El servidor esta funcionando."
        Catch Excep As Exception
            Label2.Text = "IP: trickcraft.servegame.com" & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & "El servidor esta funcionando."
        End Try
        ' Verificar el estado del servidor web.
        Dim str As String = New WebClient().DownloadString(("http://downforeveryoneorjustme.com/" + "trickcraft.servegame.com"))
        If str.Contains("It's not just you!") Then
            Label3.Text = "Fue imposible conectarse con el servidor, por favor inténtalo nuevamente más tarde." & vbCrLf & vbCrLf & "(se intentó con 'trickcraft.servegame.com')"
        Else
            Label3.Text = "El servidor está funcionando, si no puedes conectarte al servidor de Minecraft por favor verifica la información del cuadro inferior." & vbCrLf & vbCrLf & "(se intentó con 'trickcraft.servegame.com')"
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        ToolStripStatusLabel1.Text = "Actualizando en " & TiempoMin & ":" & TiempoSeg & " minutos" & " o "
        If TiempoMin >= 0 Then
            If TiempoMin = 0 And TiempoSeg = 0 Then
                TiempoMin = 5
                TiempoSeg = 0
            End If
            If TiempoSeg <= 59 And TiempoSeg > 0 Then
                TiempoSeg = TiempoSeg - 1
            Else
                TiempoMin = TiempoMin - 1
                TiempoSeg = 59
            End If
        End If
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        Try
            Dim ping As Net.NetworkInformation.Ping = New Net.NetworkInformation.Ping()
            Dim pingreply As Net.NetworkInformation.PingReply = ping.Send(IP)

            Label4.Text = "Ping: " & pingreply.RoundtripTime & "ms"
            If pingreply.RoundtripTime < 1500 Then
                ProgressBar1.Value = pingreply.RoundtripTime
            Else
                ProgressBar1.Value = 1500
            End If
            PingInMS = pingreply.RoundtripTime
        Catch
            Label4.Text = "Ping: " & "no fue posible establecer una conexión al servidor."
        End Try
    End Sub

    Private Sub Timer4_Tick(sender As Object, e As EventArgs) Handles Timer4.Tick
        Try
            Dim ping As Net.NetworkInformation.Ping = New Net.NetworkInformation.Ping()
            Dim pingreply As Net.NetworkInformation.PingReply = ping.Send(IP)

            Label4.Text = "Ping: " & "pinging..."
        Catch
            Label4.Text = "Ping: " & "no fue posible establecer una conexión al servidor."
        End Try
        Timer4.Enabled = False
        Timer3.Enabled = True
    End Sub

    Private Sub ToolStripSplitButton1_ButtonClick(sender As Object, e As EventArgs) Handles ToolStripSplitButton1.ButtonClick
        ' Habilita el parpadeo del boton
        Timer5.Enabled = True
        ' Obtener las alertas y convertilas a texto.
        Dim address As String = "http://trickcraft.servegame.com/information.txt"
        Try
            Dim client As WebClient = New WebClient()
            Dim reader As StreamReader = New StreamReader(client.OpenRead(address))
            Label1.Text = reader.ReadToEnd
        Catch ex As Exception
            Label1.Text = "Error de conexion."
        End Try

        ''''' Se omite por la presencia del usuario interaccionando '''''
       
        ' Comprobar la presencia de cambios en las alertas descargadas, si los hay, mostrar un popup advirtiendolo [...]
        'If Alertas <> Label1.Text Then
        '   MessageBox.Show("Hay nuevas alertas publicadas, por favor leelas.", _
        '   "Cambio en las alertas")
        ' y ademas, indicarlo parpadeando el icono en la barra de tareas.
        '   Dim res = WindowsApi.FlashWindow(Process.GetCurrentProcess().MainWindowHandle, True, True, 5)
        '   Alertas = Label1.Text
        'End If

        ' Intentar conectarse con el servidor de Minecraft.
        Dim MinecraftServer As New TcpClient
        Try
            MinecraftServer.Connect("trickcraft.servegame.com", 25565)
            Label2.Text = "IP: trickcraft.servegame.com" & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & "El servidor esta funcionando."
        Catch Excep As Exception
            Label2.Text = "IP: trickcraft.servegame.com" & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf & "El servidor esta funcionando."
        End Try
        ' Verificar el estado del servidor web.
        Dim str As String = New WebClient().DownloadString(("http://downforeveryoneorjustme.com/" + "trickcraft.servegame.com"))
        If str.Contains("It's not just you!") Then
            Label3.Text = "Fue imposible conectarse con el servidor, por favor inténtalo nuevamente más tarde." & vbCrLf & vbCrLf & "(se intentó con 'trickcraft.servegame.com')"
        Else
            Label3.Text = "El servidor está funcionando, si no puedes conectarte al servidor de Minecraft por favor verifica la información del cuadro inferior." & vbCrLf & vbCrLf & "(se intentó con 'trickcraft.servegame.com')"
        End If
    End Sub

    Private Sub Timer5_Tick(sender As Object, e As EventArgs) Handles Timer5.Tick
        If Veces < 10 Then
            If ToolStripSplitButton1.Visible = True Then
                ToolStripSplitButton1.Visible = False
                ToolStripSplitButton1.Enabled = False
                Veces = Veces + 1
                ToolStripStatusLabel1.Enabled = False
                Timer2.Enabled = False
            Else
                ToolStripSplitButton1.Visible = True
                ToolStripSplitButton1.Enabled = False
                Veces = Veces + 1
                ToolStripStatusLabel1.Enabled = False
                Timer2.Enabled = False
            End If
        End If
        If Veces >= 10 Then
            ToolStripSplitButton1.Visible = True
            ToolStripSplitButton1.Enabled = True
            Veces = 0
            ToolStripStatusLabel1.Enabled = True
            Timer5.Enabled = False
            TiempoMin = 5
            TiempoSeg = 0
            Timer2.Enabled = True
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ComboBox1.SelectedItem = Version
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False
        TextBox1.Text = "Ejecutando..."
        TextBox1.Enabled = False
        Process.Start("cd C:\Users\" & Environment.UserName & "AppData\Roaming\.minecraft\ &&" & "java -DsocksProxyHost=127.0.0.1 -DsocksProxyPort=24454 -XX:HeapDumpPath=MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump -Xmx1G -Dfml.ignorePatchDiscrepancies=true -Dfml.ignoreInvalidMinecraftCertificates=true -Djava.library.path=C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\versions\1.7.4\1.7.4-natives-5719613247633 -cp C:\Users\" & Environment.UserName & " \AppData\Roaming\.minecraft\libraries\java3d\vecmath\1.3.1\vecmath-1.3.1.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\net\sf\trove4j\trove4j\3.0.3\trove4j-3.0.3.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\com\ibm\icu\icu4j-core-mojang\51.2\icu4j-core-mojang-51.2.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\net\sf\jopt-simple\jopt-simple\4.5\jopt-simple-4.5.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\com\paulscode\codecjorbis\20101023\codecjorbis-20101023.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\com\paulscode\codecwav\20101023\codecwav-20101023.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\com\paulscode\libraryjavasound\20101123\libraryjavasound-20101123.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\com\paulscode\librarylwjglopenal\20100824\librarylwjglopenal-20100824.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\com\paulscode\soundsystem\20120107\soundsystem-20120107.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\io\netty\netty-all\4.0.10.Final\netty-all-4.0.10.Final.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\com\google\guava\guava\15.0\guava-15.0.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\org\apache\commons\commons-lang3\3.1\commons-lang3-3.1.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\commons-io\commons-io\2.4\commons-io-2.4.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\net\java\jinput\jinput\2.0.5\jinput-2.0.5.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\net\java\jutils\jutils\1.0.0\jutils-1.0.0.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\com\google\code\gson\gson\2.2.4\gson-2.2.4.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\com\mojang\authlib\1.2\authlib-1.2_patched.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\org\apache\logging\log4j\log4j-api\2.0-beta9\log4j-api-2.0-beta9.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\org\apache\logging\log4j\log4j-core\2.0-beta9\log4j-core-2.0-beta9.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\org\lwjgl\lwjgl\lwjgl\2.9.1-nightly-20131120\lwjgl-2.9.1-nightly-20131120.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\org\lwjgl\lwjgl\lwjgl_util\2.9.1-nightly-20131120\lwjgl_util-2.9.1-nightly-20131120.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\libraries\tv\twitch\twitch\5.16\twitch-5.16.jar;C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\versions\1.7.4\1.7.4.jar net.minecraft.client.main.Main --username " & Environment.UserName & "Armo --version 1.7.4 --gameDir C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft --assetsDir C:\Users\" & Environment.UserName & "\AppData\Roaming\.minecraft\assets --assetIndex 1.7.4 --uuid 3dc99ad2c7f218a03141a73a6e9561eb --accessToken 826692e7c85f46fe85511f62d5908e55 --userProperties {} --userType legacy")
    End Sub
End Class
