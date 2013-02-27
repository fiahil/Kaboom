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
        public int XCoord { get; set; }
        public int YCoord { get; set; }
        public EntityProxy Entity { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MapElements mapElements_;

        private bool brossing_;
        public MainWindow()
        {
            InitializeComponent();
            CreateBoard(10, 5);
            this.brossing_ = false;
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
                            YCoord = i,
                            Entity = new EntityProxy
                                {
                                    TileIdentifier = "background1",
                                    TileFramePerAnim = new[] {1},
                                    TileTotalAnim = 1,
                                    TileFrameSpeed = 1,
                                    ZIndex = 1
                                }
                        };

                    elt.MouseLeftButtonUp += (sender, args) => { this.brossing_ = false; };
                    elt.MouseLeftButtonDown += (sender, args) =>
                        {
                            this.brossing_ = true;
                            if (((SquareLabel) sender).IsMouseOver)
                            {
                                ((SquareLabel)sender).Entity = new BlockProxy
                                    {
                                        TileIdentifier = "background2",
                                        TileFramePerAnim = new[] { 1, 2 },
                                        TileTotalAnim = 2,
                                        TileFrameSpeed = 2,
                                        Destroyable = true
                                    };
                                ((SquareLabel)sender).Background = Brushes.Sienna;                                    
                            }
                        };

                    elt.MouseEnter += (sender, args) =>
                        {
                            if (this.brossing_)
                            {
                                ((SquareLabel) sender).Entity = new BlockProxy
                                    {
                                        TileIdentifier = "background2",
                                        TileFramePerAnim = new[] {1, 2},
                                        TileTotalAnim = 2,
                                        TileFrameSpeed = 2,
                                        Destroyable = true
                                    };
                                ((SquareLabel) sender).Background = Brushes.Sienna;
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
                    this.mapElements_.Board[elt.XCoord][elt.YCoord].Entities.Add(elt.Entity);
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
