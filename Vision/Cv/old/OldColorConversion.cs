﻿//From EmguCV

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision.Cv
{
    /// <summary>
    /// Color conversion operation for CvCvtColor
    /// </summary>
    public enum OldColorConversion
    {
        BgrToBgra = 0,
        RgbToRgba = 0,
        BgraToBgr = 1,
        RgbaToRgb = 1,
        BgrToRgba = 2,
        RgbToBgra = 2,
        RgbaToBgr = 3,
        BgraToRgb = 3,
        BgrToRgb = 4,
        RgbToBgr = 4,
        BgraToRgba = 5,
        RgbaToBgra = 5,
        BgrToGray = 6,
        RgbToGray = 7,
        GrayToBgr = 8,
        GrayToRgb = 8,
        GrayToBgra = 9,
        GrayToRgba = 9,
        BgraToGray = 10,
        RgbaToGray = 11,
        BgrToBgr565 = 12,
        RgbToBgr565 = 13,
        Bgr565ToBgr = 14,
        Bgr565ToRgb = 15,
        BgraToBgr565 = 16,
        RgbaToBgr565 = 17,
        Bgr565ToBgra = 18,
        Bgr565ToRgba = 19,
        GrayToBgr565 = 20,
        Bgr565ToGray = 21,
        BgrToBgr555 = 22,
        RgbToBgra555 = 23,
        Bgr555ToBgr = 24,
        Bgr555ToRgb = 25,
        BgraToBgr555 = 26,
        RgbaToBgr555 = 27,
        Bgr555ToBgra = 28,
        Bgr555ToRgba = 29,
        GrayToBgr555 = 30,
        Bgr555ToGray = 31,
        BgrToXyz = 32,
        RgbToXyz = 33,
        XyzToBgr = 34,
        XyzToRgb = 35,
        BgrToCrCb = 36,
        RgbToCrCb = 37,
        CrCbToBgr = 38,
        CrCbToRgb = 39,
        BgrToHsv = 40,
        RgbToHsv = 41,
        BgrToLab = 44,
        RgbToLab = 45,
        BayerBgToBgr = 46,
        BayerRgToRgb = 46,
        BayerGbToBgr = 47,
        BayerGrToRgb = 47,
        BayerRgToBgr = 48,
        BayerBgToRgb = 48,
        BayerGrToBgr = 49,
        BayerGbToRgb = 49,
        BgrToLuv = 50,
        RgbToLuv = 51,
        BgrToHls = 52,
        RgbToHls = 53,
        HsvToBgr = 54,
        HsvToRgb = 55,
        LabToBgr = 56,
        LabToRgb = 57,
        LuvToBgr = 58,
        LuvToRgb = 59,
        HlsToBgr = 60,
        HlsToRgb = 61,
        BayerBgToBgr_Vng = 62,
        BayerRgToRgb_Vng = 62,
        BayerGbToBgr_Vng = 63,
        BayerGrToRgb_Vng = 63,
        BayerRgToBgr_Vng = 64,
        BayerBgToRgb_Vng = 64,
        BayerGrToBgr_Vng = 65,
        BayerGbToRgb_Vng = 65,
        BgrToHsv_Full = 66,
        RgbToHsv_Full = 67,
        BgrToHls_Full = 68,
        RgbToHls_Full = 69,
        HsvToBgr_Full = 70,
        HsvToRgb_Full = 71,
        HlsToBgr_Full = 72,
        HlsToRgb_Full = 73,
        LbgrToLab = 74,
        LrgbToLab = 75,
        LbgrToLuv = 76,
        LrgbToLuv = 77,
        LabToLbgr = 78,
        LabToLrgb = 79,
        LuvToLbgr = 80,
        LubToLrgb = 81,
        BgrToYuv = 82,
        RgbToYuv = 83,
        YuvToBgr = 84,
        YuvToRgb = 85,
        BayerBgToGray = 86,
        BayerGbToGray = 87,
        BayerRGToGray = 88,
        BayerGRToGray = 89,
        YuvToRGB_NV12 = 90,
        YuvToBGR_NV12 = 91,
        YuvToRGB_NV21 = 92,
        YUV420spToRgb = 92,
        YuvToBGR_NV21 = 93,
        YUV420spToBgr = 93,
        YuvToRgba_NV12 = 94,
        YuvToBgra_NV12 = 95,
        YuvToRgba_NV21 = 96,
        YUV420spToRgba = 96,
        YuvToBgra_NV21 = 97,
        YUV420spToBgra = 97,
        YuvToRgb_YV12 = 98,
        YUV420pToRgb = 98,
        YuvToBgr_YV12 = 99,
        YUV420pToBgr = 99,
        YuvToRgb_IYUV = 100,
        YuvToRgb_I420 = 100,
        YuvToBgr_IYUV = 101,
        YuvToBgr_I420 = 101,
        YuvToRgba_YV12 = 102,
        YUV420pToRgba = 102,
        YuvToBgra_YV12 = 103,
        YUV420pToBgra = 103,
        YuvToRgba_IYUV = 104,
        YuvToRgba_I420 = 104,
        YuvToBgra_IYUV = 105,
        YuvToBgra_I420 = 105,
        YuvToGray_420 = 106,
        YuvToGray_NV21 = 106,
        YuvToGray_NV12 = 106,
        YuvToGray_YV12 = 106,
        YuvToGray_IYUV = 106,
        YuvToGray_I420 = 106,
        YUV420spToGray = 106,
        YUV420pToGray = 106,
        YuvToRgb_UYVY = 107,
        YuvToRgb_Y422 = 107,
        YuvToRgb_UYNV = 107,
        YuvToBgr_UYVY = 108,
        YuvToBgr_Y422 = 108,
        YuvToBgr_UYNV = 108,
        YuvToRgba_UYVY = 111,
        YuvToRgba_Y422 = 111,
        YuvToRgba_UYNV = 111,
        YuvToBgra_UYVY = 112,
        YuvToBgra_Y422 = 112,
        YuvToBgra_UYNV = 112,
        YuvToRgb_YUY2 = 115,
        YuvToRgb_YUYV = 115,
        YuvToRgb_YUNV = 115,
        YuvToBgr_YUY2 = 116,
        YuvToBgr_YUYV = 116,
        YuvToBgr_YUNV = 116,
        YuvToRgb_YVYU = 117,
        YuvToBgr_YVYU = 118,
        YuvToRgba_YUY2 = 119,
        YuvToRgba_YUYV = 119,
        YuvToRgba_YUNV = 119,
        YuvToBgra_YUY2 = 120,
        YuvToBgra_YUYV = 120,
        YuvToBgra_YUNV = 120,
        YuvToRgba_YVYU = 121,
        YuvToBgra_YVYU = 122,
        YuvToGray_UYVY = 123,
        YuvToGray_Y422 = 123,
        YuvToGray_UYNV = 123,
        YuvToGray_YUY2 = 124,
        YuvToGray_YVYU = 124,
        YuvToGray_YUYV = 124,
        YuvToGray_YUNV = 124,
        RgbaTomRgba = 125,
        mRgbaToRgba = 126,
        RgbToYuv_I420 = 127,
        RgbToYuv_IYUV = 127,
        BgrToYuv_I420 = 128,
        BgrToYuv_IYUV = 128,
        RgbaToYuv_I420 = 129,
        RgbaToYuv_IYUV = 129,
        BgraToYuv_I420 = 130,
        BgraToYuv_IYUV = 130,
        RgbToYuv_YV12 = 131,
        BgrToYuv_YV12 = 132,
        RgbaToYuv_YV12 = 133,
        BgraToYuv_YV12 = 134
    }
}