using System.Collections.Generic;
using SEMERU.Types.Metrics;
using TraceLabSDK;

namespace SEMERU.UI.BoxPlotGUI
{
    [Component(Name = "SEMERU - (UI) Results BoxPlots - Example Loader",
        Description = "Stores example data to the Workspace for use with the BoxPlotGUI",
        Author = "Evan Moritz",
        Version = "1.0")]
    [IOSpec(IOSpecType.Output, "ListOfDatasetResults", typeof(List<DatasetResults>))]
    public class BoxPlotGUIExample : BaseComponent
    {
        public BoxPlotGUIExample(ComponentLogger log) : base(log) { }

        public override void Compute()
        {
            // main structure
            List<DatasetResults> data = new List<DatasetResults>();

            // one dataset
            List<Metric> ds1 = new List<Metric>();

            // tab 1 - All ranks
            List<SummaryData> ds1_tab1 = new List<SummaryData>();
            SummaryData ds1_tab1_sd1 = new SummaryData("VSM", new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 5, 6, 6, 7, 8, 8, 8, 11, 11, 13, 14, 16, 16, 17, 17, 18, 19, 19, 21, 21, 21, 23, 24, 24, 27, 28, 29, 31, 32, 32, 35, 37, 37, 39, 39, 46, 48, 49, 51, 51, 52, 56, 58, 59, 63, 64, 67, 69, 70, 79, 82, 84, 87, 87, 94, 95, 99, 100, 100, 106, 118, 120, 127, 144, 147, 147, 148, 153, 156, 158, 166, 171, 173, 185, 190, 197, 199, 211, 211, 219, 222, 225, 226, 252, 253, 271, 295, 301, 314, 321, 339, 354, 354, 357, 409, 422, 454, 455, 480, 511, 525, 580, 618, 655, 657, 657 });
            SummaryData ds1_tab1_sd2 = new SummaryData("LSI", new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 5, 6, 6, 6, 7, 7, 7, 7, 8, 8, 8, 9, 9, 10, 10, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 17, 19, 20, 20, 21, 21, 22, 22, 23, 23, 25, 26, 26, 27, 27, 28, 29, 32, 33, 36, 38, 39, 41, 44, 47, 50, 53, 53, 60, 61, 61, 62, 62, 63, 65, 65, 67, 73, 74, 77, 85, 94, 94, 94, 97, 97, 98, 101, 101, 103, 104, 111, 113, 115, 115, 117, 124, 124, 142, 143, 149, 155, 181, 200, 210, 223, 233, 243, 324, 336, 336, 380, 385, 401, 442, 491, 528, 537, 563, 617, 617 });
            // add data to tab
            ds1_tab1.Add(ds1_tab1_sd1);
            ds1_tab1.Add(ds1_tab1_sd2);
            // create Metric holder and add to dataset
            Metric ds1_m1 = new EffectivenessMetric(ds1_tab1, 0.003, "Wilcoxon signed-ranks test", "All Ranks");
            ds1.Add(ds1_m1);

