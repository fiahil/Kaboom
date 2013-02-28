using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using Kaboom.Serializer;

namespace KaboomEditor.Sources
{
    public class SquareLabel : Label
    {
        public List<EntityProxy> Entities = new List<EntityProxy>();

        public int XCoord { get; set; }
        public int YCoord { get; set; }

        /// <summary>
        /// Instanciate a fresh label
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        public SquareLabel(int i, int j)
        {
            Background = j % 2 == i % 2 ? Brushes.SlateGray : Brushes.Silver;
            ToolTip = "(" + j + ", " + i + ")";
            XCoord = j;
            YCoord = i;
        }
    }
}