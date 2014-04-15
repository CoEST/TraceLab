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

namespace TraceLab.Core
{
    public static class Messages
    {
        public const string CompositeComponentDependencyNotFound = "Component {0}, (id:{1}) could not be loaded, because it depends on non-existing another component of the given id {2}";

        public const string SameComponents = "ID for component {0} in file {1} is the same as component {2} in file {3}.\nUsing {0} from file {1}";

        public const string DecisionErrorAmbigousChoiceOfTwoNodes = "Ambiguous choice of two nodes with same label.";

        public const string TypeDirectoryDoesNotExists = "Type directory {0} specified in settings does not exists, so the entry has been removed.";

        #region Progress statuses

        public const string ProgressExperimentProcessing = "Processing experiment...";

        #endregion

        #region Experiment Runner Messages

        public const string ExperimentRunnerSuccessMessage = "Experiment done!";
        public const string ExperimentRunnerErrorMessage = "An error occurred while processing";
        public const string ExperimentRunnerEarlyTerminationErrorMessage = "The experiment reached the end while some components were still executing.";
        public const string ExperimentRunnerInfiniteWaitDetected = "Infinite wait detected in the graph.";
        public const string ExperimentExecutionTerminated = "Experiment execution has been terminated.";

        #endregion

        #region ConfigWrapper

        public const string FailedToCopyFiles = "Failed to copy some referenced files";

        #endregion

        #region File Outside Directory Warnings

        public const string ExportExperimentOutsideComponentLibraryWarning = "The selected location is not within a component library. Exported component will not be usable within TraceLab. Do you want to continue?";
        public const string DefineBenchmarkOutsideBenchmarkDirectoryWarning = "The selected location is not within a benchmark directories. Defined benchmark will not be usable within TraceLab. Do you want to continue?";

        public const string DoYouWantToContinue = "Do you want to continue?";

        #endregion

        #region Is Experiment Valid

        public const string InvalidExperimentErrorMessage = "Errors occured in the validation of experiment. Please, fix the errors before exporting experiment.";
        public const string ExperimentNotValid = "There are errors in the experiment!";
        public const string CannotExportComponentWithEmptyNameErrorMessage = "Component name cannot be empty";
        
        #endregion

        #region Webservice messages

        public const string ContestFileDeserializationError = "Contest package deserialization has failed.";
        public const string ContestFileSaveError = "Contest package saving has failed. {0}";
        public const string ComponentTemplateNotFoundInContestError = "Component template has not been found in the downloaded contest.";
        public const string ExperimentResultsUnitnameNotDefined = "The benchmark info of downloaded file does not define Experiment Results Unitname. File is corrupted and cannot be executed.";
        public const string ContestDownloadError = "Contest download failed. {0}";

        public const string WebserviceUrlMalformedError = "The provided webservice url is malformed!";
        public const string WebserviceUrlEmptyError = "The webservice url cannot be empty!";
        public const string WebserviceAccessError = "Webservice access error! Error: {0}";

        public const string UploadingContestToWebsite = "Uploading contest to website...";
        public const string ContestPublished = "Contest has been published!";
        public const string RetrievingListOfContestsFailedWarning = "Retrieving online benchmarks failed. Possibly the webservice address is incorrect. Error message: {0}. Check, if webservice address provided in settings is correct.";

        public const string UploadingContestResultsToWebsite = "Uploading contest results to website...";
        public const string ContestResultsPublished = "Contest results has been published!";

        public const string PublishFailed = "Publish failed!";

        public const string ResponseEmpty = "Response from webservice was empty. Possibly server could not handle large files. Check server settings.";

        #endregion

        #region Defining Benchmark Messages

        public const string ExperimentResultsNotFoundOrNull = "Metrics field requires valid Experiment Results. The Experiment Results were not found in the m_workspace, or its value is null. "+
                                                              "Please, run your experiment before defining experiment and assure that Experiment Results are in the m_workspace.";

        public const string ExperimentResultsWithNoDatasets = "Metrics field requires valid Experiment Results. The Experiment Results found in the m_workspace do not have any datasets results defined. " +
                                                              "The Experiment Results needs to have at least one dataset result with at least one metric " +
                                                              "defined. Note, that values of metrics can be null, but metric names need to provided along with their descriptions.";
        public const string ExperimentResultsWithNoMetric = "Metrics field requires valid Experiment Results. The Experiment Results found in the m_workspace has dataset with no metric defined. " +
                                                              "The Experiment Results needs to have at least one dataset result with at least one metric " +
                                                              "defined. Please, assure that all datasets have metric defined. "+
                                                              "Note, that values of metrics can be null, but metric names need to provided along with their descriptions.";

        #endregion

        public const string ClosingUnsavedDocumentWarning = "Experiment has been modified.  Are you sure you want to close without saving?";

        public const string ShouldOverwriteFileQuestion = "The file {0} already exists. Do you want to continue and overwrite existing file?";

        public const string ConfirmRemoveSelectedNodes = "Are you sure, that you want delete selected nodes from the experiment?";
    }
}
