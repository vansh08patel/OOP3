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




        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            if (!isDealt)
            {
                StartGame();
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
                    pb.SizeMode = PictureBoxSizeMode.StretchImage;
                    pb.Dock = DockStyle.Fill;
                    pb.Margin = new Padding(0);
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

            // First Trick must start with 
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
                // Must follow lead suit if possible
                if (!string.IsNullOrEmpty(leadSuit))
                {
                    bool hasLeadSuit = gameManager.Players[0].Hand.Any(c => c.Suit == leadSuit);
                    if (hasLeadSuit && selectedCard.Suit != leadSuit)
                    {
                        MessageBox.Show($"You must follow suit: {leadSuit}");
                        return;
                    }
                }
                else
                {
                    leadSuit = selectedCard.Suit; 
                }
            }

            // Play the card
            mytrick.Image = (Image)Properties.Resources.ResourceManager.GetObject(cardId);
            clickedCard.Image = null;
            clickedCard.Enabled = false;

            gameManager.Players[0].Hand.Remove(selectedCard);

            currentPlayerIndex = 1;
            await AIPlayTurnAsync();
        }



        private async Task AIPlayTurnAsync()
        {
            while (currentPlayerIndex != 0)
            {
                await Task.Delay(1000); 

                var currentAI = gameManager.Players[currentPlayerIndex];
                PictureBox trickBox = GetTrickBoxForPlayer(currentPlayerIndex);

                Card selectedCard = null;

                if (isFirstTrick)
                {
                    selectedCard = currentAI.Hand.FirstOrDefault(c => c.Id == "2C");
                    if (selectedCard != null)
                    {
                        leadSuit = selectedCard.Suit; 
                    }
                    else
                    {
                        selectedCard = GetRandomCard(currentAI.Hand);
                        leadSuit = selectedCard.Suit;
                    }
                    isFirstTrick = false; 
                }

                else
                {
                    // Follow the lead suit if possible
                    var matchingSuitCards = currentAI.Hand.Where(c => c.Suit == leadSuit).ToList();
                    if (matchingSuitCards.Any())
                    {
                        selectedCard = GetRandomCard(matchingSuitCards);
                    }
                    else
                    {
                        selectedCard = GetRandomCard(currentAI.Hand); 
                    }
                }

                // Show card on trick panel
                trickBox.Image = (Image)Properties.Resources.ResourceManager.GetObject(selectedCard.Id);
                currentAI.Hand.Remove(selectedCard);

                await Task.Delay(3000); 

                currentPlayerIndex = (currentPlayerIndex + 1) % 4;
            }

            MessageBox.Show("Your turn!");
        }



        Card GetRandomCard(List<Card> hand)
        {
            int index = new Random().Next(hand.Count);
            return hand[index];
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

    }
}
