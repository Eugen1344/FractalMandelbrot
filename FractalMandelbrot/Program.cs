using OJE.GL;
using OJE.GLFW;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace FractalMandelbrot
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public Vector2 pos;
        public Color color;

        public Vertex(Vector2 pos, Color color)
        {
            this.pos = pos;
            this.color = color;
        }
    }

    unsafe class Program
    {
        public static Glfw.Window MainWindow;
        public static int ShaderProgram;

        public const int ScreenWidth = 800;
        public const int ScreenHeight = 800;

        private static void Main(string[] args)
        {
            MainWindow = InitGl(ScreenWidth, ScreenHeight, "Fractal");
            ShaderProgram = LoadShaders("vertexShader.glsl", "fractal.glsl");
            GL.LinkProgram(ShaderProgram);

            int vao = GL.GenVertexArray();
            int vbo = GL.GenBuffer();

            GL.BindVertexArray(vao);
            Color color = Color.Blue;
            Vertex[] vertexData = new Vertex[4];
            vertexData[0] = new Vertex { pos = new Vector2(-1, -1), color = color };
            vertexData[1] = new Vertex { pos = new Vector2(-1, 1), color = color };
            vertexData[2] = new Vertex { pos = new Vector2(1, -1), color = color };
            vertexData[3] = new Vertex { pos = new Vector2(1, 1), color = color };
            GL.BindBuffer(GL.ARRAY_BUFFER, vbo);

            GL.VertexAttribPointer(0, 2, GL.FLOAT, false, sizeof(Vertex), IntPtr.Zero);
            GL.VertexAttribPointer(1, 4, GL.FLOAT, false, sizeof(Vertex), (void*)sizeof(Vector2));
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            fixed (void* pointer = vertexData)
                GL.BufferData(GL.ARRAY_BUFFER, sizeof(Vertex) * vertexData.Length, pointer, GL.STATIC_DRAW);

            int screenSizeUniform = GL.GetUniformLocation(ShaderProgram, "screenSize");
            int cameraPosUniform = GL.GetUniformLocation(ShaderProgram, "cameraPos");
            int scaleFactorUniform = GL.GetUniformLocation(ShaderProgram, "scaleFactor");

            GL.UseProgram(ShaderProgram);
            GL.Uniform2(screenSizeUniform, ScreenWidth, ScreenHeight);

            Stopwatch time = new Stopwatch();
            time.Start();
            while (Glfw.WindowShouldClose(MainWindow) == 0)
            {
                Glfw.PollEvents();

                GL.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);

                Camera.UpdateInput(MainWindow);
                GL.Uniform2(cameraPosUniform, Camera.x, Camera.y);
                GL.Uniform1(scaleFactorUniform, Camera.Scale);
                GL.DrawArrays(GL.TRIANGLE_STRIP, 0, vertexData.Length);

                Glfw.SwapBuffers(MainWindow);

                Time.DeltaTime = time.Elapsed.TotalSeconds;
                time.Restart();
            }

            Glfw.WaitEvents();
            Glfw.Terminate();
        }

        private static int LoadShaders(string vertexPath, string fragmentPath)
        {
            int shaderObj = GL.CreateProgram();

            if (!string.IsNullOrWhiteSpace(vertexPath))
            {
                int shader = GL.CreateShader(GL.VERTEX_SHADER);
                GL.ShaderSource(shader, 1, new[] { File.ReadAllText(Path.Combine("Shaders\\", vertexPath)) }, null);
                GL.CompileShader(shader);
                GL.AttachShader(shaderObj, shader);
                GL.DeleteShader(shader);

                string message = GL.GetShaderInfoLog(shader, 512);
                Console.WriteLine(message);
            }

            if (!string.IsNullOrWhiteSpace(fragmentPath))
            {
                int shader = GL.CreateShader(GL.FRAGMENT_SHADER);
                GL.ShaderSource(shader, 1, new[] { File.ReadAllText(Path.Combine("Shaders\\", fragmentPath)) }, null);
                GL.CompileShader(shader);
                GL.AttachShader(shaderObj, shader);
                GL.DeleteShader(shader);

                string message = GL.GetShaderInfoLog(shader, 512);
                Console.WriteLine(message);
            }

            return shaderObj;
        }

        private static Glfw.Window InitGl(int width, int height, string name)
        {
            Glfw.Init();
            Glfw.Window window = Glfw.CreateWindow(width, height, name);
            Glfw.Vidmode vidmode = Glfw.GetVideoMode(Glfw.GetPrimaryMonitor());
            Glfw.SetWindowPos(window, (vidmode.Width - width) / 2, (vidmode.Height - height) / 2);

            Glfw.WindowHint(Glfw.CONTEXT_VERSION_MAJOR, 4);
            Glfw.WindowHint(Glfw.CONTEXT_VERSION_MINOR, 1);
            Glfw.MakeContextCurrent(window);
            GL.Viewport(0, 0, width, height);
            GL.Enable(GL.BLEND);
            GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);

            GL.ClearColor(1, 0, 1, 1);

            return window;
        }
    }
}