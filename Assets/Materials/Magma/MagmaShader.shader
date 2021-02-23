// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/MagmaShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        
        _RenderingBoundSize ("Rendering Bound Size", Vector) = (1, 1, 0, 0)
        _PixelationForce ("Pixelation Force", Float) = 1
        _FlowSpeed ("Flow Speed", Vector) = (1, 1, 0, 0)
        _CurveForce ("Curve Force", Float) = 1
        _DisplacementForce ("Displacement Force", Float) = 1
    }
    SubShader
    {
        Tags {
          "QUEUE"="Transparent"
          "IGNOREPROJECTOR"="true"
          "RenderType"="Transparent"
          "CanUseSpriteAtlas"="true"
          "PreviewType"="Plane"
        }
        
        ZWrite Off
        Blend One OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            float2 _RenderingBoundSize;
            float _PixelationForce;
            float2 _FlowSpeed;
            float _CurveForce;
            float _DisplacementForce;
            
            // NOISE START
            
            fixed3 mod289(fixed3 x)
            {
                return x - floor(x * (1.0 / 289.0)) * 289.0;
            }
            
            fixed2 mod289(fixed2 x)
            {
                return x - floor(x * (1.0 / 289.0)) * 289.0;
            }
            
            fixed3 permute(fixed3 x)
            {
                return mod289((x * 34.0 + 1.0) * x);
            }
            
            fixed3 taylorInvSqrt(fixed3 r)
            {
                return 1.79284291400159 - 0.85373472095314 * r;
            }
            
            float snoise(fixed2 v)
            {
                const fixed4 C = fixed4( 0.211324865405187,  // (3.0-sqrt(3.0))/6.0
                                     0.366025403784439,  // 0.5*(sqrt(3.0)-1.0)
                                    -0.577350269189626,  // -1.0 + 2.0 * C.x
                                     0.024390243902439); // 1.0 / 41.0
            
                // First corner
                fixed2 i  = floor(v + dot(v, C.yy));
                fixed2 x0 = v -   i + dot(i, C.xx);
            
                // Other corners
                fixed2 i1;
                i1.x = step(x0.y, x0.x);
                i1.y = 1.0 - i1.x;
            
                // x1 = x0 - i1  + 1.0 * C.xx;
                // x2 = x0 - 1.0 + 2.0 * C.xx;
                fixed2 x1 = x0 + C.xx - i1;
                fixed2 x2 = x0 + C.zz;
            
                // Permutations
                i = mod289(i); // Avoid truncation effects in permutation
                fixed3 p =
                  permute(permute(i.y + fixed3(0.0, i1.y, 1.0))
                                + i.x + fixed3(0.0, i1.x, 1.0));
            
                fixed3 m = max(0.5 - fixed3(dot(x0, x0), dot(x1, x1), dot(x2, x2)), 0.0);
                m = m * m;
                m = m * m;
            
                // Gradients: 41 points uniformly over a line, mapped onto a diamond.
                // The ring size 17*17 = 289 is close to a multiple of 41 (41*7 = 287)
                fixed3 x = 2.0 * frac(p * C.www) - 1.0;
                fixed3 h = abs(x) - 0.5;
                fixed3 ox = floor(x + 0.5);
                fixed3 a0 = x - ox;
            
                // Normalise gradients implicitly by scaling m
                m *= taylorInvSqrt(a0 * a0 + h * h);
            
                // Compute final noise value at P
                fixed3 g;
                g.x = a0.x * x0.x + h.x * x0.y;
                g.y = a0.y * x1.x + h.y * x1.y;
                g.z = a0.z * x2.x + h.z * x2.y;
                return 130.0 * dot(m, g);
            }
            // END: NOISE
            
            // HSV<->RGB
            fixed3 HSVToRGB(fixed3 c) {
                fixed4 K = fixed4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                fixed3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
            }
            // END: HSV<->RGB

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                const int colorNumbers = 5;
                const fixed4 colors[5] = {
                    fixed4(13.0, 0.2, 1.0, 0),
                    fixed4(8.0, 0.6, 1.0, 0.3),
                    fixed4(20, 0.8, 1, 0.5),
                    fixed4(45, 1.0, 1, 0.7),
                    fixed4(0.0, 0.0, 0.0, 1.0)
                };
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
               
                fixed2 UV = (i.uv.xy * _MainTex_ST.xy) + _MainTex_ST.zw;//i.uv.xy;
                UV *= fixed2(_RenderingBoundSize.x/_RenderingBoundSize.y, 1);
                //UV.x = frac(UV.x);
	            UV *= _PixelationForce;
	            UV = floor(UV);
	            UV /= _PixelationForce;
	
	            fixed2 noiseUV = UV;
	            noiseUV *= 2.0;
	            noiseUV -= 1.0;
	            noiseUV *= _CurveForce;
	            noiseUV += (_FlowSpeed * _Time);
	            noiseUV += 1.0;
                noiseUV /= 2.0;
	
	
	            float noise = (snoise(noiseUV) * 2.0) - 1.0;
	            UV -= fixed2(noise * _DisplacementForce, noise * _DisplacementForce);
	            UV = min(max(UV, fixed2(0, 0)), fixed2(1, 1));

	            fixed4 col = fixed4(1, 1, 1, 1);
	
                int closestColorIndex = 0;
                float closestColorDistance = 1;
                
                for (int i = 0; i < colorNumbers; i++) {
                    float dist = distance(UV.y, colors[i].w);
                    if (dist < closestColorDistance) {
                        closestColorIndex = i;
                        closestColorDistance = dist;
                    }
                }
	
	            fixed4 fc = fixed4(0, 0, 0, 0);
	            float fcPos = 0;
	            fixed4 sc = fixed4(0, 0, 0, 0);
	            float scPos = 0;
	            if (UV.y < colors[closestColorIndex].w) {
	                // Сравниваем с левым
	                fcPos = colors[closestColorIndex - 1].w;
	                fixed3 fcHSV = colors[closestColorIndex - 1].rrg;
	                fcHSV.r /= 360.0;
	                fcHSV.g = 1.0;
	                fc = fixed4(HSVToRGB(fcHSV), colors[closestColorIndex - 1].b);
	 
	                scPos = colors[closestColorIndex].w;
	                fixed3 scHSV = colors[closestColorIndex].rrg;
	                scHSV.r /= 360.0;
	                scHSV.g = 1.0;
	                sc = fixed4(HSVToRGB(scHSV), colors[closestColorIndex].b);
	            } else {
	                // сравниваем с правым
	                fcPos = colors[closestColorIndex].w;
	                fixed3 fcHSV = colors[closestColorIndex].rrg;
	                fcHSV.r /= 360.0;
	                fcHSV.g = 1.0;
	                fc = fixed4(HSVToRGB(fcHSV), colors[closestColorIndex].b);
	 
                    scPos = colors[closestColorIndex + 1].w;
                    fixed3 scHSV = colors[closestColorIndex + 1].rrg;
                    scHSV.r /= 360.0;
                    scHSV.g = 1.0;
                    sc = fixed4(HSVToRGB(scHSV), colors[closestColorIndex + 1].b);
	            }
	
	            float pos = (UV.y - fcPos) / (scPos - fcPos);
	            col = lerp(fc, sc, step(0.5, pos));
	            //col = fixed4(0,0,0,1);
	            //col.rg = UV;
	            
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
