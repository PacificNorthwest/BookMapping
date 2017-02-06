using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayProject.Contracts;
using System.Drawing;

namespace MayProject.Books
{
    public abstract class AbstractIllustratableElement : AbstractBookElement, IIllustratable
    {
        public List<Bitmap> Illustrations { get; set; }
        public abstract void AddIllustration(string path);
        public abstract void RemoveIllustration();
    }
}
