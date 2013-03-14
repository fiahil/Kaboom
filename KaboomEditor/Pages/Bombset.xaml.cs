using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Kaboom.Serializer;
using KaboomEditor.Sources;

namespace KaboomEditor.Pages
{
    /// <summary>
    /// Interaction logic for Bombset.xaml
    /// </summary>
    public partial class Bombset
    {
        private readonly MapElements me_;
        private int meIdx_;

        public Bombset(MapElements me)
        {
            this.me_ = me;
            this.meIdx_ = 0;

            InitializeComponent();
            FillBombList();
        }

        /// <summary>
        /// Initialize BombList
        /// </summary>
        private void FillBombList()
        {
            foreach (var bombInfoProxy in this.me_.Bombset[this.meIdx_])
                this.BombsetList.Items.Add(bombInfoProxy);
        }

        /// <summary>
        /// Add a bomb to the list
        /// </summary>
        /// <param name="name"></param>
        /// <param name="quantity"></param>
        private void AddToMapElt(string name, int quantity)
        {
            var bombinfo = new BombInfoProxy
                {
                    Name = name,
                    Quantity = quantity,
                    Type = KeResources.BombTypeLink[name]
                };

            if (this.me_.Bombset[this.meIdx_].Count < 5)
            {
                this.me_.Bombset[this.meIdx_].Add(bombinfo);
                this.BombsetList.Items.Add(bombinfo);
            }
        }

        /// <summary>
        /// Handle Validate bomb
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.AddToMapElt((string) ((ComboBoxItem) this.TypeSelecter.SelectedItem).Content,
                                 Convert.ToInt32(this.QuantitySelector.Value));
            }
            catch (Exception)
            {
                return;
            }

            this.TypeSelecter.SelectedItem = null;
            this.QuantitySelector.Value = 0;
        }

        /// <summary>
        /// Handle Delete bomb
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.me_.Bombset[this.meIdx_].RemoveAt(this.BombsetList.SelectedIndex);
                this.BombsetList.Items.RemoveAt(this.BombsetList.SelectedIndex);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handle slider value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuantitySelector_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.QuantityLabel.Content = (int) e.NewValue;
        }

        /// <summary>
        /// Handle MouseWheel on slider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuantitySelector_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            this.QuantitySelector.Value += e.Delta / 100;
        }
        
        /// <summary>
        /// Handle click on new button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            this.meIdx_ += 1;
            this.me_.Bombset.Add(new List<BombInfoProxy>());
            this.IdLabel.Content = "ID : " + (this.meIdx_ + 1) + " / " + this.me_.Bombset.Count;
            this.BombsetList.Items.Clear(); 
        }

        /// <summary>
        /// Handle click on remove button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.me_.Bombset.Count == 1)
                return;

            this.me_.Bombset.RemoveAt(this.meIdx_);
            this.meIdx_ = 0;
            this.IdLabel.Content = "ID : " + (this.meIdx_ + 1) + " / " + this.me_.Bombset.Count;
            this.BombsetList.Items.Clear();
            this.FillBombList();
        }

        private void GoButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.meIdx_ = Convert.ToInt32(this.IdTextBox.Text);
                this.meIdx_ -= 1;
            }
            catch (FormatException)
            {
                return;
            }
            if (this.meIdx_ >= this.me_.Bombset.Count || this.meIdx_ < 0)
                return;

            this.IdLabel.Content = "ID : " + (this.meIdx_ + 1) + " / " + this.me_.Bombset.Count;
            this.BombsetList.Items.Clear();
            this.FillBombList();
        }
    }
}
