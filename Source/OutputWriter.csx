#nullable enable

public sealed class OutputWriter : IDisposable
{
    private readonly StringBuilder builder = new StringBuilder();
    private readonly StringWriter writer;
    private int indentLevel;
    
    public OutputWriter(string fileName)
    {
        this.writer = new StringWriter(this.builder);
    }
    
    public void Dispose()
    {
        this.writer.Dispose();
    }
    
    public void WriteLine(string message)
    {
        this.writer.WriteLine(message);
    }
    
    public void WriteHeader(int level, string message)
    {
        // https://docs.github.com/ko/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax#headings
        // 깃허브 헤더는 #으로 시작하고, #의 개수에 따라서 헤더 레벨이 결정된다.
        // 헤더 레벨은 1~6까지만 지원한다.
        if (level < 1 || level > 6)
        {
            throw new ArgumentOutOfRangeException(nameof(level), level, "level must be between 1 and 6");
        }
        
        string header = new string('#', level);
        this.writer.WriteLine($"{header} {message}");
        this.writer.WriteLine();
    }

    public void WriteLine()
    {
        this.writer.WriteLine();
    }

    public void WriteList(string message)
    {
        // https://docs.github.com/ko/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax#nested-lists
        // 깃허브 들여쓰기는 상위 항목에서 텍스트가 나오는 시점까지 해야 함. 일단은 2칸 들여쓰기로 통일.
        const int indentSize = 2;
        string indent = new string(' ', this.indentLevel * indentSize);

        this.writer.WriteLine($"{indent}* {message}");
    }

    public IDisposable Indent(bool isRootCategory) 
    {
        if (isRootCategory)
        {
            return new NullIndent();
        }

        return new ScopedIndent(this);
    } 

    public override string ToString()
    {
        return this.builder.ToString();
    }

    private sealed class ScopedIndent : IDisposable
    {
        private readonly OutputWriter writer;
        
        public ScopedIndent(OutputWriter writer)
        {
            this.writer = writer;
            this.writer.indentLevel++;
        }
        
        public void Dispose()
        {
            this.writer.indentLevel--;
        }
    }

    private sealed class NullIndent : IDisposable
    {
        public NullIndent()
        {
        }
        
        public void Dispose()
        {
        }
    }
}