using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для Node.xaml
    /// </summary>
    public partial class Node : Button
    {
        public static readonly DependencyProperty AnchorPointProperty =
        DependencyProperty.Register(
            "AnchorPoint", typeof(Point), typeof(Node),
                new FrameworkPropertyMetadata(new Point(0, 0),
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Point AnchorPoint
        {
            get { return (Point)GetValue(AnchorPointProperty); }
            set { SetValue(AnchorPointProperty, value); }
        }

        private Canvas mCanvas;

        public Node(Canvas canvas, Button image, TextBlock title)
        {
            InitializeComponent();
            mCanvas = canvas;

            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 0);
            Plate.Children.Add(image);
            TitleContainer.Child = title;
        }

        public Node() : this(new Canvas(), new Button(), new TextBlock()) { }

        public void UpdateAnchor()
        {
            Size size = RenderSize;
            Point ofs = new Point(size.Width / 2, size.Height / 2);
            AnchorPoint = TransformToVisual(this.mCanvas).Transform(ofs);
        }
    }
}
