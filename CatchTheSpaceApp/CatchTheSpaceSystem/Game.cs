using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using static System.Formats.Asn1.AsnWriter;

namespace CatchTheSpaceSystem
{
    public class Game : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public event Action<int, Player>? ProgressUpdated;
        public event EventHandler? ScoreChanged;

        private static int scoreredwins;
        private static int scoreorangewins;

        List<List<Line>> lstwinningsets = new();
        bool _gameactiveval = false;
        private Player _currentTurn = Player.Red;

        public List<Line> lines { get; private set; } = new();
        public List<Space> spaces { get; private set; } = new();
        public List<Progress> progress { get; private set; } = new();
        public Dictionary<Player, int> Scores { get; private set; } = new()
        {
              { Player.Red, 0 },
              { Player.Orange, 0 }
        };
        private int player1Score;
        public int Player1Score
        {
            get => player1Score;
            set
            {
                if (player1Score != value)
                {
                    player1Score = value;
                    InvokePropertyChanged();
                }
            }
        }

        private int player2Score;
        public int Player2Score
        {
            get => player2Score;
            set
            {
                if (player2Score != value)
                {
                    player2Score = value;
                    InvokePropertyChanged();
                }
            }
        }
        public Player CurrentTurn
        {
            get => _currentTurn;
            set
            {
                if (_currentTurn != value)
                {
                    _currentTurn = value;
                    InvokePropertyChanged();
                    InvokePropertyChanged(nameof(DisplayGameStatus));
                }
            }
        }
        public void SetCurrentTurn(Player turn)
        {
            this.CurrentTurn = turn;
        }
        public string DisplayGameStatus
        {
            get
            {
                return GameActive
                    ? $"Current Turn: {this.CurrentTurn} player, it's your turn. Click between the sweets to get your line."
                    : "Click Start to begin the game.";
            }
        }
        public bool GameActive
        {
            get => _gameactiveval;
            set
            {
                if (_gameactiveval != value)
                {
                    _gameactiveval = value;
                    InvokePropertyChanged(nameof(GameActive));
                    InvokePropertyChanged(nameof(DisplayGameStatus));
                }
            }
        }
        public System.Drawing.Color LineCompletedColor { get; set; } = System.Drawing.Color.Gray;
        public System.Drawing.Color LinePlayingColor { get; set; }
        public System.Drawing.Color LineNotStartedColor { get; set; } = System.Drawing.Color.Transparent;

        public System.Drawing.Color SpaceTakenColor { get; set; }
        public System.Drawing.Color SpaceNotStartedColor { get; set; } = System.Drawing.Color.Transparent;

        public System.Drawing.Color ProgressBarNotStartedColor { get; set; } = System.Drawing.Color.Transparent;
        public System.Drawing.Color ProgressBarTakenColor { get; set; }

        public static string Score { get => $"Red wins = {scoreredwins}: Orange wins = {scoreorangewins}"; }


        public Game()
        {

            for (int i = 0; i < 24; i++)
            {
                this.lines.Add(new Line());
            }

            for (int i = 0; i < 9; i++)
            {
                this.spaces.Add(new Space());
            }
            for (int i = 0; i < 9; i++)
            {
                this.progress.Add(new Progress());
            }

            lstwinningsets = new() {
            new() { this.lines[0], this.lines[3], this.lines[4], this.lines[7] },
            new() { this.lines[1], this.lines[4],this.lines[5], this.lines[8] },
            new() { this.lines[2], this.lines[5],this.lines[6], this.lines[9] },
            new() { this.lines[7], this.lines[10], this.lines[11], this.lines[14] },
            new() { this.lines[8], this.lines[11], this.lines[12], this.lines[15] },
            new() { this.lines[9],  this.lines[12], this.lines[13], this.lines[16] },
            new() { this.lines[14], this.lines[17], this.lines[18], this.lines[21] },
            new() { this.lines[15], this.lines[18], this.lines[19], this.lines[22] },
            new() { this.lines[16], this.lines[19], this.lines[20], this.lines[23] }};
        }

