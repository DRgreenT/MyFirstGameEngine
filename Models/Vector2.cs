namespace MyFirstGameEngine.Models
{
    public class Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 Zero() => new Vector2(0, 0);

        public Vector2 Up(float value)
        {
            Y -= value;
            return this;
        }

        public Vector2 Down(float value)
        {
            Y += value;
            return this;
        }
        public Vector2 Left(float value)
        {

            X -= value;
            return this;
        }

        public Vector2 Right(float value)
        {
            X += value;
            return this;
        }

        public static Vector2 operator + (Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        public static Vector2 operator - (Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }
    }
}
