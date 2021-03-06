﻿using OpenCvSharp.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision.Cv;

namespace Vision.Tests
{
    public class SimpleCapture
    {
        Capture capture;

        public void Run()
        {
            Logger.Log("E key to exit.");

            capture = Capture.New(0);
            capture.FrameReady += Capture_FrameReady;
            capture.Start();
            capture.Wait();
            capture.Dispose();
        }

        private void Capture_FrameReady(object sender, FrameArgs e)
        {
            Profiler.Count("FPS");

            if(e.LastKey == 'e')
            {
                e.Break = true;
                Core.Cv.CloseAllWindows();
                return;
            }

            Core.Cv.ImgShow("mat", e.Mat);
        }
    }
}
