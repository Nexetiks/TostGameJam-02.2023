using Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    class FibbonaciSequence : IWave
    {
        private int a = 0;
        private int b = 1;

        public int Next()
        {
            int c = a + b;
            a = b;
            b = c;
            return c;
        }
    }
}
