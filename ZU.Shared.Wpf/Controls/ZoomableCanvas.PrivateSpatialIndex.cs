using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ZU.Semantic.Spatial;

namespace System.Windows.Controls
{
    partial class ZoomableCanvas
    {
        /// <summary>
        /// Private implementation of <see cref="ISpatialItemsSource"/> when the items source is not one.
        /// </summary>
        /// <remarks>
        /// This class efficiently implements a spatial index by internally using a PriorityQuadTree data structure.
        /// </remarks>
        // npacker: 'private' to 'public'
        public class PrivateSpatialIndex : ISpatialItemsSource
        {
            /// <summary>
            /// Private class that holds an index/bounds pair.
            /// </summary>
            /// <remarks>
            /// A Tuple could have been used instead except that we want Index to be mutable.
            /// </remarks>
            public class SpatialItem
            {
                public SpatialItem()
                {
                    Index = -1;
                    Bounds = Rect.Empty;
                }

                public int Index
                {
                    get;
                    set;
                }

                public Rect Bounds
                {
                    get;
                    set;
                }

                public override string ToString()
                {
                    return "Item[" + Index + "].Bounds = " + Bounds;
                }
            }

            /// <summary>
            /// We use a PriorityQuadTree to implement our spatial index.
            /// </summary>
            private readonly PriorityQuadTree<SpatialItem> _tree = new PriorityQuadTree<SpatialItem>();

            /// <summary>
            /// This is a list of all of the spatial items in the index.
            /// </summary>
            private readonly List<SpatialItem> _items = new List<SpatialItem>();

            /// <summary>
            /// Holds the accurate extent of all item bounds in the index.  This may be different from _tree.Extent.
            /// </summary>
            private Rect _extent = Rect.Empty;

            /// <summary>
            /// Holds the last query used in order to know when to raise the <see cref="QueryInvalidated"/> event.
            /// </summary>
            private Rect _lastQuery = Rect.Empty;

            /// <summary>
            /// Occurs when the value of the <see cref="Extent"/> property has changed.
            /// </summary>
            public event EventHandler ExtentChanged;

            /// <summary>
            /// Occurs when the results of the last query are no longer valid and should be re-queried.
            /// </summary>
            public event EventHandler QueryInvalidated;

            /// <summary>
            /// Get a list of the items that intersect the given bounds.
            /// </summary>
            /// <param name="bounds">The bounds to test.</param>
            /// <returns>
            /// List of zero or more items that intersect the given bounds, returned in the order given by the priority assigned during Insert.
            /// </returns>
            public IEnumerable<int> Query(Rect bounds)
            {
                _lastQuery = bounds;
                return _tree.GetItemsIntersecting(bounds).Select(i => i.Index);
            }

            /// <summary>
            /// Gets the computed minimum required rectangle to contain all of the items in the index.  This property is also settable for efficiency the future extent of the items is known.
            /// </summary>
            public Rect Extent
            {
                get
                {
                    if (_extent.IsEmpty)
                    {
                        foreach (var item in _items)
                        {
                            _extent.Union(item.Bounds);
                        }
                    }
                    return _extent;
                }
            }

            /// <summary>
            /// Gets or sets the bounds for the item with the given <paramref name="index"/>.
            /// </summary>
            /// <param name="index">The index of the item.</param>
            /// <returns>The bounds of the item, or <see cref="Rect.Empty"/> if the bounds are unknown.</returns>
            /// <remarks>
            /// Items with bounnds of <see cref="Rect.Empty"/> are always returned first from any query.
            /// </remarks>
            public Rect this[int index]
            {
                get
                {
                    return _items[index].Bounds;
                }
                set
                {
                    var item = _items[index];
                    var bounds = item.Bounds;
                    if (bounds != value)
                    {
                        _extent = Rect.Empty;
                        _tree.Remove(item, bounds);
                        _tree.Insert(item, value, value.IsEmpty ? Double.PositiveInfinity : value.Width + value.Height);
                        item.Bounds = value;

                        if (ExtentChanged != null)
                        {
                            ExtentChanged(this, EventArgs.Empty);
                        }

                        if (QueryInvalidated != null && (bounds.IntersectsWith(_lastQuery) || value.IntersectsWith(_lastQuery)))
                        {
                            QueryInvalidated(this, EventArgs.Empty);
                        }
                    }
                }
            }

            /// <summary>
            /// Adds or inserts the given <see cref="count"/> of items at the given <see cref="index"/>.
            /// </summary>
            /// <param name="index">The index at which to insert the items.</param>
            /// <param name="count">The number of items to insert.</param>
            /// <remarks>
            /// All items are inserted with bounds of <see cref="Rect.Empty"/>, meaning they will be returned from all queries.
            /// </remarks>
            public void InsertRange(int index, int count)
            {
                var items = new SpatialItem[count];
                for (int i = 0; i < count; i++)
                {
                    items[i] = new SpatialItem();
                    items[i].Index = index + i;
                    _tree.Insert(items[i], Rect.Empty, Double.PositiveInfinity);
                }
                _items.InsertRange(index, items);

                if (QueryInvalidated != null)
                {
                    QueryInvalidated(this, EventArgs.Empty);
                }
            }

            /// <summary>
            /// Removes the given <see cref="count"/> of items at the given <see cref="index"/>.
            /// </summary>
            /// <param name="index">The index at which to remove from.</param>
            /// <param name="count">The number of items to remove.</param>
            public void RemoveRange(int index, int count)
            {
                for (int i = index; i < _items.Count; i++)
                {
                    if (i < index + count)
                    {
                        _tree.Remove(_items[i], _items[i].Bounds);
                    }
                    else
                    {
                        _items[i].Index = i - count;
                    }
                }
                _items.RemoveRange(index, count);
                _extent = Rect.Empty;

                if (ExtentChanged != null)
                {
                    ExtentChanged(this, EventArgs.Empty);
                }

                if (QueryInvalidated != null)
                {
                    QueryInvalidated(this, EventArgs.Empty);
                }
            }

            /// <summary>
            /// Clears and resets the spatial index to hold the given <see cref="count"/> of items.
            /// </summary>
            /// <param name="count">The number of items within the index.</param>
            public void Reset(int count)
            {
                _extent = Rect.Empty;
                _items.Clear();
                InsertRange(0, count);

                if (ExtentChanged != null)
                {
                    ExtentChanged(this, EventArgs.Empty);
                }

                if (QueryInvalidated != null)
                {
                    QueryInvalidated(this, EventArgs.Empty);
                }
            }

            /// <summary>
            /// Optimizes the spatial index based on the current extent if optimization is warranted.
            /// </summary>
            public void Optimize()
            {
                var treeExtent = _tree.Extent;
                var realExtent = Extent;
                if (treeExtent.Top - realExtent.Top > treeExtent.Height ||
                    treeExtent.Left - realExtent.Left > treeExtent.Width ||
                    realExtent.Right - treeExtent.Right > treeExtent.Width ||
                    realExtent.Bottom - treeExtent.Bottom > treeExtent.Height)
                {
                    _tree.Extent = realExtent;

                    if (QueryInvalidated != null)
                    {
                        QueryInvalidated(this, EventArgs.Empty);
                    }
                }
            }
        }//class PrivateSpatialIndex
    }//class ZoomableCanvas
}//namespace
