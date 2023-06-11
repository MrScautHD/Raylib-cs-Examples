/*******************************************************************************************
*
*   raylib [models] example - Heightmap loading and drawing
*
*   This example has been created using raylib 1.8 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Models
{
    public class HeightmapDemo
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - heightmap loading and drawing");

            // Define our custom camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(18.0f, 16.0f, 18.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            Image image = LoadImage("resources/heightmap.png");
            Texture2D texture = LoadTextureFromImage(image);

            Mesh mesh = GenMeshHeightmap(image, new Vector3(16, 8, 16));
            Model model = LoadModelFromMesh(mesh);

            // Set map diffuse texture
            Raylib.SetMaterialTexture(ref model, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref texture);

            Vector3 mapPosition = new Vector3(-8.0f, 0.0f, -8.0f);

            UnloadImage(image);

            SetTargetFPS(60);                       // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera, CameraMode.CAMERA_ORBITAL);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                BeginMode3D(camera);

                DrawModel(model, mapPosition, 1.0f, Color.RED);

                DrawGrid(20, 1.0f);

                EndMode3D();

                DrawTexture(texture, screenWidth - texture.width - 20, 20, Color.WHITE);
                DrawRectangleLines(screenWidth - texture.width - 20, 20, texture.width, texture.height, Color.GREEN);

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(texture);
            UnloadModel(model);

            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
