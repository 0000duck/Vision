﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision.Cv;

namespace Vision.Detection
{
    public class FaceDetector : IDisposable
    {
        public double FaceScaleFactor { get; set; } = 1.2;
        public double FaceMinFactor { get; set; } = 0.1;
        public double FaceMaxFactor { get; set; } = 1;
        public int MaxSize { get; set; } = 180;
        
        public bool EyesDetectCascade { get; set; } = true;
        public bool EyesDetectLandmark { get; set; } = true;

        public double EyesScaleFactor { get; set; } = 1.2;
        public double EyesMinFactor { get; set; } = 0.2;
        public double EyesMaxFactor { get; set; } = 0.5;
        public int MaxFaceSize { get; set; } = 100;

        public bool LandmarkDetect { get; set; } = true;
        public bool LandmarkSolve { get; set; } = true;

        public bool SmoothLandmarks { get; set; } = false;
        public bool SmoothVectors { get; set; } = false;
        
        public Interpolation Interpolation { get; set; } = Interpolation.NearestNeighbor;

        CascadeClassifier FaceCascade;
        CascadeClassifier EyesCascade;
        FaceLandmarkDetector Landmark;
        
        List<FaceSmoother> Smoother = new List<FaceSmoother>();

        public FaceDetector(string FaceXml, string EyesXml)
        {
            FaceCascade = CascadeClassifier.New(FaceXml);
            EyesCascade = CascadeClassifier.New(EyesXml);
            Landmark = new FaceLandmarkDetector();
        }

        public FaceDetector(FaceDetectorXmlLoader Loader) : this(Loader.FaceXmlPath, Loader.EyeXmlPath)
        {

        }

        public FaceDetector() : this(new FaceDetectorXmlLoader())
        {

        }
        
        public FaceRect[] Detect(VMat frame, bool debug = false)
        {
            if (frame.IsEmpty)
                return null;

            using (VMat frame_gray = VMat.New())
            {
                Profiler.Start("DetectionPre");
                Profiler.Start("DetectionPre.Cvt");
                frame.ConvertColor(frame_gray, ColorConversion.BgrToGray);
                Profiler.End("DetectionPre.Cvt");
                double scaleFactor = frame_gray.CalcScaleFactor(MaxSize);
                VMat frame_face = null;
                if(scaleFactor != 1)
                {
                    frame_face = frame_gray.Clone();
                    frame_gray.ClampSize(MaxSize, Interpolation);
                }
                else
                {
                    frame_face = frame_gray;
                }
                frame_gray.EqualizeHistogram();
                Profiler.End("DetectionPre");

                Profiler.Start("DetectionFace");
                double frameMinSize = Math.Min(frame_gray.Width, frame_gray.Height);
                Rect[] faces = FaceCascade.DetectMultiScale(frame_gray, FaceScaleFactor, 2, HaarDetectionType.ScaleImage, new Size(frameMinSize * FaceMinFactor), new Size(frameMinSize * FaceMaxFactor));
                List<FaceRect> FaceList = new List<FaceRect>();
                Profiler.End("DetectionFace");
                
                Profiler.Start("DetectionEyes");
                int index = 0;
                foreach (Rect face in faces)
                {
                    if (Smoother.Count <= index)
                        Smoother.Add(new FaceSmoother());
                    FaceSmoother smoothCurrent = Smoother[index];

                    FaceRect faceRect = new FaceRect(face);
                    faceRect.Scale(1 / scaleFactor);

                    if (debug)
                        faceRect.Draw(frame, drawLandmarks: true);

                    if (LandmarkDetect || EyesDetectLandmark)
                    {
                        Profiler.Start("DetectionLandmark");
                        Landmark.Interpolation = Interpolation;
                        Landmark.Detect(frame_face, faceRect);
                        if (SmoothLandmarks)
                            smoothCurrent.SmoothLandmark(faceRect);

                        Profiler.Start("DetectionLandmarkSolve");
                        if (LandmarkSolve)
                        {
                            Landmark.Solve(frame_face, faceRect);
                            if (SmoothVectors)
                                smoothCurrent.SmoothVector(faceRect);
                        }
                        Profiler.End("DetectionLandmarkSolve");

                        Profiler.End("DetectionLandmark");
                    }
                    
                    if(EyesDetectCascade)
                    {
                        using (VMat faceROI = VMat.New(frame_face, faceRect))
                        {
                            double eyeScale = faceROI.ClampSize(MaxFaceSize, Interpolation);
                            double faceMinSize = Math.Min(faceROI.Width, faceROI.Height);
                            Profiler.Start("DetectionEye");
                            Rect[] eyes = EyesCascade.DetectMultiScale(faceROI, EyesScaleFactor, 2, HaarDetectionType.ScaleImage, new Size(faceMinSize * EyesMinFactor), new Size(faceMinSize * EyesMaxFactor));

                            foreach (Rect eye in eyes)
                            {
                                eye.Scale(1 / eyeScale);
                                EyeRect eyeRect = new EyeRect(faceRect, eye);
                                faceRect.Add(eyeRect);

                                if (debug)
                                    eyeRect.Draw(frame);
                            }
                            Profiler.End("DetectionEye");
                        }
                    }

                    if (EyesDetectLandmark)
                    {
                        FaceLandmarkDetector.CalcEyes(faceRect);
                    }

                    FaceList.Add(faceRect);
                    index++;
                }
                Profiler.End("DetectionEyes");
                
                if (frame_face != null)
                {
                    frame_face.Dispose();
                    frame_face = null;
                }
                var rect = FaceList.ToArray();

                return rect;
            }
        }

        public void Dispose()
        {
            if(EyesCascade != null)
            {
                EyesCascade.Dispose();
                EyesCascade = null;
            }

            if(FaceCascade != null)
            {
                FaceCascade.Dispose();
                FaceCascade = null;
            }
        }
    }

    public class FaceSmoother
    {
        PointKalmanFilter[] LandmarkFilter;
        ArrayKalmanFilter TranslateFilter;

        public FaceSmoother()
        {
            LandmarkFilter = new PointKalmanFilter[8];
            for (int i = 0; i < 8; i++)
            {
                LandmarkFilter[i] = new PointKalmanFilter();
            }

            TranslateFilter = new ArrayKalmanFilter(3);
        }

        public void SmoothLandmark(FaceRect face)
        {
            if (face.Landmarks != null)
            {
                for (int i = 0; i < LandmarkFilter.Length; i++)
                {
                    face.Landmarks[i] = LandmarkFilter[i].Calculate(face.Landmarks[i]);
                }
            }
        }

        private double translateLimit = 5000;
        public void SmoothVector(FaceRect face)
        {
            if(face.LandmarkTransformVector!=null && face.LandmarkRotationVector != null)
            {
                if(face.LandmarkTransformVector[2] < 0)
                {
                    ArrayMul(face.LandmarkTransformVector, -1);
                }

                double min = face.LandmarkTransformVector.Min();
                double max = face.LandmarkTransformVector.Max();
                double absMax = Math.Max(Math.Abs(min), Math.Abs(max));
                if(absMax > translateLimit)
                {
                    ArrayMul(face.LandmarkTransformVector, translateLimit/absMax);
                    face.LandmarkTransformVector = TranslateFilter.Clone().Calculate(face.LandmarkTransformVector);
                }
                else
                {
                    face.LandmarkTransformVector = TranslateFilter.Calculate(face.LandmarkTransformVector);
                }
            }
        }

        private void ArrayMul(double[] array, double right)
        {
            for(int i=0; i<array.Length; i++)
            {
                array[i] *= right;
            }
        }
    }
}
