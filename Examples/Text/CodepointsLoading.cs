/*******************************************************************************************
*
*   raylib [text] example - Codepoints loading
*
*   Example originally created with raylib 4.2, last time updated with raylib 2.5
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2022-2023 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Text
{
    class CodepointsLoading
    {
        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [text] example - codepoints loading");

            // Text to be displayed, must be UTF-8 (save this code file as UTF-8)
            // NOTE: It can contain all the required text for the game,
            // this text will be scanned to get all the required codepoints
            const string text =
                "いろはにほへと　ちりぬるを\nわかよたれそ　つねならむ\nうゐのおくやま　けふこえて\nあさきゆめみし　ゑひもせす";

            // Get codepoints from text
            List<int> codepoints = GetCodePoints(text);

            // Remove duplicate codepoints to generate smaller font atlas
            int[] codepointsNoDuplicates = codepoints.Distinct().ToArray();

            // Load font containing all the provided codepoint glyphs
            // A texture font atlas is automatically generated
            Font font = LoadFontEx(
                "resources/DotGothic16-Regular.ttf",
                36,
                codepointsNoDuplicates,
                codepointsNoDuplicates.Length
            );

            // Set bilinear scale filter for better font scaling
            SetTextureFilter(font.texture, TextureFilter.TEXTURE_FILTER_BILINEAR);

            bool showFontAtlas = false;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                if (IsKeyPressed(KeyboardKey.KEY_SPACE))
                {
                    showFontAtlas = !showFontAtlas;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();

                ClearBackground(Color.RAYWHITE);

                DrawRectangle(0, 0, GetScreenWidth(), 70, Color.BLACK);
                DrawText($"Total codepoints contained in provided text: {codepoints.Count}", 10, 10, 20, Color.GREEN);
                DrawText(
                    $"Total codepoints required for font atlas (duplicates excluded): {codepointsNoDuplicates.Length}",
                    10,
                    40,
                    20,
                    Color.GREEN
                );

                if (showFontAtlas)
                {
                    // Draw generated font texture atlas containing provided codepoints
                    DrawTexture(font.texture, 150, 100, Color.BLACK);
                    DrawRectangleLines(150, 100, font.texture.width, font.texture.height, Color.BLACK);
                }
                else
                {
                    // Draw provided text with laoded font, containing all required codepoint glyphs
                    DrawTextEx(font, text, new Vector2(160, 110), 48, 5, Color.BLACK);
                }

                DrawText("Press SPACE to toggle font atlas view!", 10, GetScreenHeight() - 30, 20, Color.GRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadFont(font);

            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }

        private static List<int> GetCodePoints(string text)
        {
            var codePoints = new List<int>();

            StringInfo stringInfo = new StringInfo(text);
            TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(text);

            while (enumerator.MoveNext())
            {
                int codePoint = char.ConvertToUtf32(enumerator.Current.ToString(), 0);
                codePoints.Add(codePoint);
            }

            return codePoints;
        }
    }
}
