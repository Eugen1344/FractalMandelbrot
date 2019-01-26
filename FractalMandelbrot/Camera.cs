using OJE.GLFW;

namespace FractalMandelbrot
{
    public static class Camera
    {
        public static bool Enabled = true;
        public static double x;
        public static double y;
        public static double Scale = 1f;

        //public const float MaximumScale = 2f;
        //public const float MinimalScale = 500f;
        public const float MoveSpeed = 6f;
        public const float ZoomSpeed = 2.5f;

        public static void UpdateInput(Glfw.Window window)
        {
            if (!Enabled)
                return;

            double moveFactor = MoveSpeed / Scale;

            if (Glfw.GetKey(window, Glfw.KEY_W) == Glfw.PRESS)
                y += (float)Time.DeltaTime * moveFactor;
            if (Glfw.GetKey(window, Glfw.KEY_A) == Glfw.PRESS)
                x -= (float)Time.DeltaTime * moveFactor;
            if (Glfw.GetKey(window, Glfw.KEY_S) == Glfw.PRESS)
                y -= (float)Time.DeltaTime * moveFactor;
            if (Glfw.GetKey(window, Glfw.KEY_D) == Glfw.PRESS)
                x += (float)Time.DeltaTime * moveFactor;

            double zoomFactor = (double)Time.DeltaTime * ZoomSpeed * Scale;
            if (Glfw.GetKey(window, Glfw.KEY_EQUAL) == Glfw.PRESS)
            {
                double newScale = Scale + zoomFactor;
                //Scale = newScale < MaximumScale ? MaximumScale : newScale;
                Scale = newScale;
            }
            if (Glfw.GetKey(window, Glfw.KEY_MINUS) == Glfw.PRESS)
            {
                double newScale = Scale - zoomFactor;
                //Scale = newScale > MinimalScale ? MinimalScale : newScale;
                Scale = newScale;
            }

        }
    }
}