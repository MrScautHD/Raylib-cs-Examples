/*******************************************************************************************
*
*   raylib [textures] example - Image processing
*
*   NOTE: Images are loaded in CPU memory (RAM); textures are loaded in GPU memory (VRAM)
*
*   This example has been created using raylib 1.4 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2016 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Textures
{
    public class ImageProcessing
    {
        public const int NumProcesses = 9;

        enum ImageProcess
        {
            None = 0,
            ColorGrayScale,
            ColorTint,
            ColorInvert,
            ColorContrast,
            ColorBrightness,
            GaussianBlur,
            FlipVertical,
            FlipHorizontal
        }

        static string[] processText = {
            "NO PROCESSING",
            "COLOR GRAYSCALE",
            "COLOR TINT",
            "COLOR INVERT",
            "COLOR CONTRAST",
            "COLOR BRIGHTNESS",
            "GAUSSIAN BLUR",
            "FLIP VERTICAL",
            "FLIP HORIZONTAL"
        };

        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [textures] example - image processing");

            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
            Image imageOrigin = LoadImage("resources/parrots.png");
            ImageFormat(ref imageOrigin, PixelFormat.PIXELFORMAT_UNCOMPRESSED_R8G8B8A8);
            Texture2D texture = LoadTextureFromImage(imageOrigin);

            Image imageCopy = ImageCopy(imageOrigin);

            ImageProcess currentProcess = ImageProcess.None;
            bool textureReload = false;

            Rectangle[] toggleRecs = new Rectangle[NumProcesses];
            int mouseHoverRec = -1;

            for (int i = 0; i < NumProcesses; i++)
            {
                toggleRecs[i] = new Rectangle(40, 50 + 32 * i, 150, 30);
            }

            SetTargetFPS(60);
            //---------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())    // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------

                // Mouse toggle group logic
                for (int i = 0; i < NumProcesses; i++)
                {
                    if (CheckCollisionPointRec(GetMousePosition(), toggleRecs[i]))
                    {
                        mouseHoverRec = i;

                        if (IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
                        {
                            currentProcess = (ImageProcess)i;
                            textureReload = true;
                        }
                        break;
                    }
                    else
                    {
                        mouseHoverRec = -1;
                    }
                }

                // Keyboard toggle group logic
                if (IsKeyPressed(KeyboardKey.KEY_DOWN))
                {
                    currentProcess++;
                    if ((int)currentProcess > (NumProcesses - 1))
                    {
                        currentProcess = 0;
                    }

                    textureReload = true;
                }
                else if (IsKeyPressed(KeyboardKey.KEY_UP))
                {
                    currentProcess--;
                    if (currentProcess < 0)
                    {
                        currentProcess = ImageProcess.FlipHorizontal;
                    }

                    textureReload = true;
                }

                if (textureReload)
                {
                    UnloadImage(imageCopy);
                    imageCopy = ImageCopy(imageOrigin);

                    // NOTE: Image processing is a costly CPU process to be done every frame,
                    // If image processing is required in a frame-basis, it should be done
                    // with a texture and by shaders
                    switch (currentProcess)
                    {
                        case ImageProcess.ColorGrayScale:
                            ImageColorGrayscale(ref imageCopy);
                            break;
                        case ImageProcess.ColorTint:
                            ImageColorTint(ref imageCopy, Color.GREEN);
                            break;
                        case ImageProcess.ColorInvert:
                            ImageColorInvert(ref imageCopy);
                            break;
                        case ImageProcess.ColorContrast:
                            ImageColorContrast(ref imageCopy, -40);
                            break;
                        case ImageProcess.ColorBrightness:
                            ImageColorBrightness(ref imageCopy, -80);
                            break;
                        case ImageProcess.GaussianBlur:
                            ImageBlurGaussian(ref imageCopy, 10);
                            break;
                        case ImageProcess.FlipVertical:
                            ImageFlipVertical(ref imageCopy);
                            break;
                        case ImageProcess.FlipHorizontal:
                            ImageFlipHorizontal(ref imageCopy);
                            break;
                        default:
                            break;
                    }

                    // Get pixel data from image (RGBA 32bit)
                    Color* pixels = LoadImageColors(imageCopy);
                    UpdateTexture(texture, pixels);
                    UnloadImageColors(pixels);

                    textureReload = false;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                DrawText("IMAGE PROCESSING:", 40, 30, 10, Color.DARKGRAY);

                // Draw rectangles
                for (int i = 0; i < NumProcesses; i++)
                {
                    DrawRectangleRec(toggleRecs[i], (i == (int)currentProcess) ? Color.SKYBLUE : Color.LIGHTGRAY);
                    DrawRectangleLines(
                        (int)toggleRecs[i].x,
                        (int)toggleRecs[i].y,
                        (int)toggleRecs[i].width,
                        (int)toggleRecs[i].height,
                        (i == (int)currentProcess) ? Color.BLUE : Color.GRAY
                    );

                    int labelX = (int)(toggleRecs[i].x + toggleRecs[i].width / 2);
                    DrawText(
                        processText[i],
                        (int)(labelX - MeasureText(processText[i], 10) / 2),
                        (int)toggleRecs[i].y + 11,
                        10,
                        (i == (int)currentProcess) ? Color.DARKBLUE : Color.DARKGRAY
                    );
                }

                int x = screenWidth - texture.width - 60;
                int y = screenHeight / 2 - texture.height / 2;
                DrawTexture(texture, x, y, Color.WHITE);
                DrawRectangleLines(x, y, texture.width, texture.height, Color.BLACK);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(texture);
            UnloadImage(imageOrigin);
            UnloadImage(imageCopy);

            CloseWindow();                // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
