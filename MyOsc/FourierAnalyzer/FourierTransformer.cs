using System;
using WavIO;

namespace FourierAnalyzer
{
    public class FourierTransformer
    {
        public static float[] FT(WavFile _wav, float _freqMin, float _freqMax, float _df, int _idxChannel)
        {

            // NOTE: Fourier Coefficients.
            int numC = (int)((_freqMax - _freqMin) / _df);
            float[] coefficients = new float[1 + 2 * numC];

            for(int i = 0; i < _wav.header.dataSize; i += _wav.header.bc)
            {
                int offset = i + _wav.header.bc * _idxChannel;
                float time = (float)(offset) / _wav.header.sbc;
                float data;
                // Console.WriteLine("time == {0} sec", time);

                if(_wav.header.bitsPerSample == 8)
                    data = (_wav.data[offset] - 128) / 128.0f;
                else if(_wav.header.bitsPerSample == 16)
                    data = BitConverter.ToInt16(_wav.data, offset) / 32768.0f;
                else
                    throw new NotSupportedException("Not support bitsPerSample.");

                coefficients[0] += data;

                for(int j = 0; j < numC; ++j)
                {
                    float freq = _freqMin + _df * j;
                    float radian = 2.0f * MathF.PI * freq * time;
                    coefficients[2 * j + 1] += data * MathF.Cos(radian);
                    coefficients[2 * j + 2] += data * MathF.Sin(radian);
                }
            }

            float signalLength = (float)_wav.header.dataSize / _wav.header.bc;

            for(int j = 0; j < numC; ++j)
            {
                coefficients[2 * j + 1] /= (0.5f * signalLength);
                coefficients[2 * j + 2] /= (0.5f * signalLength);
            }

            coefficients[0] /= signalLength;

            return coefficients;
        }
    }
}
