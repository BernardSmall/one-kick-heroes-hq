namespace OneKickHeroesApp.Presentation.Forms
{
    partial class FromViewAll
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvSuperheroes = new System.Windows.Forms.DataGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnViewAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuperheroes)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSuperheroes
            // 
            this.dgvSuperheroes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSuperheroes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSuperheroes.Location = new System.Drawing.Point(0, 0);
            this.dgvSuperheroes.Name = "dgvSuperheroes";
            this.dgvSuperheroes.RowTemplate.Height = 24;
            this.dgvSuperheroes.Size = new System.Drawing.Size(800, 450);
            this.dgvSuperheroes.TabIndex = 0;
            // 
            // btnViewAll
            // 
            this.btnViewAll.Location = new System.Drawing.Point(293, 238);
            this.btnViewAll.Name = "btnViewAll";
            this.btnViewAll.Size = new System.Drawing.Size(153, 23);
            this.btnViewAll.TabIndex = 1;
            this.btnViewAll.Text = "View All Superheroes";
            this.btnViewAll.UseVisualStyleBackColor = true;
            this.btnViewAll.Click += new System.EventHandler(this.btnViewAll_Click);
            // 
            // FromViewAll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnViewAll);
            this.Controls.Add(this.dgvSuperheroes);
            this.Name = "FromViewAll";
            this.Text = "FromViewAll";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuperheroes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSuperheroes;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnViewAll;
    }
}