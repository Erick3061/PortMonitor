using CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace MPSER
{
    partial class EliminaIP
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public _MPCLogica cloProject = new _MPCLogica();
        private System.ComponentModel.IContainer components = null;
        private textBoxNum txtCodEquipo;
        private Label lbIP;
        private Label lbPuerto;
        private Button btnSalir;
        private Label label3;
        private Button btnEliminar;
        private Button btnCancelar;
        private GroupBox groupBox1;
        private Label label5;
        private Label label4;

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

        private void EliminaIP_Load(object sender, EventArgs e) => ((Control)this.txtCodEquipo).Focus();

        private void btnValidar_Click(object sender, EventArgs e)
        {
            if (((Control)this.txtCodEquipo).Text.Trim() == "")
            {
                int num = (int)MessageBox.Show("Aun no ha ingresado Código de Equipo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ((Control)this.txtCodEquipo).Focus();
            }
            else
            {
                List<RecepcionIP> recepcionIpList = this.cloProject.ConsultaRecIP(Convert.ToInt32(((Control)this.txtCodEquipo).Text));
                if (recepcionIpList.Count != 0)
                {
                    this.lbIP.Text = recepcionIpList[0].IP;
                    this.lbPuerto.Text = recepcionIpList[0].Puerto.ToString();
                }
                else
                {
                    int num = (int)MessageBox.Show("Parametros no registrados", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ((Control)this.txtCodEquipo).Focus();
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (((Control)this.txtCodEquipo).Text.Trim() == "")
            {
                int num = (int)MessageBox.Show("Código de equipo no valido", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ((Control)this.txtCodEquipo).Focus();
            }
            else if (this.lbIP.Text.Trim() == "" && this.lbPuerto.Text.Trim() == "")
            {
                int num = (int)MessageBox.Show("Falta validar parametros del equipo", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ((Control)this.txtCodEquipo).Focus();
            }
            else
            {
                string text = this.cloProject.EliminarParamIP(Convert.ToInt32(((Control)this.txtCodEquipo).Text.Trim()));
                if (text.Trim() != "")
                {
                    int num = (int)MessageBox.Show(text, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ((Control)this.txtCodEquipo).Focus();
                }
                else
                {
                    int num = (int)MessageBox.Show("Parametros eliminados", "Información", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    ((Control)this.txtCodEquipo).Focus();
                    ((Control)this.txtCodEquipo).Text = "";
                    this.lbIP.Text = "";
                    this.lbPuerto.Text = "";
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => this.Close();

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EliminaIP));
            this.txtCodEquipo = new CustomControls.textBoxNum();
            this.lbIP = new System.Windows.Forms.Label();
            this.lbPuerto = new System.Windows.Forms.Label();
            this.btnSalir = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtCodEquipo
            // 
            this.txtCodEquipo.AcceptsNegative = false;
            this.txtCodEquipo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodEquipo.ForeColor = System.Drawing.Color.Black;
            this.txtCodEquipo.Location = new System.Drawing.Point(61, 33);
            this.txtCodEquipo.Mask = null;
            this.txtCodEquipo.Name = "txtCodEquipo";
            this.txtCodEquipo.Size = new System.Drawing.Size(34, 20);
            this.txtCodEquipo.TabIndex = 0;
            this.txtCodEquipo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCodEquipo.TextChanged += new System.EventHandler(this.txtCodEquipo_TextChanged);
            // 
            // lbIP
            // 
            this.lbIP.BackColor = System.Drawing.Color.White;
            this.lbIP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbIP.ForeColor = System.Drawing.Color.Black;
            this.lbIP.Location = new System.Drawing.Point(100, 33);
            this.lbIP.Name = "lbIP";
            this.lbIP.Size = new System.Drawing.Size(108, 20);
            this.lbIP.TabIndex = 2;
            this.lbIP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbPuerto
            // 
            this.lbPuerto.BackColor = System.Drawing.Color.White;
            this.lbPuerto.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbPuerto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPuerto.ForeColor = System.Drawing.Color.Black;
            this.lbPuerto.Location = new System.Drawing.Point(213, 33);
            this.lbPuerto.Name = "lbPuerto";
            this.lbPuerto.Size = new System.Drawing.Size(46, 20);
            this.lbPuerto.TabIndex = 3;
            this.lbPuerto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSalir
            // 
            this.btnSalir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalir.BackColor = System.Drawing.SystemColors.Control;
            this.btnSalir.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.ForeColor = System.Drawing.Color.Black;
            this.btnSalir.Location = new System.Drawing.Point(266, 30);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(80, 26);
            this.btnSalir.TabIndex = 1;
            this.btnSalir.Text = "&Validar";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnValidar_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Equipo:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminar.BackColor = System.Drawing.SystemColors.Control;
            this.btnEliminar.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminar.ForeColor = System.Drawing.Color.Black;
            this.btnEliminar.Location = new System.Drawing.Point(12, 102);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(88, 28);
            this.btnEliminar.TabIndex = 1;
            this.btnEliminar.Text = "&Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancelar.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.Black;
            this.btnCancelar.Location = new System.Drawing.Point(279, 102);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(88, 28);
            this.btnCancelar.TabIndex = 2;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtCodEquipo);
            this.groupBox1.Controls.Add(this.lbIP);
            this.groupBox1.Controls.Add(this.lbPuerto);
            this.groupBox1.Controls.Add(this.btnSalir);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(355, 66);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(213, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Puerto";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(97, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "IP";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EliminaIP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(379, 142);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnEliminar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "EliminaIP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Borrar Parametros";
            this.Load += new System.EventHandler(this.EliminaIP_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}