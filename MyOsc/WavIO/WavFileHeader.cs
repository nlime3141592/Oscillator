namespace WavIO
{
    public struct WavFileHeader
    {
        public string RIFF;
        public uint fileSize;
        public string WAVE;
        public string fmtSpace;
        public uint lenFormatData;
        public ushort formatType;
        public ushort numChannel;
        public uint sampleRate;
        public uint sbc; // NOTE: (sampleRate * bitsPerSample * numChannel) / 8
        public ushort bc; // NOTE: (bitsPerSample * numChannel) / 8
        public ushort bitsPerSample;
        public string data;
        public uint dataSize;

        public void SetDataSize(uint _dataSize)
        {
            dataSize = _dataSize;
            fileSize = _dataSize + 36;
        }

        public void SetSampleRate(uint _sampleRate)
        {
            sampleRate = _sampleRate;

            bc = (ushort)(bitsPerSample * numChannel);
            sbc = sampleRate * bc;
        }

        public void SetBitsPerSample(ushort _bitsPerSample)
        {
            bitsPerSample = _bitsPerSample;

            bc = (ushort)(bitsPerSample * numChannel);
            sbc = sampleRate * bc;
        }

        public void SetNumChannel(ushort _numChannel)
        {
            numChannel = _numChannel;

            bc = (ushort)(bitsPerSample * numChannel);
            sbc = sampleRate * bc;
        }
    }
}
