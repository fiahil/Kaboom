using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Kaboom.Serializer;
using KaboomEditor.Pages;

namespace KaboomEditor.Sources
{
    /// <summary>
    /// Used to store informations inside board
    /// </summary>
    public class SquareLabel : Label
    {
        public EntityProxy[] Entities = new EntityProxy[5];

        public int XCoord { get; set; }
        public int YCoord { get; set; }

        public static bool Raise;

        /// <summary>
        /// Instanciate a fresh label
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public SquareLabel(int i, int j)
        {
            Background = j % 2 == i % 2 ? Brushes.SlateGray : Brushes.Silver;
            BorderThickness = new Thickness(1.0);
            BorderBrush = Brushes.Black;
            ToolTip = "(" + j + ", " + i + ")";
            XCoord = j;
            YCoord = i;
            MouseWheel += (sender, args) =>
                {
                    if (Entities[1] != null && Raise == false)
                    {
                        Raise = true;
                        var c = new Checkpoint((CheckPointProxy) Entities[1]);
                        c.Show();
                    }
                };
        }
    }
}