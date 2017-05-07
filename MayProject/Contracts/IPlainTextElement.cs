using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MayProject.Contracts
{
    interface IPlainTextElement : IElement
    {
        string Text { get; set; }
    }
}
