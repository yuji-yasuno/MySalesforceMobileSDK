using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySalesforceMobileSDK
{
    public class MySFSoqlHelper
    {
        public static String SOQLQueryWithFields(List<String> fields, string sobject, List<string> whereConditions = null, List<String> groupbyFields = null, string havingCondition = null, List<String> orderbyFields = null, uint limit = 0)
        {
            String result = null;
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT");

            Boolean isfirst = true;
            foreach (String fieldName in fields) {

                if (isfirst)
                {
                    builder.Append(" " + fieldName);
                }
                else 
                {
                    builder.Append("," + fieldName);
                }

                isfirst = false;
            }
            builder.Append(" FROM " + sobject);

            if (whereConditions != null && whereConditions.Count > 0) {
                builder.Append(" WHERE ");

                isfirst = true;
                foreach (string condition in whereConditions)
                {

                    if (isfirst)
                    {
                        builder.Append(condition);
                    }
                    else
                    {
                        builder.Append("AND " + condition);
                    }

                    isfirst = false;
                }
            }

            if (groupbyFields != null && groupbyFields.Count > 0) {
                builder.Append(" GROUP BY ");

                isfirst = true;
                foreach (String fieldName in groupbyFields)
                {

                    if (isfirst)
                    {
                        builder.Append(fieldName);
                    }
                    else
                    {
                        builder.Append("," + fieldName);
                    }

                    isfirst = false;
                }

                if (havingCondition != null)
                {
                    builder.Append(" HAVING " + havingCondition);
                }
            }

            if (orderbyFields != null && orderbyFields.Count > 0) {
                builder.Append(" ORDER BY ");

                isfirst = true;
                foreach (String fieldName in orderbyFields) {

                    if (isfirst)
                    {
                        builder.Append(fieldName);
                    }
                    else
                    {
                        builder.Append("," + fieldName);
                    }

                    isfirst = false;
                }
            }

            if (limit != 0) {
                builder.Append(" LIMIT " + limit.ToString());
            }

            result = builder.ToString();
            return result;
        }

    }
}
