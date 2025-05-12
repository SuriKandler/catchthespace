using CatchTheSpaceSystem;
namespace CatchTheSpaceMaui;

public partial class MainPage : ContentPage
{
    Game game = new();
    List<Button> lstlines;
    List<Label> lstspaces = new();
    public MainPage()
	{
        InitializeComponent();
        this.BindingContext = game;
        lstlines = new()
            {lblLine1, lblLine2, lblLine3, lblLine4, lblLine5, lblLine6,
             lblLine7, lblLine8, lblLine9, lblLine10, lblLine11, lblLine12,
             lblLine13, lblLine14, lblLine15, lblLine16, lblLine17, lblLine18,
             lblLine19, lblLine20, lblLine21, lblLine22, lblLine23, lblLine24};
        lstspaces = new()
           { lblSpace1, lblSpace2, lblSpace3, lblSpace4, lblSpace5, lblSpace6, lblSpace7, lblSpace8, lblSpace9 };

    }

    private void StartBtn_Clicked(object sender, EventArgs e)
    {
        game.StartGame();
    }

    private void lblLine_Clicked(object sender, EventArgs e)
    {
        game.DoTurn(lstlines.IndexOf((Button)sender));
    }
}