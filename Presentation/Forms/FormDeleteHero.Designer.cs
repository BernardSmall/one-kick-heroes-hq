// FormDeleteHero.Designer.cs
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OneKickHeroesApp
{
    partial class FormDeleteHero
    {
        private IContainer components = null;

        private Label lblTitle;
        private Label lblSubtitle;
        private DataGridView grid;
        private Button btnDelete;
        private Label banner;

        // Columns
        private DataGridViewCheckBoxColumn colChk;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colAge;
        private DataGridViewTextBoxColumn colPow;
        private DataGridViewTextBoxColumn colScore;
        private DataGridViewTextBoxColumn colRank;
        private DataGridViewTextBoxColumn colIdRaw;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.grid = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();
            this.banner = new System.Windows.Forms.Label();

            this.colChk = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIdRaw = new System.Windows.Forms.DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // FormDeleteHero
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(980, 640);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "FormDeleteHero";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Delete Superhero";
            this.BackColor = ColorTranslator.FromHtml("#0d1117");
            this.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.Padding = new Padding(22);
            this.Load += new System.EventHandler(this.FormDeleteHero_Load);

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblTitle.Location = new System.Drawing.Point(36, 24);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(196, 30);
            this.lblTitle.Text = "Delete Superhero";

            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSubtitle.ForeColor = ColorTranslator.FromHtml("#8b949e");
            this.lblSubtitle.Location = new System.Drawing.Point(38, 58);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(305, 19);
            this.lblSubtitle.Text = "Select a superhero to delete from the list below.";

            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | System.Windows.Forms.AnchorStyles.Left)
                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.BackgroundColor = ColorTranslator.FromHtml("#161b22");
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.EnableHeadersVisualStyles = false;
            this.grid.GridColor = ColorTranslator.FromHtml("#30363d");
            this.grid.Location = new System.Drawing.Point(30, 96);
            this.grid.MultiSelect = true;
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ReadOnly = false;
            this.grid.EditMode = DataGridViewEditMode.EditOnEnter;
            this.grid.Size = new System.Drawing.Size(920, 420);
            // dark header & rows
            this.grid.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#161b22");
            this.grid.ColumnHeadersDefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.grid.DefaultCellStyle.BackColor = Color.FromArgb(22, 27, 34);
            this.grid.DefaultCellStyle.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(36, 41, 47);
            this.grid.DefaultCellStyle.SelectionForeColor = ColorTranslator.FromHtml("#c9d1d9");

            // columns
            this.colChk.HeaderText = "";
            this.colChk.Width = 36;
            this.colChk.ReadOnly = false;

            this.colId.HeaderText = "Hero ID";
            this.colId.Width = 110;
            this.colId.ReadOnly = true;

            this.colName.HeaderText = "Name";
            this.colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.colName.ReadOnly = true;

            this.colAge.HeaderText = "Age";
            this.colAge.Width = 70;
            this.colAge.ReadOnly = true;

            this.colPow.HeaderText = "Superpower";
            this.colPow.Width = 220;
            this.colPow.ReadOnly = true;

            this.colScore.HeaderText = "Exam Score";
            this.colScore.Width = 110;
            this.colScore.ReadOnly = true;

            this.colRank.HeaderText = "Rank";
            this.colRank.Width = 80;
            this.colRank.ReadOnly = true;

            this.colIdRaw.HeaderText = "IdRaw";
            this.colIdRaw.Name = "IdRaw";
            this.colIdRaw.Visible = false;

            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colChk, this.colId, this.colName, this.colAge, this.colPow, this.colScore, this.colRank, this.colIdRaw
            });

            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.BackColor = Color.FromArgb(208, 64, 64);
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(30, 532);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(220, 44);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // 
            // banner
            // 
            this.banner.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.banner.AutoSize = false;
            this.banner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.banner.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.banner.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.banner.BackColor = Color.FromArgb(22, 27, 34);
            this.banner.Location = new System.Drawing.Point(30, 582);
            this.banner.Name = "banner";
            this.banner.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.banner.Size = new System.Drawing.Size(920, 44);
            this.banner.Text = "✓  Superhero deleted successfully.";
            this.banner.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.banner.Visible = false;

            // 
            // add controls
            // 
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.banner);

            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
    }
}
