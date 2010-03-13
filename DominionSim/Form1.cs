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
        public Form1()
        {
            InitializeComponent();

            Simulator sim = new Simulator();
            sim.CreatePlayers(4, false);

            sim.PlayNGames(5000, false);

        }
    }
}
