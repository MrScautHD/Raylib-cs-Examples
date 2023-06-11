/*******************************************************************************************
*
*   raylib [textures] example - Draw part of the texture tiled
*
*   This example has been created using raylib 3.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2020 Vlad Adrian (@demizdor) and Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Textures
{
    public class DrawTiled
    {
        const int OptWidth = 220;
        const int MarginSize = 8;
        const int ColorSize = 16;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            int screenWidth = 800;
            int screenHeight = 450;

            SetConfigFlags(ConfigFlags.FLAG_WINDOW_RESIZABLE);
            InitWindow(screenWidth, screenHeight, "raylib [textures] example - Draw part of a texture tiled");

            // NOTE: Textures MUST be loaded after Window initialization (OpenGL context is required)
            Texture2D texPattern = LoadTexture("resources/patterns.png");

            // Makes the texture smoother when upscaled
            SetTextureFilter(texPattern, TextureFilter.TEXTURE_FILTER_TRILINEAR);

            // Coordinates for all patterns inside the texture
            Rectangle[] recPattern = new[] {
                new Rectangle(3, 3, 66, 66),
                new Rectangle(75, 3, 100, 100),
                new Rectangle(3, 75, 66, 66),
                new Rectangle(7, 156, 50, 50),
                new Rectangle(85, 106, 90, 45),
                new Rectangle(75, 154, 100, 60)
            };

            // Setup colors
            Color[] colors = new[]
            {
                Color.BLACK,
                Color.MAROON,
                Color.ORANGE,
                Color.BLUE,
                Color.PURPLE,
                Color.BEIGE,
                Color.LIME,
                Color.RED,
                Color.DARKGRAY,
                Color.SKYBLUE
            };
            Rectangle[] colorRec = new Rectangle[colors.Length];

            // Calculate rectangle for each color
            for (int i = 0, x = 0, y = 0; i < colors.Length; i++)
            {
                colorRec[i].x = 2 + MarginSize + x;
                colorRec[i].y = 22 + 256 + MarginSize + y;
                colorRec[i].width = ColorSize * 2;
                colorRec[i].height = ColorSize;

                if (i == (colors.Length / 2 - 1))
                {
                    x = 0;
                    y += ColorSize + MarginSize;
                }
                else
                {
                    x += (ColorSize * 2 + MarginSize);
                }
            }

            int activePattern = 0, activeCol = 0;
            float scale = 1.0f, rotation = 0.0f;

            SetTargetFPS(60);
            //---------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                screenWidth = GetScreenWidth();
                screenHeight = GetScreenHeight();

                // Handle mouse
                if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    Vector2 mouse = GetMousePosition();

                    // Check which pattern was clicked and set it as the active pattern
                    for (int i = 0; i < recPattern.Length; i++)
                    {
                        Rectangle rec = new Rectangle(
                            2 + MarginSize + recPattern[i].x,
                            40 + MarginSize + recPattern[i].y,
                            recPattern[i].width,
                            recPattern[i].height
                        );
                        if (CheckCollisionPointRec(mouse, rec))
                        {
                            activePattern = i;
                            break;
                        }
                    }

                    // Check to see which color was clicked and set it as the active color
                    for (int i = 0; i < colors.Length; ++i)
                    {
                        if (CheckCollisionPointRec(mouse, colorRec[i]))
                        {
                            activeCol = i;
                            break;
                        }
                    }
                }

                // Handle keys

                // Change scale
                if (IsKeyPressed(KeyboardKey.KEY_UP))
                {
                    scale += 0.25f;
                }
                if (IsKeyPressed(KeyboardKey.KEY_DOWN))
                {
                    scale -= 0.25f;
                }
                if (scale > 10.0f)
                {
                    scale = 10.0f;
                }
                else if (scale <= 0.0f)
                {
                    scale = 0.25f;
                }

                // Change rotation
                if (IsKeyPressed(KeyboardKey.KEY_LEFT))
                {
                    rotation -= 25.0f;
                }
                if (IsKeyPressed(KeyboardKey.KEY_RIGHT))
                {
                    rotation += 25.0f;
                }

                // Reset
                if (IsKeyPressed(KeyboardKey.KEY_SPACE))
                {
                    rotation = 0.0f;
                    scale = 1.0f;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                // Draw the tiled area
                Rectangle source = recPattern[activePattern];
                Rectangle dest = new Rectangle(
                    OptWidth + MarginSize,
                    MarginSize,
                    screenWidth - OptWidth - 2 * MarginSize,
                    screenHeight - 2 * MarginSize
                );
                DrawTextureTiled(texPattern, source, dest, Vector2.Zero, rotation, scale, colors[activeCol]);

                // Draw options
                Color color = ColorAlpha(Color.LIGHTGRAY, 0.5f);
                DrawRectangle(MarginSize, MarginSize, OptWidth - MarginSize, screenHeight - 2 * MarginSize, color);

                DrawText("Select Pattern", 2 + MarginSize, 30 + MarginSize, 10, Color.BLACK);
                DrawTexture(texPattern, 2 + MarginSize, 40 + MarginSize, Color.BLACK);
                DrawRectangle(
                    2 + MarginSize + (int)recPattern[activePattern].x,
                    40 + MarginSize + (int)recPattern[activePattern].y,
                    (int)recPattern[activePattern].width,
                    (int)recPattern[activePattern].height,
                    ColorAlpha(Color.DARKBLUE, 0.3f)
                );

                DrawText("Select Color", 2 + MarginSize, 10 + 256 + MarginSize, 10, Color.BLACK);
                for (int i = 0; i < colors.Length; i++)
                {
                    DrawRectangleRec(colorRec[i], colors[i]);
                    if (activeCol == i)
                    {
                        DrawRectangleLinesEx(colorRec[i], 3, ColorAlpha(Color.WHITE, 0.5f));
                    }
                }

                DrawText("Scale (UP/DOWN to change)", 2 + MarginSize, 80 + 256 + MarginSize, 10, Color.BLACK);
                DrawText($"{scale}x", 2 + MarginSize, 92 + 256 + MarginSize, 20, Color.BLACK);

                DrawText("Rotation (LEFT/RIGHT to change)", 2 + MarginSize, 122 + 256 + MarginSize, 10, Color.BLACK);
                DrawText($"{rotation} degrees", 2 + MarginSize, 134 + 256 + MarginSize, 20, Color.BLACK);

                DrawText("Press [SPACE] to reset", 2 + MarginSize, 164 + 256 + MarginSize, 10, Color.DARKBLUE);

                // Draw FPS
                DrawText($"{GetFPS()}", 2 + MarginSize, 2 + MarginSize, 20, Color.BLACK);
                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(texPattern);

            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }

        // Draw part of a texture (defined by a rectangle) with rotation and scale tiled into dest.
        static void DrawTextureTiled(
            Texture2D texture,
            Rectangle source,
            Rectangle dest,
            Vector2 origin,
            float rotation,
            float scale,
            Color tint
        )
        {
            if ((texture.id <= 0) || (scale <= 0.0f))
            {
                // Wanna see a infinite loop?!...just delete this line!
                return;
            }

            if ((source.width == 0) || (source.height == 0))
            {
                return;
            }

            int tileWidth = (int)(source.width * scale), tileHeight = (int)(source.height * scale);
            if ((dest.width < tileWidth) && (dest.height < tileHeight))
            {
                // Can fit only one tile
                DrawTexturePro(
                    texture,
                    new Rectangle(
                        source.x,
                        source.y,
                        ((float)dest.width / tileWidth) * source.width,
                        ((float)dest.height / tileHeight) * source.height
                    ),
                    new Rectangle(dest.x, dest.y, dest.width, dest.height), origin, rotation, tint
                );
            }
            else if (dest.width <= tileWidth)
            {
                // Tiled vertically (one column)
                int dy = 0;
                for (; dy + tileHeight < dest.height; dy += tileHeight)
                {
                    DrawTexturePro(
                        texture,
                        new Rectangle(
                            source.x,
                            source.y,
                            ((float)dest.width / tileWidth) * source.width,
                            source.height
                        ),
                        new Rectangle(dest.x, dest.y + dy, dest.width, (float)tileHeight),
                        origin,
                        rotation,
                        tint
                    );
                }

                // Fit last tile
                if (dy < dest.height)
                {
                    DrawTexturePro(
                        texture,
                        new Rectangle(
                            source.x,
                            source.y,
                            ((float)dest.width / tileWidth) * source.width,
                            ((float)(dest.height - dy) / tileHeight) * source.height
                        ),
                        new Rectangle(dest.x, dest.y + dy, dest.width, dest.height - dy),
                        origin,
                        rotation,
                        tint
                    );
                }
            }
            else if (dest.height <= tileHeight)
            {
                // Tiled horizontally (one row)
                int dx = 0;
                for (; dx + tileWidth < dest.width; dx += tileWidth)
                {
                    DrawTexturePro(
                        texture,
                        new Rectangle(
                            source.x,
                            source.y,
                            source.width,
                            ((float)dest.height / tileHeight) * source.height
                        ),
                        new Rectangle(dest.x + dx, dest.y, (float)tileWidth, dest.height),
                        origin,
                        rotation,
                        tint
                    );
                }

                // Fit last tile
                if (dx < dest.width)
                {
                    DrawTexturePro(
                        texture,
                        new Rectangle(
                            source.x,
                            source.y,
                            ((float)(dest.width - dx) / tileWidth) * source.width,
                            ((float)dest.height / tileHeight) * source.height
                        ),
                        new Rectangle(
                            dest.x + dx,
                            dest.y,
                            dest.width - dx,
                            dest.height
                        ),
                        origin,
                        rotation,
                        tint
                    );
                }
            }
            else
            {
                // Tiled both horizontally and vertically (rows and columns)
                int dx = 0;
                for (; dx + tileWidth < dest.width; dx += tileWidth)
                {
                    int dy = 0;
                    for (; dy + tileHeight < dest.height; dy += tileHeight)
                    {
                        DrawTexturePro(
                            texture,
                            source,
                            new Rectangle(
                                dest.x + dx,
                                dest.y + dy,
                                (float)tileWidth,
                                (float)tileHeight
                            ),
                            origin,
                            rotation,
                            tint
                        );
                    }

                    if (dy < dest.height)
                    {
                        DrawTexturePro(
                            texture,
                            new Rectangle(
                                source.x,
                                source.y,
                                source.width,
                                ((float)(dest.height - dy) / tileHeight) * source.height
                            ),
                            new Rectangle(
                                dest.x + dx,
                                dest.y + dy,
                                (float)tileWidth, dest.height - dy
                            ),
                            origin,
                            rotation,
                            tint
                        );
                    }
                }

                // Fit last column of tiles
                if (dx < dest.width)
                {
                    int dy = 0;
                    for (; dy + tileHeight < dest.height; dy += tileHeight)
                    {
                        DrawTexturePro(
                            texture,
                            new Rectangle(
                                source.x,
                                source.y,
                                ((float)(dest.width - dx) / tileWidth) * source.width,
                                source.height
                            ),
                            new Rectangle(
                                dest.x + dx,
                                dest.y + dy,
                                dest.width - dx,
                                (float)tileHeight
                            ),
                            origin,
                            rotation,
                            tint
                        );
                    }

                    // Draw final tile in the bottom right corner
                    if (dy < dest.height)
                    {
                        DrawTexturePro(
                            texture,
                            new Rectangle(
                                source.x,
                                source.y,
                                ((float)(dest.width - dx) / tileWidth) * source.width,
                                ((float)(dest.height - dy) / tileHeight) * source.height
                            ),
                            new Rectangle(
                                dest.x + dx,
                                dest.y + dy,
                                dest.width - dx,
                                dest.height - dy
                            ),
                            origin,
                            rotation,
                            tint
                        );
                    }
                }
            }
        }
    }
}
