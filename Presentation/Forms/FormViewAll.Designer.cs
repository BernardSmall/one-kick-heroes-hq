namespace OneKickHeroesApp
{
    partial class FormViewAll
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
            this.btnViewAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuperheroes)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSuperheroes
            // 
            this.dgvSuperheroes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSuperheroes.Location = new System.Drawing.Point(32, 152);
            this.dgvSuperheroes.Name = "dgvSuperheroes";
            this.dgvSuperheroes.RowTemplate.Height = 24;
            this.dgvSuperheroes.Size = new System.Drawing.Size(861, 280);
            this.dgvSuperheroes.TabIndex = 0;
            this.dgvSuperheroes.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSuperheroes_CellContentClick);
            // 
            // btnViewAll
            // 
            this.btnViewAll.Location = new System.Drawing.Point(625, 462);
            this.btnViewAll.Name = "btnViewAll";
            this.btnViewAll.Size = new System.Drawing.Size(188, 23);
            this.btnViewAll.TabIndex = 1;
            this.btnViewAll.Text = "View All Superheroes";
            this.btnViewAll.UseVisualStyleBackColor = true;
            this.btnViewAll.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormViewAll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.btnViewAll);
            this.Controls.Add(this.dgvSuperheroes);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormViewAll";
            this.Text = "FormViewAll";
            this.Load += new System.EventHandler(this.FormViewAll_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuperheroes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSuperheroes;
        private System.Windows.Forms.Button btnViewAll;
    }
}