using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OneKickHeroesApp.Data;

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
        private ComboBox cmbHeroId;
        private Button btnLoad;
        private TextBox txtName;
        private TextBox txtAge;
        private TextBox txtPower;
        private TextBox txtScore;
        private Label lblRank;
        private Button btnSave;
        private HeroService heroService;

        public FormUpdateHero()
        {
            InitializeComponent();
            heroService = new HeroService();
            BuildUI();
            LoadHeroIds();
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

            // --- Hero ID Dropdown + Load button row ---
            var lblId = mkLabel("Select Hero ID");
            card.Controls.Add(lblId);

            cmbHeroId = new ComboBox
            {
                BackColor = Color.FromArgb(22, 27, 34),
                ForeColor = Theme.Text,
                Font = Theme.Body,
                Width = card.Width - 80 - 120, // leave space for Load
                Location = new Point(12, y + 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbHeroId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            card.Controls.Add(cmbHeroId);

            btnLoad = new Button
            {
                Text = "Load",
                Font = Theme.Body,
                ForeColor = Color.White,
                BackColor = Theme.Accent,
                FlatStyle = FlatStyle.Flat,
                Height = cmbHeroId.Height + 2,
                Width = 100,
                Location = new Point(cmbHeroId.Right + 12, cmbHeroId.Top - 1),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnLoad.FlatAppearance.BorderSize = 0;
            btnLoad.Click += delegate { LoadSelectedHero(); };
            card.Controls.Add(btnLoad);

            y = cmbHeroId.Bottom + gap;

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

            var lblRankLabel = new Label
            {
                Text = "Calculated Rank",
                ForeColor = Theme.Text,
                Font = Theme.H2,
                AutoSize = true,
                Location = new Point(card.Width / 2 + 12, y)
            };
            lblRankLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            card.Controls.Add(lblRankLabel);

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

            lblRank = new Label
            {
                Text = "",
                ForeColor = Theme.Accent,
                Font = new Font(Theme.Body.FontFamily, 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(card.Width / 2 + 12, y + 28),
                BackColor = Color.FromArgb(28, 33, 41),
                BorderStyle = BorderStyle.FixedSingle,
                Width = (card.Width - 80) / 2 - 12,
                Height = txtScore.Height,
                TextAlign = ContentAlignment.MiddleCenter
            };
            lblRank.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            card.Controls.Add(lblRank);

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
                lblRank.Width = colW;
                lblRank.Left = card.Width / 2 + 12;
                lblRankLabel.Left = card.Width / 2 + 12;

                cmbHeroId.Width = card.Width - 80 - 120;
                btnLoad.Left = cmbHeroId.Right + 12;

                foreach (Control c in card.Controls)
                {
                    var tb = c as TextBox;
                    // Resize all textboxes except the special half-width score field
                    if (tb != null && tb != txtScore)
                        tb.Width = card.Width - 80;
                }
            };
        }

        // -------- Logic --------
        private void LoadHeroIds()
        {
            try
            {
                var heroes = heroService.GetAllHeroes();
                cmbHeroId.Items.Clear();

                foreach (var hero in heroes.OrderBy(h => h.Id))
                {
                    cmbHeroId.Items.Add($"{hero.Id} - {hero.Name}");
                }

                if (cmbHeroId.Items.Count > 0)
                {
                    cmbHeroId.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading heroes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSelectedHero()
        {
            if (cmbHeroId.SelectedItem == null)
            {
                MessageBox.Show("Please select a hero from the dropdown.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var selectedText = cmbHeroId.SelectedItem.ToString();
                var heroId = int.Parse(selectedText.Split(' ')[0]);

                var heroes = heroService.GetAllHeroes();
                var hero = heroes.FirstOrDefault(h => h.Id == heroId);

                if (hero == null)
                {
                    MessageBox.Show("Hero not found.", "Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                txtName.Text = hero.Name;
                txtAge.Text = hero.Age.ToString();
                txtPower.Text = hero.Power;
                txtScore.Text = hero.Score.ToString("F1");
                lblRank.Text = hero.Rank;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading hero: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            if (cmbHeroId.SelectedItem == null)
            {
                MessageBox.Show("Please select a hero to update.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Get the selected hero ID
                var selectedText = cmbHeroId.SelectedItem.ToString();
                var heroId = int.Parse(selectedText.Split(' ')[0]);

                // Validate input fields
                string name = (txtName.Text ?? "").Trim();
                string power = (txtPower.Text ?? "").Trim();

                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(power))
                {
                    MessageBox.Show("Power is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPower.Focus();
                    return;
                }

                if (!int.TryParse((txtAge.Text ?? "").Trim(), out int age) || age < 10 || age > 100)
                {
                    MessageBox.Show("Age must be a number between 10 and 100.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAge.Focus();
                    return;
                }

                if (!double.TryParse((txtScore.Text ?? "").Trim(), NumberStyles.Float, CultureInfo.CurrentCulture, out double score) ||
                    score < 0.0 || score > 100.0)
                {
                    MessageBox.Show("Hero Exam Score must be between 0 and 100.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtScore.Focus();
                    return;
                }

                // Create updated hero object
                var updatedHero = new Hero(heroId, name, age, power, score);

                // Update the hero using HeroService
                if (heroService.UpdateHero(updatedHero))
                {
                    MessageBox.Show("Hero updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the dropdown and reload the current hero
                    LoadHeroIds();
                    LoadSelectedHero();
                }
                else
                {
                    MessageBox.Show("Failed to update hero. Hero may not exist.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating hero: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateRankPreview()
        {
            if (double.TryParse((txtScore.Text ?? "").Trim(), NumberStyles.Float,
                CultureInfo.CurrentCulture, out double score))
            {
                lblRank.Text = CalculateRank(score);
            }
            else
            {
                lblRank.Text = "";
            }
        }

        private string CalculateRank(double score)
        {
            // Use the same logic as the Hero class
            if (score >= 81) return "S";
            if (score >= 61) return "A";
            if (score >= 41) return "B";
            return "C";
        }

        private void FormUpdateHero_Load(object sender, EventArgs e)
        {

        }
    }
}
