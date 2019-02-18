using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SFW.Web.XSD
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1"),
    System.SerializableAttribute(),
    System.Diagnostics.DebuggerStepThroughAttribute(),
    System.ComponentModel.DesignerCategoryAttribute("code"),
    System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.susalud.gob.pe/AppAcreditacionJava/ResquestgetFoto.xsd"),
    System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.susalud.gob.pe/AppAcreditacionJava/ResquestgetFoto.xsd", IsNullable=false)]
    public class afi11request
    {
        private string txNombreField;
        private string txPeticionField;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string txNombre { get { return this.txNombreField; } set { this.txNombreField = value; } }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string txPeticion { get { return this.txPeticionField; } set { this.txPeticionField = value; } }
    }
}