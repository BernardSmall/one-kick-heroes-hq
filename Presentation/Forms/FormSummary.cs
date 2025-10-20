using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using OneKickHeroesApp.Data;

namespace OneKickHeroesApp
{
    public partial class FormSummary : Form
    {
        // --- Theme (kept only for colors used in logic/UI tweaks if needed) ---
        private static class Theme
        {
            public static readonly System.Drawing.Color Bg = System.Drawing.ColorTranslator.FromHtml("#0d1117");
            public static readonly System.Drawing.Color Surface = System.Drawing.ColorTranslator.FromHtml("#161b22");
            public static readonly System.Drawing.Color Border = System.Drawing.ColorTranslator.FromHtml("#30363d");
            public static readonly System.Drawing.Color Accent = System.Drawing.ColorTranslator.FromHtml("#1f6feb");
            public static readonly System.Drawing.Color Text = System.Drawing.ColorTranslator.FromHtml("#c9d1d9");
            public static readonly System.Drawing.Color Muted = System.Drawing.ColorTranslator.FromHtml("#8b949e");
        }

        private int total = 0;
        private double avgAge = 0;
        private double avgScore = 0;
        private readonly Dictionary<string, int> rankCounts = new Dictionary<string, int>()
        {
            {"S",0}, {"A",0}, {"B",0}, {"C",0}
        };

        public FormSummary()
        {
            InitializeComponent(); // Designer builds the controls
        }

        private void FormSummary_Load(object sender, EventArgs e)
        {
            banner.Visible = false;
            ComputeAndRender();
        }

        private void ComputeAndRender()
        {
            try
            {
                string file = EnsureCsv();
                var rows = File.ReadAllLines(file)
                               .Skip(1)
                               .Select(l => l.Split(','))
                               .Where(p => p.Length >= 6)
                               .Select(p => new
                               {
                                   Age = SafeInt(p[2]),
                                   Score = SafeDouble(p[4]),
                                   Rank = (p[5] ?? "").Trim().ToUpperInvariant()
                               })
                               .ToList();

                total = rows.Count;
                avgAge = rows.Count > 0 ? rows.Average(r => r.Age) : 0;
                avgScore = rows.Count > 0 ? rows.Average(r => r.Score) : 0;

                rankCounts["S"] = rows.Count(r => r.Rank == "S");
                rankCounts["A"] = rows.Count(r => r.Rank == "A");
                rankCounts["B"] = rows.Count(r => r.Rank == "B");
                rankCounts["C"] = rows.Count(r => r.Rank == "C");

                // render
                statTotal.Text = total.ToString(CultureInfo.CurrentCulture);
                statAvgAge.Text = avgAge.ToString("0.#", CultureInfo.CurrentCulture);
                statAvgScore.Text = avgScore.ToString("0.#", CultureInfo.CurrentCulture);

                rankList.Items.Clear();
                AddRankRow("S");
                AddRankRow("A");
                AddRankRow("B");
                AddRankRow("C");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to compute summary.\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddRankRow(string r)
        {
            var item = new ListViewItem(new[]
            {
                r,
                rankCounts[r].ToString(CultureInfo.CurrentCulture)
            });
            rankList.Items.Add(item);
        }

        private void AddRankRowWithPercent(string r, int count, int totalCount)
        {
            string pct = totalCount > 0 ? (" (" + (count * 100.0 / totalCount).ToString("0.#", CultureInfo.CurrentCulture) + "%)") : "";
            var item = new ListViewItem(new[]
            {
                r,
                count.ToString(CultureInfo.CurrentCulture) + pct
            });
            rankList.Items.Add(item);
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                string dir = Path.Combine(Application.StartupPath, "Data");
                Directory.CreateDirectory(dir);
                string path = Path.Combine(dir, "summary.txt");

                var lines = new List<string>();
                lines.Add("One Kick Heroes HQ - Summary");
                lines.Add("----------------------------");
                lines.Add("Generated: " + DateTime.Now.ToString(CultureInfo.CurrentCulture));
                lines.Add("");
                lines.Add("Total Heroes: " + total.ToString(CultureInfo.CurrentCulture));
                lines.Add("Avg Age: " + avgAge.ToString("0.#", CultureInfo.CurrentCulture));
                lines.Add("Avg Exam Score: " + avgScore.ToString("0.#", CultureInfo.CurrentCulture));
                lines.Add("");
                lines.Add("Heroes Per Rank:");
                lines.Add("  S: " + rankCounts["S"].ToString(CultureInfo.CurrentCulture));
                lines.Add("  A: " + rankCounts["A"].ToString(CultureInfo.CurrentCulture));
                lines.Add("  B: " + rankCounts["B"].ToString(CultureInfo.CurrentCulture));
                lines.Add("  C: " + rankCounts["C"].ToString(CultureInfo.CurrentCulture));

                using (var writer = new StreamWriter(path))
                {
                    foreach (var ln in lines)
                    {
                        writer.WriteLine(ln);
                    }
                }
                banner.Text = "✓  Summary saved to summary.txt";
                banner.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save summary.\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- Helpers ----------
        private void EnableAutoRefresh()
        {
            return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int v) ? v : 0;
        }

        private void FormSummary_Load(object sender, EventArgs e)
        {
            return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out double v) ? v : 0.0;
        }
    }
}
