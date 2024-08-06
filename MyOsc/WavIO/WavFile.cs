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

        public static void Create(string _filePath, WavFile _wavFile)
        {
            FileStream fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write);
            BinaryWriter wr = new BinaryWriter(fs);
            WavFileWriter wwr = new WavFileWriter(wr);

            wwr.Save(_wavFile);
            wwr.Close();
        }
    }
}
