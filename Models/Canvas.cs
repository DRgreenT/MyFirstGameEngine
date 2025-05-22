using MyFirstGameEngine.Log;

namespace MyFirstGameEngine.Models
{
    public class Canvas : Form
    {
        public Canvas()
        {
            DoubleBuffered = true;
            Text = "My First Game Engine";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.Black;
        }

        public void SetWindow(Size size, string title, PaintEventHandler printAction, KeyEventHandler keyDownEventHandler, KeyEventHandler keyUpEventHandler)
        {
            this.Size = size;
            this.Text = title;
            this.Paint += printAction;
            this.KeyDown += keyDownEventHandler;
            this.KeyUp += keyUpEventHandler;
            ConsoleLog.Info("Main window initialized!");
        }

        public Vector2 GetWindowSize()
        {
            return new Vector2(this.Size.Width, this.Size.Height);
        }
    }
}
