using MyFirstGameEngine.Log;
using MyFirstGameEngine.Models;
using MyFirstGameEngine.EngineLogic;
using System.Diagnostics;

namespace MyFirstGameEngine
{
    internal class DemoGame : Engine
    {


        //Shape2D? player;
        Sprite2D? player;


        string[,] Map = {
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {"w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w", "w" },
        };

        public DemoGame() : base(new Vector2(800, 600), "Demo Game")
        {

        }

        public override void OnLoad()
        {
            IsDebugMode = false;
            BackGroundColor = Color.Black;
            ConsoleLog.Info("Game resources loaded!");
            //player = new Shape2D(new Vector2(100, 100), new Vector2(50, 50), Color.Red, "Player");
            player = new Sprite2D(new Vector2(10, 10), new Vector2(25, 35), "Base pack\\Player\\p1_front", "Player");
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] == "w")
                    {
                        Sprite2D wallSprite = new
                            (
                            new Vector2(j * 50, i * 35),
                            new Vector2(50, 35),
                            "Base pack\\Tiles\\stoneWall",
                            "Wall"

                            );
                    }
                }
            }

        }
        public override void OnDraw()
        {

        }
        public override void OnStart()
        {

        }

        bool Collisions()
        {

                Vector2 windowSize = Window.GetWindowSize();

                return player.IsColliding("Wall") ||
                       player.Position.X + player.Size.X > windowSize.X - 5 ||
                       player.Position.Y + player.Size.Y > windowSize.Y - 5 ||
                       player.Position.X < 0 ||
                       player.Position.Y < 0;
            
        }
        //private Vector2 playerPos = player.Position;
        public override void OnUpdate()
        {


            if (player != null && Window != null)
            {
                Vector2 playerPosOld = player.Position;
                Vector2 moveDelta = InputHandler();
                player.Position = player.Position + moveDelta;
                if (Collisions())
                {
                    player.Position = playerPosOld;
                }              
            }

        }

        public override void OnStop()
        {
            ConsoleLog.Info("Game stopped!");
        }



        public override void KeyDown(object sender, KeyEventArgs e)
        {
            activeKeys.Add(e.KeyCode);
        }

        public override void KeyUp(object sender, KeyEventArgs e)
        {
            activeKeys.Remove(e.KeyCode);
        }
    }
}
