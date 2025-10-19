// FormUpdateHero.Designer.cs
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OneKickHeroesApp
{
    partial class FormUpdateHero
    {
        private IContainer components = null;

        private Label lblTitle;
        private Panel card;

        private Label lblId;
        private TextBox txtId;
        private Button btnLoad;

        private Label lblName;
        private TextBox txtName;

        private Label lblAge;
        private TextBox txtAge;

        private Label lblPower;
        private TextBox txtPower;

        private Label lblScore;
        private TextBox txtScore;

        private Label lblRank;
        private TextBox txtRank;

        private Button btnSave;

        private void InitializeComponent()
        {
            this.components = new Container();

            // ===== Form =====
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(760, 560);
            this.MinimumSize = new Size(760, 560);
            this.Name = "FormUpdateHero";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Update Superhero";
            this.BackColor = ColorTranslator.FromHtml("#0d1117");
            this.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.Padding = new Padding(22);
            this.Load += new System.EventHandler(this.FormUpdateHero_Load);

            // ===== Title =====
            this.lblTitle = new Label();
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitle.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblTitle.Location = new Point(36, 36);
            this.lblTitle.Text = "Update Superhero";

            // ===== Card (simple bordered panel) =====
            this.card = new Panel();
            this.card.BackColor = ColorTranslator.FromHtml("#161b22");
            this.card.BorderStyle = BorderStyle.FixedSingle;
            this.card.Location = new Point(30, 90);
            this.card.Size = new Size(700, 420);
            this.card.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            // ===== ID + Load =====
            this.lblId = new Label();
            this.lblId.AutoSize = true;
            this.lblId.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.lblId.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblId.Location = new Point(12, 12);
            this.lblId.Text = "Hero ID";

            this.txtId = new TextBox();
            this.txtId.Name = "txtId";
            this.txtId.Font = new Font("Segoe UI", 10F);
            this.txtId.BackColor = Color.FromArgb(22, 27, 34);
            this.txtId.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.txtId.BorderStyle = BorderStyle.FixedSingle;
            this.txtId.Location = new Point(12, 40);
            this.txtId.Size = new Size(520, 25);
            this.txtId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            this.btnLoad = new Button();
            this.btnLoad.Text = "Load";
            this.btnLoad.Font = new Font("Segoe UI", 10F);
            this.btnLoad.BackColor = ColorTranslator.FromHtml("#1f6feb");
            this.btnLoad.ForeColor = Color.White;
            this.btnLoad.FlatStyle = FlatStyle.Flat;
            this.btnLoad.FlatAppearance.BorderSize = 0;
            this.btnLoad.Location = new Point(540, 38);
            this.btnLoad.Size = new Size(120, 29);
            this.btnLoad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);

            // ===== Name =====
            this.lblName = new Label();
            this.lblName.AutoSize = true;
            this.lblName.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.lblName.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblName.Location = new Point(12, 78);
            this.lblName.Text = "Name";

            this.txtName = new TextBox();
            this.txtName.Name = "txtName";
            this.txtName.Font = new Font("Segoe UI", 10F);
            this.txtName.BackColor = Color.FromArgb(22, 27, 34);
            this.txtName.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.txtName.BorderStyle = BorderStyle.FixedSingle;
            this.txtName.Location = new Point(12, 106);
            this.txtName.Size = new Size(648, 25);
            this.txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // ===== Age =====
            this.lblAge = new Label();
            this.lblAge.AutoSize = true;
            this.lblAge.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.lblAge.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblAge.Location = new Point(12, 144);
            this.lblAge.Text = "Age";

            this.txtAge = new TextBox();
            this.txtAge.Name = "txtAge";
            this.txtAge.Font = new Font("Segoe UI", 10F);
            this.txtAge.BackColor = Color.FromArgb(22, 27, 34);
            this.txtAge.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.txtAge.BorderStyle = BorderStyle.FixedSingle;
            this.txtAge.Location = new Point(12, 172);
            this.txtAge.Size = new Size(648, 25);
            this.txtAge.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.txtAge.MaxLength = 3;
            this.txtAge.KeyPress += new KeyPressEventHandler(this.txtAge_KeyPress);

            // ===== Power =====
            this.lblPower = new Label();
            this.lblPower.AutoSize = true;
            this.lblPower.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.lblPower.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblPower.Location = new Point(12, 210);
            this.lblPower.Text = "Superpower";

            this.txtPower = new TextBox();
            this.txtPower.Name = "txtPower";
            this.txtPower.Font = new Font("Segoe UI", 10F);
            this.txtPower.BackColor = Color.FromArgb(22, 27, 34);
            this.txtPower.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.txtPower.BorderStyle = BorderStyle.FixedSingle;
            this.txtPower.Location = new Point(12, 238);
            this.txtPower.Size = new Size(648, 25);
            this.txtPower.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // ===== Score / Rank (two inputs) =====
            this.lblScore = new Label();
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.lblScore.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblScore.Location = new Point(12, 276);
            this.lblScore.Text = "Hero Exam Score";

            this.txtScore = new TextBox();
            this.txtScore.Name = "txtScore";
            this.txtScore.Font = new Font("Segoe UI", 10F);
            this.txtScore.BackColor = Color.FromArgb(22, 27, 34);
            this.txtScore.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.txtScore.BorderStyle = BorderStyle.FixedSingle;
            this.txtScore.Location = new Point(12, 304);
            this.txtScore.Size = new Size(320, 25);
            this.txtScore.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            this.txtScore.TextChanged += new System.EventHandler(this.txtScore_TextChanged);
            this.txtScore.KeyPress += new KeyPressEventHandler(this.txtScore_KeyPress);

            this.lblRank = new Label();
            this.lblRank.AutoSize = true;
            this.lblRank.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.lblRank.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblRank.Location = new Point(356, 276);
            this.lblRank.Text = "Rank";

            this.txtRank = new TextBox();
            this.txtRank.Name = "txtRank";
            this.txtRank.Font = new Font("Segoe UI", 10F);
            this.txtRank.BackColor = Color.FromArgb(28, 33, 41);
            this.txtRank.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.txtRank.BorderStyle = BorderStyle.FixedSingle;
            this.txtRank.Location = new Point(356, 304);
            this.txtRank.Size = new Size(304, 25);
            this.txtRank.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.txtRank.ReadOnly = true;
            this.txtRank.TabStop = false;

            // ===== Save =====
            this.btnSave = new Button();
            this.btnSave.Text = "Save";
            this.btnSave.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.btnSave.BackColor = ColorTranslator.FromHtml("#1f6feb");
            this.btnSave.ForeColor = Color.White;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Size = new Size(220, 44);
            this.btnSave.Location = new Point(12, 350);
            this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // Add controls to card
            this.card.Controls.Add(this.lblId);
            this.card.Controls.Add(this.txtId);
            this.card.Controls.Add(this.btnLoad);

            this.card.Controls.Add(this.lblName);
            this.card.Controls.Add(this.txtName);

            this.card.Controls.Add(this.lblAge);
            this.card.Controls.Add(this.txtAge);

            this.card.Controls.Add(this.lblPower);
            this.card.Controls.Add(this.txtPower);

            this.card.Controls.Add(this.lblScore);
            this.card.Controls.Add(this.txtScore);

            this.card.Controls.Add(this.lblRank);
            this.card.Controls.Add(this.txtRank);

            this.card.Controls.Add(this.btnSave);

            // Add to form
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
