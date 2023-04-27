using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MPSER
{
    partial class Log
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public List<strTXT> cLog = new List<strTXT>();
        public string strNombreArchivo = "";
        private DataGridView dgvLog;
        private DataGridViewTextBoxColumn Evento;

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

        private void Log_Load(object sender, EventArgs e)
        {
            this.Text = "Log " + this.strNombreArchivo;
            if (this.cLog.Count == 0)
                return;
            foreach (strTXT strTxt in this.cLog)
                this.dgvLog.Rows.Insert(0, new object[1]
                {
          (object) strTxt.Evento
                });
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Log));
            this.dgvLog = new DataGridView();
            this.Evento = new DataGridViewTextBoxColumn();
            ((ISupportInitialize)this.dgvLog).BeginInit();
            this.SuspendLayout();
            this.dgvLog.BackgroundColor = Color.White;
            this.dgvLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLog.ColumnHeadersVisible = false;
            this.dgvLog.Columns.AddRange((DataGridViewColumn)this.Evento);
            this.dgvLog.Dock = DockStyle.Fill;
            this.dgvLog.GridColor = Color.WhiteSmoke;
            this.dgvLog.Location = new Point(0, 0);
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.ReadOnly = true;
            this.dgvLog.RowHeadersVisible = false;
            this.dgvLog.RowTemplate.Height = 17;
            this.dgvLog.Size = new Size(373, 391);
            this.dgvLog.TabIndex = 1;
            this.Evento.HeaderText = "Evento";
            this.Evento.Name = "Evento";
            this.Evento.ReadOnly = true;
            this.Evento.Width = 500;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.DimGray;
            this.ClientSize = new Size(373, 391);
            this.Controls.Add((Control)this.dgvLog);
            //this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.Name = "Log";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += new EventHandler(this.Log_Load);
            ((ISupportInitialize)this.dgvLog).EndInit();
            this.ResumeLayout(false);
        }


        #endregion
    }
}