// DeleteHero.cs

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OneKickHeroesApp
{
    public partial class FormDeleteHero : Form
    {
        // Theme palette (used at runtime for any tweaks)
        private static class Theme
        {
            public static readonly Color Bg = ColorTranslator.FromHtml("#0d1117");
            public static readonly Color Surface = ColorTranslator.FromHtml("#161b22");
            public static readonly Color Border = ColorTranslator.FromHtml("#30363d");
            public static readonly Color Accent = ColorTranslator.FromHtml("#1f6feb");
            public static readonly Color Text = ColorTranslator.FromHtml("#c9d1d9");
            public static readonly Color Muted = ColorTranslator.FromHtml("#8b949e");
            public static readonly Color Danger = Color.FromArgb(208, 64, 64);
        }

        public FormDeleteHero()
        {
            InitializeComponent();
        }

        private void FormDeleteHero_Load(object sender, EventArgs e)
        {
            // Ensure sizing feels right in Designer and at runtime
            this.grid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.grid.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F);

            // Load data now that controls exist
            LoadData();
        }

        // ---------- Data ----------
        private string EnsureCsv()
        {
            string file = Path.Combine(Application.StartupPath, @"..\..\superheroes.txt");
            if (!File.Exists(file))
                File.WriteAllText(file, "Id,Name,Age,Power,Score,Rank" + Environment.NewLine);
            return file;
        }

        private void LoadData()
        {
            this.grid.Rows.Clear();

            string file = EnsureCsv();
            var rows = File.ReadAllLines(file)
                           .Skip(1)
                           .Select(l => l.Split(','))
                           .Where(p => p.Length >= 6)
                           .Select(p => new
                           {
                               IdRaw = SafeInt(p[0]),
                               IdDisp = SafeInt(p[0]).ToString(), // numeric ID display
                               Name = UnescapeCsv(p[1]),
                               Age = p[2],
                               Power = UnescapeCsv(p[3]),
                               Score = p[4],
                               Rank = p[5]
                           });

            foreach (var r in rows)
                this.grid.Rows.Add(false, r.IdDisp, r.Name, r.Age, r.Power, r.Score, r.Rank, r.IdRaw);

            this.banner.Visible = false;
        }

        // ---------- Actions ----------
        private void btnDelete_Click(object sender, EventArgs e)
        {
            var ids = new List<int>();
            foreach (DataGridViewRow row in this.grid.Rows)
            {
                bool isChecked = row.Cells[0].Value is bool b && b;
                if (isChecked && row.Cells["IdRaw"].Value is int id)
                    ids.Add(id);
            }

            if (ids.Count == 0)
            {
                MessageBox.Show("Please select at least one superhero to delete.", "Nothing selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string msg = ids.Count == 1
                ? "Are you sure you want to delete this superhero?"
                : "Are you sure you want to delete the selected superheroes?";

            if (MessageBox.Show(msg, "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return;

            try
            {
                string file = EnsureCsv();
                var lines = File.ReadAllLines(file).ToList();

                var kept = new List<string>();
                if (lines.Count > 0) kept.Add(lines[0]); // header

                for (int i = 1; i < lines.Count; i++)
                {
                    var parts = lines[i].Split(',');
                    if (parts.Length == 0) continue;
                    int id = SafeInt(parts[0]);
                    if (!ids.Contains(id))
                        kept.Add(lines[i]);
                }

                File.WriteAllLines(file, kept);

                LoadData();
                this.banner.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete.\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- Helpers ----------
        private static int SafeInt(string s)
        {
            return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int v) ? v : -1;
        }

        private static string UnescapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            value = value.Trim();
            if (value.StartsWith("\"") && value.EndsWith("\""))
                value = value.Substring(1, value.Length - 2).Replace("\"\"", "\"");
            return value;
        }
    }
}
