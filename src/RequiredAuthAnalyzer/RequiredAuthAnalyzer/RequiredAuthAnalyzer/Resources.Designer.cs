﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProgrammerAL.Analyzers.RequiredAuthAnalyzer {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ProgrammerAL.Analyzers.RequiredAuthAnalyzer.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Controller endpoint requires attribute to authenticate the user, or an attribute specifying no anonymous requests are allowed..
        /// </summary>
        internal static string ControllerRequiredAuthAnalyzerDescription {
            get {
                return ResourceManager.GetString("ControllerRequiredAuthAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Controller endpoint &apos;{0}&apos; requires an attribute specifying authentication, or allow anonymous.
        /// </summary>
        internal static string ControllerRequiredAuthAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("ControllerRequiredAuthAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Controller endpoint requires authentication attribute.
        /// </summary>
        internal static string ControllerRequiredAuthAnalyzerTitle {
            get {
                return ResourceManager.GetString("ControllerRequiredAuthAnalyzerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minimal API endpoint requires attribute to authenticate the user, or an attribute specifying no anonymous requests are allowed..
        /// </summary>
        internal static string MinimalApiRequiredAuthAnalyzerDescription {
            get {
                return ResourceManager.GetString("MinimalApiRequiredAuthAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minimal API endpoint &apos;{0}&apos; requires an attribute specifying authentication, or allow anonymous.
        /// </summary>
        internal static string MinimalApiRequiredAuthAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("MinimalApiRequiredAuthAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Minimal API endpoint requires authentication attribute.
        /// </summary>
        internal static string MinimalApiRequiredAuthAnalyzerTitle {
            get {
                return ResourceManager.GetString("MinimalApiRequiredAuthAnalyzerTitle", resourceCulture);
            }
        }
    }
}
