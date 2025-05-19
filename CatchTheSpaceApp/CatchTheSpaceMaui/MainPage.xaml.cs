using CatchTheSpaceSystem;

namespace CatchTheSpaceMaui;

public partial class MainPage : ContentPage
{
    Game activegame;
    List<Game> lstgame = new() { new Game(), new Game(), new Game() };
    List<Button> lstlines;
    List<Label> lstspaces = new();
    public MainPage()
    {
        InitializeComponent();
        lstgame.ForEach(g => g.ScoreChanged += G_ScoreChanged);
        Game1Rb.BindingContext = lstgame[0];
        Game2Rb.BindingContext = lstgame[1];
        Game3Rb.BindingContext = lstgame[2];
        activegame = lstgame[0];
        this.BindingContext = activegame;
        lstlines = new()
            {lblLine1, lblLine2, lblLine3, lblLine4, lblLine5, lblLine6,
             lblLine7, lblLine8, lblLine9, lblLine10, lblLine11, lblLine12,
             lblLine13, lblLine14, lblLine15, lblLine16, lblLine17, lblLine18,
             lblLine19, lblLine20, lblLine21, lblLine22, lblLine23, lblLine24};
        lstspaces = new()
           { lblSpace1, lblSpace2, lblSpace3, lblSpace4, lblSpace5, lblSpace6, lblSpace7, lblSpace8, lblSpace9 };

        activegame.ProgressUpdated += Game_ProgressUpdated;

    }

    private void G_ScoreChanged(object? sender, EventArgs e)
    {
        ScoreLbl.Text = Game.Score;
    }

    private void Game_ProgressUpdated(int scoreValue, Player player)
    {
        string labelName = $"ProgressBar{scoreValue}";
        var label = this.FindByName<Label>(labelName);

        if (label != null)
        {
            if (player == Player.None)
            {
                label.BackgroundColor = Colors.Transparent;
            }
            else
            {
                label.BackgroundColor = player == Player.Red ? Color.FromRgb(250, 125, 125) : Color.FromRgb(247, 198, 113);
            }

        }
    }

    private void StartBtn_Clicked(object sender, EventArgs e)
    {
        activegame.StartGame();
    }

    private void lblLine_Clicked(object sender, EventArgs e)
    {
        activegame.DoTurn(lstlines.IndexOf((Button)sender));
    }

    private void Game_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        RadioButton rb = (RadioButton)sender;
        if (rb.IsChecked && rb.BindingContext != null)
        {
            activegame = (Game)rb.BindingContext;
            this.BindingContext = activegame;
        }
    }
}