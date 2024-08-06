using System.IO;
using System.Text;

namespace WavIO
{
    public class WavFileReader
    {
        private WavFile m_wavFile;
        private BinaryReader m_reader;

        public WavFileReader(BinaryReader _reader)
        {
            m_reader = _reader;
        }

        public WavFile Load()
        {
            m_wavFile = new WavFile();

            while (m_reader.PeekChar() >= 0 && !ParseChunk()) ;

            return m_wavFile;
        }

        public void Close()
        {
            m_reader.Close();
        }

        private bool ParseChunk()
        {
            string chunk = Encoding.ASCII.GetString(m_reader.ReadBytes(4));

            switch(chunk)
            {
                case "RIFF":
                    m_wavFile.header.RIFF = chunk;
                    Read_RIFF();
                    return false;
                case "WAVE":
                    m_wavFile.header.WAVE = chunk;
                    Read_WAVE();
                    return false;
                case "fmt ":
                    m_wavFile.header.fmtSpace = chunk;
                    Read_fmtSpace();
                    return false;
                case "JUNK":
                    Read_JUNK();
                    return false;
                case "data":
                    m_wavFile.header.data = chunk;
                    Read_data();
                    return true;
                default:
                    return true;
            }
        }

        private void Read_RIFF()
        {
            int fileSize = m_reader.ReadInt32();

            m_wavFile.header.fileSize = fileSize;
        }

        private void Read_WAVE()
        {
            // NOTE: Do anything.
        }

        private void Read_JUNK()
        {
            int chunkSize = m_reader.ReadInt32();
            m_reader.BaseStream.Seek(chunkSize, SeekOrigin.Current);
        }

        private void Read_fmtSpace()
        {
            m_wavFile.header.lenFormatData = m_reader.ReadInt32();
            m_wavFile.header.formatType = m_reader.ReadInt16();
            m_wavFile.header.numChannel = m_reader.ReadInt16();
            m_wavFile.header.sampleRate = m_reader.ReadInt32();
            m_wavFile.header.sbc = m_reader.ReadInt32();
            m_wavFile.header.bc = m_reader.ReadInt16();
            m_wavFile.header.bitsPerSample = m_reader.ReadInt16();
        }

        private void Read_data()
        {
            m_wavFile.header.dataSize = m_reader.ReadInt32();
            m_wavFile.data = m_reader.ReadBytes((int)m_wavFile.header.dataSize);
        }
    }
}
