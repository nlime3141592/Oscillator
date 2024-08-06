using System.IO;
using System.Text;

namespace WavIO
{
    public class WavFileWriter
    {
        private WavFile m_wavFile;
        private BinaryWriter m_writer;

        public WavFileWriter(BinaryWriter _writer)
        {
            m_writer = _writer;
        }

        public void Save(WavFile _wavFile)
        {
            m_wavFile = _wavFile;

            Write_RIFF();
            Write_WAVE();
            Write_JUNK(28);
            Write_fmtSpace();
            Write_data();
        }

        public void Close()
        {
            m_writer.Close();
        }

        private void Write_RIFF()
        {
            m_writer.Write(Encoding.ASCII.GetBytes(m_wavFile.header.RIFF));
            m_writer.Write(m_wavFile.header.fileSize + 28 + 8);
        }

        private void Write_JUNK(int _lenJunkData)
        {
            m_writer.Write(Encoding.ASCII.GetBytes("JUNK"));
            m_writer.Write(_lenJunkData);
            m_writer.Seek(_lenJunkData, SeekOrigin.Current);
        }

        private void Write_WAVE()
        {
            m_writer.Write(Encoding.ASCII.GetBytes(m_wavFile.header.WAVE));
        }

        private void Write_fmtSpace()
        {
            m_writer.Write(Encoding.ASCII.GetBytes(m_wavFile.header.fmtSpace));
            m_writer.Write(m_wavFile.header.lenFormatData);
            m_writer.Write(m_wavFile.header.formatType);
            m_writer.Write(m_wavFile.header.numChannel);
            m_writer.Write(m_wavFile.header.sampleRate);
            m_writer.Write(m_wavFile.header.sbc);
            m_writer.Write(m_wavFile.header.bc);
            m_writer.Write(m_wavFile.header.bitsPerSample);
        }

        private void Write_data()
        {
            m_writer.Write(Encoding.ASCII.GetBytes(m_wavFile.header.data));
            m_writer.Write(m_wavFile.header.dataSize);
            m_writer.Write(m_wavFile.data);
        }
    }
}
