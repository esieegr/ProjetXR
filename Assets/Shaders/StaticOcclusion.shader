Shader "Custom/StaticOcclusion"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1"}

        LOD 50

        ZWrite On

        ZTest LEqual

        ColorMask 0

        Cull Back


        Pass {}
    }
}
