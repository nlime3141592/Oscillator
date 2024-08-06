using System;
using WavIO;

namespace MyOsc.Tester
{
    public class Program
    {
        private static void Main(string[] args)
        {
            string wavFilePath = @"C:\Programming\Oscillator\WavSample\Sine0dB.wav";
            string wavFilePath2 = @"C:\Programming\Oscillator\WavSample\Sine-20dB.wav";

            WavFile wavFile = WavFile.Load(wavFilePath);

            Console.WriteLine("RIFF: {0}", wavFile.header.RIFF);
            Console.WriteLine("fileSize: {0}", wavFile.header.fileSize);
            Console.WriteLine("WAVE: {0}", wavFile.header.WAVE);
            Console.WriteLine("fmtSpace: {0}", wavFile.header.fmtSpace);
            Console.WriteLine("lenFormatData: {0}", wavFile.header.lenFormatData);
            Console.WriteLine("formatType: {0}", wavFile.header.formatType);
            Console.WriteLine("numChannel: {0}", wavFile.header.numChannel);
            Console.WriteLine("sampleRate: {0}", wavFile.header.sampleRate);
            Console.WriteLine("sbc: {0}", wavFile.header.sbc);
            Console.WriteLine("bc: {0}", wavFile.header.bc);
            Console.WriteLine("bitsPerSample: {0}", wavFile.header.bitsPerSample);
            Console.WriteLine("data: {0}", wavFile.header.data);
            Console.WriteLine("dataSize: {0}", wavFile.header.dataSize);

            wavFile.header.SetDataSize(wavFile.data.Length);

            for(int i = 0; i < wavFile.header.dataSize; i += 2)
            {
                short data = BitConverter.ToInt16(wavFile.data, i);
                data /= 10;
                byte[] bytes = BitConverter.GetBytes(data);

                wavFile.data[i] = bytes[0];
                wavFile.data[i + 1] = bytes[1];
            }

            WavFile.Create(wavFilePath2, wavFile);
        }
    }
}