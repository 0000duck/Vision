﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vision.Detection
{
    public class FaceDetectorXmlLoader
    {
        public string FaceXmlPath { get; private set; }
        public string EyeXmlPath { get; private set; }

        public FaceDetectorXmlLoader(string facePath, string eyesPath)
        {
            FaceXmlPath = facePath;
            EyeXmlPath = eyesPath;
        }

        public FaceDetectorXmlLoader(ManifestResource FaceResource, ManifestResource EyesResource)
        {
            FaceXmlPath = Storage.LoadResource(Cv.CascadeClassifier.DefaultFaceXmlName, true).AbosolutePath;
            EyeXmlPath = Storage.LoadResource(Cv.CascadeClassifier.DefaultEyesXmlName, true).AbosolutePath;
        }

        public FaceDetectorXmlLoader() : this(Cv.CascadeClassifier.DefaultFaceXmlName, Cv.CascadeClassifier.DefaultEyesXmlName)
        {

        }
    }
}
