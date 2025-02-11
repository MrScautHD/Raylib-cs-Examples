/*******************************************************************************************
*
*   raylib [shaders] example - Raymarching shapes generation
*
*   NOTE: This example requires raylib OpenGL 3.3 or ES2 versions for shaders support,
*         OpenGL 1.1 does not support shaders, recompile raylib to OpenGL 3.3 version.
*
*   NOTE: Shaders used in this example are #version 330 (OpenGL 3.3), to test this example
*         on OpenGL ES 2.0 platforms (Android, Raspberry Pi, HTML5), use #version 100 shaders
*         raylib comes with shaders ready for both versions, check raylib/shaders install folder
*
*   This example has been created using raylib 2.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2018 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.ConfigFlags;

namespace Examples.Shaders
{
    public class Raymarching
    {
        public const int GlslVersion = 330;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            int screenWidth = 800;
            int screenHeight = 450;

            SetConfigFlags(FLAG_WINDOW_RESIZABLE);
            InitWindow(screenWidth, screenHeight, "raylib [shaders] example - raymarching shapes");

            Camera3D camera = new Camera3D();
            camera.position = new Vector3(2.5f, 2.5f, 3.0f);
            camera.target = new Vector3(0.0f, 0.0f, 0.7f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 65.0f;

            // Load raymarching shader
            // NOTE: Defining 0 (NULL) for vertex shader forces usage of internal default vertex shader
            Shader shader = LoadShader(null, $"resources/shaders/glsl{GlslVersion}/raymarching.fs");

            // Get shader locations for required uniforms
            int viewEyeLoc = GetShaderLocation(shader, "viewEye");
            int viewCenterLoc = GetShaderLocation(shader, "viewCenter");
            int runTimeLoc = GetShaderLocation(shader, "runTime");
            int resolutionLoc = GetShaderLocation(shader, "resolution");

            float[] resolution = { (float)screenWidth, (float)screenHeight };
            Raylib.SetShaderValue(shader, resolutionLoc, resolution, ShaderUniformDataType.SHADER_UNIFORM_VEC2);

            float runTime = 0.0f;

            SetTargetFPS(60);                       // Set our game to run at 60 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Check if screen is resized
                //----------------------------------------------------------------------------------
                if (IsWindowResized())
                {
                    screenWidth = GetScreenWidth();
                    screenHeight = GetScreenHeight();
                    resolution = new float[] { (float)screenWidth, (float)screenHeight };
                    Raylib.SetShaderValue(shader, resolutionLoc, resolution, ShaderUniformDataType.SHADER_UNIFORM_VEC2);
                }

                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera, CameraMode.CAMERA_FREE);

                float deltaTime = GetFrameTime();
                runTime += deltaTime;

                // Set shader required uniform values
                Raylib.SetShaderValue(shader, viewEyeLoc, camera.position, ShaderUniformDataType.SHADER_UNIFORM_VEC3);
                Raylib.SetShaderValue(shader, viewCenterLoc, camera.target, ShaderUniformDataType.SHADER_UNIFORM_VEC3);
                Raylib.SetShaderValue(shader, runTimeLoc, runTime, ShaderUniformDataType.SHADER_UNIFORM_FLOAT);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                // We only draw a white full-screen rectangle,
                // frame is generated in shader using raymarching
                BeginShaderMode(shader);
                DrawRectangle(0, 0, screenWidth, screenHeight, Color.WHITE);
                EndShaderMode();

                DrawText(
                    "(c) Raymarching shader by Iñigo Quilez. MIT License.",
                    screenWidth - 280,
                    screenHeight - 20,
                    10,
                    Color.BLACK
                );

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadShader(shader);

            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
