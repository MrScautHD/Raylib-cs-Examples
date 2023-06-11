/*******************************************************************************************
*
*   raylib [textures] example - Texture loading and drawing a part defined by a rectangle
*
*   This example has been created using raylib 1.3 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2014 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Textures
{
    public class SpriteAnim
    {
        public const int MaxFrameSpeed = 15;
        public const int MinFrameSpeed = 1;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [texture] example - texture rectangle");

            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
            Texture2D scarfy = LoadTexture("resources/scarfy.png");

            Vector2 position = new Vector2(350.0f, 280.0f);
            Rectangle frameRec = new Rectangle(0.0f, 0.0f, (float)scarfy.width / 6, (float)scarfy.height);
            int currentFrame = 0;

            int framesCounter = 0;

            // Number of spritesheet frames shown by second
            int framesSpeed = 8;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                framesCounter++;

                if (framesCounter >= (60 / framesSpeed))
                {
                    framesCounter = 0;
                    currentFrame++;

                    if (currentFrame > 5)
                    {
                        currentFrame = 0;
                    }

                    frameRec.x = (float)currentFrame * (float)scarfy.width / 6;
                }

                if (IsKeyPressed(KeyboardKey.KEY_RIGHT))
                {
                    framesSpeed++;
                }
                else if (IsKeyPressed(KeyboardKey.KEY_LEFT))
                {
                    framesSpeed--;
                }

                framesSpeed = Math.Clamp(framesSpeed, MinFrameSpeed, MaxFrameSpeed);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                DrawTexture(scarfy, 15, 40, Color.WHITE);
                DrawRectangleLines(15, 40, scarfy.width, scarfy.height, Color.LIME);
                DrawRectangleLines(
                    15 + (int)frameRec.x,
                    40 + (int)frameRec.y,
                    (int)frameRec.width,
                    (int)frameRec.height,
                    Color.RED
                );

                DrawText("FRAME SPEED: ", 165, 210, 10, Color.DARKGRAY);
                DrawText($"{framesSpeed:2F} FPS", 575, 210, 10, Color.DARKGRAY);
                DrawText("PRESS RIGHT/LEFT KEYS to CHANGE SPEED!", 290, 240, 10, Color.DARKGRAY);

                for (int i = 0; i < MaxFrameSpeed; i++)
                {
                    if (i < framesSpeed)
                    {
                        DrawRectangle(250 + 21 * i, 205, 20, 20, Color.RED);
                    }
                    DrawRectangleLines(250 + 21 * i, 205, 20, 20, Color.MAROON);
                }

                // Draw part of the texture
                DrawTextureRec(scarfy, frameRec, position, Color.WHITE);
                DrawText("(c) Scarfy sprite by Eiden Marsal", screenWidth - 200, screenHeight - 20, 10, Color.GRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(scarfy);

            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
