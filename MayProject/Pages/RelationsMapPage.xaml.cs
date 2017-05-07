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
using SharpGL;
using SharpGL.WPF;

namespace MayProject.Pages
{
    /// <summary>
    /// Логика взаимодействия для RelationsMapPage.xaml
    /// </summary>
    public partial class RelationsMapPage : UserControl
    {
        public RelationsMapPage()
        {
            InitializeComponent();
        }

        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            var gl = args.OpenGL;
            gl.Clear(OpenGL.Gl.COLOR_BUFFER_BIT | OpenGL.Gl.DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            gl.Enable(OpenGL.Gl.MULTISAMPLE);
            gl.Enable(OpenGL.Gl.TEXTURE_2D);

            gl.Rotate(0.0f, -170.0f, 0.0f);
            gl.Rotate(-60.0f, 0.0f, 0.0f);
            gl.Translate(0.5, 0.5, 0);

            
            gl.Begin(OpenGL.Gl.QUADS);

            gl.Color(0.0f, 1.0f, 1.0f);
            gl.Vertex(0.17f, 0.5f, 0.0f);
            gl.Vertex(-0.2f, 0.5f, 0.0f);
            gl.Vertex(-0.2f, -0.5f, 0.0f);
            gl.Vertex(0.2f, -0.5f, 0.0f);

            gl.Color(0.0f, 0.5f, 1.0f);
            gl.Vertex(0.2f, -0.5f, 0.0f);
            gl.Vertex(0.2f, -0.5f, -0.03f);
            gl.Vertex(-0.2f, -0.5f, -0.03f);
            gl.Vertex(-0.2f, -0.5f, 0.0f);

            gl.Color(0.0f, 0.1f, 1.0f);
            gl.Vertex(-0.2f, -0.5f, 0.0f);
            gl.Vertex(-0.2f, -0.5f, -0.03f);
            gl.Vertex(-0.2f, 0.5f, -0.03f);
            gl.Vertex(-0.2f, 0.5f, 0.0f);
            gl.End();
            gl.Flush();
        }

        private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            var gl = args.OpenGL;
            gl.ClearColor(0.3f, 0.3f, 0.3f, 0.3f);
        }

        private void OpenGLControl_Resized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {

        }
    }
}
