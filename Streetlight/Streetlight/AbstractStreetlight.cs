using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streetlight.Interfaces;

namespace Streetlight
{
    public abstract class AbstractStreetlight : INumberToSegmentConvertable
    {
        public int InitialNumber { get; set; } = 9;

        public List<byte> DigitsSegmentsScheme = new List<byte>();

        public AbstractStreetlight(int initialNumber)
        {
            InitialNumber = initialNumber;
        }

        public virtual List<byte> ResolveNumberIntoDigitScheme(int number)
        {
            List<int> listOfInts = new List<int>();
            while (number > 0)
            {
                listOfInts.Add(number % 10);
                number = number / 10;
            }

            listOfInts.Reverse();

            DigitsSegmentsScheme = new List<byte>();

            foreach(var digit in listOfInts)
            {
                var digitScheme = new byte();
                switch (digit)
                {
                    default:
                        digitScheme = 0b_1110111_0;
                        break;
                    case 1:
                        digitScheme = 0b_0010010_0;
                        break;
                    case 2:
                        digitScheme = 0b_1011101_0;
                        break;
                    case 3:
                        digitScheme = 0b_1011011_0;
                        break;
                    case 4:
                        digitScheme = 0b_0111010_0;
                        break;
                    case 5:
                        digitScheme = 0b_1101011_0;
                        break;
                    case 6:
                        digitScheme = 0b_1101111_0;
                        break;
                    case 7:
                        digitScheme = 0b_1010010_0;
                        break;
                    case 8:
                        digitScheme = 0b_1111111_0;
                        break;
                    case 9:
                        digitScheme = 0b_1111011_0;
                        break;
                }

                DigitsSegmentsScheme.Add(digitScheme);
            }

            return DigitsSegmentsScheme;
        }
    }
}
