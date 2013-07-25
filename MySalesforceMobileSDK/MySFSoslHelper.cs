using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySalesforceMobileSDK
{
    public class MySFSoslHelper
    {
        public static String SOSLSearchText(string text, Dictionary<string, List<string>> returnObject = null, uint limit = 0)
        {
            String result = null;
            StringBuilder builder = new StringBuilder();

            builder.Append("FIND {" + text + "} IN ALL FIELDS");

            if (returnObject != null && returnObject.Keys.Count > 0) {
                builder.Append(" RETURNING ");

                List<string> fieldSpecs = new List<string>();
                foreach (KeyValuePair<string, List<string>> pair in returnObject) {

                    StringBuilder specBuilder = new StringBuilder();
                    specBuilder.Append(pair.Key + "(");
                    Boolean isfirst = true;
                    foreach (String fieldName in pair.Value) {

                        if (isfirst)
                        {
                            specBuilder.Append(fieldName);
                        }
                        else 
                        {
                            specBuilder.Append("," + fieldName);
                        }

                        isfirst = false;
                    }
                    specBuilder.Append(")");
                    fieldSpecs.Add(specBuilder.ToString());
                }

                Boolean isfirstSpec = true;
                foreach (String fieldSpec in fieldSpecs) {

                    if (isfirstSpec)
                    {
                        builder.Append(fieldSpec);
                    }
                    else 
                    {
                        builder.Append(", " + fieldSpec);
                    }

                    isfirstSpec = false;
                }
            }

            if (limit > 0) {
                builder.Append(" LIMIT " + limit.ToString());
            }

            result = builder.ToString();
            return result;
        }
    }
}
