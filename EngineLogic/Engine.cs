using MyFirstGameEngine.Log;
using MyFirstGameEngine.Models;
using System.Diagnostics;

namespace MyFirstGameEngine.EngineLogic
{

    public abstract class Engine
    {
        public bool IsDebugMode = false;

        private CancellationTokenSource cts = new CancellationTokenSource();

        private Vector2 ScreenSize = new Vector2(800, 600);
        private string WindowTitle = "My First Game Engine";
        
        public Canvas Window = new Canvas();
        
        public Color BackGroundColor = Color.Black;

        public static List<Shape2D> shape2Ds = new List<Shape2D>();
        public static List<Sprite2D> sprite2Ds = new List<Sprite2D>();

        public static Dictionary<string, Bitmap> CachedSprites = new Dictionary<string, Bitmap>();

        private Thread GameThread = null!;
        public Engine(Vector2? ScreenSize, string? WindowTitle)
        {
            if (ScreenSize is not null)
            {
                this.ScreenSize = ScreenSize;
            }
            if (WindowTitle is not null)
            {
                this.WindowTitle = WindowTitle;
            }

            Window.SetWindow(new Size((int)this.ScreenSize.X, (int)this.ScreenSize.Y), this.WindowTitle, OnRender!, KeyDown!, KeyUp!);

            ConsoleLog.Info("Game engine initialized!");

            GameThread = new Thread(() => GameLoop(cts.Token));
            GameThread.Start();

            Application.Run(Window);


            cts.Cancel();
            GameThread.Join();
            ConsoleLog.Info("Game engine terminated!");
        }
        public void OnRender(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            foreach (var shape in shape2Ds)
            {
                using (Brush brush = new SolidBrush(shape.Color))
                {
                    g.FillRectangle(brush, shape.Position.X, shape.Position.Y, shape.Size.X, shape.Size.Y);
                }
            }

            foreach (var sprite in sprite2Ds)
            {
                if (sprite != null && sprite.Sprite != null)
                    g.DrawImage(sprite.Sprite, new PointF(sprite.Position.X, sprite.Position.Y));
            }
        }
        public void GameLoop(CancellationToken token)
        {
            int frame = 0;
            OnLoad();
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                int counter = 0;
                while (!token.IsCancellationRequested)
                {
                    stopwatch.Restart();

                    try
                    {
                        OnDraw();
                        Window.BeginInvoke((MethodInvoker)delegate
                        {
                            Window.Refresh();
                        });
                        OnUpdate();
                    }
                    catch (Exception ex)
                    {
                        ConsoleLog.Error(ex.Message);
                    }

                    if (IsDebugMode)
                    {
                        double elapsedTicks = stopwatch.ElapsedTicks;
                        double elapsedMs = elapsedTicks * 1000.0 / Stopwatch.Frequency;
                        double fps = elapsedMs > 0 ? 1000.0 / elapsedMs : 0.0;

                        if (counter % 15 == 0)
                        {
                            ConsoleStats.Frame(ref frame, fps.ToString("0.0").PadLeft(4, ' '));
                            counter = 0;
                        }

                        counter++;
                        if (frame >= int.MaxValue)
                        {
                            ConsoleLog.Warning("Frame overflow detected! Resetting frame count.");
                            frame = 0;
                        }
                        frame++;
                    }

                    Thread.Sleep(1);
                }
                ConsoleLog.Info("Game loop terminated!");
            }
            catch (ThreadInterruptedException e)
            {
                ConsoleLog.Error("Game loop interrupted: " + e.Message);
            }
            catch (Exception e)
            {
                ConsoleLog.Error("An error occurred: " + e.Message);
            }
        }

        public static HashSet<Keys> activeKeys = new();
        public Vector2 InputHandler(float modifier = 1f)
        {
            Vector2 delta = Vector2.Zero();

            if (activeKeys.Contains(Keys.W)) delta.Up(1 * modifier);
            if (activeKeys.Contains(Keys.S)) delta.Down(1 * modifier);
            if (activeKeys.Contains(Keys.A)) delta.Left(1 * modifier);
            if (activeKeys.Contains(Keys.D)) delta.Right(1 * modifier);

            return delta;

        }
        public abstract void OnLoad();
        public abstract void OnDraw();
        public abstract void OnStart();
        public abstract void OnUpdate();
        public abstract void OnStop();

        public abstract void KeyDown(object sender, KeyEventArgs e);
        public abstract void KeyUp(object sender, KeyEventArgs e);

    }
}
