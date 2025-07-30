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


        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            StartGame();
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
                PictureBox pb = tableLayoutPanel5.Controls[i] as PictureBox;
                string cardId = myHand[i].Id;
                pb.Image = (Image)Properties.Resources.ResourceManager.GetObject(cardId);
                pb.Tag = cardId;
                pb.Visible = true;
                pb.Click -= PlayerCard_Click;
                pb.Click += PlayerCard_Click;
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
        }


        void SetCardBacks(TableLayoutPanel panel)
        {
            for (int i = 0; i < 13; i++)
            {
                PictureBox pb = panel.Controls[i] as PictureBox;
                pb.Image = cardBackImage;
                pb.Visible = true;
            }
        }

        // click
        void PlayerCard_Click(object sender, EventArgs e)
        {
            PictureBox clickedCard = sender as PictureBox;
            string cardId = clickedCard.Tag.ToString();

            mytrick.Image = (Image)Properties.Resources.ResourceManager.GetObject(cardId);
            clickedCard.Visible = false;

            var myPlayer = gameManager.Players[0];
            myPlayer.Hand.RemoveAll(c => c.Id == cardId);
        }


    }
}
