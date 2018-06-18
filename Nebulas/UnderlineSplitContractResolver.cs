﻿using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nebulas
{
    public class UnderlineSplitContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return CamelCaseToUnderlineSplit(propertyName);
        }

        private string CamelCaseToUnderlineSplit(string name)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {

                var ch = name[i];

                //如果第一个是小写，说明是JsonProperty自定义过的，则不处理
                if (i == 0 && char.IsLower(ch))
                {
                    return name;
                }

                if (char.IsUpper(ch) && i > 0)
                {
                    var prev = name[i - 1];
                    if (prev != '_')
                    {
                        if (char.IsUpper(prev))
                        {
                            if (i < name.Length - 1)
                            {
                                var next = name[i + 1];
                                if (char.IsLower(next))
                                {
                                    builder.Append('_');
                                }
                            }
                        }
                        else
                        {
                            builder.Append('_');
                        }
                    }
                }

                builder.Append(char.ToLower(ch));
            }

            return builder.ToString();
        }
    }
}
