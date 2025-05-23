using MyFirstGameEngine.Log;
using MyFirstGameEngine.Models;
using MyFirstGameEngine.EngineLogic;
using System.Diagnostics;

namespace MyFirstGameEngine
{
    internal class DemoGame : Engine
    {
        Sprite2D? player;

        const float maxFallSpeed = 5.0f;
        const float gravity = 0.0981f;
        const float jumpForce = 4.75f;

        string[,] Map = {
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "g", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "e" },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "g", ".", "g", "g" },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "g", "g", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", "g", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", "g", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", "g", "g", "g", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", "g", "g", "g", "g", "g", "g", "g", ".", "." },
        {".", ".", ".", ".", "g", "g", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", ".", "." },
        {"g", "g", "g", "g", "g", "g", "g", "g", "g", "g", "g", "g", "g", "g", "g", "g" },
        };

        public DemoGame() : base(new Vector2(800, 600), "Demo Game") {}
        public override void OnLoad()
        {
            IsDebugMode = false;
            ConsoleLog.Info("Game resources loaded!");

            player = new Sprite2D(new Vector2(10, 10), new Vector2(25, 35), "Base pack\\Player\\p1_front", "Player");
            LoadLevel();
        }

        void LoadLevel()
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] == "g")
                    {
                        Sprite2D wallSprite = new
                            (
                            new Vector2(j * 50, i * 35),
                            new Vector2(50, 35),
                            "Base pack\\Tiles\\stoneWall",
                            "Ground"
                            );
                    }
                    if (Map[i,j] == "e")
                    {
                        Sprite2D exit = new (
                            new Vector2((j * 50)+7, (i * 35)+15),
                            new Vector2(20,20), 
                            "Base pack\\Tiles\\signExit",
                            "Exit");
                    }
                }
            }
        }

        public override void OnDraw(){ }
        public override void OnStart(){}
              
        public override void OnUpdate()
        {
            Move(player!,jumpForce,gravity,maxFallSpeed);
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
