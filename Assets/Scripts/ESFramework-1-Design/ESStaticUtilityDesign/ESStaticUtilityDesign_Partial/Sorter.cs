using ES;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ES.EnumCollect;

namespace ES
{

    public static partial class ESStaticDesignUtility
    {
        //排序器
        public static class Sorter
        {
            public static List<Vector3> SortPath(List<Vector3> vectors, EnumCollect.PathSortType sortType, Vector3 pos = default, Transform transform = null)
            {
                if (vectors == null) return new List<Vector3>();
                if (vectors.Count <= 1) return vectors;

                switch (sortType)
                {
                    case PathSortType.NoneSort: return vectors;
                    case PathSortType.StartFromNearToFar:
                        return vectors.OrderBy((n) => Vector3.Distance(pos, n)).ToList();
                    case PathSortType.StartFromFarToNear:
                        return vectors.OrderByDescending((n) => Vector3.Distance(pos, n)).ToList();
                    case PathSortType.Yup:
                        return vectors.OrderBy((n) => n.y).ToList();
                    case PathSortType.Ydown:
                        return vectors.OrderByDescending((n) => n.y).ToList();
                    case PathSortType.Xup:
                        return vectors.OrderBy((n) => n.x).ToList();
                    case PathSortType.Xdown:
                        return vectors.OrderByDescending((n) => n.x).ToList();
                    case PathSortType.Zup:
                        return vectors.OrderBy((n) => n.z).ToList();
                    case PathSortType.Zdown:
                        return vectors.OrderByDescending((n) => n.z).ToList();
                    case PathSortType.StartForwardZup:
                        if (transform != null)
                            return vectors.OrderBy((n) => transform.InverseTransformPoint(n).z).ToList();
                        else return vectors.OrderBy((n) => n.z).ToList();
                    case PathSortType.StartForwardZdown:
                        if (transform != null)
                            return vectors.OrderByDescending((n) => transform.InverseTransformPoint(n).z).ToList();
                        else return vectors.OrderByDescending((n) => n.z).ToList();
                    case PathSortType.Random:
                        return vectors.OrderBy((n) => UnityEngine.Random.value).ToList();
                    case PathSortType.AlwaysFirstNear:
                        return SortForLast_Near(vectors, pos);
                    case PathSortType.AlwaysFirstFar:
                        return SortForLast(vectors, pos, (a, b) => -Vector3.Distance(a, b));
                    case PathSortType.AlwaysForwardZup:
                        return SortForLast_Three(vectors, pos, (a, b, c) =>
                        {
                            if (b != c)
                            {
                                return Vector3.Angle(a - b, b - c);
                            }
                            return Vector3.Distance(a, b);

                        });
                    case PathSortType.AlwaysForwardZdown:
                        return SortForLast_Three(vectors, pos, (a, b, c) =>
                        {
                            if (b != c)
                            {
                                return -Vector3.Angle(a - b, b - c);
                            }
                            return -Vector3.Distance(a, b);

                        });
                }
                return vectors;
            }

            public static List<T> SortAny<T>(List<T> vectors, Func<T, Vector3> GetPos, EnumCollect.PathSortType sortType, Vector3 pos = default, Transform transform = null)
            {
                if (vectors == null) return new List<T>();
                if (vectors.Count <= 1) return vectors;

                switch (sortType)
                {
                    case PathSortType.NoneSort: return vectors;
                    case PathSortType.StartFromNearToFar:
                        return vectors.OrderBy((n) => Vector3.Distance(pos, GetPos(n))).ToList();
                    case PathSortType.StartFromFarToNear:
                        return vectors.OrderByDescending((n) => Vector3.Distance(pos, GetPos(n))).ToList();
                    case PathSortType.Yup:
                        return vectors.OrderBy((n) => GetPos(n).y).ToList();
                    case PathSortType.Ydown:
                        return vectors.OrderByDescending((n) => GetPos(n).y).ToList();
                    case PathSortType.Xup:
                        return vectors.OrderBy((n) => GetPos(n).x).ToList();
                    case PathSortType.Xdown:
                        return vectors.OrderByDescending((n) => GetPos(n).x).ToList();
                    case PathSortType.Zup:
                        return vectors.OrderBy((n) => GetPos(n).z).ToList();
                    case PathSortType.Zdown:
                        return vectors.OrderByDescending((n) => GetPos(n).z).ToList();
                    case PathSortType.StartForwardZup:
                        if (transform != null)
                            return vectors.OrderBy((n) => transform.InverseTransformPoint(GetPos(n)).z).ToList();
                        else return vectors.OrderBy((n) => GetPos(n).z).ToList();
                    case PathSortType.StartForwardZdown:
                        if (transform != null)
                            return vectors.OrderByDescending((n) => transform.InverseTransformPoint(GetPos(n)).z).ToList();
                        else return vectors.OrderByDescending((n) => GetPos(n).z).ToList();
                    case PathSortType.Random:
                        return vectors.OrderBy((n) => UnityEngine.Random.value).ToList();

                }
                return vectors;
            }
            public static List<Vector3> SortForLast_Near(List<Vector3> vectors, Vector3 pos)
            {
                List<Vector3> reSort = new List<Vector3>(vectors);

                for (int i = 0; i < vectors.Count; i++)
                {
                    Vector3 last = i == 0 ? pos : reSort[i - 1];

                    float dis = 9999;
                    int minIndex = i;
                    for (int j = i; j < vectors.Count; j++)
                    {
                        float disN;
                        if ((disN = Vector3.Distance(reSort[j], last)) < dis)
                        {
                            minIndex = j;
                            dis = disN;
                        }
                    }
                    Vector3 cache = reSort[i];
                    reSort[i] = reSort[minIndex];
                    reSort[minIndex] = cache;
                }
                return reSort;
            }
            public static List<T> SortForLast<T>(List<T> ts, T start, Func<T, T, float> func)
            {
                List<T> reSort = new List<T>(ts);

                for (int i = 0; i < ts.Count; i++)
                {
                    T last = i == 0 ? start : reSort[i - 1];

                    float dis = 9999;
                    int minIndex = i;
                    for (int j = i; j < ts.Count; j++)
                    {
                        float disN;
                        if ((disN = func.Invoke(reSort[j], last)) < dis)
                        {
                            minIndex = j;
                            dis = disN;
                        }
                    }
                    T cache = reSort[i];
                    reSort[i] = reSort[minIndex];
                    reSort[minIndex] = cache;
                }
                return reSort;
            }
            public static List<T> SortForLast_Three<T>(List<T> ts, T start, Func<T, T, T, float> func)
            {
                List<T> reSort = new List<T>(ts);

                for (int i = 0; i < ts.Count; i++)
                {
                    T last = i == 0 ? start : reSort[i - 1];
                    T lastLast = i <= 2 ? start : reSort[i - 2];
                    float dis = 9999;
                    int minIndex = i;
                    for (int j = i; j < ts.Count; j++)
                    {
                        float disN;
                        if ((disN = func.Invoke(reSort[j], last, lastLast)) < dis)
                        {
                            minIndex = j;
                            dis = disN;
                        }
                    }
                    T cache = reSort[i];
                    reSort[i] = reSort[minIndex];
                    reSort[minIndex] = cache;
                }
                return reSort;
            }
        }
    }
}

