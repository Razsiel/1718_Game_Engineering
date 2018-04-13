// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:4013,x:32719,y:32712,varname:node_4013,prsc:2|emission-4832-RGB,custl-3037-OUT;n:type:ShaderForge.SFN_NormalVector,id:4523,x:31339,y:32391,prsc:2,pt:False;n:type:ShaderForge.SFN_LightVector,id:5946,x:31339,y:32537,varname:node_5946,prsc:2;n:type:ShaderForge.SFN_Dot,id:63,x:31512,y:32391,varname:node_63,prsc:2,dt:0|A-4523-OUT,B-5946-OUT;n:type:ShaderForge.SFN_Ceil,id:6968,x:31872,y:32447,varname:node_6968,prsc:2|IN-6471-OUT;n:type:ShaderForge.SFN_Multiply,id:6471,x:31688,y:32447,varname:node_6471,prsc:2|A-63-OUT,B-4202-OUT;n:type:ShaderForge.SFN_Divide,id:2955,x:32102,y:32447,varname:node_2955,prsc:2|A-6968-OUT,B-1984-OUT;n:type:ShaderForge.SFN_Tex2d,id:4832,x:32064,y:32760,ptovrint:False,ptlb:Main Tex,ptin:_MainTex,varname:node_4832,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ValueProperty,id:427,x:31339,y:32701,ptovrint:False,ptlb:Bands,ptin:_Bands,varname:node_427,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:4;n:type:ShaderForge.SFN_Divide,id:4202,x:31510,y:32701,varname:node_4202,prsc:2|A-427-OUT,B-3381-OUT;n:type:ShaderForge.SFN_Vector1,id:3381,x:31339,y:32778,varname:node_3381,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:1984,x:31510,y:32870,varname:node_1984,prsc:2|A-427-OUT,B-1169-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:3037,x:32317,y:32565,varname:node_3037,prsc:2,min:-1,max:1|IN-2955-OUT;n:type:ShaderForge.SFN_Slider,id:1169,x:31331,y:33088,ptovrint:False,ptlb:node_1169,ptin:_node_1169,varname:node_1169,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.07464026,max:2;proporder:4832-427-1169;pass:END;sub:END;*/

Shader "Shader Forge/ToonLit" {
    Properties {
        _MainTex ("Main Tex", 2D) = "white" {}
        _Bands ("Bands", Float ) = 4
        _node_1169 ("node_1169", Range(0, 2)) = 0.07464026
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _Bands;
            uniform float _node_1169;
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
                LIGHTING_COORDS(3,4)
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 emissive = _MainTex_var.rgb;
                float node_63 = dot(i.normalDir,lightDirection);
                float node_3381 = 2.0;
                float node_2955 = (ceil((node_63*(_Bands/node_3381)))/(_Bands*_node_1169));
                float node_3037 = clamp(node_2955,-1,1);
                float3 finalColor = emissive + float3(node_3037,node_3037,node_3037);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
