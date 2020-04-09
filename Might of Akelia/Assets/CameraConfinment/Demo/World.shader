// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:Particles/Additive,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:33983,y:32723,varname:node_2865,prsc:2|emission-640-OUT,alpha-5202-OUT;n:type:ShaderForge.SFN_TexCoord,id:3976,x:31724,y:32829,varname:node_3976,prsc:2,uv:0;n:type:ShaderForge.SFN_RemapRange,id:9957,x:31886,y:32829,varname:node_9957,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-3976-UVOUT;n:type:ShaderForge.SFN_ComponentMask,id:7016,x:32048,y:32829,varname:node_7016,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-9957-OUT;n:type:ShaderForge.SFN_Multiply,id:7740,x:32223,y:32784,varname:node_7740,prsc:2|A-7016-R,B-7016-R;n:type:ShaderForge.SFN_Multiply,id:7095,x:32223,y:32918,varname:node_7095,prsc:2|A-7016-G,B-7016-G;n:type:ShaderForge.SFN_Vector1,id:7300,x:32018,y:33059,varname:node_7300,prsc:2,v1:6;n:type:ShaderForge.SFN_ValueProperty,id:5713,x:32018,y:33124,ptovrint:False,ptlb:Scalar,ptin:_Scalar,varname:node_5713,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:9409,x:32223,y:33059,varname:node_9409,prsc:2|A-7300-OUT,B-5713-OUT;n:type:ShaderForge.SFN_Multiply,id:6264,x:32432,y:32784,varname:node_6264,prsc:2|A-7740-OUT,B-9409-OUT;n:type:ShaderForge.SFN_Multiply,id:6235,x:32451,y:32918,varname:node_6235,prsc:2|A-7095-OUT,B-9409-OUT;n:type:ShaderForge.SFN_Sin,id:1492,x:32640,y:32784,varname:node_1492,prsc:2|IN-6264-OUT;n:type:ShaderForge.SFN_Sin,id:4316,x:32640,y:32918,varname:node_4316,prsc:2|IN-6235-OUT;n:type:ShaderForge.SFN_Step,id:8941,x:32964,y:32773,varname:node_8941,prsc:2|A-1492-OUT,B-970-OUT;n:type:ShaderForge.SFN_Step,id:2696,x:32959,y:32910,varname:node_2696,prsc:2|A-4316-OUT,B-970-OUT;n:type:ShaderForge.SFN_Vector1,id:970,x:32791,y:32864,varname:node_970,prsc:2,v1:0.5;n:type:ShaderForge.SFN_Multiply,id:8008,x:33146,y:32808,varname:node_8008,prsc:2|A-8941-OUT,B-2696-OUT;n:type:ShaderForge.SFN_OneMinus,id:6650,x:33472,y:32808,varname:node_6650,prsc:2|IN-4870-OUT;n:type:ShaderForge.SFN_Clamp01,id:4870,x:33309,y:32808,varname:node_4870,prsc:2|IN-8008-OUT;n:type:ShaderForge.SFN_Color,id:8176,x:33472,y:32965,ptovrint:False,ptlb:COLOUR,ptin:_COLOUR,varname:node_8176,prsc:2,glob:False,taghide:False,taghdr:True,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:640,x:33717,y:32823,varname:node_640,prsc:2|A-6650-OUT,B-8176-RGB;n:type:ShaderForge.SFN_Subtract,id:5202,x:33744,y:33051,varname:node_5202,prsc:2|A-6650-OUT,B-5121-OUT;n:type:ShaderForge.SFN_Slider,id:5121,x:33368,y:33175,ptovrint:False,ptlb:Transparency,ptin:_Transparency,varname:node_5121,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;proporder:5713-8176-5121;pass:END;sub:END;*/

Shader "Freddie Babord/Demo/World" {
    Properties {
        _Scalar ("Scalar", Float ) = 1
        [HDR]_COLOUR ("COLOUR", Color) = (0.5,0.5,0.5,1)
        _Transparency ("Transparency", Range(0, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float _Scalar;
            uniform float4 _COLOUR;
            uniform float _Transparency;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                UNITY_FOG_COORDS(3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
////// Lighting:
////// Emissive:
                float2 node_7016 = (i.uv0*2.0+-1.0).rg;
                float node_9409 = (6.0*_Scalar);
                float node_970 = 0.5;
                float node_6650 = (1.0 - saturate((step(sin(((node_7016.r*node_7016.r)*node_9409)),node_970)*step(sin(((node_7016.g*node_7016.g)*node_9409)),node_970))));
                float3 emissive = (node_6650*_COLOUR.rgb);
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,(node_6650-_Transparency));
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float _Scalar;
            uniform float4 _COLOUR;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : SV_Target {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float2 node_7016 = (i.uv0*2.0+-1.0).rg;
                float node_9409 = (6.0*_Scalar);
                float node_970 = 0.5;
                float node_6650 = (1.0 - saturate((step(sin(((node_7016.r*node_7016.r)*node_9409)),node_970)*step(sin(((node_7016.g*node_7016.g)*node_9409)),node_970))));
                o.Emission = (node_6650*_COLOUR.rgb);
                
                float3 diffColor = float3(0,0,0);
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, 0, specColor, specularMonochrome );
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Particles/Additive"
    CustomEditor "ShaderForgeMaterialInspector"
}
