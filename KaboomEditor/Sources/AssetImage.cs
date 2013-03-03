using System.Windows.Controls;
using System.Windows.Media;

namespace KaboomEditor.Sources
{
    /// <summary>
    /// Used to associate a bucket type with an image
    /// </summary>
    class AssetImage : Image
    {
        /// <summary>
        /// Initialize a new AssetImage
        /// </summary>
        /// <param name="type">Bucket type</param>
        public AssetImage(KeResources.Type type)
        {
            this.Source = KeResources.Bitmap[type];
            this.Stretch = Stretch.Fill;
        }

        public AssetImage(string type)
        {
            this.Source = KeResources.Bitmap[KeResources.TypeLink[type]];
            this.Stretch = Stretch.Fill;
        }
    }
}
