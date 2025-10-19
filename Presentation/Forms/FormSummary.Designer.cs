// FormSummary.Designer.cs  (no locals, no arithmetic in InitializeComponent)
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OneKickHeroesApp
{
    partial class FormSummary
    {
        private IContainer components = null;

        private Label lblTitle;
        private Label lblSubtitle;

        private Panel cardTotal;
        private Panel cardAge;
        private Panel cardScore;

        private Label tTotal;
        private Label tAge;
        private Label tScore;

        private Label statTotal;
        private Label statAvgAge;
        private Label statAvgScore;

        private Panel rankCard;
        private Label rTitle;
        private ListView rankList;

        private Button btnSave;
        private Label banner;

        private void InitializeComponent()
        {
            this.components = new Container();

            // ===== Form =====
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new Size(980, 600);
            this.MinimumSize = new Size(900, 560);
            this.Name = "FormSummary";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Summary";
            this.BackColor = ColorTranslator.FromHtml("#0d1117");
            this.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.Padding = new Padding(22);
            this.Load += new System.EventHandler(this.FormSummary_Load);

            // ===== Title & Subtitle =====
            this.lblTitle = new Label();
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitle.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.lblTitle.Location = new Point(36, 24);
            this.lblTitle.Text = "Summary";

            this.lblSubtitle = new Label();
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.lblSubtitle.ForeColor = ColorTranslator.FromHtml("#8b949e");
            this.lblSubtitle.Location = new Point(38, 56);
            this.lblSubtitle.Text = "Shows a summary of hero data.";

            // ===== Stat Cards =====
            // Precomputed layout: left=30, top=92, cardW=260, cardH=120, gap=16
            // cardTotal @ (30,92)
            this.cardTotal = new Panel();
            this.cardTotal.BackColor = ColorTranslator.FromHtml("#161b22");
            this.cardTotal.BorderStyle = BorderStyle.FixedSingle;
            this.cardTotal.Location = new Point(30, 92);
            this.cardTotal.Size = new Size(260, 120);

            this.tTotal = new Label();
            this.tTotal.AutoSize = true;
            this.tTotal.Font = new Font("Segoe UI", 10F);
            this.tTotal.ForeColor = ColorTranslator.FromHtml("#8b949e");
            this.tTotal.Location = new Point(8, 8);
            this.tTotal.Text = "Total Heroes";

            this.statTotal = new Label();
            this.statTotal.AutoSize = true;
            this.statTotal.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            this.statTotal.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.statTotal.Location = new Point(8, 38);
            this.statTotal.Text = "0";

            this.cardTotal.Controls.Add(this.tTotal);
            this.cardTotal.Controls.Add(this.statTotal);

            // cardAge @ (306,92)
            this.cardAge = new Panel();
            this.cardAge.BackColor = ColorTranslator.FromHtml("#161b22");
            this.cardAge.BorderStyle = BorderStyle.FixedSingle;
            this.cardAge.Location = new Point(306, 92);
            this.cardAge.Size = new Size(260, 120);

            this.tAge = new Label();
            this.tAge.AutoSize = true;
            this.tAge.Font = new Font("Segoe UI", 10F);
            this.tAge.ForeColor = ColorTranslator.FromHtml("#8b949e");
            this.tAge.Location = new Point(8, 8);
            this.tAge.Text = "Avg Age";

            this.statAvgAge = new Label();
            this.statAvgAge.AutoSize = true;
            this.statAvgAge.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            this.statAvgAge.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.statAvgAge.Location = new Point(8, 38);
            this.statAvgAge.Text = "0";

            this.cardAge.Controls.Add(this.tAge);
            this.cardAge.Controls.Add(this.statAvgAge);

            // cardScore @ (582,92)
            this.cardScore = new Panel();
            this.cardScore.BackColor = ColorTranslator.FromHtml("#161b22");
            this.cardScore.BorderStyle = BorderStyle.FixedSingle;
            this.cardScore.Location = new Point(582, 92);
            this.cardScore.Size = new Size(260, 120);

            this.tScore = new Label();
            this.tScore.AutoSize = true;
            this.tScore.Font = new Font("Segoe UI", 10F);
            this.tScore.ForeColor = ColorTranslator.FromHtml("#8b949e");
            this.tScore.Location = new Point(8, 8);
            this.tScore.Text = "Avg Exam Score";

            this.statAvgScore = new Label();
            this.statAvgScore.AutoSize = true;
            this.statAvgScore.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            this.statAvgScore.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.statAvgScore.Location = new Point(8, 38);
            this.statAvgScore.Text = "0";

            this.cardScore.Controls.Add(this.tScore);
            this.cardScore.Controls.Add(this.statAvgScore);

            // ===== Rank Card =====
            // rankCard @ (30,236) size 380x250
            this.rankCard = new Panel();
            this.rankCard.BackColor = ColorTranslator.FromHtml("#161b22");
            this.rankCard.BorderStyle = BorderStyle.FixedSingle;
            this.rankCard.Location = new Point(30, 236);
            this.rankCard.Size = new Size(380, 250);

            this.rTitle = new Label();
            this.rTitle.AutoSize = true;
            this.rTitle.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.rTitle.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.rTitle.Location = new Point(8, 8);
            this.rTitle.Text = "Heroes Per Rank";

            this.rankList = new ListView();
            this.rankList.Location = new Point(8, 42);
            this.rankList.Size = new Size(364, 200); // 380-16 by 250-50
            this.rankList.BorderStyle = BorderStyle.None;
            this.rankList.View = View.Details;
            this.rankList.FullRowSelect = true;
            this.rankList.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.rankList.BackColor = Color.FromArgb(22, 27, 34);
            this.rankList.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.rankList.Columns.Add("Rank", 120, HorizontalAlignment.Left);
            this.rankList.Columns.Add("Total", 100, HorizontalAlignment.Left);

            this.rankCard.Controls.Add(this.rTitle);
            this.rankCard.Controls.Add(this.rankList);

            // ===== Save Button =====
            // btnSave @ (450,442)
            this.btnSave = new Button();
            this.btnSave.Text = "Save to summary.txt";
            this.btnSave.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.BackColor = ColorTranslator.FromHtml("#1f6feb");
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Size = new Size(260, 44);
            this.btnSave.Location = new Point(450, 442);
            this.btnSave.Click += new System.EventHandler(this.OnSaveClicked);

            // ===== Success Banner =====
            // banner @ (430,382)
            this.banner = new Label();
            this.banner.AutoSize = false;
            this.banner.Size = new Size(320, 42);
            this.banner.TextAlign = ContentAlignment.MiddleLeft;
            this.banner.Padding = new Padding(12, 0, 0, 0);
            this.banner.Text = "✓  Summary saved to summary.txt";
            this.banner.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.banner.ForeColor = ColorTranslator.FromHtml("#c9d1d9");
            this.banner.BackColor = Color.FromArgb(22, 27, 34);
            this.banner.BorderStyle = BorderStyle.FixedSingle;
            this.banner.Location = new Point(430, 382);
            this.banner.Visible = false;

            // ===== Add to form =====
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.cardTotal);
            this.Controls.Add(this.cardAge);
            this.Controls.Add(this.cardScore);
            this.Controls.Add(this.rankCard);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.banner);

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
