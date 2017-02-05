using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MayProject.Contracts;

namespace MayProject.Books
{
    public abstract class AbstractIllustratableElement : AbstractBookElement, IIllustratable
    {
        public abstract void AddIllustration();
        public abstract void RemoveIllustration();
    }
}
