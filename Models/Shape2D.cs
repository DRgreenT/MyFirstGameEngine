using MyFirstGameEngine.Log;
using MyFirstGameEngine.EngineLogic;

namespace MyFirstGameEngine.Models
{
    public class Shape2D
    {

        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Color Color { get; set; }

        public string Tag { get; set; } = "";

        public Shape2D(Vector2 position, Vector2 size, Color color, string tag)
        {
            Position = position;
            Size = size;
            Color = color;
            Tag = tag;
            RegisterShape();
            ConsoleLog.Info($"Shape: ({tag} created and registered.");
        }
        void RegisterShape()
        {
            Engine.shape2Ds.Add(this);
        }

        public void DeleteShape()
        {
            Engine.shape2Ds.Remove(this);
            ConsoleLog.Info($"{this.Tag} -> Shape deleted");
        }

        public bool IsColliding(string collingObjectTag)
        {

            foreach (var item in Engine.sprite2Ds)
            {

                if (item.Tag == collingObjectTag)
                {

                    if (this.Position.X < item.Position.X + item.Size.X &&
                        this.Position.X + this.Size.X > item.Position.X &&
                        this.Position.Y < item.Position.Y + item.Size.Y &&
                        this.Position.Y + this.Size.Y > item.Position.Y)
                    {
                        ConsoleLog.Info($"{this.Tag} colliding with {collingObjectTag}");
                        return true;
                    }
                }

            }
            return false;
        }
    }
}
