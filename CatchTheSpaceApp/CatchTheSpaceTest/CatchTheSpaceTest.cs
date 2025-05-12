using CatchTheSpaceSystem;
using static CatchTheSpaceSystem.Game;
using System.Drawing;
namespace CatchTheSpaceTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestStartGame()
        {
            Game game = new();
            game.StartGame();
            string msg = $"game status = {game.GameActive.ToString()} current turn = {game.CurrentTurn.ToString()} num lines = {game.lines.Count} num spaces = {game.spaces.Count}";
            Assert.IsTrue(game.CurrentTurn == Game.TurnEnum.Red && game.GameActive == true && game.lines.Count == 24 && game.spaces.Count == 9 , msg);
            TestContext.WriteLine(msg);
        }
        [Test]
        public void TestDoTurn()
        {
            Game game = new Game();
            game.StartGame();
            game.DoTurn(0);
            string msg = $"If the game is active, line backcolor = {game.lines[0].BackColor} current turn = {game.CurrentTurn.ToString()}";
            Assert.IsTrue(game.GameActive == true && game.LinePlayingColor == Color.Red && game.CurrentTurn == Game.TurnEnum.Orange, msg);
            TestContext.WriteLine(msg);
        }
        [Test]
        public void DetectWinner_SetsSpaceBackColor_AndDisablesLines_WhenAllLinesSelected()
        {
            var game = new Game();
            game.SpaceNotStartedColor = Color.Transparent;
            game.SpaceTakenColor = Color.Red; 
            game.LineCompletedColor = Color.White;

            int testSetIndex = 0; 
            var winningSet = new List<Line> {
                game.lines[0], game.lines[3], game.lines[4], game.lines[7]
            };

            foreach (var line in winningSet)
            {
                line.BackColor = Color.Blue;
            }

            var targetSpace = game.spaces[testSetIndex];
            targetSpace.BackColor = game.SpaceNotStartedColor;
            typeof(Game)
                .GetMethod("DetectWinner", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(game, new object[] { winningSet, testSetIndex });
            Assert.AreEqual(game.SpaceTakenColor, targetSpace.BackColor, "Space should be filled with taken color.");
            foreach (var line in winningSet)
            {
                Assert.AreEqual(game.LineCompletedColor, line.BackColor, "Line should be set to completed color.");
                Assert.IsFalse(line.Enabled, "Line should be disabled after completing a box.");
            }
        }
    }
}
        
     
    
