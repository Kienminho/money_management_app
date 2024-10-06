using log4net.Util;

namespace Common.Utils.LayoutCSVLog4net;

public class EndRowConverter : PatternConverter
{
    public override void Convert(TextWriter writer, object state)
    {
        var ctw = writer as CsvTextWriter;

        ctw?.WriteQuote();

        writer.WriteLine();
    }
}