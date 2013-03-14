using System;
using System.Windows;
using Kaboom.Serializer;
using KaboomEditor.Sources;

namespace KaboomEditor.Pages
{
    /// <summary>
    /// Interaction logic for Checkpoint.xaml
    /// </summary>
    public partial class Checkpoint
    {
        private readonly CheckPointProxy proxy_;

        public Checkpoint(CheckPointProxy proxy)
        {
            proxy_ = proxy;
            InitializeComponent();
        }

        /// <summary>
        /// Close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.proxy_.Activated = (int) this.ActivationSlider.Value == 1;
                this.proxy_.Bombsetidx = Convert.ToInt32(this.BombsetButton.Text) - 1;
            }
            catch (Exception)
            {
            }
            finally
            {
                this.Close();
                SquareLabel.Raise = false;
            }
        }

        private void ActivationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.StateLabel.Content = (int) this.ActivationSlider.Value == 1 ? "True" : "False";
        }
    }
}
