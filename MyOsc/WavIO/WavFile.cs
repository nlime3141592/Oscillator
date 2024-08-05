using System.IO;

namespace WavIO
{
    public class WavFile
    {
        public WavFileHeader header;
        public byte[] data;

        public static WavFile Load(string _filePath)
        {
            FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            BinaryReader rd = new BinaryReader(fs);
            WavFileReader wrd = new WavFileReader(rd);

            WavFile file = wrd.Load();
            wrd.Close();

            return file;
        }
    }
}
