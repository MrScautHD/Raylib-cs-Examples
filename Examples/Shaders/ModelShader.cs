/*******************************************************************************************
*
*   raylib [shaders] example - Apply a shader to a 3d model
*
*   NOTE: This example requires raylib OpenGL 3.3 or ES2 versions for shaders support,
*         OpenGL 1.1 does not support shaders, recompile raylib to OpenGL 3.3 version.
*
*   NOTE: Shaders used in this example are #version 330 (OpenGL 3.3), to test this example
*         on OpenGL ES 2.0 platforms (Android, Raspberry Pi, HTML5), use #version 100 shaders
*         raylib comes with shaders ready for both versions, check raylib/shaders install folder
*
*   This example has been created using raylib 1.3 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2014 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Shaders
{
    public class ModelShader
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            // Enable Multi Sampling Anti Aliasing 4x (if available)
            SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);

            InitWindow(screenWidth, screenHeight, "raylib [shaders] example - model shader");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(4.0f, 4.0f, 4.0f);
            camera.target = new Vector3(0.0f, 1.0f, -1.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            Model model = LoadModel("resources/models/watermill.obj");
            Texture2D texture = LoadTexture("resources/models/watermill_diffuse.png");
            Shader shader = LoadShader("resources/shaders/glsl330/base.vs",
                                       "resources/shaders/glsl330/grayscale.fs");

            Raylib.SetMaterialShader(ref model, 0, ref shader);
            Raylib.SetMaterialTexture(ref model, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref texture);

            Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);

            SetTargetFPS(60);                           // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera, CameraMode.CAMERA_FREE);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                BeginMode3D(camera);

                DrawModel(model, position, 0.2f, Color.WHITE);

                DrawGrid(10, 1.0f);

                EndMode3D();

                DrawText(
                    "(c) Watermill 3D model by Alberto Cano",
                    screenWidth - 210,
                    screenHeight - 20,
                    10,
                    Color.GRAY
                );

                DrawText($"Camera3D position: ({camera.position})", 600, 20, 10, Color.BLACK);
                DrawText($"Camera3D target: ({camera.position})", 600, 40, 10, Color.GRAY);

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadShader(shader);
            UnloadTexture(texture);
            UnloadModel(model);

            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
