using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace EDITranslatorServiceLib
{
    [ServiceContract(Namespace = "http://rev-a-shelf.com")]
    public interface ITranslator
    {
        [OperationContract]
        string GetEDIDocFromJSON(string jsonSegment);

        [OperationContract]
        string GetEDIDocFromXML(string xmlSegment);

        [OperationContract]
        string GetJSONFromEDIDoc(string ediDoc);

        [OperationContract]
        string GetXMLFromEDIDoc(string ediDoc);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types
    // defined there, with the namespace "EDITranslatorServiceLib.ContractType".
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
