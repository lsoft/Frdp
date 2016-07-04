// dllmain.cpp : Defines the entry point for the DLL application.
#include <math.h>

#include "stdafx.h"
#include "dllmain.h"

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}

namespace Useful
{
    void __stdcall DoCopy(
        ulong blockWidth
        ,ulong blockHeight
        ,ulong x1
        ,ulong y1
        ,long imageWidth
        ,long imageHeight
        ,byte* p
        ,byte* data
        //*/
        )
    {
        auto cc = 0;
        for (auto y = 0; y < blockHeight; y++)
        {
            auto x2 = x1;
            auto y2 = y1 + y;

            if (y2 < imageHeight)
            {
                auto p3 = p + (y2*imageWidth + x2)*4;

                for (auto x = 0; x < blockWidth; x++)
                {
                    if (x1 + x < imageWidth)
                    {
                        auto d = data[x + cc];

                        p3[0] = d;
                        p3[1] = d;
                        p3[2] = d;
                        p3[3] = 255;

                        p3 += 4;
                    }
                }

                cc += blockWidth;
            }
        }
        //*/
    }

    unsigned int __stdcall DoCrcCalculate(
        byte* input,
        unsigned long length
        )
    {
        unsigned int crc = 0xffff;
        byte i;

        for (auto cc = 0; cc < length; cc++)
        {
            crc ^= (unsigned int)(((unsigned int)input[cc]) << 8);

            for (i = 0; i < 8; i++)
                crc = (unsigned int)(((crc & 0x8000) > 0) ? (crc << 1) ^ 0x1021 : crc << 1);
        }

        return crc;
    }

    void __stdcall DoCut(
        byte mask,

        uint blockWidth,
        uint blockHeight,

        int blockCountHorizontal,
        int blockCountVertical,

        int blockHorizontalShift,
        int blockVerticalShift,

        uint blockHorizontalRightMask,
        uint blockVerticalRightMask,

        byte* blockData,

        byte* mat,

        int mat_Height,
        int mat_Width
        )
    {
        auto singleBlockSize = blockWidth * blockHeight;

        uint cc = 0;
        for (uint h = 0; h < mat_Height; h++)
        {
            auto vbi = h >> blockVerticalShift;
            auto bi = vbi * blockCountHorizontal + 0;

            auto preleft = (h & blockVerticalRightMask) * blockWidth;

            for (uint hbi = 0; hbi < blockCountHorizontal; hbi++)
            {
                auto w = hbi * blockWidth;
                
                auto wright = min(
                    (hbi + 1) * blockWidth,
                    mat_Width
                    );

                auto concretebi = bi + hbi;

                auto left = preleft + concretebi * singleBlockSize;

                auto right = left + (wright - w);

                for (auto concreteinbiindex = left; concreteinbiindex < right; concreteinbiindex++)
                {
                    auto a = mat[cc] & mask;

                    blockData[concreteinbiindex] = (byte)a;

                    cc++;
                }
            }
        }

        /*auto singleBlockSize = blockWidth * blockHeight;

        uint cc = 0;
        for (uint h = 0; h < mat_Height; h++)
        {
            auto vbi = h >> blockVerticalShift;
            auto bi = vbi * blockCountHorizontal + 0;

            auto inbiindex = (h & blockVerticalRightMask) * blockWidth;

            for (uint w = 0; w < mat_Width; w++)
            {
                auto a = mat[cc] & mask;

                auto hbi = w >> blockHorizontalShift;
                auto concretebi = bi + hbi;

                auto concreteinbiindex = inbiindex + (w & blockHorizontalRightMask);

                blockData[concretebi * singleBlockSize + concreteinbiindex] = (byte)a;

                cc++;
            }
        }*/
    }
}
