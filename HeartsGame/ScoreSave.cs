using System;
using System.Collections.Generic;

namespace HeartsGame
{
    public class ScoreSave
    {
        public int Trick { get; set; } = 0;
        public List<int> Scores { get; set; } = new List<int>();  
        public DateTime SavedAt { get; set; } = DateTime.Now;
    }
}
