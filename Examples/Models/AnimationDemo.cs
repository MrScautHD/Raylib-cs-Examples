/*******************************************************************************************
*
*   raylib [models] example - Load 3d model with animations and play them
*
*   This example has been created using raylib 2.5 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Example contributed by Culacant (@culacant) and reviewed by Ramon Santamaria (@raysan5)
*
*   Copyright (c) 2019 Culacant (@culacant) and Ramon Santamaria (@raysan5)
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

namespace Examples.Models
{
    public class AnimationDemo
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [models] example - model animation");

            // Define the camera to look into our 3d world
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(10.0f, 10.0f, 10.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            Model model = LoadModel("resources/models/iqm/guy.iqm");
            Texture2D texture = LoadTexture("resources/models/iqm/guytex.png");
            Raylib.SetMaterialTexture(ref model, 0, MaterialMapIndex.MATERIAL_MAP_ALBEDO, ref texture);

            Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
            // Load animation data
            uint animsCount = 0;
            var anims = LoadModelAnimations("resources/models/iqm/guyanim.iqm", ref animsCount);
            int animFrameCounter = 0;

            SetTargetFPS(60);                   // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera, CameraMode.CAMERA_FREE);

                // Play animation when spacebar is held down
                if (IsKeyDown(KeyboardKey.KEY_SPACE))
                {
                    animFrameCounter++;
                    UpdateModelAnimation(model, anims[0], animFrameCounter);
                    if (animFrameCounter >= anims[0].frameCount)
                    {
                        animFrameCounter = 0;
                    }
                }
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                BeginMode3D(camera);

                DrawModelEx(
                    model,
                    position,
                    new Vector3(1.0f, 0.0f, 0.0f),
                    -90.0f,
                    new Vector3(1.0f, 1.0f, 1.0f),
                    Color.WHITE
                );

                for (int i = 0; i < model.boneCount; i++)
                {
                    var framePoses = anims[0].FramePoses;
                    DrawCube(framePoses[animFrameCounter][i].translation, 0.2f, 0.2f, 0.2f, Color.RED);
                }

                DrawGrid(10, 1.0f);

                EndMode3D();

                DrawText("PRESS SPACE to PLAY MODEL ANIMATION", 10, 10, 20, Color.MAROON);
                DrawText("(c) Guy IQM 3D model by @culacant", screenWidth - 200, screenHeight - 20, 10, Color.GRAY);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadTexture(texture);

            for (int i = 0; i < animsCount; i++)
            {
                UnloadModelAnimation(anims[i]);
            }

            UnloadModel(model);

            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
