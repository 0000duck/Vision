﻿using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision.Cv
{
    public static class MatTool
    {
        public static Mat New()
        {
            return new Mat();
        }

        public static Mat New(Size size, MatType type)
        {
            return new Mat(size.ToCvSize(), type);
        }

        public static Mat New(Size size, MatType type, Array buffer)
        {
            return new Mat((int)size.Width, (int)size.Height, type, buffer);
        }

        public static Mat New(Mat Mat, Rect Rect, bool clamp = false)
        {
            double clmpX = Math.Max(0, Rect.X);
            double clmpY = Math.Max(0, Rect.Y);
            double clmpW = Math.Min(Mat.Width - 1, Rect.X + Rect.Width) - clmpX;
            double clmpH = Math.Min(Mat.Height - 1, Rect.Y + Rect.Height) - clmpY;

            return new Mat(Mat, new Rect(clmpX, clmpY, clmpW, clmpH).ToCvRect());
        }

        #region extensions

        public static void ConvertColor(this Mat self, Mat output, ColorConversionCodes convert)
        {
            Cv2.CvtColor(self, output, convert);
        }

        public static void ConvertColor(this Mat self, ColorConversionCodes convert)
        {
            ConvertColor(self, convert);
        }

        public static void EqualizeHistogram(this Mat self, Mat output)
        {
            Cv2.EqualizeHist(self, output);
        }

        public static void EqualizeHistogram(this Mat self)
        {
            EqualizeHistogram(self, self);
        }

        public static void Canny(this Mat self)
        {
            Canny(self, self, 50, 100);
        }

        public static void Canny(this Mat self, Mat output, double thresold1, double thresold2)
        {
            Cv2.Canny(self, output, thresold1, thresold2);
        }

        public static void NormalizeRGB(this Mat self, Mat output, double clip)
        {
            if (self.Channel != 3)
                throw new NotSupportedException("Channel sould be RGB");

            ConvertColor(self, ColorConversionCodes.BGR2Lab);

            Mat[] spl = Split(self);

            CLAHE c = CLAHE.Create(clip, new OpenCvSharp.Size(8, 8));
            c.Apply(spl[0], spl[0]);

            Merge(self, spl);

            ConvertColor(self, ColorConversionCodes.Lab2BGR);
        }

        public static void NormalizeRGB(this Mat self, Mat output)
        {
            NormalizeRGB(self, output, 1);
        }

        public static void NormalizeRGB(this Mat self)
        {
            NormalizeRGB(self);
        }

        public static Mat[] Split(this Mat self)
        {
            return Cv2.Split(self);
        }

        public static void Merge(this Mat self, Mat[] channels)
        {
            Cv2.Merge(channels, self);
        }

        public static double CalcScaleFactor(this Mat self, double maxsize)
        {
            double fwidth = self.Width;
            double fheight = self.Height;

            double scaleFactor = 1;

            if (Math.Max(fwidth, fheight) > maxsize)
            {
                if (fwidth > fheight)
                {
                    scaleFactor = maxsize / fwidth;
                }
                else
                {
                    scaleFactor = maxsize / fheight;
                }
            }

            return scaleFactor;
        }

        public static double ClampSize(this Mat self, double maxsize, InterpolationFlags inter = InterpolationFlags.Nearest)
        {
            double scaleFactor = CalcScaleFactor(self, maxsize);

            if (scaleFactor != 1)
            {
                Resize(self, scaleFactor, inter);
            }

            return scaleFactor;
        }

        public static void Resize(this Mat self, double scaleFactor, InterpolationFlags inter = InterpolationFlags.Linear)
        {
            Resize(self, new Size(self.Width * scaleFactor, self.Height * scaleFactor), 0, 0, inter);
        }

        public static void Resize(this Mat self, Size size, double fx = 0, double fy = 0, InterpolationFlags inter = InterpolationFlags.Linear)
        {
            Resize(self, self, size, fx, fy, inter);
        }

        public static void Resize(this Mat self, Mat output, Size size, double fx = 0, double fy = 0, InterpolationFlags inter = InterpolationFlags.Linear)
        {
            Cv2.Resize(self, output, size.ToCvSize(), fx, fy, inter);
        }

        public static void DrawText(this Mat self, double x, double y, string text)
        {
            DrawText(self, x, y, text, Scalar.BgrWhite);
        }

        public static void DrawText(this Mat self, double x, double y, string text, Scalar color)
        {
            DrawText(self, new Point(x, y), text, color);
        }

        public static void DrawText(this Mat self, Point org, string txt)
        {
            DrawText(self, org, txt, Scalar.BgrWhite);
        }

        public static void DrawText(this Mat self, Point org, string txt, Scalar color)
        {
            Core.Cv.DrawText(self, txt, org, HersheyFonts.HersheyPlain, 2.5, color, 2, LineTypes.Link4);
        }

        public static void DrawRectangle(this Mat self, Rect rect, Scalar color, int thickness = 1, LineTypes lineType = LineTypes.Link8, int shift = 0)
        {
            Core.Cv.DrawRectangle(self, rect, color, thickness, lineType, shift);
        }

        public static void DrawCircle(this Mat self, Point center, double radius, Scalar color, double thickness = 1, LineTypes lineType = LineTypes.Link8, int shift = 0)
        {
            Core.Cv.DrawCircle(self, center, radius, color, thickness, lineType, shift);
        }

        public static void DrawEllipse(this Mat self, Point center, Size axes, double angle, double startAngle, double endAngle, Scalar color, double thickness = 1, LineTypes lineType = LineTypes.Link8, int shift = 0)
        {
            Core.Cv.DrawEllipse(self, center, axes, angle, startAngle, endAngle, color, thickness, lineType, shift);
        }

        public static void DrawLine(this Mat self, double x, double y, double x1, double y1)
        {
            DrawLine(self, new Point(x, y), new Point(x1, y1));
        }

        public static void DrawLine(this Mat self, Point start, Point end)
        {
            DrawLine(self, start, end, Scalar.BgrWhite);
        }

        public static void DrawLine(this Mat self, Point start, Point end, Scalar scalar, double thickness = 1, LineTypes lineType = LineTypes.Link8, int shift = 0)
        {
            Core.Cv.DrawLine(self, start, end, scalar, (int)thickness, lineType, shift);
        }

        public static string Print(this Mat self)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{ ");
            for (int r = 0; r < self.Rows; r++)
            {
                builder.Append("{ ");
                for (int c = 0; c < self.Cols; c++)
                {
                    builder.Append($"{self.At<double>(r, c)}, ");
                }
                builder.AppendLine("}");
            }
            builder.AppendLine("}");
            return builder.ToString();
        }

        public static float[] GetArray(this Mat self, float[] buffer = null, bool bgr2rgb = true)
        {
            int width = self.Width;
            int height = self.Height;
            float[] f;
            if (buffer == null)
            {
                f = new float[width * height * self.Channel];
            }
            else
            {
                if (buffer.Length < width * height * self.Channel)
                    throw new ArgumentOutOfRangeException(nameof(buffer));
                f = buffer;
            }
            using (MatOfByte3 matByte = new MatOfByte3())
            {
                self.CopyTo(matByte);

                var indexer = matByte.GetIndexer();
                int i = 0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Vec3b color = indexer[y, x];
                        if (bgr2rgb)
                        {
                            f[i] = color.Item2;
                            i++;
                            f[i] = color.Item1;
                            i++;
                            f[i] = color.Item0;
                            i++;
                        }
                        else
                        {
                            f[i] = color.Item0;
                            i++;
                            f[i] = color.Item1;
                            i++;
                            f[i] = color.Item2;
                            i++;
                        }
                    }
                }
            }

            return f;
        }

        #endregion Extensions
    }
}