/*******************************************************************************************
*
*   raylib [shapes] example - easings rectangle array
*
*   NOTE: This example requires 'easings.h' library, provided on raylib/src. Just copy
*   the library to same directory as example or make sure it's available on include path.
*
*   This example has been created using raylib 2.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2014-2019 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Shapes
{
    public class EasingsRectangleArray
    {
        public const int RecsWidth = 50;
        public const int RecsHeight = 50;
        public const int MaxRecsX = 800 / RecsWidth;
        public const int MaxRecsY = 450 / RecsHeight;

        // At 60 fps = 4 seconds
        public const int PlayTimeInFrames = 240;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [shapes] example - easings rectangle array");

            Rectangle[] recs = new Rectangle[MaxRecsX * MaxRecsY];

            for (int y = 0; y < MaxRecsY; y++)
            {
                for (int x = 0; x < MaxRecsX; x++)
                {
                    recs[y * MaxRecsX + x].x = RecsWidth / 2 + RecsWidth * x;
                    recs[y * MaxRecsX + x].y = RecsHeight / 2 + RecsHeight * y;
                    recs[y * MaxRecsX + x].width = RecsWidth;
                    recs[y * MaxRecsX + x].height = RecsHeight;
                }
            }

            float rotation = 0.0f;
            int framesCounter = 0;

            // Rectangles animation state: 0-Playing, 1-Finished
            int state = 0;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                if (state == 0)
                {
                    framesCounter++;

                    for (int i = 0; i < MaxRecsX * MaxRecsY; i++)
                    {
                        recs[i].height = Easings.EaseCircOut(framesCounter, RecsHeight, -RecsHeight, PlayTimeInFrames);
                        recs[i].width = Easings.EaseCircOut(framesCounter, RecsWidth, -RecsWidth, PlayTimeInFrames);

                        if (recs[i].height < 0)
                        {
                            recs[i].height = 0;
                        }
                        if (recs[i].width < 0)
                        {
                            recs[i].width = 0;
                        }

                        // Finish playing
                        if ((recs[i].height == 0) && (recs[i].width == 0))
                        {
                            state = 1;
                        }
                        rotation = Easings.EaseLinearIn(framesCounter, 0.0f, 360.0f, PlayTimeInFrames);
                    }
                }
                else if ((state == 1) && IsKeyPressed(KeyboardKey.KEY_SPACE))
                {
                    // When animation has finished, press space to restart
                    framesCounter = 0;

                    for (int i = 0; i < MaxRecsX * MaxRecsY; i++)
                    {
                        recs[i].height = RecsHeight;
                        recs[i].width = RecsWidth;
                    }

                    state = 0;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                if (state == 0)
                {
                    for (int i = 0; i < MaxRecsX * MaxRecsY; i++)
                    {
                        DrawRectanglePro(
                            recs[i],
                            new Vector2(recs[i].width / 2, recs[i].height / 2),
                            rotation,
                            Color.RED
                        );
                    }
                }
                else if (state == 1)
                {
                    DrawText("PRESS [SPACE] TO PLAY AGAIN!", 240, 200, 20, Color.GRAY);
                }

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
