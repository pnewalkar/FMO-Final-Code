using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.NYBLoader.Interfaces
{
    public interface IFileMover
    {
        void MoveFile(string[] source, string[] destination);
    }
}
