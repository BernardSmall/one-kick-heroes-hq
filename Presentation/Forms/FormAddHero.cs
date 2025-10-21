using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OneKickHeroesApp.Data;

namespace OneKickHeroesApp
{
    public partial class FormAddHero : Form
    {
        // --- Theme ---
        private static class Theme
        {
            public static readonly Color Bg      = ColorTranslator.FromHtml("#0d1117");
            public static readonly Color Surface = ColorTranslator.FromHtml("#161b22");
            public static readonly Color Border  = ColorTranslator.FromHtml("#30363d");
            public static readonly Color Accent  = ColorTranslator.FromHtml("#1f6feb");
            public static readonly Color Text    = ColorTranslator.FromHtml("#c9d1d9");
            public static readonly Color Muted   = ColorTranslator.FromHtml("#8b949e");

            public static Font H1  { get { return new Font("Segoe UI", 16, FontStyle.Bold); } }
            public static Font H2  { get { return new Font("Segoe UI", 13, FontStyle.Bold); } }
            public static Font Body{ get { return new Font("Segoe UI", 10, FontStyle.Regular); } }
        }

        // Inputs
        private TextBox txtHeroId;
        private TextBox txtName;
        private TextBox txtAge;
        private TextBox txtPower;
        private TextBox txtScore;
        private Button btnAdd;
        private Button btnBack;
        private Button btnClear;

        // Services
        private readonly HeroService _heroService;
        
        // UI Components
        private ToolTip _tooltip;
        private bool _hasUnsavedChanges = false;

        public FormAddHero()
        {
            InitializeComponent();
            _heroService = new HeroService();
            _tooltip = new ToolTip();
            BuildUI();
            SetupKeyboardShortcuts();
        }

        private void BuildUI()
        {
            // Form basics
            Text = "Add New Superhero";
            BackColor = Theme.Bg;
            ForeColor = Theme.Text;
            MinimumSize = new Size(720, 560);
            StartPosition = FormStartPosition.CenterParent;
            Padding = new Padding(22);
            DoubleBuffered = true;

            // Header
            var title = new Label
            {
                Text = "Add New Superhero",
                Font = Theme.H1,
                ForeColor = Theme.Text,
                AutoSize = true,
                Location = new Point(36, 36)
            };
            Controls.Add(title);

            var subtitle = new Label
            {
                Text = "Enter the details for the new superhero.",
                Font = Theme.Body,
                ForeColor = Theme.Muted,
                AutoSize = true,
                Location = new Point(38, 72)
            };
            Controls.Add(subtitle);

            // Card container
            var card = new Panel
            {
                BackColor = Theme.Surface,
                Width = ClientSize.Width - 72,
                Height = ClientSize.Height - 150,
                Location = new Point(30, 110),
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
            int y = 24;
            int gap = 18;

            Func<string, Label> mkLabel = (text) => new Label
            {
                Text = text,
                ForeColor = Theme.Text,
                Font = Theme.H2,
                AutoSize = true,
                Location = new Point(12, y)
            };

            Func<TextBox> mkInput = () =>
            {
                var tb = new TextBox
                {
                    BackColor = Color.FromArgb(22, 27, 34),
                    ForeColor = Theme.Text,
                    BorderStyle = BorderStyle.FixedSingle,
                    Font = Theme.Body,
                    Width = card.Width - 80,
                    Location = new Point(12, y + 32)
                };
                tb.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                tb.GotFocus += delegate { tb.BackColor = Color.FromArgb(28, 33, 41); };
                tb.LostFocus += delegate { tb.BackColor = Color.FromArgb(22, 27, 34); };
                tb.TextChanged += delegate { _hasUnsavedChanges = true; };
                return tb;
            };

            // Hero ID
            var lblHeroId = mkLabel("Hero ID");
            card.Controls.Add(lblHeroId);
            txtHeroId = mkInput();
            txtHeroId.MaxLength = 4;
            txtHeroId.KeyPress += delegate (object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };
            _tooltip.SetToolTip(txtHeroId, "Enter a unique 4-digit number between 1000-9999");
            card.Controls.Add(txtHeroId);
            y = txtHeroId.Bottom + gap;

            // Name
            var lblName = mkLabel("Name");
            card.Controls.Add(lblName);
            txtName = mkInput();
            _tooltip.SetToolTip(txtName, "Enter the full name of the superhero");
            card.Controls.Add(txtName);
            y = txtName.Bottom + gap;

            // Age
            var lblAge = mkLabel("Age");
            card.Controls.Add(lblAge);
            txtAge = mkInput();
            txtAge.MaxLength = 3;
            txtAge.KeyPress += delegate (object sender, KeyPressEventArgs e)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };
            _tooltip.SetToolTip(txtAge, "Enter the superhero's age between 10 and 100 years");
            card.Controls.Add(txtAge);
            y = txtAge.Bottom + gap;

            // Superpower
            var lblPower = mkLabel("Superpower");
            card.Controls.Add(lblPower);
            txtPower = mkInput();
            _tooltip.SetToolTip(txtPower, "Enter the superhero's special ability or power");
            card.Controls.Add(txtPower);
            y = txtPower.Bottom + gap;

            // Exam Score
            var lblScore = mkLabel("Hero Exam Score");
            card.Controls.Add(lblScore);
            txtScore = mkInput();
            txtScore.MaxLength = 5;
            txtScore.KeyPress += delegate (object sender, KeyPressEventArgs e)
            {
                char dec = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != dec) e.Handled = true;
                if (e.KeyChar == dec && (sender as TextBox).Text.IndexOf(dec) > -1) e.Handled = true;
            };
            _tooltip.SetToolTip(txtScore, "Enter the hero's exam score between 0.0 and 100.0 (decimals allowed)");
            card.Controls.Add(txtScore);
            y = txtScore.Bottom + gap;


