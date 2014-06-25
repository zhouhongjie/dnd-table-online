using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Entities;
using DndTable.Core.Factories;
using NUnit.Framework;

namespace DndTable.Core.Test.UnitTests
{
    [TestFixture]
    public class MoveTest
    {
        private static Game CreateGame(Board board)
        {
            return new Game(board, null);
        }

        [Test]
        public void OneStep()
        {
            {
                MoveAction moveAction = CreateMoveAction(10);

                Assert.IsFalse(moveAction.DoOneStep(Position.Create(1, 3)), "too far for 1 step");
                Assert.IsTrue(moveAction.DoOneStep(Position.Create(1, 2)));
            }
            {
                MoveAction moveAction = CreateMoveAction(10);

                Assert.IsTrue(moveAction.DoOneStep(Position.Create(2, 2)));
            }
        }

        [Test]
        public void MaxNrOfSteps()
        {
            {
                MoveAction moveAction = CreateMoveAction(10);

                Assert.IsTrue(moveAction.DoOneStep(Position.Create(1, 2)));
                Assert.IsTrue(moveAction.DoOneStep(Position.Create(1, 3)));
                Assert.IsFalse(moveAction.DoOneStep(Position.Create(1, 4)), "max nrOfSteps exceeded");
            }
            {
                MoveAction moveAction = CreateMoveAction(5);

                Assert.IsTrue(moveAction.DoOneStep(Position.Create(1, 2)));
                Assert.IsFalse(moveAction.DoOneStep(Position.Create(1, 3)), "max nrOfSteps exceeded");
            }
        }

        [Test]
        public void DisableAfterDo()
        {
            MoveAction moveAction = CreateMoveAction(10);

            moveAction.Do();
            Assert.IsFalse(moveAction.DoOneStep(Position.Create(1, 2)));
        }

        private MoveAction CreateMoveAction(int speed)
        {
            var board = new Board(10, 10);
            var game = CreateGame(board);
            var actionFactory = new AbstractActionFactory(null, board, null);

            // 2 steps max
            var sheet = new CharacterSheet { Speed = speed };
            var char1 = new Character(sheet);

            game.AddCharacter(char1, Position.Create(1, 1));


            var moveAction = new MoveAction(char1);
            moveAction.Initialize(actionFactory);
            return moveAction;
        }

        [Test]
        public void StepByStepMove()
        {
            var board = new Board(10, 10);
            var game = CreateGame(board);
            var actionFactory = new AbstractActionFactory(null, board, null);

            // 2 steps max
            var sheet = new CharacterSheet {Speed = 10};
            var char1 = new Character(sheet);

            game.AddCharacter(char1, Position.Create(1, 1));
            Assert.AreEqual(char1, game.GameBoard.GetEntity(Position.Create(1, 1), EntityTypeEnum.Character));

            var moveAction = new MoveAction(char1);
            moveAction.Initialize(actionFactory);

            // Step
            Assert.IsTrue(moveAction.DoOneStep(Position.Create(1, 2)));
            AssertCharMoved(game, char1, Position.Create(1, 2));

            // Step
            Assert.IsTrue(moveAction.DoOneStep(Position.Create(1, 3)));
            AssertCharMoved(game, char1, Position.Create(1, 3));

            // Cannot step anymore
            Assert.IsFalse(moveAction.DoOneStep(Position.Create(1, 4)), "max nrOfSteps exceeded");
        }

        private void AssertCharMoved(Game game, ICharacter char1, Position position)
        {
            Assert.AreEqual(char1, game.GameBoard.GetEntity(position, EntityTypeEnum.Character));
            Assert.AreEqual(position.X, char1.Position.X);
            Assert.AreEqual(position.Y, char1.Position.Y);
        }

        [Test]
        public void CannotStepOnEntity()
        {
            var board = new Board(10, 10);
            var game = CreateGame(board);
            var actionFactory = new AbstractActionFactory(null, board, null);

            // 2 steps max
            var sheet = new CharacterSheet {Speed = 10};
            var char1 = new Character(sheet);
            game.AddCharacter(char1, Position.Create(1, 1));

            // Add obstacles
            game.AddCharacter(Factory.CreateCharacter("obstacle1"), Position.Create(1, 2));
            game.AddWall(Position.Create(2, 1));

            var moveAction = new MoveAction(char1);
            moveAction.Initialize(actionFactory);

            Assert.IsFalse(moveAction.DoOneStep(Position.Create(1, 2)), "stepped on obstacle");
            Assert.IsFalse(moveAction.DoOneStep(Position.Create(2, 1)), "stepped on obstacle");
        }
    }
}
