using DressII;
using UnityEngine;
using System.Collections.Generic;

public class AimLineSet : MonoBehaviour
{
    private List<Aimline> m_Lines;
    private Aimline m_Template;

    public IEnumerable<Aimline> Lines => m_Lines;

    public Aimline First => m_Lines.Count > 0 ? m_Lines[0] : null;

    private void Awake()
    {
        m_Lines = new List<Aimline>();
        m_Template = transform.Find("AimLine").GetComponent<Aimline>();
        m_Template.gameObject.SetActive(false);
    }

    public Aimline Get(int index)
    {
        if (index < 0 || index > m_Lines.Count - 1) return default;

        return m_Lines[index];
    }

    public Aimline Add()
    {
        GameObject inst = Instantiate(m_Template.gameObject);
        inst.SetActive(true);
        Aimline line = inst.GetComponent<Aimline>();
        line.transform.SetParent(transform, false);
        m_Lines.Add(line);
        return line;
    }

    public void AddWithSpriteShape(Sprite sprite)
    {
        int shapeCount = sprite.GetPhysicsShapeCount();
        List<Vector2> list = new List<Vector2>();
        for (int i = 0; i < shapeCount; i++)
        {
            list.Clear();
            int count = sprite.GetPhysicsShape(i, list);
            Aimline line = Add();
            Vector3[] points = new Vector3[count];
            for (int j = 0; j < count; j++)
            {
                Vector2 p = list[j];
                points[j] = p;
            }
            line.SetPoints(points, transform.position);
        }
    }

    public void AddWithCollider(Collider2D collider)
    {
        PhysicsShapeGroup2D group = new PhysicsShapeGroup2D(128, 1024);
        int shapeCount = collider.GetShapes(group);
        List<Vector2> list = new List<Vector2>();
        for (int i = 0; i < shapeCount; i++)
        {
            list.Clear();
            group.GetShapeVertices(i, list);
            Aimline line = Add();
            Vector3[] points = new Vector3[list.Count];
            for (int j = 0; j < list.Count; j++)
            {
                Vector2 p = list[j];
                points[j] = p;
            }
            line.SetPoints(points);
        }
    }
}
