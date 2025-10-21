using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace OneKickHeroesApp.Presentation.Forms
{
    public partial class FromViewAll : Form
    {
        public FromViewAll()
        {
            InitializeComponent();
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "superheroes.txt";

                if (!File.Exists(filePath))
                {
                    MessageBox.Show("No superhero records found.", "File Missing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length == 0)
                {
                    MessageBox.Show("No superheroes to display.", "Empty File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                List<Superhero> heroes = new List<Superhero>();

                foreach (string line in lines)
                {
                    // Each record should be: HeroID,Name,Age,Superpower,ExamScore,Rank,ThreatLevel
                    string[] data = line.Split(',');

                    if (data.Length == 7)
                    {
                        heroes.Add(new Superhero
                        {
                            HeroID = data[0],
                            Name = data[1],
                            Age = data[2],
                            Superpower = data[3],
                            ExamScore = data[4],
                            Rank = data[5],
                            ThreatLevel = data[6]
                        });
                    }
                }

                dgvSuperheroes.DataSource = null; // Clear old data
                dgvSuperheroes.DataSource = heroes; // Show the superheroes
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading superheroes: {ex.Message}");
            }

            public class Superhero
        {
            public string HeroID { get; set; }
            public string Name { get; set; }
            public string Age { get; set; }
            public string Superpower { get; set; }
            public string ExamScore { get; set; }
            public string Rank { get; set; }
            public string ThreatLevel { get; set; }
        }
    }
    }
}
