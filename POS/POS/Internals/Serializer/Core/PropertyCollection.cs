using System.Collections.ObjectModel;

namespace Polenter.Serialization.Core
{
    /// <summary>
    ///   It represents some properties of an object, or some items of a collection/dictionary/array
    /// </summary>
    public sealed class PropertyCollection : Collection<Property>
    {
        ///<summary>
        ///  Parent property
        ///</summary>
        public Property Parent { get; set; }

        /// <summary>
        /// </summary>
        protected override void ClearItems()
        {
            foreach (Property item in this.Items)
            {
                item.Parent = null;
            }
            base.ClearItems();
        }

        /// <summary>
        /// </summary>
        /// <param name = "index"></param>
        /// <param name = "item"></param>
        protected override void InsertItem(int index, Property item)
        {
            base.InsertItem(index, item);
            item.Parent = this.Parent;
        }

        /// <summary>
        /// </summary>
        /// <param name = "index"></param>
        protected override void RemoveItem(int index)
        {
            this.Items[index].Parent = null;
            base.RemoveItem(index);
        }

        /// <summary>
        /// </summary>
        /// <param name = "index"></param>
        /// <param name = "item"></param>
        protected override void SetItem(int index, Property item)
        {
            this.Items[index].Parent = null;
            base.SetItem(index, item);
            item.Parent = this.Parent;
        }
    }
}