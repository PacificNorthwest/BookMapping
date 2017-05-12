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
using MayProject.Contracts;
using MayProject.Controller;
using System.Windows.Markup;

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для RelationsMapPage.xaml
    /// </summary>
    public partial class RelationsMapPage : UserControl
    {
        private MapState _currentState;
        private Book _book;
        private Node _focusedCharacter;
        private Point _m_start;
        private Vector _m_startOffset;

        public RelationsMapPage(Book book)
        {
            _book = book;
            InitializeComponent();
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent("IIllustratable") &&
                !(Map.Children.Cast<UIElement>().ToList()
                .Where(c => c is Node).ToList()
                .Exists(c => ((c as Node).DataContext as IElement).Title ==
                             (e.Data.GetData("IIllustratable") as IElement).Title)))
                { 
                Node node = NodeFactory(e.Data.GetData("IIllustratable") as IIllustratable);
                node.DataContext = e.Data.GetData("IIllustratable") as IElement;
                Point position = e.GetPosition(Map);
                Canvas.SetLeft(node, position.X);
                Canvas.SetTop(node, position.Y);
                Map.Children.Add(node);
                if (e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
                    e.Effects = DragDropEffects.Copy;
                else
                    e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }

        private Node NodeFactory(IIllustratable element)
        {
            Image img = new Image();
            if (element.Illustrations.Count > 0)
                img.Source = element.Illustrations[element.Illustrations.Count-1].ToBitmapImage();
            else if (element is Character)
                img.Source = Properties.Resources.avatar.ToBitmapImage();
            else
                img.Source = Properties.Resources.park.ToBitmapImage();
            Button plate = new Button();
            plate.Content = img;
            if (element is Character)
                plate.Style = this.FindResource("CharacterPlateStyle") as Style;
            else
                plate.Style = this.FindResource("LocationPlateStyle") as Style;
            TextBlock name = new TextBlock();
            name.Text = element.Title;
            name.FontSize = 25;

            Node node = new Node(Map, plate, name);
            node.Style = this.FindResource("PlainNode") as Style;
            node.MouseRightButtonDown += Node_MouseRightButtonDown;
            node.PreviewMouseDown += Node_PreviewMouseDown;
            node.PreviewMouseMove += Node_PreviewMouseMove;
            node.PreviewMouseUp += Node_PreviewMouseUp;

            return node;
        }

        private void Node_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _focusedCharacter = sender as Node;
        }

        private void Node_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            (sender as Node).ReleaseMouseCapture();
        }

        private void Node_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_currentState == MapState.Link)
            {
                Link link = new Link();
                link.SetBinding(Link.SourceProperty,
                                new Binding()
                                {
                                    Source = _focusedCharacter,
                                    Path = new PropertyPath(Node.AnchorPointProperty)
                                });
                link.SetBinding(Link.DestinationProperty,
                                new Binding()
                                {
                                    Source = sender as Node,
                                    Path = new PropertyPath(Node.AnchorPointProperty)
                                });
                link.Label.SetBinding(TextBox.MarginProperty,
                                new Binding()
                                {
                                    Source = link,
                                    Path = new PropertyPath(Link.LabelPositionProperty),
                                    Converter = new AnchorPointToMarginConverter()
                                });
                Map.Children.Add(link);
                _currentState = MapState.Normal;
            }

            _m_start = e.GetPosition(Map);
            _m_startOffset = new Vector(((sender as Node).RenderTransform as TranslateTransform).X,
                                        ((sender as Node).RenderTransform as TranslateTransform).Y);
            (sender as Node).CaptureMouse();
        }

        private void Node_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if ((sender as Node).IsMouseCaptured)
            {
                Vector offset = Point.Subtract(e.GetPosition(Map), _m_start);

                ((sender as Node).RenderTransform as TranslateTransform).X = _m_startOffset.X + offset.X;
                ((sender as Node).RenderTransform as TranslateTransform).Y = _m_startOffset.Y + offset.Y;
                (sender as Node).UpdateAnchor();
            }
        }

        private void ContextMenuButton_MouseOver(object sender, RoutedEventArgs e)
        {
            ((((sender as Button).Parent as Grid).Children
                .Cast<UIElement>()
                .First(x => x.GetType() == typeof(Viewbox)) as Viewbox).Child as TextBlock)
                .Text = (sender as Button).DataContext as string;
        }

        private void ContextMenuButton_MouseLeave(object sender, RoutedEventArgs e)
        {
            ((((sender as Button).Parent as Grid).Children
                .Cast<UIElement>()
                .First(x => x.GetType() == typeof(Viewbox)) as Viewbox).Child as TextBlock)
                .Text = string.Empty;
        }

        private void ButtonLink_Click(object sender, RoutedEventArgs e)
        {
            _currentState = MapState.Link;
        }
    }
}
