using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrexRunner
{
    [Serializable]
    internal class SaveState
    {
        public int HighScore { get; set; }
        public DateTime HighScoreDate { get; set; }

    }
}
