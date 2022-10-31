using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactorySaveParser
{
    internal class FSaveHeader
    {

        public void Parse(FileStream SaveFile)
        {
            // encoding is zlib CINFO(windowsize) 7
            // encoding is zlib FLEVEL(compression level) 3
            // visit https://stackoverflow.com/a/54915442/20364636 for additional info

            FileStream OutputStream = File.Create("C:/Users/gerha/AppData/Local/SatisfactorySaveparser-Reloaded/DecompressedSave.txt");
            var decompressor = new DeflateStream(SaveFile, CompressionMode.Decompress);
            decompressor.CopyTo(OutputStream);
        }
    }
}
