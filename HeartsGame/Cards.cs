namespace HeartsGame
{
    public class Card
    {
        // suit
        public string Suit { get; }
        // value of cards
        public string Value { get; }
        public string Id => $"{Value}{Suit}";

        // set variable
        public Card(string suit, string value)
        {
            Suit = suit;
            Value = value;
        }

        public override string ToString() => $"{Value} of {Suit}";
    }
}
