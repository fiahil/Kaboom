﻿using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using Kaboom.Serializer;
using KaboomEditor.Sources;

namespace KaboomEditor.Pages
{
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

        /// <summary>
        /// Instanciate and fill a new board
        /// </summary>
        /// <param name="w">width in square</param>
        /// <param name="h">height in square</param>
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
                    var elt = new SquareLabel(i, j);

                    elt.Entities.Add(KeResources.Proxy[KeResources.Type.Background].Clone());

                    elt.MouseLeftButtonUp += OnLeftButtonUp;
                    elt.MouseLeftButtonDown += OnLeftButtonDown;
                    elt.MouseRightButtonUp += OnRightButtonUp;
                    elt.MouseRightButtonDown += OnRightButtonDown;
                    elt.MouseEnter += OnMouseEnter;

                    uniformGrid.Children.Add(elt);
                }
            }
        }

        /// <summary>
        /// Serialize current map to xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Create a new board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #region Handlers

        private void OnMouseEnter(object sender, MouseEventArgs args)
        {
            if (this.painting_)
            {
                ((SquareLabel)sender).Entities.Add(KeResources.Proxy[KeResources.Type.BlockUbk].Clone());
                ((SquareLabel)sender).Background = Brushes.Sienna; //TODO
            }
            if (this.cleaning_)
            {
                ((SquareLabel)sender).Entities.Clear();
                ((SquareLabel)sender).Background = Brushes.CadetBlue; //TODO
            }
        }

        private void OnLeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            this.painting_ = false;
        }

        private void OnRightButtonUp(object sender, MouseButtonEventArgs args)
        {
            this.cleaning_ = false;
        }

        private void OnLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            this.painting_ = true;
            if (((SquareLabel)sender).IsMouseOver)
            {
                this.OnMouseEnter(sender, args);
            }
        }

        private void OnRightButtonDown(object sender, MouseButtonEventArgs args)
        {
            this.cleaning_ = true;
            if (((SquareLabel)sender).IsMouseOver)
            {
                this.OnMouseEnter(sender, args);
            }
        }
        #endregion
    }
}