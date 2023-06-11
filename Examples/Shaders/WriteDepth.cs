/*******************************************************************************************
*
*   raylib [shaders] example - Depth buffer writing
*
*   Example originally created with raylib 4.2, last time updated with raylib 4.2
*
*   Example contributed by Buğra Alptekin Sarı (@BugraAlptekinSari) and reviewed by Ramon Santamaria (@raysan5)
*
*   Example licensed under an unmodified zlib/libpng license, which is an OSI-certified,
*   BSD-like license that allows static linking with closed source software
*
*   Copyright (c) 2022-2023 Buğra Alptekin Sarı (@BugraAlptekinSari)
*
********************************************************************************************/

using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Rlgl;

namespace Examples.Shaders
{
    public class WriteDepth
    {
        const int GLSL_VERSION = 330;

        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
            const int screenWidth = 800;
            const int screenHeight = 450;

            InitWindow(screenWidth, screenHeight, "raylib [shaders] example - write depth buffer");

            // The shader inverts the depth buffer by writing into it by `gl_FragDepth = 1 - gl_FragCoord.z;`
            Shader shader = LoadShader(null, $"resources/shaders/glsl{GLSL_VERSION}/write_depth.fs");

            // Use customized function to create writable depth texture buffer
            RenderTexture2D target = LoadRenderTextureDepthTex(screenWidth, screenHeight);

            // Define the camera to look into our 3d world
            Camera3D camera;
            camera.position = new Vector3(2.0f, 2.0f, 3.0f);
            camera.target = new Vector3(0.0f, 0.5f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 45.0f;
            camera.projection = CameraProjection.CAMERA_PERSPECTIVE;

            SetTargetFPS(60);               // Set our game to run at 60 frames-per-second
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
                // Draw into our custom render texture (framebuffer)
                BeginTextureMode(target);
                ClearBackground(Color.WHITE);

                BeginMode3D(camera);
                BeginShaderMode(shader);

                DrawCubeWiresV(new Vector3(0.0f, 0.5f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), Color.RED);
                DrawCubeV(new Vector3(0.0f, 0.5f, 1.0f), new Vector3(1.0f, 1.0f, 1.0f), Color.PURPLE);
                DrawCubeWiresV(new Vector3(0.0f, 0.5f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f), Color.DARKGREEN);
                DrawCubeV(new Vector3(0.0f, 0.5f, -1.0f), new Vector3(1.0f, 1.0f, 1.0f), Color.YELLOW);
                DrawGrid(10, 1.0f);

                EndShaderMode();
                EndMode3D();
                EndTextureMode();

                // Draw custom render texture
                BeginDrawing();
                ClearBackground(Color.RAYWHITE);

                DrawTextureRec(
                    target.texture,
                    new Rectangle(0, 0, screenWidth, -screenHeight),
                    Vector2.Zero,
                    Color.WHITE
                );
                DrawFPS(10, 10);

                EndDrawing();
                //----------------------------------------------------------------------------------
            }

            // De-Initialization
            //--------------------------------------------------------------------------------------
            UnloadRenderTextureDepthTex(target);
            UnloadShader(shader);

            CloseWindow();
            //--------------------------------------------------------------------------------------

            return 0;
        }

        // Load custom render texture, create a writable depth texture buffer
        private static unsafe RenderTexture2D LoadRenderTextureDepthTex(int width, int height)
        {
            RenderTexture2D target = new RenderTexture2D();

            // Load an empty framebuffer
            target.id = rlLoadFramebuffer(width, height);

            if (target.id > 0)
            {
                rlEnableFramebuffer(target.id);

                // Create color texture (default to RGBA)
                target.texture.id = rlLoadTexture(
                    null,
                    width,
                    height,
                    PixelFormat.PIXELFORMAT_UNCOMPRESSED_R8G8B8A8,
                    1
                );
                target.texture.width = width;
                target.texture.height = height;
                target.texture.format = PixelFormat.PIXELFORMAT_UNCOMPRESSED_R8G8B8A8;
                target.texture.mipmaps = 1;

                // Create depth texture buffer (instead of raylib default renderbuffer)
                target.depth.id = rlLoadTextureDepth(width, height, false);
                target.depth.width = width;
                target.depth.height = height;
                target.depth.format = PixelFormat.PIXELFORMAT_COMPRESSED_PVRT_RGBA;
                target.depth.mipmaps = 1;

                // Attach color texture and depth texture to FBO
                rlFramebufferAttach(
                    target.id,
                    target.texture.id,
                    FramebufferAttachType.RL_ATTACHMENT_COLOR_CHANNEL0,
                    FramebufferAttachTextureType.RL_ATTACHMENT_TEXTURE2D,
                    0
                );
                rlFramebufferAttach(
                    target.id,
                    target.depth.id,
                    FramebufferAttachType.RL_ATTACHMENT_DEPTH,
                    FramebufferAttachTextureType.RL_ATTACHMENT_TEXTURE2D,
                    0
                );

                // Check if fbo is complete with attachments (valid)
                if (rlFramebufferComplete(target.id))
                {
                    TraceLog(TraceLogLevel.LOG_INFO, $"FBO: [ID {target.id}] Framebuffer object created successfully");
                }

                rlDisableFramebuffer();
            }
            else
            {
                TraceLog(TraceLogLevel.LOG_WARNING, "FBO: Framebuffer object can not be created");
            }

            return target;
        }

        // Unload render texture from GPU memory (VRAM)
        private static void UnloadRenderTextureDepthTex(RenderTexture2D target)
        {
            if (target.id > 0)
            {
                // Color texture attached to FBO is deleted
                rlUnloadTexture(target.texture.id);
                rlUnloadTexture(target.depth.id);

                // NOTE: Depth texture is automatically
                // queried and deleted before deleting framebuffer
                rlUnloadFramebuffer(target.id);
            }
        }
    }
}
