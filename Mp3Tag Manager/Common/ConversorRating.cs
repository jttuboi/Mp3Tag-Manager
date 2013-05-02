using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Mp3Tag_Manager.Common {
    class ConversorRating : IValueConverter {

        //http://en.wikipedia.org/wiki/ID3
        //224-255 = 5 stars when READ with windows explorer, writes 255
        //160-223 = 4 stars when READ with windows explorer, writes 196
        //096-159 = 3 stars when READ with windows explorer, writes 128
        //032-095 = 2 stars when READ with windows explorer, writes 64
        //001-031 = 1 stars when READ with windows explorer, writes 1

        public object Convert(object value, System.Type type, object parameter, string language) {
            int _value;

            switch (Int32.Parse(value.ToString())) {
                case 0: { _value = 0; }
                    break;
                case 1: { _value = 1; }
                    break;
                case 2: { _value = 64; }
                    break;
                case 3: { _value = 128; }
                    break;
                case 4: { _value = 196; }
                    break;
                case 5: { _value = 255; }
                    break;
                default: { _value = 0; }
                    break;
            }

            return _value;
        }

        public object ConvertBack(object value, System.Type type, object parameter, string language) {
            int _value;
            int _grade = 0;

            if (Int32.TryParse(value.ToString(), out _value)) {
                if (_value == 0) {
                    _grade = 0;
                } else if (1 <= _value && _value <= 31) {
                    _grade = 1;
                } else if (32 <= _value && _value <= 95) {
                    _grade = 2;
                } else if (96 <= _value && _value <= 159) {
                    _grade = 3;
                } else if (160 <= _value && _value <= 223) {
                    _grade = 4;
                } else if (224 <= _value && _value <= 255) {
                    _grade = 5;
                } else {
                    _grade = 0;
                }
            }

            return _grade;
        }
    }
}
