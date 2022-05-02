using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetlight
{
    public class StreetlightMalfunction : AbstractStreetlight
    {
        List<byte> ListOfBadSegmentsIndexes = new List<byte>();

        public StreetlightMalfunction(int initialNumber) : base(initialNumber)
        {
            var numberOfBadSegments = new Random().Next(2)+1;
            var previousBadSegmentIndex = 0;
            
            byte badSegmentIndex = 0;
            for (int i = 1; i <= numberOfBadSegments; i++)
            {
                if (i == 0)
                {
                    badSegmentIndex = (byte)new Random().Next(7);
                    ListOfBadSegmentsIndexes.Add(badSegmentIndex);
                    previousBadSegmentIndex = badSegmentIndex;
                }
                else
                {
                    do
                    {
                        badSegmentIndex = (byte)new Random().Next(7);
                    }
                    while (previousBadSegmentIndex == numberOfBadSegments);

                    ListOfBadSegmentsIndexes.Add(badSegmentIndex);
                }
            }
        }

        public override List<byte> ResolveNumberIntoDigitScheme(int number)
        {
            base.ResolveNumberIntoDigitScheme(number);

            List<byte> ListOfBadSchemes = new List<byte>();
            foreach (var goodScheme in DigitsSegmentsScheme)
            {
                var bits = Convert.ToString(goodScheme, 2)
                    .PadLeft(8, '0')
                    .ToCharArray()
                    .Select((c, i) => (ListOfBadSegmentsIndexes.Contains((byte)i) ? "0" : (c == 48 ? 0 : 1).ToString()));

                var bitsString = string.Join("", bits);

                var badScheme = new byte();
                for (int i = bitsString.Length - 1; i >= 0; i -= 8)
                {
                    string byteString = "";
                    for (int j = 0; (i - j) >= 0 && j < 8; j++)
                    {
                        byteString = bitsString[i - j] + byteString;
                    }
                    if (byteString != "")
                    {
                        badScheme = Convert.ToByte(byteString, 2);
                    }
                }

                ListOfBadSchemes.Add(badScheme);
            }
            DigitsSegmentsScheme = ListOfBadSchemes;
            return ListOfBadSchemes;
        }
    }
}
