Shader "CustomRenderTexture/RenderSameColor3D"
{
    Properties
    {
        _Threshold ("Threshold", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue"="Geometry"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Name "BackgroundColor"

            Tags
            {
                "LightMode"="UniversalForward"
            }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D_X(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);

            float _Threshold;

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionCS =
                    TransformObjectToHClip(IN.positionOS.xyz);

                OUT.screenPos =
                    ComputeScreenPos(OUT.positionCS);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 uv =
                    IN.screenPos.xy /
                    IN.screenPos.w;

                half3 background =
                    SAMPLE_TEXTURE2D_X(
                        _CameraOpaqueTexture,
                        sampler_CameraOpaqueTexture,
                        uv
                    ).rgb;

                // Convertir a luminosidad
                half luminance =
                    dot(
                        background,
                        half3(
                            0.2126,
                            0.7152,
                            0.0722
                        )
                    );

                // Negro / blanco
                half value =
                    step(_Threshold, luminance);

                return half4(
                    value,
                    value,
                    value,
                    1
                );
            }

            ENDHLSL
        }
    }
}