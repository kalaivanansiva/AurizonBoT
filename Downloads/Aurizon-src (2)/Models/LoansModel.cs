 using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microsoft.Bot.Sample.LuisBot
{
public class Stickyheader
    {

        [JsonProperty("columnClassNames")]
        public string columnClassNames { get; set; }

        [JsonProperty(":type")]
        public string _type { get; set; }
}

public class Stickyfooter
{

    [JsonProperty("columnClassNames")]
    public string columnClassNames { get; set; }

    [JsonProperty(":type")]
    public string _type { get; set; }
    }

    public class LoanName
{

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("dataType")]
    public string dataType { get; set; }

    [JsonProperty("value")]
    public string value { get; set; }

    [JsonProperty(":type")]
    public string _type { get; set; }
    }

    public class LoanSummary
{

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("dataType")]
    public string dataType { get; set; }

    [JsonProperty("paragraphs")]
    public IList<string> paragraphs { get; set; }

    [JsonProperty("value")]
    public string value { get; set; }

    [JsonProperty(":type")]
    public string _type { get; set; }
    }

    public class Benefits
{

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("dataType")]
    public string dataType { get; set; }

    [JsonProperty("paragraphs")]
    public IList<string> paragraphs { get; set; }

    [JsonProperty("value")]
    public string value { get; set; }

    [JsonProperty(":type")]
    public string _type { get; set; }
    }

    public class LoanImage
{

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("dataType")]
    public string dataType { get; set; }

    [JsonProperty("value")]
    public string value { get; set; }

    [JsonProperty(":type")]
    public string _type { get; set; }
    }

    public class KnowMoreBtn
{

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("dataType")]
    public string dataType { get; set; }

    [JsonProperty("value")]
    public string value { get; set; }

    [JsonProperty(":type")]
    public string _type { get; set; }
    }

    public class KnowMoreCTA
{

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("dataType")]
    public string dataType { get; set; }

    [JsonProperty(":type")]
    public string _type { get; set; }
    }

    public class ApplyNowBtn
{

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("dataType")]
    public string dataType { get; set; }

    [JsonProperty("value")]
    public string value { get; set; }

    [JsonProperty(":type")]
    public string _type { get; set; }
    }

    public class ApplyNowCTA
{

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("dataType")]
    public string dataType { get; set; }

    [JsonProperty(":type")]
    public string _type { get; set; }
    }

    public class Elements
{

    [JsonProperty("loanName")]
    public LoanName loanName { get; set; }

    [JsonProperty("loanSummary")]
    public LoanSummary loanSummary { get; set; }

    [JsonProperty("benefits")]
    public Benefits benefits { get; set; }

    [JsonProperty("loanImage")]
    public LoanImage loanImage { get; set; }

    [JsonProperty("knowMoreBtn")]
    public KnowMoreBtn knowMoreBtn { get; set; }

    [JsonProperty("knowMoreCTA")]
    public KnowMoreCTA knowMoreCTA { get; set; }

    [JsonProperty("applyNowBtn")]
    public ApplyNowBtn applyNowBtn { get; set; }

    [JsonProperty("applyNowCTA")]
    public ApplyNowCTA applyNowCTA { get; set; }
}

public class CqResponsive
{

    [JsonProperty(":type")]
    public string _type { get; set; }
    }

    public class Items
{

    [JsonProperty("cq:responsive")]
    public CqResponsive cq_responsive { get; set; }
    }

    public class CfLoan
{

    [JsonProperty("columnClassNames")]
    public string columnClassNames { get; set; }

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("description")]
    public string description { get; set; }

    [JsonProperty("model")]
    public string model { get; set; }

    [JsonProperty("elements")]
    public Elements elements { get; set; }

    [JsonProperty("elementsOrder")]
    public IList<string> elementsOrder { get; set; }

    [JsonProperty(":type")]
    public string _type { get; set; }

[JsonProperty(":items")]
public Items _items { get; set; }

        [JsonProperty(":itemsOrder")]
public IList<string> _itemsOrder { get; set; }
    }

    public class CfLoan1529340239
{

    [JsonProperty("columnClassNames")]
    public string columnClassNames { get; set; }

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("description")]
    public string description { get; set; }

    [JsonProperty("model")]
    public string model { get; set; }

    [JsonProperty("elements")]
        public Elements elements { get; set; }

[JsonProperty("elementsOrder")]
public IList<string> elementsOrder { get; set; }

[JsonProperty(":type")]
public string _type { get; set; }

        [JsonProperty(":items")]
        public Items _items { get; set; }

        [JsonProperty(":itemsOrder")]
public IList<string> _itemsOrder { get; set; }
    }

    public class CfLoan632079441
{

    [JsonProperty("columnClassNames")]
    public string columnClassNames { get; set; }

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("description")]
    public string description { get; set; }

    [JsonProperty("model")]
    public string model { get; set; }

    [JsonProperty("elements")]
        public Elements elements { get; set; }

[JsonProperty("elementsOrder")]
public IList<string> elementsOrder { get; set; }

[JsonProperty(":type")]
public string _type { get; set; }

        [JsonProperty(":items")]
        public Items _items { get; set; }

        [JsonProperty(":itemsOrder")]
public IList<string> _itemsOrder { get; set; }
    }

    public class Items2
{

    [JsonProperty("stickyheader")]
    public Stickyheader stickyheader { get; set; }

    [JsonProperty("stickyfooter")]
    public Stickyfooter stickyfooter { get; set; }

    [JsonProperty("cf_loan")]
    public CfLoan cf_loan { get; set; }

    [JsonProperty("cf_loan_1529340239")]
    public CfLoan1529340239 cf_loan_1529340239 { get; set; }

    [JsonProperty("cf_loan_632079441")]
    public CfLoan632079441 cf_loan_632079441 { get; set; }
}

public class Root
{

    [JsonProperty("columnCount")]
    public int columnCount { get; set; }

    [JsonProperty("gridClassNames")]
    public string gridClassNames { get; set; }

    [JsonProperty(":items")]
    public Items2 _items { get; set; }

[JsonProperty(":itemsOrder")]
public IList<string> _itemsOrder { get; set; }

        [JsonProperty(":type")]
public string _type { get; set; }
    }

    public class Items1
{

    [JsonProperty("root")]
    public Root root { get; set; }
}
     
    public class CustomisedModel  
    {
        public string title { get; set; }=string.Empty;
        public string Description { get; set; }=string.Empty;
        public string Image { get; set; }   =   string.Empty;
                                              
    }
     


    public class LoansModel
{

    [JsonProperty("title")]
    public string title { get; set; }

    [JsonProperty("templateType")]
    public string templateType { get; set; }

    [JsonProperty(":items")]
    public Items1 _items { get; set; }

[JsonProperty(":itemsOrder")]
public IList<string> _itemsOrder { get; set; }

        [JsonProperty(":type")]
public string _type { get; set; }
    }
}
