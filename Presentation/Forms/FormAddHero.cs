using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OneKickHeroesApp
{
    public partial class FormAddHero : Form
    {
        // --- Theme ---
        private static class Theme
        {
            public static readonly Color Bg      = ColorTranslator.FromHtml("#0d1117");
            public static readonly Color Surface = ColorTranslator.FromHtml("#161b22");
            public static readonly Color Border  = ColorTranslator.FromHtml("#30363d");
            public static readonly Color Accent  = ColorTranslator.FromHtml("#1f6feb");
            public static readonly Color Text    = ColorTranslator.FromHtml("#c9d1d9");
            public static readonly Color Muted   = ColorTranslator.FromHtml("#8b949e");

            public static Font H1  { get { return new Font("Segoe UI", 16, FontStyle.Bold); } }
            public static Font H2  { get { return new Font("Segoe UI", 13, FontStyle.Bold); } }
            public static Font Body{ get { return new Font("Segoe UI", 10, FontStyle.Regular); } }
        }

        // Inputs
        private TextBox txtHeroId;
        private TextBox txtName;
        private TextBox txtAge;
        private TextBox txtPower;
        private TextBox txtScore;
        private Button btnAdd;
        private Button btnBack;

        public FormAddHero()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            // Form basics
            Text = "Add New Superhero";
            BackColor = Theme.Bg;
            ForeColor = Theme.Text;
            MinimumSize = new Size(720, 560);
            StartPosition = FormStartPosition.CenterParent;
            Padding = new Padding(22);
            DoubleBuffered = true;

            // Header
            var title = new Label
            {
                Text = "Add New Superhero",
                Font = Theme.H1,
                ForeColor = Theme.Text,
                AutoSize = true,
                Location = new Point(36, 36)
            };
            Controls.Add(title);

            var subtitle = new Label
            {
                Text = "Enter the details for the new superhero.",
                Font = Theme.Body,
                ForeColor = Theme.Muted,
                AutoSize = true,
                Location = new Point(38, 72)
            };
            Controls.Add(subtitle);

            // Card container
            var card = new Panel
            {
                BackColor = Theme.Surface,
                Width = ClientSize.Width - 72,
                Height = ClientSize.Height - 150,
                Location = new Point(30, 110),
                Padding = new Padding(28),
                AutoScroll = true
            };
            card.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            card.Paint += delegate (object s, PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var gp = new GraphicsPath())
                {
                    int r = 14;
                    gp.AddArc(0, 0, r, r, 180, 90);
                    gp.AddArc(card.Width - r - 1, 0, r, r, 270, 90);
                    gp.AddArc(card.Width - r - 1, card.Height - r - 1, r, r, 0, 90);
                    gp.AddArc(0, card.Height - r - 1, r, r, 90, 90);
                    gp.CloseAllFigures();
                    card.Region = new Region(gp);
                    using (var pen = new Pen(Theme.Border, 1)) { g.DrawPath(pen, gp); }
                }
            };
            Controls.Add(card);

            // Layout helpers
            int y = 24;
            int gap = 18;

            Func<string, Label> mkLabel = (text) => new Label
            {
                Text = text,
                ForeColor = Theme.Text,
                Font = Theme.H2,
                AutoSize = true,
                Location = new Point(12, y)
            };

            Func<TextBox> mkInput = () =>
            {
                var tb = new TextBox
                {
                    BackColor = Color.FromArgb(22, 27, 34),
                    ForeColor = Theme.Text,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = Theme.Body,
                    Width = card.Width - 80,
                    Location = new Point(12, y + 32)
                };
                tb.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                tb.GotFocus += delegate { tb.BackColor = Color.FromArgb(28, 33, 41); };
                tb.LostFocus += delegate { tb.BackColor = Color.FromArgb(22, 27, 34); };
                return tb;
            };

            // Hero ID
            var lblHeroId = mkLabel("Hero ID");
            card.Controls.Add(lblHeroId);
            txtHeroId = mkInput();
            txtHeroId.MaxLength = 4;
            txtHeroId.KeyPress += delegate (object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };
            card.Controls.Add(txtHeroId);
            y = txtHeroId.Bottom + gap;

            // Name
            var lblName = mkLabel("Name");
            card.Controls.Add(lblName);
            txtName = mkInput();
            card.Controls.Add(txtName);
            y = txtName.Bottom + gap;

            // Age
            var lblAge = mkLabel("Age");
            card.Controls.Add(lblAge);
            txtAge = mkInput();
            txtAge.MaxLength = 3;
            txtAge.KeyPress += delegate (object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };
            card.Controls.Add(txtAge);
            y = txtAge.Bottom + gap;

            // Superpower
            var lblPower = mkLabel("Superpower");
            card.Controls.Add(lblPower);
            txtPower = mkInput();
            card.Controls.Add(txtPower);
            y = txtPower.Bottom + gap;

            // Exam Score
            var lblScore = mkLabel("Hero Exam Score");
            card.Controls.Add(lblScore);
            txtScore = mkInput();
            txtScore.MaxLength = 5;
            txtScore.KeyPress += delegate (object sender, KeyPressEventArgs e)
            {
                char dec = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != dec) e.Handled = true;
                if (e.KeyChar == dec && (sender as TextBox).Text.IndexOf(dec) > -1) e.Handled = true;
            };
            card.Controls.Add(txtScore);
            y = txtScore.Bottom + gap;

            // Buttons row

            btnAdd = new Button
            {
                Text = "Add Superhero",
                Font = Theme.H2,
                ForeColor = Color.White,
                BackColor = Theme.Accent,
                FlatStyle = FlatStyle.Flat,
                Height = 44,
                Width = 180,
                Location = new Point(12, y)
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += OnAddClicked;
            card.Controls.Add(btnAdd);

            btnBack = new Button
            {
                Text = "\u2190 Back",
                Font = Theme.Body,
                ForeColor = Theme.Text,
                BackColor = Color.FromArgb(22, 27, 34),
                FlatStyle = FlatStyle.Flat,
                Height = 44,
                Width = 120,
                Location = new Point(btnAdd.Right + 12, y)
            };
            btnBack.FlatAppearance.BorderSize = 1;
            btnBack.FlatAppearance.BorderColor = Theme.Border;
            btnBack.Click += (s,e)=> { var home = new FormHome(); home.Show(); this.Hide(); };
            card.Controls.Add(btnBack);

            AcceptButton = btnAdd;

            // Resize widths with card
            card.SizeChanged += delegate
            {
                foreach (Control c in card.Controls)
                {
                    var tb = c as TextBox;
                    if (tb != null) tb.Width = card.Width - 80;
                }
            };
        }

        private void OnAddClicked(object sender, EventArgs e)
        {
            // Validate
            string heroIdText = (txtHeroId.Text ?? "").Trim();
            int heroId;
            if (!int.TryParse(heroIdText, out heroId) || heroId < 1000 || heroId > 9999)
            {
                MessageBox.Show("Hero ID must be a 4-digit number (1000-9999).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHeroId.Focus(); return;
            }
            string name  = (txtName.Text  ?? "").Trim();
            string power = (txtPower.Text ?? "").Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus(); return;
            }

            int age;
            if (!int.TryParse((txtAge.Text ?? "").Trim(), out age) || age < 10 || age > 100)
            {
                MessageBox.Show("Age must be a number between 10 and 100.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAge.Focus(); return;
            }

            double score;
            if (!double.TryParse((txtScore.Text ?? "").Trim(), NumberStyles.Float, CultureInfo.CurrentCulture, out score) ||
                score < 0.0 || score > 100.0)
            {
                MessageBox.Show("Hero Exam Score must be between 0 and 100.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtScore.Focus(); return;
            }

            // Rank auto-generated and corresponding threat level
            string rank = CalculateRank(score);
            string threat = CalculateThreat(rank);

            try
            {
                string dataDir = Path.Combine(Application.StartupPath, "Data");
                Directory.CreateDirectory(dataDir);
                string file = Path.Combine(dataDir, "superheroes.txt");

                if (!File.Exists(file))
                {
                    using (var writer = new StreamWriter(file))
                    {
                        writer.WriteLine("Id|Name|Age|Power|Score|Rank|ThreatLevel");
                    }
                }

                string line = string.Format(CultureInfo.InvariantCulture,
                    "{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                    heroId, name, age, power, score, rank, threat);

                using (var writer = new StreamWriter(file, true))
                {
                    writer.WriteLine(line);
                }

                MessageBox.Show(
                    "Superhero added successfully!" + Environment.NewLine +
                    "Hero ID: " + heroId + Environment.NewLine +
                    "Rank: " + rank,
                    "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save hero.\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string CalculateRank(double s)
        {
            if (s >= 81) return "S";
            if (s >= 61) return "A";
            if (s >= 41) return "B";
            return "C";
        }

        private string CalculateThreat(string rank)
        {
            switch ((rank ?? "").ToUpperInvariant())
            {
                case "S": return "FinalsWeek";
                case "A": return "MidtermMadness";
                case "B": return "GroupProjectGoneWrong";
                default: return "PopQuiz";
            }
        }

        private void ClearForm()
        {
            txtHeroId.Text = "";
            txtName.Text = "";
            txtAge.Text = "";
            txtPower.Text = "";
            txtScore.Text = "";
            txtHeroId.Focus();
        }

        private void FormAddHero_Load(object sender, EventArgs e)
        {

        }
    }
}
