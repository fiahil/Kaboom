using System.Windows.Controls;

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
        }
    }
}
