using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OneKickHeroesApp.Data
{
    /// <summary>
    /// Service class for managing Hero data persistence
    /// </summary>
    public class HeroService
    {
        #region Private Fields
        private readonly string _dataDirectory;
        private readonly string _filePath;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the HeroService
        /// </summary>
        /// <param name="dataDirectory">Directory path for data storage</param>
        public HeroService(string dataDirectory = null)
        {
            _dataDirectory = dataDirectory ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _filePath = Path.Combine(_dataDirectory, "superheroes.txt");
            
            // Ensure data directory exists
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Saves a hero to the data file
        /// </summary>
        /// <param name="hero">Hero object to save</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool SaveHero(Hero hero)
        {
            try
            {
                if (hero == null)
                    throw new ArgumentNullException(nameof(hero), "Hero cannot be null.");

                if (!hero.IsValid())
                    throw new ArgumentException("Hero data is invalid.");

                // Check if hero ID already exists
                if (HeroIdExists(hero.Id))
                    throw new InvalidOperationException($"Hero ID {hero.Id} already exists.");

                // Create file with header if it doesn't exist
                if (!File.Exists(_filePath))
                {
                    CreateHeader();
                }

                // Append hero data
                using (var writer = new StreamWriter(_filePath, true))
                {
                    writer.WriteLine(hero.ToString());
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save hero: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves all heroes from the data file
        /// </summary>
        /// <returns>List of Hero objects</returns>
        public List<Hero> GetAllHeroes()
        {
            var heroes = new List<Hero>();

            try
            {
                if (!File.Exists(_filePath))
                    return heroes;

                using (var reader = new StreamReader(_filePath))
                {
                    string line;
                    bool isFirstLine = true;

                    while ((line = reader.ReadLine()) != null)
                    {
                        // Skip header line
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            try
                            {
                                var hero = Hero.FromString(line);
                                heroes.Add(hero);
                            }
                            catch (ArgumentException)
                            {
                                // Skip invalid lines
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read heroes: {ex.Message}", ex);
            }

            return heroes;
        }

        /// <summary>
        /// Checks if a hero ID already exists
        /// </summary>
        /// <param name="heroId">Hero ID to check</param>
        /// <returns>True if exists, false otherwise</returns>
        public bool HeroIdExists(int heroId)
        {
            try
            {
                if (!File.Exists(_filePath))
                    return false;

                using (var reader = new StreamReader(_filePath))
                {
                    string line;
                    bool isFirstLine = true;

                    while ((line = reader.ReadLine()) != null)
                    {
                        // Skip header line
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var parts = line.Split('|');
                            if (parts.Length > 0)
                            {
                                if (int.TryParse(parts[0].Trim(), out int existingId) && existingId == heroId)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Gets the next available hero ID
        /// </summary>
        /// <returns>Next available ID</returns>
        public int GetNextHeroId()
        {
            try
            {
                var heroes = GetAllHeroes();
                return heroes.Any() ? heroes.Max(h => h.Id) + 1 : 1;
            }
            catch
            {
                return 1;
            }
        }

        /// <summary>
        /// Deletes a hero by ID
        /// </summary>
        /// <param name="heroId">Hero ID to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool DeleteHero(int heroId)
        {
            try
            {
                var heroes = GetAllHeroes();
                var heroToDelete = heroes.FirstOrDefault(h => h.Id == heroId);
                
                if (heroToDelete == null)
                    return false;

                heroes.Remove(heroToDelete);
                SaveAllHeroes(heroes);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Updates an existing hero
        /// </summary>
        /// <param name="updatedHero">Updated hero object</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdateHero(Hero updatedHero)
        {
            try
            {
                var heroes = GetAllHeroes();
                var existingHero = heroes.FirstOrDefault(h => h.Id == updatedHero.Id);
                
                if (existingHero == null)
                    return false;

                var index = heroes.IndexOf(existingHero);
                heroes[index] = updatedHero;
                
                SaveAllHeroes(heroes);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates the header for the data file
        /// </summary>
        private void CreateHeader()
        {
            using (var writer = new StreamWriter(_filePath))
            {
                writer.WriteLine("Id|Name|Age|Power|Score|Rank|ThreatLevel");
            }
        }

        /// <summary>
        /// Saves all heroes to the file (overwrites existing file)
        /// </summary>
        /// <param name="heroes">List of heroes to save</param>
        private void SaveAllHeroes(List<Hero> heroes)
        {
            using (var writer = new StreamWriter(_filePath))
            {
                writer.WriteLine("Id|Name|Age|Power|Score|Rank|ThreatLevel");
                
                foreach (var hero in heroes)
                {
                    writer.WriteLine(hero.ToString());
                }
            }
        }
        #endregion
    }
}
