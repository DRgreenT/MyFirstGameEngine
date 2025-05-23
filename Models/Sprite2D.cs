using MyFirstGameEngine.EngineLogic;
using MyFirstGameEngine.Log;
using MyFirstGameEngine.Models;

/// <summary>
/// Represents a 2D visual object (sprite) within the game world.
/// A Sprite2D can be rendered, moved, cloned, cached, and checked for collisions.
/// It maintains a static list of active sprites and a shared cache for sprite bitmaps.
/// </summary>
public class Sprite2D
{
    /// <summary>
    /// The current position of the sprite in the game world.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// The size of the sprite in pixels (width and height).
    /// </summary>
    public Vector2 Size { get; set; }

    /// <summary>
    /// The velocity vector used to update the sprite’s movement over time.
    /// </summary>
    public Vector2 Velocity { get; set; } = Vector2.Zero();

    /// <summary>
    /// A string tag used to identify or categorize the sprite (e.g., "Player", "Wall").
    /// </summary>
    public string Tag { get; set; } = "";

    /// <summary>
    /// The file path to the image asset used for rendering the sprite.
    /// </summary>
    public string ImagePath { get; set; } = "";

    /// <summary>
    /// The actual bitmap object used to draw the sprite.
    /// </summary>
    public Bitmap? Sprite { get; set; } = null;

    private static List<Sprite2D> ActiveSprites = new List<Sprite2D>();
    private static Dictionary<string, Bitmap> CachedSprites = new Dictionary<string, Bitmap>();

    /// <summary>
    /// Gets the current list of active Sprite2D instances in the scene.
    /// </summary>
    public static List<Sprite2D> GetActiveSprites()
    {
        return ActiveSprites;
    }

    /// <summary>
    /// Creates a new Sprite2D instance with the given position, size, image path, and tag.
    /// Automatically loads and caches the image, and registers the sprite globally.
    /// </summary>
    /// <param name="position">Initial position of the sprite.</param>
    /// <param name="size">Size of the sprite in pixels.</param>
    /// <param name="imagePath">Relative path to the sprite image file (without extension).</param>
    /// <param name="tag">Tag used for categorizing the sprite.</param>
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

    /// <summary>
    /// Builds the full absolute file path to the sprite image based on its relative path and optional extension.
    /// </summary>
    /// <param name="imgPath">Relative path to the image (without extension).</param>
    /// <param name="fileExtension">File extension (default is ".png").</param>
    /// <returns>Full path to the image file.</returns>
    private string GetImagePath(string imgPath, string fileExtension = ".png")
    {
        string directory = Environment.CurrentDirectory;
        string assetFolder = "Assets";
        string spriteFolder = "Sprites";
        directory = Path.Combine(directory, assetFolder, spriteFolder);
        directory = Path.Combine(directory, imgPath + fileExtension);
        return directory;
    }

    /// <summary>
    /// Adds the sprite’s bitmap to the global cache if not already present.
    /// </summary>
    private void CacheSprite()
    {
        if (Sprite != null)
        {
            if (CachedSprites.TryAdd(this.Tag, this.Sprite))
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

    /// <summary>
    /// Attempts to retrieve the sprite from cache.
    /// If not found, loads it from disk, resizes it, and caches it.
    /// </summary>
    /// <returns>The loaded or cached Bitmap instance.</returns>
    private Bitmap? LoadOrGetFromCache()
    {
        if (CachedSprites.TryGetValue(this.Tag, out var cached))
        {
            ConsoleLog.Info($"{this.Tag} loaded from cache!");
            return cached;
        }

        try
        {
            using Image tmp = Image.FromFile(this.ImagePath);
            Bitmap? bitmap = new Bitmap(tmp, new Size((int)this.Size.X, (int)this.Size.Y));
            CachedSprites[this.Tag] = bitmap;
            ConsoleLog.Info($"{this.Tag} loaded from disk!");
            return bitmap;
        }
        catch (Exception ex)
        {
            ConsoleLog.Error(ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Adds this sprite to the global list of active sprites.
    /// </summary>
    public void RegisterSprite()
    {
        ActiveSprites.Add(this);
    }

    /// <summary>
    /// Disposes the sprite’s image resource and removes it from the active list.
    /// </summary>
    public void DeleteSprite()
    {
        ConsoleLog.Info($"{this.Tag} -> Sprite deleted");
        Sprite?.Dispose();
        ActiveSprites.Remove(this);
    }

    /// <summary>
    /// Creates a new Sprite2D instance that is a copy of this one.
    /// The clone is automatically registered as a new sprite.
    /// </summary>
    /// <returns>A new Sprite2D instance with copied properties.</returns>
    public Sprite2D Clone()
    {
        return new Sprite2D(
            new Vector2(Position.X, Position.Y),
            new Vector2(Size.X, Size.Y),
            ImagePath,
            Tag
        );
    }

    /// <summary>
    /// Checks whether this sprite is colliding with any other active sprite that has the specified tag.
    /// Collision is determined using axis-aligned bounding box (AABB) logic.
    /// </summary>
    /// <param name="collingObjectTag">The tag to check for collision against.</param>
    /// <returns>True if a collision is detected; otherwise, false.</returns>
    public bool IsColliding(string collingObjectTag)
    {
        foreach (var item in Sprite2D.ActiveSprites)
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
