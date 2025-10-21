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
    public partial class FormDeleteHero : Form
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
            public static readonly Color Danger = Color.FromArgb(208, 64, 64);

            public static Font H1 { get { return new Font("Segoe UI", 16, FontStyle.Bold); } }
            public static Font H2 { get { return new Font("Segoe UI", 13, FontStyle.Bold); } }
            public static Font Body { get { return new Font("Segoe UI", 10, FontStyle.Regular); } }
        }

        private DataGridView grid;
        private Button btnDelete;
        private Button btnBack;
        private Label banner;
        private HeroService _heroService;

        public FormDeleteHero()
        {
            InitializeComponent();
            _heroService = new HeroService();
            BuildUI();
            LoadData();
        }

        // ---------- UI ----------
        private void BuildUI()
        {
            Text = "Delete Superhero";
            BackColor = Theme.Bg;
            ForeColor = Theme.Text;
            MinimumSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterParent;
            Padding = new Padding(22);
            DoubleBuffered = true;

            var title = new Label
            {
                Text = "Delete Superhero",
                Font = Theme.H1,
                ForeColor = Theme.Text,
                AutoSize = true,
                Location = new Point(36, 24)
            };
            Controls.Add(title);

            var subtitle = new Label
            {
                Text = "Select a superhero to delete from the list below.",
                Font = Theme.Body,
                ForeColor = Theme.Muted,
                AutoSize = true,
                Location = new Point(38, 58)
            };
            Controls.Add(subtitle);

            // Grid
            grid = new DataGridView
            {
                Location = new Point(30, 96),
                Size = new Size(ClientSize.Width - 60, ClientSize.Height - 220),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                BackgroundColor = Theme.Surface,
                GridColor = Theme.Border,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false
            };

            // Dark style
            grid.ColumnHeadersDefaultCellStyle.BackColor = Theme.Surface;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Theme.Text;
            grid.ColumnHeadersDefaultCellStyle.Font = Theme.Body;
            grid.DefaultCellStyle.BackColor = Color.FromArgb(22, 27, 34);
            grid.DefaultCellStyle.ForeColor = Theme.Text;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(36, 41, 47);
            grid.DefaultCellStyle.SelectionForeColor = Theme.Text;

            // Columns
            var colId = new DataGridViewTextBoxColumn { HeaderText = "Hero ID", Width = 110, ReadOnly = true };
            var colName = new DataGridViewTextBoxColumn { HeaderText = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true };
            var colAge = new DataGridViewTextBoxColumn { HeaderText = "Age", Width = 70, ReadOnly = true };
            var colPow = new DataGridViewTextBoxColumn { HeaderText = "Superpower", Width = 220, ReadOnly = true };
            var colScore = new DataGridViewTextBoxColumn { HeaderText = "Exam Score", Width = 110, ReadOnly = true };
            var colRank = new DataGridViewTextBoxColumn { HeaderText = "Rank", Width = 80, ReadOnly = true };
            var colIdRaw = new DataGridViewTextBoxColumn { Name = "IdRaw", Visible = false }; // numeric id for logic

            grid.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colAge, colPow, colScore, colRank, colIdRaw });
            Controls.Add(grid);

            // Delete button
            btnDelete = new Button
            {
                Text = "Delete Selected Hero",
                Font = Theme.H2,
                ForeColor = Color.White,
                BackColor = Theme.Danger,
                FlatStyle = FlatStyle.Flat,
                Height = 44,
                Width = 200,
                Location = new Point(30, grid.Bottom + 16),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Enabled = false
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += OnDeleteClicked;
            Controls.Add(btnDelete);

            // Back button
            btnBack = new Button
            {
                Text = "← Back to Home",
                Font = Theme.Body,
                ForeColor = Theme.Text,
                BackColor = Color.FromArgb(22, 27, 34),
                FlatStyle = FlatStyle.Flat,
                Height = 44,
                Width = 150,
                Location = new Point(btnDelete.Right + 16, grid.Bottom + 16),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            btnBack.FlatAppearance.BorderSize = 1;
            btnBack.FlatAppearance.BorderColor = Theme.Border;
            btnBack.Click += (s, e) => { var home = new FormHome(); home.Show(); this.Hide(); };
            Controls.Add(btnBack);

            // Success banner (hidden by default)
            banner = new Label
            {
                Text = "✓  Superhero deleted successfully.",
                Font = Theme.Body,
                ForeColor = Theme.Text,
                BackColor = Color.FromArgb(22, 27, 34),
                AutoSize = false,
                Height = 44,
                Width = ClientSize.Width - 60,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
                Location = new Point(30, btnDelete.Bottom + 12),
                Visible = false,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            banner.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(banner);

            // Resize handler to keep grid height right
            this.Resize += delegate
            {
                grid.Size = new Size(ClientSize.Width - 60, ClientSize.Height - 220);
                btnDelete.Top = grid.Bottom + 16;
                btnBack.Top = grid.Bottom + 16;
                banner.Top = btnDelete.Bottom + 12;
                banner.Width = ClientSize.Width - 60;
            };

            // Enable/disable delete button based on selection
            grid.SelectionChanged += (s, e) => btnDelete.Enabled = grid.SelectedRows.Count > 0;
        }

        // ---------- Data ----------
        private void LoadData()
        {
            try
            {
                grid.Rows.Clear();

                var heroes = _heroService.GetAllHeroes()
                    .Where(h => h != null && h.IsValid())
                    .OrderBy(h => h.Id)
                    .ToList();

                foreach (var hero in heroes)
                {
                    grid.Rows.Add(
                        FormatId(hero.Id),           // Hero ID (H-0001 format)
                        hero.Name,                   // Name
                        hero.Age.ToString(),         // Age
                        hero.Power,                  // Superpower
                        hero.Score.ToString("0.#"),  // Exam Score
                        hero.Rank,                   // Rank
                        hero.Id                      // Raw ID for logic
                    );
                }

                banner.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load heroes.\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- Actions ----------
        private void OnDeleteClicked(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a superhero to delete.", "Nothing selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedRow = grid.SelectedRows[0];
            int heroId = (int)selectedRow.Cells["IdRaw"].Value;
            string heroName = selectedRow.Cells[1].Value.ToString(); // Name column

            var confirm = MessageBox.Show(
                $"Are you sure you want to delete {heroName} (ID: {FormatId(heroId)})?",
                "Confirm Delete",
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Warning, 
                MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes) return;

            try
            {
                bool success = _heroService.DeleteHero(heroId);
                
                if (success)
                {
                    // Refresh the grid
                    LoadData();
                    banner.Text = $"✓  {heroName} deleted successfully.";
                    banner.Visible = true;
                }
                else
                {
                    MessageBox.Show($"Failed to delete {heroName}. Hero may not exist.", "Delete Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete hero.\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------- Helpers ----------
        private static string FormatId(int id)
        {
            // H-0001 style
            if (id < 0) return "";
            return "H-" + id.ToString("0000", CultureInfo.InvariantCulture);
        }

        private void FormDeleteHero_Load(object sender, EventArgs e)
        {

        }
    }
}