        public void StartGame()
        {
            this.spaces.ForEach(s => s.BackColor = this.SpaceNotStartedColor);
            this.lines.ForEach(l => l.BackColor = this.LineNotStartedColor);
            this.lines.ForEach(l => l.Enabled = true);
            this.GameActive = true;
            this.CurrentTurn = Player.Red;
            Scores[Player.Red] = 0;
            Scores[Player.Orange] = 0;
            Player1Score = 0;
            Player2Score = 0;

            for (int i = 0; i < progress.Count; i++)
            {
                ProgressUpdated?.Invoke(i, Player.None);
            }

        }
        public void DoTurn(int linenum)
        {
            Line Line = this.lines[linenum];
            if (this.GameActive)
            {
                if (Line.BackColor == this.LineNotStartedColor)
                {
                    if (this.CurrentTurn == Player.Red)
                    {

                        this.LinePlayingColor = Player.Red.GetColor();
                        this.CurrentTurn = Player.Orange;
                    }
                    else
                    {
                        this.LinePlayingColor = Player.Orange.GetColor();
                        this.CurrentTurn = Player.Red;
                    }
                    Line.BackColor = this.LinePlayingColor;

                    for (int i = 0; i < this.lstwinningsets.Count; i++)
                    {
                        DetectWinner(lstwinningsets[i], i);
                    }
                    CheckIfGameIsOver();

                }
            }

        }

        private void DetectWinner(List<Line> lst, int index)
        {
            if (lst.All(l => l.BackColor != this.LineNotStartedColor))
            {
                this.SpaceTakenColor = this.CurrentTurn == Player.Red ? Player.Orange.GetColor() : Player.Red.GetColor();

                if (index >= 0 && index < this.spaces.Count)
                {
                    Space space = this.spaces[index];



                    if (space != null && space.BackColor == this.SpaceNotStartedColor)
                    {
                        space.BackColor = this.SpaceTakenColor;
                        UpdateScore(this.SpaceTakenColor);
                    }

                    lst.ForEach(l =>
                    {
                        l.BackColor = this.LineCompletedColor;
                        l.Enabled = false;
                    });
                }
            }
        }


        private void CheckIfGameIsOver()
        {
            int totalBoxes = 9;
            int filledBoxes = 0;

            for (int i = 0; i < totalBoxes; i++)
            {
                Space space = this.spaces[i];
                if (space != null && space.BackColor != this.SpaceNotStartedColor)
                {
                    filledBoxes++;
                }
            }
            if (filledBoxes == totalBoxes)
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            if (player1Score > Player2Score)
            {
                scoreredwins++;
            }
            if (player2Score > Player1Score)
            {
                scoreorangewins++;
            }
            ScoreChanged?.Invoke(this, new EventArgs());
            this.GameActive = false;

            //if (redPlayerScore > greenPlayerScore)
            //{
            //    lblMessage.Text = "Red Player Wins!"; // Display the winner
            //}
            //else if (greenPlayerScore > redPlayerScore)
            //{
            //    lblMessage.Text = "Green Player Wins!"; // Display the winner
            //}
        }


        private void UpdateScore(Color winner)
        {
            var winningPlayer = winner == Player.Red.GetColor() ? Player.Red : Player.Orange;
            Scores[winningPlayer]++;

            if (winningPlayer == Player.Red)
            {
                Player1Score = Scores[Player.Red];
                UpdateProgressLabel(Player1Score, winner);
            }
            else
            {
                Player2Score = Scores[Player.Orange];
                UpdateProgressLabel(Player2Score, winner);
            }

            this.CurrentTurn = (this.CurrentTurn == Player.Red) ? Player.Orange : Player.Red;
        }


        private void UpdateProgressLabel(int score, Color winnerColor)
        {
            int scorevalue = winnerColor == Player.Red.GetColor() ? score - 1 : 9 - score;
            var player = winnerColor == Player.Red.GetColor() ? Player.Red : Player.Orange;
            ProgressUpdated?.Invoke(scorevalue, player);
        }

        private void InvokePropertyChanged([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }


    }
}