            // tab 2 - Best ranks
            List<SummaryData> ds1_tab2 = new List<SummaryData>();
            SummaryData ds1_tab2_sd1 = new SummaryData("VSM", new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 5, 6, 6, 7, 8, 8, 8, 11, 11, 13, 14, 16, 16, 17, 17, 18, 19, 19, 21, 21, 21, 23, 24, 24, 27, 28, 29, 31, 32, 32, 35, 37, 37, 39, 39, 46, 48, 49, 51, 51, 52, 56, 58, 59, 63, 64, 67, 69, 70, 79, 82, 84, 87, 87, 94, 95, 99, 100, 100, 106, 118, 120, 127, 144, 147, 147, 148, 153, 156, 158, 166, 171, 173, 185, 190, 197, 199, 211, 211, 219, 222, 225, 226, 252, 253, 271, 295, 301, 314, 321, 339, 354, 354, 357, 409, 422, 454, 455, 480, 511, 525, 580, 618, 655, 657, 657 });
            SummaryData ds1_tab2_sd2 = new SummaryData("LSI", new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 5, 6, 6, 6, 7, 7, 7, 7, 8, 8, 8, 9, 9, 10, 10, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 17, 19, 20, 20, 21, 21, 22, 22, 23, 23, 25, 26, 26, 27, 27, 28, 29, 32, 33, 36, 38, 39, 41, 44, 47, 50, 53, 53, 60, 61, 61, 62, 62, 63, 65, 65, 67, 73, 74, 77, 85, 94, 94, 94, 97, 97, 98, 101, 101, 103, 104, 111, 113, 115, 115, 117, 124, 124, 142, 143, 149, 155, 181, 200, 210, 223, 233, 243, 324, 336, 336, 380, 385, 401, 442, 491, 528, 537, 563, 617, 617 });
            // add data to tab
            ds1_tab2.Add(ds1_tab2_sd1);
            ds1_tab2.Add(ds1_tab2_sd2);
            // create Metric holder and add to dataset
            Metric ds1_m2 = new EffectivenessMetric(ds1_tab2, 0.003, "Wilcoxon signed-ranks test", "Best Ranks");
            ds1.Add(ds1_m2);

            // add dataset to structure
            data.Add(new DatasetResults("jEdit4.3", ds1));


            // second dataset
            List<Metric> ds2 = new List<Metric>();

            // tab 1 - All ranks
            List<SummaryData> ds2_tab1 = new List<SummaryData>();
            SummaryData ds2_tab1_sd1 = new SummaryData("VSM", new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 5, 6, 6, 7, 8, 8, 8, 11, 11, 13, 14, 16, 16, 17, 17, 18, 19, 19, 21, 21, 21, 23, 24, 24, 27, 28, 29, 31, 32, 32, 35, 37, 37, 39, 39, 46, 48, 49, 51, 51, 52, 56, 58, 59, 63, 64, 67, 69, 70, 79, 82, 84, 87, 87, 94, 95, 99, 100, 100, 106, 118, 120, 127, 144, 147, 147, 148, 153, 156, 158, 166, 171, 173, 185, 190, 197, 199, 211, 211, 219, 222, 225, 226, 252, 253, 271, 295, 301, 314, 321, 339, 354, 354, 357, 409, 422, 454, 455, 480, 511, 525, 580, 618, 655, 657, 657 });
            SummaryData ds2_tab1_sd2 = new SummaryData("LSI", new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 5, 6, 6, 6, 7, 7, 7, 7, 8, 8, 8, 9, 9, 10, 10, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 17, 19, 20, 20, 21, 21, 22, 22, 23, 23, 25, 26, 26, 27, 27, 28, 29, 32, 33, 36, 38, 39, 41, 44, 47, 50, 53, 53, 60, 61, 61, 62, 62, 63, 65, 65, 67, 73, 74, 77, 85, 94, 94, 94, 97, 97, 98, 101, 101, 103, 104, 111, 113, 115, 115, 117, 124, 124, 142, 143, 149, 155, 181, 200, 210, 223, 233, 243, 324, 336, 336, 380, 385, 401, 442, 491, 528, 537, 563, 617, 617 });
            // add data to tab
            ds2_tab1.Add(ds2_tab1_sd1);
            ds2_tab1.Add(ds2_tab1_sd2);
            // create Metric holder and add to dataset
            Metric ds2_m1 = new EffectivenessMetric(ds2_tab1, 0.003, "Wilcoxon signed-ranks test", "All Ranks");
            ds2.Add(ds1_m1);

            // tab 2 - Best ranks
            List<SummaryData> ds2_tab2 = new List<SummaryData>();
            SummaryData ds2_tab2_sd1 = new SummaryData("VSM", new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 5, 6, 6, 7, 8, 8, 8, 11, 11, 13, 14, 16, 16, 17, 17, 18, 19, 19, 21, 21, 21, 23, 24, 24, 27, 28, 29, 31, 32, 32, 35, 37, 37, 39, 39, 46, 48, 49, 51, 51, 52, 56, 58, 59, 63, 64, 67, 69, 70, 79, 82, 84, 87, 87, 94, 95, 99, 100, 100, 106, 118, 120, 127, 144, 147, 147, 148, 153, 156, 158, 166, 171, 173, 185, 190, 197, 199, 211, 211, 219, 222, 225, 226, 252, 253, 271, 295, 301, 314, 321, 339, 354, 354, 357, 409, 422, 454, 455, 480, 511, 525, 580, 618, 655, 657, 657 });
            SummaryData ds2_tab2_sd2 = new SummaryData("LSI", new double[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 5, 6, 6, 6, 7, 7, 7, 7, 8, 8, 8, 9, 9, 10, 10, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 17, 19, 20, 20, 21, 21, 22, 22, 23, 23, 25, 26, 26, 27, 27, 28, 29, 32, 33, 36, 38, 39, 41, 44, 47, 50, 53, 53, 60, 61, 61, 62, 62, 63, 65, 65, 67, 73, 74, 77, 85, 94, 94, 94, 97, 97, 98, 101, 101, 103, 104, 111, 113, 115, 115, 117, 124, 124, 142, 143, 149, 155, 181, 200, 210, 223, 233, 243, 324, 336, 336, 380, 385, 401, 442, 491, 528, 537, 563, 617, 617 });
            // add data to tab
            ds2_tab2.Add(ds2_tab2_sd1);
            ds2_tab2.Add(ds2_tab2_sd2);
            // create Metric holder and add to dataset
            Metric ds2_m2 = new EffectivenessMetric(ds2_tab2, 0.003, "Wilcoxon signed-ranks test", "Best Ranks");
            ds2.Add(ds2_m2);

            // add dataset to structure
            data.Add(new DatasetResults("MyProg v1.2", ds2));

            // store to workspace
            Workspace.Store("ListOfDatasetResults", data);
        }
    }
}