using MyFirstGameEngine.Log;
using MyFirstGameEngine.EngineLogic;
using System.Diagnostics.Eventing.Reader;

namespace MyFirstGameEngine.Models
{
    public class Sprite2D
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public string Tag { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public Bitmap? Sprite { get; set; } = null;

        public Sprite2D(Vector2 position, Vector2 size, string imagePath, string tag)
        {
            this.Position = position;
            this.Size = size;
            this.ImagePath = GetImagePath(imagePath);
            this.Tag = tag;
            this.Sprite = LoadOrGetFromCache();
            RegisterSprite();
            CacheSprite();
            ConsoleLog.Info($"Sprite: ({tag}) created and registered -> {this.Sprite != null}.");
        }

        private string GetImagePath(string imgPath, string fileExtension = ".png")
        {
            string directory = Environment.CurrentDirectory;
            string assetFolder = "Assets";
            string spriteFolder = "Sprites";
            directory = Path.Combine(directory, assetFolder, spriteFolder);
            directory = Path.Combine(directory, imgPath + fileExtension);
            return directory;
        }

        private void CacheSprite()
        {
            if (Sprite != null)
            {
                if (Engine.CachedSprites.TryAdd(this.Tag, this.Sprite))
                {
                    ConsoleLog.Info($"{this.Tag} cached!");
                }
                else
                {
                    ConsoleLog.Info($"{this.Tag} already cached!");
                }
            }
            else
            {
                ConsoleLog.Error($"Bitmap {this.Tag} was null and is not cached!");
            }
        }
        private Bitmap? LoadOrGetFromCache()
        {
            if (Engine.CachedSprites.TryGetValue(this.Tag, out var cached))
            {
                ConsoleLog.Info($"{this.Tag} loaded from cache!");
                return cached;
            }


            try
            {
                using Image tmp = Image.FromFile(this.ImagePath);
                Bitmap? bitmap = new Bitmap(tmp, new Size((int)this.Size.X, (int)this.Size.Y));
                Engine.CachedSprites[this.Tag] = bitmap;
                ConsoleLog.Info($"{this.Tag} loaded from disk!");
                return bitmap;
            }
            catch (Exception ex)
            {
                ConsoleLog.Error(ex.Message);
                return null;
            }
        }

        public void RegisterSprite()
        {
            Engine.sprite2Ds.Add(this);
        }
        public void DeleteSprite()
        {
            ConsoleLog.Info($"{this.Tag} -> Sprite deleted");
            Sprite?.Dispose();
            Engine.sprite2Ds.Remove(this);
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
