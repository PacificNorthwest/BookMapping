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
using MayProject.DataModel;

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для EventNode.xaml
    /// </summary>
    public partial class EventNode : UserControl
    {
        public static readonly DependencyProperty AnchorPointProperty =
        DependencyProperty.Register(
            "AnchorPoint", typeof(Point), typeof(EventNode),
                new FrameworkPropertyMetadata(new Point(0, 0),
                FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Point AnchorPoint
        {
            get { return (Point)GetValue(AnchorPointProperty); }
            set { SetValue(AnchorPointProperty, value); }
        }

        private Canvas mCanvas;

        public EventNode(Canvas canvas, string title, string description, List<Character> characters, Location location, string time)
        {
            InitializeComponent();
            mCanvas = canvas;
            EventTitle.Text = title;
            EventDescription.Text = description;
            EventCharacters.Text = characters
                                      .Select(c => c.Title).ToList()
                                      .Aggregate((current, next) => $"{current}\n{next}");
            EventLocation.Text = location.Title;
            EventTime.Text = time;
        }

        public void UpdateAnchor()
        {
            Size size = RenderSize;
            Point ofs = new Point((size.Width / 2) + 50, (size.Height / 2) + 120);
            AnchorPoint = TransformToVisual(this.mCanvas).Transform(ofs);
        }
    }
}
