using System.Windows.Controls;

namespace KaboomEditor.Sources
{
    /// <summary>
    /// Used to choose the current bucket
    /// </summary>
    class BucketButton : Button
    {
        private KeResources.Type type_;

        public KeResources.Type Type
        {
            get
            {
                return this.type_;
            }

            set
            {
                this.type_ = value;
                this.Content = new AssetImage(this.type_);
            }
        }
    }
}
