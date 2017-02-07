using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MayProject.Contracts
{
    interface IIllustratable : IElement
    {
        List<Bitmap> Illustrations { get; set; }
    }
}
