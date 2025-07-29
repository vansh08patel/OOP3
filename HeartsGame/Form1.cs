using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeartsGame
{
    public partial class Form1 : Form
    {

        List<string> deck = new List<string>();
        List<string> myHand, player2Hand, player3Hand, player4Hand;
        Random rng = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            DealCards();
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

        //CREATE & SHUFFLE DECK
        void InitializeDeck()
        {
            string[] suits = { "H", "D", "C", "S" };
            string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

            deck.Clear();
            foreach (string suit in suits)
            {
                foreach (string value in values)
                {
                    deck.Add(value + suit); 
                }
            }

            deck = deck.OrderBy(x => rng.Next()).ToList(); 
        }

        // DEAL CARDS TO ALL 4 PLAYERS
        void DealCards()
        {
            InitializeDeck();

            myHand = deck.Take(13).ToList();
            player2Hand = deck.Skip(13).Take(13).ToList();
            player3Hand = deck.Skip(26).Take(13).ToList();
            player4Hand = deck.Skip(39).Take(13).ToList();

            // 👉 YOUR CARDS – panel: panel_P1 > tableLayoutPanel5
            for (int i = 0; i < 13; i++)
            {
                PictureBox pb = tableLayoutPanel5.Controls[i] as PictureBox;
                string card = myHand[i];
                pb.Image = (Image)Properties.Resources.ResourceManager.GetObject(card);
                pb.Tag = card;
                pb.Visible = true;
                pb.Click -= PlayerCard_Click;
                pb.Click += PlayerCard_Click;
            }

            
            Image back = Properties.Resources.card_back;

            for (int i = 0; i < 13; i++)
            {
                (tableLayoutPanel2.Controls[i] as PictureBox).Image = back; // Player 2
                (tableLayoutPanel3.Controls[i] as PictureBox).Image = back; // Player 3
                (tableLayoutPanel4.Controls[i] as PictureBox).Image = back; // Player 4
            }

            // Clear center trick area
            mytrick.Image = null;
            Player2trick.Image = null;
            Player3trick.Image = null;
            Player4trick.Image = null;
        }

        //  YOUR CARD CLICK LOGIC
        void PlayerCard_Click(object sender, EventArgs e)
        {
            PictureBox clickedCard = sender as PictureBox;
            string card = clickedCard.Tag.ToString();

            // Play in center 
            mytrick.Image = (Image)Properties.Resources.ResourceManager.GetObject(card);
            clickedCard.Visible = false;
            myHand.Remove(card);


        }
        }
}
