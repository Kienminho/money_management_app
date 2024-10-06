using System.Text;

namespace Common.Utils.LayoutCSVLog4net;

public class CsvTextWriter : TextWriter
{
    private readonly TextWriter _textWriter;

    public CsvTextWriter(TextWriter textWriter)
    {
        _textWriter = textWriter;
    }

    public override Encoding Encoding => _textWriter.Encoding;

    public override void Write(char value)
    {
        _textWriter.Write(value);
        if (value == '"')
            _textWriter.Write(value);
    }

    public void WriteQuote()
    {
        _textWriter.Write('"');
    }
}