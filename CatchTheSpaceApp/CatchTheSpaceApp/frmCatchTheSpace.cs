using CatchTheSpaceSystem;
using Microsoft.Maui.Graphics;
using System.Numerics;
namespace CatchTheSpaceApp
{
    public partial class frmCatchTheSpace : Form
    {
        Game game = new();
        List<Label> lstlines;
        List<Label> lstspaces = new();
        public frmCatchTheSpace()
        {
            InitializeComponent();
            lstlines = new()
            {lblLine1, lblLine2, lblLine3, lblLine4, lblLine5, lblLine6,
             lblLine7, lblLine8, lblLine9, lblLine10, lblLine11, lblLine12,
             lblLine13, lblLine14, lblLine15, lblLine16, lblLine17, lblLine18,
             lblLine19, lblLine20, lblLine21, lblLine22, lblLine23, lblLine24};

            lstlines.ForEach(l => {
                l.Click += LineClick_Click;
                l.DataBindings.Add("BackColor", game.lines[lstlines.IndexOf(l)], "BackColor");
                l.DataBindings.Add("Enabled", game.lines[lstlines.IndexOf(l)], "Enabled");
            });
           lstspaces = new()
           { lblSpace1, lblSpace2, lblSpace3, lblSpace4, lblSpace5, lblSpace6, lblSpace7, lblSpace8, lblSpace9 };

            for (int i = 0; i < lstspaces.Count; i++)
            {
                lstspaces[i].DataBindings.Add("BackColor", game.spaces[i], "BackColor");
            }
            btnStart.Click += BtnStart_Click;

            lblMessage.DataBindings.Add("Text", game, "DisplayGameStatus");

            lblPlayer1.DataBindings.Add("Text", game, "Player1Score");
            lblPlayer2.DataBindings.Add("Text", game, "Player2Score");

            game.ProgressUpdated += Game_ProgressUpdated;
        }

        private void Game_ProgressUpdated(int scorevalue, Player player)
        {
            string labelName = $"lblProgress{scorevalue}";
            var foundControls = this.Controls.Find(labelName, true); 

            if (foundControls.Length > 0 && foundControls[0] is Label label)
            {
                if (player == Player.None)
                {
                    label.BackColor = game.ProgressBarNotStartedColor;
                }
                else
                {
                    label.BackColor = player == Player.Red ? Player.Red.GetColor() : Player.Orange.GetColor();
                }
                label.Visible = true;
                label.BringToFront();
            }


        }

        private void StartGame()
        {
            game.StartGame();

        }

        private void DoTurn(Label label)
        {
            int num = lstlines.IndexOf(label);
            game.DoTurn(num);
        }
    
        private void LineClick_Click(object? sender, EventArgs e)
        {
            if (sender is Label label && label.BackColor == game.LineNotStartedColor && label.Enabled)
            {
                DoTurn(label);
            }
        }
        private void BtnStart_Click(object? sender, EventArgs e)
        {
            StartGame();
        }
    }
}
