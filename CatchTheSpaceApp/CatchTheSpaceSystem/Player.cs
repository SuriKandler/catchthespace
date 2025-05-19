using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchTheSpaceSystem
{
    public enum Player
    {
        None,
        Red,
        Orange
    }

     public static class PlayerExtensions
    {
        public static readonly Dictionary<Player, Color> PlayerColors = new()
        {
            { Player.Red, Color.FromArgb(250, 125, 125) },
            { Player.Orange, Color.FromArgb(247, 198, 113) },
            { Player.None, Color.Transparent }
        };

        public static Color GetColor(this Player player)
        {
            return PlayerColors[player];
        }
    }
}
