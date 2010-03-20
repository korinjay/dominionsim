using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DominionSim.VirtualCards
{
    /// <summary>
    /// Simply List wrapper for VirtualCards.
    /// </summary>
    class VirtualCardList : IList<VirtualCard>
    {
        /// <summary>
        /// Inner list
        /// </summary>
        private List<VirtualCard> mVirtualCardList = new List<VirtualCard>();

        /// <summary>
        /// Basic constructor
        /// </summary>
        public VirtualCardList()
        {

        }

        /// <summary>
        /// Constructor - make a new list starting with the given cards
        /// </summary>
        /// <param name="cardId">Card identifier we care about</param>
        /// <param name="count">Number of them</param>
        public VirtualCardList(CardIdentifier cardId, int count)
        {
            for (var i = 0; i < count; ++i)
            {
                Add(new VirtualCard(cardId));
            }
        }

        /// <summary>
        /// Convert the List to a read-only version of itself
        /// </summary>
        /// <returns>A read-only collection</returns>
        public System.Collections.ObjectModel.ReadOnlyCollection<VirtualCard> AsReadOnly()
        {
            return mVirtualCardList.AsReadOnly();
        }

        /// <summary>
        /// Return the CardIds in this List
        /// </summary>
        /// <returns>Enumerable through card Ids</returns>
        public IEnumerable<CardIdentifier> GetCardIds()
        {
            return mVirtualCardList.Select(vc => vc.CardId);
        }

        #region IList<VirtualCard> Members

        public int IndexOf(VirtualCard item)
        {
            return mVirtualCardList.IndexOf(item);
        }

        public void Insert(int index, VirtualCard item)
        {
            mVirtualCardList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            mVirtualCardList.RemoveAt(index);
        }

        public VirtualCard this[int index]
        {
            get
            {
                return mVirtualCardList[index];
            }
            set
            {
                mVirtualCardList[index] = value;
            }
        }

        #endregion

        #region ICollection<VirtualCard> Members

        public void Add(VirtualCard item)
        {
            mVirtualCardList.Add(item);
        }

        public void Clear()
        {
            mVirtualCardList.Clear();
        }

        public bool Contains(VirtualCard item)
        {
            return mVirtualCardList.Contains(item);
        }

        public void CopyTo(VirtualCard[] array, int arrayIndex)
        {
            mVirtualCardList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return mVirtualCardList.Count; }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(VirtualCard item)
        {
            return mVirtualCardList.Remove(item);
        }

        #endregion

        #region IEnumerable<VirtualCard> Members

        public IEnumerator<VirtualCard> GetEnumerator()
        {
            return mVirtualCardList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mVirtualCardList.GetEnumerator();
        }

        #endregion
    }
}
