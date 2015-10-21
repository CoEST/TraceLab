using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TraceLab.Core.Components;
using TraceLab.Core.Utilities;

namespace TraceLab.Core.Experiments
{
    /// <summary>
    /// The Component Factory creates components metadata and adds them to the experiment based on given metadata definition. 
    /// In other words, when user drags and drops the component definition from library into experiment canvas it creates the 
    /// component metadata object based on the metadata definition and assigns it to the vertex node. Such metadata object 
    /// keep the reference to its definition, and in addition it has properties that hold the values.
    /// for example configuration values (ConfigWrapper class), and IO specification (IOSpec class)
    /// 
    /// Depending on the provided metadata definition, it will create Component, Composite Component, Decision, or Loop Scope.
    /// </summary>
    public static class ComponentFactory
    {
        /// <summary>
        /// Adds the component from definition to experiment.
        /// </summary>
        /// <param name="experiment">The experiment to which new component is added.</param>
        /// <param name="metadataDefinition">The metadata definition that defines newly added component.</param>
        /// <param name="positionX">The position X at where new component is added</param>
        /// <param name="positionY">The position Y at where new comopnent is added</param>
        /// <returns>The newly constucted node</returns>
        public static ExperimentNode AddComponentFromDefinitionToExperiment(IEditableExperiment experiment, MetadataDefinition metadataDefinition, double positionX, double positionY)
        {
            if (metadataDefinition == null)
                throw new ArgumentNullException("metadataDefinition");

            ExperimentNode newNode = null;

            SerializedVertexData data = new SerializedVertexData();
            data.X = positionX;
            data.Y = positionY;

            // case 1. Primitive Component
            bool isCreated = TryCreateComponent(experiment, metadataDefinition, data, out newNode);

            // case 2. Composite Component
            if(!isCreated)
                isCreated = TryCreateCompositeComponent(experiment, metadataDefinition, data, out newNode);

            // case 3. Decision
            if (!isCreated)
                isCreated = TryCreateDecisionComponent(experiment, metadataDefinition, data, out newNode);

            // case 4. Loop
            if (!isCreated)
                isCreated = TryCreateLoopComponent(experiment, metadataDefinition, data, out newNode);

            // HERZUM SPRINT 0.0
            // case 5. Scope
            if (!isCreated)
                isCreated = TryCreateScopeComponent(experiment, metadataDefinition, data, out newNode);
            // END HERZUM 0.0

            // HERZUM SPRINT 1.0
            if (!isCreated)
                isCreated = TryCreateComment(experiment, metadataDefinition, data, out newNode);
            // END HERZUM SPRINT 1.0

            // HERZUM SPRINT 2.0: TLAB-65
            // case 5. Challenge
            if (!isCreated)
                isCreated = TryCreateChallengeComponent(experiment, metadataDefinition, data, out newNode);
            // END HERZUM SPRINT 2.0: TLAB-65

            
            return newNode;
        }

        // HERZUM SPRINT 1.0
        private static bool TryCreateComment(IEditableExperiment experiment, MetadataDefinition metadataDefinition, 
                                               SerializedVertexData data, out ExperimentNode newCreatedNode)
        {
            bool isCreated = false;
            newCreatedNode = null;

            CommentMetadataDefinition commentMetadataDefinition = metadataDefinition as CommentMetadataDefinition;
            if (commentMetadataDefinition != null)
            {
                data.Metadata = new CommentMetadata(commentMetadataDefinition.Label);
                string commentNodeId = Guid.NewGuid().ToString();

                // HERZUM SPRINT 1.2 COMMENT
                // newCreatedNode = new CommentNode(commentNodeId, data);
                var data_with_size = new SerializedVertexDataWithSize();
                data_with_size.X = data.X;
                data_with_size.Y = data.Y;
                data_with_size.Metadata = data.Metadata;
                data_with_size.Width  = 160;
                data_with_size.Height = 160;
                data_with_size.WidgetStatus = "normal";
                newCreatedNode = new CommentNode(commentNodeId, data_with_size);
                // END SPRINT 1.2 COMMENT

                experiment.AddVertex(newCreatedNode);

                //set the log settings of the new component
                experiment.SetLogLevelSettings(newCreatedNode);

                isCreated = true;
            }

            return isCreated;
        }
        // END HERZUM SPRINT 1.0

