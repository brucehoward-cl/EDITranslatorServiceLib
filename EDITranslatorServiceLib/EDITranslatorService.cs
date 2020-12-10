using EdiEngine;
using EdiEngine.Runtime;
using EdiEngine.Standards.X12_004010.Maps;
using System;
using SegmentDefinitions = EdiEngine.Standards.X12_004010.Segments;
using Newtonsoft.Json;
using EdiEngine.Common.Definitions;

namespace EDITranslatorServiceLib
{
    public class EDITranslatorService : ITranslator
    {
            //string json3 = @"{'Type': 'M',
            //    'Name': 'M_940',
            //    'Content': [{
            //        'Type': 'S',
            //        'Name': 'W05',
            //        'Content': [ { 'E': 'N' }, { 'E': '538686' }, { 'E': '' }, { 'E': '001001' }, { 'E': '538686' }]
            //    }]
            //}";
            //{'Type': 'M','Name': 'M_940','Content': [{'Type': 'S','Name': 'W05','Content': [ { 'E': 'N' }, { 'E': '538686' }, { 'E': '' }, { 'E': '001001' }, { 'E': '538686' }]}]}
        public string GetEDIDocFromJSON(string jsonEDIdata)
        {
            dynamic json = JsonConvert.DeserializeObject(jsonEDIdata, typeof(object));

            //TODO: convert logic to determine map to separate method

            MapLoop map = Factory.GetMap((string)json.Name);    //TODO: add exception handling for the string cast operation
            JsonMapReader jReader = new JsonMapReader(map);
            EdiTrans ediTxn = jReader.ReadToEnd(jsonEDIdata);

            var eGroup = new EdiGroup("OW");
            eGroup.Transactions.Add(ediTxn);

            var eInterchange = new EdiInterchange();
            eInterchange.Groups.Add(eGroup);

            EdiBatch eBatch = new EdiBatch();
            eBatch.Interchanges.Add(eInterchange);

            //Add all service segments
            EdiDataWriterSettings settings = new EdiDataWriterSettings(
                new SegmentDefinitions.ISA(), new SegmentDefinitions.IEA(),
                new SegmentDefinitions.GS(), new SegmentDefinitions.GE(),
                new SegmentDefinitions.ST(), new SegmentDefinitions.SE(),
                "ZZ", "SENDER", "ZZ", "RECEIVER", "GSSENDER", "GSRECEIVER", "00401", "004010", "T", 100, 200, "\r\n", "*");

            EdiDataWriter ediWriter = new EdiDataWriter(settings);
            return ediWriter.WriteToString(eBatch);
        }

        public string GetEDIDocFromXML(string xmlSegment)
        {
            return string.Format("You entered: {0}", xmlSegment);
        }

        public string GetJSONFromEDIDoc(string ediDoc)
        {
            string edi =
                @"ISA*01*0000000000*01*0000000000*ZZ*ABCDEFGHIJKLMNO*ZZ*123456789012345*101127*1719*U*00400*000003438*0*P*>
                GS*OW*7705551212*3111350000*20000128*0557*3317*T*004010
                ST*940*0001
                W05*N*538686**001001*538686
                LX*1
                W01*12*CA*000100000010*VN*000100*UC*DEC0199******19991205
                G69*11.500 STRUD BLUBRY
                W76*56*500*LB*24*CF
                SE*7*0001
                GE*1*3317
                IEA*1*000003438";

            EdiDataReader ediReader = new EdiDataReader();
            EdiBatch ediBatch = ediReader.FromString(ediDoc);

            //Serialize the whole batch to JSON
            JsonDataWriter w1 = new JsonDataWriter();
            string json = w1.WriteToString(ediBatch);

            ////OR Serialize selected EDI message to Json
            //string jsonTrans = JsonConvert.SerializeObject(ediBatch.Interchanges[0].Groups[0].Transactions[0]);

            ////Serialize the whole batch to XML
            //XmlDataWriter w2 = new XmlDataWriter();
            //string xml = w2.WriteToString(ediBatch);
            return JsonConvert.SerializeObject(json);
        }

        public string GetXMLFromEDIDoc(string ediDoc)
        {
            return string.Format("You entered: {0}", ediDoc);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }


    }
}
