using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetlight
{
    public class StreetlightFactory
    {
        public static AbstractStreetlight CreateStreetlight()
        {
            var StreetlightSubtypes = Enum.GetNames(typeof(StreetlightSubtypesEnum)).Length;
            var RandomSubtype = new Random().Next(StreetlightSubtypes);
            var initialNumber = new Random().Next(7) + 2;

            switch ((StreetlightSubtypesEnum)RandomSubtype)
            {
                case StreetlightSubtypesEnum.DualDigit:
                    initialNumber = new Random().Next(5) + 12;
                    return new StreetlightDualDigits(initialNumber);

                case StreetlightSubtypesEnum.Malfunction:
                    return new StreetlightDefault(initialNumber);

                case StreetlightSubtypesEnum.IdleWhileRed:
                    return new StreetlightIdleWhileRed(initialNumber);

                default:
                    return new StreetlightDefault(initialNumber);
            }
        }
    }
}
