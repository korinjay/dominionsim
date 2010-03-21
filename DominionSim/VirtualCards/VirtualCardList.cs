using System;
using System.Collections.Generic;
using System.Linq;

namespace DominionSim.VirtualCards
{
    /// <summary>
    /// Simply List wrapper for VirtualCards.
    /// </summary>
    class VirtualCardList : IList<VirtualCard>, ICollection<VirtualCard>, IEnumerable<VirtualCard>
    {
        /// <summary>
        /// Inner list
        /// </summary>
        private IList<VirtualCard> mVirtualCardList;

        /// <summary>
        /// Basic constructor
        /// </summary>
        public VirtualCardList()
        {
            mVirtualCardList = new List<VirtualCard>();
        }

        /// <summary>
        /// Constructor - make a new list starting with the given cards
        /// </summary>
        /// <param name="cardId">Card identifier we care about</param>
        /// <param name="count">Number of them</param>
        public VirtualCardList(CardIdentifier cardId, int count) : this()
        {
            for (var i = 0; i < count; ++i)
            {
                Add(new VirtualCard(cardId));
            }
        }

        /// <summary>
        /// Constructor, taking a read-only list
        /// </summary>
        /// <param name="readOnlyList">The list as a read-only list</param>
        private VirtualCardList(System.Collections.ObjectModel.ReadOnlyCollection<VirtualCard> readOnlyList)
        {
            mVirtualCardList = readOnlyList;
        }

        /// <summary>
        /// Convert the List to a read-only version of itself
        /// </summary>
        /// <returns>A read-only version</returns>
        public VirtualCardList AsReadOnly()
        {
            if (IsReadOnly)
            {
                return this;
            }
            else
            {
                return new VirtualCardList((mVirtualCardList as List<VirtualCard>).AsReadOnly());
            }
        }

        /// <summary>
        /// Return the CardIds in this List
        /// </summary>
        /// <returns>Enumerable through card Ids</returns>
        public IEnumerable<CardIdentifier> GetCardIds()
        {
            return mVirtualCardList.Select(vc => vc.CardId);
        }

        /// <summary>
        /// Adds the elements of the specified list to the end of this VirtualCardList
        /// </summary>
        /// <param name="cards">List of cards to add</param>
        /// <exception cref="System.ArgumentNullException">If the argument is null</exception>
        public void AddRange(IEnumerable<VirtualCard> cards)
        {
            var enumer = cards.GetEnumerator();
            while (enumer.MoveNext())
            {
                mVirtualCardList.Add(enumer.Current);
            }
        }

        /// <summary>
        /// Return whether this VirtualCardList contains a VirtualCard with the given CardId
        /// </summary>
        /// <param name="cardId">Identifier</param>
        /// <returns>true if the VirtualCardList has any Cards matching that type</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if cardId is null</exception>
        public bool Contains(CardIdentifier cardId)
        {
            if (cardId == null) throw new ArgumentNullException("cardId");

            var enumerate = GetEnumerator();
            while (enumerate.MoveNext())
            {
                if (enumerate.Current.CardId == cardId)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Return the first VirtualCard in this VirtualCardList with the given CardIdentifier
        /// </summary>
        /// <param name="cardId">CardIdentifier of the card we want</param>
        /// <returns>First card with that identifier</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if cardId is null</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if no Card found with that CardId</exception>
        public VirtualCard First(CardIdentifier cardId)
        {
            if (cardId == null) throw new ArgumentNullException("cardId");

            var enumerate = GetEnumerator();
            while (enumerate.MoveNext())
            {
                if (enumerate.Current.CardId == cardId)
                {
                    return enumerate.Current;
                }
            }
            throw new InvalidOperationException("No VirtualCard found with the given cardId " + cardId.ToString());
        }

        public VirtualCardList Shuffle()
        {
            if (IsReadOnly) throw new InvalidOperationException("Cannot Shuffle a ReadOnly list");

            mVirtualCardList = Utility.Shuffle(mVirtualCardList as List<VirtualCard>);
            return this;
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
            get { return mVirtualCardList.IsReadOnly; }
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
