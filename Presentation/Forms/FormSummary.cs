using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OneKickHeroesApp.Data;

namespace OneKickHeroesApp
{
    public partial class FormSummary : Form
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
            public static Font Stat { get { return new Font("Segoe UI", 22, FontStyle.Bold); } }
            public static Font Body { get { return new Font("Segoe UI", 10, FontStyle.Regular); } }
        }

        private Label statTotal;
        private Label statAvgAge;
        private Label statAvgScore;
        private Label statMinScore;
        private Label statMaxScore;
        private ListView rankList;
        private Button btnSave;
        private Button btnRefresh;
        private Label banner;

        private HeroService _heroService;
        private FileSystemWatcher _watcher;

        private int total = 0;
        private double avgAge = 0;
        private double avgScore = 0;
        private readonly Dictionary<string, int> rankCounts = new Dictionary<string, int>()
        {
            {"S",0}, {"A",0}, {"B",0}, {"C",0}
        };

        public FormSummary()
        {
            InitializeComponent();
            _heroService = new HeroService();
            BuildUI();
            ComputeAndRender();
            EnableAutoRefresh();
        }

        // ---------- UI ----------
        private Panel MakeCard(int width, int height)
        {
            var p = new Panel
            {
                Width = width,
                Height = height,
                BackColor = Theme.Surface,
                Padding = new Padding(16)
            };
            p.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var pen = new Pen(Theme.Border, 1)) { e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1); }
            };
            return p;
        }

        private void BuildUI()
        {
            Text = "Summary";
            BackColor = Theme.Bg;
            ForeColor = Theme.Text;
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterParent;
            Padding = new Padding(22);
            DoubleBuffered = true;

            var title = new Label
            {
                Text = "Summary",
                Font = Theme.H1,
                ForeColor = Theme.Text,
                AutoSize = true,
                Location = new Point(36, 24)
            };
            Controls.Add(title);

            var subtitle = new Label
            {
                Text = "Shows a summary of hero data.",
                Font = Theme.Body,
                ForeColor = Theme.Muted,
                AutoSize = true,
                Location = new Point(38, 56)
            };
            Controls.Add(subtitle);

            // Stat cards row
            var cardW = 260; var cardH = 180; var gap = 16; var left = 30;
            var top = 92;

            var cardTotal = MakeCard(cardW, cardH);
            cardTotal.Location = new Point(left, top);
            Controls.Add(cardTotal);

            var cardAge = MakeCard(cardW, cardH);
            cardAge.Location = new Point(left + cardW + gap, top);
            Controls.Add(cardAge);

            var cardScore = MakeCard(cardW, cardH);
            cardScore.Location = new Point(left + 2 * (cardW + gap), top);
            Controls.Add(cardScore);

            // Labels inside cards
            var t1 = new Label { Text = "Total Heroes", Font = Theme.Body, ForeColor = Theme.Muted, AutoSize = true, Location = new Point(8, 8) };
            statTotal = new Label { Text = "0", Font = Theme.Stat, ForeColor = Theme.Text, AutoSize = true, Location = new Point(8, 38) };
            cardTotal.Controls.Add(t1); cardTotal.Controls.Add(statTotal);

            var t2 = new Label { Text = "Avg Age", Font = Theme.Body, ForeColor = Theme.Muted, AutoSize = true, Location = new Point(8, 8) };
            statAvgAge = new Label { Text = "0", Font = Theme.Stat, ForeColor = Theme.Text, AutoSize = true, Location = new Point(8, 38) };
            cardAge.Controls.Add(t2); cardAge.Controls.Add(statAvgAge);

            var t3 = new Label { Text = "Avg Exam Score", Font = Theme.Body, ForeColor = Theme.Muted, AutoSize = true, Location = new Point(8, 8) };
            statAvgScore = new Label { Text = "0", Font = Theme.Stat, ForeColor = Theme.Text, AutoSize = true, Location = new Point(8, 36) };
            // Min/Max stacked with wrapping to avoid clipping
            var t3a = new Label { Text = "Min / Max", Font = Theme.Body, ForeColor = Theme.Muted, AutoSize = true, Location = new Point(8, 78) };
            statMinScore = new Label { Text = "Min: -", Font = Theme.Body, ForeColor = Theme.Text, AutoSize = true, Location = new Point(8, 98) };
            statMaxScore = new Label { Text = "Max: -", Font = Theme.Body, ForeColor = Theme.Text, AutoSize = true, Location = new Point(8, 124) };
            // Allow wrapping within the card width
            statMinScore.MaximumSize = new Size(cardW - 24, 0);
            statMaxScore.MaximumSize = new Size(cardW - 24, 0);
            cardScore.Controls.Add(t3); cardScore.Controls.Add(statAvgScore); cardScore.Controls.Add(t3a); cardScore.Controls.Add(statMinScore); cardScore.Controls.Add(statMaxScore);

            // Rank table
            var rankCard = MakeCard(380, 250);
            rankCard.Location = new Point(left, top + cardH + 24);
            Controls.Add(rankCard);

            var rTitle = new Label { Text = "Heroes Per Rank", Font = Theme.H2, ForeColor = Theme.Text, AutoSize = true, Location = new Point(8, 8) };
            rankCard.Controls.Add(rTitle);

            rankList = new ListView
            {
                View = View.Details,
                FullRowSelect = true,
                HeaderStyle = ColumnHeaderStyle.Nonclickable,
                BorderStyle = BorderStyle.None,
                Location = new Point(8, 42),
                Size = new Size(rankCard.Width - 16, rankCard.Height - 50),
                BackColor = Color.FromArgb(22, 27, 34),
                ForeColor = Theme.Text
            };
            rankList.Columns.Add("Rank", 120, HorizontalAlignment.Left);
            rankList.Columns.Add("Total", 100, HorizontalAlignment.Left);
            rankCard.Controls.Add(rankList);

            // Save button
            btnSave = new Button
            {
                Text = "Save to summary.txt",
                Font = Theme.H2,
                ForeColor = Color.White,
                BackColor = Theme.Accent,
                FlatStyle = FlatStyle.Flat,
                Height = 44,
                Width = 200,
                Location = new Point(rankCard.Right + 40, rankCard.Bottom - 44)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSave.Click += OnSaveClicked;
            Controls.Add(btnSave);

            // Refresh button
            btnRefresh = new Button
            {
                Text = "Refresh",
                Font = Theme.Body,
                ForeColor = Theme.Text,
                BackColor = Color.FromArgb(22, 27, 34),
                FlatStyle = FlatStyle.Flat,
                Height = 44,
                Width = 120,
                Location = new Point(btnSave.Right + 16, rankCard.Bottom - 44)
            };
            btnRefresh.FlatAppearance.BorderSize = 1;
            btnRefresh.FlatAppearance.BorderColor = Theme.Border;
            btnRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRefresh.Click += (s,e) => ComputeAndRender();
            Controls.Add(btnRefresh);

            // Success banner
            banner = new Label
            {
                Text = "✓  Summary saved to summary.txt",
                Font = Theme.Body,
                ForeColor = Theme.Text,
                BackColor = Color.FromArgb(22, 27, 34),
                AutoSize = false,
                Height = 42,
                Width = 320,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
                Location = new Point(btnSave.Left - 20, btnSave.Top - 60),
                Visible = false
            };
            banner.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(banner);

            // Resize behavior
            this.Resize += delegate
            {
                cardTotal.Location = new Point(left, top);
                cardAge.Location = new Point(left + cardW + gap, top);
                cardScore.Location = new Point(left + 2 * (cardW + gap), top);

                rankCard.Location = new Point(left, top + cardH + 24);
                rankList.Size = new Size(rankCard.Width - 16, rankCard.Height - 50);

                btnSave.Location = new Point(rankCard.Right + 40, rankCard.Bottom - 44);
                btnRefresh.Location = new Point(btnSave.Right + 16, rankCard.Bottom - 44);
                banner.Location = new Point(btnSave.Left - 20, btnSave.Top - 60);
            };
        }

        private void ComputeAndRender()
        {
            var heroes = new List<OneKickHeroesApp.Data.Hero>();
            try { heroes = new OneKickHeroesApp.Data.HeroService().GetAllHeroes(); }
            catch { heroes = new List<OneKickHeroesApp.Data.Hero>(); }

            total = heroes.Count;
            avgAge = heroes.Count > 0 ? heroes.Average(h => h.Age) : 0;
            avgScore = heroes.Count > 0 ? heroes.Average(h => h.Score) : 0;

            // reset counts
            var sCount = heroes.Count(h => h.Rank == "S");
            var aCount = heroes.Count(h => h.Rank == "A");
            var bCount = heroes.Count(h => h.Rank == "B");
            var cCount = heroes.Count(h => h.Rank == "C");
            rankCounts["S"] = sCount;
            rankCounts["A"] = aCount;
            rankCounts["B"] = bCount;
            rankCounts["C"] = cCount;

            // min/max (compute scores first to avoid any ordering ambiguity)
            Hero minHero = null;
            Hero maxHero = null;
            if (heroes.Count > 0)
            {
                var minScore = heroes.Min(h => h.Score);
                var maxScore = heroes.Max(h => h.Score);
                minHero = heroes.FirstOrDefault(h => Math.Abs(h.Score - minScore) < 0.000001);
                maxHero = heroes.FirstOrDefault(h => Math.Abs(h.Score - maxScore) < 0.000001);
            }

            // render
            statTotal.Text = total.ToString(CultureInfo.CurrentCulture);
            statAvgAge.Text = avgAge.ToString("0.#", CultureInfo.CurrentCulture);
            statAvgScore.Text = avgScore.ToString("0.#", CultureInfo.CurrentCulture);
            statMinScore.Text = minHero != null ?
                ("Min: " + minHero.Score.ToString("0.#", CultureInfo.CurrentCulture) + " (" + minHero.Id.ToString(CultureInfo.CurrentCulture) + ": " + minHero.Name + ")") : "Min: -";
            statMaxScore.Text = maxHero != null ?
                ("Max: " + maxHero.Score.ToString("0.#", CultureInfo.CurrentCulture) + " (" + maxHero.Id.ToString(CultureInfo.CurrentCulture) + ": " + maxHero.Name + ")") : "Max: -";

            rankList.Items.Clear();
            AddRankRowWithPercent("S", sCount, total);
            AddRankRowWithPercent("A", aCount, total);
            AddRankRowWithPercent("B", bCount, total);
            AddRankRowWithPercent("C", cCount, total);
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

                File.WriteAllLines(path, lines.ToArray());
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
            try
            {
                string dir = Path.Combine(Application.StartupPath, "Data");
                Directory.CreateDirectory(dir);
                _watcher = new FileSystemWatcher(dir, "superheroes.txt");
                _watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName;
                _watcher.Changed += (s, e) => BeginInvoke((Action)ComputeAndRender);
                _watcher.Created += (s, e) => BeginInvoke((Action)ComputeAndRender);
                _watcher.Renamed += (s, e) => BeginInvoke((Action)ComputeAndRender);
                _watcher.EnableRaisingEvents = true;
            }
            catch { /* ignore watcher errors */ }
        }

        private void FormSummary_Load(object sender, EventArgs e)
        {

        }
    }
}
