using OneKickHeroesApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OneKickHeroesApp
{
    // Must be partial because FormHome.Designer.cs defines another partial of this class.
    public partial class FormHome : Form
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
            public static Font Small { get { return new Font("Segoe UI", 9, FontStyle.Regular); } }
        }

        public FormHome()
        {
            // InitializeComponent is generated in FormHome.Designer.cs
            InitializeComponent();
            BuildUI();
        }

        // ---------- Rounded card helper ----------
        private Panel Card(string title, string subtitle, int w, int h)
        {
            var p = new Panel
            {
                Width = w,
                Height = h,
                BackColor = Theme.Surface,
                Margin = new Padding(16),
                Padding = new Padding(18),
            };

            p.Paint += delegate (object sender, PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                using (var gp = new GraphicsPath())
                {
                    int r = 14;
                    gp.AddArc(0, 0, r, r, 180, 90);
                    gp.AddArc(p.Width - r - 1, 0, r, r, 270, 90);
                    gp.AddArc(p.Width - r - 1, p.Height - r - 1, r, r, 0, 90);
                    gp.AddArc(0, p.Height - r - 1, r, r, 90, 90);
                    gp.CloseAllFigures();

                    p.Region = new Region(gp);
                    using (var pen = new Pen(Theme.Border, 1))
                    {
                        g.DrawPath(pen, gp);
                    }
                }
            };

            if (!string.IsNullOrWhiteSpace(title))
            {
                var lblTitle = new Label
                {
                    Text = title,
                    ForeColor = Theme.Text,
                    Font = Theme.H2,
                    AutoSize = true,
                    Location = new Point(8, 6)
                };
                p.Controls.Add(lblTitle);
            }

            if (!string.IsNullOrWhiteSpace(subtitle))
            {
                var lblSub = new Label
                {
                    Text = subtitle,
                    ForeColor = Theme.Muted,
                    Font = Theme.Body,
                    AutoSize = true,
                    Location = new Point(10, 40)
                };
                p.Controls.Add(lblSub);
            }

            return p;
        }

        // ---------- Link-like label ----------
        private LinkLabel Link(string text, EventHandler onClick)
        {
            var l = new LinkLabel
            {
                Text = text,
                LinkColor = Theme.Text,
                ActiveLinkColor = Theme.Accent,
                VisitedLinkColor = Theme.Text,
                Font = Theme.Body,
                AutoSize = true,
                LinkBehavior = LinkBehavior.HoverUnderline
            };
            l.Click += onClick;
            return l;
        }

        // ---------- Primary nav button ----------
        private Button NavButton(string title, string subtitle, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = title + "\n" + subtitle,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(18, 14, 18, 10),
                BackColor = Theme.Surface,
                ForeColor = Theme.Text,
                FlatStyle = FlatStyle.Flat,
                Font = Theme.Body,
                Height = 88,
                Width = 380,
                Margin = new Padding(16)
            };
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Theme.Border;

            // subtle hover animation
            btn.MouseEnter += delegate { btn.BackColor = Color.FromArgb(28, 33, 41); };
            btn.MouseLeave += delegate { btn.BackColor = Theme.Surface; };

            // rounded
            btn.Paint += delegate (object sender, PaintEventArgs e)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = new GraphicsPath())
                {
                    int r = 12;
                    path.AddArc(0, 0, r, r, 180, 90);
                    path.AddArc(btn.Width - r - 1, 0, r, r, 270, 90);
                    path.AddArc(btn.Width - r - 1, btn.Height - r - 1, r, r, 0, 90);
                    path.AddArc(0, btn.Height - r - 1, r, r, 90, 90);
                    path.CloseAllFigures();
                    btn.Region = new Region(path);
                }
            };

            btn.Click += onClick;
            return btn;
        }

        private void BuildUI()
        {
            // form basics
            Text = "One Kick Heroes HQ";
            BackColor = Theme.Bg;
            Padding = new Padding(18);
            MinimumSize = new Size(980, 680);
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;

            // top bar
            var lblTitle = new Label
            {
                Text = "One Kick Heroes HQ",
                ForeColor = Theme.Text,
                Font = Theme.H1,
                AutoSize = true,
                Location = new Point(4, 8)
            };
            Controls.Add(lblTitle);

            // container layout
            var main = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Location = new Point(0, 56),
                Padding = new Padding(0, 16, 0, 0)
            };
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            Controls.Add(main);

            // ---------- Left column ----------
            var left = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                WrapContents = false,
                AutoScroll = true
            };
            main.Controls.Add(left, 0, 0);

            var welcome = Card("Welcome back, Cadet!", "Manage the Academy’s heroes.", 420, 110);
            left.Controls.Add(welcome);

            left.Controls.Add(NavButton("➕  Add Superhero", "Create a new record",
                delegate { new FormAddHero().ShowDialog(); }));

            left.Controls.Add(NavButton("📝  Update Superhero", "Edit details by ID",
                delegate { new FormUpdateHero().ShowDialog(); }));

            left.Controls.Add(NavButton("🗑️  Delete Superhero", "Remove selected record",
                delegate { new FormDeleteHero().ShowDialog(); }));

            left.Controls.Add(NavButton("📊  Summary", "Totals & averages",
                delegate { new FormSummary().ShowDialog(); }));

            // NEW: Single Hero Summary button (same style)
            left.Controls.Add(NavButton("👤  Single Hero Summary", "View one hero’s details",
                delegate { new FormHeroSummary().ShowDialog(); }));

            // ---------- Right column ----------
            var right = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
                WrapContents = false,
                AutoScroll = true
            };
            main.Controls.Add(right, 1, 0);

            // Stats card
            var stats = Card("Stats Overview", null, 420, 220);
            right.Controls.Add(stats);

            var lblTotal = new Label { Text = "Total Heroes: 128", ForeColor = Theme.Text, Font = Theme.Body, AutoSize = true, Location = new Point(12, 48) };
            var lblAge = new Label { Text = "Avg Age: 24.7", ForeColor = Theme.Text, Font = Theme.Body, AutoSize = true, Location = new Point(12, 72) };
            var lblScore = new Label { Text = "Avg Exam Score  72.4", ForeColor = Theme.Text, Font = Theme.Body, AutoSize = true, Location = new Point(12, 96) };
            var lblRank = new Label { Text = "Rank Distribution", ForeColor = Theme.Text, Font = Theme.Body, AutoSize = true, Location = new Point(12, 125) };

            // tiny rank bar
            var rankBar = new Panel { BackColor = Theme.Border, Width = 380, Height = 8, Location = new Point(12, 148) };
            var segS = new Panel { BackColor = Theme.Text, Width = (int)(rankBar.Width * 0.32), Height = rankBar.Height, Left = 0 };
            var segA = new Panel { BackColor = Theme.Accent, Width = (int)(rankBar.Width * 0.28), Height = rankBar.Height, Left = segS.Right };
            var segB = new Panel { BackColor = Color.FromArgb(180, 180, 120), Width = (int)(rankBar.Width * 0.22), Height = rankBar.Height, Left = segA.Right };
            var segC = new Panel { BackColor = Color.FromArgb(170, 110, 110), Width = rankBar.Width - (segS.Width + segA.Width + segB.Width), Height = rankBar.Height, Left = segB.Right };
            rankBar.Controls.Add(segS);
            rankBar.Controls.Add(segA);
            rankBar.Controls.Add(segB);
            rankBar.Controls.Add(segC);

            stats.Controls.Add(lblTotal);
            stats.Controls.Add(lblAge);
            stats.Controls.Add(lblScore);
            stats.Controls.Add(lblRank);
            stats.Controls.Add(rankBar);

            // Helpful Links card
            var links = Card("Helpful Links", null, 420, 150);
            right.Controls.Add(links);

            var l1 = Link("View All Heroes", delegate { new FormViewAll().ShowDialog(); });
            l1.Location = new Point(12, 52);

            var l2 = Link("Import/Export (optional)", delegate { new FormImportExport().ShowDialog(); });
            l2.Location = new Point(12, 80);

            var l3 = Link("Project GitHub Repo", delegate
            {
                try
                {
                    Process.Start(new ProcessStartInfo { FileName = "https://github.com/", UseShellExecute = true });
                }
                catch
                {
                    MessageBox.Show("Could not open the browser.");
                }
            });
            l3.Location = new Point(12, 108);

            links.Controls.Add(l1);
            links.Controls.Add(l2);
            links.Controls.Add(l3);
        }

        private void FormHome_Load(object sender, EventArgs e)
        {
        }
    }
}
