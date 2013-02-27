using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Xml.Serialization;
using Kaboom.Serializer;

namespace KaboomEditor.Pages
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MapElements mapElements_;

        public MainWindow()
        {
            InitializeComponent();
            CreateBoard(5, 10);
        }

        private void CreateBoard(int w, int h)
        {
            this.mapElements_ = new MapElements(w, h);
            var uniformGrid = (UniformGrid)this.FindName("Board");
            if (uniformGrid != null)
            {
                uniformGrid.Children.Clear();
                uniformGrid.Columns = h;
                uniformGrid.Rows = w;
                for (var i = 0; i < w; i++)
                {
                    for (var j = 0; j < h; j++)
                    {
                        uniformGrid.Children.Add(new Label
                            {
                                Background = j % 2 == i % 2 ? Brushes.SlateGray : Brushes.Silver,
                                ToolTip = "(" + i + ", " + j + ")"
                            });
                    }
                }
            }
        }


        /*
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
                        me.Board[i][j].Entities.Add(new BombProxy
                        {
                            TileIdentifier = "BombSheet",
                            TileFramePerAnim = new[] { 8, 18 },
                            TileTotalAnim = 2,
                            TileFrameSpeed = 20,
                            Type = 0
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
        */

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            var mySerializer = new XmlSerializer(typeof(MapElements));

            var box = (TextBox) this.FindName("TextBoxFilename");
            if (box != null)
            {
                using (var writer = new StreamWriter(box.Text + ".xml"))
                {
                    mySerializer.Serialize(writer, this.mapElements_);
                }
            }
        }

        private void ButtonNew_OnClick(object sender, RoutedEventArgs e)
        {
            var height = (TextBox) this.FindName("TextBoxHeight");
            var width = (TextBox) this.FindName("TextBoxWidth");

            if (width != null && height != null)
            {
                int h, w;
                int.TryParse(width.Text, out h);
                int.TryParse(height.Text, out w);
                this.CreateBoard(w, h);
            }
        }
    }
}
