/*******************************************************************************************
*
*   raylib [core] example - Scissor test
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Chris Dill (@MysteriousSpace) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2019 Chris Dill (@MysteriousSpace)
*
********************************************************************************************/

using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Core
{
    public class ScissorTest
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - scissor test");

            Rectangle scissorArea = new Rectangle(0, 0, 300, 300);
            bool scissorMode = true;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                if (IsKeyPressed(KeyboardKey.KEY_S))
                {
                    scissorMode = !scissorMode;
                }

                // Centre the scissor area around the mouse position
                scissorArea.x = GetMouseX() - scissorArea.width / 2;
                scissorArea.y = GetMouseY() - scissorArea.height / 2;
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                if (scissorMode)
                {
                    BeginScissorMode((int)scissorArea.x, (int)scissorArea.y, (int)scissorArea.width, (int)scissorArea.height);
                }

                // Draw full screen rectangle and some text
                // NOTE: Only part defined by scissor area will be rendered
                DrawRectangle(0, 0, GetScreenWidth(), GetScreenHeight(), Color.RED);
                DrawText("Move the mouse around to reveal this text!", 190, 200, 20, Color.LIGHTGRAY);

                if (scissorMode)
                {
                    EndScissorMode();
                }

                DrawRectangleLinesEx(scissorArea, 1, Color.BLACK);
                DrawText("Press S to toggle scissor test", 10, 10, 20, Color.BLACK);

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
