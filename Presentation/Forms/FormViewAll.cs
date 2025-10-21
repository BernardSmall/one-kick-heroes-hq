using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace OneKickHeroesApp
{
    public partial class FormViewAll : Form
    {
        // ---- Theme (same palette as FormHome) ----
        private static readonly Color Bg = ColorTranslator.FromHtml("#0d1117");
        private static readonly Color Surface = ColorTranslator.FromHtml("#161b22");
        private static readonly Color Border = ColorTranslator.FromHtml("#30363d");
        private static readonly Color Accent = ColorTranslator.FromHtml("#1f6feb");
        private static readonly Color TextPrimary = ColorTranslator.FromHtml("#c9d1d9");
        private static readonly Color Muted = ColorTranslator.FromHtml("#8b949e");

        private Label header;
        private Panel divider;

        private Panel controlsCard;
        private TextBox txtSearch;
        private ComboBox cboRank;
        private ComboBox cboThreat;

        private DataGridView grid;
        private BindingSource bs;

        public FormViewAll()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            // Form basics
            this.Text = "View All Superheroes";   // use this.Text to avoid name clashes
            StartPosition = FormStartPosition.CenterParent;
            MinimumSize = new Size(1000, 600);
            BackColor = Bg;
            Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            Padding = new Padding(24);
            DoubleBuffered = true;

            // Title
            header = new Label
            {
                Text = "View All Superheroes",
                ForeColor = TextPrimary,
                Font = new Font("Segoe UI Semibold", 20f, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40
            };
            Controls.Add(header);

            // Divider line
            divider = new Panel { Height = 1, Dock = DockStyle.Top, BackColor = Border, Margin = new Padding(0, 12, 0, 16) };
            Controls.Add(divider);

            // Controls "card"
            controlsCard = new Panel
            {
                Dock = DockStyle.Top,
                Height = 92,
                BackColor = Surface,
                Padding = new Padding(16),
                Margin = new Padding(0, 0, 0, 16)
            };
            controlsCard.Paint += PaintRoundedCard;
            Controls.Add(controlsCard);

            // Search box (PlaceholderText not on older frameworks—using normal Text)
            txtSearch = new TextBox
            {
                Text = "Search...",
                ForeColor = TextPrimary,
                BackColor = Bg,
                BorderStyle = BorderStyle.FixedSingle,
                Width = 260,
                Location = new Point(12, 24)
            };
            txtSearch.GotFocus += (s, e) => { if (txtSearch.Text == "Search...") txtSearch.Text = ""; };
            txtSearch.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) txtSearch.Text = "Search..."; };
            txtSearch.TextChanged += (s, e) => ApplyFilter();
            controlsCard.Controls.Add(txtSearch);

            // Rank filter
            cboRank = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                ForeColor = TextPrimary,
                BackColor = Bg,
                FlatStyle = FlatStyle.Flat,
                Width = 180,
                Location = new Point(txtSearch.Right + 20, 24)
            };
            cboRank.Items.AddRange(new object[] { "All Ranks", "S", "A", "B", "C" });
            cboRank.SelectedIndex = 0;
            cboRank.SelectedIndexChanged += (s, e) => ApplyFilter();
            controlsCard.Controls.Add(cboRank);

            // Threat Level filter
            cboThreat = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                ForeColor = TextPrimary,
                BackColor = Bg,
                FlatStyle = FlatStyle.Flat,
                Width = 220,
                Location = new Point(cboRank.Right + 20, 24)
            };
            cboThreat.Items.AddRange(new object[] { "All Threat Levels", "Finals Week", "Midterm Madness", "Group Project", "Pop Quiz" });
            cboThreat.SelectedIndex = 0;
            cboThreat.SelectedIndexChanged += (s, e) => ApplyFilter();
            controlsCard.Controls.Add(cboThreat);

            // Grid container "card"
            var gridCard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Surface,
                Padding = new Padding(1)
            };
            gridCard.Paint += PaintRoundedCard;
            Controls.Add(gridCard);

            // Grid
            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Bg,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = true,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                GridColor = Border,
                EnableHeadersVisualStyles = false
            };

            // Reduce flicker (reflection)
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, grid, new object[] { true });

            // Header style
            grid.ColumnHeadersDefaultCellStyle.BackColor = Surface;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Surface;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = TextPrimary;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10f);

            // Cell style
            grid.DefaultCellStyle.BackColor = Bg;
            grid.DefaultCellStyle.ForeColor = TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#1f2937");
            grid.DefaultCellStyle.SelectionForeColor = TextPrimary;

            // Alternating rows
            grid.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#0f141b");

            // Data + binding
            bs = new BindingSource();
            grid.DataSource = bs;
            grid.CellContentClick += Grid_CellContentClick;

            // Add to card
            gridCard.Controls.Add(grid);

            // Load demo data (swap later for real data)
            LoadData();
            CreateActionColumns();
        }

        private void PaintRoundedCard(object sender, PaintEventArgs e)
        {
            var panel = (Panel)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = new GraphicsPath())
            {
                int r = 12;
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(panel.Width - r - 1, 0, r, r, 270, 90);
                path.AddArc(panel.Width - r - 1, panel.Height - r - 1, r, r, 0, 90);
                path.AddArc(0, panel.Height - r - 1, r, r, 90, 90);
                path.CloseAllFigures();

                panel.Region = new Region(path);
                using (var pen = new Pen(Border, 1))
                    e.Graphics.DrawPath(pen, path);
            }
        }

        private void LoadData()
        {
            // Build a demo table that matches your columns.
            var dt = new DataTable("Heroes");
            dt.Columns.Add("Hero ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Age");
            dt.Columns.Add("Superpower");
            dt.Columns.Add("Exam Score");
            dt.Columns.Add("Rank");
            dt.Columns.Add("Threat Level");

            dt.Rows.Add("H-011", "Guardian", 30, "Strength", 85, "S", "Finals Week");
            dt.Rows.Add("H-022", "Arcanist", 28, "Magic", 74, "A", "Midterm Madness");
            dt.Rows.Add("H-062", "Bolt", 25, "Speed", 62, "B", "Group Project");
            dt.Rows.Add("H-040", "Shadow", 32, "Stealth", 40, "S", "Pop Quiz");
            dt.Rows.Add("H-031", "Quasar Kid", 19, "Energy Manipulation", 91, "S", "Finals Week");

            bs.DataSource = dt;
        }

        private void CreateActionColumns()
        {
            // Avoid duplicates when reloading
            foreach (var name in new[] { "View", "Edit", "Delete" })
            {
                if (grid.Columns.Contains(name))
                    grid.Columns.Remove(name);
            }

            // View link
            var colView = new DataGridViewLinkColumn
            {
                HeaderText = "View",
                Text = "View",
                UseColumnTextForLinkValue = true,
                LinkColor = Accent,
                ActiveLinkColor = Accent,
                VisitedLinkColor = Accent,
                Name = "View",
                Width = 60,
                TrackVisitedState = false
            };

            // Edit link
            var colEdit = new DataGridViewLinkColumn
            {
                HeaderText = "Edit",
                Text = "Edit",
                UseColumnTextForLinkValue = true,
                LinkColor = Accent,
                ActiveLinkColor = Accent,
                VisitedLinkColor = Accent,
                Name = "Edit",
                Width = 60,
                TrackVisitedState = false
            };

            // Delete button
            var colDelete = new DataGridViewButtonColumn
            {
                HeaderText = "Delete",
                Text = "Delete",
                UseColumnTextForButtonValue = true,
                Name = "Delete",
                Width = 80
            };

            // Add at the end
            grid.Columns.Add(colView);
            grid.Columns.Add(colEdit);
            grid.Columns.Add(colDelete);

            // Muted headers for action block
            grid.Columns["View"].HeaderCell.Style.ForeColor = Muted;
            grid.Columns["Edit"].HeaderCell.Style.ForeColor = Muted;
            grid.Columns["Delete"].HeaderCell.Style.ForeColor = Muted;
        }

        private void ApplyFilter()
        {
            var dt = bs != null ? bs.DataSource as DataTable : null;  // C# 7.3 friendly
            if (dt == null) return;

            string q = (txtSearch.Text ?? string.Empty).Trim().Replace("'", "''");
            string r = cboRank.SelectedItem != null ? cboRank.SelectedItem.ToString() : "All Ranks";
            string t = cboThreat.SelectedItem != null ? cboThreat.SelectedItem.ToString() : "All Threat Levels";

            var filters = new System.Collections.Generic.List<string>();

            if (!string.IsNullOrEmpty(q) && q != "Search...")
            {
                filters.Add(
                    $"CONVERT([Hero ID], 'System.String') LIKE '%{q}%' OR " +
                    $"CONVERT([Name], 'System.String') LIKE '%{q}%' OR " +
                    $"CONVERT([Superpower], 'System.String') LIKE '%{q}%' OR " +
                    $"CONVERT([Exam Score], 'System.String') LIKE '%{q}%' OR " +
                    $"CONVERT([Rank], 'System.String') LIKE '%{q}%' OR " +
                    $"CONVERT([Threat Level], 'System.String') LIKE '%{q}%' OR " +
                    $"CONVERT([Age], 'System.String') LIKE '%{q}%'"
                );
            }

            if (r != "All Ranks")
                filters.Add($"[Rank] = '{r.Replace("'", "''")}'");

            if (t != "All Threat Levels")
                filters.Add($"[Threat Level] = '{t.Replace("'", "''")}'");

            bs.Filter = filters.Count == 0 ? "" : string.Join(" AND ", filters);
        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string colName = grid.Columns[e.ColumnIndex].Name;
            var drv = grid.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (drv == null) return;

            var row = drv.Row;
            string id = row["Hero ID"] != null ? row["Hero ID"].ToString() : "";

            if (colName == "View")
            {
                MessageBox.Show("Viewing hero " + id, "View", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (colName == "Edit")
            {
                MessageBox.Show("Editing hero " + id, "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (colName == "Delete")
            {
                var confirm = MessageBox.Show("Delete hero " + id + "?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    grid.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        // Optional: bind a real table later
        public void Bind(DataTable heroesTable)
        {
            bs.DataSource = heroesTable;
            CreateActionColumns();
            ApplyFilter();
        }

        private void FormViewAll_Load(object sender, EventArgs e) { }
    }
}
