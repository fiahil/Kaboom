using System.Collections.Generic;
using System.Windows.Controls;

namespace KaboomEditor.Sources
{
    /// <summary>
    /// Used to associate a bucket type with an image
    /// </summary>
    class AssetImage : Image
    {
        private static readonly Dictionary<string, KeResources.Type> typeLink_ = new Dictionary<string, KeResources.Type>
            {
                {"background1", KeResources.Type.Background},
                {"background2", KeResources.Type.BlockBk},
                {"background3", KeResources.Type.BlockUbk},
                {"BombSheetTNT", KeResources.Type.Bomb},
                {"goal", KeResources.Type.Goal},
                {"checkpoint", KeResources.Type.Checkpoint}
            };

        /// <summary>
        /// Initialize a new AssetImage
        /// </summary>
        /// <param name="type">Bucket type</param>
        public AssetImage(KeResources.Type type)
        {
            this.Source = KeResources.Bitmap[type];
        }

        public AssetImage(string type)
        {
            this.Source = KeResources.Bitmap[typeLink_[type]];
        }
    }
}
