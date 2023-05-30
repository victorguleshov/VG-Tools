using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VG.Extensions
{
    public class SetViewsWrapper<T0, T1> where T0 : Component
    {
        public readonly List<(IList<T0>, IList<T1>)> viewsAndDataPairs;

        public SetViewsWrapper((IList<T0> views, IList<T1> datas) valueTuple) =>
            viewsAndDataPairs = new List<(IList<T0>, IList<T1>)> { valueTuple };

        public SetViewsWrapper<T0, T1> ByFunc(Func<T0, T1, int, bool> func)
        {
            foreach (var (views, datas) in viewsAndDataPairs) views.SetViews(datas, func);
            return this;
        }

        public SetViewsWrapper<T0, T1> ByAction(Action<T0, T1, int> action)
        {
            foreach (var (views, datas) in viewsAndDataPairs) views.SetViews(datas, action);
            return this;
        }

        public SetViewsWrapper<T0, T1> AndViews(List<T0> views, List<T1> datas)
        {
            viewsAndDataPairs.Add((views, datas));
            return this;
        }

        public SetViewsWrapper<T0, T1> AndViews(List<T0> views, List<T1> datas, Func<T0, T1, int, bool> func)
        {
            AndViews(views, datas);
            ByFunc(func);
            return this;
        }

        public SetViewsWrapper<T0, T1> AndViews(List<T0> views, List<T1> datas, Action<T0, T1, int> action)
        {
            AndViews(views, datas);
            ByAction(action);
            return this;
        }
    }

    public static class ViewExtensions
    {
        public static SetViewsWrapper<T0, T1> SetViews<T0, T1>(IList<T0> views, IList<T1> datas) where T0 : Component =>
            new((views, datas));

        /// <param name="views">List of views</param>
        /// <param name="datas">List of datas</param>
        /// <param name="func">Callback</param>
        public static bool SetViews<T0, T1>(this IList<T0> views, IList<T1> datas, Func<T0, T1, int, bool> func)
            where T0 : Component
        {
            if (views.Count == 0) return false;
            var maxSiblingIndexDelta =
                views[0].transform.parent.childCount - views.Max(x => x.transform.GetSiblingIndex());
            var dataIndex = 0;
            var viewIndex = 0;
            var result = false;

            while (viewIndex < views.Count)
            {
                var view = views[viewIndex];

                if (viewIndex < datas.Count && dataIndex < datas.Count)
                {
                    var data = datas[dataIndex];

                    view.gameObject.SetActive(true);

                    if (!func(view, data, dataIndex))
                        viewIndex--;
                    else
                        result = true;
                }
                else
                {
                    view.gameObject.SetActive(false);
                }

                viewIndex++;
                dataIndex++;
            }

            T0 newView = null;
            var savedViewsCount = views.Count;

            while (dataIndex < datas.Count)
            {
                if (newView == null)
                {
                    newView = Object.Instantiate(views[dataIndex % savedViewsCount],
                        views[dataIndex % savedViewsCount].transform.parent);
                    newView.transform.SetSiblingIndex(newView.transform.parent.childCount - maxSiblingIndexDelta);
                    views.Add(newView);
                }

                var data = datas[dataIndex];

                newView.gameObject.SetActive(true);
                if (func(newView, data, dataIndex))
                {
                    newView = null;
                    result = true;
                }

                dataIndex++;
            }

            if (newView != null) newView.gameObject.SetActive(false);

            return result;
        }

        /// <param name="views">List of views</param>
        /// <param name="datas">List of datas</param>
        /// <param name="action">Callback</param>
        public static bool SetViews<T0, T1>(this IList<T0> views, IList<T1> datas, Action<T0, T1, int> action)
            where T0 : Component
        {
            if (views.Count == 0) return false;
            var maxSiblingIndexDelta =
                views[0].transform.parent.childCount - views.Max(x => x.transform.GetSiblingIndex());
            var dataIndex = 0;
            var viewIndex = 0;
            var result = false;

            while (viewIndex < views.Count)
            {
                var view = views[viewIndex];

                if (viewIndex < datas.Count && dataIndex < datas.Count)
                {
                    var data = datas[dataIndex];

                    view.gameObject.SetActive(true);

                    action(view, data, dataIndex);
                    result = true;
                }
                else
                {
                    view.gameObject.SetActive(false);
                }

                viewIndex++;
                dataIndex++;
            }

            var savedViewsCount = views.Count;

            while (dataIndex < datas.Count)
            {
                var newView = Object.Instantiate(views[dataIndex % savedViewsCount],
                    views[dataIndex % savedViewsCount].transform.parent);
                newView.transform.SetSiblingIndex(newView.transform.parent.childCount - maxSiblingIndexDelta);
                views.Add(newView);

                var data = datas[dataIndex];

                newView.gameObject.SetActive(true);
                action.Invoke(newView, data, dataIndex);

                result = true;

                dataIndex++;
            }

            return result;
        }

        /// <param name="views">List of views</param>
        /// <param name="count">Count of views</param>
        /// <param name="action">Callback</param>
        public static bool SetViews<T0>(this IList<T0> views, int count, Action<T0, int> action = null)
            where T0 : Component
        {
            if (views.Count == 0) return false;
            var maxSiblingIndexDelta =
                views[0].transform.parent.childCount - views.Max(x => x.transform.GetSiblingIndex());
            var dataIndex = 0;
            var viewIndex = 0;
            var result = false;

            while (viewIndex < views.Count)
            {
                var view = views[viewIndex];

                if (viewIndex < count && dataIndex < count)
                {
                    view.gameObject.SetActive(true);

                    action?.Invoke(view, dataIndex);
                    result = true;
                }
                else
                {
                    view.gameObject.SetActive(false);
                }

                viewIndex++;
                dataIndex++;
            }

            var savedViewsCount = views.Count;

            while (dataIndex < count)
            {
                var newView = Object.Instantiate(views[dataIndex % savedViewsCount],
                    views[dataIndex % savedViewsCount].transform.parent);
                newView.transform.SetSiblingIndex(newView.transform.parent.childCount - maxSiblingIndexDelta);
                views.Add(newView);

                newView.gameObject.SetActive(true);
                action?.Invoke(newView, dataIndex);

                result = true;

                dataIndex++;
            }

            return result;
        }

        /// <param name="views">List of views</param>
        /// <param name="count">Count of views</param>
        /// <param name="func">Callback</param>
        public static bool SetViews<T0>(this IList<T0> views, int count, Func<T0, int, bool> func) where T0 : Component
        {
            if (views.Count == 0) return false;
            var maxSiblingIndexDelta =
                views[0].transform.parent.childCount - views.Max(x => x.transform.GetSiblingIndex());
            var dataIndex = 0;
            var viewIndex = 0;
            var result = false;

            while (viewIndex < views.Count)
            {
                var view = views[viewIndex];

                if (viewIndex < count && dataIndex < count)
                {
                    view.gameObject.SetActive(true);

                    if (!func(view, dataIndex))
                        viewIndex--;
                    else
                        result = true;
                }
                else
                {
                    view.gameObject.SetActive(false);
                }

                viewIndex++;
                dataIndex++;
            }

            T0 newView = null;
            var savedViewsCount = views.Count;

            while (dataIndex < count)
            {
                if (newView == null)
                {
                    newView = Object.Instantiate(views[dataIndex % savedViewsCount],
                        views[dataIndex % savedViewsCount].transform.parent);
                    newView.transform.SetSiblingIndex(newView.transform.parent.childCount - maxSiblingIndexDelta);
                    views.Add(newView);
                }

                newView.gameObject.SetActive(true);
                if (func(newView, dataIndex))
                {
                    newView = null;
                    result = true;
                }

                dataIndex++;
            }

            if (newView != null) newView.gameObject.SetActive(false);

            return result;
        }

        public static void AddViews<T0>(this List<T0> views, int count, Action<T0> action = null) where T0 : Component
        {
            var data = new List<int>();
            for (var i = 0; i < count; i++) data.Add(i);

            AddViews(views, data, action: (view, _, __) => action?.Invoke(view));
        }

        public static void AddViews<T0, T1>(
            List<T0> views, List<T1> datas,
            Func<T0, T1, int, bool> func = null,
            Action<T0, T1, int> action = null
        ) where T0 : Component
        {
            var dataIndex = 0;

            T0 newView = null;
            var savedViewsCount = views.Count;
            while (dataIndex < datas.Count)
            {
                if (newView == null)
                {
                    newView = Object.Instantiate(views[dataIndex % savedViewsCount],
                        views[dataIndex % savedViewsCount].transform.parent);
                    views.Add(newView);
                }

                var data = datas[dataIndex];

                newView.gameObject.SetActive(true);

                if (func != null)
                    if (func.Invoke(newView, data, dataIndex))
                        newView = null;

                if (action != null)
                {
                    action.Invoke(newView, data, dataIndex);
                    newView = null;
                }

                dataIndex++;
            }

            if (newView != null) newView.gameObject.SetActive(false);
        }

        public static void RemoveViews<T0>(this List<T0> views, int count) where T0 : Component
        {
            var dataIndex = 0;

            while (dataIndex < count)
            {
                var view = views[^1];
                Object.Destroy(view.gameObject);
                views.RemoveAt(views.Count - 1);

                dataIndex++;
            }
        }
    }
}