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
            Assert.IsTrue(game.CurrentTurn == Player.Red && game.GameActive == true && game.lines.Count == 24 && game.spaces.Count == 9 , msg);
            TestContext.WriteLine(msg);
        }
        [Test]
        public void TestDoTurn()
        {
            Game game = new Game();
            game.StartGame();
            game.DoTurn(0);
            string msg = $"If the game is active, line backcolor = {game.lines[0].BackColor} current turn = {game.CurrentTurn.ToString()}";
            Assert.IsTrue(game.GameActive == true && game.LinePlayingColor == Color.FromArgb(250, 125, 125) && game.CurrentTurn == Player.Orange, msg);
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

        [Test]
        public void DoTurn_ShouldUpdateScore_WhenWinningSetCompleted()
        {
            // Arrange
            var game = new Game();
            game.StartGame();

            // Act
            // The winning set #0 consists of lines: 0, 3, 4, 7 (from your lstwinningsets)
            // We'll simulate turns that claim these lines:
            game.DoTurn(0); // Red plays line 0
            game.DoTurn(1); // Orange plays line 1 (irrelevant for the winning set)
            game.DoTurn(3); // Red plays line 3
            game.DoTurn(2); // Orange plays line 2 (irrelevant for the winning set)
            game.DoTurn(4); // Red plays line 4
            game.DoTurn(5); // Orange plays line 5 (irrelevant)
            game.DoTurn(7); // Red plays line 7 - this should complete the winning set and update the score

            // Assert
            // Since Red completed the set, Red's score should have increased by 1
            string msg = $"Red Player Score should be 1, and Orange player should be 0. RedPlayerScore: {game.Scores[Player.Red]} ,OrangePlayerScore: {game.Scores[Player.Orange]} ";
            Assert.That(game.Scores[Player.Red], Is.EqualTo(1),msg);
            Assert.That(game.Scores[Player.Orange], Is.EqualTo(0), msg);
            TestContext.WriteLine(msg);
        }
    
}
}
        
     
    
