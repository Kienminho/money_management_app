using log4net.Core;
using log4net.Layout;

namespace Common.Utils.LayoutCSVLog4net;

public class CsvPatternLayout : PatternLayout
{
    public override void ActivateOptions()
    {
        AddConverter("newfield", typeof(NewFieldConverter));
        AddConverter("endrow", typeof(EndRowConverter));
        base.ActivateOptions();
    }

    public override void Format(TextWriter writer, LoggingEvent loggingEvent)
    {
        var ctw = new CsvTextWriter(writer);
        ctw.WriteQuote();
        base.Format(ctw, loggingEvent);
    }
}