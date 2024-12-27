using Synaptic.Analysis;

namespace Synaptic.Services.Analysis;

public partial class ParsingService
{
    //  private class ParserFactory
    private class ParserFactory
    {
        /*
            The parser factory will be provided the results of the input parsing
            operation and will return a parser that can be used to process the
            input data. The factory will be responsible for selecting the correct
            parser based on the context descriptor provided.
        */
        internal ParserFactory() { }

    }


    public void Process(ContextDescriptor descriptor)
    {
        throw new NotImplementedException();
    }
}
