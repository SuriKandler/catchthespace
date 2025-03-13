namespace CatchTheSpaceApp
{
    public partial class frmCatchTheSpace : Form
    {
        int i = 0;
        Label turn = new Label();

        private int redPlayerScore = 0;
        private int greenPlayerScore = 0;

        enum TurnEnum { Red, Green }
        TurnEnum currentturn = TurnEnum.Red;
        bool gameactive = false;

        List<Label> lstspaces;
        List<Label> lstlines;
        List<List<Label>> lstwinningsets;
        List<Label> lstprogress;

        public frmCatchTheSpace()
        {
            InitializeComponent();
            lstlines = new()
            {lblLine1, lblLine2, lblLine3, lblLine4, lblLine5, lblLine6,
             lblLine7, lblLine8, lblLine9, lblLine10, lblLine11, lblLine12,
             lblLine13, lblLine14, lblLine15, lblLine16, lblLine17, lblLine18,
             lblLine19, lblLine20, lblLine21, lblLine22, lblLine23, lblLine24};

            lstlines.ForEach(l => l.Click += LineClick_Click);
            btnStart.Click += BtnStart_Click;

            lstwinningsets = new() {
            new() { lblLine1, lblLine4, lblLine5, lblLine8 },
            new() { lblLine2, lblLine5, lblLine6, lblLine9 },
            new() { lblLine3, lblLine6, lblLine7, lblLine10 },
            new() { lblLine8, lblLine11, lblLine12, lblLine15 },
            new() { lblLine9, lblLine12, lblLine13, lblLine16 },
            new() { lblLine10, lblLine13, lblLine14, lblLine17 },
            new() { lblLine15, lblLine18, lblLine19, lblLine22 },
            new() { lblLine16, lblLine19, lblLine20, lblLine23 },
            new() { lblLine17, lblLine20, lblLine21, lblLine24 }
        };
            lstspaces = new() 
            {lblSpace1, lblSpace2,lblSpace3, 
             lblSpace4, lblSpace5, lblSpace6, 
             lblSpace7, lblSpace8, lblSpace9};

            lstprogress = new()
            {lblProgress1, lblProgress2,lblProgress3,
             lblProgress4, lblProgress5, lblProgress6,
             lblProgress7, lblProgress8, lblProgress9};

            DisplayGameStatus();
        }

        private void LineClick_Click(object? sender, EventArgs e)
        {
            if (sender is Label label && label.BackColor == Color.White && label.Enabled)
            {
                DoTurn(label);
            }
        }
        private void DoTurn(Label label)
        {
            if (gameactive)
            {
                if (label.BackColor == Color.White)
                {
                    if (currentturn == TurnEnum.Red)
                    {
                        label.BackColor = Color.Red;
                        currentturn = TurnEnum.Green;
                    }
                    else
                    {
                        label.BackColor = Color.Green;
                        currentturn = TurnEnum.Red;
                    }

                    for (int i = 0; i < lstwinningsets.Count; i++)
                    {
                        DetectWinner(lstwinningsets[i], i);
                    }
                    DisplayGameStatus();
                    CheckIfGameIsOver();
                }
            }
        }

        private void DetectWinner(List<Label> winningSet, int index)
        {
            if (winningSet.All(label => label.BackColor != Color.White))
            {
                Color winnerColor = currentturn == TurnEnum.Red ? Color.Green : Color.Red;

                string labelName = $"lblSpace{index + 1}";
                Label? lblToChange = this.Controls.Find(labelName, true).FirstOrDefault() as Label;

                if (lblToChange != null && lblToChange.BackColor == Color.FromArgb(224, 224, 224))
                {
                    lblToChange.BackColor = winnerColor;
                    UpdateScore(winnerColor);
                }

                winningSet.ForEach(label =>
                {
                    label.BackColor = Color.Black;
                    label.Enabled = false;
                });
            }
        }


        private void UpdateScore(Color winnerColor)
        {
            if (winnerColor == Color.Red)
            {
                redPlayerScore++;
                lblPlayer1.Text = redPlayerScore.ToString();
            }
            else
            {
                greenPlayerScore++;
                lblPlayer2.Text = greenPlayerScore.ToString();
            }

            UpdateProgressLabel(winnerColor == Color.Red ? redPlayerScore : greenPlayerScore, winnerColor);
            currentturn = (currentturn == TurnEnum.Red) ? TurnEnum.Green : TurnEnum.Red;
        }

        private void UpdateProgressLabel(int score, Color winnerColor)
        {
            string progressLabelName = winnerColor == Color.Red ? $"lblProgress{score}" : $"lblProgress{10 - score}";

            Label? progressLabel = this.Controls.Find(progressLabelName, true).FirstOrDefault() as Label;

            if (progressLabel != null)
            {
                progressLabel.BackColor = winnerColor;
            }
        }

        private void StartGame()
        {
            lstprogress.ForEach(l => l.BackColor = Color.White);
            lstspaces.ForEach(l => l.BackColor = Color.FromArgb(224, 224, 224));
            lstlines.ForEach(l => l.BackColor = Color.White);
            lstlines.ForEach(l => l.Enabled = true);
            gameactive = true;
            currentturn = TurnEnum.Red;
            redPlayerScore = 0;
            greenPlayerScore = 0;
            lblPlayer1.Text = redPlayerScore.ToString();
            lblPlayer2.Text = greenPlayerScore.ToString();
            DisplayGameStatus();
        }

        private void DisplayGameStatus()
        {
            string msg = gameactive ? $"Current Turn: {currentturn} player" : "Click Start to begin Game";
            lblMessage.Text = msg;
        }

        private void CheckIfGameIsOver()
        {
            int totalBoxes = 9; // Number of boxes (9 boxes in your case)
            int filledBoxes = 0;

            // Loop through all the space labels and count how many are filled (not white)
            for (int i = 0; i < totalBoxes; i++)
            {
                string labelName = $"lblSpace{i + 1}"; // lblSpace1 to lblSpace9
                Label? lblSpace = this.Controls.Find(labelName, true).FirstOrDefault() as Label;

                if (lblSpace != null && lblSpace.BackColor != Color.FromArgb(224, 224, 224))
                {
                    filledBoxes++;
                }
            }

            // If all boxes are filled, end the game
            if (filledBoxes == totalBoxes)

            {
                EndGame(); // End the game when all boxes are filled
            }
        }
        private void EndGame()
        {
            gameactive = false; // Stop the game
            DisplayGameStatus(); // Update the status label to reflect the game has ended

            // Determine who won by comparing the scores
            if (redPlayerScore > greenPlayerScore)
            {
                lblMessage.Text = "Red Player Wins!"; // Display the winner
            }
            else if (greenPlayerScore > redPlayerScore)
            {
                lblMessage.Text = "Green Player Wins!"; // Display the winner
            }
           
        }
        private void BtnStart_Click(object? sender, EventArgs e)
        {
            StartGame();
        }
    }
}
