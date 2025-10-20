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
        // ------------- THEME (shared) -------------
        private static class Theme
        {
            public static readonly Color Bg = ColorTranslator.FromHtml("#0d1117");
            public static readonly Color Surface = ColorTranslator.FromHtml("#161b22");
            public static readonly Color Border = ColorTranslator.FromHtml("#30363d");
            public static readonly Color Accent = ColorTranslator.FromHtml("#1f6feb");
            public static readonly Color Text = ColorTranslator.FromHtml("#c9d1d9");
            public static readonly Color Muted = ColorTranslator.FromHtml("#8b949e");
            public static readonly Color Success = Color.FromArgb(34, 197, 94);

            public static Font H1 { get { return new Font("Segoe UI", 16, FontStyle.Bold); } }
            public static Font H2 { get { return new Font("Segoe UI", 13, FontStyle.Bold); } }
            public static Font Body { get { return new Font("Segoe UI", 10, FontStyle.Regular); } }
            public static Font Em { get { return new Font("Segoe UI", 12, FontStyle.Bold); } }
        }

        // ------------- UI CONTROLS (fields so logic can use them) -------------
        private Panel card;
        private ComboBox cboId;
        private Button btnLoad, btnSave;
        private Label vName, vAge, vPower, vScore, vRank;

        // ------------- STATE -------------
        private int loadedId = -1;
        private HeroService _heroService;

        public FormHeroSummary()
        {
            InitializeComponent();  // minimal designer; UI built in BuildUI()
            _heroService = new HeroService();
            BuildUI();              // <--- ALL UI lives here
            LoadIds();
        }

        // ======================================================
        // ===============       UI SECTION       ===============
        // ======================================================
        #region UI

        private void BuildUI()
        {
            // Form basics
            Text = "Single Hero Summary";
            BackColor = Theme.Bg;
            ForeColor = Theme.Text;
            MinimumSize = new Size(900, 560);
            StartPosition = FormStartPosition.CenterParent;
            Padding = new Padding(22);
            DoubleBuffered = true;

            var title = new Label
            {
                Text = "Single Hero Summary",
                Font = Theme.H1,
                ForeColor = Theme.Text,
                AutoSize = true,
                Location = new Point(36, 24)
            };
            Controls.Add(title);

            // Card (rounded with border)
            card = new Panel
            {
                BackColor = Theme.Surface,
                Location = new Point(30, 70),
                Size = new Size(ClientSize.Width - 60, ClientSize.Height - 100),
                Padding = new Padding(20),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            card.Paint += (s, e) =>
            {
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
                    using (var pen = new Pen(Theme.Border, 1)) e.Graphics.DrawPath(pen, gp);
                }
            };
            Controls.Add(card);

            // --- Input row: Hero ID + Load + Save ---
            var lblId = new Label
            {
                Text = "Hero ID",
                Font = Theme.H2,
                ForeColor = Theme.Text,
                AutoSize = true,
                Location = new Point(12, 16)
            };
            card.Controls.Add(lblId);

            cboId = new ComboBox
            {
                Font = Theme.Body,
                BackColor = Color.FromArgb(22, 27, 34),
                ForeColor = Theme.Text,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(12, 44),
                Width = card.Width - 12 - 280,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            card.Controls.Add(cboId);

            btnLoad = new Button
            {
                Text = "Load",
                Font = Theme.Body,
                ForeColor = Color.White,
                BackColor = Theme.Accent,
                FlatStyle = FlatStyle.Flat,
                Height = cboId.Height + 4,
                Width = 100,
                Location = new Point(cboId.Right + 12, cboId.Top - 2),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnLoad.FlatAppearance.BorderSize = 0;
            btnLoad.Click += (s, e) => LoadHero();     // <--- wire to logic
            card.Controls.Add(btnLoad);

            btnSave = new Button
            {
                Text = "Save Summary to Txt",
                Font = Theme.Body,
                ForeColor = Color.White,
                BackColor = Theme.Accent,
                FlatStyle = FlatStyle.Flat,
                Height = cboId.Height + 4,
                Width = 170,
                Location = new Point(btnLoad.Right + 12, cboId.Top - 2),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Enabled = false
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += (s, e) => SaveSummary();  // <--- wire to logic
            card.Controls.Add(btnSave);

            // separator
            var sep = new Panel
            {
                BackColor = Theme.Border,
                Height = 1,
                Width = card.Width - 24,
                Location = new Point(12, 90),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            card.Controls.Add(sep);

            // Two-column details layout
            int leftCol = 12;
            int rightCol = card.Width / 2 + 12;

            Func<Point, Label> mkLabel = (pt) => new Label
            {
                Font = Theme.H2,
                ForeColor = Theme.Muted,
                AutoSize = true,
                Location = pt
            };
            Func<Point, Label> mkValue = (pt) => new Label
            {
                Font = Theme.Em,
                ForeColor = Theme.Text,
                AutoSize = true,
                Location = pt
            };

            // Left column
            var lName = mkLabel(new Point(leftCol, 110)); lName.Text = "Name:"; card.Controls.Add(lName);
            vName = mkValue(new Point(leftCol + 120, 110)); card.Controls.Add(vName);

            var lAge = mkLabel(new Point(leftCol, 150)); lAge.Text = "Age:"; card.Controls.Add(lAge);
            vAge = mkValue(new Point(leftCol + 120, 150)); card.Controls.Add(vAge);

            var lPower = mkLabel(new Point(leftCol, 190)); lPower.Text = "Superpower:"; card.Controls.Add(lPower);
            vPower = mkValue(new Point(leftCol + 120, 190));
            vPower.MaximumSize = new Size(card.Width - (leftCol + 140), 0);
            card.Controls.Add(vPower);

            // Right column
            var lScore = mkLabel(new Point(rightCol, 110)); lScore.Text = "Exam Score:"; card.Controls.Add(lScore);
            vScore = mkValue(new Point(rightCol + 160, 110)); card.Controls.Add(vScore);

            var lRank = mkLabel(new Point(rightCol, 150)); lRank.Text = "Rank:"; card.Controls.Add(lRank);
            vRank = mkValue(new Point(rightCol + 160, 150)); card.Controls.Add(vRank);

            // Keep things aligned when resizing
            card.SizeChanged += (s, e) =>
            {
                cboId.Width = card.Width - 12 - 280;
                btnLoad.Left = cboId.Right + 12;
                btnSave.Left = btnLoad.Right + 12;

                sep.Width = card.Width - 24;

                rightCol = card.Width / 2 + 12;
                lScore.Left = rightCol; vScore.Left = rightCol + 160;
                lRank.Left = rightCol; vRank.Left = rightCol + 160;
                vPower.MaximumSize = new Size(card.Width - (leftCol + 140), 0);
            };
        }

        #endregion
        // ================== END UI SECTION ====================


        // ======================================================
        // ============   EVENT HANDLERS / LOGIC   ==============
        // ======================================================
        #region EventHandlers_And_Logic

        /// <summary>Loads hero by selected ID and fills the UI.</summary>
        private void LoadHero()
        {
            if (cboId.SelectedItem == null)
            {
                MessageBox.Show("Select a Hero ID.", "Missing ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboId.Focus(); return;
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

        /// <summary>Saves the currently loaded hero summary to /Data/hero_####_summary.txt</summary>
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

        /// <summary>Helper to push values to UI labels.</summary>
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

        #endregion
        // ================= END EVENT/LOGIC SECTION ============


        // ======================================================
        // =====================  HELPERS  ======================
        // ======================================================
        #region Helpers

        private void LoadIds()
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

        #endregion
        // ===================== END HELPERS ====================
    }
}