        /// <summary>
        /// Attempts to create the primitive component. Component is created only if definition is a ComponentMetadataDefinition.
        /// Otherwise method returns false, and null node.
        /// </summary>
        /// <param name="experiment">The experiment to which new component is added.</param>
        /// <param name="metadataDefinition">The metadata definition which component is created based on.</param>
        /// <param name="data">The data containing position of new vertex</param>
        /// <param name="newCreatedNode">Output of new created node; null if metadatadefinition was not a ComponentMetadataDefinition</param>
        /// <returns>
        /// true if metadatadefinition was a ComponentMetadataDefinition and node has been created, otherwise false
        /// </returns>
        private static bool TryCreateComponent(IEditableExperiment experiment, MetadataDefinition metadataDefinition, 
                                               SerializedVertexData data, out ExperimentNode newCreatedNode)
        {
            bool isCreated = false;
            newCreatedNode = null;

            ComponentMetadataDefinition componentMetadataDefinition = metadataDefinition as ComponentMetadataDefinition;
            if (componentMetadataDefinition != null)
            {
                data.Metadata = new ComponentMetadata(componentMetadataDefinition, System.IO.Path.GetDirectoryName(experiment.ExperimentInfo.FilePath));
                string componentNodeId = Guid.NewGuid().ToString();
                newCreatedNode = new ComponentNode(componentNodeId, data);
                experiment.AddVertex(newCreatedNode);

                //set the log settings of the new component
                experiment.SetLogLevelSettings(newCreatedNode);

                isCreated = true;
            }

            return isCreated;
        }

        /// <summary>
        /// Attempts to create the composite component. Component is created only if definition is a CompositeComponentMetadataDefinition.
        /// Otherwise method returns false, and null node.
        /// </summary>
        /// <param name="experiment">The experiment to which new component is added.</param>
        /// <param name="metadataDefinition">The metadata definition which component is created based on.</param>
        /// <param name="data">The data containing position of new vertex</param>
        /// <param name="newCreatedNode">Output of new created node; null if metadatadefinition was not a CompositeComponentMetadataDefinition</param>
        /// <returns>
        /// true if metadatadefinition was a CompositeComponentMetadataDefinition and node has been created, otherwise false
        /// </returns>
        private static bool TryCreateCompositeComponent(IEditableExperiment experiment, MetadataDefinition metadataDefinition, SerializedVertexData data,
                                                       out ExperimentNode newCreatedNode)
        {
            bool isCreated = false;
            newCreatedNode = null;

            CompositeComponentMetadataDefinition compositeComponentMetadataDefinition = metadataDefinition as CompositeComponentMetadataDefinition;
            if (compositeComponentMetadataDefinition != null)
            {
                data.Metadata = new CompositeComponentMetadata(compositeComponentMetadataDefinition, System.IO.Path.GetDirectoryName(experiment.ExperimentInfo.FilePath));
                string componentNodeId = Guid.NewGuid().ToString();
                string componentName = data.Metadata.Label;
                newCreatedNode = new CompositeComponentNode(componentNodeId, data, experiment.Settings);
                experiment.AddVertex(newCreatedNode);

                //set the log settings of the new component
                experiment.SetLogLevelSettings(newCreatedNode);

                isCreated = true;
            }

            return isCreated;
        }

        #region Decision node with scope

        /// <summary>
        /// Attempts to create the composite component. Component is created only if definition is a DecisionMetadataDefinition.
        /// Otherwise method returns false, and null node.
        /// </summary>
        /// <param name="experiment">The experiment to which new component is added.</param>
        /// <param name="metadataDefinition">The metadata definition which component is created based on.</param>
        /// <param name="data">The data containing position of new vertex</param>
        /// <param name="newCreatedNode">Output of new created node; null if metadatadefinition was not a DecisionMetadataDefinition</param>
        /// <returns>
        /// true if metadatadefinition was a DecisionMetadataDefinition and node has been created, otherwise false
        /// </returns>
        private static bool TryCreateDecisionComponent(IEditableExperiment experiment, MetadataDefinition metadataDefinition, SerializedVertexData data, 
                                                       out ExperimentNode newCreatedNode)
        {
            bool isCreated = false;
            newCreatedNode = null;

            DecisionMetadataDefinition decisionMetadataDefinition = metadataDefinition as DecisionMetadataDefinition;
            if (decisionMetadataDefinition != null)
            {
                newCreatedNode = CreateDecisionComponent(experiment, decisionMetadataDefinition, data);
                isCreated = true;
            }

            return isCreated;
        }

