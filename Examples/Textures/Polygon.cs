/*******************************************************************************************
*
*   raylib [shapes] example - Draw Textured Polygon
*
*   This example has been created using raylib 99.98 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2014 Ramon Santamaria (@raysan5)
*   Copyright (c) 2021 Chris Camacho (codifies - bedroomcoders.co.uk)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Rlgl;

namespace Examples.Textures
{
    public class Polygon
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [textures] example - Textured Polygon");

            Vector2[] texcoords = new[] {
                new Vector2(0.75f, 0),
                new Vector2(0.25f, 0),
                new Vector2(0, 0.5f),
                new Vector2(0, 0.75f),
                new Vector2(0.25f, 1),
                new Vector2(0.375f, 0.875f),
                new Vector2(0.625f, 0.875f),
                new Vector2(0.75f, 1),
                new Vector2(1, 0.75f),
                new Vector2(1, 0.5f),
                // Close the poly
                new Vector2(0.75f, 0)
            };

            Vector2[] points = new Vector2[11];

            // Define the base poly vertices from the UV's
            // NOTE: They can be specified in any other way
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X = (texcoords[i].X - 0.5f) * 256.0f;
                points[i].Y = (texcoords[i].Y - 0.5f) * 256.0f;
            }

            // Define the vertices drawing position
            // NOTE: Initially same as points but updated every frame
            Vector2[] positions = new Vector2[points.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = points[i];
            }

            Texture2D texture = LoadTexture("resources/cat.png");
            float angle = 0;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                // Update your variables here
                //----------------------------------------------------------------------------------
                angle += 1;
                for (int i = 0; i < positions.Length; i++)
                {
                    positions[i] = Raymath.Vector2Rotate(points[i], angle * Raylib.DEG2RAD);
                }

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                DrawText("Textured Polygon", 20, 20, 20, Color.DARKGRAY);
                Vector2 center = new Vector2(screenWidth / 2, screenHeight / 2);
                DrawTexturePoly(texture, center, positions, texcoords, positions.Length, Color.WHITE);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            UnloadTexture(texture);

            // De-Initialization
            //--------------------------------------------------------------------------------------
            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }

        // Draw textured polygon, defined by vertex and texture coordinates
        // NOTE: Polygon center must have straight line path to all points
        // without crossing perimeter, points must be in anticlockwise order
        static void DrawTexturePoly(
            Texture2D texture,
            Vector2 center,
            Vector2[] points,
            Vector2[] texcoords,
            int pointCount,
            Color tint
        )
        {
            rlSetTexture(texture.id);

            // Texturing is only supported on RL_QUADS
            rlBegin(DrawMode.QUADS);

            rlColor4ub(tint.r, tint.g, tint.b, tint.a);

            for (int i = 0; i < pointCount - 1; i++)
            {
                rlTexCoord2f(0.5f, 0.5f);
                rlVertex2f(center.X, center.Y);

                rlTexCoord2f(texcoords[i].X, texcoords[i].Y);
                rlVertex2f(points[i].X + center.X, points[i].Y + center.Y);

                rlTexCoord2f(texcoords[i + 1].X, texcoords[i + 1].Y);
                rlVertex2f(points[i + 1].X + center.X, points[i + 1].Y + center.Y);

                rlTexCoord2f(texcoords[i + 1].X, texcoords[i + 1].Y);
                rlVertex2f(points[i + 1].X + center.X, points[i + 1].Y + center.Y);
            }
            rlEnd();

            rlSetTexture(0);
        }
    }
}
