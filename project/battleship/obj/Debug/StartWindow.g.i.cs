﻿#pragma checksum "..\..\startWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "BD8F49868A1BAB81D2B73ACBE2F4A7532C212499"
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
using battleship;


namespace battleship {
    
    
    /// <summary>
    /// startWindow
    /// </summary>
    public partial class startWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\startWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MediaElement Video;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\startWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NewGame;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\startWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button LoadGame;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\startWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image LogoImage;
        
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
            System.Uri resourceLocater = new System.Uri("/battleship;component/startwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\startWindow.xaml"
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
            this.Video = ((System.Windows.Controls.MediaElement)(target));
            
            #line 12 "..\..\startWindow.xaml"
            this.Video.Loaded += new System.Windows.RoutedEventHandler(this.Video_Loaded);
            
            #line default
            #line hidden
            
            #line 12 "..\..\startWindow.xaml"
            this.Video.MediaEnded += new System.Windows.RoutedEventHandler(this.Video_MediaEnded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.NewGame = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\startWindow.xaml"
            this.NewGame.Click += new System.Windows.RoutedEventHandler(this.NewGame_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.LoadGame = ((System.Windows.Controls.Button)(target));
            
            #line 22 "..\..\startWindow.xaml"
            this.LoadGame.Click += new System.Windows.RoutedEventHandler(this.LoadGame_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.LogoImage = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

