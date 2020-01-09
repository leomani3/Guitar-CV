using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.IO;

public class CV : MonoBehaviour
{
    public string fileName;
    public Vector3Int min;
    public Vector3Int max;

    public bool isCircle;
    public bool isRectangle;
    public bool isTriangle;

    private VideoCapture videoCapture;

    private Image<Gray, byte> path;
    // Start is called before the first frame update
    void Start()
    {
        //videoCapture = new VideoCapture(fileName);
        videoCapture = new VideoCapture(0);
        path = new Image<Gray, byte>(640, 480);
    }

    // Update is called once per frame
    void Update()
    {
        Mat image = videoCapture.QueryFrame();
        Mat imageHSV = new Mat();
        Mat imageGray = new Mat();
        Mat imageBlured = new Mat();

        CvInvoke.CvtColor(image, imageGray, ColorConversion.Bgr2Gray);

        //CvInvoke.Blur(imageHSV, imageBlured, new Size(30,30), new Point(-1, -1));
        CvInvoke.GaussianBlur(image, imageBlured, new Size(3, 3), 10);

        CvInvoke.CvtColor(imageBlured, imageHSV, ColorConversion.Bgr2Hsv);

        Image<Hsv, byte> imgHSV = imageHSV.ToImage<Hsv, byte>();
        Image<Bgr, byte> img = image.ToImage<Bgr, byte>();
        imgHSV = imgHSV.Flip(FlipType.Horizontal);


        Image<Gray, byte> imgGray = imgHSV.InRange(new Hsv(min.x, min.y, min.z), new Hsv(max.x, max.y, max.z));

        Mat structElement = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(5, 5), new Point(-1, -1));
        CvInvoke.Erode(imgGray, imgGray, structElement, new Point(-1, -1), 2, BorderType.Constant, new MCvScalar(0));

        //détection de contours
        VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
        Mat m = new Mat();
        CvInvoke.FindContours(imgGray, contours, m, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
        for (int i = 0; i < contours.Size; i++)
        {
            double perimeter = CvInvoke.ArcLength(contours[i], true);
            VectorOfPoint approx = new VectorOfPoint();
            CvInvoke.ApproxPolyDP(contours[i], approx, 0.04 * perimeter, true);

            CvInvoke.DrawContours(image, contours, i, new MCvScalar(0, 0, 255));

            //calcul du centroid
            /*Moments moments = CvInvoke.Moments(contours[i]);
            int x = (int)(moments.M10 / moments.M00);
            int y = (int)(moments.M01 / moments.M00);

            path.Data[y, x, 0] = 255;*/

            if (approx.Size == 4)
            {
                isRectangle = true;
            }
            else
            {
                isRectangle = false;
            }

            if (approx.Size == 3)
            {
                isTriangle = true;
            }
            else
            {
                isTriangle = false;
            }

            if (approx.Size > 4)
            {
                isCircle = true;
            }
            else
            {
                isCircle = false;
            }
        }

        CvInvoke.Imshow("Cam view", imgGray);
        CvInvoke.WaitKey(24);
    }

    private void OnDestroy()
    {
        videoCapture.Dispose();
        CvInvoke.DestroyAllWindows();
    }
}
