﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision.Detection
{
    public class ScreenProperties
    {
        /// <summary>
        /// Origin point of screen in mm (left, top aligned)
        /// </summary>
        public Point3D Origin { get; set; }

        /// <summary>
        /// Size in mm
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// Size in pixel
        /// </summary>
        public Size PixelSize { get; set; }

        public ScreenProperties()
        {

        }

        public ScreenProperties(Point3D trans, Size size, Size pixelSize)
        {
            Origin = trans;
            Size = size;
            PixelSize = pixelSize;
        }

        public static ScreenProperties CreatePixelScreen(double width, double height, double dpi = 96)
        {
            return CreatePixelScreen(new Size(width, height), dpi);
        }

        public static ScreenProperties CreatePixelScreen(Size pixelSize, double dpi = 96)
        {
            return new ScreenProperties()
            {
                PixelSize = pixelSize,
                Size = new Size(pixelSize.Width / dpi * 25, pixelSize.Height / dpi * 25),
                Origin = new Point3D(-(pixelSize.Width / dpi * 25) / 2, 0, 0)
            };
        }

        public Point3D ToCameraCoordinate(double unitPerMM, Point pt)
        {
            return new Point3D((pt.X / PixelSize.Width * Size.Width + Origin.X) * unitPerMM, (pt.Y / PixelSize.Height * Size.Height + Origin.Y) * unitPerMM, Origin.Z * unitPerMM);
        }

        public Point ToScreenCoordinate(double unitPerMM, Point3D pt)
        {
            double mmX = pt.X / unitPerMM;
            double mmY = pt.Y / unitPerMM;
            mmX = mmX - Origin.X;
            mmY = mmY - Origin.Y;
            mmX = mmX / Size.Width;
            mmY = mmY / Size.Height;
            mmX = mmX * PixelSize.Width;
            mmY = mmY * PixelSize.Height;

            return new Point(mmX, mmY);
        }
    }
}
