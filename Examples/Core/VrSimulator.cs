/*******************************************************************************************
*
*   raylib [core] example - VR Simulator (Oculus Rift CV1 parameters)
*
*   This example has been created using raylib 1.7 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2017 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Examples.Core
{
    public class VrSimulator
    {
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 1080;
            const int screenHeight = 600;

            SetConfigFlags(ConfigFlags.FLAG_MSAA_4X_HINT);
            InitWindow(screenWidth, screenHeight, "raylib [core] example - vr simulator");

            // VR device parameters definition
            VrDeviceInfo device = new VrDeviceInfo
            {
                // Oculus Rift CV1 parameters for simulator
                hResolution = 2160,
                vResolution = 1200,
                hScreenSize = 0.133793f,
                vScreenSize = 0.0669f,
                vScreenCenter = 0.04678f,
                eyeToScreenDistance = 0.041f,
                lensSeparationDistance = 0.07f,
                interpupillaryDistance = 0.07f,
            };

            // NOTE: CV1 uses a Fresnel-hybrid-asymmetric lenses with specific distortion compute shaders.
            // Following parameters are an approximation to distortion stereo rendering but results differ from actual
            // device.
            unsafe
            {
                device.lensDistortionValues[0] = 1.0f;
                device.lensDistortionValues[1] = 0.22f;
                device.lensDistortionValues[2] = 0.24f;
                device.lensDistortionValues[3] = 0.0f;
                device.chromaAbCorrection[0] = 0.996f;
                device.chromaAbCorrection[1] = -0.004f;
                device.chromaAbCorrection[2] = 1.014f;
                device.chromaAbCorrection[3] = 0.0f;
            }

            // Load VR stereo config for VR device parameteres (Oculus Rift CV1 parameters)
            VrStereoConfig config = LoadVrStereoConfig(device);

            // Distortion shader (uses device lens distortion and chroma)
            Shader distortion = LoadShader(null, "resources/distortion330.fs");

            // Update distortion shader with lens and distortion-scale parameters
            Raylib.SetShaderValue(
                distortion,
                GetShaderLocation(distortion, "leftLensCenter"),
                config.leftLensCenter,
                ShaderUniformDataType.SHADER_UNIFORM_VEC2
            );
            Raylib.SetShaderValue(
                distortion,
                GetShaderLocation(distortion, "rightLensCenter"),
                config.rightLensCenter,
                ShaderUniformDataType.SHADER_UNIFORM_VEC2
            );
            Raylib.SetShaderValue(
                distortion,
                GetShaderLocation(distortion, "leftScreenCenter"),
                config.leftScreenCenter,
                ShaderUniformDataType.SHADER_UNIFORM_VEC2
            );
            Raylib.SetShaderValue(
                distortion,
                GetShaderLocation(distortion, "rightScreenCenter"),
                config.rightScreenCenter,
                ShaderUniformDataType.SHADER_UNIFORM_VEC2
            );

            Raylib.SetShaderValue(
                distortion,
                GetShaderLocation(distortion, "scale"),
                config.scale,
                ShaderUniformDataType.SHADER_UNIFORM_VEC2
            );
            Raylib.SetShaderValue(
                distortion,
                GetShaderLocation(distortion, "scaleIn"),
                config.scaleIn,
                ShaderUniformDataType.SHADER_UNIFORM_VEC2
            );

            unsafe
            {
                SetShaderValue(
                    distortion,
                    GetShaderLocation(distortion, "deviceWarpParam"),
                    device.lensDistortionValues,
                    ShaderUniformDataType.SHADER_UNIFORM_VEC4
                );
                SetShaderValue(
                    distortion,
                    GetShaderLocation(distortion, "chromaAbParam"),
                    device.chromaAbCorrection,
                    ShaderUniformDataType.SHADER_UNIFORM_VEC4
                );
            }

            // Initialize framebuffer for stereo rendering
            // NOTE: Screen size should match HMD aspect ratio
            RenderTexture2D target = LoadRenderTexture(GetScreenWidth(), GetScreenHeight());

            // Define the camera to look into our 3d world
            Camera3D camera;
            camera.position = new Vector3(5.0f, 2.0f, 5.0f);
            camera.target = new Vector3(0.0f, 2.0f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 60.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            Vector3 cubePosition = new Vector3(0.0f, 0.0f, 0.0f);

            SetTargetFPS(90);                   // Set our game to run at 90 frames-per-second
            //--------------------------------------------------------------------------------------

            // Main game loop
            while (!WindowShouldClose())
            {
                // Update
                //----------------------------------------------------------------------------------
                UpdateCamera(ref camera, CameraMode.CAMERA_FIRST_PERSON);
                //----------------------------------------------------------------------------------

                // Draw
                //----------------------------------------------------------------------------------
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                BeginTextureMode(target);
                ClearBackground(Color.RAYWHITE);

                BeginVrStereoMode(config);
                BeginMode3D(camera);

                DrawCube(cubePosition, 2.0f, 2.0f, 2.0f, Color.RED);
                DrawCubeWires(cubePosition, 2.0f, 2.0f, 2.0f, Color.MAROON);
                DrawGrid(40, 1.0f);

                EndMode3D();
                EndVrStereoMode();
                EndTextureMode();

                BeginShaderMode(distortion);
                DrawTextureRec(
                    target.texture,
                    new Rectangle(0, 0, (float)target.texture.width, (float)-target.texture.height),
                    new Vector2(0.0f, 0.0f),
                    Color.WHITE
                );
                EndShaderMode();

                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadVrStereoConfig(config);
            UnloadRenderTexture(target);
            UnloadShader(distortion);

            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }
    }
}
