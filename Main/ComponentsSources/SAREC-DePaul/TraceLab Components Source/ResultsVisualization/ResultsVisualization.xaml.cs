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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using TraceLabSDK;
using TraceLabSDK.Types;
using TraceLabSDK.Types.Contests;

using ResultsVisualization.Charts;

namespace ResultsVisualization
{
    /// <summary>
    /// Results Visualization Window Logic
    /// </summary>
    public partial class ResultsVisualizationWindow : Window
    {
        #region Members

        /// <summary>
        /// Data structure holding metric results data for all datasets
        /// </summary>
        private SortedDictionary<string, Dictionary<string, Chart>> allDatasetMetricCharts;

        /// <summary>
        /// Data structure holding metric results data for selected dataset
        /// </summary>
        private Dictionary<string, Chart> currentDatasetMetricCharts = null;

        /// <summary>
        /// Current selected tab
        /// </summary>
        private TabItem currentTabItem = null;

        /// <summary>
        /// Current displayed chart
        /// </summary>
        private Chart currentChart = null;

        #endregion

        #region Methods

        /// <summary>
        /// Extracts necessary data from TLExperimentsResultsCollection for creating individual charts
        /// </summary>
        /// <param name="allResults">Results across all datasets and metrics.</param>
        public ResultsVisualizationWindow(SortedDictionary<string, Dictionary<string, Chart>> allResults)
        {
            this.InitializeComponent();

            this.allDatasetMetricCharts = allResults;

            this.FillDropDownList();
        }

        /// <summary>
        /// Fills dropdown menu with all dataset names
        /// </summary>
        private void FillDropDownList()
        {
            // Do not display dropdown menu when there's only one dataset
            if (this.allDatasetMetricCharts.Count <= 1)
            {
                this.DatasetLabel.Visibility = System.Windows.Visibility.Hidden;
                this.DatasetDropDown.Visibility = System.Windows.Visibility.Hidden;
            }

            foreach (string datasetName in this.allDatasetMetricCharts.Keys)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = datasetName;
                this.DatasetDropDown.Items.Add(item);
            }

            this.DatasetDropDown.SelectedIndex = 0;
        }

        /// <summary>
        /// Display the tabs and charts for the selected dataset
        /// </summary>
        private void GenerateTabs()
        {
            // Removing parent-children relationship with old tabs
            foreach (TabItem item in TabStrip.Items)
            {
                item.Content = null;
            }
            this.TabStrip.Items.Clear();

            // Creating new tabs with tooltips
            bool firstChart = true;
            foreach (Chart c in this.currentDatasetMetricCharts.Values)
            {
                TabItem item = new TabItem();

                TextBlock titleBlock = new TextBlock();
                titleBlock.Text = c.Title;

                TextBlock tooltipBlock = new TextBlock();
                tooltipBlock.TextWrapping = TextWrapping.Wrap;
                tooltipBlock.Width = 300.0d;
                tooltipBlock.Text = c.Description;

                ToolTip toolTip = new ToolTip();
                toolTip.Content = tooltipBlock;

                titleBlock.ToolTip = toolTip;
                item.Header = titleBlock;

                if (firstChart)
                {
                    this.currentTabItem = item;
                    this.currentChart = c;
                    item.Content = xamlPanel;
                    firstChart = false;
                }

                this.TabStrip.Items.Add(item);
            }

            this.TabStrip.SelectedIndex = 0;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event handler for changing selected item in dropdown menu (datasets)
        /// </summary>
        void DatasetChanged(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)DatasetDropDown.SelectedItem;

            if (item != null)
            {
                string datasetName = item.Content.ToString();

                Dictionary<string, Chart> charts;

                if (this.allDatasetMetricCharts.TryGetValue(datasetName, out charts))
                {
                    this.currentDatasetMetricCharts = charts;
                    this.GenerateTabs();
                }
            }
        }

        /// <summary>
        /// Event handler for changing selected tab in tabstrip (metrics)
        /// </summary>
        void MetricChanged(object sender, RoutedEventArgs e)
        {
            if (this.currentTabItem != null)
            {
                this.currentTabItem.Content = null;
            }

            if (this.TabStrip.SelectedItem != null)
            {
                TabItem tabItem = (TabItem)this.TabStrip.SelectedItem;
                TextBlock textBlock = (TextBlock)tabItem.Header;
                string metricName = textBlock.Text;
                Chart chart;

                // Find the corresponding chart to be displayed
                if (this.currentDatasetMetricCharts != null &&
                    this.currentDatasetMetricCharts.TryGetValue(metricName, out chart))
                {
                    xamlTextCanvas.Children.RemoveRange(1, xamlTextCanvas.Children.Count - 1);
                    xamlChartCanvas.Children.Clear();
                    xamlLegendCanvas.Children.Clear();

                    this.currentTabItem = tabItem;
                    this.currentChart = chart;
                    tabItem.Content = xamlPanel;

                    chart.Draw(xamlTextCanvas, xamlChartCanvas, xamlLegendCanvas);
                }

                this.currentTabItem = tabItem;
            }
        }

        /// <summary>
        /// Event handler for re-drawing window (chart) when it is re-sized
        /// </summary>
        void WindowsReSized(object sender, SizeChangedEventArgs e)
        {
            double width = MainGrid.ActualWidth - 180.0d;
            double height = MainGrid.ActualHeight - 80.0d;

            xamlTextCanvas.Width = width;
            xamlTextCanvas.Height = height;

            xamlTextCanvas.Children.RemoveRange(1, xamlTextCanvas.Children.Count - 1);
            xamlChartCanvas.Children.Clear();
            xamlLegendCanvas.Children.Clear();

            if (this.currentChart != null)
            {
                this.currentChart.Draw(xamlTextCanvas, xamlChartCanvas, xamlLegendCanvas);
            }
        }

        #endregion
    }
}
