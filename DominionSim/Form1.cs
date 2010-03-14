using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DominionSim
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// List of all the Combo Boxes for players so we can quickly loop through them
        /// </summary>
        private List<ComboBox> PlayerComboBoxes = new List<ComboBox>();

        public Form1()
        {
            InitializeComponent();

            PlayerComboBoxes.AddRange(new ComboBox[] { playerCombo0, playerCombo1, playerCombo2, playerCombo3, playerCombo4, playerCombo5 });

            // TODO Get this from Reflection or something
            // TODO These can be objects of any kind, not just Strings
            foreach (ComboBox comboBox in PlayerComboBoxes)
            {
                comboBox.Items.Add("None");
                comboBox.Items.Add("Big Money");
                comboBox.Items.Add("Big Money Duchy");
                comboBox.Items.Add("Smithy");
                comboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Play button was clicked
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Event params</param>
        private void playButton_Click(object sender, EventArgs e)
        {
            // Track a count of AIs we have so we can number them
            Dictionary<string, int> countAIs = new Dictionary<string, int>();

            // Loop through all the AI selections
            int totalPlayers = 0;
            foreach (ComboBox comboBox in PlayerComboBoxes)
            {
                if ("None" != comboBox.Text)
                {
                    if (countAIs.ContainsKey(comboBox.Text))
                    {
                        countAIs[comboBox.Text] += 1;
                    }
                    else
                    {
                        countAIs[comboBox.Text] = 1;
                    }
                    totalPlayers++;
                }
            }

            if (1 < totalPlayers)
            {
                Simulator sim = new Simulator();

                foreach (var iter in countAIs)
                {
                    for (var i = 0; i < iter.Value; ++i)
                    {
                        string name = iter.Key;
                        if (1 < iter.Value)
                        {
                            name += " " + (i + 1);
                        }
                        Player newPlayer = new Player(name);
                        newPlayer.Strategy = MakeStrategyFromKey(iter.Key);
                        newPlayer.Verbose = false;
                        sim.Players.Add(newPlayer);
                    }
                }

                const int NumGames = 5000;
                sim.PlayNGames(NumGames, false);

                int numTies = sim.Wins.ContainsKey("Tie") ? sim.Wins["Tie"] : 0;
                outputBox.Text = NumGames + " games played, " + (NumGames - numTies) + " games had an outright winner.\r\n";

                for (int i = 0; i < sim.Players.Count; i++)
                {
                    string playerName = sim.Players[i].Name;
                    int numWins = sim.Wins.ContainsKey(playerName) ? sim.Wins[playerName] : 0;

                    float percent = 100.0f * numWins / (NumGames - numTies);
                    outputBox.Text += playerName + " : " + numWins + " / " + (NumGames - numTies) + " = " + percent + "%\r\n";
                }
            }
            else
            {
                outputBox.Text = "Not enough Players.  Please select at least 2 players.";
            }
        }

        /// <summary>
        /// Return a new Strategy with the given Key
        /// </summary>
        /// <param name="key">Name of the Strategy</param>
        /// <returns>A new Strategy</returns>
        private IStrategy MakeStrategyFromKey(string key)
        {
 	        switch (key)
            {
                case "Big Money":
                    return new Strategy.BigMoney();

                case "Big Money Duchy":
                    return new Strategy.BigMoneyDuchy();

                case "Smithy":
                    return new Strategy.BigMoneyDuchy();

                default:
                    throw new Exception("Count not find Strategy named " + key);
            }
        }
    }
}

