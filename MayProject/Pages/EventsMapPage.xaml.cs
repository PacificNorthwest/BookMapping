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

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для EventsMap.xaml
    /// </summary>
    public partial class EventsMapPage : UserControl
    {
        private MapState _currentState;
        private EventNode _focusedNode;
        private Book _book;
        private Point _m_start;
        private Vector _m_startOffset;

        public EventsMapPage(Book book)
        {
            _book = book;
            InitializeComponent();
        }

        private void NewEventButton_Click(object sender, RoutedEventArgs e)
        {
            NewEventWindow window = new NewEventWindow(_book);
            window.ShowDialog();
            if (window.CreatedEvent != null)
            {
                AddEvent(window.CreatedEvent);
            }
        }

        private void AddEvent(Event storyEvent)
        {
            EventNode node = new EventNode(Map,
                                           storyEvent.Title,
                                           storyEvent.Description,
                                           storyEvent.Characters,
                                           storyEvent.Location,
                                           "12:00");
            node.ContextMenu = this.FindResource("NodeContextMenu") as ContextMenu;
            node.MouseDown += Node_PreviewMouseDown;
            node.PreviewMouseMove += Node_PreviewMouseMove;
            node.PreviewMouseUp += Node_PreviewMouseUp;
            node.MouseRightButtonDown += Node_MouseRightButtonDown;
            Map.Children.Add(node);
        }

        private void LinkNodes(EventNode sourceNode, EventNode destinationNode, string label)
        {
            Link link = new Link();
            link.DataContext = new EventNode[] { sourceNode, destinationNode };
            link.SetBinding(Link.SourceProperty,
                            new Binding()
                            {
                                Source = sourceNode,
                                Path = new PropertyPath(EventNode.AnchorPointProperty)
                            });
            link.SetBinding(Link.DestinationProperty,
                            new Binding()
                            {
                                Source = destinationNode,
                                Path = new PropertyPath(EventNode.AnchorPointProperty)
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

        private void Node_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _focusedNode = sender as EventNode;
        }

        private void Node_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            (sender as EventNode).ReleaseMouseCapture();
        }

        private void Node_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_currentState == MapState.Link)
            {
                LinkNodes(_focusedNode, sender as EventNode, string.Empty);
                _currentState = MapState.Normal;
            }
            _m_start = e.GetPosition(Map);
            _m_startOffset = new Vector(((sender as EventNode).RenderTransform as TranslateTransform).X,
                                        ((sender as EventNode).RenderTransform as TranslateTransform).Y);
            (sender as EventNode).CaptureMouse();
        }

        private void Node_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if ((sender as EventNode).IsMouseCaptured)
            {
                Vector offset = Point.Subtract(e.GetPosition(Map), _m_start);

                ((sender as EventNode).RenderTransform as TranslateTransform).X = _m_startOffset.X + offset.X;
                ((sender as EventNode).RenderTransform as TranslateTransform).Y = _m_startOffset.Y + offset.Y;
                (sender as EventNode).UpdateAnchor();
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
