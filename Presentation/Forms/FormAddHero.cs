// FormAddHero.cs
using System;
using System.Drawing;
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
        public FormAddHero()
        {
            InitializeComponent();
        }

        // Optional: fine-tune theme on Load (focus colors, borders, etc.)
        private void FormAddHero_Load(object sender, EventArgs e)
        {
            // Subtle border tint for the card
            this.card.BackColor = ColorTranslator.FromHtml("#161b22");
            this.card.ForeColor = ColorTranslator.FromHtml("#c9d1d9");

            // Focus glow simulation: handle Enter/Leave for textboxes
            WireFocusColors(txtName);
            WireFocusColors(txtAge);
            WireFocusColors(txtPower);
            WireFocusColors(txtScore);
        }

        private void WireFocusColors(TextBox tb)
        {
            tb.Enter += (s, ev) => tb.BackColor = Color.FromArgb(28, 33, 41);
            tb.Leave += (s, ev) => tb.BackColor = Color.FromArgb(22, 27, 34);
        }

        // --- Events wired by Designer ---
        private void txtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void txtScore_KeyPress(object sender, KeyPressEventArgs e)
        {
            char dec = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != dec)
            {
                e.Handled = true;
                return;
            }
            if (e.KeyChar == dec && (sender as TextBox).Text.IndexOf(dec) > -1)
            {
                e.Handled = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate
            string name = (txtName.Text ?? "").Trim();
            string power = (txtPower.Text ?? "").Trim();

                ClearForm();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Name is required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (!int.TryParse((txtAge.Text ?? "").Trim(), out int age) || age < 10 || age > 100)
            {
                MessageBox.Show("Age must be a number between 10 and 100.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAge.Focus();
                return;
            }

            if (!double.TryParse((txtScore.Text ?? "").Trim(), NumberStyles.Float,
                CultureInfo.CurrentCulture, out double score) || score < 0 || score > 100)
            {
                MessageBox.Show("Hero Exam Score must be between 0 and 100.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtScore.Focus();
                return;
            }
        }

            // Rank from combo or auto
            string rank = cboRank.SelectedIndex > 0 ? (string)cboRank.SelectedItem : CalculateRank(score);

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

        // --- Helpers (same logic you had) ---
        private int GetNextId(string file)
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
                var ids = File.ReadAllLines(file)
                    .Skip(1)
                    .Select(l => l.Split(','))
                    .Where(parts => parts.Length > 0)
                    .Select(parts =>
                    {
                        if (int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int v))
                            return (int?)v;
                        return null;
                    })
                    .Where(v => v.HasValue)
                    .Select(v => v.Value);

                return ids.Any() ? ids.Max() + 1 : 1;
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
            cboRank.SelectedIndex = 0;
            txtName.Focus();
        }
    }
}
