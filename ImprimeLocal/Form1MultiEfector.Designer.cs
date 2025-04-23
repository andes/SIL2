namespace ImprimeLocal
{
    partial class Form1MultiEfector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1MultiEfector));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.comboBoximp = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnImprimir = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.txtDesde = new System.Windows.Forms.TextBox();
            this.txtHasta = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Protocolos = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.nupLaboratorio = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.nupForense = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nupNoPaciente = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nupMicrobiologia = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label16 = new System.Windows.Forms.Label();
            this.txtDesdeEfector = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnImprimirEfector = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.ddlEfector = new System.Windows.Forms.ComboBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.chkImpresora = new System.Windows.Forms.CheckBox();
            this.chkModoAutomatico = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblIdEfector = new System.Windows.Forms.Label();
            this.txtmensajes = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.Protocolos.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupLaboratorio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupForense)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNoPaciente)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupMicrobiologia)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // comboBoximp
            // 
            this.comboBoximp.FormattingEnabled = true;
            this.comboBoximp.Location = new System.Drawing.Point(120, 27);
            this.comboBoximp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoximp.Name = "comboBoximp";
            this.comboBoximp.Size = new System.Drawing.Size(298, 28);
            this.comboBoximp.TabIndex = 10;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(36, 129);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(388, 71);
            this.textBox1.TabIndex = 0;
            // 
            // btnImprimir
            // 
            this.btnImprimir.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimir.Location = new System.Drawing.Point(36, 243);
            this.btnImprimir.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(388, 60);
            this.btnImprimir.TabIndex = 2;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Impresora:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(40, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(406, 33);
            this.label2.TabIndex = 11;
            this.label2.Text = "NUMERO DE PROTOCOLO";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Maroon;
            this.button2.Location = new System.Drawing.Point(36, 302);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(388, 60);
            this.button2.TabIndex = 14;
            this.button2.Text = "Imprimir";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtDesde
            // 
            this.txtDesde.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDesde.Location = new System.Drawing.Point(36, 69);
            this.txtDesde.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDesde.Name = "txtDesde";
            this.txtDesde.Size = new System.Drawing.Size(253, 71);
            this.txtDesde.TabIndex = 15;
            // 
            // txtHasta
            // 
            this.txtHasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHasta.Location = new System.Drawing.Point(36, 197);
            this.txtHasta.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtHasta.Name = "txtHasta";
            this.txtHasta.Size = new System.Drawing.Size(253, 71);
            this.txtHasta.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(28, 151);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 38);
            this.label4.TabIndex = 18;
            this.label4.Text = "Hasta";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(28, 5);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 42);
            this.label5.TabIndex = 20;
            this.label5.Text = "Desde";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Protocolos);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(481, 148);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(462, 426);
            this.tabControl1.TabIndex = 21;
            this.tabControl1.Visible = false;
            // 
            // Protocolos
            // 
            this.Protocolos.Controls.Add(this.label2);
            this.Protocolos.Controls.Add(this.textBox1);
            this.Protocolos.Controls.Add(this.btnImprimir);
            this.Protocolos.Location = new System.Drawing.Point(4, 29);
            this.Protocolos.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Protocolos.Name = "Protocolos";
            this.Protocolos.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Protocolos.Size = new System.Drawing.Size(454, 393);
            this.Protocolos.TabIndex = 0;
            this.Protocolos.Text = "Protocolos";
            this.Protocolos.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.txtDesde);
            this.tabPage2.Controls.Add(this.txtHasta);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Size = new System.Drawing.Size(454, 393);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Seroteca";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.nupLaboratorio);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.button3);
            this.tabPage1.Controls.Add(this.nupForense);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.nupNoPaciente);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.nupMicrobiologia);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(454, 393);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Parametros";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // nupLaboratorio
            // 
            this.nupLaboratorio.Location = new System.Drawing.Point(159, 68);
            this.nupLaboratorio.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nupLaboratorio.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nupLaboratorio.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupLaboratorio.Name = "nupLaboratorio";
            this.nupLaboratorio.Size = new System.Drawing.Size(111, 26);
            this.nupLaboratorio.TabIndex = 9;
            this.nupLaboratorio.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(30, 71);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(105, 20);
            this.label12.TabIndex = 8;
            this.label12.Text = "Laboratorio";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(9, 23);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(313, 20);
            this.label8.TabIndex = 7;
            this.label8.Text = "Cantidad de Etiquetas por Protocolo";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Gainsboro;
            this.button3.Location = new System.Drawing.Point(152, 237);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(118, 45);
            this.button3.TabIndex = 6;
            this.button3.Text = "Guardar";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // nupForense
            // 
            this.nupForense.Location = new System.Drawing.Point(158, 182);
            this.nupForense.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nupForense.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nupForense.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupForense.Name = "nupForense";
            this.nupForense.Size = new System.Drawing.Size(112, 26);
            this.nupForense.TabIndex = 5;
            this.nupForense.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(66, 186);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "Forense";
            // 
            // nupNoPaciente
            // 
            this.nupNoPaciente.Location = new System.Drawing.Point(159, 145);
            this.nupNoPaciente.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nupNoPaciente.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nupNoPaciente.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupNoPaciente.Name = "nupNoPaciente";
            this.nupNoPaciente.Size = new System.Drawing.Size(111, 26);
            this.nupNoPaciente.TabIndex = 3;
            this.nupNoPaciente.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(28, 146);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "No Pacientes";
            // 
            // nupMicrobiologia
            // 
            this.nupMicrobiologia.Location = new System.Drawing.Point(159, 108);
            this.nupMicrobiologia.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.nupMicrobiologia.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nupMicrobiologia.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nupMicrobiologia.Name = "nupMicrobiologia";
            this.nupMicrobiologia.Size = new System.Drawing.Size(111, 26);
            this.nupMicrobiologia.TabIndex = 1;
            this.nupMicrobiologia.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(30, 111);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Microbiologia";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label16);
            this.tabPage3.Controls.Add(this.txtDesdeEfector);
            this.tabPage3.Controls.Add(this.label15);
            this.tabPage3.Controls.Add(this.btnImprimirEfector);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.ddlEfector);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(454, 393);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "Lote por Efector";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(143, 134);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(127, 42);
            this.label16.TabIndex = 29;
            this.label16.Text = "Desde";
            // 
            // txtDesdeEfector
            // 
            this.txtDesdeEfector.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDesdeEfector.Location = new System.Drawing.Point(21, 181);
            this.txtDesdeEfector.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDesdeEfector.Name = "txtDesdeEfector";
            this.txtDesdeEfector.Size = new System.Drawing.Size(388, 71);
            this.txtDesdeEfector.TabIndex = 28;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(26, 167);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(0, 20);
            this.label15.TabIndex = 27;
            this.label15.Visible = false;
            // 
            // btnImprimirEfector
            // 
            this.btnImprimirEfector.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnImprimirEfector.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImprimirEfector.Location = new System.Drawing.Point(21, 274);
            this.btnImprimirEfector.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnImprimirEfector.Name = "btnImprimirEfector";
            this.btnImprimirEfector.Size = new System.Drawing.Size(388, 60);
            this.btnImprimirEfector.TabIndex = 22;
            this.btnImprimirEfector.Text = "Imprimir";
            this.btnImprimirEfector.UseVisualStyleBackColor = false;
            this.btnImprimirEfector.Click += new System.EventHandler(this.btnImprimirEfector_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(40, 27);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(318, 42);
            this.label13.TabIndex = 21;
            this.label13.Text = "Efector Solicitante";
            // 
            // ddlEfector
            // 
            this.ddlEfector.FormattingEnabled = true;
            this.ddlEfector.Location = new System.Drawing.Point(21, 86);
            this.ddlEfector.Name = "ddlEfector";
            this.ddlEfector.Size = new System.Drawing.Size(385, 28);
            this.ddlEfector.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label20);
            this.tabPage4.Controls.Add(this.label19);
            this.tabPage4.Controls.Add(this.label17);
            this.tabPage4.Controls.Add(this.label14);
            this.tabPage4.Controls.Add(this.label9);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(454, 393);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "Versiones";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(29, 257);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(389, 74);
            this.label20.TabIndex = 34;
            this.label20.Text = "Version 04-02-2022 Impresion por Areas desde el SIL";
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(29, 196);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(389, 74);
            this.label19.TabIndex = 33;
            this.label19.Text = "Version 02-11-2021 numero de protocolo arriba del codigo de barras para etiqueta " +
    "pequeña";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(29, 137);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(421, 59);
            this.label17.TabIndex = 31;
            this.label17.Text = "Version 28-10-2021 numero de protocolo de lado izquierdo vertical para etiqueta p" +
    "equeña";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(29, 78);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(421, 59);
            this.label14.TabIndex = 30;
            this.label14.Text = "Version 18-10-2021 numero de protocolo de lado izquierdo vertical";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(27, 16);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(391, 62);
            this.label9.TabIndex = 29;
            this.label9.Text = "Version 20-09-2021 habilitacion de Impresion por Lote de Efector";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(22, 443);
            this.lblError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(76, 25);
            this.lblError.TabIndex = 22;
            this.lblError.Text = "label3";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblError.Visible = false;
            // 
            // chkImpresora
            // 
            this.chkImpresora.AutoSize = true;
            this.chkImpresora.Location = new System.Drawing.Point(120, 66);
            this.chkImpresora.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkImpresora.Name = "chkImpresora";
            this.chkImpresora.Size = new System.Drawing.Size(177, 24);
            this.chkImpresora.TabIndex = 23;
            this.chkImpresora.Text = "Recordar Impresora";
            this.chkImpresora.UseVisualStyleBackColor = true;
            this.chkImpresora.CheckedChanged += new System.EventHandler(this.chkImpresora_CheckedChanged);
            // 
            // chkModoAutomatico
            // 
            this.chkModoAutomatico.AutoSize = true;
            this.chkModoAutomatico.Checked = true;
            this.chkModoAutomatico.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkModoAutomatico.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkModoAutomatico.ForeColor = System.Drawing.Color.Red;
            this.chkModoAutomatico.Location = new System.Drawing.Point(120, 100);
            this.chkModoAutomatico.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkModoAutomatico.Name = "chkModoAutomatico";
            this.chkModoAutomatico.Size = new System.Drawing.Size(212, 33);
            this.chkModoAutomatico.TabIndex = 26;
            this.chkModoAutomatico.Text = "Iniciar-Detener";
            this.chkModoAutomatico.UseVisualStyleBackColor = true;
            this.chkModoAutomatico.CheckedChanged += new System.EventHandler(this.chkModoAutomatico_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(429, 598);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 20);
            this.label10.TabIndex = 25;
            this.label10.Text = "iniciando....";
            this.label10.Visible = false;
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(420, 578);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(199, 20);
            this.label11.TabIndex = 26;
            this.label11.Text = "Activado Modo Automatico";
            this.label11.Visible = false;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(18, 485);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(263, 28);
            this.label18.TabIndex = 32;
            this.label18.Text = "Version Marzo-2024 MultiEfector";
            // 
            // lblIdEfector
            // 
            this.lblIdEfector.AutoSize = true;
            this.lblIdEfector.Location = new System.Drawing.Point(376, 485);
            this.lblIdEfector.Name = "lblIdEfector";
            this.lblIdEfector.Size = new System.Drawing.Size(60, 20);
            this.lblIdEfector.TabIndex = 33;
            this.lblIdEfector.Text = "label21";
            // 
            // txtmensajes
            // 
            this.txtmensajes.Location = new System.Drawing.Point(22, 148);
            this.txtmensajes.Multiline = true;
            this.txtmensajes.Name = "txtmensajes";
            this.txtmensajes.Size = new System.Drawing.Size(525, 276);
            this.txtmensajes.TabIndex = 34;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(345, 485);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(25, 20);
            this.label21.TabIndex = 35;
            this.label21.Text = "id:";
            this.label21.Click += new System.EventHandler(this.label21_Click);
            // 
            // Form1MultiEfector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 524);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.txtmensajes);
            this.Controls.Add(this.lblIdEfector);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.chkModoAutomatico);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.chkImpresora);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoximp);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1MultiEfector";
            this.Text = "Impresión de Etiquetas - SIL";
            this.tabControl1.ResumeLayout(false);
            this.Protocolos.ResumeLayout(false);
            this.Protocolos.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupLaboratorio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupForense)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupNoPaciente)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupMicrobiologia)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.ComboBox comboBoximp;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnImprimir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtDesde;
        private System.Windows.Forms.TextBox txtHasta;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Protocolos;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.CheckBox chkImpresora;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.NumericUpDown nupForense;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nupNoPaciente;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nupMicrobiologia;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkModoAutomatico;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown nupLaboratorio;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox ddlEfector;
        private System.Windows.Forms.Button btnImprimirEfector;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtDesdeEfector;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblIdEfector;
        private System.Windows.Forms.TextBox txtmensajes;
        private System.Windows.Forms.Label label21;
    }
}

