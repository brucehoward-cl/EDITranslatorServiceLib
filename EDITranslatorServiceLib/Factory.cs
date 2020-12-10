using EdiEngine.Common.Definitions;
using EdiEngine.Standards.X12_004010.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDITranslatorServiceLib
{
    public static class Factory
    {
        public static MapLoop GetMap(string mapname)
        {
            switch (mapname)
            {
                case "M_810":
                    return new M_810();
                case "M_820":
                    return new M_820();
                case "M_824":
                    return new M_824();
                case "M_850":
                    return new M_850();
                case "M_855":
                    return new M_855();
                case "M_856":
                    return new M_856();
                case "M_864":
                    return new M_864();
                case "M_940":
                    return new M_940();
                case "M_943":
                    return new M_943();
                case "M_944":
                    return new M_944();
                case "M_945":
                    return new M_945();
                case "M_947":
                    return new M_947();
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
