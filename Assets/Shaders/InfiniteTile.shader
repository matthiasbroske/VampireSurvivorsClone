// The MIT License
// Copyright © 2017 Inigo Quilez
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software. THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

// One way to avoid texture tile repetition is by using using one small
// texture to cover a huge area. Basically, it creates 8 different
// offsets for the texture and picks two to interpolate between.
//
// Unlike previous methods that tile space (https://www.shadertoy.com/view/lt2GDd
// or https://www.shadertoy.com/view/4tsGzf), this one uses a random
// low frequency texture (cache friendly) to pick the actual
// texture's offset.
//
// Also, this one mipmaps to something (ugly, but that's better than
// not having mipmaps at all like in previous methods)
//
// More info here: https://iquilezles.org/articles/texturerepetition

// The original shader was modified to function in Unity by Matthias Broske

Shader "Custom/InfiniteTile"
{
    Properties
    {
        [MaterialToggle] _AvoidRepitition ("Avoid Texture Repitition", Int) = 1
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Texture", 2D) = "white" {}
        _Scale ("Scale", Float) = 1
        _Tint ("Tint", Color) = (1, 1, 1, 1)
        _Shockwave ("Ripple", Float) = 1
        _ShockwaveWidth ("Ripple Width", Float) = 0.2
        _ShockwaveIntensity ("Ripple Intensity", Float) = 1
        _ResetOffset ("Reset Offset", Vector) = (0,0,0,0)
        _TempResetOffset ("Temp Reset Offset", Vector) = (0,0,0,0)
        _ResetBlend ("Reset Blend", Float) = 0
        [MaterialToggle] _Resetting ("Resetting", Int) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                half4 vertex : POSITION;
                half2 uv : TEXCOORD0;
            };

            struct v2f
            {
                half2 uv : TEXCOORD0;
                half2 worldPosition : TEXCOORD1;
                half4 vertex : SV_POSITION;
            };

            int _AvoidRepitition;
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            half4 _MainTex_ST;
            half _Scale;
            half4 _Tint;
            half _Shockwave;
            half _ShockwaveWidth;
            half _ShockwaveIntensity;
            half2 _PlayerPosition;
            half _ResetBlend;
            int _Resetting;
            half2 _ResetOffset;
            half2 _TempResetOffset;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex).xy;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half sum(half3 v) { return v.x+v.y+v.z; }

            fixed4 frag (v2f i) : SV_Target
            {
                half2 uv = (i.worldPosition + _ResetOffset)*1/_Scale;
                if (_Shockwave > 0)
                {
                    half d = length(i.worldPosition - _PlayerPosition);
                    if (d > _Shockwave-_ShockwaveWidth && d < _Shockwave+_ShockwaveWidth)
                    {
                        //return (_ShockwaveWidth-abs(_Shockwave - d));
                        uv.xy -= normalize(i.worldPosition) * (_ShockwaveWidth-abs(_Shockwave - d)) *  _ShockwaveIntensity;// * (_Shockwave - d)*2;
                    }
                }

                fixed4 col = 0;

                if (_AvoidRepitition)
                {
                    half k = tex2D( _NoiseTex, 0.005*uv ).x;
        
                    half2 duvdx = ddx( uv );
                    half2 duvdy = ddy( uv );
                    
                    half l = k*8.0;
                    half ia = floor(l);
                    half f = l-ia;
                    half ib = ia + 1.0;
                    
                    half2 offa = sin(half2(3.0,7.0)*ia);
                    half2 offb = sin(half2(3.0,7.0)*ib);

                    half3 cola = tex2D( _MainTex, uv + offa, duvdx, duvdy ).xyz;
                    half3 colb = tex2D( _MainTex, uv + offb, duvdx, duvdy ).xyz;
                    
                    col.xyz = lerp( cola, colb, smoothstep(0.2,0.8,f-0.1*sum(cola-colb)) );

                    if (_Resetting)
                    {
                        uv = (i.worldPosition + _ResetOffset + _TempResetOffset)*1/_Scale;
                        k = tex2D( _NoiseTex, 0.005*uv ).x;
            
                        duvdx = ddx( uv );
                        duvdy = ddy( uv );
                        
                        l = k*8.0;
                        ia = floor(l);
                        f = l-ia;
                        ib = ia + 1.0;
                        
                        offa = sin(half2(3.0,7.0)*ia);
                        offb = sin(half2(3.0,7.0)*ib);

                        cola = tex2D( _MainTex, uv + offa, duvdx, duvdy ).xyz;
                        colb = tex2D( _MainTex, uv + offb, duvdx, duvdy ).xyz;
                        
                        col.xyz = lerp(col.xyz, lerp( cola, colb, smoothstep(0.2,0.8,f-0.1*sum(cola-colb)) ), _ResetBlend);
                    }
                }
                else
                {
                    col = tex2D(_MainTex, uv);
                }

                return col*_Tint;
            }
            ENDCG
        }
    }
}
