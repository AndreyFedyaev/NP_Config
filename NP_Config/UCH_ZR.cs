using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NP_Config
{
    public class UCH_ZR
    {
        public int ZR_Address { get; set; }
        public int ZR_NP { get; set; }
        public int ZR_Channel { get; set; }
        public UCH_ZR() { }
        public UCH_ZR(int ZR_Address, int ZR_NP, int ZR_Channel)
        {
            this.ZR_Address = ZR_Address;
            this.ZR_NP = ZR_NP;
            this.ZR_Channel = ZR_Channel;
        }
    }
}
