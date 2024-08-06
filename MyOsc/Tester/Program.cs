using FourierAnalyzer;
using System;
using System.IO;
using WavIO;

namespace MyOsc.Tester
{
    public class Program
    {
        private static void Main(string[] args)
        {
            AnalyzeSignal("Music 001", 0, 1.0f, 20000.0f, 1.0f, 0);
        }

        private static void AnalyzeSignal(string _directoryName, int _srcDecibel, float _freqMin, float _freqMax, float _df, int _idxChannel)
        {
            string srcPath = GetPath(_directoryName, $"{_srcDecibel}dB.wav");
            WavFile wav = WavFile.Load(srcPath);

            FileStream fs = new FileStream("C:\\Programming\\Oscillator\\FT.txt", FileMode.Create, FileAccess.Write);
            StreamWriter wr = new StreamWriter(fs);

            float[] coefficients = FourierTransformer.FT(wav, _freqMin, _freqMax, _df, _idxChannel);

            wr.WriteLine("DC: {0}", coefficients[0]);
            wr.WriteLine("F == <frequency>: (<amplitude>, <phase>)");

            float maxA = -1.0f;
            float maxF = -1.0f;

            for (int i = 1; i < coefficients.Length; i += 2)
            {
                float amp = MathF.Sqrt(coefficients[i] * coefficients[i] + coefficients[i + 1] * coefficients[i + 1]);
                float phase = MathF.Atan(coefficients[i + 1] / coefficients[i]) * (180.0f / MathF.PI);
                float freq = _freqMin + _df * (i / 2);

                if (amp > maxA)
                {
                    maxA = amp;
                    maxF = freq;
                }

                wr.WriteLine("F == {0}: ({1}, {2})", freq, amp, phase);
            }

            wr.WriteLine("MaxFreq == {0}, MaxAmp == {1}", maxF, maxA);
            wr.Close();
        }

        private static void CopyWav(string _directoryName, int _srcDecibel, int _dstDecibel)
        {
            string srcPath = GetPath(_directoryName, $"{_srcDecibel}dB.wav");
            string dstPath = GetPath(_directoryName, $"{_dstDecibel}dB.wav");

            WavFile wav = WavFile.Load(srcPath);
            wav.header.SetDataSize(wav.data.Length);

            int bytesPerSample = wav.header.bitsPerSample / 8;

            for(int i = 0; i < wav.header.dataSize; i += bytesPerSample)
            {
                if (bytesPerSample == 1)
                {
                    sbyte data = (sbyte)wav.data[i];
                    data /= 10;
                    wav.data[i] = (byte)data;
                }
                else if(bytesPerSample == 2)
                {
                    short data = BitConverter.ToInt16(wav.data, i);
                    data /= 10;
                    byte[] bytes = BitConverter.GetBytes(data);

                    wav.data[i] = bytes[0];
                    wav.data[i + 1] = bytes[1];
                }
            }

            WavFile.Create(dstPath, wav);
        }

        private static void LogWav(WavFile _wavFile)
        {
            Console.WriteLine("RIFF: {0}", _wavFile.header.RIFF);
            Console.WriteLine("fileSize: {0}", _wavFile.header.fileSize);
            Console.WriteLine("WAVE: {0}", _wavFile.header.WAVE);
            Console.WriteLine("fmtSpace: {0}", _wavFile.header.fmtSpace);
            Console.WriteLine("lenFormatData: {0}", _wavFile.header.lenFormatData);
            Console.WriteLine("formatType: {0}", _wavFile.header.formatType);
            Console.WriteLine("numChannel: {0}", _wavFile.header.numChannel);
            Console.WriteLine("sampleRate: {0}", _wavFile.header.sampleRate);
            Console.WriteLine("sbc: {0}", _wavFile.header.sbc);
            Console.WriteLine("bc: {0}", _wavFile.header.bc);
            Console.WriteLine("bitsPerSample: {0}", _wavFile.header.bitsPerSample);
            Console.WriteLine("data: {0}", _wavFile.header.data);
            Console.WriteLine("dataSize: {0}", _wavFile.header.dataSize);
        }

        private static string GetPath(string _directoryName, string _fileName)
        {
            return $"C:\\Programming\\Oscillator\\WavSample\\{_directoryName}\\{_fileName}";
        }
    }
}