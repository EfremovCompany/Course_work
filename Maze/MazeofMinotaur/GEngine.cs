using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace MazeofMinotaur
{
    class GEngine
    {
        /*--------------members--------------*/
        private Graphics drawHandle;
        private Thread renderThread;

        private Bitmap tex_shadow;
        private Bitmap tex_dirt;

        /*-------------functions-------------*/
        public GEngine(Graphics g)
        {
            drawHandle = g;
        }
        public void init()
        {
            loadAssets();

            //Starts the render thread
            renderThread = new Thread(new ThreadStart(render));
            renderThread.Start();
        }

        //Load resorces
        private void loadAssets()
        {
            tex_shadow = MazeofMinotaur.Properties.Resources.little_shadow;
            tex_dirt = MazeofMinotaur.Properties.Resources.tex_dirt;
        }

        public void stop()
        {
            renderThread.Abort();
        }
        private void render()
        {
            int framesRendered = 0;
            long startTime = Environment.TickCount;

            Bitmap frame = new Bitmap(Game.CANVAS_WIDTH, Game.CANVAS_HEIGHT);
            Graphics frameGraphics = Graphics.FromImage(frame);

            TextureID[,] textures = Level.Blocks;

            while (true)
            {
                //Background color
                frameGraphics.FillRectangle(new SolidBrush(Color.Aqua), 0, 0, Game.CANVAS_WIDTH, Game.CANVAS_HEIGHT);

                for(int x = 0; x < Game.LEVEL_WIDTH; x++)
                {
                    for(int y = 0; y < Game.LEVEL_HEIGTH; y++)
                    {
                        switch(textures[x, y])
                        {
                            case TextureID.air:
                                break;
                            case TextureID.dirt:
                                frameGraphics.DrawImage(tex_dirt, x * Game.TILE_SIDE_LENGTH, y * Game.TILE_SIDE_LENGTH);
                                break;
                        }
                    }
                }
                //Draw my frame
                frameGraphics.DrawImage(tex_shadow, 100, 100);

                drawHandle.DrawImage(frame, 0, 0);

                framesRendered++;
                if (Environment.TickCount >= startTime + 1000)
                {
                    Console.WriteLine("GEngine: " + framesRendered + " " + "fps");
                    framesRendered = 0;
                    startTime = Environment.TickCount;
                }
            }
        }
    }
}
