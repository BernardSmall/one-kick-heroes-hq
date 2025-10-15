using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OneKickHeroesApp
{
    public partial class FormUpdateHero : Form
    {
        // --- Theme ---
        private static class Theme
        {
            public static readonly Color Bg = ColorTranslator.FromHtml("#0d1117");
            public static readonly Color Surface = ColorTranslator.FromHtml("#161b22");
            public static readonly Color Border = ColorTranslator.FromHtml("#30363d");
            public static readonly Color Accent = ColorTranslator.FromHtml("#1f6feb");
            public static readonly Color Text = ColorTranslator.FromHtml("#c9d1d9");
            public static readonly Color Muted = ColorTranslator.FromHtml("#8b949e");

            public static Font H1 { get { return new Font("Segoe UI", 16, FontStyle.Bold); } }
            public static Font H2 { get { return new Font("Segoe UI", 13, FontStyle.Bold); } }
            public static Font Body { get { return new Font("Segoe UI", 10, FontStyle.Regular); } }
        }

        // Inputs
        private TextBox txtId;
        private Button btnLoad;
        private TextBox txtName;
        private TextBox txtAge;
        private TextBox txtPower;
        private TextBox txtScore;
        private TextBox txtRank;
        private Button btnSave;

        public FormUpdateHero()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            // Form basics
            Text = "Update Superhero";
            BackColor = Theme.Bg;
            ForeColor = Theme.Text;
            MinimumSize = new Size(760, 560);
            StartPosition = FormStartPosition.CenterParent;
            Padding = new Padding(22);
            DoubleBuffered = true;

            // Header
            var title = new Label
            {
                Text = "Update Superhero",
                Font = Theme.H1,
                ForeColor = Theme.Text,
                AutoSize = true,
                Location = new Point(36, 36)
            };
            Controls.Add(title);

            // Card container
            var card = new Panel
            {
                BackColor = Theme.Surface,
                Width = ClientSize.Width - 72,
                Height = ClientSize.Height - 120,
                Location = new Point(30, 90),
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
            int y = 10;
            int gap = 18;

            Func<string, Label> mkLabel = (text) => new Label
            {
                Text = text,
                ForeColor = Theme.Text,
                Font = Theme.H2,
                AutoSize = true,
                Location = new Point(12, y)
            };

            Func<int, TextBox> mkInput = (topOffset) =>
            {
                var tb = new TextBox
                {
                    BackColor = Color.FromArgb(22, 27, 34),
                    ForeColor = Theme.Text,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = Theme.Body,
                    Width = card.Width - 80,
                    Location = new Point(12, topOffset)
                };
                tb.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                tb.GotFocus += delegate { tb.BackColor = Color.FromArgb(28, 33, 41); };
                tb.LostFocus += delegate { tb.BackColor = Color.FromArgb(22, 27, 34); };
                return tb;
            };

            // --- Hero ID + Load button row ---
            var lblId = mkLabel("Hero ID");
            card.Controls.Add(lblId);
            txtId = mkInput(y + 28);
            txtId.Width = card.Width - 80 - 120; // leave space for Load
            card.Controls.Add(txtId);

            btnLoad = new Button
            {
                Text = "Load",
                Font = Theme.Body,
                ForeColor = Color.White,
                BackColor = Theme.Accent,
                FlatStyle = FlatStyle.Flat,
                Height = txtId.Height + 2,
                Width = 100,
                Location = new Point(txtId.Right + 12, txtId.Top - 1),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnLoad.FlatAppearance.BorderSize = 0;
            btnLoad.Click += delegate { LoadById(); };
            card.Controls.Add(btnLoad);

            y = txtId.Bottom + gap;

            // Name
            var lblName = mkLabel("Name"); lblName.Top = y;
            card.Controls.Add(lblName);
            txtName = mkInput(y + 28);
            card.Controls.Add(txtName);
            y = txtName.Bottom + gap;

            // Age
            var lblAge = mkLabel("Age"); lblAge.Top = y;
            card.Controls.Add(lblAge);
            txtAge = mkInput(y + 28);
            txtAge.MaxLength = 3;
            txtAge.KeyPress += delegate (object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };
            card.Controls.Add(txtAge);
            y = txtAge.Bottom + gap;

            // Superpower
            var lblPower = mkLabel("Superpower"); lblPower.Top = y;
            card.Controls.Add(lblPower);
            txtPower = mkInput(y + 28);
            card.Controls.Add(txtPower);
            y = txtPower.Bottom + gap;

            // Score + Rank two-column row
            var lblScore = mkLabel("Hero Exam Score"); lblScore.Top = y;
            card.Controls.Add(lblScore);

            var lblRank = new Label
            {
                Text = "Rank",
                ForeColor = Theme.Text,
                Font = Theme.H2,
                AutoSize = true,
                Location = new Point(card.Width / 2 + 12, y)
            };
            lblRank.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            card.Controls.Add(lblRank);

            txtScore = mkInput(y + 28);
            txtScore.Width = (card.Width - 80) / 2 - 12;
            txtScore.TextChanged += delegate { UpdateRankPreview(); };
            txtScore.KeyPress += delegate (object sender, KeyPressEventArgs e)
            {
                char dec = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != dec) e.Handled = true;
                if (e.KeyChar == dec && (sender as TextBox).Text.IndexOf(dec) > -1) e.Handled = true;
            };
            card.Controls.Add(txtScore);

            txtRank = mkInput(y + 28);
            txtRank.ReadOnly = true;
            txtRank.TabStop = false;
            txtRank.Location = new Point(card.Width / 2 + 12, y + 28);
            txtRank.Width = (card.Width - 80) / 2 - 12;
            txtRank.BackColor = Color.FromArgb(28, 33, 41);
            card.Controls.Add(txtRank);

            y = txtScore.Bottom + 28;

            // Save button
            btnSave = new Button
            {
                Text = "Save",
                Font = Theme.H2,
                ForeColor = Color.White,
                BackColor = Theme.Accent,
                FlatStyle = FlatStyle.Flat,
                Height = 44,
                Width = 220,
                Location = new Point(12, y)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += OnSaveClicked;
            card.Controls.Add(btnSave);

            // Resizing for 2-column row
            card.SizeChanged += delegate
            {
                int colW = (card.Width - 80) / 2 - 12;
                txtScore.Width = colW;
                txtRank.Width = colW;
                txtRank.Left = card.Width / 2 + 12;
                lblRank.Left = card.Width / 2 + 12;

                txtId.Width = card.Width - 80 - 120;
                btnLoad.Left = txtId.Right + 12;

                foreach (Control c in card.Controls)
                {
                    var tb = c as TextBox;
                    if (tb != null && tb != txtScore && tb != txtRank && tb != txtId)
                        tb.Width = card.Width - 80;
                }
            };
        }

        // -------- Logic --------
        private void LoadById()
        {
            int id;
            if (!TryParseId(txtId.Text, out id))
            {
                MessageBox.Show("Enter a valid numeric Hero ID (e.g., 119 or #H-0119).",
                    "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtId.Focus(); return;
            }

            string file = EnsureCsv();
            var rec = File.ReadAllLines(file)
                          .Skip(1)
                          .Select(l => l.Split(','))
                          .FirstOrDefault(p => p.Length >= 6 && SafeInt(p[0]) == id);

            if (rec == null)
            {
                MessageBox.Show("Hero not found.", "Not found",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txtName.Text = UnescapeCsv(rec[1]);
            txtAge.Text = rec[2];
            txtPower.Text = UnescapeCsv(rec[3]);
            txtScore.Text = rec[4];
            txtRank.Text = rec[5];
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            int id;
            if (!TryParseId(txtId.Text, out id))
            {
                MessageBox.Show("Enter a valid numeric Hero ID (e.g., 119 or #H-0119).",
                    "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtId.Focus(); return;
            }

            string name = (txtName.Text ?? "").Trim();
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

            string rank = CalculateRank(score);
            txtRank.Text = rank;

            string file = EnsureCsv();
            var lines = File.ReadAllLines(file).ToList();

            // find row index for this ID
            int idx = -1;
            for (int i = 1; i < lines.Count; i++)
            {
                var parts = lines[i].Split(',');
                if (parts.Length >= 1 && SafeInt(parts[0]) == id)
                {
                    idx = i; break;
                }
            }

            if (idx == -1)
            {
                MessageBox.Show("Hero not found. Load it first or verify the ID.",
                    "Not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string newline = string.Format(CultureInfo.InvariantCulture,
                "{0},{1},{2},{3},{4},{5}",
                id, EscapeCsv(name), age, EscapeCsv(power), score, rank);

            lines[idx] = newline;
            File.WriteAllLines(file, lines.ToArray());

            MessageBox.Show("Hero updated.", "Saved",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateRankPreview()
        {
            double s;
            if (double.TryParse((txtScore.Text ?? "").Trim(), NumberStyles.Float,
                CultureInfo.CurrentCulture, out s))
            {
                txtRank.Text = CalculateRank(s);
            }
            else
            {
                txtRank.Text = "";
            }
        }

        private string CalculateRank(double s)
        {
            if (s >= 85) return "S";
            if (s >= 75) return "A";
            if (s >= 60) return "B";
            return "C";
        }

        private static string EscapeCsv(string value)
        {
            if (value == null) return "";
            bool needQuotes = value.Contains(",") || value.Contains("\"") || value.Contains("\n");
            string v = value.Replace("\"", "\"\"");
            return needQuotes ? "\"" + v + "\"" : v;
        }

        private static string UnescapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            value = value.Trim();
            if (value.StartsWith("\"") && value.EndsWith("\""))
                value = value.Substring(1, value.Length - 2).Replace("\"\"", "\"");
            return value;
        }

        private static bool TryParseId(string raw, out int id)
        {
            id = 0;
            if (string.IsNullOrWhiteSpace(raw)) return false;

            // Accept: "119", "H-0119", "#H-0119"
            var digits = new string(raw.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, NumberStyles.Integer, CultureInfo.InvariantCulture, out id);
        }

        private static int SafeInt(string s)
        {
            int v; return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out v) ? v : -1;
        }

        private static string EnsureCsv()
        {
            string dataDir = Path.Combine(Application.StartupPath, "Data");
            Directory.CreateDirectory(dataDir);
            string file = Path.Combine(dataDir, "heroes.csv");
            if (!File.Exists(file))
                File.WriteAllText(file, "Id,Name,Age,Power,Score,Rank" + Environment.NewLine);
            return file;
        }

        private void FormUpdateHero_Load(object sender, EventArgs e)
        {

        }
    }
}
