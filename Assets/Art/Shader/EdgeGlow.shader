
Shader "Study/EdgeGlow" 
{
    //法线外扩实现描边
    Properties
    {
        _Outline("Outline",Range(0,0.2)) = 0.1
        _OutlineColor("OutlineColor",Color) = (0,0,0,1)
    }
    SubShader
    {
        //Queue设置为Transparent-1，放在天空盒之后，透明物体之前渲染
        Tags { "RenderType"="Opaque" "Queue" = "Transparent-1" }
        LOD 100
        //描边阶段放在第二个Pass，可以通过深度测试减少overdraw
        //描边阶段，法线外扩，渲染背面
        Pass
        {
            //只需要边缘外扩
            Cull Front
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv :TEXCOORD0;
            };
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv:TEXCOORD0;
            };
            float _Outline;
            float4 _OutlineColor;           
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //把法线转换到视图空间
                float3 vnormal = mul((float3x3)UNITY_MATRIX_IT_MV,v.normal);
                //把法线转换到投影空间
                float2 pnormal_xy = mul((float2x2)UNITY_MATRIX_P,vnormal.xy);
                //朝法线方向外扩
                o.vertex.xy = o.vertex.xy + pnormal_xy * _Outline;
                //如果想描边大小不会随着深度而变化，则乘以o.vertex.w，因为裁剪空间到屏幕空间会做齐次除法
                //o.vertex.xy = o.vertex.xy + pnormal_xy * o.vertex.w * _Outline;
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}
