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
    /// Карта событий
    /// </summary>
    public partial class EventsMapPage : UserControl, ISideMenuHandler
    {
        private MapState _currentState;
        private EventNode _focusedNode;
        private Book _book;
        private Point _m_start;
        private Vector _m_startOffset;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        /// <param name="book"></param>
        public EventsMapPage(Book book)
        {
            _book = book;
            InitializeComponent();
            PopulateSideMenu();
            LoadMap(_book.EventsMap);
        }

        /// <summary>
        /// Заполнение боковго меню контекстными данными
        /// </summary>
        public void PopulateSideMenu()
        {
            var menu = new EventsMapSideMenu();
            menu.RelationsMapSwitch.Click += RelationsMapSwitch_Click;
            menu.SideMenuEvents.Children.Clear();


            foreach (var bookEvent in _book.Events)
            {
                Button button = new Button()
                {
                    Content = bookEvent.Title,
                    Background = new SolidColorBrush(Color.FromArgb(255, 236, 235, 231)),
                    FontSize = 20,
                    Height = 70,
                    DataContext = bookEvent
                };

                button.PreviewMouseMove += Button_PreviewMouseMove;
                menu.SideMenuEvents.Children.Add(button);
            }

            MainWindow.SelectedTab.SideMenu.Content = menu;
        }

        /// <summary>
        /// Чтение карты из модели данных
        /// </summary>
        /// <param name="map">Карта для чтения</param>
        private void LoadMap(Map map)
        {
            List<Event> events = _book.Events;

            foreach (string title in map.Elements)
            {
                Event bookEvent = events.Find(e => e.Title == title);
                EventNode node = NodeFactory(bookEvent);
                node.DataContext = bookEvent;
                Point position = map.Coordinates[title];
                Canvas.SetLeft(node, position.X);
                Canvas.SetTop(node, position.Y);
                Map.Children.Add(node);
                node.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                node.Arrange(new Rect(0, 0, node.DesiredSize.Width, node.DesiredSize.Height));
                node.AnchorPoint = new Point(position.X + node.ActualWidth / 2 + 50,
                                             position.Y + node.ActualHeight / 2 + 120);
            }

            foreach (LinkInfo info in map.Links)
            {
                Event sourceElement = events.Find(e => e.Title == info.SourceNodeName);
                Event destinationElement = events.Find(e => e.Title == info.DestinationNodeName);
                EventNode sourceNode = Map.Children.Cast<UIElement>().ToList()
                                       .Where(c => c is EventNode).ToList()
                                       .Find(n => ((n as EventNode).DataContext as Event)
                                       .Title == sourceElement.Title) as EventNode;
                EventNode destinationNode = Map.Children.Cast<UIElement>().ToList()
                                       .Where(c => c is EventNode).ToList()
                                       .Find(n => ((n as EventNode).DataContext as Event)
                                       .Title == destinationElement.Title) as EventNode;

                LinkNodes(sourceNode, destinationNode, info.LabelText);
            }
        }

        /// <summary>
        /// Обработчик события движения курсора мыши над элементом боковго меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject data = new DataObject();
                data.SetData("Event", (sender as Button).DataContext as Event);

                DragDrop.DoDragDrop(this, data, DragDropEffects.Move | DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// Обработчик события визуализации контекста перетаскивания обьекта
        /// </summary>
        /// <param name="e"></param>
        protected override void OnGiveFeedback(GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
            if (e.Effects.HasFlag(DragDropEffects.Copy))
            {
                Mouse.SetCursor(Cursors.Cross);
            }
            else if (e.Effects.HasFlag(DragDropEffects.Move))
            {
                Mouse.SetCursor(Cursors.Pen);
            }
            else
            {
                Mouse.SetCursor(Cursors.No);
            }
            e.Handled = true;
        }

        /// <summary>
        /// Обработчик события перетаскивания элемента в рабочую область карты
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent("Event") &&
                !(Map.Children.Cast<UIElement>().ToList()
                .Where(c => c is EventNode).ToList()
                .Exists(c => ((c as EventNode).DataContext as Event).Title ==
                             (e.Data.GetData("Event") as Event).Title)))
            {
                EventNode node = NodeFactory(e.Data.GetData("Event") as Event);
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

        /// <summary>
        /// Сохранение содержимого рабочей области в модель данных
        /// </summary>
        private void SaveMap()
        {
            _book.EventsMap.Elements.Clear();
            _book.EventsMap.Coordinates.Clear();
            _book.EventsMap.Links.Clear();

            foreach (UIElement element in Map.Children)
            {
                if (element is EventNode)
                {
                    EventNode node = element as EventNode;
                    _book.Events[_book.Events.FindIndex(n => n.Title == (node.DataContext as Event).Title)] =
                         new Event(node.EventTitle.Text,
                                   node.EventDescription.Text,
                                   node.EventTime.Text,
                                   _book.Characters.Where(
                                                    c => node.EventCharacters.Text
                                                         .Split(new string[] { "\n" }, StringSplitOptions.None)
                                                         .Contains(c.Title)).ToList(),
                                   _book.Locations.Find(
                                                    c => node.EventLocation.Text == c.Title));

                    _book.EventsMap.Elements.Add((node.DataContext as Event).Title);
                    _book.EventsMap.Coordinates.Add((node.DataContext as Event).Title,
                                                        new Point(node.AnchorPoint.X - (node.ActualWidth / 2 + 50),
                                                                  node.AnchorPoint.Y - (node.ActualHeight / 2 + 120)));
                }
                else if (element is Link)
                    _book.EventsMap.Links.Add(new LinkInfo
                    {
                        SourceNodeName = (((element as Link)
                                                .DataContext as EventNode[])[0]
                                                .DataContext as Event).Title,
                        DestinationNodeName = (((element as Link)
                                                        .DataContext as EventNode[])[1]
                                                        .DataContext as Event).Title,
                        LabelText = (element as Link).Label.Text
                    });
            }
            Bookshelf.Books.Save();
        }

        /// <summary>
        /// Обработчик события перехода на карту связей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RelationsMapSwitch_Click(object sender, RoutedEventArgs e)
        {
            PageSwitcher.Switch(new RelationsMapPage(_book));
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку создания нового книжного события
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewEventButton_Click(object sender, RoutedEventArgs e)
        {
            NewEventWindow window = new NewEventWindow(_book);
            window.ShowDialog();
            if (window.CreatedEvent != null)
            {
                _book.Events.Add(window.CreatedEvent);
                PopulateSideMenu();
            }
        }

        /// <summary>
        /// Фабричный класс для создания узлов событий
        /// </summary>
        /// <param name="storyEvent"></param>
        /// <returns></returns>
        private EventNode NodeFactory(Event storyEvent)
        {
            EventNode node = new EventNode(Map,
                                           storyEvent.Title,
                                           storyEvent.Description,
                                           storyEvent.Characters,
                                           storyEvent.Location,
                                           storyEvent.Time);
            node.DataContext = storyEvent;
            node.ContextMenu = this.FindResource("NodeContextMenu") as ContextMenu;
            node.MouseDown += Node_PreviewMouseDown;
            node.PreviewMouseMove += Node_PreviewMouseMove;
            node.PreviewMouseUp += Node_PreviewMouseUp;
            node.MouseRightButtonDown += Node_MouseRightButtonDown;
            return node;
        }

        /// <summary>
        /// Установка связи между двумя узлами
        /// </summary>
        /// <param name="sourceNode">Начальный узел</param>
        /// <param name="destinationNode">Конечный узел</param>
        /// <param name="label">Подпись связи</param>
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

        /// <summary>
        /// Обработчик события нажатия правой клавишей мыши на узле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Node_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _focusedNode = sender as EventNode;
        }

        /// <summary>
        /// Обработчик события отпускания клавиши мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Node_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            (sender as EventNode).ReleaseMouseCapture();
            SaveMap();
            Bookshelf.Books.Save();
        }

        /// <summary>
        /// Обработчик события нажатия клавишей мыши на узле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Обработчик события передвижения курсора мыши над узлом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Обработчик события вхождения курсора мыши в область кнопки контекстного меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuButton_MouseOver(object sender, RoutedEventArgs e)
        {
            ((((sender as Button).Parent as Grid).Children
                .Cast<UIElement>()
                .First(x => x.GetType() == typeof(Viewbox)) as Viewbox).Child as TextBlock)
                .Text = (sender as Button).DataContext as string;
        }

        /// <summary>
        /// Обработчик события выхода курсора мыши из области кнопки контекстного меню
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuButton_MouseLeave(object sender, RoutedEventArgs e)
        {
            ((((sender as Button).Parent as Grid).Children
                .Cast<UIElement>()
                .First(x => x.GetType() == typeof(Viewbox)) as Viewbox).Child as TextBlock)
                .Text = string.Empty;
        }

        /// <summary>
        /// Обработчик события нажатия на кнопку установления свзяи между узлами
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLink_Click(object sender, RoutedEventArgs e)
        {
            _currentState = MapState.Link;
        }
    }
}
