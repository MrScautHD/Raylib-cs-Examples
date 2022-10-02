/*******************************************************************************************
*
*   raylib [text] example - Font filters
*
*   After font loading, font texture atlas filter could be configured for a softer
*   display of the font when scaling it to different sizes, that way, it's not required
*   to generate multiple fonts at multiple sizes (as long as the scaling is not very different)
*
*   This example has been created using raylib 1.3.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;
using static Raylib_cs.TextureFilter;

namespace Examples.Text
{
    public class FontFilters
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [text] example - font filters");

            string msg = "Loaded Font";

            // NOTE: Textures/Fonts MUST be loaded after Window initialization (OpenGL context is required)

            // TTF Font loading with custom generation parameters
            Font font = LoadFontEx("resources/fonts/KAISG.ttf", 96, null, 0);

            // Generate mipmap levels to use trilinear filtering
            // NOTE: On 2D drawing it won't be noticeable, it looks like TEXTURE_FILTER_BILINEAR
            GenTextureMipmaps(ref font.texture);

            float fontSize = font.baseSize;
            Vector2 fontPosition = new Vector2(40, screenHeight / 2 - 80);
            Vector2 textSize = new Vector2(0.0f, 0.0f);

            // Setup texture scaling filter
            SetTextureFilter(font.texture, TEXTURE_FILTER_POINT);
            TextureFilter currentFontFilter = TEXTURE_FILTER_POINT;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                fontSize += GetMouseWheelMove() * 4.0f;

                // Choose font texture filter method
                if (IsKeyPressed(KEY_ONE))
                {
                    SetTextureFilter(font.texture, TEXTURE_FILTER_POINT);
                    currentFontFilter = TEXTURE_FILTER_POINT;
                }
                else if (IsKeyPressed(KEY_TWO))
                {
                    SetTextureFilter(font.texture, TEXTURE_FILTER_BILINEAR);
                    currentFontFilter = TEXTURE_FILTER_BILINEAR;
                }
                else if (IsKeyPressed(KEY_THREE))
                {
                    // NOTE: Trilinear filter won't be noticed on 2D drawing
                    SetTextureFilter(font.texture, TEXTURE_FILTER_TRILINEAR);
                    currentFontFilter = TEXTURE_FILTER_TRILINEAR;
                }

                textSize = MeasureTextEx(font, msg, fontSize, 0);

                if (IsKeyDown(KEY_LEFT))
                {
                    fontPosition.X -= 10;
                }
                else if (IsKeyDown(KEY_RIGHT))
                {
                    fontPosition.X += 10;
                }

                // Load a dropped TTF file dynamically (at current fontSize)
                if (IsFileDropped())
                {
                    string[] files = Raylib.GetDroppedFiles();

                    // NOTE: We only support first ttf file dropped
                    if (IsFileExtension(files[0], ".ttf"))
                    {
                        UnloadFont(font);
                        font = LoadFontEx(files[0], (int)fontSize, null, 0);
                    }
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(RAYWHITE);

                DrawText("Use mouse wheel to change font size", 20, 20, 10, GRAY);
                DrawText("Use KEY_RIGHT and KEY_LEFT to move text", 20, 40, 10, GRAY);
                DrawText("Use 1, 2, 3 to change texture filter", 20, 60, 10, GRAY);
                DrawText("Drop a new TTF font for dynamic loading", 20, 80, 10, DARKGRAY);

                DrawTextEx(font, msg, fontPosition, fontSize, 0, BLACK);

                DrawRectangle(0, screenHeight - 80, screenWidth, 80, LIGHTGRAY);
                DrawText("CURRENT TEXTURE FILTER:", 250, 400, 20, GRAY);

                if (currentFontFilter == TEXTURE_FILTER_POINT)
                {
                    DrawText("POINT", 570, 400, 20, BLACK);
                }
                else if (currentFontFilter == TEXTURE_FILTER_POINT)
                {
                    DrawText("BILINEAR", 570, 400, 20, BLACK);
                }
                else if (currentFontFilter == TEXTURE_FILTER_TRILINEAR)
                {
                    DrawText("TRILINEAR", 570, 400, 20, BLACK);
                }

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadFont(font);           // Font unloading
            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
