using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using EdiEngine;
using EdiEngine.Common.Definitions;
using EdiEngine.Runtime;
using SegmentDefinitions = EdiEngine.Standards.X12_004010.Segments;
using EdiEngine.Standards.X12_004010.Maps;
using Newtonsoft.Json;


namespace EDITranslatorServiceLib
{
    public class EDITranslatorService : ITranslator
    {
        public string GetEDIDocFromJSON(string jsonSegment)
        {
            //string json3 = @"{'Type': 'M',
            //    'Name': 'M_940',
            //    'Content': [{
            //        'Type': 'S',
            //        'Name': 'W05',
            //        'Content': [ { 'E': 'N' }, { 'E': '538686' }, { 'E': '' }, { 'E': '001001' }, { 'E': '538686' }]
            //    }]
            //}";

            M_940 map = new M_940();
            JsonMapReader jReader = new JsonMapReader(map);
            EdiTrans ediTxn = jReader.ReadToEnd(jsonSegment);

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
            return string.Format("You entered: {0}", ediDoc);
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
