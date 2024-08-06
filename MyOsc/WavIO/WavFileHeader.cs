namespace WavIO
{
    public struct WavFileHeader
    {
        public string RIFF;
        public int fileSize;
        public string WAVE;
        public string fmtSpace;
        public int lenFormatData;
        public short formatType;
        public short numChannel;
        public int sampleRate;
        public int sbc; // NOTE: (sampleRate * bitsPerSample * numChannel) / 8
        public short bc; // NOTE: (bitsPerSample * numChannel) / 8
        public short bitsPerSample;
        public string data;
        public int dataSize;

        public void SetDataSize(int _dataSize)
        {
            dataSize = _dataSize;
            fileSize = _dataSize + 36;
        }

        public void SetSampleRate(int _sampleRate)
        {
            sampleRate = _sampleRate;

            bc = (short)(bitsPerSample * numChannel);
            sbc = sampleRate * bc;
        }

        public void SetBitsPerSample(short _bitsPerSample)
        {
            bitsPerSample = _bitsPerSample;

            bc = (short)(bitsPerSample * numChannel);
            sbc = sampleRate * bc;
        }

        public void SetNumChannel(short _numChannel)
        {
            numChannel = _numChannel;

            bc = (short)(bitsPerSample * numChannel);
            sbc = sampleRate * bc;
        }
    }
}