        /// <summary>
        /// Creates the decision component.
        /// </summary>
        /// <param name="experiment">The experiment to which new component is added.</param>
        /// <param name="decisionMetadataDefinition">The decision metadata definition.</param>
        /// <param name="data">The data containing position of new vertex</param>
        /// <returns>Newly added desicion node (note, that in case of decision additional nodes are constructed, two scopes and exit decision node). They are not returned.</returns>
        private static ExperimentDecisionNode CreateDecisionComponent(IEditableExperiment experiment, DecisionMetadataDefinition decisionMetadataDefinition, SerializedVertexData data)
        {
            data.Metadata = new DecisionMetadata(decisionMetadataDefinition.Label);
            
            ExperimentDecisionNode decisionNode = new ExperimentDecisionNode(Guid.NewGuid().ToString(), data);
            experiment.AddVertex(decisionNode);

            if (decisionMetadataDefinition.ID == DecisionMetadataDefinition.DecisionGuid)
            {
                //generate corresponding scopes and exit nodes
                GenerateScopesAndExit(experiment, data, decisionNode);
            }
            //otherwise it is GoTo decision that does not have corresponding scopes, so simply return it

            return decisionNode;
        }

        private static void GenerateScopesAndExit(IEditableExperiment experiment, SerializedVertexData data, ExperimentDecisionNode decisionNode)
        {
            ExitDecisionNode exitDecisionNode = GenerateExitDecisionNode(data.X, data.Y + 240);
            experiment.AddVertex(exitDecisionNode);

            //add invisible fixed connection between decision and exit - so that if scope can be skipped upon condition
            experiment.AddFixedConnection(decisionNode, exitDecisionNode, true);

            //set the log settings of all new nodes
            experiment.SetLogLevelSettings(decisionNode);
            experiment.SetLogLevelSettings(exitDecisionNode);

            AddScopeToDecision("Scope 1", data.X - 90, data.Y + 120, decisionNode, exitDecisionNode, experiment);
            AddScopeToDecision("Scope 2", data.X + 90, data.Y + 120, decisionNode, exitDecisionNode, experiment);
        }

        /// <summary>
        /// Adds the scope to decision.
        /// Default scope size is 160 by 160
        /// </summary>
        /// <param name="label">The label.</param>
        /// <param name="positionX">The center X position of the scope.</param>
        /// <param name="positionY">The center Y position of the scope.</param>
        /// <param name="decisionNode">The decision node.</param>
        /// <param name="exitDecisionNode">The exit decision node.</param>
        /// <param name="experiment">The experiment.</param>
        public static void AddScopeToDecision(string label, double positionX, double positionY, ExperimentDecisionNode decisionNode, ExitDecisionNode exitDecisionNode, IEditableExperiment experiment, double width = 160, double height = 160)
        {
            ScopeNode scopeNode = GenerateScopeNode(label, experiment, positionX, positionY, width, height);
            
            //set decision and exit decision references
            //(also note that ExperimentFactory also sets it when deseralizing experiment)
            scopeNode.DecisionNode = decisionNode;
            scopeNode.ExitDecisionNode = exitDecisionNode;
            
            experiment.AddVertex(scopeNode);
            experiment.AddFixedConnection(decisionNode, scopeNode, true);
            experiment.AddFixedConnection(scopeNode, exitDecisionNode, true);

            experiment.SetLogLevelSettings(scopeNode);
        }

        /// <summary>
        /// Generates the exit decision node.
        /// </summary>
        /// <param name="positionX">The position X.</param>
        /// <param name="positionY">The position Y.</param>
        /// <returns></returns>
        private static ExitDecisionNode GenerateExitDecisionNode(double positionX, double positionY)
        {
            var data = new SerializedVertexData();
            data.X = positionX;
            data.Y = positionY;
            data.Metadata = new ExitDecisionMetadata();
            var exitDecisionNode = new ExitDecisionNode(Guid.NewGuid().ToString(), data);
            return exitDecisionNode;
        }

