﻿#pragma checksum "..\..\wpfMain.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "53E365F28336AD2A09FD7069258F5199"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
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
using System.Windows.Forms.Integration;
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


namespace LoanManagement.Desktop {
    
    
    /// <summary>
    /// wpfMain
    /// </summary>
    public partial class wpfMain : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\wpfMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button txtEmployee;
        
        #line default
        #line hidden
        
        
        #line 7 "..\..\wpfMain.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button txtServices;
        
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
            System.Uri resourceLocater = new System.Uri("/LoanManagement.Desktop;component/wpfmain.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\wpfMain.xaml"
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
            this.txtEmployee = ((System.Windows.Controls.Button)(target));
            
            #line 6 "..\..\wpfMain.xaml"
            this.txtEmployee.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.txtEmployee_MouseUp);
            
            #line default
            #line hidden
            
            #line 6 "..\..\wpfMain.xaml"
            this.txtEmployee.Click += new System.Windows.RoutedEventHandler(this.txtEmployee_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtServices = ((System.Windows.Controls.Button)(target));
            
            #line 7 "..\..\wpfMain.xaml"
            this.txtServices.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.txtServices_MouseUp);
            
            #line default
            #line hidden
            
            #line 7 "..\..\wpfMain.xaml"
            this.txtServices.Click += new System.Windows.RoutedEventHandler(this.txtServices_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

