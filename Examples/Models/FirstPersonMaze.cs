/*******************************************************************************************
*
*   raylib [models] example - first person maze
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2019 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Models
{
    public class FirstPersonMaze
    {
        public unsafe static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - first person maze");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(0.2f, 0.4f, 0.2f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            Image imMap = LoadImage("resources/cubicmap.png");
            Texture2D cubicmap = LoadTextureFromImage(imMap);
            Mesh mesh = GenMeshCubicmap(imMap, new Vector3(1.0f, 1.0f, 1.0f));
            Model model = LoadModelFromMesh(mesh);

            // NOTE: By default each cube is mapped to one part of texture atlas
            Texture2D texture = LoadTexture("resources/cubicmap_atlas.png");

            // Set map diffuse texture
            Raylib.SetMaterialTexture(ref model, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref texture);

            // Get map image data to be used for collision detection
            Color* mapPixels = LoadImageColors(imMap);
            UnloadImage(imMap);

            Vector3 mapPosition = new Vector3(-16.0f, 0.0f, -8.0f);
            Vector3 playerPosition = camera.position;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                Vector3 oldCamPos = camera.position;

                UpdateCamera(ref camera, CameraMode.CAMERA_FIRST_PERSON);

                // Check player collision (we simplify to 2D collision detection)
                Vector2 playerPos = new Vector2(camera.position.X, camera.position.Z);

                // Collision radius (player is modelled as a cilinder for collision)
                float playerRadius = 0.1f;

                int playerCellX = (int)(playerPos.X - mapPosition.X + 0.5f);
                int playerCellY = (int)(playerPos.Y - mapPosition.Z + 0.5f);

                // Out-of-limits security check
                if (playerCellX < 0)
                {
                    playerCellX = 0;
                }
                else if (playerCellX >= cubicmap.width)
                {
                    playerCellX = cubicmap.width - 1;
                }

                if (playerCellY < 0)
                {
                    playerCellY = 0;
                }
                else if (playerCellY >= cubicmap.height)
                {
                    playerCellY = cubicmap.height - 1;
                }

                // Check map collisions using image data and player position
                // TODO: Improvement: Just check player surrounding cells for collision
                for (int y = 0; y < cubicmap.height; y++)
                {
                    for (int x = 0; x < cubicmap.width; x++)
                    {
                        Color* mapPixelsData = mapPixels;

                        // Collision: Color.white pixel, only check R channel
                        Rectangle rec = new Rectangle(
                            mapPosition.X - 0.5f + x * 1.0f,
                            mapPosition.Z - 0.5f + y * 1.0f,
                            1.0f,
                            1.0f
                        );

                        bool collision = CheckCollisionCircleRec(playerPos, playerRadius, rec);
                        if ((mapPixelsData[y * cubicmap.width + x].r == 255) && collision)
                        {
                            // Collision detected, reset camera position
                            camera.position = oldCamPos;
                        }
                    }
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                // Draw maze map
                BeginMode3D(camera);
                DrawModel(model, mapPosition, 1.0f, Color.WHITE);
                EndMode3D();

                DrawTextureEx(cubicmap, new Vector2(GetScreenWidth() - cubicmap.width * 4 - 20, 20), 0.0f, 4.0f, Color.WHITE);
                DrawRectangleLines(GetScreenWidth() - cubicmap.width * 4 - 20, 20, cubicmap.width * 4, cubicmap.height * 4, Color.GREEN);

                // Draw player position radar
                DrawRectangle(GetScreenWidth() - cubicmap.width * 4 - 20 + playerCellX * 4, 20 + playerCellY * 4, 4, 4, Color.RED);

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadImageColors(mapPixels);

            UnloadTexture(cubicmap);
            UnloadTexture(texture);
            UnloadModel(model);

            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
