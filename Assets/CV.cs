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

    public Material notValidMat;
    public Material validMat;

    public List<Zone> zones;

    private bool isWaiting = false;

    private Image<Gray, byte> path;
    Mat imageFix = new Mat();
    Image<Gray, byte> imgFix = new Image<Gray, byte>(626, 626);
    private VideoCapture fluxVideo;
    Mat image;
    // Start is called before the first frame update
    void Start()
    {
        //videoCapture = new VideoCapture(fileName);
        image = new Mat();
        fluxVideo = new VideoCapture(0, VideoCapture.API.Any);
        fluxVideo.ImageGrabbed += ProcessFrame;
    }

    // Update is called once per frame
    void Update()
    {
        fluxVideo.Grab();
    }

    private void ProcessFrame(object sender, EventArgs e)
    {

        try
        {
            Mat imageHSV = new Mat();
            Mat imageGray = new Mat();
            Mat imageBlured = new Mat();
            fluxVideo.Retrieve(image, 0);


            CvInvoke.CvtColor(image, imageGray, ColorConversion.Bgr2Gray);

            CvInvoke.GaussianBlur(image, imageBlured, new Size(3, 3), 10);

            CvInvoke.CvtColor(imageBlured, imageHSV, ColorConversion.Bgr2Hsv);

            Image<Hsv, byte> imgHSV = imageHSV.ToImage<Hsv, byte>();
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

                if (approx.Size == 4)
                {
                    isRectangle = true;
                }

                if (approx.Size == 3)
                {
                    if (perimeter > 100)
                    {
                        isTriangle = true;
                    }
                    else
                    {
                        isTriangle = false;
                    }

                }

                if (approx.Size > 4)
                {
                    isCircle = true;
                }
            }

            if (!isTriangle)
            {
                zones[0].GetComponent<Zone>().SetIsValid(true);
                zones[0].GetComponent<MeshRenderer>().material = validMat;
            }
            else
            {
                zones[0].GetComponent<Zone>().SetIsValid(false);
            }

            if (!isRectangle)
            {
                zones[1].GetComponent<Zone>().SetIsValid(true);
                zones[1].GetComponent<MeshRenderer>().material = validMat;
            }
            else
            {
                zones[1].GetComponent<Zone>().SetIsValid(false);
            }

            if (!isCircle)
            {
                zones[2].GetComponent<Zone>().SetIsValid(true);
                zones[2].GetComponent<MeshRenderer>().material = validMat;
            }
           else
            {
                zones[2].GetComponent<Zone>().SetIsValid(false);
            }

            /*isRectangle = false;
            isCircle = false;
            isTriangle = false;*/

            if (!isWaiting)
            {
                StartCoroutine(wait());
            }




            CvInvoke.Imshow("Cam view", imgGray);
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }
    }

    public IEnumerator wait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(0.05f);
        isCircle = false;
        isRectangle = false;
        isTriangle = false;
        isWaiting = false;
        zones[0].GetComponent<MeshRenderer>().material = notValidMat;
        zones[1].GetComponent<MeshRenderer>().material = notValidMat;
        zones[2].GetComponent<MeshRenderer>().material = notValidMat;
    }

    private void OnDestroy()
    {
        fluxVideo.Dispose();
        CvInvoke.DestroyAllWindows();
    }
}
