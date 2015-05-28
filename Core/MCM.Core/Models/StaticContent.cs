﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MCM.Core.Models
{
    public class StaticContent
    {
        public string Id { get; set; }
        public string ContentTypeCode { get; set; }
        public string ContentName { get; set; }
        public string Content { get; set; }
        //Even though the database column is CreatedAt, the jsonproperty being passed back from EntityData is __createdAt
        public DateTimeOffset? __createdAt { get; set; }
        //Even though the database column is UpdatedAt, the jsonproperty being passed back from EntityData is __updatedAt
        public DateTimeOffset? __updatedAt { get; set; }
    }
}
