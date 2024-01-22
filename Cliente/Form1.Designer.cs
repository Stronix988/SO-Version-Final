namespace Cliente
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label2 = new Label();
            label3 = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            comoFuncionaElProgramaToolStripMenuItem = new ToolStripMenuItem();
            button3 = new Button();
            button4 = new Button();
            Invitar = new Button();
            menuStrip1 = new MenuStrip();
            listaConectados = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            label4 = new Label();
            textBox3 = new TextBox();
            eliminar = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            contextMenuStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(367, 125);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(142, 31);
            button1.TabIndex = 0;
            button1.Text = "Iniciar sesion";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(367, 164);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(142, 31);
            button2.TabIndex = 1;
            button2.Text = "Desconectarse";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(453, 76);
            label1.Name = "label1";
            label1.Size = new Size(0, 20);
            label1.TabIndex = 2;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(97, 125);
            textBox1.Margin = new Padding(3, 4, 3, 4);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(114, 27);
            textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(218, 125);
            textBox2.Margin = new Padding(3, 4, 3, 4);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(114, 27);
            textBox2.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(97, 89);
            label2.Name = "label2";
            label2.Size = new Size(59, 20);
            label2.TabIndex = 5;
            label2.Text = "Usuario";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(218, 89);
            label3.Name = "label3";
            label3.Size = new Size(83, 20);
            label3.TabIndex = 6;
            label3.Text = "Contraseña";
            // 
            // timer1
            // 
            timer1.Interval = 3000;
            timer1.Tick += timer1_Tick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(20, 20);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { comoFuncionaElProgramaToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(266, 28);
            contextMenuStrip1.Text = "Menu";
            // 
            // comoFuncionaElProgramaToolStripMenuItem
            // 
            comoFuncionaElProgramaToolStripMenuItem.Name = "comoFuncionaElProgramaToolStripMenuItem";
            comoFuncionaElProgramaToolStripMenuItem.Size = new Size(265, 24);
            comoFuncionaElProgramaToolStripMenuItem.Text = "Como funciona el programa";
            comoFuncionaElProgramaToolStripMenuItem.Click += comoFuncionaElProgramaToolStripMenuItem_Click;
            // 
            // button3
            // 
            button3.Location = new Point(367, 84);
            button3.Margin = new Padding(3, 4, 3, 4);
            button3.Name = "button3";
            button3.Size = new Size(142, 31);
            button3.TabIndex = 8;
            button3.Text = "Conectarse";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(367, 203);
            button4.Margin = new Padding(3, 4, 3, 4);
            button4.Name = "button4";
            button4.Size = new Size(142, 31);
            button4.TabIndex = 9;
            button4.Text = "registrarse";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // Invitar
            // 
            Invitar.Location = new Point(367, 241);
            Invitar.Name = "Invitar";
            Invitar.Size = new Size(142, 37);
            Invitar.TabIndex = 11;
            Invitar.Text = "Invitar";
            Invitar.UseVisualStyleBackColor = true;
            Invitar.Click += Invitar_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { listaConectados });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(6, 3, 0, 3);
            menuStrip1.Size = new Size(914, 30);
            menuStrip1.TabIndex = 12;
            menuStrip1.Text = "menuStrip1";
            // 
            // listaConectados
            // 
            listaConectados.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4 });
            listaConectados.Name = "listaConectados";
            listaConectados.Size = new Size(137, 24);
            listaConectados.Text = "Gente Conectada";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(83, 26);
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(83, 26);
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(83, 26);
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(83, 26);
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(591, 95);
            label4.Name = "label4";
            label4.Size = new Size(186, 20);
            label4.TabIndex = 13;
            label4.Text = "Nombre usuario a eliminar";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(625, 127);
            textBox3.Margin = new Padding(3, 4, 3, 4);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(114, 27);
            textBox3.TabIndex = 14;
            // 
            // eliminar
            // 
            eliminar.Location = new Point(367, 285);
            eliminar.Margin = new Padding(3, 4, 3, 4);
            eliminar.Name = "eliminar";
            eliminar.Size = new Size(142, 31);
            eliminar.TabIndex = 15;
            eliminar.Text = "Eliminar Usuario";
            eliminar.UseVisualStyleBackColor = true;
            eliminar.Click += eliminar_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Location = new Point(171, 200);
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(229, 133);
            tableLayoutPanel1.TabIndex = 16;
            // 
            // button5
            // 
            button5.Location = new Point(367, 434);
            button5.Margin = new Padding(3, 4, 3, 4);
            button5.Name = "button5";
            button5.Size = new Size(86, 31);
            button5.TabIndex = 17;
            button5.Text = "Dado";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new Point(500, 434);
            button6.Margin = new Padding(3, 4, 3, 4);
            button6.Name = "button6";
            button6.Size = new Size(86, 31);
            button6.TabIndex = 18;
            button6.Text = "Comprar";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new Point(500, 499);
            button7.Margin = new Padding(3, 4, 3, 4);
            button7.Name = "button7";
            button7.Size = new Size(86, 56);
            button7.TabIndex = 19;
            button7.Text = "Acabar Turno";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Location = new Point(354, 494);
            button8.Name = "button8";
            button8.Size = new Size(113, 61);
            button8.TabIndex = 20;
            button8.Text = "Acabar partida";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(eliminar);
            Controls.Add(textBox3);
            Controls.Add(label4);
            Controls.Add(menuStrip1);
            Controls.Add(Invitar);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Form1";
            contextMenuStrip1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Label label1;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label2;
        private Label label3;
        private System.Windows.Forms.Timer timer1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem comoFuncionaElProgramaToolStripMenuItem;
        private Button button3;
        private Button button4;
        private Button Invitar;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem listaConectados;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private Label label4;
        private TextBox textBox3;
        private Button eliminar;
        private TableLayoutPanel tableLayoutPanel1;
        private Button button5;
        private Button button6;
        private Button button7;
        private Button button8;
    }
}