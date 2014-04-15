// TraceLab - Software Traceability Instrument to Facilitate and Empower Traceability Research
// Copyright (C) 2012-2013 CoEST - National Science Foundation MRI-R2 Grant # CNS: 0959924
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLab.Core.Experiments;
using TraceLab.Core.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TraceLab.Core.Benchmarks
{
    public class BenchmarkSettingCollection<T> : KeyedCollection<T, BenchmarkItemSetting<T>>
    {
        protected override T GetKeyForItem(BenchmarkItemSetting<T> item)
        {
            if (item == null)
                throw new ArgumentNullException();

            return item.Item;
        }
    }

    public class BenchmarkItemSetting<T> : INotifyPropertyChanged
    {
        public BenchmarkItemSetting(T item, ItemSettingCollection candidates)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            if (candidates == null)
                throw new ArgumentNullException("candidates");

            Item = item;
            CandidateSettings = candidates;
        }

        private T m_item;
        public T Item
        {
            get { return m_item; }
            private set
            {
                m_item = value;
                NotifyPropertyChanged("Item");
            }
        }

        private ItemSettingCollection m_candidateSettings;
        public ItemSettingCollection CandidateSettings
        {
            get { return m_candidateSettings; }
            private set
            {
                m_candidateSettings = value;
                NotifyPropertyChanged("CandidateSettings");
            }
        }

        private ItemSetting m_selectedSetting;
        public ItemSetting SelectedSetting
        {
            get { return m_selectedSetting; }
            set
            {
                if (m_selectedSetting != value)
                {
                    m_selectedSetting = value;
                    NotifyPropertyChanged("SelectedSetting");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }

    //public class BenchmarkSetting<T>
    //{
    //    public BenchmarkSetting()
    //    {
    //    }

    //    private Dictionary<T, ItemSettingCollection> m_candidateMatchingItems = new Dictionary<T, ItemSettingCollection>();
    //    public Dictionary<T, ItemSettingCollection> CandidateMatchingItems
    //    {
    //        get { return m_candidateMatchingItems; }
    //    }

    //    private Dictionary<T, ItemSetting> m_selectedItemMatch = new Dictionary<T, ItemSetting>();
    //    public Dictionary<T, ItemSetting> SelectedItemMatch
    //    {
    //        get { return m_selectedItemMatch; }
    //    }

    //    internal void AddCandidateMatchingItemSettings(T item, ItemSettingCollection candidateItemSettings)
    //    {
    //        CandidateMatchingItems.Add(item, candidateItemSettings);
    //        SelectedItemMatch.Add(item, null);
    //    }

    //    internal void SelectMatch(T item, ItemSetting itemSetting)
    //    {
    //        if (CandidateMatchingItems.ContainsKey(item) == false)
    //            throw new ArgumentException("Incorrect item.");

    //        if (CandidateMatchingItems[item].Contains(itemSetting) == false)
    //            throw new ArgumentException("Incorrect item setting. The item setting does not exist in the candidate choices for the given item.");

    //        //get old item setting and do not include it in the export, if it is not selected by any other item
    //        ItemSetting oldItemSetting = SelectedItemMatch[item];
    //        Deselect(oldItemSetting);

    //        itemSetting.Include = true;
    //        SelectedItemMatch[item] = itemSetting;
    //    }

    //    private void Deselect(ItemSetting oldItemSetting)
    //    {
    //        if (oldItemSetting != null)
    //        {
    //            bool foundMatch = false;
    //            foreach (T i in SelectedItemMatch.Keys)
    //            {
    //                if (SelectedItemMatch[i].Equals(oldItemSetting) == true)
    //                {
    //                    foundMatch = true;
    //                }
    //            }
    //            //if there is no other item to be matched with this do not include it
    //            if (foundMatch == false)
    //            {
    //                oldItemSetting.Include = false;
    //            }
    //        }
    //    }
    //}
}
