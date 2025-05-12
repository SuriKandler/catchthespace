using CatchTheSpaceSystem;
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
        //private void UpdateScore(Color winnerColor)
        //{
        //    if (winnerColor == Color.Red)
        //    {
        //        redPlayerScore++;
        //        lblPlayer1.Text = redPlayerScore.ToString();
        //    }
        //    else
        //    {
        //        greenPlayerScore++;
        //        lblPlayer2.Text = greenPlayerScore.ToString();
        //    }

        //    UpdateProgressLabel(winnerColor == Color.Red ? redPlayerScore : greenPlayerScore, winnerColor);
        //    currentturn = (currentturn == TurnEnum.Red) ? TurnEnum.Green : TurnEnum.Red;
        //}

        //private void UpdateProgressLabel(int score, Color winnerColor)
        //{
        //    string progressLabelName = winnerColor == Color.Red ? $"lblProgress{score}" : $"lblProgress{10 - score}";

        //    Label? progressLabel = this.Controls.Find(progressLabelName, true).FirstOrDefault() as Label;

        //    if (progressLabel != null)
        //    {
        //        progressLabel.BackColor = winnerColor;
        //    }
        //}

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