        /// <summary>
        /// Generates the scope node at the given position.
        /// </summary>
        /// <param name="scopeName">Name of the scope.</param>
        /// <param name="experiment">The experiment to which new component is added.</param>
        /// <param name="positionX">The position X.</param>
        /// <param name="positionY">The position Y.</param>
        /// <returns></returns>
        private static ScopeNode GenerateScopeNode(string scopeName, IExperiment experiment, double positionX, double positionY, double width, double height)
        {
            var data = new SerializedVertexDataWithSize();
            data.X = positionX;
            data.Y = positionY;
            data.Width = width;
            data.Height = height;

            string componentId = Guid.NewGuid().ToString();

            var componentGraph = new CompositeComponentEditableGraph(true);

            if (componentGraph.References != null)
            {
                componentGraph.References = experiment.References.CopyCollection();
            }

            data.Metadata = new ScopeMetadata(componentGraph, scopeName, System.IO.Path.GetDirectoryName(experiment.ExperimentInfo.FilePath));
            
            string componentNodeId = Guid.NewGuid().ToString();
            string componentName = data.Metadata.Label;

            var scopeNode = new ScopeNode(Guid.NewGuid().ToString(), data);
            return scopeNode;
        }
        
        #endregion

        #region Loop Component

        /// <summary>
        /// Attempts to create the composite component. Component is created only if definition is a LoopMetadataDefinition.
        /// Otherwise method returns false, and null node.
        /// </summary>
        /// <param name="experiment">The experiment to which new component is added.</param>
        /// <param name="metadataDefinition">The metadata definition which component is created based on.</param>
        /// <param name="data">The data containing position of new vertex</param>
        /// <param name="newCreatedNode">Output of new created node; null if metadatadefinition was not a LoopMetadataDefinition</param>
        /// <returns>
        /// true if metadatadefinition was a LoopMetadataDefinition and node has been created, otherwise false
        /// </returns>
        private static bool TryCreateLoopComponent(IEditableExperiment experiment, MetadataDefinition metadataDefinition, SerializedVertexData data,
                                                       out ExperimentNode newCreatedNode)
        {
            bool isCreated = false;
            newCreatedNode = null;

            LoopMetadataDefinition loopMetadataDefinition = metadataDefinition as LoopMetadataDefinition;
            if (loopMetadataDefinition != null)
            {
                newCreatedNode = CreateLoopScopeNode(loopMetadataDefinition.Label, experiment, data.X, data.Y);
                isCreated = true;
            }

            return isCreated;
        }

        /// <summary>
        /// Generates the scope node.
        /// </summary>
        /// <param name="scopeName">Name of the scope.</param>
        /// <param name="experiment">The experiment to which new component is added.</param>
        /// <param name="positionX">The position X.</param>
        /// <param name="positionY">The position Y.</param>
        /// <returns></returns>
        private static LoopScopeNode CreateLoopScopeNode(string scopeName, IEditableExperiment experiment, double positionX, double positionY)
        {
            var data = new SerializedVertexDataWithSize();
            data.X = positionX;
            data.Y = positionY;
            data.Width = 160;
            data.Height = 160;

            string componentId = Guid.NewGuid().ToString();

            var componentGraph = new CompositeComponentEditableGraph(true);

            if (componentGraph.References != null)
            {
                componentGraph.References = experiment.References.CopyCollection();
            }
            
            data.Metadata = new LoopScopeMetadata(componentGraph, scopeName, System.IO.Path.GetDirectoryName(experiment.ExperimentInfo.FilePath));

            string componentNodeId = Guid.NewGuid().ToString();
            string componentName = data.Metadata.Label;

            var loopScopeNode = new LoopScopeNode(Guid.NewGuid().ToString(), data);

            experiment.AddVertex(loopScopeNode);

            return loopScopeNode;
        }

        // HERZUM SPRINT 0.0
        /// <summary>
        /// Attempts to create the composite component. Component is created only if definition is a LoopMetadataDefinition.
        /// Otherwise method returns false, and null node.
        /// </summary>
        /// <param name="experiment">The experiment to which new component is added.</param>
        /// <param name="metadataDefinition">The metadata definition which component is created based on.</param>
        /// <param name="data">The data containing position of new vertex</param>
        /// <param name="newCreatedNode">Output of new created node; null if metadatadefinition was not a LoopMetadataDefinition</param>
        /// <returns>
        /// true if metadatadefinition was a LoopMetadataDefinition and node has been created, otherwise false
        /// </returns>
        private static bool TryCreateScopeComponent(IEditableExperiment experiment, MetadataDefinition metadataDefinition, SerializedVertexData data,
                                                   out ExperimentNode newCreatedNode)
        {
            bool isCreated = false;
            newCreatedNode = null;

            ScopeMetadataDefinition scopeMetadataDefinition = metadataDefinition as ScopeMetadataDefinition;
            if (scopeMetadataDefinition != null)
            {
                newCreatedNode = CreateScopeNode(scopeMetadataDefinition.Label, experiment, data.X, data.Y);
                isCreated = true;
            }

            return isCreated;
        }

