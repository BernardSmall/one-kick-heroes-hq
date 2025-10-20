// FormAddHero.cs
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OneKickHeroesApp
{
    public partial class FormAddHero : Form
    {
        public FormAddHero()
        {
            InitializeComponent();
        }

        // Optional: fine-tune theme on Load (focus colors, borders, etc.)
        private void FormAddHero_Load(object sender, EventArgs e)
        {
            // Subtle border tint for the card
            this.card.BackColor = ColorTranslator.FromHtml("#161b22");
            this.card.ForeColor = ColorTranslator.FromHtml("#c9d1d9");

            // Focus glow simulation: handle Enter/Leave for textboxes
            WireFocusColors(txtName);
            WireFocusColors(txtAge);
            WireFocusColors(txtPower);
            WireFocusColors(txtScore);
        }

        private void WireFocusColors(TextBox tb)
        {
            tb.Enter += (s, ev) => tb.BackColor = Color.FromArgb(28, 33, 41);
            tb.Leave += (s, ev) => tb.BackColor = Color.FromArgb(22, 27, 34);
        }

        // --- Events wired by Designer ---
        private void txtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtScore_KeyPress(object sender, KeyPressEventArgs e)
        {
            char dec = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != dec)
            {
                e.Handled = true;
                return;
            }
            if (e.KeyChar == dec && (sender as TextBox).Text.IndexOf(dec) > -1)
            {
                e.Handled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate
            string name = (txtName.Text ?? "").Trim();
            string power = (txtPower.Text ?? "").Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Name is required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (!int.TryParse((txtAge.Text ?? "").Trim(), out int age) || age < 10 || age > 100)
            {
                MessageBox.Show("Age must be a number between 10 and 100.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAge.Focus();
                return;
            }

            if (!double.TryParse((txtScore.Text ?? "").Trim(), NumberStyles.Float,
                CultureInfo.CurrentCulture, out double score) || score < 0 || score > 100)
            {
                MessageBox.Show("Hero Exam Score must be between 0 and 100.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtScore.Focus();
                return;
            }

            // Rank from combo or auto
            string rank = cboRank.SelectedIndex > 0 ? (string)cboRank.SelectedItem : CalculateRank(score);

            try
            {
                // Save to superheroes.txt in main project folder
                string file = Path.Combine(Application.StartupPath, @"..\..\superheroes.txt");

                if (!File.Exists(file))
                    File.WriteAllText(file, "Id,Name,Age,Power,Score,Rank" + Environment.NewLine);

                int nextId = GetNextId(file);

                string line = string.Format(CultureInfo.InvariantCulture,
                    "{0},{1},{2},{3},{4},{5}",
                    nextId, EscapeCsv(name), age, EscapeCsv(power), score, rank);

                File.AppendAllText(file, line + Environment.NewLine);

                MessageBox.Show(
                    "Superhero added successfully!" + Environment.NewLine +
                    "Assigned ID: " + nextId + Environment.NewLine +
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

        // --- Helpers (same logic you had) ---
        private int GetNextId(string file)
        {
            try
            {
                var ids = File.ReadAllLines(file)
                    .Skip(1)
                    .Select(l => l.Split(','))
                    .Where(parts => parts.Length > 0)
                    .Select(parts =>
                    {
                        if (int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int v))
                            return (int?)v;
                        return null;
                    })
                    .Where(v => v.HasValue)
                    .Select(v => v.Value);

                return ids.Any() ? ids.Max() + 1 : 1;
            }
            catch
            {
                return (int)(DateTime.UtcNow.Ticks % int.MaxValue);
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

        private void ClearForm()
        {
            txtName.Text = "";
            txtAge.Text = "";
            txtPower.Text = "";
            txtScore.Text = "";
            cboRank.SelectedIndex = 0;
            txtName.Focus();
        }
    }
}

