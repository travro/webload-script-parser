using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WLScriptParser.Utilities
{
    public class ColorDispenser
    {
        byte _r, _g, _b, _step;
        int _stepsTaken;

        public ColorDispenser()
        {
            _r = 255;
            _g = 180;
            _b = 0;
            _step = 10;
            _stepsTaken = 0;
        }
        public ColorDispenser(byte step)
        {
            _r = 255;
            _g = 180;
            _b = 0;
            byte maxstep = 12;
            byte minstep = 1;
            if (step > maxstep) step = maxstep; else _step = step;
            if (step < minstep) step = minstep; else _step = step;
            _stepsTaken = 0;
        }
        public Color GetNextColor()
        {
            byte currentRed = _r;
            byte currentGreen = _g;
            byte currentBlue = _b;
            Step();

            return Color.FromRgb(currentRed, currentGreen, currentBlue);
        }
        public Color GetColorBySeed(int i)
        {
            int seed = i;
            if (i == -1) return Color.FromRgb(255, 93, 93);

            if (seed < _stepsTaken) Reset();

            while(seed > _stepsTaken)
            {
                Step();
            }

            byte currentRed = _r;
            byte currentGreen = _g;
            byte currentBlue = _b;


            return Color.FromRgb(currentRed, currentGreen, currentBlue);
        }
        public void Reset()
        {
            _r = 255; _g = 180; _b = 0; _stepsTaken = 0;
        }
        private void Step()
        {
            //if (_r > Byte.MinValue)
            //{
            //    _r = (_r - _step > Byte.MinValue) ? _r -= _step : Byte.MinValue;
            //}
            //else if (_b < Byte.MaxValue)
            //{
            //    _b = (_b + _step < Byte.MaxValue) ? _b += _step : Byte.MaxValue;
            //}
            //else
            //{
            //    _g = (_g - _step > Byte.MinValue) ? _g -= _step : Byte.MinValue;
            //}

            if (_g + _step > Byte.MaxValue) _g = Byte.MaxValue; else _g += _step;
            if (_b + _step > Byte.MaxValue) _b = Byte.MaxValue; else _b += _step;


            _stepsTaken++;
        }
    }
}
