using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Xml.Serialization;
using Kaboom.Serializer;

namespace KaboomEditor.Pages
{
    public class SquareLabel : Label
    {
        public List<EntityProxy> Entities = new List<EntityProxy>();

        public int XCoord { get; set; }
        public int YCoord { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MapElements mapElements_;

        private bool painting_;
        private bool cleaning_;

        public MainWindow()
        {
            InitializeComponent();
            CreateBoard(10, 5);
            this.painting_ = false;
            this.cleaning_ = false;
        }

        private void CreateBoard(int w, int h)
        {
            var uniformGrid = (UniformGrid)this.FindName("Board");
            if (uniformGrid == null)
                return;

            this.mapElements_ = new MapElements(w, h);
            uniformGrid.Children.Clear();
            uniformGrid.Columns = w;
            uniformGrid.Rows = h;
            for (var i = 0; i < h; i++)
            {
                for (var j = 0; j < w; j++)
                {
                    var elt = new SquareLabel
                        {
                            Background = j % 2 == i % 2 ? Brushes.SlateGray : Brushes.Silver,
                            ToolTip = "(" + j + ", " + i + ")",
                            XCoord = j,
                            YCoord = i
                        };
                    elt.Entities.Add(new EntityProxy
                        {
                            TileIdentifier = "background1",
                            TileFramePerAnim = new[] {1},
                            TileTotalAnim = 1,
                            TileFrameSpeed = 1,
                            ZIndex = 1
                        });

                    elt.MouseLeftButtonUp += (sender, args) => { this.painting_ = false; };
                    elt.MouseLeftButtonDown += (sender, args) =>
                        {
                            this.painting_ = true;
                            if (((SquareLabel) sender).IsMouseOver)
                            {
                                ((SquareLabel) sender).Entities.Add(new BlockProxy
                                    {
                                        TileIdentifier = "background2",
                                        TileFramePerAnim = new[] {1, 2},
                                        TileTotalAnim = 2,
                                        TileFrameSpeed = 2,
                                        Destroyable = true
                                    });
                                ((SquareLabel)sender).Background = Brushes.Sienna;
                            }
                        };

                    elt.MouseRightButtonUp += (sender, args) => { this.cleaning_ = false; };
                    elt.MouseRightButtonDown += (sender, args) =>
                        {
                            this.cleaning_ = true;
                            if (((SquareLabel) sender).IsMouseOver)
                            {
                                ((SquareLabel)sender).Entities.Clear();
                                ((SquareLabel)sender).Background = Brushes.CadetBlue;
                            }
                        };

                    elt.MouseEnter += (sender, args) =>
                        {
                            if (this.painting_)
                            {
                                ((SquareLabel) sender).Entities.Add(new BlockProxy
                                    {
                                        TileIdentifier = "background2",
                                        TileFramePerAnim = new[] {1, 2},
                                        TileTotalAnim = 2,
                                        TileFrameSpeed = 2,
                                        Destroyable = true
                                    });
                                ((SquareLabel) sender).Background = Brushes.Sienna;
                            }
                            if (this.cleaning_)
                            {
                                ((SquareLabel)sender).Entities.Clear();
                                ((SquareLabel) sender).Background = Brushes.CadetBlue;
                            }
                        };
                    uniformGrid.Children.Add(elt);
                }
            }
        }

        private void ButtonSave_OnClick(object sender, RoutedEventArgs e)
        {
            var board = this.FindName("Board") as UniformGrid;

            if (board != null)
            {
                foreach (SquareLabel elt in board.Children)
                {
                    this.mapElements_.Board[elt.XCoord][elt.YCoord].Entities = elt.Entities;
                }
            }

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
