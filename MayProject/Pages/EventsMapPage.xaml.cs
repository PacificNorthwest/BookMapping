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
            EventNode node = new EventNode();
            node.EventTitle.Text = storyEvent.Title;
            node.EventDescription.Text = storyEvent.Description;
            node.EventCharacters.Text = storyEvent.Characters
                                                  .Select(c => c.Title).ToList()
                                                  .Aggregate((current, next) => $"{current}\n{next}");
            node.EventLocation.Text = storyEvent.Location.Title;
            node.EventTime.Text = "Whenever";
            node.MouseDown += Node_PreviewMouseDown;
            node.PreviewMouseMove += Node_PreviewMouseMove;
            node.PreviewMouseUp += Node_PreviewMouseUp;
            Map.Children.Add(node);
        }

        private void Node_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            (sender as EventNode).ReleaseMouseCapture();
        }

        private void Node_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
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
            }
        }
    }
}
