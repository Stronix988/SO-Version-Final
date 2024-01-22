using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics.Eventing.Reader;


namespace Cliente
{
    public partial class Form1 : Form
    {
        CircularPictureBox ficha2 = new CircularPictureBox(Color.Blue, 15);
        CircularPictureBox ficha1 = new CircularPictureBox(Color.Red, 15);
        ListaPropiedades listaPropiedades = new ListaPropiedades();
        ListaJugadores listaJugadores = new ListaJugadores();
        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("192.168.56.102"), 9058);
        Thread atender;
        string invitados;
        int nInvitados = 0;
        int totalconectados;
        int partida;
        bool anfitrion = false;
        public string jugador;
        public string anfi;
        public int prioridad;
        public string rival;
        public int mov;
        public string compra;
        public int suma;
        public string yo;
        public Form1()
        {
            InitializeComponent();
            CrearTableroMonopoly();
            contextMenuStrip1.Show();
            button1.Visible = false;
            button2.Visible = false;
            button4.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            Invitar.Visible = false;
            listaConectados.Visible = false;
            textBox3.Visible = false;
            eliminar.Visible = false;
            label4.Visible = false;

            tableLayoutPanel1.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button8.Visible = false;

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void atendermensaje()//esta funcion recibe los mensajes del servidor y manda los datos a la funcion necesaria
        {
            while (true)
            {
                byte[] respuesta = new byte[1024];
                socket.Receive(respuesta);
                string mensaje = Encoding.ASCII.GetString(respuesta).Split("\0")[0];
                string[] trozos = mensaje.Split("/");
                int codigo = Convert.ToInt32(trozos[0]);

                switch (codigo)
                {
                    case 0: // recibe la confirmacion del inicio de sesion
                        Invoke(new Action(() =>
                        {
                            string mensaje2 = trozos[1];
                            if (trozos[1] == "Conectado\n")
                            {
                                MessageBox.Show("has iniciado sesion con exito");

                            }
                            else
                            {
                                MessageBox.Show("No has podido iniciar sesion");
                            }
                        }
                        ));
                        break;
                    case 2: //Registrar
                        Invoke(new Action(() =>
                        {
                            string mensaje3 = trozos[1];
                            if (trozos[1] != "Error")
                            {
                                MessageBox.Show("has podido registrate");
                            }
                            else
                            {
                                MessageBox.Show("No se ha  podido registrasrse");
                            }
                        }
                        ));
                        break;
                    case 5://Desconectar
                        break;
                    case 6:
                        Invoke(new Action(() =>
                            {
                                int c = Convert.ToInt32(trozos[1]);
                                string b = Convert.ToString(trozos[2]);



                                int id = 0;
                                listaConectados.DropDownItems.Clear();
                                foreach (String items in dameListaConectados(mensaje, c))
                                {
                                    ToolStripMenuItem item = new ToolStripMenuItem(items);
                                    item.Tag = id;
                                    id++;
                                    listaConectados.DropDownItems.Add(item);
                                    item.Click += new EventHandler(item_Click);
                                }

                            }
                            ));
                        break;
                    case 7:
                        if (trozos[1] != "Error")       //avisa al cliente de que ha sido invitado a jugar y le da la opcion de aceptar o rechazar
                        {
                            string peticion;
                            Convert.ToString(mensaje);
                            string invitador = trozos[2];
                            partida = Convert.ToInt32(trozos[3]);
                            anfi = invitador;
                            if (MessageBox.Show("Has sido invitado a jugar por: " + invitador, "Invitacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                peticion = "8/" + partida + "/Yes" + "/" + textBox1.Text + "/" + invitador;
                                byte[] enviar = System.Text.Encoding.ASCII.GetBytes(peticion);
                                socket.Send(enviar);
                            }
                            else
                            {
                                peticion = "8/" + partida + "No";
                                byte[] enviar = System.Text.Encoding.ASCII.GetBytes(peticion);
                                socket.Send(enviar);
                            }
                        }
                        break;
                    case 9://Recibir mensaje si hay partida
                        Invoke(new Action(() =>
                        {
                            {
                                prioridad = Convert.ToInt32(trozos[2]);
                                MessageBox.Show("Empieza la partida");
                                button5.Visible = true;
                                button6.Visible = true;
                                button7.Visible = true;
                                button8.Visible = true;
                                tableLayoutPanel1.Visible = true;
                                button5.Visible = true;
                                button6.Visible = true;
                                button7.Visible = true;
                                button1.Visible = false;
                                button2.Visible = false;
                                button4.Visible = false;
                                label2.Visible = false;
                                label3.Visible = false;
                                textBox1.Visible = false;
                                textBox2.Visible = false;
                                Invitar.Visible = false;
                                listaConectados.Visible = false;
                                textBox3.Visible = false;
                                eliminar.Visible = false;
                                label4.Visible = false;
                                listaPropiedades.OrdenarPorId();
                                listaJugadores.Añadir(new Jugador(anfi, 1500, 0, 0, 1));
                                listaJugadores.Añadir(new Jugador(jugador, 1500, 0, 0, 2));
                                int x = listaPropiedades.GetPropiedad(0).GetPosX();
                                int y = listaPropiedades.GetPropiedad(0).GetPosY();
                                ficha1.Location = new Point(x, y);
                                this.Controls.Add(ficha1);
                                ficha1.BringToFront();
                                ficha1.Location = listaJugadores.GetJugador(0).MoverFicha(0, listaPropiedades);
                                ficha2.Location = new Point(x, y);
                                this.Controls.Add(ficha2);
                                ficha2.Location = listaJugadores.GetJugador(1).MoverFicha(0, listaPropiedades);
                                ficha2.BringToFront();
                            }

                        }
                        ));
                        break;
                    case 8:
                        MessageBox.Show("HOLA");
                        break;
                    case 10:
                        Invoke(new Action(() =>
                        {
                            if (trozos[1] != "Error")
                            {
                                string peticion;
                                MessageBox.Show(trozos[1]);
                                trozos[2] = invitados;
                                peticion = "9/" + partida + "/" + "Empezar";
                                byte[] enviar = System.Text.Encoding.ASCII.GetBytes(peticion);
                                socket.Send(enviar);

                            }
                        }
                        ));
                        break;
                    case 11:
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show(trozos[1]);
                        }
                        ));
                        break;
                    case 12:
                        Invoke(new Action(() =>
                        {
                            rival = trozos[1];
                            if (rival == textBox1.Text)
                            {
                                MessageBox.Show("Turno del rival");
                            }
                            else
                            {
                                mov = Convert.ToInt32(trozos[2]);
                                if (trozos[3] != "No")
                                {
                                    compra = trozos[4];
                                }
                                prioridad = 1;
                                int x = listaPropiedades.GetPropiedad(mov).GetPosX();
                                int y = listaPropiedades.GetPropiedad(mov).GetPosY();
                                if (rival == anfi)
                                {
                                    ficha1.Location = listaJugadores.GetJugador(0).MoverFicha(mov, listaPropiedades);
                                }
                                else
                                {
                                    ficha2.Location = listaJugadores.GetJugador(1).MoverFicha(mov, listaPropiedades);
                                }

                            }
                            MessageBox.Show("Tu turno");
                        }
                        ));
                        break;
                    case 14:
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show("El rival ha entrado en bancarrota. Has ganado");
                            button5.Visible = false;
                            button6.Visible = false;
                            button7.Visible = false;
                            tableLayoutPanel1.Visible = false;
                            button5.Visible = false;
                            button6.Visible = false;
                            button7.Visible = false;
                            button1.Visible = true;
                            button2.Visible = true;
                            button4.Visible = true;
                            label2.Visible = true;
                            label3.Visible = true;
                            textBox1.Visible = true;
                            textBox2.Visible = true;
                            Invitar.Visible = true;
                            listaConectados.Visible = true;
                            textBox3.Visible = true;
                            eliminar.Visible = true;
                            label4.Visible = true;
                            button8.Visible = false;
                            ficha2.Visible = false;
                            ficha1.Visible = false;
                        }
                        ));
                        break;
                    case 15:
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show("El rival ha abandonado la partida");
                            button5.Visible = false;
                            button6.Visible = false;
                            button7.Visible = false;
                            tableLayoutPanel1.Visible = false;
                            button5.Visible = false;
                            button6.Visible = false;
                            button7.Visible = false;
                            button1.Visible = true;
                            button2.Visible = true;
                            button4.Visible = true;
                            label2.Visible = true;
                            label3.Visible = true;
                            textBox1.Visible = true;
                            textBox2.Visible = true;
                            Invitar.Visible = true;
                            listaConectados.Visible = true;
                            textBox3.Visible = true;
                            eliminar.Visible = true;
                            label4.Visible = true;
                            button8.Visible = false;
                            ficha2.Visible = false;
                            ficha1.Visible = false;
                        }
));
                        break;
                }

            }

        }



        private void CrearTableroMonopoly()
        {

            int numFilas = 11;
            int numColumnas = 11;
            int tamañoCasilla = 70; // Tamaño de cada casilla en píxeles

            tableLayoutPanel1.RowCount = numFilas;
            tableLayoutPanel1.ColumnCount = numColumnas;
            tableLayoutPanel1.AutoSize = true;

            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.ColumnStyles.Clear();

            for (int i = 0; i < numFilas; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, tamañoCasilla));
            }

            for (int i = 0; i < numColumnas; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, tamañoCasilla));
            }

            // Matrices con los números específicos y nombres para cada casilla del tablero
            int[,] precioCasillas = new int[11, 11]
{
             { 0,  60,  60,  0,  70,  0,  100,  0,  100, 120, 0},
             {400,  0,  0,  0,  0,  0,  0,  0,  0,  0, 140},
             {0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 140},
             {350,  0,  0,  0,  0,  0,  0,  0,  0,  0 ,0},
             {340,  0,  0,  0,  0,  0,  0,  0,  0,  0, 160},
             {0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 0},
             {320,  0,  0,  0,  0,  0,  0,  0,  0,  0, 170},
             {0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 180},
             {300,  0,  0,  0,  0,  0,  0,  0,  0,  0, 0},
             {300,  0,  0,  0,  0,  0,  0,  0,  0,  0, 200},
             {0, 280, 0, 260, 260, 0, 240, 0, 220, 220, 0}
};

            int[,] alquilerCasillas = new int[11, 11]
{
             { 0,  6,  6,  0,  7,  0,  10,  0,  10, 12, 0},
             {40,  0,  0,  0,  0,  0,  0,  0,  0,  0, 14},
             {0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 14},
             {35,  0,  0,  0,  0,  0,  0,  0,  0,  0 ,0},
             {34,  0,  0,  0,  0,  0,  0,  0,  0,  0, 16},
             {0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 0},
             {32,  0,  0,  0,  0,  0,  0,  0,  0,  0, 17},
             {0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 18},
             {30,  0,  0,  0,  0,  0,  0,  0,  0,  0, 0},
             {30,  0,  0,  0,  0,  0,  0,  0,  0,  0, 20},
             {0, 28, 0, 26, 26, 0, 24, 0, 22, 22, 0}
};

            int[,] numerosCasillas = new int[11, 11]
{
             { 1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11},
             {40,  0,  0,  0,  0,  0,  0,  0,  0,  0, 12},
             {39,  0,  0,  0,  0,  0,  0,  0,  0,  0, 13},
             {38,  0,  0,  0,  0,  0,  0,  0,  0,  0, 14},
             {37,  0,  0,  0,  0,  0,  0,  0,  0,  0, 15},
             {36,  0,  0,  0,  0,  0,  0,  0,  0,  0, 16},
             {35,  0,  0,  0,  0,  0,  0,  0,  0,  0, 17},
             {34,  0,  0,  0,  0,  0,  0,  0,  0,  0, 18},
             {33,  0,  0,  0,  0,  0,  0,  0,  0,  0, 19},
             {32,  0,  0,  0,  0,  0,  0,  0,  0,  0, 20},
             {31, 30, 29, 28, 27, 26, 25, 24, 23, 22, 21}
};
            string[,] nombresCasillas = new string[11, 11]
            {
             {"Go", "Taller 228B", "Taller 229B", "Carta", "Taller 231B", "Lab Circuitos", "Aula 336B", "Sorpresa", "Aula 337B", "Aula 339B", "Casa"},
             {"Aula 703B", "", "", "", "", "", "", "", "", "", "Taller 348V"},
             {"Carta", "", "", "", "", "", "", "", "", "", "Taller 349V"},
             {"Aula 702B", "", "", "", "", "", "", "", "", "", "Sorpresa"},
             {"Aula 701B", "", "", "", "", "", "", "", "", "", "Taller 351V"},
             {"Lab electronica", "", "", "", "", "", "", "", "", "", "Lab Telematica"},
             {"Aula 132V", "", "", "", "", "", "", "", "", "", "Aula 401B"},
             {"Sorpresa", "", "", "", "", "", "", "", "", "", "Aula 402B"},
             {"Aula 133V", "", "", "", "", "", "", "", "", "", "Carta"},
             {"Aula 134V", "", "", "", "", "", "", "", "", "", "Aula 405B"},
             {"Casa", "Aula 603V", "Carta", "Aula 602V", "Aula 601V", "Lab Drones", "Taller 505G", "Sorpresa", "Taller 503G", "Taller 502G", "Parking"}
 };


            for (int fila = 0; fila < numFilas; fila++)
            {
                for (int columna = 0; columna < numColumnas; columna++)
                {
                    if (fila == 0 || fila == numFilas - 1 || columna == 0 || columna == numColumnas - 1)
                    {
                        Panel panelCasilla = new Panel();

                        if (numerosCasillas[fila, columna] == 2 || numerosCasillas[fila, columna] == 3 || numerosCasillas[fila, columna] == 5)
                        {
                            panelCasilla.BackColor = Color.LightSalmon;
                        }
                        else if (numerosCasillas[fila, columna] == 7 || numerosCasillas[fila, columna] == 9 || numerosCasillas[fila, columna] == 10)
                        {
                            panelCasilla.BackColor = Color.LightGreen;
                        }
                        else if (numerosCasillas[fila, columna] == 12 || numerosCasillas[fila, columna] == 13 || numerosCasillas[fila, columna] == 15)
                        {
                            panelCasilla.BackColor = Color.LightSkyBlue;
                        }
                        else if (numerosCasillas[fila, columna] == 17 || numerosCasillas[fila, columna] == 18 || numerosCasillas[fila, columna] == 20)
                        {
                            panelCasilla.BackColor = Color.MistyRose;
                        }
                        else if (numerosCasillas[fila, columna] == 22 || numerosCasillas[fila, columna] == 23 || numerosCasillas[fila, columna] == 25)
                        {
                            panelCasilla.BackColor = Color.Olive;
                        }
                        else if (numerosCasillas[fila, columna] == 27 || numerosCasillas[fila, columna] == 28 || numerosCasillas[fila, columna] == 30)
                        {
                            panelCasilla.BackColor = Color.PaleVioletRed;
                        }
                        else if (numerosCasillas[fila, columna] == 32 || numerosCasillas[fila, columna] == 33 || numerosCasillas[fila, columna] == 35)
                        {
                            panelCasilla.BackColor = Color.CornflowerBlue;
                        }
                        else if (numerosCasillas[fila, columna] == 37 || numerosCasillas[fila, columna] == 38 || numerosCasillas[fila, columna] == 40)
                        {
                            panelCasilla.BackColor = Color.YellowGreen;
                        }
                        else if (numerosCasillas[fila, columna] == 6 || numerosCasillas[fila, columna] == 16 || numerosCasillas[fila, columna] == 26 || numerosCasillas[fila, columna] == 36)
                        {
                            panelCasilla.BackColor = Color.LightGray;
                        }
                        else if (numerosCasillas[fila, columna] == 1 || numerosCasillas[fila, columna] == 11 || numerosCasillas[fila, columna] == 21 || numerosCasillas[fila, columna] == 31)
                        {
                            panelCasilla.BackColor = Color.LightSteelBlue;
                        }
                        else if (numerosCasillas[fila, columna] == 8 || numerosCasillas[fila, columna] == 14 || numerosCasillas[fila, columna] == 24 || numerosCasillas[fila, columna] == 34)
                        {
                            panelCasilla.BackColor = Color.LightYellow;
                        }
                        else if (numerosCasillas[fila, columna] == 4 || numerosCasillas[fila, columna] == 19 || numerosCasillas[fila, columna] == 29 || numerosCasillas[fila, columna] == 39)
                        {
                            panelCasilla.BackColor = Color.LightYellow;
                        }

                        panelCasilla.Dock = DockStyle.Fill;

                        Label labelNumero = new Label();
                        labelNumero.Text = numerosCasillas[fila, columna].ToString();
                        labelNumero.AutoSize = true;
                        labelNumero.Dock = DockStyle.Top;
                        labelNumero.TextAlign = ContentAlignment.MiddleCenter;

                        Label labelNombre = new Label();
                        labelNombre.Text = nombresCasillas[fila, columna];
                        labelNombre.AutoSize = true;
                        labelNombre.Dock = DockStyle.Bottom;
                        labelNombre.TextAlign = ContentAlignment.MiddleCenter;

                        panelCasilla.Controls.Add(labelNumero);
                        panelCasilla.Controls.Add(labelNombre);


                        tableLayoutPanel1.Controls.Add(panelCasilla, columna, fila);


                        if (numerosCasillas[fila, columna] < 41)
                        {
                            Point posicion = tableLayoutPanel1.PointToScreen(panelCasilla.Location);
                            int x = posicion.X;
                            int y = posicion.Y;
                            listaPropiedades.Añadir(new Propiedad(nombresCasillas[fila, columna], numerosCasillas[fila, columna], x, y, precioCasillas[fila, columna], alquilerCasillas[fila, columna], null));

                        }
                    }

                    else
                    {
                        tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.GetControlFromPosition(columna, fila));
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e) // inicia sesion 
        {

            try
            {

                string usuario = textBox1.Text;
                jugador = usuario;
                string contraseña = textBox2.Text;
                string mensaje = ("0/" + usuario + "/" + contraseña); // envia al servidor el inicio de sesion 
                socket.Send(Encoding.ASCII.GetBytes(mensaje));


            }
            catch (SocketException)
            {
                MessageBox.Show("No has podido iniciar sesion");
            }
        }

        private void button2_Click(object sender, EventArgs e) //Cerrar sesion
        {
            try
            {
                string usuario = textBox1.Text;
                string sesion = ("5/" + usuario); //cierra sesion del usuario
                MessageBox.Show("Desconectado del servidor");
                socket.Send(Encoding.ASCII.GetBytes(sesion));
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (SocketException)
            {

            }

        }

        private void timer1_Tick(object sender, EventArgs e) //pide la lista de conectados cada 3 segundos
        {
            string mensaje = "6/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            socket.Send(msg);
        }

        private void comoFuncionaElProgramaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("El programa se conecta al servidor directamente cuando lo abres(si no va dele al boton conectarse)  y para iniciar sesion simplemente tienes que rellenar los textbox");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                socket.Connect(remoteEP);
                MessageBox.Show("Conectado al servidor");
                button1.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
                button4.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                Invitar.Visible = true;
                listaConectados.Visible = true;
                button3.Visible = false;
                eliminar.Visible = true;
                textBox3.Visible = true;
                label4.Visible = true;
                ThreadStart ts = delegate { atendermensaje(); };
                atender = new Thread(ts);
                atender.Start();
            }
            catch (Exception)
            {

            }

        }
        public void item_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            MessageBox.Show("Se enviara una invitacion a " + item.Text);
            listaInvitacion(item.Text);
        }
        private void button4_Click(object sender, EventArgs e) //registrarse
        {

            try
            {
                string usuario = textBox1.Text;
                string contraseña = textBox2.Text;
                string mensaje = ("4/" + usuario + "/" + contraseña); // envia al servidor el registro 
                socket.Send(Encoding.ASCII.GetBytes(mensaje));
            }
            catch (SocketException)
            {
                MessageBox.Show("No has podido crear un usuario");

            }
        }

        private void Invitar_Click(object sender, EventArgs e)//Invitar a una persona
        {
            try
            {
                string invite = ("7/" + textBox1.Text + "/" + nInvitados + "/" + invitados);
                byte[] mensaje = System.Text.Encoding.ASCII.GetBytes(invite);
                socket.Send(mensaje);
                anfitrion = true;
            }
            catch (SocketException)
            {
                MessageBox.Show("Fijate si has puesto bien al invitado");
            }
        }
        private void listaInvitacion(string nombre)
        {
            if (nInvitados < 4)
            {
                invitados = invitados + "/" + nombre;
                nInvitados++;
            }
            else
            {
                MessageBox.Show("Solo se puede invitar un maximo de 4 jugadores");
            }
        }
        private List<String> dameListaConectados(string lista, int nConectados)
        {
            {
                List<String> conectados = new List<String>();
                string[] lconectados = lista.Split('/');

                int numeroConectados = Convert.ToInt32(lconectados[1]);
                int i = 0;
                while (i < nConectados)
                {

                    string nombre = lconectados[i + 2];
                    string[] Nnombre;
                    Nnombre = nombre.Split('6');
                    conectados.Add(Nnombre[0]);
                    i++;
                }

                return conectados;

            }
        }

        private void eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = textBox3.Text;
                string mensaje = ("11/" + usuario);
                socket.Send(Encoding.ASCII.GetBytes(mensaje));
            }
            catch (Exception ex)
            {

            }
        }
        public void Movimientos()//envia movimiento de cada jugador
        {
            if (compra != null)
            {
                string peticion = "13/" + textBox1.Text + "/" + suma + "/" + "Yes" + "/" + compra;
                socket.Send(Encoding.ASCII.GetBytes(peticion));
            }
            else
            {
                string peticion = "13/" + textBox1.Text + "/" + suma + "/" + "No";
                socket.Send(Encoding.ASCII.GetBytes(peticion));
            }
            prioridad = 0;
        }
        public void Bancarrota()//envia que hay una bancarrota
        {
            string peticion = "14/" + textBox1.Text;
            socket.Send(Encoding.ASCII.GetBytes(peticion));
        }
        public void AcabarPartido()//envia que alguien ha dejado la partida
        {
            string peticion = "15/" + textBox1.Text;
            socket.Send(Encoding.ASCII.GetBytes(peticion));
        }
        public class Propiedad
        {
            int Id;
            string Propietario;
            string Nombre;
            int Precio;
            int Alquiler;
            int PosX;
            int PosY;
            public string GetPropietario()
            {
                return this.Propietario;
            }

            public void SetPropietario(string propietario)
            {
                this.Propietario = propietario;
            }

            public int GetId()
            {
                return this.Id;
            }

            public void SetId(int id)
            {
                this.Id = id;
            }

            public string GetNombre()
            {
                return this.Nombre;
            }

            public void SetNombre(string nombre)
            {
                this.Nombre = nombre;
            }

            public int GetPrecio()
            {
                return this.Precio;
            }

            public void SetPrecio(int precio)
            {
                this.Precio = precio;
            }

            public int GetAlquiler()
            {
                return this.Alquiler;
            }

            public void SetAlquiler(int alquiler)
            {
                this.Alquiler = alquiler;
            }

            public int GetPosX()
            {
                return this.PosX;
            }

            public void SetPosX(int posX)
            {
                this.PosX = posX;
            }

            public int GetPosY()
            {
                return this.PosY;
            }

            public void SetPosY(int posY)
            {
                this.PosY = posY;
            }

            public Propiedad(string nombre, int id, int posX, int posY)
            {
                this.Nombre = nombre;
                this.Id = id;
                this.PosX = posX;
                this.PosY = posY;


            }
            public Propiedad(string nombre, int id, int posX, int posY, int precio, int alquiler, string propietario)
            {
                this.Nombre = nombre;
                this.Id = id;
                this.PosX = posX;
                this.PosY = posY;
                this.Precio = precio;
                this.Alquiler = alquiler;
                this.Propietario = propietario;
            }


        }




        public class Jugador
        {

            string Nombre;
            int Dinero;
            int PosX;
            int PosY;
            int Casillactual;
            int Numeroficha;
            bool Turno;
            bool Bancarrota;


            public string GetNombre()
            {
                return this.Nombre;
            }

            public void SetNombre(string nombre)
            {
                this.Nombre = nombre;
            }

            public int GetDinero()
            {
                return this.Dinero;
            }

            public void SetDinero(int dinero)
            {
                this.Dinero = dinero;
            }
            public int GetPosX()
            {
                return this.PosX;
            }

            public void SetPosX(int posX)
            {
                this.PosX = posX;
            }

            public int GetPosY()
            {
                return this.PosY;
            }

            public void SetPosY(int posY)
            {
                this.PosY = posY;
            }

            public int GetCasillactual()
            {
                return this.Casillactual;
            }

            public void SetCasillactual(int idcasilla)
            {
                this.Casillactual = idcasilla;
            }

            public int GetNumeroFicha()
            {
                return this.Numeroficha;
            }

            public void SetNumeroFicha(int numeroficha)
            {
                this.Numeroficha = numeroficha;
            }
            public bool GetTurno()
            {
                return this.Turno;
            }
            public void SetTurno(bool turno)
            {
                this.Turno = turno;
            }
            public bool GetBancarrota()
            {
                return this.Bancarrota;
            }
            public void SetBancarrota(bool bancarrota)
            {
                this.Bancarrota = bancarrota;
            }

            public void DecidirSiBancarrota()
            {
                if (this.Dinero < 0)
                {
                    this.Bancarrota = true;
                }
                else
                {
                    this.Bancarrota = false;
                }
            }

            public Point MoverFicha(int n, ListaPropiedades milistapropiedades) //n es el numero que te haya dado el dado que viene del server
            {
                Point punto = new Point();
                this.Casillactual = (Casillactual + n) % 40;
                this.PosX = milistapropiedades.GetPropiedad(Casillactual).GetPosX();
                this.PosY = milistapropiedades.GetPropiedad(Casillactual).GetPosY();
                punto.X = this.PosX;
                punto.Y = this.PosY;
                return punto;
            }

            public Jugador(string nombre, int dineroInicial, int posx, int posy, int numeroficha)
            {
                this.Nombre = nombre;
                this.Dinero = dineroInicial;
                this.PosX = posx;
                this.PosY = posy;
                this.Numeroficha = numeroficha;

            }



        }

        public class ListaJugadores
        {
            int numero;
            Jugador[] listajugadores = new Jugador[4];

            public int Numero()
            {
                return this.numero;
            }
            public Jugador GetJugador(int i)
            {
                if (i < 0 || i >= this.numero)
                    return null;

                return this.listajugadores[i];
            }
            public int Añadir(Jugador jugador)
            {
                if (this.numero == 4)
                {
                    return -1;
                }
                else
                {
                    this.listajugadores[this.numero] = jugador;
                    this.numero++;
                    return 0;
                }
            }
        }







        public class ListaPropiedades
        {
            int numero = 0;
            Propiedad[] listapropiedades = new Propiedad[40];
            public void OrdenarPorId()
            {
                // Filtrar las propiedades no nulas y ordenar por Id
                var propiedadesOrdenadas = listapropiedades
                    .Where(propiedad => propiedad != null)
                    .OrderBy(propiedad => propiedad.GetId())
                    .ToArray();

                // Actualizar la lista original con las propiedades ordenadas
                Array.Copy(propiedadesOrdenadas, listapropiedades, propiedadesOrdenadas.Length);
            }


            public int Numero()
            {
                return this.numero;
            }

            public Propiedad GetPropiedad(int i)
            {
                if (i < 0 || i >= this.numero)
                    return null;

                return this.listapropiedades[i];
            }

            public int Añadir(Propiedad propiedad)
            {
                if (this.numero == 40)
                {
                    return -1;
                }
                else
                {
                    this.listapropiedades[this.numero] = propiedad;
                    this.numero++;
                    return 0;
                }
            }
        }

        public class CircularPictureBox : PictureBox//crear ficha
        {
            private Color fillColor;

            public CircularPictureBox(Color color, int size)
            {
                this.fillColor = color;
                this.BackColor = Color.Transparent;
                this.Size = new Size(size, size);
                this.Paint += CircularPictureBox_Paint;
            }

            private void CircularPictureBox_Paint(object sender, PaintEventArgs e)
            {
                using (SolidBrush brush = new SolidBrush(fillColor))
                {
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    e.Graphics.FillEllipse(brush, 0, 0, this.Width - 1, this.Height - 1);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e) //dado
        {
            Random rnd = new Random();
            int dado1 = rnd.Next(1, 6);
            int dado2 = rnd.Next(1, 6);
            suma = dado1 + dado2;
            if (dado1 == dado2)
            {
                MessageBox.Show("Has sacado dobles");
            }
            else
            {

            }
            MessageBox.Show("Has sacado un " + dado1 + " y " + dado2);
            if (prioridad == 1)
            {
                if (yo == anfi)
                {
                    ficha1.Location = listaJugadores.GetJugador(0).MoverFicha(suma, listaPropiedades);
                    ficha1.BringToFront();
                    int casilla = listaJugadores.GetJugador(0).GetCasillactual();
                    if (listaPropiedades.GetPropiedad(casilla).GetPropietario() != null)
                    {
                        if (listaPropiedades.GetPropiedad(casilla).GetPropietario() != listaJugadores.GetJugador(0).GetNombre())
                        {
                            MessageBox.Show("Esta casilla es de un jugador toca pagar el alquiler");
                            int b = listaJugadores.GetJugador(0).GetDinero();
                            if (b >= listaPropiedades.GetPropiedad(casilla).GetAlquiler())
                            {
                                listaJugadores.GetJugador(0).SetDinero(b - listaPropiedades.GetPropiedad(casilla).GetAlquiler());
                            }
                            else
                            {
                                MessageBox.Show("Estas en bancarrota fin de la partida para ti");
                                listaJugadores.GetJugador(0).SetBancarrota(true);
                                Bancarrota();
                                button5.Visible = false;
                                button6.Visible = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Esta casilla es tuya");
                        }
                    }
                    else if (listaPropiedades.GetPropiedad(casilla).GetNombre() == "Carta")
                    {
                        int random1 = rnd.Next(0, 10);
                        int valor = rnd.Next(1, 4);
                        int resultado = suma * valor;
                        int b = listaJugadores.GetJugador(0).GetDinero();
                        if (random1 < 4)
                        {
                            MessageBox.Show("Has ganado " + resultado + " creditos");
                            listaJugadores.GetJugador(0).SetDinero(b + resultado);
                        }
                        else
                        {
                            MessageBox.Show("Has perdido " + resultado + " creditos");
                            listaJugadores.GetJugador(0).SetDinero(b - resultado);
                        }
                    }

                    else if (listaPropiedades.GetPropiedad(casilla).GetNombre() == "Sorpresa")
                    {
                        int random1 = rnd.Next(0, 10);
                        int valor = rnd.Next(1, 3);
                        int resultado = suma * valor;
                        int b = listaJugadores.GetJugador(0).GetDinero();
                        if (random1 < 6)
                        {
                            MessageBox.Show("Has ganado " + resultado + " creditos");
                            listaJugadores.GetJugador(0).SetDinero(b + resultado);
                        }
                        else
                        {
                            MessageBox.Show("Has perdido " + resultado + " creditos");
                            listaJugadores.GetJugador(0).SetDinero(b - resultado);
                        }
                    }
                }
                else
                {
                    ficha2.Location = listaJugadores.GetJugador(1).MoverFicha(suma, listaPropiedades);
                    ficha2.BringToFront();
                    int casilla = listaJugadores.GetJugador(1).GetCasillactual();
                    if (listaPropiedades.GetPropiedad(casilla).GetPropietario() != null)
                    {
                        if (listaPropiedades.GetPropiedad(casilla).GetPropietario() != listaJugadores.GetJugador(1).GetNombre())
                        {
                            MessageBox.Show("Esta casilla es de un jugador toca pagar el alquiler");
                            int b = listaJugadores.GetJugador(1).GetDinero();
                            if (b >= listaPropiedades.GetPropiedad(casilla).GetAlquiler())
                            {
                                listaJugadores.GetJugador(1).SetDinero(b - listaPropiedades.GetPropiedad(casilla).GetAlquiler());
                            }
                            else
                            {
                                MessageBox.Show("Estas en bancarrota fin de la partida para ti");
                                listaJugadores.GetJugador(1).SetBancarrota(true);
                                Bancarrota();
                                button5.Visible = false;
                                button6.Visible = false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Esta casilla es tuya");
                        }
                    }
                    else if (listaPropiedades.GetPropiedad(casilla).GetNombre() == "Carta")
                    {
                        int random1 = rnd.Next(0, 10);
                        int valor = rnd.Next(1, 4);
                        int resultado = suma * valor;
                        int b = listaJugadores.GetJugador(1).GetDinero();
                        if (random1 < 4)
                        {
                            MessageBox.Show("Has ganado " + resultado + " creditos");
                            listaJugadores.GetJugador(1).SetDinero(b + resultado);
                        }
                        else
                        {
                            MessageBox.Show("Has perdido " + resultado + " creditos");
                            listaJugadores.GetJugador(1).SetDinero(b - resultado);
                        }
                    }

                    else if (listaPropiedades.GetPropiedad(casilla).GetNombre() == "Sorpresa")
                    {
                        int random1 = rnd.Next(0, 10);
                        int valor = rnd.Next(1, 3);
                        int resultado = suma * valor;
                        int b = listaJugadores.GetJugador(0).GetDinero();
                        if (random1 < 6)
                        {
                            MessageBox.Show("Has ganado " + resultado + " creditos");
                            listaJugadores.GetJugador(1).SetDinero(b + resultado);
                        }
                        else
                        {
                            MessageBox.Show("Has perdido " + resultado + " creditos");
                            listaJugadores.GetJugador(1).SetDinero(b - resultado);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No es tu turno todavia");
            }

        }


        private void button6_Click(object sender, EventArgs e) //comprar
        {
            if (prioridad == 1)
            {
                if (yo == anfi)
                {
                    {
                        int a = listaJugadores.GetJugador(0).GetCasillactual();
                        if (listaPropiedades.GetPropiedad(a).GetPropietario() == null && listaPropiedades.GetPropiedad(a).GetPrecio() != 0)
                        {
                            int b = listaJugadores.GetJugador(0).GetDinero();
                            if (b >= listaPropiedades.GetPropiedad(a).GetPrecio())
                            {
                                listaJugadores.GetJugador(0).SetDinero(b - listaPropiedades.GetPropiedad(a).GetPrecio());
                                listaPropiedades.GetPropiedad(a).SetPropietario(listaJugadores.GetJugador(0).GetNombre());
                                MessageBox.Show("Has comprado la casilla");
                                compra = Convert.ToString(listaPropiedades.GetPropiedad(a).GetNombre());
                            }
                            else
                            {
                                MessageBox.Show("no tienes suficiente dinero");
                            }
                        }
                        else
                        {
                            MessageBox.Show("la casilla ya ha sido comprada o no se puede comprar");

                        }
                    }
                }
                else
                {
                    int a = listaJugadores.GetJugador(1).GetCasillactual();
                    if (listaPropiedades.GetPropiedad(a).GetPropietario() == null && listaPropiedades.GetPropiedad(a).GetPrecio() != 0)
                    {
                        int b = listaJugadores.GetJugador(1).GetDinero();
                        if (b >= listaPropiedades.GetPropiedad(a).GetPrecio())
                        {
                            listaJugadores.GetJugador(1).SetDinero(b - listaPropiedades.GetPropiedad(a).GetPrecio());
                            listaPropiedades.GetPropiedad(a).SetPropietario(listaJugadores.GetJugador(1).GetNombre());
                            MessageBox.Show("Has comprado la casilla");
                            compra = Convert.ToString(listaPropiedades.GetPropiedad(a).GetNombre());
                        }
                        else
                        {
                            MessageBox.Show("no tienes suficiente dinero");
                        }
                    }
                    else
                    {
                        MessageBox.Show("la casilla ya ha sido comprada o no se puede comprar");

                    }
                }
            }
            else
            {
                MessageBox.Show("No es tu turno, por favor, espera");
            }
        }

        private void button7_Click(object sender, EventArgs e) //acabar turno
        {

            if (prioridad == 1)
            {
                Movimientos();
            }
            else
            {
                MessageBox.Show("Todavia no es tu turno");
            }
            button3.Visible = false;

        }

        private void button8_Click(object sender, EventArgs e)
        {
            AcabarPartido();
            MessageBox.Show("Has abandonado la partida");
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            ficha1.Visible = false;
            ficha2.Visible = false;
            tableLayoutPanel1.Visible = false;
            button8.Visible = false;
            button5.Visible = false;
            button6.Visible = false;
            button7.Visible = false;
            button1.Visible = true;
            button2.Visible = true;
            button4.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible = true;
            Invitar.Visible = true;
            listaConectados.Visible = true;
            textBox3.Visible = true;
            eliminar.Visible = true;
            label4.Visible = true;

        }
    }
}
