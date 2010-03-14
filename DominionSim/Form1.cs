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

            // Use cool C# runtime type info and reflection stuff to find all non-abstract classes in all loaded
            // assemblies that inherit from IStrategy, and dump them into my simple StrategyTypeHolder class that
            // we can throw into a ComboBox
            var inheritType = typeof(IStrategy);
            var types = AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(assemblies => assemblies.GetTypes())
                .Where(type => inheritType.IsAssignableFrom(type) && !type.IsAbstract)
                .Select(type => new StrategyTypeHolder(type.Name, type))
                .OrderBy(holder => holder.Name);

            // Populate each ComboBox with those StrategyTypeHolders, plus a Dummy one for "None"
            foreach (ComboBox comboBox in PlayerComboBoxes)
            {
                comboBox.Items.Add(new StrategyTypeHolder("None", null));
                comboBox.Items.AddRange(types.ToArray());
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
            // Count up all the AIs of each type we have selected so we can spawn the right number of each
            var countAIs = new Dictionary<StrategyTypeHolder, int>();
            int totalPlayers = 0;
            foreach (ComboBox comboBox in PlayerComboBoxes)
            {
                StrategyTypeHolder typeInfo = comboBox.SelectedItem as StrategyTypeHolder;
                if (null != typeInfo.Type)
                {
                    if (countAIs.ContainsKey(typeInfo))
                    {
                        countAIs[typeInfo] += 1;
                    }
                    else
                    {
                        countAIs[typeInfo] = 1;
                    }
                    totalPlayers++;
                }
            }

            if (1 < totalPlayers)
            {
                Simulator sim = new Simulator();

                // Loop through all the AIs that were selected and spawn Players and Strategies for each
                foreach (var iter in countAIs)
                {
                    for (var i = 0; i < iter.Value; ++i)
                    {
                        string name = iter.Key.ToString();
                        if (1 < iter.Value)
                        {
                            name += " " + (i + 1);
                        }
                        Player newPlayer = new Player(name);
                        newPlayer.Strategy = Activator.CreateInstance(iter.Key.Type) as IStrategy;
                        newPlayer.Verbose = false;
                        sim.Players.Add(newPlayer);
                    }
                }

                const int NumGames = 5000;
                sim.PlayNGames(NumGames, false);

                int numTies = sim.Wins.ContainsKey("Tie") ? sim.Wins["Tie"] : 0;
                outputBox.Text = NumGames + " games played, " + (NumGames - numTies) + " games had an outright winner.\r\n";

                // Sort out the players so the most wins go on top
                var sortedPlayes = sim.Players.OrderBy(p => sim.Wins[p.Name]).Reverse();
                foreach (var player in sortedPlayes)
                {
                    string playerName = player.Name;
                    int numWins = sim.Wins.ContainsKey(player.Name) ? sim.Wins[player.Name] : 0;

                    float percent = 100.0f * numWins / (NumGames - numTies);
                    outputBox.Text += playerName + " : " + numWins + " / " + (NumGames - numTies) + " = " + percent + "%\r\n";
                }
            }
            else
            {
                outputBox.Text = "Not enough Players.  Please select at least 2 players.";
            }
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

