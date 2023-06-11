/*******************************************************************************************
*
*   raylib [models] example - Cubicmap loading and drawing
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
    public class CubicmapDemo
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - cubesmap loading and drawing");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(16.0f, 14.0f, 16.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            Image image = LoadImage("resources/cubicmap.png");
            Texture2D cubicmap = LoadTextureFromImage(image);

            Mesh mesh = GenMeshCubicmap(image, new Vector3(1.0f, 1.0f, 1.0f));
            Model model = LoadModelFromMesh(mesh);

            // NOTE: By default each cube is mapped to one part of texture atlas
            Texture2D texture = LoadTexture("resources/cubicmap_atlas.png");

            // Set map diffuse texture
            Raylib.SetMaterialTexture(ref model, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref texture);

            Vector3 mapPosition = new Vector3(-16.0f, 0.0f, -8.0f);
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

                DrawModel(model, mapPosition, 1.0f, Color.WHITE);

                EndMode3D();

                Vector2 position = new Vector2(screenWidth - cubicmap.width * 4 - 20, 20);
                DrawTextureEx(cubicmap, position, 0.0f, 4.0f, Color.WHITE);
                DrawRectangleLines(
                    screenWidth - cubicmap.width * 4 - 20,
                    20,
                    cubicmap.width * 4,
                    cubicmap.height * 4,
                    Color.GREEN
                );

                DrawText("cubicmap image used to", 658, 90, 10, Color.GRAY);
                DrawText("generate map 3d model", 658, 104, 10, Color.GRAY);

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(cubicmap);
            UnloadTexture(texture);
            UnloadModel(model);

            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
