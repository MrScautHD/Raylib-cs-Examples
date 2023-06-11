/*******************************************************************************************
*
*   raylib [core] example - 2d camera
*
*   This example has been created using raylib 1.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2016 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Core
{
    public class Camera2dDemo
    {
        public const int MaxBuildings = 100;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [core] example - 2d camera");

            Rectangle player = new Rectangle(400, 280, 40, 40);
            Rectangle[] buildings = new Rectangle[MaxBuildings];
            Color[] buildColors = new Color[MaxBuildings];

            int spacing = 0;

            for (int i = 0; i < MaxBuildings; i++)
            {
                buildings[i].width = GetRandomValue(50, 200);
                buildings[i].height = GetRandomValue(100, 800);
                buildings[i].y = screenHeight - 130 - buildings[i].height;
                buildings[i].x = -6000 + spacing;

                spacing += (int)buildings[i].width;

                buildColors[i] = new Color(
                    GetRandomValue(200, 240),
                    GetRandomValue(200, 240),
                    GetRandomValue(200, 250),
                    255
                );
            }

            Camera2D camera = new Camera2D();
            camera.target = new Vector2(player.x + 20, player.y + 20);
            camera.offset = new Vector2(screenWidth / 2, screenHeight / 2);
            camera.rotation = 0.0f;
            camera.zoom = 1.0f;

            SetTargetFPS(60);
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------

                // Player movement
                if (IsKeyDown(KeyboardKey.KEY_RIGHT))
                {
                    player.x += 2;
                }
                else if (IsKeyDown(KeyboardKey.KEY_LEFT))
                {
                    player.x -= 2;
                }

                // Camera3D target follows player
                camera.target = new Vector2(player.x + 20, player.y + 20);

                // Camera3D rotation controls
                if (IsKeyDown(KeyboardKey.KEY_A))
                {
                    camera.rotation--;
                }
                else if (IsKeyDown(KeyboardKey.KEY_S))
                {
                    camera.rotation++;
                }

                // Limit camera rotation to 80 degrees (-40 to 40)
                if (camera.rotation > 40)
                {
                    camera.rotation = 40;
                }
                else if (camera.rotation < -40)
                {
                    camera.rotation = -40;
                }

                // Camera3D zoom controls
                camera.zoom += ((float)GetMouseWheelMove() * 0.05f);

                if (camera.zoom > 3.0f)
                {
                    camera.zoom = 3.0f;
                }
                else if (camera.zoom < 0.1f)
                {
                    camera.zoom = 0.1f;
                }

                // Camera3D reset (zoom and rotation)
                if (IsKeyPressed(KeyboardKey.KEY_R))
                {
                    camera.zoom = 1.0f;
                    camera.rotation = 0.0f;
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                BeginMode2D(camera);

                DrawRectangle(-6000, 320, 13000, 8000, Color.DARKGRAY);

                for (int i = 0; i < MaxBuildings; i++)
                {
                    DrawRectangleRec(buildings[i], buildColors[i]);
                }

                DrawRectangleRec(player, Color.RED);

                DrawRectangle((int)camera.target.X, -500, 1, (int)(screenHeight * 4), Color.GREEN);
                DrawLine(
                    (int)(-screenWidth * 10),
                    (int)camera.target.Y,
                    (int)(screenWidth * 10),
                    (int)camera.target.Y,
                    Color.GREEN
                );

                EndMode2D();

                DrawText("SCREEN AREA", 640, 10, 20, Color.RED);

                DrawRectangle(0, 0, (int)screenWidth, 5, Color.RED);
                DrawRectangle(0, 5, 5, (int)screenHeight - 10, Color.RED);
                DrawRectangle((int)screenWidth - 5, 5, 5, (int)screenHeight - 10, Color.RED);
                DrawRectangle(0, (int)screenHeight - 5, (int)screenWidth, 5, Color.RED);

                DrawRectangle(10, 10, 250, 113, ColorAlpha(Color.SKYBLUE, 0.5f));
                DrawRectangleLines(10, 10, 250, 113, Color.BLUE);

                DrawText("Free 2d camera controls:", 20, 20, 10, Color.BLACK);
                DrawText("- Right/Left to move Offset", 40, 40, 10, Color.DARKGRAY);
                DrawText("- Mouse Wheel to Zoom in-out", 40, 60, 10, Color.DARKGRAY);
                DrawText("- A / S to Rotate", 40, 80, 10, Color.DARKGRAY);
                DrawText("- R to reset Zoom and Rotation", 40, 100, 10, Color.DARKGRAY);

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
