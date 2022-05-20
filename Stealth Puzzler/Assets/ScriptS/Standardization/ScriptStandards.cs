using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//General Naming Conventions:
//Limit abbreviation. Make the names of fields and methods explicit.
//Do not concern yourself with names being too long if seemingly unavoidable.
//Ex: private field for ship acceleration rate --> _shipAccelerationRate.
//Ref: https://www.youtube.com/watch?v=ECb31GwoSsM&ab_channel=JasonWeimann

//Use Header attribute for groups of related fields.
//Use ToolTip attribute for fields that may not be very explicit.
//Use '///<summary>' and other XML tags to describe complex methods.
//XML Tags ref: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags

namespace ScriptStandardization //Namespace to separate this class from the rest of the project code
{
    public class ScriptStandards
    {
        //Serialized Fields
        [SerializeField] private int _serializedInt;
        
        //Serialized Properties
        [field: SerializeField] private int SerializedProperty { get; } //can be set in inspector
        
        //Class member fields
        public int PublicIntField;
        private int _privateIntField;
        
        //Properties
        public int Property { get; set; }
        private int PrivateProperty { get; set; }
        
        //Methods
        public void FirstMethod()
        {
            
        }

        private void SecondMethod()
        {
            
        }
    }    
}
