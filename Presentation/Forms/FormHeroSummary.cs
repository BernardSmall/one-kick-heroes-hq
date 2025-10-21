using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OneKickHeroesApp.Data;
using System.Collections.Generic;

namespace OneKickHeroesApp
{
    public partial class FormHeroSummary : Form
    {
        // Theme colors (runtime only)
        private static class Theme
        {
            public static readonly Color Border = ColorTranslator.FromHtml("#30363d");
            public static readonly Color Text = ColorTranslator.FromHtml("#c9d1d9");
            public static readonly Color Success = Color.FromArgb(34, 197, 94);
        }

        private int loadedId = -1;
        private HeroService _heroService;

        public FormHeroSummary()
        {
            InitializeComponent(); // Designer builds all controls
        }

        private void FormHeroSummary_Load(object sender, EventArgs e)
        {
            // ensure long power text wraps nicely
            vPower.MaximumSize = new Size(card.Width - (12 + 140), 0);
            ApplyRightColumnPositions();
        }

        // --- UI events wired by Designer ---
        private void txtId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadHero();
                e.SuppressKeyPress = true;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e) => LoadHero();

        private void btnSave_Click(object sender, EventArgs e) => SaveSummary();

        private void card_SizeChanged(object sender, EventArgs e)
        {
            // keep input + buttons aligned
            txtId.Width = card.Width - 20 - 12 - 280; // padding(20) + left gap(12) + buttons area
            btnLoad.Left = txtId.Right + 12;
            btnSave.Left = btnLoad.Right + 12;
            sep.Width = card.Width - 40; // padding L+R

            vPower.MaximumSize = new Size(card.Width - (12 + 140), 0);
            ApplyRightColumnPositions();
        }

        private void card_Paint(object sender, PaintEventArgs e)
        {
            // rounded card border
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var gp = new GraphicsPath())
            {
                int r = 14;
                gp.AddArc(0, 0, r, r, 180, 90);
                gp.AddArc(card.Width - r - 1, 0, r, r, 270, 90);
                gp.AddArc(card.Width - r - 1, card.Height - r - 1, r, r, 0, 90);
                gp.AddArc(0, card.Height - r - 1, r, r, 90, 90);
                gp.CloseAllFigures();
                card.Region = new Region(gp);
                using (var pen = new Pen(Theme.Border, 1))
                    e.Graphics.DrawPath(pen, gp);
            }
        }

        private void ApplyRightColumnPositions()
        {
            int rightCol = card.Width / 2 + 12;
            lScore.Left = rightCol; vScore.Left = rightCol + 160;
            lRank.Left = rightCol; vRank.Left = rightCol + 160;
        }

        // ======================= Logic =======================
        private void LoadHero()
        {
            if (!TryParseId(txtId.Text, out int id))
            {
                MessageBox.Show("Enter a valid Hero ID (e.g., 128 or H-0128).",
                    "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtId.Focus();
                return;
            }
            int id = (int)((KeyValuePair<int, string>)cboId.SelectedItem).Key;

            var hero = _heroService.GetAllHeroes().FirstOrDefault(h => h.Id == id);
            if (hero == null)
            {
                MessageBox.Show("Hero not found.", "Not found",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                loadedId = -1;
                SetValues("—", "—", "—", "—", "—");
                return;
            }

            loadedId = id;
            SetValues(hero.Name, hero.Age.ToString(CultureInfo.CurrentCulture), hero.Power,
                hero.Score.ToString("0.#", CultureInfo.CurrentCulture), hero.Rank);
            btnSave.Enabled = true;
        }

        private void SaveSummary()
        {
            if (loadedId < 0)
            {
                MessageBox.Show("Load a hero first.", "Nothing to save",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string dir = Path.Combine(Application.StartupPath, "Data");
                Directory.CreateDirectory(dir);
                string path = Path.Combine(dir, $"hero_{loadedId:0000}_summary.txt");

                var lines = new[]
                {
                    "One Kick Heroes HQ - Single Hero Summary",
                    "----------------------------------------",
                    "Generated: " + DateTime.Now.ToString(CultureInfo.CurrentCulture),
                    "",
                    "ID: H-" + loadedId.ToString("0000", CultureInfo.InvariantCulture),
                    "Name: " + vName.Text,
                    "Age: " + vAge.Text,
                    "Superpower: " + vPower.Text,
                    "Exam Score: " + vScore.Text,
                    "Rank: " + vRank.Text
                };

                File.WriteAllLines(path, lines);
                MessageBox.Show("Summary saved to " + Path.GetFileName(path),
                    "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save summary.\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetValues(string name, string age, string power, string score, string rank)
        {
            vName.Text = string.IsNullOrEmpty(name) ? "—" : name;
            vAge.Text = string.IsNullOrEmpty(age) ? "—" : age;
            vPower.Text = string.IsNullOrEmpty(power) ? "—" : power;
            vScore.Text = string.IsNullOrEmpty(score) ? "—" : score;

            var r = (rank ?? "").Trim().ToUpperInvariant();
            vRank.Text = string.IsNullOrEmpty(r) ? "—" : (r == "S" ? "S-Rank" : r + "-Rank");
            vRank.ForeColor = r == "S" ? Theme.Success : Theme.Text;
        }

        // ======================= Helpers ======================
        private static string EnsureCsv()
        {
            string dataDir = Path.Combine(Application.StartupPath, "Data");
            Directory.CreateDirectory(dataDir);
            string file = Path.Combine(dataDir, "heroes.csv");
            if (!File.Exists(file))
                File.WriteAllText(file, "Id,Name,Age,Power,Score,Rank" + Environment.NewLine);
            return file;
        }

        private static bool TryParseId(string raw, out int id)
        {
            id = 0;
            if (string.IsNullOrWhiteSpace(raw)) return false;
            string digits = new string(raw.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, NumberStyles.Integer, CultureInfo.InvariantCulture, out id);
        }

        private static int SafeInt(string s)
        {
            return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int v) ? v : -1;
        }

        private static string UnescapeCsv(string value)
        {
            try
            {
                var heroes = _heroService.GetAllHeroes()
                    .Where(h => h != null && h.IsValid())
                    .OrderBy(h => h.Id)
                    .Select(h => new KeyValuePair<int, string>(h.Id, $"{h.Id} - {h.Name}"))
                    .ToList();
                
                if (heroes.Count == 0)
                {
                    MessageBox.Show("No valid heroes found in the data file.", "No Data", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                cboId.DataSource = heroes;
                cboId.DisplayMember = "Value";
                cboId.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load hero IDs.\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
