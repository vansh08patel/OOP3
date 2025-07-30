using System;
using System.Collections.Generic;
using System.Linq;

namespace HeartsGame
{
    public class GameManager
    {

        public List<Player> Players { get; set; } = new List<Player>();
        private List<Card> deck = new List<Card>();
        private Random rng = new Random();

        public GameManager()
        {
            InitializeDeck();
            Shuffle();
          
        }

        private void InitializeDeck()
        {
            string[] suits = { "H", "D", "C", "S" };
            string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

            deck.Clear();
            foreach (var suit in suits)
            {
                foreach (var value in values)
                {
                    deck.Add(new Card(suit, value));
                }
            }
        }
        // shuffle
        private void Shuffle()
        {
            deck = deck.OrderBy(x => rng.Next()).ToList();
        }
        // deal same card with players
        public void DealCards()
        {
            if (Players.Count != 4)
                throw new InvalidOperationException("Exactly 4 players must be initialized before dealing cards.");

            for (int i = 0; i < Players.Count; i++)
            {
                Players[i].Hand = deck.Skip(i * 13).Take(13).ToList();
            }
        }
    }
}
