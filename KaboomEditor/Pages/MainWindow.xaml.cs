using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using Kaboom.Serializer;

namespace KaboomEditor.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random(777);
            var me = new MapElements(15, 15);

            for (var i = 0; i < 15; i++)
            {
                for (var j = 0; j < 15; j++)
                {
                    me.Board[i][j].Entities.Add(new EntityProxy
                    {
                        TileIdentifier = "background1",
                        TileFramePerAnim = new[] { 1 },
                        TileTotalAnim = 1,
                        TileFrameSpeed = 1,
                        ZIndex = 1
                    });

                    if (i == 6 && j == 7)
                    {
                        me.Board[i][j].Entities.Add(new CheckPointProxy 
                        {
                            TileIdentifier = "highlight",
                            TileFramePerAnim = new[] { 8, 18 },
                            TileTotalAnim = 2,
                            TileFrameSpeed = 20,
                        });
                    }
                    if ((i == 7 || i == 6 || i == 5) && j == 7)
                        continue;

                    if (r.Next(2) == 0)
                    {
                        me.Board[i][j].Entities.Add(new BlockProxy
                        {
                            Destroyable = true,
                            TileIdentifier = "background2",
                            TileFramePerAnim = new[] { 1, 2 },
                            TileTotalAnim = 2,
                            TileFrameSpeed = 2
                        });
                    }
                    else
                    {
                        me.Board[i][j].Entities.Add(new BlockProxy
                        {
                            Destroyable = false,
                            TileIdentifier = "background3",
                            TileFramePerAnim = new[] { 1 },
                            TileTotalAnim = 1,
                            TileFrameSpeed = 1
                        });
                    }
                }
            }

            
            var mySerializer = new XmlSerializer(typeof(MapElements));
            // To write to a file, create a StreamWriter object.

            using (var writer = new StreamWriter("level1.xml"))
            {
                mySerializer.Serialize(writer, me);
            }
        }
    }
}