        /// <summary>
        /// Generates the scope node.
        /// </summary>
        /// <param name="scopeName">Name of the scope.</param>
        /// <param name="experiment">The experiment to which new component is added.</param>
        /// <param name="positionX">The position X.</param>
        /// <param name="positionY">The position Y.</param>
        /// <returns></returns>
        private static ScopeNode CreateScopeNode(string scopeName, IEditableExperiment experiment, double positionX, double positionY)
        {
            var data = new SerializedVertexDataWithSize();
            data.X = positionX;
            data.Y = positionY;
            data.Width = 160;
            data.Height = 160;

            string componentId = Guid.NewGuid().ToString();

            var componentGraph = new CompositeComponentEditableGraph(true);

            if (componentGraph.References != null)
            {
                componentGraph.References = experiment.References.CopyCollection();
            }

            data.Metadata = new ScopeMetadata(componentGraph, scopeName, System.IO.Path.GetDirectoryName(experiment.ExperimentInfo.FilePath));

            string componentNodeId = Guid.NewGuid().ToString();
            string componentName = data.Metadata.Label;

            var scopeNode = new ScopeNode(Guid.NewGuid().ToString(), data);

            experiment.AddVertex(scopeNode);

            return scopeNode;
        }
        // END HERZUM 0.0

        // HERZUM SPRINT 2.0: TLAB-65 CLASS
        /// <summary>
        /// Attempts to create the composite component. Component is created only if definition is a ChallengeMetadataDefinition.
        /// Otherwise method returns false, and null node.
        /// </summary>
        /// <param name="experiment">The experiment to which new component is added.</param>
        /// <param name="metadataDefinition">The metadata definition which component is created based on.</param>
        /// <param name="data">The data containing position of new vertex</param>
        /// <param name="newCreatedNode">Output of new created node; null if metadatadefinition was not a LoopMetadataDefinition</param>
        /// <returns>
        /// true if metadatadefinition was a ChallengeMetadataDefinition and node has been created, otherwise false
        /// </returns>
        private static bool TryCreateChallengeComponent(IEditableExperiment experiment, MetadataDefinition metadataDefinition, SerializedVertexData data,
                                                    out ExperimentNode newCreatedNode)
        {

            bool isCreated = false;
            newCreatedNode = null;

            ChallengeMetadataDefinition challengeMetadataDefinition = metadataDefinition as ChallengeMetadataDefinition;
            if (challengeMetadataDefinition != null)
            {
                newCreatedNode = CreateChallengeNode(challengeMetadataDefinition.Label, experiment, data.X, data.Y);
                isCreated = true;
            }

            return isCreated;
        }

        /// <summary>
        /// Generates the challenge node.
        /// </summary>
        /// <param name="challengeName">Name of the challenge.</param>
        /// <param name="experiment">The experiment to which new component is added.</param>
        /// <param name="positionX">The position X.</param>
        /// <param name="positionY">The position Y.</param>
        /// <returns></returns>
        private static ChallengeNode CreateChallengeNode(string challengeName, IEditableExperiment experiment, double positionX, double positionY)
        {
            var data = new SerializedVertexDataWithSize();
            data.X = positionX;
            data.Y = positionY;
            data.Width = 160;
            data.Height = 160;

            string componentId = Guid.NewGuid().ToString();

            var componentGraph = new CompositeComponentEditableGraph(true);

            if (componentGraph.References != null)
            {
                componentGraph.References = experiment.References.CopyCollection();
            }

            data.Metadata = new ChallengeMetadata(componentGraph, challengeName, System.IO.Path.GetDirectoryName(experiment.ExperimentInfo.FilePath));

            string componentNodeId = Guid.NewGuid().ToString();
            string componentName = data.Metadata.Label;

            var challengeNode = new ChallengeNode(Guid.NewGuid().ToString(), data);

            experiment.AddVertex(challengeNode);

            return challengeNode;
        }
        // END HERZUM SPRINT 2.0: TLAB-65 CLASS

        #endregion
    }
}
