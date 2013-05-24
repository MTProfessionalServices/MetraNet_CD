﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MetraTech.ActivityServices.Common {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Exceptions {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Exceptions() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MetraTech.ActivityServices.Common.Exceptions", typeof(Exceptions).Assembly);
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
        ///   Looks up a localized string similar to Could not apply change..
        /// </summary>
        internal static string APPROVAL_COULD_NOT_APPLY_CHANGE {
            get {
                return ResourceManager.GetString("APPROVAL_COULD_NOT_APPLY_CHANGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to These transactions are not in a valid state for this action..
        /// </summary>
        internal static string BAD_TRANSACTION_STATE {
            get {
                return ResourceManager.GetString("BAD_TRANSACTION_STATE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This type cannot be voided.
        /// </summary>
        internal static string BAD_TRANSACTION_TYPE {
            get {
                return ResourceManager.GetString("BAD_TRANSACTION_TYPE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not log transaction to database.
        /// </summary>
        internal static string COULD_NOT_LOG_TRANSACTION {
            get {
                return ResourceManager.GetString("COULD_NOT_LOG_TRANSACTION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have exceeded the maximum transactions allowed in MetraPay.
        /// </summary>
        internal static string EXCEEDED_MAX_TRANSACTIONS {
            get {
                return ResourceManager.GetString("EXCEEDED_MAX_TRANSACTIONS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User subs exist after po effective end date.
        /// </summary>
        internal static string MTPCUSER_SUBS_EXIST_AFTER_PO_EFF_END_DATE {
            get {
                return ResourceManager.GetString("MTPCUSER_SUBS_EXIST_AFTER_PO_EFF_END_DATE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User subs exist before po effective start date..
        /// </summary>
        internal static string MTPCUSER_SUBS_EXIST_BEFORE_PO_EFF_START_DATE {
            get {
                return ResourceManager.GetString("MTPCUSER_SUBS_EXIST_BEFORE_PO_EFF_START_DATE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This account does not have the capability to do a manual override.
        /// </summary>
        internal static string NO_OVERRIDE_CAPABILITY {
            get {
                return ResourceManager.GetString("NO_OVERRIDE_CAPABILITY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This transaction has already failed in a previous attempt..
        /// </summary>
        internal static string TRANSACTION_ALREADY_FAILED {
            get {
                return ResourceManager.GetString("TRANSACTION_ALREADY_FAILED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This transaction was rejected by the processor in a previous attempt..
        /// </summary>
        internal static string TRANSACTION_ALREADY_REJECTED {
            get {
                return ResourceManager.GetString("TRANSACTION_ALREADY_REJECTED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The payment provider already settled this transaction.
        /// </summary>
        internal static string TRANSACTION_ALREADY_SETTLED {
            get {
                return ResourceManager.GetString("TRANSACTION_ALREADY_SETTLED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Transaction not in a failed state.
        /// </summary>
        internal static string TRANSACTION_NOT_FAILED {
            get {
                return ResourceManager.GetString("TRANSACTION_NOT_FAILED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This transaction timed out.
        /// </summary>
        internal static string TRANSACTION_TIMED_OUT {
            get {
                return ResourceManager.GetString("TRANSACTION_TIMED_OUT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Void failed.
        /// </summary>
        internal static string VOID_FAILED {
            get {
                return ResourceManager.GetString("VOID_FAILED", resourceCulture);
            }
        }
    }
}
