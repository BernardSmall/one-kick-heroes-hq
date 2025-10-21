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
        private Label banner;

        public FormDeleteHero()
        {
            InitializeComponent();
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
                ReadOnly = false,
                MultiSelect = true,
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
            var colChk = new DataGridViewCheckBoxColumn { HeaderText = "", Width = 36, ReadOnly = false };
            var colId = new DataGridViewTextBoxColumn { HeaderText = "Hero ID", Width = 110, ReadOnly = true };
            var colName = new DataGridViewTextBoxColumn { HeaderText = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true };
            var colAge = new DataGridViewTextBoxColumn { HeaderText = "Age", Width = 70, ReadOnly = true };
            var colPow = new DataGridViewTextBoxColumn { HeaderText = "Superpower", Width = 220, ReadOnly = true };
            var colScore = new DataGridViewTextBoxColumn { HeaderText = "Exam Score", Width = 110, ReadOnly = true };
            var colRank = new DataGridViewTextBoxColumn { HeaderText = "Rank", Width = 80, ReadOnly = true };
            var colIdRaw = new DataGridViewTextBoxColumn { Name = "IdRaw", Visible = false }; // numeric id for logic

            grid.Columns.AddRange(new DataGridViewColumn[] { colChk, colId, colName, colAge, colPow, colScore, colRank, colIdRaw });
            Controls.Add(grid);

            // Delete button
            btnDelete = new Button
            {
                Text = "Delete",
                Font = Theme.H2,
                ForeColor = Color.White,
                BackColor = Theme.Danger,
                FlatStyle = FlatStyle.Flat,
                Height = 44,
                Width = 220,
                Location = new Point(30, grid.Bottom + 16),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += OnDeleteClicked;
            Controls.Add(btnDelete);

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
                banner.Top = btnDelete.Bottom + 12;
                banner.Width = ClientSize.Width - 60;
            };
        }

        // ---------- Data ----------
        private string EnsureCsv()
        {
            string dataDir = Path.Combine(Application.StartupPath, "Data");
            Directory.CreateDirectory(dataDir);
            string file = Path.Combine(dataDir, "heroes.csv");
            if (!File.Exists(file))
                File.WriteAllText(file, "Id,Name,Age,Power,Score,Rank" + Environment.NewLine);
            return file;
        }

        private void LoadData()
        {
            grid.Rows.Clear();

            string file = EnsureCsv();
            var rows = File.ReadAllLines(file)
                           .Skip(1)
                           .Select(l => l.Split(','))
                           .Where(p => p.Length >= 6)
                           .Select(p => new
                           {
                               IdRaw = SafeInt(p[0]),
                               IdDisp = FormatId(SafeInt(p[0])),
                               Name = UnescapeCsv(p[1]),
                               Age = p[2],
                               Power = UnescapeCsv(p[3]),
                               Score = p[4],
                               Rank = p[5]
                           });

            foreach (var r in rows)
            {
                grid.Rows.Add(false, r.IdDisp, r.Name, r.Age, r.Power, r.Score, r.Rank, r.IdRaw);
            }

            banner.Visible = false;
        }

        // ---------- Actions ----------
        private void OnDeleteClicked(object sender, EventArgs e)
        {
            // Collect selected IDs
            var ids = new List<int>();
            foreach (DataGridViewRow row in grid.Rows)
            {
                var chk = row.Cells[0].Value;
                bool isChecked = chk != null && (bool)chk;
                if (isChecked)
                {
                    int id = (int)row.Cells["IdRaw"].Value;
                    ids.Add(id);
                }
            }

            if (ids.Count == 0)
            {
                MessageBox.Show("Please select at least one superhero to delete.", "Nothing selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string msg = ids.Count == 1 ? "Are you sure you want to delete this superhero?"
                                        : "Are you sure you want to delete the selected superheroes?";
            var confirm = MessageBox.Show(msg, "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (confirm != DialogResult.Yes) return;

            try
            {
                string file = EnsureCsv();
                var lines = File.ReadAllLines(file).ToList();

                // Keep header + rows whose ID is not in ids
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

                File.WriteAllLines(file, kept.ToArray());

                // Refresh grid
                LoadData();
                banner.Visible = true;
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
            int v; return int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out v) ? v : -1;
        }

        private static string UnescapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            value = value.Trim();
            if (value.StartsWith("\"") && value.EndsWith("\""))
                value = value.Substring(1, value.Length - 2).Replace("\"\"", "\"");
            return value;
        }

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
