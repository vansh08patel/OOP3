using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HeartsGame;



namespace HeartsGame
{
    public partial class Form1 : Form
    {
        // access with class
        GameManager gameManager;
        // access images
        Image cardBackImage;
        bool isDealt = false; 
        int currentPlayerIndex = 0; 
        bool isMyTurn => currentPlayerIndex == 0;
       
     
        bool isFirstTrick = true;
        string leadSuit = null;

        List<Card> currentTrick = new List<Card>();
        Dictionary<int, Card> playedCards = new Dictionary<int, Card>();
        int trickCount = 0;





        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (!isDealt)
            {
                isDealt = true; 
            }
            else
            {
                MessageBox.Show("Cards already dealt. Start playing!");
            }
        }

        private void pictureBox31_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox33_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox34_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox32_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox36_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox37_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox38_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox39_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox40_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox41_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox42_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox43_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox35_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox44_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox45_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox46_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox47_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox48_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        // DEAL CARDS TO ALL 4 PLAYERS
        void StartGame()
        {
            gameManager = new GameManager();

            gameManager.Players.Add(new Player("You"));
            gameManager.Players.Add(new Player("Player 2"));
            gameManager.Players.Add(new Player("Player 3"));
            gameManager.Players.Add(new Player("Player 4"));


            gameManager.DealCards();

            cardBackImage = Properties.Resources.card_back;

            // Display your hand
            var myHand = gameManager.Players[0].Hand;
            for (int i = 0; i < 13; i++)
            {
                if (tableLayoutPanel5.Controls[i] is PictureBox pb)
                {
                    string cardId = myHand[i].Id;
                    pb.Image = (Image)Properties.Resources.ResourceManager.GetObject(cardId);
                    pb.Tag = cardId;

                    // Set visual properties to remove space
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.Margin = new Padding(0);
                    pb.Padding = new Padding(0);
                    pb.Dock = DockStyle.Fill;

                    // Enable and attach event
                    pb.Enabled = true;
                    pb.Click -= PlayerCard_Click;
                    pb.Click += PlayerCard_Click;
                }
            }


            // Set card for other players
            SetCardBacks(tableLayoutPanel2); 
            SetCardBacks(tableLayoutPanel3); 
            SetCardBacks(tableLayoutPanel4); 

            // Clear trick area
            mytrick.Image = null;
            Player2trick.Image = null;
            Player3trick.Image = null;
            Player4trick.Image = null;

            for (int i = 0; i < gameManager.Players.Count; i++)
            {
                if (gameManager.Players[i].Hand.Any(c => c.Suit == "C" && c.Value == "2"))
                {
                    currentPlayerIndex = i;
                    break;
                }
            }

            // Inform the player
            if (currentPlayerIndex == 0)
                MessageBox.Show("You start! Play the 2 of Clubs.");
            else
                _ = AIPlayTurnAsync();
        }


        void SetCardBacks(TableLayoutPanel panel)
        {
            for (int i = 0; i < 13; i++)
            {
                if (panel.Controls[i] is PictureBox pb)
                {
                    pb.Image = cardBackImage;
                    pb.Enabled = false;
                }
            }
        }


        // click
        private async void PlayerCard_Click(object sender, EventArgs e)
        {
            if (!isMyTurn)
            {
                MessageBox.Show("It's not your turn yet!");
                return;
            }

            PictureBox clickedCard = sender as PictureBox;
            string cardId = clickedCard.Tag.ToString();
            var selectedCard = gameManager.Players[0].Hand.FirstOrDefault(c => c.Id == cardId);

            if (isFirstTrick)
            {
                if (selectedCard.Suit != "C" || selectedCard.Value != "2")
                {
                    MessageBox.Show("First move must be 2 of Clubs.");
                    return;
                }

                leadSuit = "C";
                isFirstTrick = false;
            }
            else
            {
                if (string.IsNullOrEmpty(leadSuit))
                {
                    leadSuit = selectedCard.Suit;
                }
                else
                {
                    bool hasLeadSuit = gameManager.Players[0].Hand.Any(c => c.Suit == leadSuit);
                    if (hasLeadSuit && selectedCard.Suit != leadSuit)
                    {
                        MessageBox.Show($"You must follow suit: {leadSuit}");
                        return;
                    }
                }

            }

            // Play
            mytrick.Image = (Image)Properties.Resources.ResourceManager.GetObject(cardId);
            clickedCard.Image = null;
            clickedCard.Enabled = false;

            if (playedCards.ContainsKey(0))
            {
                MessageBox.Show("You already played this trick.");
                return;
            }

            gameManager.Players[0].Hand.Remove(selectedCard);
            playedCards[0] = selectedCard;
            currentTrick.Add(selectedCard);


            currentPlayerIndex = 1;
            await AIPlayTurnAsync();
        }

