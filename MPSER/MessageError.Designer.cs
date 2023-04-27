
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MPSER
{
    partial class MessageError
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public string mensajeError = "";
        private Label label1;
        private Panel panel1;
        private Button btnAceptar;


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

        private void MessageError_Load(object sender, EventArgs e) => this.label1.Text = this.mensajeError;

        private void btnAceptar_Click(object sender, EventArgs e) => this.Close();

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.panel1 = new Panel();
            this.btnAceptar = new Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            this.label1.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.label1.Location = new Point(16, 12);
            this.label1.Name = "label1";
            this.label1.Size = new Size(316, 82);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            this.panel1.BackColor = SystemColors.Control;
            this.panel1.Controls.Add((Control)this.btnAceptar);
            this.panel1.Location = new Point(0, 101);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(347, 54);
            this.panel1.TabIndex = 3;
            this.btnAceptar.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.btnAceptar.Location = new Point(228, 15);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new Size(102, 27);
            this.btnAceptar.TabIndex = 0;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new EventHandler(this.btnAceptar_Click);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.BackColor = Color.White;
            this.ClientSize = new Size(346, 156);
            this.Controls.Add((Control)this.label1);
            this.Controls.Add((Control)this.panel1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MessageError";
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Error";
            this.Load += new EventHandler(this.MessageError_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }


        #endregion
    }
}