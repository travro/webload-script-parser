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
            _r = 180;
            _g = 255;
            _b = 0;
            _step = 25;
            _stepsTaken = 0;
        }
        public ColorDispenser(byte step)
        {
            _r = 180;
            _g = 255;
            _b = 0;
            byte maxstep = 75;
            if (step > maxstep) step = maxstep;
            if (step < 5) step = 5;
            _step = step;
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
            _r = 150; _g = 255; _b = 0; _stepsTaken = 0;
        }
        private void Step()
        {
            if (_r > Byte.MinValue)
            {
                _r = (_r - _step > Byte.MinValue) ? _r -= _step : Byte.MinValue;
            }
            else if (_b < Byte.MaxValue)
            {
                _b = (_b + _step < Byte.MaxValue) ? _b += _step : Byte.MaxValue;
            }
            else
            {
                _g = (_g - _step > Byte.MinValue) ? _g -= _step : Byte.MinValue;
            }
            _stepsTaken++;
        }
    }
}
