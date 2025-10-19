using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OneKickHeroesApp
{
    public partial class FormUpdateHero : Form
    {
        // --- Theme (minimal; useful for future tweaks) ---
        private static class Theme
        {
            public static readonly System.Drawing.Color Bg = System.Drawing.ColorTranslator.FromHtml("#0d1117");
            public static readonly System.Drawing.Color Surface = System.Drawing.ColorTranslator.FromHtml("#161b22");
            public static readonly System.Drawing.Color Border = System.Drawing.ColorTranslator.FromHtml("#30363d");
            public static readonly System.Drawing.Color Accent = System.Drawing.ColorTranslator.FromHtml("#1f6feb");
            public static readonly System.Drawing.Color Text = System.Drawing.ColorTranslator.FromHtml("#c9d1d9");
            public static readonly System.Drawing.Color Muted = System.Drawing.ColorTranslator.FromHtml("#8b949e");
        }

        public FormUpdateHero()
        {
            InitializeComponent();  // Designer builds the UI
        }

        private void FormUpdateHero_Load(object sender, EventArgs e)
        {
            // reserved for runtime tweaks if needed
        }

        // -------- UI Event wrappers (Designer wires to these) --------
        private void btnLoad_Click(object sender, EventArgs e) => LoadById();

        private void btnSave_Click(object sender, EventArgs e) => OnSaveClicked(sender, e);

        private void txtScore_TextChanged(object sender, EventArgs e) => UpdateRankPreview();

        private void txtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private void txtScore_KeyPress(object sender, KeyPressEventArgs e)
        {
            char dec = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != dec) e.Handled = true;
            if (e.KeyChar == dec && (sender as TextBox).Text.IndexOf(dec) > -1) e.Handled = true;
        }

        // -------- Logic (your original code, untouched in spirit) --------
        private void LoadById()
        {
            int id;
            if (!TryParseId(txtId.Text, out id))
            {
                MessageBox.Show("Enter a valid numeric Hero ID (e.g., 119 or H-0119).",
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
                MessageBox.Show("Enter a valid numeric Hero ID (e.g., 119 or H-0119).",
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
                MessageBox.Show("Age must be a number between 10 and 100.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAge.Focus(); return;
            }

            double score;
            if (!double.TryParse((txtScore.Text ?? "").Trim(), NumberStyles.Float, CultureInfo.CurrentCulture, out score) ||
                score < 0.0 || score > 100.0)
            {
                MessageBox.Show("Hero Exam Score must be between 0 and 100.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                { idx = i; break; }
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
            return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int v) ? v : -1;
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
    }
}
