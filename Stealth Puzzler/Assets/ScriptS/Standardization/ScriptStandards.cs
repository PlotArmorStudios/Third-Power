using System;
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
        //1. Events
        public event Action OnEventCall;
        
        //2. Serialized Fields
        [SerializeField] private int _serializedInt;
        
        //3. Serialized Properties
        [field: SerializeField] private int SerializedProperty { get; } //can be set in inspector
        
        //4. Object fields
        private ScriptStandards _scriptStandards;
        
        //5. Class member fields
        public int PublicIntField;
        private int _privateIntField;
        
        //6. Properties
        public int Property { get; set; }
        private int PrivateProperty { get; set; }
        
        //7. Methods
        public void FirstMethod()
        {
            //Local variables
            int localVariable = 0;
        }

        private void SecondMethod()
        {
            
        }
    }    
}
