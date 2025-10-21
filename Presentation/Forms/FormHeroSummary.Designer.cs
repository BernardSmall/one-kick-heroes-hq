// FormHeroSummary.Designer.cs
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OneKickHeroesApp
{
    partial class FormHeroSummary
    {
        private IContainer components = null;

        private Label lblTitle;
        private Panel card;
        private Label lblId;
        private TextBox txtId;
        private Button btnLoad;
        private Button btnSave;
        private Panel sep;

        // Left col
        private Label lName;
        private Label vName;
        private Label lAge;
        private Label vAge;
        private Label lPower;
        private Label vPower;

        // Right col
        private Label lScore;
        private Label vScore;
        private Label lRank;
        private Label vRank;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.lblTitle = new System.Windows.Forms.Label();
            this.card = new System.Windows.Forms.Panel();
            this.lblId = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.sep = new System.Windows.Forms.Panel();

            this.lName = new System.Windows.Forms.Label();
            this.vName = new System.Windows.Forms.Label();
            this.lAge = new System.Windows.Forms.Label();
            this.vAge = new System.Windows.Forms.Label();
            this.lPower = new System.Windows.Forms.Label();
            this.vPower = new System.Windows.Forms.Label();

            this.lScore = new System.Windows.Forms.Label();
            this.vScore = new System.Windows.Forms.Label();
            this.lRank = new System.Windows.Forms.Label();
            this.vRank = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // ===== Form =====
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(980, 600);
            this.MinimumSize = new System.Drawing.Size(900, 560);
            this.Name = "FormHeroSummary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Single Hero Summary";
            this.BackColor = ColorTranslator.FromHtml("#0d1117");
            this.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.Padding = new Padding(22);
            this.Load += new System.EventHandler(this.FormHeroSummary_Load);

            // ===== Title =====
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblTitle.Location = new System.Drawing.Point(36, 24);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(232, 30);
            this.lblTitle.Text = "Single Hero Summary";

            // ===== Card =====
            this.card.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.card.BackColor = ColorTranslator.FromHtml("#161b22");
            this.card.Location = new System.Drawing.Point(30, 70);
            this.card.Name = "card";
            this.card.Padding = new Padding(20);
            this.card.Size = new System.Drawing.Size(920, 500);
            this.card.Paint += new PaintEventHandler(this.card_Paint);
            this.card.SizeChanged += new EventHandler(this.card_SizeChanged);

            // ===== Input row =====
            // lblId
            this.lblId.AutoSize = true;
            this.lblId.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblId.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblId.Location = new System.Drawing.Point(12, 16);
            this.lblId.Name = "lblId";
            this.lblId.Text = "Hero ID";

            // txtId
            this.txtId.Name = "txtId";
            this.txtId.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtId.BackColor = Color.FromArgb(22, 27, 34);
            this.txtId.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.txtId.BorderStyle = BorderStyle.FixedSingle;
            this.txtId.Location = new System.Drawing.Point(12, 44);
            this.txtId.Size = new System.Drawing.Size(620, 25);
            this.txtId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.txtId.KeyDown += new KeyEventHandler(this.txtId_KeyDown);

            // btnLoad
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Text = "Load";
            this.btnLoad.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnLoad.BackColor = ColorTranslator.FromHtml("#1f6feb");
            this.btnLoad.FlatStyle = FlatStyle.Flat;
            this.btnLoad.FlatAppearance.BorderSize = 0;
            this.btnLoad.ForeColor = Color.White;
            this.btnLoad.Location = new System.Drawing.Point(646, 42);
            this.btnLoad.Size = new System.Drawing.Size(100, 29);
            this.btnLoad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnLoad.Click += new EventHandler(this.btnLoad_Click);

            // btnSave
            this.btnSave.Name = "btnSave";
            this.btnSave.Text = "Save Summary to Txt";
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnSave.BackColor = ColorTranslator.FromHtml("#1f6feb");
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new System.Drawing.Point(752, 42);
            this.btnSave.Size = new System.Drawing.Size(156, 29);
            this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnSave.Enabled = false;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);

            // sep
            this.sep.BackColor = ColorTranslator.FromHtml("#30363d");
            this.sep.Location = new System.Drawing.Point(12, 90);
            this.sep.Name = "sep";
            this.sep.Size = new System.Drawing.Size(896, 1);
            this.sep.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // ===== Two-column details =====
            // left
            this.lName.AutoSize = true;
            this.lName.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lName.ForeColor = ColorTranslator.FromHtml("#8b949e");
            this.lName.Location = new System.Drawing.Point(12, 110);
            this.lName.Text = "Name:";

            this.vName.AutoSize = true;
            this.vName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.vName.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.vName.Location = new System.Drawing.Point(132, 111);
            this.vName.Text = "—";

            this.lAge.AutoSize = true;
            this.lAge.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lAge.ForeColor = ColorTranslator.FromHtml("#8b949e");
            this.lAge.Location = new System.Drawing.Point(12, 150);
            this.lAge.Text = "Age:";

            this.vAge.AutoSize = true;
            this.vAge.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.vAge.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.vAge.Location = new System.Drawing.Point(132, 151);
            this.vAge.Text = "—";

            this.lPower.AutoSize = true;
            this.lPower.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lPower.ForeColor = ColorTranslator.FromHtml("#8b949e");
            this.lPower.Location = new System.Drawing.Point(12, 190);
            this.lPower.Text = "Superpower:";

            this.vPower.AutoSize = true;
            this.vPower.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.vPower.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.vPower.Location = new System.Drawing.Point(132, 191);
            this.vPower.MaximumSize = new System.Drawing.Size(760, 0);
            this.vPower.Text = "—";

            // right (initial x; re-aligned on SizeChanged)
            this.lScore.AutoSize = true;
            this.lScore.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lScore.ForeColor = ColorTranslator.FromHtml("#8b949e");
            this.lScore.Location = new System.Drawing.Point(480, 110);
            this.lScore.Text = "Exam Score:";

            this.vScore.AutoSize = true;
            this.vScore.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.vScore.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.vScore.Location = new System.Drawing.Point(640, 111);
            this.vScore.Text = "—";

            this.lRank.AutoSize = true;
            this.lRank.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lRank.ForeColor = ColorTranslator.FromHtml("#8b949e");
            this.lRank.Location = new System.Drawing.Point(480, 150);
            this.lRank.Text = "Rank:";

            this.vRank.AutoSize = true;
            this.vRank.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.vRank.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.vRank.Location = new System.Drawing.Point(640, 151);
            this.vRank.Text = "—";

            // ===== Add to card =====
            this.card.Controls.Add(this.lblId);
            this.card.Controls.Add(this.txtId);
            this.card.Controls.Add(this.btnLoad);
            this.card.Controls.Add(this.btnSave);
            this.card.Controls.Add(this.sep);

            this.card.Controls.Add(this.lName);
            this.card.Controls.Add(this.vName);
            this.card.Controls.Add(this.lAge);
            this.card.Controls.Add(this.vAge);
            this.card.Controls.Add(this.lPower);
            this.card.Controls.Add(this.vPower);

            this.card.Controls.Add(this.lScore);
            this.card.Controls.Add(this.vScore);
            this.card.Controls.Add(this.lRank);
            this.card.Controls.Add(this.vRank);

            // ===== Add to form =====
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.card);

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