            // Buttons
            btnAdd = new Button
            {
                Text = "Add Superhero",
                Font = Theme.H2,
                ForeColor = Color.White,
                BackColor = Theme.Accent,
                FlatStyle = FlatStyle.Flat,
                Height = 44,
                Width = 150,
                Location = new Point(12, y)
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += OnAddClicked;
            _tooltip.SetToolTip(btnAdd, "Save the superhero to the database (Ctrl+S)");
            card.Controls.Add(btnAdd);

            btnClear = new Button
            {
                Text = "Clear Form",
                Font = Theme.Body,
                ForeColor = Theme.Text,
                BackColor = Color.FromArgb(22, 27, 34),
                FlatStyle = FlatStyle.Flat,
                Height = 44,
                Width = 120,
                Location = new Point(180, y)
            };
            btnClear.FlatAppearance.BorderSize = 1;
            btnClear.FlatAppearance.BorderColor = Theme.Border;
            btnClear.Click += OnClearClicked;
            _tooltip.SetToolTip(btnClear, "Clear all form fields (Escape)");
            card.Controls.Add(btnClear);

            btnBack = new Button
            {
                Text = "← Back to Home",
                Font = Theme.Body,
                ForeColor = Theme.Text,
                BackColor = Color.FromArgb(22, 27, 34),
                FlatStyle = FlatStyle.Flat,
                Height = 44,
                Width = 140,
                Location = new Point(310, y)
            };
            btnBack.FlatAppearance.BorderSize = 1;
            btnBack.FlatAppearance.BorderColor = Theme.Border;
            btnBack.Click += OnBackClicked;
            _tooltip.SetToolTip(btnBack, "Return to the home screen (Alt+Left)");
            card.Controls.Add(btnBack);

            AcceptButton = btnAdd;

            // Resize widths with card
            card.SizeChanged += delegate
            {
                foreach (Control c in card.Controls)
                {
                    var tb = c as TextBox;
                    if (tb != null) tb.Width = card.Width - 80;
                }
            };
        }

