/*******************************************************************************************
*
*   raylib [models] example - Load 3d gltf model
*
*   This example has been created using raylib 3.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Hristo Stamenov (@object71) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2021 Hristo Stamenov (@object71) and Ramon Santamaria (@raysan5)
*
********************************************************************************************
*
* To export a model from blender, make sure it is not posed, the vertices need to be in the
* same position as they would be in edit mode.
* and that the scale of your models is set to 0. Scaling can be done from the export menu.
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using static Raylib_cs.KeyboardKey;

namespace Examples.Models
{
    public class LoadingGltf
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - model animation");

            // Define the camera to look into our 3d world
            Camera3D camera;
            camera.position = new Vector3(5.0f, 5.0f, 5.0f);
            camera.target = new Vector3(0.0f, 2.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            // Load gltf model
            Model model = LoadModel("resources/models/gltf/robot.glb");

            // Load gltf model animations
            uint animsCount = 0;
            uint animIndex = 0;
            uint animCurrentFrame = 0;
            var modelAnimations = LoadModelAnimations("resources/models/gltf/robot.glb", ref animsCount);

            // Set model position
            Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);

            // Limit cursor to relative movement inside the window
            DisableCursor();

            SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())        // Detect window close button or ESC key
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera, CameraMode.CAMERA_FREE);

                // Select current animation
                if (IsKeyReleased(KEY_UP))
                {
                    animIndex = (animIndex + 1) % animsCount;
                }

                if (IsKeyReleased(KEY_DOWN))
                {
                    animIndex = (animIndex + animsCount - 1) % animsCount;
                }

                // Update model animation
                ModelAnimation anim = modelAnimations[(int)animIndex];
                animCurrentFrame = (animCurrentFrame + 1) % (uint)anim.frameCount;
                UpdateModelAnimation(model, anim, (int)animCurrentFrame);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(SKYBLUE);

                BeginMode3D(camera);

                DrawModel(model, position, 1.0f, WHITE);
                DrawGrid(10, 1.0f);

                EndMode3D();

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------

            // Unload model and meshes/material
            UnloadModel(model);

            CloseWindow();              // Close window and OpenGL context
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
