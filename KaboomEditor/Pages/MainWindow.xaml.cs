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
            var myObject = new MapElements(2, 5);

            myObject.Board[0][0].Entities.Add(new BlockProxy()
                {
                    Destroyable = true,
                    TileFramePerAnim = new[] {1},
                    TileFrameSpeed = 1,
                    TileIdentifier = "koukou",
                    TileTotalAnim = 1,
                    ZIndex = 0
                });
            
            var mySerializer = new XmlSerializer(typeof(MapElements));
            // To write to a file, create a StreamWriter object.

            using (var writer = new StreamWriter("myFileName.xml"))
            {
                mySerializer.Serialize(writer, myObject);
            }
        }
    }
}
