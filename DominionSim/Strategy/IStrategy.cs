using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim
{
    interface IStrategy
    {
        void TurnAction(Player p, Supply s);
        void TurnBuy(Player p, Supply s);

        List<string> ChooseCardsToTrash(Player p, int min, int max);
    }
}
