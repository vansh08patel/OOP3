using System.Collections.Generic;

namespace HeartsGame
{
    
    public class Player
    {
        // name , score
        public string Name { get; set; }
        public List<Card> Hand { get; set; } = new List<Card>();
        public int Score { get; set; }

        public Player(string name)
        {
            Name = name;
            Score = 0;
        }

        public Card PlayCard(int index)
        {
            Card card = Hand[index];
            Hand.RemoveAt(index);
            return card;
        }

        public void AddPoints(int points)
        {
            Score += points;
        }
    }
}



