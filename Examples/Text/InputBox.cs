/*******************************************************************************************
*
*   raylib [text] example - Input Box
*
*   This example has been created using raylib 1.7 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2017 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Text
{
    public class InputBox
    {
        public const int MaxInputChars = 9;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [text] example - input box");

            // NOTE: One extra space required for line ending char '\0'
            char[] name = new char[MaxInputChars];
            int letterCount = 0;

            Rectangle textBox = new Rectangle(screenWidth / 2 - 100, 180, 225, 50);
            bool mouseOnText = false;

            int framesCounter = 0;

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                if (CheckCollisionPointRec(GetMousePosition(), textBox))
                {
                    mouseOnText = true;
                }
                else
                {
                    mouseOnText = false;
                }

                if (mouseOnText)
                {
                    // Set the window's cursor to the I-Beam
                    SetMouseCursor(MouseCursor.MOUSE_CURSOR_IBEAM);

                    // Check if more characters have been pressed on the same frame
                    int key = GetCharPressed();

                    while (key > 0)
                    {
                        // NOTE: Only allow keys in range [32..125]
                        if ((key >= 32) && (key <= 125) && (letterCount < MaxInputChars))
                        {
                            name[letterCount] = (char)key;
                            letterCount++;
                        }

                        // Check next character in the queue
                        key = GetCharPressed();
                    }

                    if (IsKeyPressed(KeyboardKey.KEY_BACKSPACE))
                    {
                        letterCount -= 1;
                        if (letterCount < 0)
                        {
                            letterCount = 0;
                        }
                        name[letterCount] = '\0';
                    }
                }
                else
                {
                    SetMouseCursor(MouseCursor.MOUSE_CURSOR_DEFAULT);
                }

                if (mouseOnText)
                {
                    framesCounter += 1;
                }
                else
                {
                    framesCounter = 0;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                DrawText("PLACE MOUSE OVER INPUT BOX!", 240, 140, 20, Color.GRAY);
                DrawRectangleRec(textBox, Color.LIGHTGRAY);

                if (mouseOnText)
                {
                    DrawRectangleLines(
                        (int)textBox.x,
                        (int)textBox.y,
                        (int)textBox.width,
                        (int)textBox.height,
                        Color.RED
                    );
                }
                else
                {
                    DrawRectangleLines(
                        (int)textBox.x,
                        (int)textBox.y,
                        (int)textBox.width,
                        (int)textBox.height,
                        Color.DARKGRAY
                    );
                }

                DrawText(new string(name), (int)textBox.x + 5, (int)textBox.y + 8, 40, Color.MAROON);
                DrawText($"INPUT CHARS: {letterCount}/{MaxInputChars}", 315, 250, 20, Color.DARKGRAY);

                if (mouseOnText)
                {
                    if (letterCount < MaxInputChars)
                    {
                        // Draw blinking underscore char
                        if ((framesCounter / 20 % 2) == 0)
                        {
                            DrawText(
                                "_",
                                (int)textBox.x + 8 + MeasureText(new string(name), 40),
                                (int)textBox.y + 12,
                                40,
                                Color.MAROON
                            );
                        }
                    }
                    else
                    {
                        DrawText("Press BACKSPACE to delete chars...", 230, 300, 20, Color.GRAY);
                    }
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
