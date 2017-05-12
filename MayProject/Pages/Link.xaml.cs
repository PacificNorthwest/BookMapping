using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для Link.xaml
    /// </summary>
    public partial class Link : UserControl
    {
        public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(
            "Source", typeof(Point), typeof(Link),
            new FrameworkPropertyMetadata(default(Point)));

        public Point Source
        {
            get { return (Point)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty DestinationProperty =
            DependencyProperty.Register(
                "Destination", typeof(Point), typeof(Link),
                    new FrameworkPropertyMetadata(default(Point)));

        public Point Destination
        {
            get { return (Point)this.GetValue(DestinationProperty); }
            set { this.SetValue(DestinationProperty, value); }
        }

        public static readonly DependencyProperty LabelPositionProperty =
            DependencyProperty.Register(
                "LabelPosition", typeof(Point), typeof(Link),
                    new FrameworkPropertyMetadata(default(Point)));

        public Point LabelPosition
        {
            get { return (Point)this.GetValue(LabelPositionProperty); }
            set { this.SetValue(LabelPositionProperty, value); }
        }

        public Link()
        {
            InitializeComponent();
            LineSegment segment = new LineSegment(default(Point), true);
            PathFigure figure = new PathFigure(default(Point), new[] { segment }, false);
            PathGeometry geometry = new PathGeometry(new[] { figure });
            BindingBase sourceBinding =
               new Binding { Source = this, Path = new PropertyPath(SourceProperty) };
            BindingBase destinationBinding =
               new Binding { Source = this, Path = new PropertyPath(DestinationProperty) };
            MultiBinding labelPositionMultibinding =
                new MultiBinding();
            labelPositionMultibinding.Bindings.Add(sourceBinding);
            labelPositionMultibinding.Bindings.Add(destinationBinding);
            labelPositionMultibinding.NotifyOnSourceUpdated = true;
            labelPositionMultibinding.Converter = new LinkLabelPositionConverter();

            BindingOperations.SetBinding(
                figure, PathFigure.StartPointProperty, sourceBinding);
            BindingOperations.SetBinding(
                segment, LineSegment.PointProperty, destinationBinding);
            BindingOperations.SetBinding(
                this, LabelPositionProperty, labelPositionMultibinding);
           

            Panel.Children.Add(new Path
            {
                Data = geometry,
                StrokeThickness = 2,
                Stroke = Brushes.White,
                MinWidth = 1,
                MinHeight = 1,
            });
            (Panel.Children[1] as Path).MouseDown +=
                (object sender, MouseButtonEventArgs e) => Label.Focus();
        }

        private void Label_TextChanged(object sender, TextChangedEventArgs e)
        {
            var formattedText = new FormattedText(
                    (sender as TextBox).Text,
                    CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(this.Label.FontFamily, this.Label.FontStyle, this.Label.FontWeight, this.Label.FontStretch),
                    this.Label.FontSize,
                    Brushes.Black);
            (sender as TextBox).Width = formattedText.Width;
        }
    }
}
