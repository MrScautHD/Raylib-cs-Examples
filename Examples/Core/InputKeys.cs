/*******************************************************************************************
*
*   raylib [core] example - Keyboard input
*
*   This example has been created using raylib 1.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2014 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Core
{
    public class InputKeys
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - keyboard input");

            Vector2 ballPosition = new Vector2((float)screenWidth / 2, (float)screenHeight / 2);

            SetTargetFPS(60);       // Set target frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                if (IsKeyDown(KeyboardKey.KEY_RIGHT))
                {
                    ballPosition.X += 2.0f;
                }

                if (IsKeyDown(KeyboardKey.KEY_LEFT))
                {
                    ballPosition.X -= 2.0f;
                }

                if (IsKeyDown(KeyboardKey.KEY_UP))
                {
                    ballPosition.Y -= 2.0f;
                }

                if (IsKeyDown(KeyboardKey.KEY_DOWN))
                {
                    ballPosition.Y += 2.0f;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                DrawText("move the ball with arrow keys", 10, 10, 20, Color.DARKGRAY);

                DrawCircleV(ballPosition, 50, Color.MAROON);

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
