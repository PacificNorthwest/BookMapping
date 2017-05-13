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
            LoadMap(_book.RelationsMap);
        }

        private void LoadMap(RelationsMap map)
        {
            List<IIllustratable> elements = new List<IIllustratable>(_book.Characters.Count +
                                                                     _book.Locations.Count);
            elements.AddRange(_book.Characters);
            elements.AddRange(_book.Locations);

            foreach (string title in map.Elements)
            {
                IIllustratable element = elements.Find(e => e.Title == title);
                Node node = NodeFactory(element);
                node.DataContext = element;
                Point position = map.Coordinates[title];
                Canvas.SetLeft(node, position.X);
                Canvas.SetTop(node, position.Y);
                Map.Children.Add(node);
                node.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                node.Arrange(new Rect(0, 0, node.DesiredSize.Width, node.DesiredSize.Height));
                node.AnchorPoint = new Point(position.X + node.ActualWidth/2,
                                             position.Y + node.ActualHeight/2);
            }

            foreach (LinkInfo info in map.Links)
            {
                IElement sourceElement = elements.Find(e => e.Title == info.SourceNodeName);
                IElement destinationElement = elements.Find(e => e.Title == info.DestinationNodeName);
                Node sourceNode = Map.Children.Cast<UIElement>().ToList()
                                       .Where(c => c is Node).ToList()
                                       .Find(n => ((n as Node).DataContext as IElement)
                                                    .Title == sourceElement.Title) as Node;
                Node destinationNode = Map.Children.Cast<UIElement>().ToList()
                                       .Where(c => c is Node).ToList()
                                       .Find(n => ((n as Node).DataContext as IElement)
                                                    .Title == destinationElement.Title) as Node;

                LinkNodes(sourceNode, destinationNode, info.LabelText);
            }
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
                SaveMap();
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

        private void LinkNodes(Node sourceNode, Node destinationNode, string label)
        {
            Link link = new Link();
            link.DataContext = new Node[] { sourceNode, destinationNode };
            link.SetBinding(Link.SourceProperty,
                            new Binding()
                            {
                                Source = sourceNode,
                                Path = new PropertyPath(Node.AnchorPointProperty)
                            });
            link.SetBinding(Link.DestinationProperty,
                            new Binding()
                            {
                                Source = destinationNode,
                                Path = new PropertyPath(Node.AnchorPointProperty)
                            });
            link.Label.SetBinding(TextBox.MarginProperty,
                            new Binding()
                            {
                                Source = link,
                                Path = new PropertyPath(Link.LabelPositionProperty),
                                Converter = new AnchorPointToMarginConverter()
                            });
            link.Label.Text = label;
            Map.Children.Add(link);
        }

        private void SaveMap()
        {
            _book.RelationsMap.Elements.Clear();
            _book.RelationsMap.Coordinates.Clear();
            _book.RelationsMap.Links.Clear();

            foreach (UIElement element in Map.Children)
            {
                if (element is Node)
                {
                    _book.RelationsMap.Elements.Add(((element as Node).DataContext as IElement).Title);
                    _book.RelationsMap.Coordinates.Add(((element as Node).DataContext as IElement).Title,
                                                        new Point((element as Node).AnchorPoint.X - (element as Node).ActualWidth / 2,
                                                                  (element as Node).AnchorPoint.Y - (element as Node).ActualHeight / 2));
                }
                else if (element is Link)
                    _book.RelationsMap.Links.Add(new LinkInfo
                                                {
                                                     SourceNodeName = (((element as Link)
                                                                         .DataContext as Node[])[0]
                                                                         .DataContext as IElement).Title,
                                                     DestinationNodeName = (((element as Link)
                                                                         .DataContext as Node[])[1]
                                                                         .DataContext as IElement).Title,
                                                                 LabelText = (element as Link).Label.Text
                                                });
            }
            Bookshelf.Books.Save();
        }

        private void Node_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _focusedCharacter = sender as Node;
        }

        private void Node_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            (sender as Node).ReleaseMouseCapture();
            SaveMap();
            Bookshelf.Books.Save();
        }

        private void Node_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_currentState == MapState.Link)
            {
                LinkNodes(_focusedCharacter, sender as Node, string.Empty);
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
