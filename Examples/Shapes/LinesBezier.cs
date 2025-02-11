/*******************************************************************************************
*
*   raylib [shapes] example - Cubic-bezier lines
*
*   This example has been created using raylib 1.7 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2017 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Shapes
{
    public class LinesBezier
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
            InitWindow(screenWidth, screenHeight, "raylib [shapes] example - cubic-bezier lines");

            Vector2 start = new Vector2(0, 0);
            Vector2 end = new Vector2(screenWidth, screenHeight);

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                if (IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    start = GetMousePosition();
                }
                else if (IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
                {
                    end = GetMousePosition();
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                DrawText("USE MOUSE LEFT-RIGHT CLICK to DEFINE LINE START and END POINTS", 15, 20, 20, Color.GRAY);
                DrawLineBezier(start, end, 2.0f, Color.RED);

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
