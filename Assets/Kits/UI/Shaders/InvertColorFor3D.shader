Shader "CustomRenderTexture/InvertColorFor3D"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex("InputTex", 2D) = "white" {}
     }

     SubShader {
   Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
   Lighting Off Cull Off ZWrite Off Fog { Mode Off }
   
   Pass {
      Color [_Color]
      AlphaTest Greater 0.5
      Blend SrcColor DstColor
      BlendOp Sub
      SetTexture [_MainTex] {
         combine previous, texture * primary
      }
   }
}
}
