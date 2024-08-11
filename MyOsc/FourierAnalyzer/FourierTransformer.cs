using System;
using WavIO;

namespace FourierAnalyzer
{
    public class FourierTransformer
    {
        public static float[] FFT(WavFile _wav, float _freqMin, float _freqMax, float _df, int _idxChannel)
        {
            int numC = (int)((_freqMax - _freqMin) / _df);
            float[] coefficients = new float[1 + 2 * numC];
            int totalSampleCount = _wav.header.dataSize / _wav.header.bc;
            int nextSampleCount = totalSampleCount / 2;

            for(int j = 0; j < numC; ++j)
            {
                float freq = _freqMin + _df * j;

                rec_FFT(_wav, _idxChannel, coefficients, j, freq, 0, totalSampleCount - nextSampleCount, 2);
                rec_FFT(_wav, _idxChannel, coefficients, j, freq, 1, nextSampleCount, 2);
            }

            coefficients[0] = 0.0f;
            return coefficients;
        }

        private static void rec_FFT(WavFile _wav, int _idxChannel, float[] _coefficients, int _targetFreqIndex, float _targetFreq, int _idxBegin, int _sampleCount, int _deltaSample)
        {
            Console.WriteLine(_sampleCount);
            if(_sampleCount > 2)
            {
                int nextSampleCount = _sampleCount / 2;
                int nextDeltaSample = 2 * _deltaSample;

                rec_FFT(_wav, _idxChannel, _coefficients, _targetFreqIndex, _targetFreq, _idxBegin, _sampleCount - nextSampleCount, nextDeltaSample);
                rec_FFT(_wav, _idxChannel, _coefficients, _targetFreqIndex, _targetFreq, _idxBegin + _deltaSample, nextSampleCount, nextDeltaSample);
            }

            float dRadian = 2.0f * MathF.PI * _targetFreq;
            float radian = 0.0f;
            int offset = _idxBegin * _wav.header.bc + _wav.header.bitsPerSample * _idxChannel;

            for(int j = 0; j < _sampleCount; ++j)
            {
                float cos = MathF.Cos(radian);
                float sin = MathF.Sin(radian);
                float data;

                if(_wav.header.bitsPerSample == 8)
                    data = (_wav.data[offset] - 128) / 128.0f;
                else if(_wav.header.bitsPerSample == 16)
                    data = BitConverter.ToInt16(_wav.data, offset) / 32768.0f;
                else
                    throw new NotSupportedException("Not support bitsPerSample.");

                _coefficients[2 * _targetFreqIndex + 1] += data * cos;
                _coefficients[2 * _targetFreqIndex + 2] -= data * sin;

                radian += dRadian;
                radian /= MathF.PI * 2.0f;
                offset += _wav.header.bc * _deltaSample;
            }
        }

        public static float[] DFT(WavFile _wav, float _freqMin, float _freqMax, float _df, int _idxChannel)
        {
            // NOTE: Fourier Coefficients.
            int numC = (int)((_freqMax - _freqMin) / _df);
            float[] coefficients = new float[1 + 2 * numC];

            for(int i = 0; i < _wav.header.dataSize; i += _wav.header.bc)
            {
                int offset = i + _wav.header.bitsPerSample * _idxChannel;
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
