#pragma once

typedef char byte;
typedef unsigned int uint;
typedef unsigned long ulong;

namespace Useful
{
    extern "C" __declspec(dllexport) void __stdcall DoCopy(
        ulong blockWidth
        ,ulong blockHeight
        ,ulong x1
        ,ulong y1
        ,long imageWidth
        ,long imageHeight
        ,byte* p
        ,byte* data
        //*/
        );

    extern "C" __declspec(dllexport) unsigned int __stdcall DoCrcCalculate(
        byte* input,
        unsigned long length
        );

    extern "C" __declspec(dllexport) void __stdcall DoCut(
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
        );
}

