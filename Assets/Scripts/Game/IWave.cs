using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameplay
{
    public interface IWave
    {
        /// <summary>
        /// Returns number of units to spawn.
        /// </summary>
        /// <returns></returns>
        public int Next();
    }
}
