﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TileExplorer.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TileExplorer.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to {0:F6}.
        /// </summary>
        internal static string CoordFmt {
            get {
                return ResourceManager.GetString("CoordFmt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Загрузка....
        /// </summary>
        internal static string ProgramStatusLoadData {
            get {
                return ResourceManager.GetString("ProgramStatusLoadData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Чтение....
        /// </summary>
        internal static string ProgramStatusLoadGpx {
            get {
                return ResourceManager.GetString("ProgramStatusLoadGpx", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Удалить маркер «{0}»?.
        /// </summary>
        internal static string QuestionMarkerDelete {
            get {
                return ResourceManager.GetString("QuestionMarkerDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Удалить трек «{0}»?.
        /// </summary>
        internal static string QuestionTrackDelete {
            get {
                return ResourceManager.GetString("QuestionTrackDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Курсор: {0:F6}, {1:F6}.
        /// </summary>
        internal static string StatusMousePosition {
            get {
                return ResourceManager.GetString("StatusMousePosition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Центр: {0:F6}, {1:F6}.
        /// </summary>
        internal static string StatusPosition {
            get {
                return ResourceManager.GetString("StatusPosition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Плитка: {0}, {1}.
        /// </summary>
        internal static string StatusTileId {
            get {
                return ResourceManager.GetString("StatusTileId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Кластер: {0}.
        /// </summary>
        internal static string StatusTilesMaxCluster {
            get {
                return ResourceManager.GetString("StatusTilesMaxCluster", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Квадрат: {0}.
        /// </summary>
        internal static string StatusTilesMaxSquare {
            get {
                return ResourceManager.GetString("StatusTilesMaxSquare", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Открыто: {0}.
        /// </summary>
        internal static string StatusTilesVisited {
            get {
                return ResourceManager.GetString("StatusTilesVisited", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Масштаб: {0}.
        /// </summary>
        internal static string StatusZoom {
            get {
                return ResourceManager.GetString("StatusZoom", resourceCulture);
            }
        }
    }
}
