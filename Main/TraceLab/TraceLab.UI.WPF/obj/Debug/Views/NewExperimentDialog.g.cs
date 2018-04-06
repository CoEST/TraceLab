﻿#pragma checksum "..\..\..\Views\NewExperimentDialog.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7186E1109E34258E65F63CDEB0525380BC24B71D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace TraceLab.UI.WPF.Views {
    
    
    /// <summary>
    /// NewExperimentDialog
    /// </summary>
    public partial class NewExperimentDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 62 "..\..\..\Views\NewExperimentDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ExperimentNameBox;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\Views\NewExperimentDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox DirectoryBox;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\Views\NewExperimentDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox FileNameBox;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\Views\NewExperimentDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox AuthorBox;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\Views\NewExperimentDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ContributorsBox;
        
        #line default
        #line hidden
        
        
        #line 86 "..\..\..\Views\NewExperimentDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox DescriptionBox;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\Views\NewExperimentDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CancelButton;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\Views\NewExperimentDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/TraceLab.UI.WPF;component/views/newexperimentdialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\NewExperimentDialog.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ExperimentNameBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 63 "..\..\..\Views\NewExperimentDialog.xaml"
            this.ExperimentNameBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.DirectoryBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 71 "..\..\..\Views\NewExperimentDialog.xaml"
            this.DirectoryBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 72 "..\..\..\Views\NewExperimentDialog.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.OnBrowseClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.FileNameBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 76 "..\..\..\Views\NewExperimentDialog.xaml"
            this.FileNameBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.FileNameBox_KeyDown);
            
            #line default
            #line hidden
            
            #line 77 "..\..\..\Views\NewExperimentDialog.xaml"
            this.FileNameBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.AuthorBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.ContributorsBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.DescriptionBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.CancelButton = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.CreateButton = ((System.Windows.Controls.Button)(target));
            
            #line 93 "..\..\..\Views\NewExperimentDialog.xaml"
            this.CreateButton.Click += new System.Windows.RoutedEventHandler(this.CreateButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

