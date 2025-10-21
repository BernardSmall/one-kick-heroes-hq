// FormAddHero.Designer.cs
using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace OneKickHeroesApp
{
    partial class FormAddHero
    {
        private IContainer components = null;

        private Label lblTitle;
        private Label lblSubtitle;
        private Panel card;
        private Label lblName;
        private TextBox txtName;
        private Label lblAge;
        private TextBox txtAge;
        private Label lblPower;
        private TextBox txtPower;
        private Label lblScore;
        private TextBox txtScore;
        private Label lblRank;
        private ComboBox cboRank;
        private Button btnAdd;

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.card = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cboRank = new System.Windows.Forms.ComboBox();
            this.lblRank = new System.Windows.Forms.Label();
            this.txtScore = new System.Windows.Forms.TextBox();
            this.lblScore = new System.Windows.Forms.Label();
            this.txtPower = new System.Windows.Forms.TextBox();
            this.lblPower = new System.Windows.Forms.Label();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.lblAge = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.card.SuspendLayout();
            this.SuspendLayout();
            // 
            // Form basics
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(900, 620);
            this.MinimumSize = new System.Drawing.Size(720, 560);
            this.Name = "FormAddHero";
            this.Text = "Add New Superhero";
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ColorTranslator.FromHtml("#0d1117");
            this.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.Padding = new Padding(22);
            this.Load += new System.EventHandler(this.FormAddHero_Load);

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblTitle.Location = new System.Drawing.Point(36, 36);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(228, 30);
            this.lblTitle.Text = "Add New Superhero";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblSubtitle.ForeColor = ColorTranslator.FromHtml("#8b949e");
            this.lblSubtitle.Location = new System.Drawing.Point(38, 72);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(273, 19);
            this.lblSubtitle.Text = "Enter the details for the new superhero.";
            // 
            // card
            // 
            this.card.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                    | System.Windows.Forms.AnchorStyles.Left)
                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.card.BackColor = ColorTranslator.FromHtml("#161b22");
            this.card.BorderStyle = BorderStyle.FixedSingle; // thin border; we’ll tint it in Load
            this.card.Location = new System.Drawing.Point(30, 110);
            this.card.Name = "card";
            this.card.Padding = new System.Windows.Forms.Padding(28);
            this.card.Size = new System.Drawing.Size(840, 480);
            this.card.TabIndex = 0;
            this.card.AutoScroll = true;

            // ---- Inside card: controls placed with simple y spacing ----
            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblName.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblName.Location = new System.Drawing.Point(12, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(60, 25);
            this.lblName.Text = "Name";

            // txtName
            this.txtName.BackColor = Color.FromArgb(22, 27, 34);
            this.txtName.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.txtName.BorderStyle = BorderStyle.FixedSingle;
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.txtName.Location = new System.Drawing.Point(12, 56);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(780, 25);
            this.txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.txtName.TabIndex = 0;

            // lblAge
            this.lblAge.AutoSize = true;
            this.lblAge.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblAge.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblAge.Location = new System.Drawing.Point(12, 99);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(44, 25);
            this.lblAge.Text = "Age";

            // txtAge
            this.txtAge.BackColor = Color.FromArgb(22, 27, 34);
            this.txtAge.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.txtAge.BorderStyle = BorderStyle.FixedSingle;
            this.txtAge.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.txtAge.Location = new System.Drawing.Point(12, 131);
            this.txtAge.MaxLength = 3;
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(780, 25);
            this.txtAge.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.txtAge.TabIndex = 1;
            this.txtAge.KeyPress += new KeyPressEventHandler(this.txtAge_KeyPress);

            // lblPower
            this.lblPower.AutoSize = true;
            this.lblPower.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblPower.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblPower.Location = new System.Drawing.Point(12, 174);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(113, 25);
            this.lblPower.Text = "Superpower";

            // txtPower
            this.txtPower.BackColor = Color.FromArgb(22, 27, 34);
            this.txtPower.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.txtPower.BorderStyle = BorderStyle.FixedSingle;
            this.txtPower.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.txtPower.Location = new System.Drawing.Point(12, 206);
            this.txtPower.Name = "txtPower";
            this.txtPower.Size = new System.Drawing.Size(780, 25);
            this.txtPower.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.txtPower.TabIndex = 2;

            // lblScore
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblScore.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblScore.Location = new System.Drawing.Point(12, 249);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(160, 25);
            this.lblScore.Text = "Hero Exam Score";

            // txtScore
            this.txtScore.BackColor = Color.FromArgb(22, 27, 34);
            this.txtScore.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.txtScore.BorderStyle = BorderStyle.FixedSingle;
            this.txtScore.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.txtScore.Location = new System.Drawing.Point(12, 281);
            this.txtScore.MaxLength = 5;
            this.txtScore.Name = "txtScore";
            this.txtScore.Size = new System.Drawing.Size(780, 25);
            this.txtScore.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.txtScore.TabIndex = 3;
            this.txtScore.KeyPress += new KeyPressEventHandler(this.txtScore_KeyPress);

            // lblRank
            this.lblRank.AutoSize = true;
            this.lblRank.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblRank.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblRank.Location = new System.Drawing.Point(12, 324);
            this.lblRank.Name = "lblRank";
            this.lblRank.Size = new System.Drawing.Size(56, 25);
            this.lblRank.Text = "Rank";

            // cboRank
            this.cboRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRank.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboRank.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.cboRank.BackColor = Color.FromArgb(22, 27, 34);
            this.cboRank.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.cboRank.Location = new System.Drawing.Point(12, 356);
            this.cboRank.Name = "cboRank";
            this.cboRank.Size = new System.Drawing.Size(780, 25);
            this.cboRank.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.cboRank.TabIndex = 4;

            // btnAdd
            this.btnAdd.BackColor = ColorTranslator.FromHtml("#1f6feb");
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnAdd.Location = new System.Drawing.Point(12, 410);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(220, 44);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add Superhero";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // add children to card
            this.card.Controls.Add(this.btnAdd);
            this.card.Controls.Add(this.cboRank);
            this.card.Controls.Add(this.lblRank);
            this.card.Controls.Add(this.txtScore);
            this.card.Controls.Add(this.lblScore);
            this.card.Controls.Add(this.txtPower);
            this.card.Controls.Add(this.lblPower);
            this.card.Controls.Add(this.txtAge);
            this.card.Controls.Add(this.lblAge);
            this.card.Controls.Add(this.txtName);
            this.card.Controls.Add(this.lblName);

            // add top-level controls
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.card);

            // AcceptButton so Enter submits
            this.AcceptButton = this.btnAdd;

            this.card.ResumeLayout(false);
            this.card.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            // Populate combo items (do it here so it shows in Designer too)
            this.cboRank.Items.AddRange(new object[] { "Auto (from score)", "S", "A", "B", "C" });
            this.cboRank.SelectedIndex = 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }
    }
}
