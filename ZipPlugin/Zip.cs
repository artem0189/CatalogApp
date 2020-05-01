using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginInterface;
using System.IO;
using System.IO.Compression;

namespace ZipPlugin
{
    class Zip : IPlugin
    {
        public string Name { get; } = "Zip";
        public string Extension { get; } = ".myzip";
        public byte[] OnSave(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Compress))
                {
                    zipStream.Write(data, 0, data.Length);
                }
                return stream.ToArray();
            }
        }

        public byte[] OnLoad(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
    }
}
