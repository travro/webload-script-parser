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

        public ColorDispenser()
        {
            _r = 200;
            _g = 255;
            _b = 0;
            _step = 25;
        }
        public ColorDispenser(byte step)
        {
            _r = 200;
            _g = 255;
            _b = 0;
            byte maxstep = 45;
            if (step > maxstep) step = maxstep;
            if (step < 5) step = 5;
            _step = step;
        }
        public Color GetNextColor()
        {
            byte currentRed = _r;
            byte currentGreen = _g;
            byte currentBlue = _b;

            if(_r > Byte.MinValue)
            {
                _r = (_r - _step > Byte.MinValue) ? _r -= _step : Byte.MinValue;
            }
            else if (_b < Byte.MaxValue )
            {
                _b = (_b + _step < Byte.MaxValue) ? _b += _step : Byte.MaxValue;
            }
            else
            {
                _g = (_g - _step > Byte.MinValue) ? _g -= _step : Byte.MinValue;
            }
            /*
                       
            if (_g == Byte.MinValue)
            {
                _r = (_r + _step > Byte.MaxValue) ? _r += _step : Byte.MaxValue;
            }

            if (_b == Byte.MaxValue)
            {
                _g = (_g - _step > Byte.MinValue) ? _g -= _step : Byte.MinValue;
            }
            else
            {
                _b = (_b + _step < Byte.MaxValue) ? _b += _step : Byte.MaxValue;
            }*/


            return Color.FromRgb(currentRed, currentGreen, currentBlue);
        }
        public void Reset()
        {
            _g = 255; _r = 0; _b = 0;
        }



    }
}
