﻿#pragma checksum "..\..\wpfLoanDeclining.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1BC041DB37E2EAF98B96923A0E0A750F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.Controls;
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
    /// wpfLoanDeclining
    /// </summary>
    public partial class wpfLoanDeclining : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 1 "..\..\wpfLoanDeclining.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LoanManagement.Desktop.wpfLoanDeclining wdw1;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\wpfLoanDeclining.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cb1;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\wpfLoanDeclining.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cb2;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\wpfLoanDeclining.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cb3;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\wpfLoanDeclining.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbOthers;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\wpfLoanDeclining.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtOthers;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\wpfLoanDeclining.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbSend;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\wpfLoanDeclining.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDecline;
        
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
            System.Uri resourceLocater = new System.Uri("/LoanManagement.Desktop;component/wpfloandeclining.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\wpfLoanDeclining.xaml"
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
            this.wdw1 = ((LoanManagement.Desktop.wpfLoanDeclining)(target));
            
            #line 5 "..\..\wpfLoanDeclining.xaml"
            this.wdw1.Loaded += new System.Windows.RoutedEventHandler(this.wdw1_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cb1 = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 3:
            this.cb2 = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 4:
            this.cb3 = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 5:
            this.cbOthers = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.txtOthers = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.cbSend = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 8:
            this.btnDecline = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\wpfLoanDeclining.xaml"
            this.btnDecline.Click += new System.Windows.RoutedEventHandler(this.btnDecline_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

