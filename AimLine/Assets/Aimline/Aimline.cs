using UnityEngine;
using BezierSolution;

namespace DressII
{
    public class Aimline : MonoBehaviour
    {
        private BezierSpline m_Spline;
        private LineRenderer m_Line;
        private Material m_LineMat;
        private BezierLineRenderer m_LineRenderer;

        private Camera m_FitCamera;
        private float m_CurSize;
        private float m_FitSize;
        private float m_FitWidth;
        private float m_FitLength;

        public float TotalLength => m_Spline.length;

        private void Awake()
        {
            m_Spline = GetComponent<BezierSpline>();
            m_Line = transform.Find("Line").GetComponent<LineRenderer>();
            m_LineRenderer = m_Line.GetComponent<BezierLineRenderer>();
            m_LineMat = new Material(m_Line.sharedMaterial);
            m_Line.material = m_LineMat;
        }

        private void Update()
        {
            if (m_FitCamera == null)
                return;

            float camSize = m_FitCamera.orthographicSize;
            float rate = camSize / m_FitSize;
            if (!Mathf.Approximately(camSize, m_CurSize))
            {
                m_CurSize = camSize;
                Width = rate * m_FitWidth;
                PerLength = (int)(m_FitLength / rate);
            }
        }

        public int Smooth
        {
            get { return m_LineRenderer.smoothness; }
            set { m_LineRenderer.smoothness = value; }
        }

        public bool LineLoop
        {
            get { return m_Spline.loop; }
            set { m_Spline.loop = value; }
        }

        public float Width
        {
            get { return m_Line.startWidth; }
            set
            {
                m_Line.startWidth = value;
                m_Line.endWidth = value;
            }
        }

        public float PerLength
        {
            get { return m_LineMat.GetFloat("_Tiling"); }
            set { m_LineMat.SetFloat("_Tiling", value); }
        }

        public float Speed
        {
            get { return m_LineMat.GetFloat("Speed"); }
            set { m_LineMat.SetFloat("Speed", value); }
        }

        public Color Color
        {
            get { return m_LineMat.GetColor("Color"); }
            set { m_LineMat.SetColor("Color", value); }
        }

        public float Alpha
        {
            get { return Color.a; }
            set
            {
                Color color = Color;
                color.a = value;
                Color = color;
            }
        }

        public void SetFitCamera(Camera camera, float fitSize, float fitWidth = 0.06f, float fitLength = 5)
        {
            m_FitCamera = camera;
            m_FitSize = fitSize;
            m_FitWidth = fitWidth;
            m_FitLength = fitLength;
        }

        public void SetPoints(Vector3[] points, Vector3 basePos = default)
        {
            for (int i = 0; i < points.Length; i++)
            {
                Vector3 point = points[i];

                BezierPoint bezier;
                if (i < m_Spline.Count)
                {
                    bezier = m_Spline[i];
                }
                else
                {
                    bezier = m_Spline.InsertNewPointAt(m_Spline.Count);
                }

                bezier.position = point + basePos;
            }

            m_LineRenderer.Refresh(m_LineRenderer.smoothness);
        }
    }
}