        private async Task AIPlayTurnAsync()
        {
            while (currentTrick.Count < 4 && currentPlayerIndex != 0)
            {
                await Task.Delay(800);

                var ai = gameManager.Players[currentPlayerIndex];
                var trickBox = GetTrickBoxForPlayer(currentPlayerIndex);

                Card selectedCard;

                if (isFirstTrick)
                {
                    selectedCard = ai.Hand.FirstOrDefault(c => c.Id == "2C");
                    if (selectedCard == null)
                        selectedCard = GetRandomCard(ai.Hand);

                    leadSuit = selectedCard.Suit;
                    isFirstTrick = false;
                }
                else
                {
                    bool hasLeadSuit = ai.Hand.Any(c => c.Suit == leadSuit);
                    if (hasLeadSuit)
                    {
                        var suitCards = ai.Hand.Where(c => c.Suit == leadSuit).ToList();
                        selectedCard = GetRandomCard(suitCards);
                    }
                    else
                    {
                        selectedCard = GetRandomCard(ai.Hand);
                    }
                }

                trickBox.Image = (Image)Properties.Resources.ResourceManager.GetObject(selectedCard.Id);
                ai.Hand.Remove(selectedCard);
                // Remove one card image from the AI's panel
                TableLayoutPanel aiPanel = GetPanelForPlayer(currentPlayerIndex);
                HideTopCardFromPanel(aiPanel);

                playedCards[currentPlayerIndex] = selectedCard;
                currentTrick.Add(selectedCard);

                currentPlayerIndex = (currentPlayerIndex + 1) % 4;
            }

            if (currentTrick.Count == 4)
            {
                await Task.Delay(500);
                EvaluateTrick();
            }
            else if (currentPlayerIndex == 0)
            {
                MessageBox.Show("Your turn to play!");
            }
        }

        void HideTopCardFromPanel(TableLayoutPanel panel)
        {
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                if (panel.Controls[i] is PictureBox pb && pb.Image != null)
                {
                    pb.Image = null;
                    break;
                }
            }
        }

        TableLayoutPanel GetPanelForPlayer(int index)
        {
            switch (index)
            {
                case 1: return tableLayoutPanel2;
                case 2: return tableLayoutPanel3;
                case 3: return tableLayoutPanel4;
                default: return null;
            }
        }



        void EvaluateTrick()
        {

            mytrick.Image = Player2trick.Image = Player3trick.Image = Player4trick.Image = null;

            // Determine winner based on highest card of lead suit
            var leadCards = playedCards.Where(p => p.Value.Suit == leadSuit).ToList();
            var winner = leadCards.OrderByDescending(p => GetCardRank(p.Value.Value)).First().Key;

            // Calculate score
            int trickScore = 0;
            foreach (var card in playedCards.Values)
            {
                if (card.Suit == "H") trickScore += 1;
                if (card.Suit == "S" && card.Value == "Q") trickScore += 13;
            }

            gameManager.Players[winner].AddPoints(trickScore);
            UpdateScoreLabels();

            MessageBox.Show($"{gameManager.Players[winner].Name} wins the trick and gets {trickScore} points!");

            // Reset for next trick
            currentTrick.Clear();
            playedCards.Clear();
            trickCount++;
            leadSuit = null;
          


            currentPlayerIndex = winner;
            // Check if game is over
            bool allHandsEmpty = gameManager.Players.All(p => p.Hand.Count == 0);
            if (allHandsEmpty)
            {
                CalculateFinalScores();
                return;
            }
            // If player starts next
            if (currentPlayerIndex == 0)
                MessageBox.Show("Your turn to start the next trick!");
            else
                _ = AIPlayTurnAsync();
        }


        int GetCardRank(string value)
        {
            switch (value)
            {
                case "2": return 2;
                case "3": return 3;
                case "4": return 4;
                case "5": return 5;
                case "6": return 6;
                case "7": return 7;
                case "8": return 8;
                case "9": return 9;
                case "10": return 10;
                case "J": return 11;
                case "Q": return 12;
                case "K": return 13;
                case "A": return 14;
                default: return 0;
            }
        }



        Card GetRandomCard(List<Card> hand)
        {
            int index = new Random().Next(hand.Count);
            return hand[index];
        }

        void UpdateScoreLabels()
        {
            My_Score.Text = gameManager.Players[0].Score.ToString();
            Player2_Score.Text = gameManager.Players[1].Score.ToString();
            Player3_Score.Text = gameManager.Players[2].Score.ToString();
            Player4_Score.Text = gameManager.Players[3].Score.ToString();
        }



        PictureBox GetTrickBoxForPlayer(int playerIndex)
        {
            switch (playerIndex)
            {
                case 1: return Player2trick;
                case 2: return Player3trick;
                case 3: return Player4trick;
                default: return mytrick;
            }
        }

        private void CalculateFinalScores()
        {
            StringBuilder result = new StringBuilder();
            int lowestScore = int.MaxValue;
            string winner = "";

            foreach (var player in gameManager.Players)
            {
                result.AppendLine($"{player.Name}: {player.Score} points");

                if (player.Score < lowestScore)
                {
                    lowestScore = player.Score;
                    winner = player.Name;
                }
            }

            result.AppendLine($"\n🏆 Winner: {winner}");

            MessageBox.Show(result.ToString(), "Game Over");
            Close();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            if (!isDealt)
            {
                var result = MessageBox.Show("Ready to deal the cards and start a new game?", "Start Game", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    StartGame();
                    isDealt = true;
                }
            }
            else
            {
                MessageBox.Show("Game is already in progress. Finish it or restart the app.");
            }
        }
    }
}
