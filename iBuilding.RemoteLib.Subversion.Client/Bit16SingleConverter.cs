using System;

namespace RanOpt.Common.RemoteLib.Subversion.Client
{
    public class Bit16SingleConverter
    {
        private float _divisor = 1;
        private float _maxValue;
        private float _minValue = -400f;
        private int _precision = 1;

        public float MaxValue => _maxValue;

        public float MinValue
        {
            get { return _minValue; }
            set { _minValue = value; Initialize(); }
        }

        public int Precision
        {
            get { return _precision; }
            set { _precision = value; Initialize(); }
        }


        public Bit16SingleConverter()
        {
            Initialize();
        }

        private void Initialize()
        {
            for (var i = 0; i < Precision; i++)
            {
                _divisor *= 10;
            }
            _maxValue = ushort.MaxValue / _divisor + MinValue;
        }

        public byte[] GetBytes(float value)
        {
            value = (value + MinValue) * _divisor;
            return BitConverter.GetBytes((ushort)value);
        }
        public float ToSingle(byte[] bytes)
        {
            var valUInt16 = BitConverter.ToUInt16(bytes, 0);
            return valUInt16 / _divisor - MinValue;
        }
    }
}