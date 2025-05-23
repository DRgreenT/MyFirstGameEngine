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

            //if (activeKeys.Contains(Keys.W)) delta.Up(1 * modifier);
            //if (activeKeys.Contains(Keys.S)) delta.Down(1 * modifier);
            if (activeKeys.Contains(Keys.A)) delta.Left(1 * modifier);
            if (activeKeys.Contains(Keys.D)) delta.Right(1 * modifier);
            if (activeKeys.Contains(Keys.Space)) delta.Up(4 * modifier);

            return delta;

        }

        public void Move(Sprite2D player, float jumpForce, float gravity, float maxFallSpeed)
        {
            if (player == null || Window == null) return;

            Vector2 moveDelta = InputHandler();

            bool isGrounded = !IsMoveValid(player, "Ground", player.Position + new Vector2(0, 1));

            if (isGrounded && moveDelta.Y < -0.5f)
            {
                player.Velocity.Y = -jumpForce;
            }

            player.Velocity.X = moveDelta.X;
            player.Velocity.Y += gravity;

            if (player.Velocity.Y > maxFallSpeed)
                player.Velocity.Y = maxFallSpeed;

            // === X-Axis ===
            Vector2 targetPosX = player.Position + new Vector2(player.Velocity.X, 0);
            if (IsMoveValid(player, "Ground", targetPosX))
            {
                player.Position.X = targetPosX.X;
            }
            else
            {
                player.Velocity.X = 0;
            }

            // === Y-Axis ===
            Vector2 targetPosY = player.Position + new Vector2(0, player.Velocity.Y);
            if (IsMoveValid(player, "Ground", targetPosY))
            {
                player.Position.Y = targetPosY.Y;
            }
            else
            {
                player.Velocity.Y = 0;
            }
        }

        /// <summary>
        /// Determines whether the specified sprite can move to a target position without colliding 
        /// with any other sprite that has the given collision tag, and ensures the movement stays within 
        /// the window boundaries.
        /// </summary>
        /// <param name="item">The sprite that is attempting to move.</param>
        /// <param name="tagCollisionElement">The tag of elements to check for potential collisions.</param>
        /// <param name="targetPosition">The position the sprite wants to move to.</param>
        /// <returns>
        /// Returns <c>true</c> if the new position does not overlap any relevant objects and remains within
        /// the bounds of the game window; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMoveValid(Sprite2D item, string tagCollisionElement ,Vector2 targetPosition)
        {
            Vector2 windowSize = Window.GetWindowSize();
            //ConsoleLog.Info($"{targetPosition.X} : {windowSize.X} : {item.Size.X}");
            //ConsoleLog.Info($"{targetPosition.Y} : {windowSize.Y} : {item.Size.Y}");
            return !sprite2Ds.Any(obj =>
                       obj != item &&
                       obj.Tag == tagCollisionElement &&
                       IsOverlapping(targetPosition, item.Size, obj.Position, obj.Size))
                   &&
                   targetPosition.X >= 0 &&
                   targetPosition.Y >= 0 &&
                   targetPosition.X <= windowSize.X - item.Size.X - 15 &&
                   targetPosition.Y <= windowSize.Y - item.Size.Y - 40;
        }

        /// <summary>
        /// Determines whether two rectangles, defined by their positions and sizes, overlap.
        /// </summary>
        /// <param name="pos1">The top-left corner of the first rectangle.</param>
        /// <param name="size1">The size (width and height) of the first rectangle.</param>
        /// <param name="pos2">The top-left corner of the second rectangle.</param>
        /// <param name="size2">The size (width and height) of the second rectangle.</param>
        /// <returns>
        /// Returns <c>true</c> if the two rectangles overlap; otherwise, <c>false</c>.
        /// </returns>
        private bool IsOverlapping(Vector2 pos1, Vector2 size1, Vector2 pos2, Vector2 size2)
        {
            return pos1.X < pos2.X + size2.X &&
                   pos1.X + size1.X > pos2.X &&
                   pos1.Y < pos2.Y + size2.Y &&
                   pos1.Y + size1.Y > pos2.Y;
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
