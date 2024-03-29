﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DominionSim.Tournament;

namespace DominionSim
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// List of all the Combo Boxes for players so we can quickly loop through them
        /// </summary>
        private List<AIUIInfo> AIUIComponents = new List<AIUIInfo>();

        private Simulator mSim;

        private IEnumerable<StrategyTypeHolder> mTypes;

        public Form1()
        {
            InitializeComponent();

            AIUIComponents.AddRange(new AIUIInfo[] {
                new AIUIInfo(playerCombo0, playerVerbose0),
                new AIUIInfo(playerCombo1, playerVerbose1),
                new AIUIInfo(playerCombo2, playerVerbose2),
                new AIUIInfo(playerCombo3, playerVerbose3),
                new AIUIInfo(playerCombo4, playerVerbose4),
                new AIUIInfo(playerCombo5, playerVerbose5)});

            // Use cool C# runtime type info and reflection stuff to find all non-abstract classes in all loaded
            // assemblies that inherit from IStrategy, and dump them into my simple StrategyTypeHolder class that
            // we can throw into a ComboBox
            var inheritType = typeof(Strategy.IStrategy);
            mTypes = AppDomain.CurrentDomain.GetAssemblies().ToList()                // List of all loaded assemblies (this exe, dlls)
                .SelectMany(assemblies => assemblies.GetTypes())                        // Convert that list to a list of all loaded Types from each Assembly
                .Where(type => inheritType.IsAssignableFrom(type) && !type.IsAbstract)  // Only pick out the bits that implement the given type (IStrategy) and are not abstract
                .Select(type => new StrategyTypeHolder(type.Name, type))                // Throw each of those into my little StrategyTypeHolder class
                .OrderBy(holder => holder.Name);                                        // Sort by name

            // Populate each ComboBox with those StrategyTypeHolders, plus a Dummy one for "None"
            foreach (ComboBox comboBox in AIUIComponents.Select(aiui=>aiui.AISelection))
            {
                comboBox.Items.Add(new StrategyTypeHolder("None", null));
                comboBox.Items.AddRange(mTypes.ToArray());
                comboBox.SelectedIndex = 0;
            }

            ToolTip toolTip = new ToolTip();
            foreach (CheckBox checkBox in AIUIComponents.Select(aiui => aiui.VerboseCheckbox))
            {
                toolTip.SetToolTip(checkBox, "Verbose");
                checkBox.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
            }

            RandomizePlayers();
        }

        void RandomizePlayers()
        {
            // Default to something playable immediately
            var rand = new Random();
            int numTypes = mTypes.Count();
            playerCombo0.SelectedIndex = (rand.Next() % numTypes) + 1;
            playerCombo1.SelectedIndex = (rand.Next() % numTypes) + 1;
            playerCombo2.SelectedIndex = (rand.Next() % numTypes) + 1;
            playerCombo3.SelectedIndex = (rand.Next() % numTypes) + 1;
        }

        void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            if (checkBox.Checked)
            {
                gameVerbose.Checked = true;
                txtNumGames.Text = "1";
            }
        }

        /// <summary>
        /// Play button was clicked
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Event params</param>
        private void playButton_Click(object sender, EventArgs e)
        {
            // Get a list of all the AIs to use
            var aisToUse = AIUIComponents.Where(aiui => (aiui.AISelection.SelectedItem as StrategyTypeHolder).Type != null);

            if (1 < aisToUse.Count())
            {
                mSim = new Simulator();

                // Loop through all the AIs that were selected and spawn Players and Strategies for each
                foreach (var iter in aisToUse)
                {
                    StrategyTypeHolder strategyTypeHolder = (iter.AISelection.SelectedItem as StrategyTypeHolder);
                    string baseName = strategyTypeHolder.ToString();

                    IEnumerable<string> existingNames = mSim.Players.Select((p) => p.Name);
                    string name = baseName;
                    int i = 2;
                    while (existingNames.Contains(name))
                    {
                        name = baseName + " #"+i.ToString();
                        i++;
                    }
                    Player newPlayer = new Player(name);
                    newPlayer.Strategy = Activator.CreateInstance(strategyTypeHolder.Type) as Strategy.IStrategy;
                    newPlayer.Verbose = (iter.VerboseCheckbox != null ? iter.VerboseCheckbox.Checked : false);
                    mSim.Players.Add(newPlayer);
                }

                int NumGames = int.Parse(txtNumGames.Text);

                progressBar.Minimum = 0;
                progressBar.Maximum = NumGames;
                mSim.PlayNGames(NumGames, gameVerbose.Checked);

                outputBox.Text = NumGames + " games played" + Environment.NewLine;

                // Sort out the players so the most wins go on top
                var sortedPlayes = mSim.Players.OrderByDescending(p => mSim.Wins[p] + mSim.Ties[p]);
                foreach (var player in sortedPlayes)
                {
                    string playerName = player.Name;
                    int numWins = mSim.Wins.ContainsKey(player) ? mSim.Wins[player] : 0;
                    int numTies = mSim.Ties.ContainsKey(player) ? mSim.Ties[player] : 0;

                    outputBox.Text += playerName + " - Wins: " + numWins + ", Ties: " + numTies + ", Highest Score %: " + ((numWins + numTies) * 100.0f / NumGames) + Environment.NewLine;
                }

                outputBox.Text += Environment.NewLine + Stats.Tracker.Instance.GetAllPlayCountsString();

                Stats.Tracker.Instance.Clear();
            }
            else
            {
                outputBox.Text = "Not enough Players.  Please select at least 2 players.";
            }
        }

        private void gameVerbose_CheckedChanged(object sender, EventArgs e)
        {
            txtNumGames.Text = "1";
        }

        private void tourneyChosenButton_Click(object sender, EventArgs e)
        {
            outputBox.Text = "";
            // Get a list of all the AIs to use
            var aisToUse = AIUIComponents.Where(aiui => (aiui.AISelection.SelectedItem as StrategyTypeHolder).Type != null)
                                         .Select( aiui => (aiui.AISelection.SelectedItem as StrategyTypeHolder).Type )
                                         .Select(type => new Contender(type.Name, Activator.CreateInstance(type) as Strategy.IStrategy));

            Tourney t = new Tourney(aisToUse);
            int minPlayers = int.Parse(txtMinPlayers.Text);
            int maxPlayers = int.Parse(txtMaxPlayers.Text);
            int gamesPerMatch = int.Parse(txtGamesPerMatch.Text);

            int numMatchups = t.Run(minPlayers, maxPlayers, gamesPerMatch);
            outputBox.Text += numMatchups + " different matchups..." + Environment.NewLine;

            var stats = t.GetStats();

            var sortedDict = stats.OrderByDescending((kvp) => kvp.Value.Wins + kvp.Value.Ties);

            foreach (var kvp in sortedDict)
            {
                string name = kvp.Key.ToString();
                int wins = kvp.Value.Wins;
                int ties = kvp.Value.Ties;
                int games = kvp.Value.Games;
                float percentWins = 100.0f * wins / games;
                float percentTies = 100.0f * ties / games;
                outputBox.Text += name + " - Wins: " + wins + " (" + percentWins + "%), Ties: " + ties + " (" + percentTies + "%), Highest Score %: " + ((wins + ties) * 100.0f / games) + Environment.NewLine;
            }
            
        }

        private void tourneyAllButton_Click(object sender, EventArgs e)
        {
            outputBox.Text = "";
            // Get a list of all the AIs to use
            var aisToUse = mTypes.Select(sth => sth.Type)
                                 .Where(type => (Activator.CreateInstance(type) as Strategy.IStrategy).ValidForTourney() == true)
                                 .Select(type => new Contender(type.Name, Activator.CreateInstance(type) as Strategy.IStrategy));

            Tourney t = new Tourney(aisToUse);
            t.Feature(aisToUse.First(con => con.Strategy is Strategy.Smithy1));

            int minPlayers = int.Parse(txtMinPlayers.Text);
            int maxPlayers = int.Parse(txtMaxPlayers.Text);
            int gamesPerMatch = int.Parse(txtGamesPerMatch.Text);

            int numMatchups = t.Run(minPlayers, maxPlayers, gamesPerMatch);

            outputBox.Text += numMatchups + " different matchups..." + Environment.NewLine;

            var stats = t.GetStats();

            var sortedDict = stats.OrderByDescending((kvp) => kvp.Value.Wins + kvp.Value.Ties);

            foreach (var kvp in sortedDict)
            {
                string name = kvp.Key.ToString();
                int wins = kvp.Value.Wins;
                int ties = kvp.Value.Ties;
                int games = kvp.Value.Games;
                float percentWins = 100.0f * wins / games;
                float percentTies = 100.0f * ties / games;
                outputBox.Text += name + " - Wins: " + wins + " (" + percentWins + "%), Ties: " + ties + " (" + percentTies + "%), Highest Score %: " + ((wins + ties) * 100.0f / games) + Environment.NewLine;
            }
        }

        private void randomizeButton_Click(object sender, EventArgs e)
        {
            RandomizePlayers();
        }

  
    }

    /// <summary>
    /// Simple class to hold info on a given AI.  Contains all the UI elements for one.
    /// Could be its own Component if we cared.
    /// </summary>
    class AIUIInfo
    {
        public ComboBox AISelection;
        public CheckBox VerboseCheckbox;
        public AIUIInfo (ComboBox ai, CheckBox verbose)
        {
            AISelection = ai;
            VerboseCheckbox = verbose;
        }
    }

    /// <summary>
    /// Simple class to hold info about a Type we could use.  This class only exists so that I can make the dummy
    /// "None" type, which has no class.
    /// </summary>
    class StrategyTypeHolder
    {
        public string Name;
        public Type Type;
        public StrategyTypeHolder(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        /// <summary>
        /// Return a String for this 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (null != Type)
            {
                // See if the Strategy has supplied us with an alternate display name via a public static function
                var methodInfo = Type.GetMethod("GetDisplayName", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                if (null != methodInfo)
                {
                    return methodInfo.Invoke(null, null).ToString();
                }
            }
            return Name;
        }

        /// <summary>
        /// Overriding equality
        /// </summary>
        /// <param name="obj">Compare against</param>
        /// <returns>true if equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is StrategyTypeHolder)
            {
                return (obj as StrategyTypeHolder).Type == Type;
            }
            return false;
        }

        /// <summary>
        /// Override hashcode
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
    }
}

