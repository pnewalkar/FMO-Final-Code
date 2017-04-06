using Fmo.NYBLoader.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.NYBLoader.Common
{
    public class FileMover : IFileMover
    {
        public void MoveFile(string[] source, string[] destination)
        {
            string sourceFilePath =  Path.Combine(source);
            string destinationFilePath = Path.Combine(destination);

            File.Move(sourceFilePath, destinationFilePath);
        }
    }
}