        private void OnAddClicked(object sender, EventArgs e)
        {
            try
            {
                // Show progress indicator
                SetProgressState(true);

                // Get and validate input data
                var heroData = GetHeroDataFromForm();
                
                // Create Hero object (this will validate all data)
                var hero = new Hero(
                    heroData.Id,
                    heroData.Name,
                    heroData.Age,
                    heroData.Power,
                    heroData.Score
                );

                // Save hero using service
                _heroService.SaveHero(hero);

                // Show success message
                MessageBox.Show(
                    "Superhero added successfully!" + Environment.NewLine +
                    "Hero ID: " + hero.Id + Environment.NewLine +
                    "Rank: " + hero.Rank + Environment.NewLine +
                    "Threat Level: " + hero.ThreatLevel,
                    "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                FocusOnInvalidField(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (ex.Message.Contains("already exists"))
                    txtHeroId.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save hero.\n" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Reset progress indicator
                SetProgressState(false);
            }
        }

        /// <summary>
        /// Gets and validates hero data from the form
        /// </summary>
        /// <returns>Hero data object</returns>
        private (int Id, string Name, int Age, string Power, double Score) GetHeroDataFromForm()
        {
            // Validate Hero ID
            string heroIdText = (txtHeroId.Text ?? "").Trim();
            if (string.IsNullOrEmpty(heroIdText))
                throw new ArgumentException("Hero ID is required. Please enter a valid Hero ID.", nameof(txtHeroId));

            if (!int.TryParse(heroIdText, out int heroId) || heroId < 1000 || heroId > 9999)
                throw new ArgumentException("Hero ID must be a 4-digit number (1000-9999). Please enter a valid Hero ID.", nameof(txtHeroId));

            // Validate Name
            string name = (txtName.Text ?? "").Trim();
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name is required. Please enter the superhero's name.", nameof(txtName));

            // Validate Age
            string ageText = (txtAge.Text ?? "").Trim();
            if (string.IsNullOrEmpty(ageText))
                throw new ArgumentException("Age is required. Please enter the superhero's age.", nameof(txtAge));

            if (!int.TryParse(ageText, out int age) || age < 10 || age > 100)
                throw new ArgumentException("Age must be a number between 10 and 100. Please enter a valid age.", nameof(txtAge));

            // Validate Superpower
            string power = (txtPower.Text ?? "").Trim();
            if (string.IsNullOrEmpty(power))
                throw new ArgumentException("Superpower is required. Please enter the superhero's power.", nameof(txtPower));

            // Validate Hero Exam Score
            string scoreText = (txtScore.Text ?? "").Trim();
            if (string.IsNullOrEmpty(scoreText))
                throw new ArgumentException("Hero Exam Score is required. Please enter a score between 0 and 100.", nameof(txtScore));

            if (!double.TryParse(scoreText, NumberStyles.Float, CultureInfo.CurrentCulture, out double score))
                throw new ArgumentException("Hero Exam Score must be a valid number. Please enter a valid score.", nameof(txtScore));

            return (heroId, name, age, power, score);
        }

        /// <summary>
        /// Focuses on the appropriate field based on the error message
        /// </summary>
        /// <param name="errorMessage">Error message to analyze</param>
        private void FocusOnInvalidField(string errorMessage)
        {
            if (errorMessage.Contains("Hero ID"))
                txtHeroId.Focus();
            else if (errorMessage.Contains("Name"))
                txtName.Focus();
            else if (errorMessage.Contains("Age"))
                txtAge.Focus();
            else if (errorMessage.Contains("Superpower") || errorMessage.Contains("power"))
                txtPower.Focus();
            else if (errorMessage.Contains("Score"))
                txtScore.Focus();
            else
                txtHeroId.Focus(); // Default focus
        }



        /// <summary>
        /// Sets up keyboard shortcuts for the form
        /// </summary>
        private void SetupKeyboardShortcuts()
        {
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.S)
                {
                    OnAddClicked(s, e);
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    OnClearClicked(s, e);
                    e.Handled = true;
                }
                else if (e.Alt && e.KeyCode == Keys.Left)
                {
                    OnBackClicked(s, e);
                    e.Handled = true;
                }
            };
        }

        /// <summary>
        /// Sets the progress state for the form
        /// </summary>
        /// <param name="isLoading">True to show loading state, false to reset</param>
        private void SetProgressState(bool isLoading)
        {
            if (isLoading)
            {
                btnAdd.Enabled = false;
                btnAdd.Text = "Saving...";
                btnAdd.BackColor = Color.Gray;
                btnClear.Enabled = false;
                btnBack.Enabled = false;
            }
            else
            {
                btnAdd.Enabled = true;
                btnAdd.Text = "Add Superhero";
                btnAdd.BackColor = Theme.Accent;
                btnClear.Enabled = true;
                btnBack.Enabled = true;
            }
        }

        /// <summary>
        /// Handles the clear button click with confirmation
        /// </summary>
        private void OnClearClicked(object sender, EventArgs e)
        {
            if (HasUnsavedChanges())
            {
                var result = MessageBox.Show(
                    "Are you sure you want to clear the form? All unsaved changes will be lost.",
                    "Confirm Clear",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;
            }

            ClearForm();
        }

        /// <summary>
        /// Handles the back button click
        /// </summary>
        private void OnBackClicked(object sender, EventArgs e)
        {
            if (HasUnsavedChanges())
            {
                var result = MessageBox.Show(
                    "You have unsaved changes. Are you sure you want to go back?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;
            }

            // Return to home form
            var homeForm = new FormHome();
            homeForm.Show();
            this.Hide();
        }

        /// <summary>
        /// Checks if there are unsaved changes in the form
        /// </summary>
        /// <returns>True if there are unsaved changes</returns>
        private bool HasUnsavedChanges()
        {
            return _hasUnsavedChanges && (
                !string.IsNullOrWhiteSpace(txtHeroId.Text) ||
                !string.IsNullOrWhiteSpace(txtName.Text) ||
                !string.IsNullOrWhiteSpace(txtAge.Text) ||
                !string.IsNullOrWhiteSpace(txtPower.Text) ||
                !string.IsNullOrWhiteSpace(txtScore.Text)
            );
        }

        private void ClearForm()
        {
            txtHeroId.Text = "";
            txtName.Text = "";
            txtAge.Text = "";
            txtPower.Text = "";
            txtScore.Text = "";
            _hasUnsavedChanges = false;
            txtHeroId.Focus();
        }

        private void FormAddHero_Load(object sender, EventArgs e)
        {

        }
    }
}
